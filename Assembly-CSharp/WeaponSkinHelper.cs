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
using System.Reflection;
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
	};

	public static readonly Dictionary<int, string> IconTextures = new Dictionary<int, string>
	{
		{ 9007, "9007_PlasmaBat_Icon.png" },
		{ 9008, "9008_InfernoMG_Icon.png" },
		{ 9009, "9009_CryoStrike_Icon.png" },
		{ 9010, "9010_SolarCannon_Icon.png" },
		{ 9011, "9011_ToxicSplatter_Icon.png" },
	};

	private static readonly Dictionary<int, Texture2D> _skinCache = new Dictionary<int, Texture2D>();
	private static readonly Dictionary<int, Texture2D> _iconCache = new Dictionary<int, Texture2D>();

	private static Texture2D LoadEmbedded(string resourceName)
	{
		Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		if (stream == null)
		{
			Debug.LogError("WeaponSkinHelper: embedded resource not found: " + resourceName);
			return null;
		}

		byte[] data;
		using (stream)
		{
			data = new byte[stream.Length];
			int total = 0;
			while (total < data.Length)
			{
				int read = stream.Read(data, total, data.Length - total);
				if (read <= 0) break;
				total += read;
			}
		}

		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		if (!tex.LoadImage(data))
		{
			Debug.LogError("WeaponSkinHelper: Texture2D.LoadImage failed for " + resourceName);
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

		Texture2D tex = LoadEmbedded(resourceName);
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

		Texture2D tex = LoadEmbedded(resourceName);
		if (tex != null)
			_iconCache[itemId] = tex;
		return tex;
	}

	// Called from Avatar.AssignWeapon right after a weapon is attached to a player.
	public static void ApplyToWeapon(GameObject weaponRoot, int itemId)
	{
		if (weaponRoot == null)
			return;

		Texture2D tex = GetSkinTexture(itemId);
		if (tex == null)
			return; // not one of our skins, leave the weapon alone

		Renderer[] renderers = weaponRoot.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in renderers)
		{
			if (r == null || r.material == null)
				continue;

			r.material.mainTexture = tex;
			if (r.material.HasProperty("_MainTex"))
				r.material.SetTexture("_MainTex", tex);
		}
	}
}
