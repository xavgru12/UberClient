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
		string path = SkinPath(fileName);
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
