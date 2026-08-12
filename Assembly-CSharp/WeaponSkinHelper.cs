// WeaponSkinHelper.cs — source-level port of the Elements weapon skin batch.
//
// Previously shipped as a runtime patch (BepInEx/Harmony mod, then a Mono.Cecil static
// IL patch against a compiled Assembly-CSharp.dll — see constripacity/uberstrike-weapon-skins
// on disk). HaZard asked for this done directly in this repo's source instead, so this
// ports the same, already-verified-working logic: hook the real weapon-attach path
// (Avatar.AssignWeapon) and swap the material's texture, and override the shop icon
// (ProxyItem's constructor) for these 5 item ids. No custom mesh, no AssetBundle --
// these are pure re-textures of weapons that already exist in the game.
//
// Item ID -> base weapon mapping (from unity_2022_tg/skin-framework commit 850893ee):
//   9007 Plasma Bat     (base 1000 TheSplatbat)
//   9008 Inferno MG     (base 1002 MachineGun)
//   9009 Cryo Strike    (base 1004 PaintSniper)
//   9010 Solar Cannon   (base 1005 Cannon)
//   9011 Toxic Splatter (base 1003 PaintShotty)
//   9016 Crimson Dragon (base 6  MythicEdge-DE, premium melee)
//   9017 Frostbound     (base 6  MythicEdge-DE, premium melee) -- see-through, animated
//
// This does NOT check ownership/equip state beyond what the game itself already enforces
// via AssignWeapon (only ever called with an item the player has equipped in their
// loadout) -- once the server knows about these item ids and a player's loadout
// references one, this just changes what texture renders. No local bypass.

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WeaponSkinHelper
{
	// itemId -> embedded resource name (LogicalName in the .csproj).
	public static readonly Dictionary<int, string> SkinTextures = new Dictionary<int, string>
	{
		{ 9007, "9007_PlasmaBat.png" },
		// 9008 repainted 2026-08-11 against the 4.7.1 MachineGun base, same as 9015.
		// It was parked on the theory that the texture was rotated on this client's mesh;
		// that was wrong. The real cause was the UV layout, see the 9015 note below.
		// Measures 4.1% unpainted at brightness 153.0.
		{ 9008, "9008_InfernoMG.png" },
		{ 9009, "9009_CryoStrike.png" },
		{ 9010, "9010_SolarCannon.png" },
		{ 9011, "9011_ToxicSplatter.png" },
		// 2026-08-06 batch. Sniper, shotgun and cannon port cleanly to the 4.7.1
		// meshes: like the four above, these are authored against the 4.3.8 base UV
		// layouts, which measure 0.959 to 0.979 island recall against the meshes
		// this client actually uses.
		{ 9012, "9012_VoidAmethyst.png" },
		{ 9013, "9013_Bloodhound.png" },
		{ 9014, "9014_AbyssalLeviathan.png" },
		// 9015 repainted 2026-08-11 against the 4.7.1 MachineGun base. The original was
		// authored on the 4.3.8 texture, whose UV layout uses 47% of the sheet against
		// 4.7.1's 87%, so it left ~37% of the mesh unpainted and rendered black in patches.
		// The repaint measures 5.1% unpainted, in line with every skin that renders
		// correctly (0.4% to 4.4%). 9008 above is the same fix applied to the other
		// MachineGun skin, so both are now live.
		{ 9015, "9015_NeonCircuit.png" },
		// 2026-08-11. First two skins on PREMIUM base weapons rather than the five stock
		// ones. These live in Resources/items/weapons/<slug>/res/ with their own material
		// and prefab, so unlike the others their base texture is not in Texture2D/.
		//
		// 9016 Crimson Dragon is the Mythic Edge katana (base item 6, prefab MythicEdge-DE).
		// Its base is 512x1024, not square, and the generator only emits squares: asking it
		// directly made it re-lay-out the UV islands to fill the canvas. Fixed by padding
		// the base into a square before generation and cropping the padding back off, which
		// leaves every island on its original pixel. Measures 10.7% unpainted against the
		// base's own 10.7% floor.
		//
		//
		// An AWP skin was built and withdrawn the same day. It measured well (3.6-7.3%
		// unpainted against that base's 22.4% floor) but never resembled the concept art it
		// was chasing, and the reason turned out to be structural rather than fixable: that
		// concept is a separate 1.5M-triangle PBR asset with its own UV layout and
		// metallic/roughness maps, not a texture for this rifle at all. No reskin of
		// AWP_Roughed can reach it. Doing so needs the mesh decimated, baked down to
		// diffuse+normal, and shipped in an AssetBundle -- a custom mesh, not a re-texture.
		{ 9016, "9016_CrimsonDragon.png" },
		// 2026-08-12. Second skin on the katana, and the first that is not a pure re-texture:
		// Frostbound is genuinely see-through, with animated flames over it.
		//
		// Its ALPHA CHANNEL MEANS SOMETHING DIFFERENT from every other skin here. The stock
		// material is Bumped Specular, whose shader reads `o.Gloss = tex.a`, so alpha is a
		// GLOSS mask everywhere else in this file. This skin swaps the shader for an
		// alpha-blended one, where _MainTex is "Base (RGB) Trans (A)" -- so its alpha is
		// TRANSPARENCY. Do not composite the base weapon's specular mask onto this one; it
		// would come out as an opacity map and read as a smeared, half-dissolved sword.
		//
		// The translucency is authored from the base's own gloss mask through a smoothstep
		// rather than copied: gloss is bimodal on this weapon (p50 0.15, p95 1.00), so the
		// polished-metal texels are separable from the wrap and saya panels. That puts the
		// blade at 8.5% of the sheet under alpha 0.55 while 85% stays solid, which is what
		// keeps the grip looking held rather than ghostly.
		{ 9017, "9017_Frostbound.png" },
	};

	/// <summary>
	/// Optional per-skin SHADER override, in preference order, with the first one that
	/// resolves winning.
	///
	/// Order is not cosmetic. Glass-Hangar is the better look -- it carries a reflection
	/// cubemap, which sells ice far better than a specular highlight -- but it is referenced
	/// by ZERO item materials in the client, so nothing guarantees it survived shader
	/// stripping into the shipped build, and Shader.Find would then return null. Transparent/
	/// Diffuse is referenced by shipped gear (bandannahead, beardandmo, cap, juliaenzo) and
	/// by the Wrecker's own glass submaterial, so it is certain to be present. The Wrecker is
	/// also the precedent that this works at all: it already ships an alpha-blended
	/// submaterial on a weapon the player holds.
	///
	/// Note both are Lambert -- there is no alpha-BLENDED bumped specular anywhere in the
	/// client's 63 shaders, only a cutout one, which does binary on/off and reads as holes
	/// rather than glass. So a see-through blade costs us the normal map, and the surface
	/// relief has to live in the painted colour instead. That trade is deliberate.
	/// </summary>
	public static readonly Dictionary<int, string[]> SkinShaders = new Dictionary<int, string[]>
	{
		{ 9017, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
	};

	/// <summary>
	/// Optional per-skin animated flame overlay: a second copy of the weapon's own mesh,
	/// drawn additively over the top with its UVs scrolling.
	///
	/// Additive is why the sheet is black-backed. "Particles/Additive" adds its texture to
	/// whatever is behind it, so black contributes nothing and brightness IS opacity -- the
	/// black background is the transparency, not a placeholder for it. A sheet whose
	/// background sits just above zero glows as a permanent haze over the whole weapon.
	/// </summary>
	public static readonly Dictionary<int, string> SkinFlames = new Dictionary<int, string>
	{
		{ 9017, "9017_Frostbound_Flames.png" },
	};

	// Shop icons. ProxyItem loads the BASE weapon's "<prefabPath>-Icon" from Resources and we
	// replace it by item id, so an id missing here silently shows the base weapon's icon --
	// or nothing, since the five stock weapons (TheSplatbat, MachineGun, SniperRifle, Cannon,
	// ShotGun) ship no icon at all and fall back to a per-class default.
	//
	// All ten are rendered by tools/render_weapon_icon.py in uberstrike-patcher-workshop, to
	// the convention measured off the 133 stock 48x48 shop icons rather than to taste:
	// 48x48 opaque RGBA on the recovered plate, weapon bbox 0.923 of the width, centroid at
	// (0.533, 0.459), long axis near horizontal, and the muzzle pointing LEFT, which 115 of
	// 115 unambiguously directional stock icons do.
	public static readonly Dictionary<int, string> IconTextures = new Dictionary<int, string>
	{
		{ 9007, "9007_PlasmaBat_Icon.png" },
		{ 9008, "9008_InfernoMG_Icon.png" },
		{ 9009, "9009_CryoStrike_Icon.png" },
		{ 9010, "9010_SolarCannon_Icon.png" },
		{ 9011, "9011_ToxicSplatter_Icon.png" },
		{ 9012, "9012_VoidAmethyst_Icon.png" },
		{ 9013, "9013_Bloodhound_Icon.png" },
		{ 9014, "9014_AbyssalLeviathan_Icon.png" },
		{ 9015, "9015_NeonCircuit_Icon.png" },
		{ 9016, "9016_CrimsonDragon_Icon.png" },
		{ 9017, "9017_Frostbound_Icon.png" },
	};

	// Optional per item tracer: gives a weapon a travelling muzzle to hitpoint beam it
	// would not otherwise have, in a custom colour. Purely cosmetic, but note it IS
	// visible in gameplay rather than being a pure re-texture like everything above.
	public struct TracerSpec
	{
		public ParticleConfigurationType Effect;
		public Color Start;
		public Color End;

		// Material _TintColor, deliberately separate from the line colours because it is
		// MULTIPLIED by the trail texture. SRParticleLanceTrail5 averages RGB 144,91,45,
		// so its blue channel is only about 0.18 against red at 0.56 and an even handed
		// pink tint comes out RED. Channels are compensated roughly target/texture, which
		// is why blue exceeds 1.0. Unity allows that for material colours.
		public Color MatTint;
	}

	public static readonly Dictionary<int, TracerSpec> TracerOverrides = new Dictionary<int, TracerSpec>
	{
		// Only ParticleLance and FusionLance set UseTrailrendererForTrail, so ParticleLance
		// is what actually produces a beam. The stock SniperRifleDefault is muzzle flash only.
		{ 9012, new TracerSpec {
			Effect  = ParticleConfigurationType.ParticleLance,
			Start   = new Color(1.00f, 0.45f, 0.85f, 1f),
			End     = new Color(1.00f, 0.15f, 0.60f, 1f),
			MatTint = new Color(0.90f, 0.30f, 3.00f, 1f) } },
	};

	private static readonly Dictionary<int, Texture2D> _skinCache = new Dictionary<int, Texture2D>();
	private static readonly Dictionary<int, Texture2D> _iconCache = new Dictionary<int, Texture2D>();

	/// <summary>
	/// Folder holding the skin PNGs, relative to <c>UberStrike_Data</c>.
	/// Distributed through the patcher's Entry.txt like any other game file.
	/// </summary>
	public const string SkinFolder = "Skins";

	/// <summary>Absolute path of a skin file, for logging and for the patcher's manifest.</summary>
	public static string SkinPath(string fileName)
	{
		// Application.dataPath is "<install>/UberStrike_Data" for a Windows player.
		return Path.Combine(Path.Combine(Application.dataPath, SkinFolder), fileName);
	}

	/// <summary>
	/// Load a skin texture from disk.
	///
	/// These used to be EmbeddedResources compiled into this assembly, which took
	/// Assembly-CSharp.dll from 1.4 MB to 51 MB - 97 percent of the file was PNG. Nothing
	/// else in UberStrike ships that way: the game has 1,228 textures and 1.4 GB of art,
	/// none of it in a managed assembly. Loading from disk puts the assembly back to its
	/// normal size and lets the patcher update art without re-shipping code, and vice versa.
	///
	/// Texture2D.LoadImage handles PNG and JPEG on this client (Unity 4.6.5), and produces
	/// a texture with mipmaps disabled, matching the previous embedded behaviour exactly.
	/// </summary>
	private static Texture2D LoadFromDisk(string fileName)
	{
		// Preferred layout: colour as JPEG, specular mask as a separate lossless PNG.
		//
		// A 2048 skin is 5.8-7.3 MB as RGBA PNG because PNG is lossless and this art is
		// dense AI-generated detail with little to compress. The same colour at JPEG q92
		// (chroma subsampling OFF - 4:2:0 would smear the colour and is exactly what
		// wrecks textures) measures 41.9-45.2 dB PSNR against the original, roughly 1%
		// average per-channel error, for 5.4-7.5x less data.
		//
		// The alpha channel is NOT compressed. It carries the specular mask composited
		// from the base weapon, which is what makes these read as metal rather than flat
		// paint, so it stays bit-for-bit lossless in its own greyscale PNG.
		//
		// Falls back to a single RGBA PNG when no pair is present, so both layouts work
		// and a skin can be switched over one at a time.
		string stem = Path.GetFileNameWithoutExtension(fileName);
		string jpeg = SkinPath(stem + ".jpg");
		string mask = SkinPath(stem + ".alpha.png");

		if (File.Exists(jpeg))
		{
			Texture2D colour = LoadImageFile(jpeg);
			if (colour == null)
				return null;
			if (!File.Exists(mask))
				return colour; // colour-only skin, e.g. one with no specular mask

			Texture2D maskTex = LoadImageFile(mask);
			if (maskTex == null)
				return colour;

			if (maskTex.width != colour.width || maskTex.height != colour.height)
			{
				Debug.LogError("WeaponSkinHelper: alpha mask size " + maskTex.width + "x" + maskTex.height
					+ " does not match colour " + colour.width + "x" + colour.height + " for " + stem);
				return colour;
			}

			// Texture2D.LoadImage REPLACES the texture format to match the file it read.
			// A JPEG has no alpha, so `colour` comes back as RGB24 and writing alpha into
			// it is silently discarded on Apply. The mask has to go into a texture that
			// actually has an alpha channel, so allocate a fresh RGBA32 one.
			//
			// This is not cosmetic. ApplyToWeapon assigns the skin to every Renderer under
			// the weapon, which includes the muzzle flash quad. That quad is alpha blended,
			// so a mask of mostly zero alpha leaves it invisible as intended, while a fully
			// opaque texture turns it into a visible square. Losing the alpha here shows up
			// on the flash long before it is noticeable on the gun body.
			Texture2D merged = new Texture2D(colour.width, colour.height, TextureFormat.RGBA32, false);
			Color[] rgb = colour.GetPixels();
			Color[] a = maskTex.GetPixels();
			for (int i = 0; i < rgb.Length; i++)
				rgb[i].a = a[i].r; // greyscale mask: any channel carries the value
			merged.SetPixels(rgb);
			merged.Apply(false);
			return merged;
		}

		return LoadImageFile(SkinPath(fileName));
	}

	private static Texture2D LoadImageFile(string path)
	{
		if (!File.Exists(path))
		{
			Debug.LogError("WeaponSkinHelper: skin file not found: " + path);
			return null;
		}

		byte[] data;
		try
		{
			data = File.ReadAllBytes(path);
		}
		catch (Exception e)
		{
			Debug.LogError("WeaponSkinHelper: could not read " + path + ": " + e.Message);
			return null;
		}

		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		if (!tex.LoadImage(data))
		{
			Debug.LogError("WeaponSkinHelper: Texture2D.LoadImage failed for " + path);
			return null;
		}
		return tex;
	}

	public static Texture2D GetSkinTexture(int itemId)
	{
		Texture2D cached;
		if (_skinCache.TryGetValue(itemId, out cached) && cached != null)
			return cached;

		string resourceName;
		if (!SkinTextures.TryGetValue(itemId, out resourceName))
			return null; // not one of our skins

		Texture2D tex = LoadFromDisk(resourceName);
		if (tex != null)
			_skinCache[itemId] = tex;
		return tex;
	}

	public static Texture2D GetIconTexture(int itemId)
	{
		Texture2D cached;
		if (_iconCache.TryGetValue(itemId, out cached) && cached != null)
			return cached;

		string resourceName;
		if (!IconTextures.TryGetValue(itemId, out resourceName))
			return null;

		Texture2D tex = LoadFromDisk(resourceName);
		if (tex != null)
			_iconCache[itemId] = tex;
		return tex;
	}

	// Called from Avatar.AssignWeapon right after a weapon is attached to a player.
	public static void ApplyToWeapon(GameObject weaponRoot, int itemId)
	{
		if (weaponRoot == null)
			return;

		// Before the texture check: the tracer is independent of whether this item has
		// a skin registered, so an item could have one without the other.
		ApplyTracer(weaponRoot, itemId);

		Texture2D tex = GetSkinTexture(itemId);
		if (tex == null)
			return; // not one of our skins, leave the weapon alone

		Renderer[] renderers = weaponRoot.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in renderers)
		{
			if (r == null || r.material == null)
				continue;

			// Skip effect renderers. A weapon's children include its muzzle flash and shell
			// casing, which use "Particle Add" and friends, and painting the gun's diffuse
			// onto an additive quad makes the flash render as a bright rectangle showing a
			// slab of the UV atlas.
			//
			// This has been wrong since the first version of this file, but it stayed
			// invisible for a long time by luck: the earlier skins carried a specular mask
			// that is 82-84% near-zero alpha, so the additive quad multiplied out to nothing.
			// The 4.7.1 MachineGun base's mask is 74.6% MID-range, so the same bug finally
			// showed up as a visible square. Measured, not guessed.
			//
			// Verified in game 2026-08-11: the square is gone. Note the flash then renders
			// as NOTHING rather than as the stock flash, which is not what skipping the
			// assignment alone should do, so something else on that quad depends on this
			// path. Accepted as-is for now; a skinned weapon with no muzzle flash is a
			// better outcome than one with a bright rectangle, but this is not fully
			// understood and is worth revisiting.
			Shader sh = r.material.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;

			r.material.mainTexture = tex;
			if (r.material.HasProperty("_MainTex"))
				r.material.SetTexture("_MainTex", tex);

			ApplyShaderOverride(r, itemId);
		}

		ApplyFlames(weaponRoot, itemId);
	}

	/// <summary>
	/// Swap this renderer's shader, for skins that need to be something other than opaque.
	///
	/// Renderer.material is already a per-instance copy, so assigning a shader here does not
	/// touch the shared material and cannot leak onto another player's weapon.
	///
	/// Falls through the candidate list and takes the first that resolves, because
	/// Shader.Find only finds shaders that actually made it into the build. A shader no
	/// material references may have been stripped, and the failure is silent: the skin would
	/// simply render opaque with no error. Logged so it is visible which one bound.
	/// </summary>
	private static void ApplyShaderOverride(Renderer r, int itemId)
	{
		string[] candidates;
		if (!SkinShaders.TryGetValue(itemId, out candidates) || candidates == null)
			return;

		for (int i = 0; i < candidates.Length; i++)
		{
			Shader s = Shader.Find(candidates[i]);
			if (s == null)
				continue;

			r.material.shader = s;
			if (i > 0)
			{
				Debug.Log("WeaponSkinHelper: skin " + itemId + " fell back to shader '"
					+ candidates[i] + "' ('" + candidates[0] + "' is not in this build)");
			}
			return;
		}

		Debug.LogWarning("WeaponSkinHelper: skin " + itemId
			+ " found none of its shaders in the build; it will render opaque");
	}

	/// <summary>
	/// Attach the animated flame overlay: for every mesh we just skinned, add a child holding
	/// the SAME mesh with an additive material, and scroll its UVs.
	///
	/// The overlay is a separate GameObject rather than a second material on the weapon so it
	/// can be removed by deleting one child, and so the scroll cannot disturb the blade's own
	/// texture offset.
	///
	/// Draw order is set explicitly. The glass sits in the Transparent queue (3000) and the
	/// overlay must come after it, or the flames render behind the blade they are supposed to
	/// be licking across. Transparent geometry does not write depth, so this ordering is the
	/// only thing deciding it.
	/// </summary>
	private static void ApplyFlames(GameObject weaponRoot, int itemId)
	{
		string sheet;
		if (!SkinFlames.TryGetValue(itemId, out sheet))
			return;

		Texture2D flame = LoadFromDisk(sheet);
		if (flame == null)
			return;
		flame.wrapMode = TextureWrapMode.Repeat;   // it scrolls, so it must tile

		Shader additive = Shader.Find("Particles/Additive");
		if (additive == null)
		{
			Debug.LogWarning("WeaponSkinHelper: 'Particles/Additive' missing, no flames for " + itemId);
			return;
		}

		MeshFilter[] filters = weaponRoot.GetComponentsInChildren<MeshFilter>(true);
		foreach (MeshFilter mf in filters)
		{
			if (mf == null || mf.sharedMesh == null)
				continue;

			Renderer src = mf.GetComponent<Renderer>();
			if (src == null || src.material == null)
				continue;

			// Never overlay an effect renderer -- that is the muzzle flash, and stacking an
			// additive copy on an additive quad doubles it into a bright block.
			Shader sh = src.material.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;

			// AssignWeapon can run more than once for the same weapon instance; without this
			// each call would stack another overlay and the flames would get brighter every
			// respawn until the blade was a white blob.
			if (mf.transform.FindChild(FlameChildName) != null)
				continue;

			GameObject go = new GameObject(FlameChildName);
			go.transform.parent = mf.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			// A hair larger so it never z-fights with the surface it sits on.
			go.transform.localScale = new Vector3(1.015f, 1.015f, 1.015f);

			MeshFilter of = go.AddComponent<MeshFilter>();
			of.sharedMesh = mf.sharedMesh;

			MeshRenderer or = go.AddComponent<MeshRenderer>();
			Material m = new Material(additive);
			m.mainTexture = flame;
			if (m.HasProperty("_TintColor"))
			{
				// Particles/Additive multiplies by _TintColor, so this is the intensity dial.
				// Held back deliberately: the sheet is 30% coverage at full white, and at
				// full tint it washes the ice out to a flat glare.
				m.SetColor("_TintColor", new Color(0.42f, 0.62f, 0.78f, 0.5f));
			}
			m.renderQueue = 3100;               // after the glass at 3000
			or.material = m;
			or.castShadows = false;
			or.receiveShadows = false;

			go.AddComponent<WeaponFlameAnimator>();
		}
	}

	private const string FlameChildName = "__SkinFlameOverlay";

	/// <summary>
	/// Swap the weapon's impact/particle config so it draws a travelling beam, and attach
	/// the tinter that recolours each spawned trail.
	/// </summary>
	public static void ApplyTracer(GameObject weaponRoot, int itemId)
	{
		TracerSpec spec;
		if (!TracerOverrides.TryGetValue(itemId, out spec))
			return; // no tracer for this item, leave the weapon alone

		BaseWeaponDecorator decorator = weaponRoot.GetComponent<BaseWeaponDecorator>();
		if (decorator == null)
			return;

		decorator.SetSurfaceEffect(spec.Effect);

		WeaponTracerTinter tinter = weaponRoot.GetComponent<WeaponTracerTinter>();
		if (tinter == null)
			tinter = weaponRoot.AddComponent<WeaponTracerTinter>();
		tinter.StartColour = spec.Start;
		tinter.EndColour = spec.End;
		tinter.MatTint = spec.MatTint;
	}

}


/// <summary>
/// Recolours the muzzle to hitpoint beam produced by MoveTrailrendererObject.
///
/// Two things make this less obvious than it looks:
///
/// 1. The trail is NOT under the weapon. BaseWeaponDecorator caches
///    _parent = transform.parent during Awake, but WeaponSlot.ConfigureWeaponDecorator
///    re-parents the decorator afterwards, so _parent is stale and effectively null.
///    ParticleEffectController.ShowTrailEffect then parents each spawned trail to that,
///    which drops it at the scene root. Hence the scene wide lookup rather than a walk
///    down our own hierarchy.
///
/// 2. Colour lives in two places. The LineRenderer vertex colours decide the hue (the
///    stock ParticleLance gradient has red at zero, which is exactly why that beam reads
///    cyan no matter what the material says), while the material _TintColor is multiplied
///    by the trail texture. MoveTrailrendererObject.Update only rewrites _TintColor's
///    alpha and preserves RGB, so a tint applied once survives the whole fade.
///
/// Renderer.material returns a per instance copy, so nothing shared is touched. That
/// matters because SRParticleLanceTrail.mat is also used by SpringGrenade and
/// LR_FinalWord_MissileSticky.
/// </summary>
/// <summary>
/// Scrolls the flame overlay's UVs so the fire moves up the blade.
///
/// The sheet is authored to tile seamlessly top to bottom -- verified by measuring the wrap
/// join against the texture's own row-to-row difference, which came out at 0.07 where 1.0
/// would mean "indistinguishable from any other row". A sheet that does not loop produces a
/// seam that marches up the weapon once per cycle, forever.
///
/// Offset is wrapped with Mathf.Repeat rather than left to grow. Time.time is a float, and
/// after a long match it is large enough that adding a small delta stops changing the low
/// bits -- the scroll would visibly stutter and then freeze. Keeping the value inside 0..1
/// avoids that entirely.
/// </summary>
public class WeaponFlameAnimator : MonoBehaviour
{
	public float Speed = 0.35f;          // sheet heights per second
	public float SwaySpeed = 0.13f;      // slight horizontal drift so it does not look rigid
	public float SwayAmount = 0.015f;

	private Renderer _renderer;
	private float _v;

	private void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	private void LateUpdate()
	{
		if (_renderer == null || _renderer.material == null)
			return;

		_v = Mathf.Repeat(_v + Speed * Time.deltaTime, 1f);
		float u = Mathf.Sin(Time.time * SwaySpeed * 6.2832f) * SwayAmount;
		_renderer.material.SetTextureOffset("_MainTex", new Vector2(u, _v));
	}
}

public class WeaponTracerTinter : MonoBehaviour
{
	public Color StartColour = Color.white;
	public Color EndColour = Color.white;
	public Color MatTint = Color.white;

	private void LateUpdate()
	{
		UnityEngine.Object[] trails = UnityEngine.Object.FindObjectsOfType(typeof(MoveTrailrendererObject));
		for (int i = 0; i < trails.Length; i++)
		{
			MoveTrailrendererObject trail = trails[i] as MoveTrailrendererObject;
			if (trail == null)
				continue;

			LineRenderer line = trail.GetComponent<LineRenderer>();
			if (line == null)
				line = trail.GetComponentInChildren<LineRenderer>();
			if (line == null)
				continue;

			// SetColors, not startColor/endColor: those properties are Unity 5.5 and later,
			// and this client is built with Unity 4.6.5.
			line.SetColors(StartColour, EndColour);

			Material mat = line.material;
			if (mat != null && mat.HasProperty("_TintColor"))
			{
				Color existing = mat.GetColor("_TintColor");
				mat.SetColor("_TintColor", new Color(MatTint.r, MatTint.g, MatTint.b, existing.a));
			}
		}
	}
}
