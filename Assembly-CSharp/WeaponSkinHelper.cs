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
//   9008 Inferno MG     (base 1002 MachineGun)  -- texture PARKED, see SkinTextures below
//   9009 Cryo Strike    (base 1004 PaintSniper)
//   9010 Solar Cannon   (base 1005 Cannon)
//   9011 Toxic Splatter (base 1003 PaintShotty)
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
		// 9008 Inferno MG parked: texture appears rotated on the real Steam client's
		// MachineGun mesh UVs. Needs a proper Unity Editor side-by-side comparison
		// against the 4.3.8 reference to fix correctly. 9008 still equips fine, just
		// shows the unskinned base MachineGun until this is revisited.
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
		// correctly (0.4% to 4.4%). 9008 Inferno MG is still parked for the same original
		// reason and needs the same repaint.
		{ 9015, "9015_NeonCircuit.png" },
	};

	public static readonly Dictionary<int, string> IconTextures = new Dictionary<int, string>
	{
		{ 9007, "9007_PlasmaBat_Icon.png" },
		{ 9008, "9008_InfernoMG_Icon.png" },
		{ 9009, "9009_CryoStrike_Icon.png" },
		{ 9010, "9010_SolarCannon_Icon.png" },
		{ 9011, "9011_ToxicSplatter_Icon.png" },
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
			return null; // not one of our skins (or parked, e.g. 9008)

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
		}
	}

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
