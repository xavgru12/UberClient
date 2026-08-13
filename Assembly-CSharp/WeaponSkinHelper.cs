// WeaponSkinHelper.cs — Natural Shotgun (item 9011) weapon skin.
//
// A pure re-texture of an existing weapon: no custom mesh, no AssetBundle, no shader work.
// It hooks the real weapon-attach path (Avatar.AssignWeapon, WeaponSlot.Initialize) and swaps
// the material's texture, and overrides the shop icon in ProxyItem's constructor.
//
//   9011 Natural Shotgun  (base 1003 PaintShotty)
//
// This does NOT check ownership or equip state beyond what the game already enforces:
// AssignWeapon is only ever called with an item the player has equipped in their loadout, so
// once the server knows about item 9011 and a loadout references it, this only changes what
// texture renders. No local bypass.
//
// ---------------------------------------------------------------------------------------
// WHERE THE ART LIVES, AND WHY
//
// The art is COMPILED INTO THIS ASSEMBLY as an embedded resource, so a build of
// Assembly-CSharp.dll is self-contained and there is no second file to deploy.
//
// This is a deliberate reversal of the earlier design, and the reason is worth recording.
// The previous version loaded from "<install>/UberStrike_Data/Skins/", which kept the DLL
// small but made the art a separate deployment step that was never written down. Applying
// the code without also placing those files produces NO skin and NO crash — the loader logs
// a "file not found" and the weapon simply renders stock. That is exactly what happened on
// first integration, and it is indistinguishable from "the patch does not work".
//
// The cost of embedding was measured before choosing it, not assumed:
//
//     base Assembly-CSharp.dll                    1.35 MB
//     + 9011 as lossless RGBA PNG                 8.3 MB total
//     + 9011 as JPEG colour + lossless PNG mask   3.0 MB total   <-- shipped
//
// The colour is JPEG q92 with chroma subsampling OFF (4:2:0 smears colour and is exactly what
// wrecks textures); it measures 41.9-45.2 dB PSNR against the original, ~1% average per-channel
// error. The ALPHA is not compressed: it carries the specular mask composited from the base
// weapon, which is what makes this read as metal rather than flat paint, so it stays
// bit-for-bit lossless in its own greyscale PNG.
//
// For the same reason, embedding does NOT scale to the full skin set: all twelve skins are
// 16.7 MB as JPEG pairs and 62 MB as PNG, which would take the assembly to 18 MB or 63 MB.
// That set should ship as game files with a deployment workflow instead — see
// tools/deploy-skins.ps1 and WEAPON_SKINS.md.
//
// A loose file still WINS over the embedded copy when present, so art can be iterated
// in-place without rebuilding the assembly. Absent, the embedded copy is used and everything
// works out of the box.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class WeaponSkinHelper
{
	/// <summary>Item id -> base file name of the skin texture.</summary>
	public static readonly Dictionary<int, string> SkinTextures = new Dictionary<int, string>
	{
		{ 9011, "9011_NaturalShotgun.png" },
	};

	/// <summary>Item id -> shop icon.</summary>
	public static readonly Dictionary<int, string> IconTextures = new Dictionary<int, string>
	{
		{ 9011, "9011_NaturalShotgun_Icon.png" },
	};

	private static readonly Dictionary<int, Texture2D> _skinCache = new Dictionary<int, Texture2D>();
	private static readonly Dictionary<int, Texture2D> _iconCache = new Dictionary<int, Texture2D>();

	/// <summary>
	/// Optional on-disk override folder, relative to <c>UberStrike_Data</c>. A file here wins
	/// over the embedded copy, so art can be swapped without rebuilding. Nothing needs to be
	/// installed here for the skin to work.
	/// </summary>
	public const string SkinFolder = "Skins";

	/// <summary>Prefix for the embedded resource names, set via LogicalName in the csproj.</summary>
	private const string ResourcePrefix = "WeaponSkins.";

	/// <summary>Absolute path of the optional on-disk override for a skin file.</summary>
	public static string SkinPath(string fileName)
	{
		// Application.dataPath is "<install>/UberStrike_Data" for a Windows player.
		return Path.Combine(Path.Combine(Application.dataPath, SkinFolder), fileName);
	}

	// ------------------------------------------------------------------ byte sources

	/// <summary>Read a skin file: loose file first, then the embedded copy. Null if neither.</summary>
	private static byte[] ReadSkinBytes(string fileName)
	{
		string disk = SkinPath(fileName);
		if (File.Exists(disk))
		{
			try
			{
				return File.ReadAllBytes(disk);
			}
			catch (Exception e)
			{
				// Fall through to the embedded copy rather than failing: a half-written or
				// locked override should not break the skin.
				Debug.LogWarning("WeaponSkinHelper: could not read override " + disk
					+ " (" + e.Message + "); using the embedded copy.");
			}
		}
		return ReadEmbedded(fileName);
	}

	private static byte[] ReadEmbedded(string fileName)
	{
		string resource = ResourcePrefix + fileName;
		try
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			using (Stream s = asm.GetManifestResourceStream(resource))
			{
				if (s == null)
					return null;

				// Read in a loop rather than one Read call: Stream.Read is permitted to
				// return fewer bytes than asked for, and Stream.CopyTo does not exist on
				// the .NET 3.5 profile this client compiles against.
				byte[] buffer = new byte[s.Length];
				int read = 0;
				while (read < buffer.Length)
				{
					int n = s.Read(buffer, read, buffer.Length - read);
					if (n <= 0)
						break;
					read += n;
				}
				if (read != buffer.Length)
				{
					Debug.LogError("WeaponSkinHelper: short read on embedded " + resource
						+ " (" + read + " of " + buffer.Length + " bytes)");
					return null;
				}
				return buffer;
			}
		}
		catch (Exception e)
		{
			Debug.LogError("WeaponSkinHelper: could not read embedded " + resource + ": " + e.Message);
			return null;
		}
	}

	// ------------------------------------------------------------------ decoding

	private static Texture2D DecodeTexture(byte[] data, string label)
	{
		if (data == null)
			return null;

		// Texture2D.LoadImage handles PNG and JPEG on this client (Unity 4.6.5) and produces
		// a texture with mipmaps disabled.
		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		if (!tex.LoadImage(data))
		{
			Debug.LogError("WeaponSkinHelper: Texture2D.LoadImage failed for " + label);
			return null;
		}
		return tex;
	}

	/// <summary>
	/// Load a skin texture. Preferred layout is JPEG colour plus a separate lossless greyscale
	/// PNG carrying the specular mask; falls back to a single RGBA PNG when no pair exists, so
	/// either layout works.
	/// </summary>
	private static Texture2D LoadSkin(string fileName)
	{
		string stem = Path.GetFileNameWithoutExtension(fileName);

		byte[] colourBytes = ReadSkinBytes(stem + ".jpg");
		if (colourBytes == null)
		{
			// No JPEG pair — try the single-file RGBA PNG layout.
			byte[] png = ReadSkinBytes(fileName);
			if (png == null)
			{
				Debug.LogError("WeaponSkinHelper: no art found for " + stem
					+ " (looked for an override in " + SkinPath(stem + ".*")
					+ " and for embedded " + ResourcePrefix + stem + ".*)");
				return null;
			}
			return DecodeTexture(png, stem + ".png");
		}

		Texture2D colour = DecodeTexture(colourBytes, stem + ".jpg");
		if (colour == null)
			return null;

		byte[] maskBytes = ReadSkinBytes(stem + ".alpha.png");
		if (maskBytes == null)
			return colour; // colour-only skin, no specular mask

		Texture2D maskTex = DecodeTexture(maskBytes, stem + ".alpha.png");
		if (maskTex == null)
			return colour;

		if (maskTex.width != colour.width || maskTex.height != colour.height)
		{
			Debug.LogError("WeaponSkinHelper: alpha mask size " + maskTex.width + "x" + maskTex.height
				+ " does not match colour " + colour.width + "x" + colour.height + " for " + stem);
			return colour;
		}

		// Texture2D.LoadImage REPLACES the texture format to match the file it read. A JPEG has
		// no alpha, so `colour` comes back as RGB24 and writing alpha into it is silently
		// discarded on Apply. The mask has to go into a texture that actually has an alpha
		// channel, so allocate a fresh RGBA32 one.
		Texture2D merged = new Texture2D(colour.width, colour.height, TextureFormat.RGBA32, false);
		Color[] rgb = colour.GetPixels();
		Color[] a = maskTex.GetPixels();
		for (int i = 0; i < rgb.Length; i++)
			rgb[i].a = a[i].r; // greyscale mask: any channel carries the value
		merged.SetPixels(rgb);
		merged.Apply(false);
		return merged;
	}

	// ------------------------------------------------------------------ public API

	public static Texture2D GetSkinTexture(int itemId)
	{
		Texture2D cached;
		if (_skinCache.TryGetValue(itemId, out cached) && cached != null)
			return cached;

		string resourceName;
		if (!SkinTextures.TryGetValue(itemId, out resourceName))
			return null; // not one of our skins

		Texture2D tex = LoadSkin(resourceName);
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

		Texture2D tex = DecodeTexture(ReadSkinBytes(resourceName), resourceName);
		if (tex != null)
			_iconCache[itemId] = tex;
		return tex;
	}

	/// <summary>
	/// Called from Avatar.AssignWeapon right after a weapon is attached to a player, and from
	/// WeaponSlot.Initialize for the first-person view.
	/// </summary>
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

			// Skip effect renderers. A weapon's children include its muzzle flash and shell
			// casing, which use "Particle Add" and friends, and painting the gun's diffuse onto
			// an additive quad makes the flash render as a bright rectangle showing a slab of
			// the UV atlas.
			//
			// This stayed invisible for a long time by luck: skins whose specular mask is
			// 82-84% near-zero alpha multiplied out to nothing on an additive quad. A base whose
			// mask is mostly MID-range finally showed it as a visible square. Measured, not
			// guessed, and verified fixed in game on 2026-08-11.
			Shader sh = r.material.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;

			r.material.mainTexture = tex;
			if (r.material.HasProperty("_MainTex"))
				r.material.SetTexture("_MainTex", tex);
		}
	}
}
