using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shared framework for the simpler additive-billboard weapon auras (Cyber Neon, Toxic Venom,
/// Molten Inferno). One camera-facing sprite pool per weapon; a subclass supplies the palette,
/// the sprite art and the per-frame motion in Emit(). The AWP [Uberverse] effect is deliberately
/// its own file - it carries bespoke planets/ribbons and does not fit this pool-of-motes shape.
///
/// Attachment, layer inheritance, billboarding and cleanup are lifted verbatim from the proven
/// UberverseWeaponEffect so all four skins behave identically around pooling, scopes and cameras.
/// </summary>
public abstract class WeaponEmitterEffect : MonoBehaviour
{
    protected abstract int SpriteCount { get; }

    private readonly List<UnityEngine.Object> owned = new List<UnityEngine.Object>();
    private readonly List<Renderer> renderers = new List<Renderer>();
    protected Vector3[] positions;
    protected float[] sizes;
    protected Color[] tints;
    private Mesh spriteMesh;
    private Vector3[] spriteVertices;
    private Color[] spriteColours;
    private Renderer source;
    protected float scale;
    protected Vector3 anchor;
    protected Bounds bodyBounds;
    protected double clock;
    private bool ready;

    public static bool Owns(Renderer renderer)
    {
        if (renderer == null) return false;
        for (Transform t = renderer.transform; t != null; t = t.parent)
            if (t.GetComponent<WeaponEmitterEffect>() != null) return true;
        return false;
    }

    // Attach a concrete effect to the measured static AWP body mesh. Always enumerates and removes
    // existing components of this exact type (so switching to another skin tears this one down),
    // retaining one live instance when the item id still matches. Mirrors UberverseWeaponEffect.Apply.
    protected static void Attach<T>(GameObject weaponRoot, int itemId, int expectedId, string rootName,
        string meshName, float refLength)
        where T : WeaponEmitterEffect
    {
        if (weaponRoot == null) return;
        T[] existing = weaponRoot.GetComponentsInChildren<T>(true);
        bool retained = false;
        foreach (T effect in existing)
        {
            if (itemId == expectedId && effect.ready && effect.source != null && !retained)
            {
                retained = true;
                continue;
            }
            effect.ready = false;
            effect.gameObject.SetActive(false);
            Destroy(effect.gameObject);
        }
        if (itemId != expectedId || retained) return;

        foreach (MeshFilter filter in weaponRoot.GetComponentsInChildren<MeshFilter>(true))
        {
            if (filter.sharedMesh == null || filter.sharedMesh.name != meshName) continue;
            Renderer body = filter.GetComponent<Renderer>();
            if (body == null || Owns(body)) continue;
            GameObject root = new GameObject(rootName);
            root.transform.parent = body.transform;
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = body.gameObject.layer;
            T effect = root.AddComponent<T>();
            try { effect.Initialize(body, filter.sharedMesh.bounds, refLength); }
            catch (Exception error)
            {
                root.SetActive(false);
                Destroy(root);
                Debug.LogError(rootName + ": " + error.Message);
            }
            return;
        }
    }

    protected T Keep<T>(T resource) where T : UnityEngine.Object { owned.Add(resource); return resource; }

    private void Initialize(Renderer body, Bounds bounds, float refLength)
    {
        source = body;
        bodyBounds = bounds;
        // refLength = the weapon's own exported longitudinal (z) extent; every distance below scales
        // with the real body mesh so the same FX code fits any weapon, not just the AWP.
        scale = bounds.size.z / refLength;
        anchor = new Vector3(bounds.center.x, bounds.max.y + .02f * scale, bounds.center.z);
        Shader additive = Shader.Find("Particles/Additive");
        if (additive == null || !additive.isSupported) additive = Shader.Find("Particles/Alpha Blended");
        if (additive == null || !additive.isSupported)
            throw new InvalidOperationException("No additive particle shader available.");

        Material material = Keep(new Material(additive));
        material.name = GetType().Name + "_Mat";
        material.mainTexture = Keep(BuildSpriteTexture());
        ConfigureMaterial(material);

        positions = new Vector3[SpriteCount];
        sizes = new float[SpriteCount];
        tints = new Color[SpriteCount];
        spriteMesh = Keep(NewQuadMesh(SpriteCount, out spriteVertices, out spriteColours));
        // OnWillRenderObject fires on the object that owns the billboard renderer.
        gameObject.AddComponent<MeshFilter>().sharedMesh = spriteMesh;
        MeshRenderer r = gameObject.AddComponent<MeshRenderer>();
        Configure(r, material);

        ready = true;
        Emit(0.0);
        UpdateBillboards(Vector3.right, Vector3.up);
    }

    // Subclass hooks.
    protected abstract Texture2D BuildSpriteTexture();
    protected abstract void ConfigureMaterial(Material material);
    protected abstract void Emit(double time); // fill positions[], sizes[], tints[]

    private void Configure(Renderer renderer, Material material)
    {
        renderer.sharedMaterial = material;
        renderer.castShadows = false;
        renderer.receiveShadows = false;
        renderer.enabled = source.enabled;
        renderers.Add(renderer);
    }

    private void LateUpdate()
    {
        if (!ready) return;
        if (source == null) { gameObject.SetActive(false); Destroy(gameObject); return; }
        bool visible = source.enabled && source.gameObject.activeInHierarchy;
        gameObject.layer = source.gameObject.layer;
        foreach (Renderer r in renderers)
        {
            r.gameObject.layer = gameObject.layer;
            r.enabled = visible;
        }
        if (!visible) return;
        clock += Time.deltaTime;
        Emit(clock);
    }

    private void OnWillRenderObject()
    {
        if (!ready || Camera.current == null) return;
        UpdateBillboards(transform.InverseTransformVector(Camera.current.transform.right).normalized,
            transform.InverseTransformVector(Camera.current.transform.up).normalized);
    }

    private void UpdateBillboards(Vector3 right, Vector3 up)
    {
        if (spriteMesh == null) return;
        for (int i = 0; i < SpriteCount; i++)
        {
            Vector3 x = right * sizes[i], y = up * sizes[i], p = positions[i];
            int v = i * 4;
            spriteVertices[v] = p - x - y; spriteVertices[v + 1] = p + x - y;
            spriteVertices[v + 2] = p + x + y; spriteVertices[v + 3] = p - x + y;
            for (int j = 0; j < 4; j++) spriteColours[v + j] = tints[i];
        }
        spriteMesh.vertices = spriteVertices;
        spriteMesh.colors = spriteColours;
    }

    protected Mesh NewQuadMesh(int quads, out Vector3[] vertices, out Color[] colours)
    {
        Mesh mesh = new Mesh();
        mesh.name = GetType().Name + "_Dynamic";
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
        // All motion is bounded near the body; fixed bounds so no per-frame recalculation.
        mesh.bounds = new Bounds(anchor, Vector3.one * 2.0f * scale);
        return mesh;
    }

    protected static Color WithAlpha(Color colour, float alpha) { colour.a = alpha; return colour; }

    protected static float Phase(double time, double speed, double offset)
    {
        return (float)((time * speed + offset) % (Math.PI * 2.0));
    }

    // Stable per-particle pseudo-random in [0,1); no state, deterministic, resume-safe.
    protected static float Rand(int i, int salt)
    {
        float v = Mathf.Sin(i * 12.9898f + salt * 78.233f) * 43758.5453f;
        return v - Mathf.Floor(v);
    }

    // Round soft mote. power 2 = gas blob, 3 = tight ember core.
    protected static Texture2D SoftDot(string name, float power)
    {
        const int size = 64;
        Texture2D t = new Texture2D(size, size, TextureFormat.RGBA32, true);
        t.filterMode = FilterMode.Trilinear; t.wrapMode = TextureWrapMode.Clamp; t.name = name;
        Color[] px = new Color[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float u = x / (size - 1f) * 2f - 1f, v = y / (size - 1f) * 2f - 1f;
            float a = Mathf.Pow(Mathf.Max(0f, 1f - (u * u + v * v)), power);
            px[y * size + x] = new Color(1f, 1f, 1f, a);
        }
        t.SetPixels(px); t.Apply(true, true);
        return t;
    }

    // Square-ish node for a digital/holographic look (box falloff instead of radial).
    protected static Texture2D SoftBox(string name, float power)
    {
        const int size = 64;
        Texture2D t = new Texture2D(size, size, TextureFormat.RGBA32, true);
        t.filterMode = FilterMode.Trilinear; t.wrapMode = TextureWrapMode.Clamp; t.name = name;
        Color[] px = new Color[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float u = x / (size - 1f) * 2f - 1f, v = y / (size - 1f) * 2f - 1f;
            float d = Mathf.Max(Mathf.Abs(u), Mathf.Abs(v));
            float a = Mathf.Pow(Mathf.Max(0f, 1f - d), power);
            px[y * size + x] = new Color(1f, 1f, 1f, a);
        }
        t.SetPixels(px); t.Apply(true, true);
        return t;
    }

    private void OnDisable()
    {
        foreach (Renderer r in renderers) if (r != null) r.enabled = false;
    }

    private void OnDestroy()
    {
        ready = false;
        foreach (UnityEngine.Object o in owned) if (o != null) Destroy(o);
        owned.Clear();
    }
}

/// <summary>AWP [Cyber Neon] (9080): a rotating dual-colour holo-ring of sharp cyan/magenta nodes
/// around the scope, swept by a scanning highlight. Reads as a digital targeting halo.</summary>
public sealed class CyberNeonWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9080;
    private static readonly Color Cyan = new Color(.15f, .90f, 1f);
    private static readonly Color Magenta = new Color(1f, .18f, .80f);
    protected override int SpriteCount { get { return 30; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<CyberNeonWeaponEffect>(weaponRoot, itemId, ItemId, "CyberNeon_HoloRing", "AWP", 1.477879f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftBox("CyberNeon_Node", 1.4f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        Vector3 centre = new Vector3(bodyBounds.center.x, bodyBounds.max.y + .05f * scale,
            bodyBounds.center.z + bodyBounds.size.z * .12f);
        Quaternion tilt = Quaternion.Euler(72f, 0f, 0f); // shallow ellipse tilted toward the player
        float spin = (float)(time * .7);
        int n = SpriteCount;
        for (int i = 0; i < n; i++)
        {
            float a = i * (Mathf.PI * 2f / n) + spin;
            float radius = (i % 2 == 0 ? .050f : .066f) * scale;
            positions[i] = centre + tilt * new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
            float scan = Mathf.Pow(.5f + .5f * Mathf.Cos(a - (float)time * 2.2f), 10f); // sharp sweep
            sizes[i] = (.0038f + scan * .0052f) * scale;
            tints[i] = WithAlpha(i % 2 == 0 ? Cyan : Magenta, .30f + scan * .60f);
        }
    }
}

/// <summary>AWP [Toxic Venom] (9081): slow acid-green gas wisps rising and swelling, threaded with
/// smaller bright bubble motes. Reads as bubbling biohazard ooze venting off the body.</summary>
public sealed class ToxicVenomWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9081;
    private const int GasCount = 18;
    private static readonly Color Bright = new Color(.70f, 1f, .30f);
    private static readonly Color Deep = new Color(.12f, .45f, .05f);
    private static readonly Color Acid = new Color(.55f, 1f, .12f);
    protected override int SpriteCount { get { return 34; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<ToxicVenomWeaponEffect>(weaponRoot, itemId, ItemId, "ToxicVenom_Gas", "AWP", 1.477879f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftDot("ToxicVenom_Blob", 2f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        // Body cross-section is taller than wide (exported AWP ~.097 x .269): an elliptical surface
        // so wisps start ON the gun. rx/ry keep them hugging it.
        float rx = .045f * scale, ry = .11f * scale;
        for (int i = 0; i < SpriteCount; i++)
        {
            bool gas = i < GasCount;
            float life = (float)(time * (gas ? .30 : .55) + Rand(i, 1)) % 1f;
            // Spread the emit point along the whole barrel, and give each a radial angle around it
            // (slowly swirling so it is not striped) so wisps seep out in EVERY direction, not just up.
            float along = Rand(i, 2);
            float ang = Rand(i, 3) * 6.283f + (float)time * .25f;
            float z = Mathf.Lerp(bodyBounds.min.z + bodyBounds.size.z * .16f,
                                 bodyBounds.min.z + bodyBounds.size.z * .88f, along);
            Vector3 radial = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f);
            Vector3 surface = new Vector3(bodyBounds.center.x + radial.x * rx,
                                          bodyBounds.center.y + radial.y * ry, z);
            // Drift only a short way outward so the cloud stays tight to the body, with a little
            // tangential curl and axial slosh so it reads as gas rather than straight spokes.
            float travel = (gas ? .030f : .045f) * scale * life;
            Vector3 tangent = new Vector3(-radial.y, radial.x, 0f);
            float curl = Mathf.Sin(life * Mathf.PI * 2f + Rand(i, 4) * 6.283f) * (gas ? .012f : .006f) * scale;
            float axial = Mathf.Cos(life * Mathf.PI * 2f + Rand(i, 5) * 6.283f) * .010f * scale;
            positions[i] = surface + radial * travel + tangent * curl + new Vector3(0f, 0f, axial);
            float fade = Mathf.Sin(life * Mathf.PI); // 0 at spawn/edge, 1 mid-life
            if (gas)
            {
                sizes[i] = (.011f + life * .018f) * scale;
                tints[i] = WithAlpha(Color.Lerp(Bright, Deep, life), .12f * fade);
            }
            else
            {
                sizes[i] = (.0035f + (1f - life) * .004f) * scale;
                tints[i] = WithAlpha(Acid, .60f * fade);
            }
        }
    }
}

/// <summary>AWP [Molten Inferno] (9082): embers rising off the body, flickering and cooling from
/// white-hot through orange to deep red as they climb, with a few brighter drifting flakes.</summary>
public sealed class MoltenInfernoWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9082;
    private const int EmberCount = 28;
    private static readonly Color WhiteHot = new Color(1f, .95f, .80f);
    private static readonly Color Orange = new Color(1f, .48f, .08f);
    private static readonly Color DeepRed = new Color(.55f, .07f, .02f);
    protected override int SpriteCount { get { return 32; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<MoltenInfernoWeaponEffect>(weaponRoot, itemId, ItemId, "MoltenInferno_Embers", "AWP", 1.477879f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftDot("MoltenInferno_Ember", 2.5f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        for (int i = 0; i < SpriteCount; i++)
        {
            bool flake = i >= EmberCount;
            float life = (float)(time * (flake ? .30 : .60) + Rand(i, 1)) % 1f;
            float along = Rand(i, 2);
            float side = Rand(i, 3) - .5f;
            Vector3 basePos = new Vector3(
                bodyBounds.center.x + side * .035f * scale,
                bodyBounds.max.y,
                Mathf.Lerp(bodyBounds.min.z + bodyBounds.size.z * .22f,
                           bodyBounds.min.z + bodyBounds.size.z * .85f, along));
            float rise = life * (flake ? .14f : .10f) * scale;
            float drift = Mathf.Sin(life * 6.283f + Rand(i, 4) * 6.283f) * .015f * scale;
            positions[i] = basePos + new Vector3(drift, rise + Rand(i, 5) * .01f * scale, 0f);
            float flick = .70f + .30f * Mathf.Sin((float)time * 25f + Rand(i, 6) * 30f);
            float fade = 1f - life; // brightest at spawn, fades as it climbs
            Color hot = Color.Lerp(WhiteHot, Color.Lerp(Orange, DeepRed, life), life);
            if (flake)
            {
                sizes[i] = (.005f + .003f * flick) * scale;
                tints[i] = WithAlpha(hot, .60f * fade * flick);
            }
            else
            {
                sizes[i] = (.0022f + .0014f * flick) * scale;
                tints[i] = WithAlpha(hot, .75f * fade * flick);
            }
        }
    }
}

/// <summary>Wrecker [Voidglass] (9085): the amethyst crystal core lit from within, a slow energy
/// pulse plus refracted light-shards orbiting the glass on tilted planes. Attaches to polySurface24
/// (the Wrecker's glass-bearing body), not the AWP.</summary>
public sealed class VoidglassWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9085;
    private const int CoreMotes = 6;
    private static readonly Color Violet = new Color(.51f, .18f, 1f);
    private static readonly Color Magenta = new Color(1f, .12f, .70f);
    private static readonly Color CoolWhite = new Color(.85f, .80f, 1f);
    protected override int SpriteCount { get { return 34; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<VoidglassWeaponEffect>(weaponRoot, itemId, ItemId, "Voidglass_Crystal", "polySurface24", 0.853306f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftBox("Voidglass_Shard", 1.3f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        Vector3 core = bodyBounds.center; // orbit the crystal core inside the weapon, not a scope
        float pulse = .5f + .5f * Mathf.Sin(Phase(time, .6, 0));
        Quaternion tiltA = Quaternion.Euler(20f, 0f, 15f), tiltB = Quaternion.Euler(20f, 0f, -15f);
        int orbit = SpriteCount - CoreMotes;
        for (int i = 0; i < SpriteCount; i++)
        {
            if (i < CoreMotes)
            {
                // Inner glow breathing inside the crystal core with the energy pulse.
                float ang = i * (Mathf.PI * 2f / CoreMotes) + (float)time * .5f;
                float r = .010f * scale * (.4f + Rand(i, 3) * .6f);
                core.y = bodyBounds.center.y;
                positions[i] = core + new Vector3(Mathf.Cos(ang) * r, Mathf.Sin(ang) * r * .6f, Mathf.Sin(ang) * r);
                sizes[i] = (.010f + .006f * pulse) * scale;
                tints[i] = WithAlpha(Color.Lerp(Violet, Magenta, pulse), .10f + .12f * pulse);
            }
            else
            {
                int j = i - CoreMotes;
                float a = j * (Mathf.PI * 2f / orbit) + Phase(time, .35, 0);
                float radius = (.045f + (j % 3) * .010f) * scale;
                Quaternion tilt = (j % 2 == 0) ? tiltA : tiltB;
                positions[i] = bodyBounds.center + tilt * new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius * .7f);
                sizes[i] = (.0035f + Rand(j, 1) * .0035f) * scale;
                float sparkle = Mathf.Pow(.5f + .5f * Mathf.Sin(a - (float)time * 2f + Rand(j, 2) * 6.283f), 8f);
                tints[i] = WithAlpha(Color.Lerp(Violet, CoolWhite, sparkle), .12f + sparkle * .55f);
            }
        }
    }
}

/// <summary>Splattergun [Prism Splatter] (9086): glowing paint droplets flicking off the muzzle in
/// cyan / magenta / lime, thrown forward on a downward gravity arc. Attaches to SplatterBody.</summary>
public sealed class PrismSplatterWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9086;
    private static readonly Color Cyan = new Color(.15f, .90f, 1f);
    private static readonly Color Magenta = new Color(1f, .15f, .80f);
    private static readonly Color Lime = new Color(.60f, 1f, .10f);
    protected override int SpriteCount { get { return 26; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<PrismSplatterWeaponEffect>(weaponRoot, itemId, ItemId, "PrismSplatter_Drops", "SplatterBody", 0.645136f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftDot("PrismSplatter_Drop", 3f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        Vector3 muzzle = new Vector3(bodyBounds.center.x, bodyBounds.center.y + .02f * scale, bodyBounds.max.z);
        for (int i = 0; i < SpriteCount; i++)
        {
            float life = (float)(time * (.5 + Rand(i, 5) * .3) + Rand(i, 1)) % 1f;
            float ang = Rand(i, 2) * 6.283f;
            float spread = .25f + Rand(i, 3) * .55f;
            // forward off the barrel (+z) with lateral spread + slight lift, then gravity arcs it down
            Vector3 vel = new Vector3(Mathf.Cos(ang) * spread, Mathf.Sin(ang) * spread + .35f, .55f + Rand(i, 4) * .5f);
            Vector3 arc = vel * life + new Vector3(0f, -1.4f, 0f) * (life * life * .5f);
            positions[i] = muzzle + arc * (.11f * scale);
            float fade = Mathf.Sin(life * Mathf.PI);
            sizes[i] = (.0045f + (1f - life) * .0030f) * scale;
            Color c = (i % 3 == 0) ? Cyan : (i % 3 == 1) ? Magenta : Lime;
            tints[i] = WithAlpha(c, .60f * fade);
        }
    }
}

/// <summary>Grenade Launcher [Dragon's Maw] (9087): a faint ember breath drifting out of the muzzle
/// mouth, hot embers cooling white->orange->red among soft warm haze puffs. Attaches to "Launcher".</summary>
public sealed class DragonsMawWeaponEffect : WeaponEmitterEffect
{
    public const int ItemId = 9087;
    private const int HazeCount = 8;
    private static readonly Color WhiteHot = new Color(1f, .92f, .70f);
    private static readonly Color Orange = new Color(1f, .45f, .07f);
    private static readonly Color DeepRed = new Color(.55f, .07f, .02f);
    private static readonly Color Haze = new Color(.70f, .28f, .08f); // warm breath (additive-friendly)
    protected override int SpriteCount { get { return 30; } }

    public static void Apply(GameObject weaponRoot, int itemId)
    {
        Attach<DragonsMawWeaponEffect>(weaponRoot, itemId, ItemId, "DragonsMaw_Breath", "Launcher", 0.783206f);
    }

    protected override Texture2D BuildSpriteTexture() { return Keep(SoftDot("DragonsMaw_Ember", 2.5f)); }

    protected override void ConfigureMaterial(Material material)
    {
        if (material.HasProperty("_TintColor")) material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    protected override void Emit(double time)
    {
        // Muzzle mouth = front of the barrel; embers and warm haze breathe out and up, cooling.
        Vector3 mouth = new Vector3(bodyBounds.center.x, bodyBounds.center.y + .01f * scale, bodyBounds.max.z);
        for (int i = 0; i < SpriteCount; i++)
        {
            bool haze = i < HazeCount;
            float life = (float)(time * (haze ? .22 : .50) + Rand(i, 1)) % 1f;
            float ang = Rand(i, 2) * 6.283f;
            float spread = (haze ? .04f : .03f) * scale;
            float curl = Mathf.Sin(life * Mathf.PI * 2f + ang) * spread * .5f;
            positions[i] = mouth + new Vector3(
                Mathf.Cos(ang) * spread + curl,
                (.02f + life * (haze ? .12f : .09f)) * scale,
                (.01f + life * .05f) * scale + Mathf.Sin(life * 3f + ang) * .004f * scale);
            float flick = .70f + .30f * Mathf.Sin((float)time * 22f + Rand(i, 6) * 30f);
            float fade = Mathf.Sin(life * Mathf.PI);
            if (haze)
            {
                sizes[i] = (.014f + life * .024f) * scale;
                tints[i] = WithAlpha(Haze, .07f * fade);
            }
            else
            {
                Color hot = Color.Lerp(WhiteHot, Color.Lerp(Orange, DeepRed, life), life);
                sizes[i] = (.0025f + .0015f * flick) * scale;
                tints[i] = WithAlpha(hot, .70f * (1f - life) * flick);
            }
        }
    }
}
