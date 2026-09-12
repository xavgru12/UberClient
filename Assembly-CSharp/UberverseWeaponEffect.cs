using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>Weapon-local miniature solar system. Unity 4.6.5; no bundles or mesh readback.</summary>
public sealed class UberverseWeaponEffect : MonoBehaviour
{
    // Next unused ID after PR #8's retired 9068-9078 range. Catalog must use this same ID.
    public const int ItemId = 9079;
    public const string RootName = "Uberverse_OrbitalSystem";
    private const int PlanetCount = 3;
    private const int RibbonSegments = 64;
    private const int RibbonCount = 7; // three orbits, three comet tails, one planet ring
    private const int SpriteCount = 36; // 3 atmospheres, 9 nebula wisps, 24 sparse star motes
    // Gemini/Nano-Banana baked planet orbs, embedded as WeaponSkins.<name> in the csproj.
    // Additive billboards replace the old procedural spheres: image quality, no shimmer.
    private static readonly string[] PlanetTextures = { "planet_violet.png", "planet_magenta.png", "planet_blue.png" };
    private const float PlanetBillboardScale = 1.85f; // quad half-extent as a multiple of PlanetRadius
    private readonly List<UnityEngine.Object> owned = new List<UnityEngine.Object>();
    private readonly List<Renderer> renderers = new List<Renderer>();
    private readonly Mesh[] planetMeshes = new Mesh[PlanetCount];
    private readonly Vector3[] planetQuad = new Vector3[4]; // scratch for one billboard rebuild
    private readonly Vector3[] planetPositions = new Vector3[PlanetCount];
    private readonly Vector3[] spritePositions = new Vector3[SpriteCount];
    private readonly float[] spriteSizes = new float[SpriteCount];
    private readonly Color[] spriteColours = new Color[SpriteCount];
    private static readonly Color Gold = new Color(1f, .65f, .20f, 1f);
    private static readonly Color Cyan = new Color(.20f, .85f, 1f, 1f);
    private static readonly Color[] Palette = {
        new Color(.43f, .16f, 1f), new Color(1f, .12f, .55f), new Color(.08f, .64f, 1f)
    };
    private Renderer source;
    private Mesh ribbons, sprites;
    private Vector3[] ribbonVertices, spriteVertices;
    private Vector3[] ribbonStarts, ribbonEnds;
    private float[] ribbonWidths;
    private Color[] ribbonColours, spriteVertexColours;
    private double clock; // Wrap each phase independently; never jump the whole system's clock.
    private float scale;
    private Vector3 anchor;
    private bool ready;

    public static bool Owns(Renderer renderer)
    {
        if (renderer == null) return false;
        for (Transform t = renderer.transform; t != null; t = t.parent)
            if (t.GetComponent<UberverseWeaponEffect>() != null) return true;
        return false;
    }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        if (weaponRoot == null) return;
        UberverseWeaponEffect[] existing = weaponRoot.GetComponentsInChildren<UberverseWeaponEffect>(true);
        bool retained = false;
        foreach (UberverseWeaponEffect effect in existing)
        {
            if (itemId == ItemId && effect.ready && effect.source != null && !retained)
            {
                retained = true;
                continue;
            }
            // Destroy is deferred; hide immediately and mark invalid so a second attach
            // in the same frame cannot resurrect the scheduled-for-destruction component.
            effect.ready = false;
            effect.gameObject.SetActive(false);
            Destroy(effect.gameObject);
        }
        if (itemId != ItemId || retained) return;

        // AWP is the measured static body mesh, including its scope. Never use Handle,
        // an animation mesh, or a muzzle renderer as the attachment frame.
        foreach (MeshFilter filter in weaponRoot.GetComponentsInChildren<MeshFilter>(true))
        {
            if (filter.sharedMesh == null || filter.sharedMesh.name != "AWP") continue;
            Renderer body = filter.GetComponent<Renderer>();
            if (body == null || Owns(body)) continue;
            GameObject root = new GameObject(RootName);
            root.transform.parent = body.transform;
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = body.gameObject.layer;
            UberverseWeaponEffect effect = root.AddComponent<UberverseWeaponEffect>();
            try { effect.Initialize(body, filter.sharedMesh.bounds); }
            catch (Exception error)
            {
                root.SetActive(false);
                Destroy(root);
                Debug.LogError("Uberverse effects: " + error.Message);
            }
            return;
        }
        Debug.LogWarning("Uberverse effects: AWP body mesh not found; no effects attached.");
    }

    private T Keep<T>(T resource) where T : UnityEngine.Object
    {
        owned.Add(resource);
        return resource;
    }

    private void Initialize(Renderer body, Bounds bounds)
    {
        source = body;
        // Exported AWP: centre (0,.042137,.413189), size (.097197,.269362,1.477879).
        // Coordinates below are in that mesh's frame and scale with its longitudinal extent.
        scale = bounds.size.z / 1.477879f;
        anchor = new Vector3(bounds.center.x, bounds.max.y + .045f * scale,
            bounds.min.z + bounds.size.z * .39f);
        Shader additive = FindSupported("Particles/Additive", "Particles/Alpha Blended");
        if (additive == null)
            throw new InvalidOperationException("No additive/alpha-blended particle shader for the Uberverse planets.");

        // Each planet is a single camera-facing quad textured with its baked Gemini orb. Black
        // reads as empty under additive; a per-camera OnWillRenderObject keeps the quad facing.
        for (int i = 0; i < PlanetCount; i++)
        {
            Material material = Keep(new Material(additive));
            material.name = "Uberverse_Planet_" + i;
            material.mainTexture = Keep(LoadPlanetTexture(PlanetTextures[i]));
            if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
            Vector3[] verts; Color[] cols;
            planetMeshes[i] = Keep(NewQuadMesh(1, out verts, out cols));
            for (int j = 0; j < 4; j++) cols[j] = Color.white; // additive zeroes on a black vertex colour
            planetMeshes[i].colors = cols;
            NewRenderer("Planet_" + i, planetMeshes[i], material)
                .gameObject.AddComponent<UberversePlanetCamera>().Set(this, i);
        }
        Material glow = Keep(new Material(additive));
        glow.name = "Uberverse_Nebula";
        glow.mainTexture = Keep(BuildFalloff(false));
        if (glow.HasProperty("_TintColor")) glow.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
        Material gold = Keep(new Material(additive));
        gold.name = "Uberverse_OrbitGold";
        gold.mainTexture = Keep(BuildFalloff(true));
        if (gold.HasProperty("_TintColor")) gold.SetColor("_TintColor", new Color(.5f, .5f, .5f, .7f));
        ribbons = Keep(NewQuadMesh(RibbonCount * RibbonSegments, out ribbonVertices, out ribbonColours));
        ribbonStarts = new Vector3[RibbonCount * RibbonSegments];
        ribbonEnds = new Vector3[ribbonStarts.Length];
        ribbonWidths = new float[ribbonStarts.Length];
        Renderer paths = NewRenderer("Golden_Orbits", ribbons, gold);
        paths.gameObject.AddComponent<UberverseRibbonCamera>().Owner = this;
        sprites = Keep(NewQuadMesh(SpriteCount, out spriteVertices, out spriteVertexColours));
        // OnWillRenderObject must be on the object which owns the billboard renderer.
        gameObject.AddComponent<MeshFilter>().sharedMesh = sprites;
        MeshRenderer spriteRenderer = gameObject.AddComponent<MeshRenderer>();
        Configure(spriteRenderer, glow);
        ready = true;
        Animate(0f);
        UpdateBillboards(Vector3.right, Vector3.up);
        for (int i = 0; i < PlanetCount; i++) BuildPlanetQuad(i, Vector3.right, Vector3.up);
    }

    private static Shader FindSupported(string preferred, string fallback)
    {
        Shader shader = Shader.Find(preferred);
        if (shader != null && shader.isSupported) return shader;
        shader = Shader.Find(fallback);
        return shader != null && shader.isSupported ? shader : null;
    }

    private Renderer NewRenderer(string label, Mesh mesh, Material material)
    {
        GameObject child = new GameObject(label);
        child.layer = gameObject.layer;
        child.transform.parent = transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;
        child.AddComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = child.AddComponent<MeshRenderer>();
        Configure(renderer, material);
        return renderer;
    }

    private void Configure(Renderer renderer, Material material)
    {
        renderer.sharedMaterial = material;
        renderer.castShadows = false;
        renderer.receiveShadows = false;
        renderer.enabled = source.enabled;
        renderers.Add(renderer);
    }

    private float PlanetRadius(int i) { return (i == 0 ? .018f : i == 1 ? .013f : .0095f) * scale; }

    private Vector3 Orbit(int i, float angle)
    {
        float radius = (.044f + i * .016f) * scale;
        Vector3 p = new Vector3(Mathf.Cos(angle) * radius,
            Mathf.Sin(angle) * (.012f + i * .0025f) * scale,
            Mathf.Sin(angle) * (.066f + i * .011f) * scale);
        return anchor + p + new Vector3(0f, i * .015f * scale, (i - 1) * .015f * scale);
    }

    private void LateUpdate()
    {
        if (!ready) return;
        if (source == null) { gameObject.SetActive(false); Destroy(gameObject); return; }
        // Inherit the weapon camera layer, including changes after attachment. The game's
        // scope hides that camera; no global zoom flag may hide another player's planets.
        bool visible = source.enabled && source.gameObject.activeInHierarchy;
        gameObject.layer = source.gameObject.layer;
        foreach (Renderer renderer in renderers)
        {
            renderer.gameObject.layer = gameObject.layer;
            renderer.enabled = visible;
        }
        if (!visible) return;
        clock += Time.deltaTime;
        Animate(clock);
    }

    private static float Phase(double time, double speed, double offset)
    {
        return (float)((time * speed + offset) % (Math.PI * 2.0));
    }

    private void Animate(double time)
    {
        int quad = 0;
        for (int i = 0; i < PlanetCount; i++)
        {
            float phase = Phase(time, .10 + i * .05, i * 2.094395);
            planetPositions[i] = Orbit(i, phase); // quad is built per-camera in BuildPlanetQuad
            for (int j = 0; j < RibbonSegments; j++)
            {
                float a = j * Mathf.PI * 2f / RibbonSegments;
                float b = (j + 1) * Mathf.PI * 2f / RibbonSegments;
                RibbonQuad(quad++, Orbit(i, a), Orbit(i, b), .00065f * scale,
                    new Color(Gold.r, Gold.g, Gold.b, .32f));
            }
            for (int j = 0; j < RibbonSegments; j++)
            {
                float t = j / (float)RibbonSegments;
                float a = phase - (1f - t) * .95f;
                float b = phase - (1f - (j + 1f) / RibbonSegments) * .95f;
                RibbonQuad(quad++, Orbit(i, a), Orbit(i, b), (.0005f + t * .0011f) * scale,
                    new Color(1f, .72f + t * .18f, .32f + t * .38f, t * t * .85f));
            }
            // Faint outer atmosphere just BEYOND the orb's own baked glow (half-extent 2.7 sits
            // outside the planet quad's 1.85), so it rings the orb rather than washing its core.
            spritePositions[i] = planetPositions[i];
            spriteSizes[i] = PlanetRadius(i) * 2.7f;
            spriteColours[i] = WithAlpha(Color.Lerp(Palette[i], Cyan, .28f), .16f);
        }
        // Saturn-like ring on the largest world: follows its orbit, has its own fixed tilt.
        Quaternion tilt = Quaternion.Euler(24f, 0f, -22f);
        for (int j = 0; j < RibbonSegments; j++)
        {
            float a = j * Mathf.PI * 2f / RibbonSegments;
            float b = (j + 1) * Mathf.PI * 2f / RibbonSegments;
            Vector3 p = tilt * new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * .030f * scale;
            Vector3 q = tilt * new Vector3(Mathf.Cos(b), 0f, Mathf.Sin(b)) * .030f * scale;
            RibbonQuad(quad++, planetPositions[0] + p, planetPositions[0] + q,
                .0018f * scale, new Color(1f, .73f, .38f, .65f));
        }
        if (ribbons != null)
        {
            ribbons.vertices = ribbonVertices;
            ribbons.colors = ribbonColours;
        }

        for (int i = 3; i < SpriteCount; i++)
        {
            int n = i - 3;
            float phase = Phase(time, i < 12 ? .05 : .10, n * 2.399963);
            float wave = .5f + .5f * Mathf.Sin(Phase(time, .5, n * 1.7));
            if (i < 12)
            {
                // Concentrate translucent wisps around scope/receiver, below the planets.
                spritePositions[i] = anchor + new Vector3(Mathf.Cos(phase) * .040f,
                    -.075f + Mathf.Sin(phase) * .018f, -.13f + n * .043f) * scale;
                spriteSizes[i] = (.062f + .012f * wave) * scale;
                spriteColours[i] = WithAlpha(Palette[n % 3], .055f + wave * .020f);
            }
            else
            {
                spritePositions[i] = anchor + new Vector3(Mathf.Cos(phase) * (.050f + n % 4 * .011f),
                    -.035f + Mathf.Sin(Phase(time, .14, n * 2.399963 * 1.4)) * .060f,
                    -.13f + (n % 13) * .024f) * scale;
                spriteSizes[i] = (.0019f + .0011f * wave + (n % 9 == 0 ? .0014f : 0f)) * scale;
                spriteColours[i] = WithAlpha(n % 3 == 0 ? Gold : Cyan, .2f + wave * .45f);
            }
        }
    }

    private static Color WithAlpha(Color colour, float alpha) { colour.a = alpha; return colour; }

    private void RibbonQuad(int quad, Vector3 a, Vector3 b, float width, Color colour)
    {
        if (ribbons == null) return;
        ribbonStarts[quad] = a; ribbonEnds[quad] = b; ribbonWidths[quad] = width;
        Vector3 side = Vector3.Cross((b - a).normalized, Vector3.up).normalized * width;
        int v = quad * 4;
        ribbonVertices[v] = a - side; ribbonVertices[v + 1] = a + side;
        ribbonVertices[v + 2] = b + side; ribbonVertices[v + 3] = b - side;
        for (int j = 0; j < 4; j++) ribbonColours[v + j] = colour;
    }

    internal void FaceRibbons(Camera camera)
    {
        if (!ready || ribbons == null || camera == null) return;
        Vector3 eye = transform.InverseTransformPoint(camera.transform.position);
        Vector3 facing = transform.InverseTransformVector(-camera.transform.forward).normalized;
        Vector3 cameraUp = transform.InverseTransformVector(camera.transform.up).normalized;
        for (int i = 0; i < ribbonStarts.Length; i++)
        {
            Vector3 a = ribbonStarts[i], b = ribbonEnds[i], tangent = (b - a).normalized;
            Vector3 view = camera.orthographic ? facing : (eye - (a + b) * .5f).normalized;
            Vector3 side = Vector3.Cross(tangent, view);
            if (side.sqrMagnitude < .000001f) side = Vector3.Cross(tangent, cameraUp);
            side = side.normalized * ribbonWidths[i];
            int v = i * 4;
            ribbonVertices[v] = a - side; ribbonVertices[v + 1] = a + side;
            ribbonVertices[v + 2] = b + side; ribbonVertices[v + 3] = b - side;
        }
        // Called by this renderer immediately before each camera draws it. Gold paths stay
        // thin and legible from the side; a second camera never reuses the first camera's facing.
        ribbons.vertices = ribbonVertices;
    }

    // Called from each planet renderer's own OnWillRenderObject, immediately before that camera
    // draws it, so every planet quad faces the current view (never a stale one from another camera).
    internal void FacePlanet(int index, Camera camera)
    {
        if (!ready || camera == null) return;
        BuildPlanetQuad(index,
            transform.InverseTransformVector(camera.transform.right).normalized,
            transform.InverseTransformVector(camera.transform.up).normalized);
    }

    private void BuildPlanetQuad(int index, Vector3 right, Vector3 up)
    {
        if (!ready || index < 0 || index >= PlanetCount) return;
        Mesh mesh = planetMeshes[index];
        if (mesh == null) return;
        float h = PlanetRadius(index) * PlanetBillboardScale;
        Vector3 x = right * h, y = up * h, p = planetPositions[index];
        planetQuad[0] = p - x - y; planetQuad[1] = p + x - y;
        planetQuad[2] = p + x + y; planetQuad[3] = p - x + y;
        mesh.vertices = planetQuad;
    }

    private void OnWillRenderObject()
    {
        if (!ready || Camera.current == null) return;
        // World vectors transformed back into mesh coordinates also handle scaled weapon parents.
        UpdateBillboards(transform.InverseTransformVector(Camera.current.transform.right).normalized,
            transform.InverseTransformVector(Camera.current.transform.up).normalized);
    }

    private void UpdateBillboards(Vector3 right, Vector3 up)
    {
        if (sprites == null) return;
        for (int i = 0; i < SpriteCount; i++)
        {
            Vector3 x = right * spriteSizes[i], y = up * spriteSizes[i], p = spritePositions[i];
            int v = i * 4;
            spriteVertices[v] = p - x - y; spriteVertices[v + 1] = p + x - y;
            spriteVertices[v + 2] = p + x + y; spriteVertices[v + 3] = p - x + y;
            for (int j = 0; j < 4; j++) spriteVertexColours[v + j] = spriteColours[i];
        }
        sprites.vertices = spriteVertices;
        sprites.colors = spriteVertexColours;
    }

    private Mesh NewQuadMesh(int quads, out Vector3[] vertices, out Color[] colours)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Uberverse_Dynamic";
        mesh.MarkDynamic();
        vertices = new Vector3[quads * 4]; colours = new Color[quads * 4];
        Vector2[] uv = new Vector2[quads * 4];
        int[] indices = new int[quads * 6];
        for (int i = 0; i < quads; i++)
        {
            int v = i * 4, t = i * 6;
            uv[v] = new Vector2(0, 0); uv[v + 1] = new Vector2(1, 0);
            uv[v + 2] = new Vector2(1, 1); uv[v + 3] = new Vector2(0, 1);
            indices[t] = v; indices[t + 1] = v + 1; indices[t + 2] = v + 2;
            indices[t + 3] = v; indices[t + 4] = v + 2; indices[t + 5] = v + 3;
        }
        mesh.vertices = vertices; mesh.uv = uv; mesh.colors = colours; mesh.triangles = indices;
        // All motion is bounded; do not read geometry or recalculate bounds each frame.
        mesh.bounds = new Bounds(anchor + Vector3.forward * .12f * scale, Vector3.one * 1.5f * scale);
        return mesh;
    }

    private static Texture2D BuildFalloff(bool ribbon)
    {
        const int size = 64;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true);
        texture.filterMode = FilterMode.Trilinear;
        texture.name = ribbon ? "Uberverse_SoftThread" : "Uberverse_SoftHalo";
        texture.wrapMode = TextureWrapMode.Clamp;
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float u = x / (size - 1f) * 2f - 1f, v = y / (size - 1f) * 2f - 1f;
            float distance = ribbon ? u * u : u * u + v * v;
            float alpha = Mathf.Pow(Mathf.Max(0f, 1f - distance), ribbon ? 2f : 3f);
            pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
        }
        texture.SetPixels(pixels); texture.Apply(true, true);
        return texture;
    }

    // Load a baked Gemini orb (WeaponSkins.<name>, embedded) and bake luminance into its alpha.
    // The orbs are RGB on black; folding brightness into alpha makes the black drop out under the
    // alpha-blended fallback shader too, and tapers the glow edge. Mipmaps + trilinear de-shimmer.
    private Texture2D LoadPlanetTexture(string fileName)
    {
        byte[] data = ReadPlanetBytes(fileName);
        if (data == null)
            throw new InvalidOperationException("Uberverse planet image missing: " + fileName);
        Texture2D raw = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!raw.LoadImage(data))
        {
            Destroy(raw);
            throw new InvalidOperationException("Uberverse planet decode failed: " + fileName);
        }
        Color[] pixels = raw.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];
            float lum = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
            c.a = Mathf.Clamp01(lum * 1.25f); // lift so the bright core stays solid
            pixels[i] = c;
        }
        Texture2D planet = new Texture2D(raw.width, raw.height, TextureFormat.RGBA32, true);
        planet.name = "Uberverse_GeminiPlanet_" + fileName;
        planet.wrapMode = TextureWrapMode.Clamp;
        planet.SetPixels(pixels);
        planet.Apply(true);
        planet.filterMode = FilterMode.Trilinear;
        Destroy(raw);
        return planet;
    }

    private static byte[] ReadPlanetBytes(string fileName)
    {
        string resource = "WeaponSkins." + fileName;
        try
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream s = asm.GetManifestResourceStream(resource))
            {
                if (s == null) return null;
                // Read in a loop: Stream.Read may return short, and CopyTo is absent on .NET 3.5.
                byte[] buffer = new byte[s.Length];
                int read = 0;
                while (read < buffer.Length)
                {
                    int n = s.Read(buffer, read, buffer.Length - read);
                    if (n <= 0) break;
                    read += n;
                }
                return read == buffer.Length ? buffer : null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Uberverse: could not read embedded planet " + resource + ": " + e.Message);
            return null;
        }
    }

    private void OnDisable()
    {
        foreach (Renderer renderer in renderers)
            if (renderer != null) renderer.enabled = false;
    }

    private void OnDestroy()
    {
        ready = false;
        foreach (UnityEngine.Object resource in owned)
            if (resource != null) Destroy(resource);
        owned.Clear();
    }
}

// Separate callback on the ribbon renderer avoids transparent-sort order or multi-camera
// timing making the paths face a stale view. No mesh/material allocation occurs in this callback.
public sealed class UberverseRibbonCamera : MonoBehaviour
{
    public UberverseWeaponEffect Owner;
    private void OnWillRenderObject()
    {
        if (Owner != null) Owner.FaceRibbons(Camera.current);
    }
}

// One per planet renderer: billboards that planet's quad to the drawing camera, immediately
// before it draws. Same reason as the ribbon callback - a child renderer's own OnWillRenderObject
// is the only place that sees the correct per-camera facing without transparent-sort surprises.
public sealed class UberversePlanetCamera : MonoBehaviour
{
    private UberverseWeaponEffect owner;
    private int index;
    public void Set(UberverseWeaponEffect effect, int planetIndex) { owner = effect; index = planetIndex; }
    private void OnWillRenderObject()
    {
        if (owner != null) owner.FacePlanet(index, Camera.current);
    }
}
