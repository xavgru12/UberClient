using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>Weapon-local miniature solar system. Unity 4.6.5; no bundles or mesh readback.</summary>
public sealed class UberverseWeaponEffect : MonoBehaviour
{
    // Next unused ID after PR #8's retired 9068-9078 range. Catalog must use this same ID.
    public const int ItemId = 2067;
    // Death Hammer [Galaxy] (2076) reuses this same orbital aura; it has no "AWP" mesh, so it anchors
    // to the weapon's largest body mesh and Initialize scales the system to those bounds.
    public const int ItemIdDeathHammer = 2076;
    public const string RootName = "Uberverse_OrbitalSystem";
    private const int PlanetCount = 6;  // array MAX (AWP shows 3, Death Hammer 6: five orbs + Saturn)
    private const int RibbonSegments = 64;
    private const int SpriteCount = 96; // AWP sprite capacity (36 active); Death Hammer sizes its own (GalaxySprites)
    // Gemini/Nano-Banana baked planet orbs, embedded as WeaponSkins.<name> in the csproj.
    // Additive billboards replace the old procedural spheres: image quality, no shimmer.
    private static readonly string[] PlanetTextures = { "planet_violet.png", "planet_magenta.png", "planet_blue.png" };
    private const float PlanetBillboardScale = 1.85f; // quad half-extent as a multiple of PlanetRadius
    private readonly List<UnityEngine.Object> owned = new List<UnityEngine.Object>();
    private readonly List<Renderer> renderers = new List<Renderer>();
    private readonly Mesh[] planetMeshes = new Mesh[PlanetCount];
    private readonly Vector3[] planetQuad = new Vector3[4]; // scratch for one billboard rebuild
    private readonly Vector3[] planetPositions = new Vector3[PlanetCount];
    private Vector3[] spritePositions; // sized spriteCapacity in Initialize
    private float[] spriteSizes;
    private Color[] spriteColours;
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
    private int activeSprites = 36;  // 36 AWP (tight); GalaxySprites on the Death Hammer
    private int spriteCapacity = SpriteCount;
    private int activePlanets = 3;   // 3 AWP; 6 Death Hammer
    private int goldThreads = 0;     // 0 AWP; GalaxyStreaks filigree streaks on the Death Hammer
    private int ribbonStrips;        // sized in Initialize
    private float zSpread = 0f;       // 0 = AWP orbital cluster; >0 = Death Hammer [Galaxy] (AnimateGalaxy)
    private Vector3 bodyCenter, bodyHalf; // gun bounds centre/half-extents (Death Hammer surface profile)

    // ---- Death Hammer [Galaxy] (zSpread > 0) ----
    private const int GalaxySprites = 128; // 6 planet atmospheres + GalaxyHaze aura + the rest 4-point sparkles
    private const int GalaxyHaze = 14;
    private const int GalaxyStreaks = 7;   // crest, 2 stock faces, 2 barrel flanks, muzzle wrap, forend-front wrap
    private const int GalaxySaturn = 2;    // the ringed orb, largest, mid-gun above the forend
    // Cross-section of the Death_Hammer mesh (2453 verts, skin-studio export), ray-sampled every 5% of
    // its length, breech -> muzzle, as fractions of its bounds: top/bottom of the y range, half-width of
    // x. Stock 0-.27 (lens section), receiver .30-.53 (box, top rail .37-.53), side-by-side DOUBLE
    // barrel from .41 to the muzzle (tubes at x = +-.45 half-width, y = .758, r = .0295), pump side +
    // top plates .62-.80, magazine tube below to .83, thin tube to .92. Streaks, aura and sparkles sit
    // on this surface instead of the bounding box.
    private static readonly float[] GalaxyTop = {
        .541f, .559f, .559f, .559f, .498f, .555f, .732f, .965f, .965f, .965f, .965f,
        .930f, .861f, .917f, .917f, .917f, .917f, .883f, .883f, .883f, .883f };
    private static readonly float[] GalaxyBottom = {
        .027f, .066f, .122f, .178f, .213f, .153f, .403f, .330f, .343f, .338f, .338f,
        .416f, .416f, .356f, .364f, .356f, .356f, .416f, .416f, .628f, .628f };
    private static readonly float[] GalaxyHalfW = {
        .279f, .337f, .352f, .352f, .323f, .323f, .455f, .513f, .777f, .777f, .777f,
        .821f, 1f, 1f, 1f, 1f, 1f, .880f, .880f, .880f, .880f };
    private const float GalaxyBarrelAxis = .758f; // y of both barrel axes (the muzzle flash sits there), fraction of the y range
    private const float GalaxyBarrelX = .45f;     // x of each barrel axis, fraction of the half-width
    private const float GalaxyWrapCentre = .658f; // centre of barrels + thin tube, for the forend-front wrap
    private static readonly float[] GalaxyPlanetT = { .12f, .18f, .55f, .78f, .93f, .42f };
    private static readonly float[] GalaxyPlanetLift = { .045f, -.050f, .080f, -.045f, .045f, .065f }; // +above crest / -below belly
    private static readonly float[] GalaxyPlanetRadius = { .010f, .0085f, .017f, .0090f, .0080f, .0075f };
    private static readonly int[] GalaxyPlanetTex = { 0, 1, 0, 2, 1, 0 }; // purple worlds; one blue-violet
    private static readonly Color[] GalaxyHazePalette = {
        new Color(.45f, .18f, 1f), new Color(.78f, .20f, .85f), new Color(.32f, .36f, 1f)
    };

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
        bool mine = (itemId == ItemId || itemId == ItemIdDeathHammer);
        UberverseWeaponEffect[] existing = weaponRoot.GetComponentsInChildren<UberverseWeaponEffect>(true);
        bool retained = false;
        foreach (UberverseWeaponEffect effect in existing)
        {
            if (mine && effect.ready && effect.source != null && !retained)
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
        if (!mine || retained) return;

        // 2067 AWP anchors to the measured static body mesh named "AWP" (its scope included; never Handle,
        // an animation mesh, or a muzzle renderer). 2076 Death Hammer has no "AWP" mesh, so pick the
        // largest body mesh (skipping obvious non-body renderers); Initialize scales the system to it.
        string wantName = (itemId == ItemId) ? "AWP" : null;
        MeshFilter chosen = null;
        float bestVolume = -1f;
        foreach (MeshFilter filter in weaponRoot.GetComponentsInChildren<MeshFilter>(true))
        {
            if (filter.sharedMesh == null) continue;
            Renderer r = filter.GetComponent<Renderer>();
            if (r == null || Owns(r)) continue;
            if (wantName != null)
            {
                if (filter.sharedMesh.name == wantName) { chosen = filter; break; }
                continue;
            }
            string name = filter.sharedMesh.name.ToLowerInvariant();
            if (name.Contains("handle") || name.Contains("muzzle") || name.Contains("scope")
                || name.Contains("flash") || name.Contains("light")) continue;
            Vector3 size = filter.sharedMesh.bounds.size;
            float volume = size.x * size.y * size.z;
            if (volume > bestVolume) { bestVolume = volume; chosen = filter; }
        }
        if (chosen == null)
        {
            Debug.LogWarning("Uberverse effects: body mesh not found for item " + itemId + "; no effects attached.");
            return;
        }
        Renderer body = chosen.GetComponent<Renderer>();
        GameObject root = new GameObject(RootName);
        root.transform.parent = body.transform;
        root.transform.localPosition = Vector3.zero;
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;
        root.layer = body.gameObject.layer;
        UberverseWeaponEffect newEffect = root.AddComponent<UberverseWeaponEffect>();
        if (itemId == ItemIdDeathHammer)
        {
            newEffect.zSpread = 1f; newEffect.activePlanets = PlanetCount;
            newEffect.spriteCapacity = newEffect.activeSprites = GalaxySprites;
            newEffect.goldThreads = GalaxyStreaks;
        }
        try { newEffect.Initialize(body, chosen.sharedMesh.bounds); }
        catch (Exception error)
        {
            root.SetActive(false);
            Destroy(root);
            Debug.LogError("Uberverse effects: " + error.Message);
        }
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
        bodyCenter = bounds.center; bodyHalf = bounds.extents;
        // AWP: full ring + tail per planet, one Saturn ring. Galaxy: tail per planet, two Saturn bands,
        // glow + core strip per filigree streak.
        ribbonStrips = zSpread > 0f ? activePlanets + 2 + goldThreads * 2 : activePlanets * 2 + 1 + goldThreads;
        spritePositions = new Vector3[spriteCapacity];
        spriteSizes = new float[spriteCapacity];
        spriteColours = new Color[spriteCapacity];
        Shader additive = FindSupported("Particles/Additive", "Particles/Alpha Blended");
        if (additive == null)
            throw new InvalidOperationException("No additive/alpha-blended particle shader for the Uberverse planets.");

        // Each planet is a single camera-facing quad textured with its baked Gemini orb. Black
        // reads as empty under additive; a per-camera OnWillRenderObject keeps the quad facing.
        for (int i = 0; i < activePlanets; i++)
        {
            Material material = Keep(new Material(additive));
            material.name = "Uberverse_Planet_" + i;
            int tex = zSpread > 0f ? GalaxyPlanetTex[i] : i % PlanetTextures.Length;
            material.mainTexture = Keep(LoadPlanetTexture(PlanetTextures[tex]));
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
        glow.mainTexture = Keep(zSpread > 0f ? BuildGalaxyAtlas() : BuildFalloff(false)); // star | halo atlas on the Death Hammer
        if (glow.HasProperty("_TintColor")) glow.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
        Material gold = Keep(new Material(additive));
        gold.name = "Uberverse_OrbitGold";
        gold.mainTexture = Keep(BuildFalloff(true));
        if (gold.HasProperty("_TintColor")) gold.SetColor("_TintColor", new Color(.5f, .5f, .5f, .7f));
        ribbons = Keep(NewQuadMesh(ribbonStrips * RibbonSegments, out ribbonVertices, out ribbonColours));
        ribbonStarts = new Vector3[ribbonStrips * RibbonSegments];
        ribbonEnds = new Vector3[ribbonStarts.Length];
        ribbonWidths = new float[ribbonStarts.Length];
        Renderer paths = NewRenderer("Golden_Orbits", ribbons, gold);
        paths.gameObject.AddComponent<UberverseRibbonCamera>().Owner = this;
        sprites = Keep(NewQuadMesh(spriteCapacity, out spriteVertices, out spriteVertexColours));
        if (zSpread > 0f)
        {
            // Atlas halves: planet atmospheres + aura haze take the soft halo (right), sparkles the star (left).
            Vector2[] uv = sprites.uv;
            int halos = activePlanets + GalaxyHaze;
            for (int i = 0; i < spriteCapacity; i++)
            {
                float u0 = i < halos ? .5f : 0f, u1 = u0 + .5f;
                int v = i * 4;
                uv[v] = new Vector2(u0, 0f); uv[v + 1] = new Vector2(u1, 0f);
                uv[v + 2] = new Vector2(u1, 1f); uv[v + 3] = new Vector2(u0, 1f);
            }
            sprites.uv = uv;
        }
        // OnWillRenderObject must be on the object which owns the billboard renderer.
        gameObject.AddComponent<MeshFilter>().sharedMesh = sprites;
        MeshRenderer spriteRenderer = gameObject.AddComponent<MeshRenderer>();
        Configure(spriteRenderer, glow);
        ready = true;
        Animate(0f);
        UpdateBillboards(Vector3.right, Vector3.up);
        for (int i = 0; i < activePlanets; i++) BuildPlanetQuad(i, Vector3.right, Vector3.up);
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

    private float PlanetRadius(int i)
    {
        if (zSpread > 0f) return GalaxyPlanetRadius[i] * scale;
        return (i == 0 ? .018f : i == 1 ? .013f : i == 2 ? .011f : i == 3 ? .0125f : .0095f) * scale;
    }

    private Vector3 Orbit(int i, float angle)
    {
        float radiusA = (.044f + i * .016f) * scale;
        Vector3 p = new Vector3(Mathf.Cos(angle) * radiusA,
            Mathf.Sin(angle) * (.012f + i * .0025f) * scale,
            Mathf.Sin(angle) * (.066f + i * .011f) * scale);
        return anchor + p + new Vector3(0f, i * .015f * scale, (i - 1) * .015f * scale);
    }

    // ---- Death Hammer [Galaxy] surface helpers: t = 0 breech .. 1 muzzle ----
    private static float Sample(float[] table, float t)
    {
        float f = Mathf.Clamp01(t) * (table.Length - 1);
        int i = (int)f;
        if (i >= table.Length - 1) return table[table.Length - 1];
        return Mathf.Lerp(table[i], table[i + 1], f - i);
    }

    private float GalaxyYAt(float fraction) { return bodyCenter.y - bodyHalf.y + fraction * 2f * bodyHalf.y; }
    private float GalaxyY(float[] table, float t) { return GalaxyYAt(Sample(table, t)); }
    private float GalaxyHalfX(float t) { return Sample(GalaxyHalfW, t) * bodyHalf.x; }
    private float GalaxyZ(float t) { return bodyCenter.z - bodyHalf.z + t * 2f * bodyHalf.z; }

    private static float Hash(int n, float k)
    {
        float v = (n + 1) * k;
        return v - Mathf.Floor(v);
    }

    // Each orb sways on a small ellipse about its own spot: above the crest or below the belly.
    private Vector3 GalaxyOrbit(int i, float angle)
    {
        float t = GalaxyPlanetT[i], lift = GalaxyPlanetLift[i];
        float cy = (lift > 0f ? GalaxyY(GalaxyTop, t) : GalaxyY(GalaxyBottom, t)) + lift * scale;
        return new Vector3(bodyCenter.x + Mathf.Cos(angle) * .035f * scale,
            cy + Mathf.Sin(angle) * .010f * scale, GalaxyZ(t) + Mathf.Sin(angle) * .025f * scale);
    }

    // Filigree streak k at s (0..1), lying on the measured surface. 0: crest, receiver -> muzzle (rail-
    // width weave over the receiver, barrel-to-barrel over the double barrel). 1/2: lightning across
    // each stock face. 3/4: each flank, on the receiver box then along the outer barrel equator / pump
    // plates. 5: 1.5 turns around both barrels near the muzzle. 6: one turn around barrels + thin tube
    // just in front of the forend. Wraps are ellipses enclosing both tubes, not a single-barrel circle.
    private Vector3 Streak(int k, float s)
    {
        float t, x, y, w;
        switch (k)
        {
            case 0:
                t = .27f + s * .73f;
                w = Mathf.Lerp(.15f, GalaxyBarrelX, Mathf.Clamp01((t - .50f) / .08f)) * bodyHalf.x;
                x = Mathf.Sin(s * 9.42f) * w + Mathf.Sin(s * 23.6f + 1f) * .004f * scale;
                y = GalaxyY(GalaxyTop, t) + .004f * scale;
                break;
            case 1: case 2:
                t = s * .30f;
                x = (k == 1 ? 1f : -1f) * (GalaxyHalfX(t) + .004f * scale);
                w = .5f + .30f * Mathf.Sin(s * 7.85f + k * 2.1f) + .08f * Mathf.Sin(s * 19.6f + k);
                y = Mathf.Lerp(GalaxyY(GalaxyBottom, t), GalaxyY(GalaxyTop, t), w);
                break;
            case 3: case 4:
                t = .33f + s * .67f;
                x = (k == 3 ? 1f : -1f) * (GalaxyHalfX(t) + .004f * scale);
                w = Mathf.Sin(s * 12.57f + k * 1.9f) + .3f * Mathf.Sin(s * 31.4f + k);
                y = Mathf.Lerp(
                    Mathf.Lerp(GalaxyY(GalaxyBottom, t), GalaxyY(GalaxyTop, t), .45f + .18f * w),
                    GalaxyYAt(GalaxyBarrelAxis) + w * .012f * scale,
                    Mathf.Clamp01((t - .40f) / .02f));
                break;
            case 5:
                t = .90f + s * .05f;
                w = s * 9.42f;
                x = Mathf.Cos(w) * .088f * scale;
                y = GalaxyYAt(GalaxyBarrelAxis) + Mathf.Sin(w) * .048f * scale;
                break;
            default:
                t = .845f + s * .03f;
                w = s * 6.2832f;
                x = Mathf.Cos(w) * .093f * scale;
                y = GalaxyYAt(GalaxyWrapCentre) + Mathf.Sin(w) * .083f * scale;
                break;
        }
        return new Vector3(bodyCenter.x + x, y, GalaxyZ(t));
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
        if (zSpread > 0f) { AnimateGalaxy(time); return; }
        int quad = 0;
        for (int i = 0; i < activePlanets; i++)
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
                float tw = (.0005f + t * .0011f) * scale;
                RibbonQuad(quad++, Orbit(i, a), Orbit(i, b), tw,
                    new Color(1f, .72f + t * .18f, .32f + t * .38f, t * t * .85f));
            }
            // Faint outer atmosphere just BEYOND the orb's own baked glow (half-extent 2.7 sits
            // outside the planet quad's 1.85), so it rings the orb rather than washing its core.
            spritePositions[i] = planetPositions[i];
            spriteSizes[i] = PlanetRadius(i) * 2.7f;
            spriteColours[i] = WithAlpha(Color.Lerp(Palette[i % Palette.Length], Cyan, .28f), .16f);
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

        int starStart = activePlanets + 9;
        for (int i = activePlanets; i < activeSprites; i++)
        {
            int n = i - activePlanets;
            float phase = Phase(time, i < starStart ? .05 : .10, n * 2.399963);
            float wave = .5f + .5f * Mathf.Sin(Phase(time, .5, n * 1.7));
            if (i < starStart)
            {
                // Translucent purple nebula haze.
                spritePositions[i] = anchor + new Vector3(Mathf.Cos(phase) * .040f,
                    -.075f + Mathf.Sin(phase) * .018f, -.13f + n * .043f) * scale;
                spriteSizes[i] = (.062f + .014f * wave) * scale;
                spriteColours[i] = WithAlpha(Palette[n % 3], .055f + wave * .022f);
            }
            else
            {
                // Star sparkles: scattered along the barrel.
                spritePositions[i] = anchor + new Vector3(Mathf.Cos(phase) * (.050f + n % 4 * .011f),
                    -.035f + Mathf.Sin(Phase(time, .14, n * 2.399963 * 1.4)) * .060f,
                    -.13f + (n % 13) * .024f) * scale;
                spriteSizes[i] = (.0019f + .0011f * wave + (n % 9 == 0 ? .0014f : 0f)) * scale;
                spriteColours[i] = WithAlpha(n % 3 == 0 ? Gold : Cyan, .2f + wave * .45f);
            }
        }
    }

    // Death Hammer [Galaxy]: gold filigree on the gun's measured surface, dense 4-point sparkles, a soft
    // purple aura, six purple orbs with gold wisp tails, and a two-band gold Saturn. Concept-matched.
    private void AnimateGalaxy(double time)
    {
        int quad = 0;
        for (int i = 0; i < activePlanets; i++)
        {
            float phase = Phase(time, .10 + i * .05, i * 2.094395);
            planetPositions[i] = GalaxyOrbit(i, phase);
            // Gold wisp trailing the orb: bold at the head, fading to a fine tail.
            for (int j = 0; j < RibbonSegments; j++)
            {
                float t = j / (float)RibbonSegments;
                float a = phase - (1f - t) * 1.4f;
                float b = phase - (1f - (j + 1f) / RibbonSegments) * 1.4f;
                RibbonQuad(quad++, GalaxyOrbit(i, a), GalaxyOrbit(i, b), (.0008f + t * .0026f) * scale,
                    new Color(1f, .78f + t * .14f, .38f + t * .30f, t * t * .85f));
            }
            spritePositions[i] = planetPositions[i];
            spriteSizes[i] = PlanetRadius(i) * 2.7f;
            spriteColours[i] = WithAlpha(Color.Lerp(Palette[GalaxyPlanetTex[i]], Cyan, .20f), .14f);
        }
        // Saturn: bright inner band, paler outer band, a gap between.
        Quaternion tilt = Quaternion.Euler(24f, 0f, -22f);
        Vector3 saturn = planetPositions[GalaxySaturn];
        for (int band = 0; band < 2; band++)
        {
            float radius = (band == 0 ? .034f : .046f) * scale;
            float width = (band == 0 ? .0032f : .0024f) * scale;
            Color colour = band == 0 ? new Color(1f, .78f, .40f, .85f) : new Color(1f, .70f, .34f, .45f);
            for (int j = 0; j < RibbonSegments; j++)
            {
                float a = j * Mathf.PI * 2f / RibbonSegments;
                float b = (j + 1) * Mathf.PI * 2f / RibbonSegments;
                Vector3 p = tilt * new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * radius;
                Vector3 q = tilt * new Vector3(Mathf.Cos(b), 0f, Mathf.Sin(b)) * radius;
                RibbonQuad(quad++, saturn + p, saturn + q, width, colour);
            }
        }
        // Filigree: a wide soft glow under a bright core, with pulses of light running along each streak.
        for (int k = 0; k < goldThreads; k++)
        {
            float run = Phase(time, 3.0, k * 1.3);
            for (int layer = 0; layer < 2; layer++)
            for (int j = 0; j < RibbonSegments; j++)
            {
                float s0 = j / (float)RibbonSegments, s1 = (j + 1) / (float)RibbonSegments;
                float pulse = Mathf.Max(0f, Mathf.Sin(s0 * 12.566f - run));
                pulse *= pulse;
                Color colour = layer == 0
                    ? new Color(1f, .68f, .28f, .18f + .14f * pulse)
                    : new Color(1f, .86f, .52f, .60f + .40f * pulse);
                RibbonQuad(quad++, Streak(k, s0), Streak(k, s1), (layer == 0 ? .011f : .0035f) * scale, colour);
            }
        }
        if (ribbons != null)
        {
            ribbons.vertices = ribbonVertices;
            ribbons.colors = ribbonColours;
        }

        int hazeEnd = activePlanets + GalaxyHaze;
        for (int i = activePlanets; i < activeSprites; i++)
        {
            int n = i - activePlanets;
            if (i < hazeEnd)
            {
                // Nebula aura: soft halos centred in the body, spaced along it; the gun occludes the inner
                // half so only the outer glow shows. Drifts slowly.
                float wave = .5f + .5f * Mathf.Sin(Phase(time, .30, n * 1.3));
                float t = (n + .5f) / GalaxyHaze + .02f * Mathf.Sin(Phase(time, .25, n * 2.1));
                float top = GalaxyY(GalaxyTop, t), bottom = GalaxyY(GalaxyBottom, t);
                spritePositions[i] = new Vector3(bodyCenter.x,
                    (top + bottom) * .5f + .010f * scale * Mathf.Sin(Phase(time, .3, n)), GalaxyZ(t));
                spriteSizes[i] = (top - bottom) * .55f + (.045f + .012f * wave) * scale;
                spriteColours[i] = WithAlpha(GalaxyHazePalette[n % 3], .10f + .05f * wave);
            }
            else
            {
                // Sparkles: 4-point stars on a shell just outside the surface, all along the gun, in three
                // sizes; gold / warm white / violet; each glints with its own rhythm and creeps around the gun.
                int m = i - hazeEnd;
                float t = Hash(m, .7548777f);
                float theta = Phase(time, .05 + .03 * (m % 3), Hash(m, .5698403f) * 6.2832f);
                float lift = (.008f + .020f * Hash(m, .3183099f)) * scale;
                float top = GalaxyY(GalaxyTop, t), bottom = GalaxyY(GalaxyBottom, t);
                float wave = .5f + .5f * Mathf.Sin(Phase(time, .8 + .6 * Hash(m, .1707f), m * 1.7));
                float glint = wave * wave;
                spritePositions[i] = new Vector3(bodyCenter.x + (GalaxyHalfX(t) + lift) * Mathf.Cos(theta),
                    (top + bottom) * .5f + ((top - bottom) * .5f + lift) * Mathf.Sin(theta), GalaxyZ(t));
                float size = m % 9 == 0 ? .020f : m % 3 == 0 ? .010f : .006f;
                spriteSizes[i] = size * (.75f + .35f * glint) * scale;
                Color sc = m % 5 == 0 ? new Color(.72f, .48f, 1f) : (m % 2 == 0 ? Gold : new Color(1f, .94f, .82f));
                spriteColours[i] = WithAlpha(sc, .25f + .75f * glint);
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
        for (int i = 0; i < spriteCapacity; i++)
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
        // All motion is bounded; do not read geometry or recalculate bounds each frame. The spread
        // Death Hammer system runs the whole gun, so its bounds must be larger or the far end culls.
        mesh.bounds = new Bounds(anchor + Vector3.forward * .12f * scale,
            Vector3.one * (zSpread > 0f ? 2.7f : 1.5f) * scale);
        return mesh;
    }

    // Death Hammer sprite atlas, 128x64. Left: 4-point twinkle (tight core, soft bloom, thin tapered rays).
    // Right: the soft halo. One material draws both; each quad picks a half by UV. Both halves are
    // transparent at the seam, so mips cannot bleed anything visible across.
    private static Texture2D BuildGalaxyAtlas()
    {
        const int size = 64;
        Texture2D texture = new Texture2D(size * 2, size, TextureFormat.RGBA32, true);
        texture.filterMode = FilterMode.Trilinear;
        texture.name = "Uberverse_GalaxyAtlas";
        texture.wrapMode = TextureWrapMode.Clamp;
        Color[] pixels = new Color[size * 2 * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size * 2; x++)
        {
            float u = (x % size) / (size - 1f) * 2f - 1f, v = y / (size - 1f) * 2f - 1f;
            float r = Mathf.Sqrt(u * u + v * v), alpha;
            if (x < size)
            {
                float core = Mathf.Pow(Mathf.Max(0f, 1f - r), 3f) + .30f * Mathf.Pow(Mathf.Max(0f, 1f - r * 1.4f), 2f);
                float rayH = Mathf.Pow(Mathf.Max(0f, 1f - Mathf.Abs(v) * 11f), 2f) * Mathf.Pow(Mathf.Max(0f, 1f - Mathf.Abs(u)), 1.4f);
                float rayV = Mathf.Pow(Mathf.Max(0f, 1f - Mathf.Abs(u) * 11f), 2f) * Mathf.Pow(Mathf.Max(0f, 1f - Mathf.Abs(v)), 1.4f);
                alpha = Mathf.Clamp01(core + (rayH + rayV) * .85f);
            }
            else alpha = Mathf.Pow(Mathf.Max(0f, 1f - r * r), 3f);
            pixels[y * size * 2 + x] = new Color(1f, 1f, 1f, alpha);
        }
        texture.SetPixels(pixels); texture.Apply(true, true);
        return texture;
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
