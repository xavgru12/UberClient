using System.Collections.Generic;
using UnityEngine;

// One authored per-id block per skin. Empty options omitted. The skin tool exports this
// format; paste a block in here. A static ctor fans Skins out into the legacy per-attribute
// dictionaries in WeaponSkinHelper, so all existing lookups keep working unchanged.
public static partial class WeaponSkinHelper
{
	public class SkinDef
	{
		public string SkinTextures;
		public float? SkinShellScale;
		public FlameMode? SkinFlameModes;
		public string[] SkinShaders;
		public string SkinShaderResources;
		public MaterialBindings SkinMaterialBindings;
		public Color? SkinReflectTints;
		public Color? SkinFlameTints;
		public SleeveSpec? SkinSleeves;
		public string SkinFlames;
		public string IconTextures;
		public TracerSpec? TracerOverrides;
		public MuzzleTintSpec? MuzzleTints;
	}

	public static readonly Dictionary<int, SkinDef> Skins = new Dictionary<int, SkinDef>
	{
		{ 2010, new SkinDef { SkinTextures = "2010_HazardousShotgun.png", IconTextures = "2010_HazardousShotgun_Icon.png" } },
	};

	static WeaponSkinHelper()
	{
		foreach (var kv in Skins)
		{
			int id = kv.Key; var s = kv.Value;
			if (s.SkinTextures != null) SkinTextures[id] = s.SkinTextures;
			if (s.SkinShellScale.HasValue) SkinShellScale[id] = s.SkinShellScale.Value;
			if (s.SkinFlameModes.HasValue) SkinFlameModes[id] = s.SkinFlameModes.Value;
			if (s.SkinShaders != null) SkinShaders[id] = s.SkinShaders;
			if (s.SkinShaderResources != null) SkinShaderResources[id] = s.SkinShaderResources;
			if (s.SkinMaterialBindings != null) SkinMaterialBindings[id] = s.SkinMaterialBindings;
			if (s.SkinReflectTints.HasValue) SkinReflectTints[id] = s.SkinReflectTints.Value;
			if (s.SkinFlameTints.HasValue) SkinFlameTints[id] = s.SkinFlameTints.Value;
			if (s.SkinSleeves.HasValue) SkinSleeves[id] = s.SkinSleeves.Value;
			if (s.SkinFlames != null) SkinFlames[id] = s.SkinFlames;
			if (s.IconTextures != null) IconTextures[id] = s.IconTextures;
			if (s.TracerOverrides.HasValue) TracerOverrides[id] = s.TracerOverrides.Value;
			if (s.MuzzleTints.HasValue) MuzzleTints[id] = s.MuzzleTints.Value;
		}
	}
}
