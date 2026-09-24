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
		{ 2011, new SkinDef { SkinTextures = "2011_PlasmaBat.png", IconTextures = "2011_PlasmaBat_Icon.png" } },
		{ 2012, new SkinDef { SkinTextures = "2012_InfernoMG.png", IconTextures = "2012_InfernoMG_Icon.png" } },
		{ 2013, new SkinDef { SkinTextures = "2013_CryoStrike.png", IconTextures = "2013_CryoStrike_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } } },
		{ 2014, new SkinDef { SkinTextures = "2014_SolarCannon.png", IconTextures = "2014_SolarCannon_Icon.png" } },
		{ 2015, new SkinDef { SkinTextures = "2015_VoidAmethyst.png", IconTextures = "2015_VoidAmethyst_Icon.png", TracerOverrides = new TracerSpec { Effect = ParticleConfigurationType.ParticleLance, Start = new Color(1.00f, 0.45f, 0.85f, 1f), End = new Color(1.00f, 0.15f, 0.60f, 1f), MatTint = new Color(0.90f, 0.30f, 3.00f, 1f) } } },
		{ 2016, new SkinDef { SkinTextures = "2016_Bloodhound.png", IconTextures = "2016_Bloodhound_Icon.png" } },
		{ 2017, new SkinDef { SkinTextures = "2017_AbyssalLeviathan.png", IconTextures = "2017_AbyssalLeviathan_Icon.png" } },
		{ 2018, new SkinDef { SkinTextures = "2018_NeonCircuit.png", IconTextures = "2018_NeonCircuit_Icon.png" } },
		{ 2019, new SkinDef { SkinTextures = "2019_CrimsonDragon.png", IconTextures = "2019_CrimsonDragon_Icon.png" } },
		{ 2020, new SkinDef { SkinTextures = "2020_Frostbound.png", SkinFlameModes = FlameMode.Helix, SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinFlameTints = new Color(0.13f, 0.28f, 0.50f, 0.5f), SkinSleeves = new SleeveSpec { RadiusMult = 2.0f, Ribbons = 3, RibbonArc = 1.5f, Twist = 1.4f, Start = 0.30f, MaxLenFrac = 0.048f, VRepeat = 2f }, SkinFlames = "2020_Frostbound_BlueFlames.png", IconTextures = "2020_Frostbound_Icon.png" } },
		{ 2021, new SkinDef { SkinTextures = "2020_Frostbound.png", SkinFlameModes = FlameMode.Surface, SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2021_Frostfire_Icon.png" } },
		{ 2022, new SkinDef { SkinTextures = "2022_Bloodglass.png", SkinFlameModes = FlameMode.Surface, SkinShaders = new string[] { "Transparent/Diffuse" }, SkinReflectTints = new Color(0.95f, 0.55f, 0.52f, 0.08f), SkinFlameTints = new Color(0.11f, 0.09f, 0.09f, 0.5f), SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2022_Bloodglass_Icon.png" } },
		{ 2023, new SkinDef { SkinTextures = "2023_Permafrost.png", SkinShellScale = 1.09f, SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinMaterialBindings = GlassIceBindings, SkinSleeves = new SleeveSpec { RadiusMult = 2.1f, Ribbons = 3, RibbonArc = 1.30f, Twist = 1.2f, Start = 0.55f, MaxLenFrac = 0.068f, VRepeat = 3f }, IconTextures = "2023_Permafrost_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f), ParticleObjects = new string[] { "Sfx", "Spark" }, TintRenderers = new string[] { "SplatterTrail" } } } },
		{ 2024, new SkinDef { SkinTextures = "2024_Icebreaker.png", SkinShellScale = 1.07f, SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinMaterialBindings = GlassIceBindings, SkinSleeves = new SleeveSpec { RadiusMult = 2.0f, Ribbons = 3, RibbonArc = 0.95f, Twist = 1.0f, Start = 0.38f, MaxLenFrac = 0.118f, VRepeat = 3f }, IconTextures = "2024_Icebreaker_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } } },
		{ 2025, new SkinDef { SkinTextures = "2025_MGWatery.png", SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = WaterBindings, IconTextures = "2025_MGWatery_Icon.png" } },
		{ 2026, new SkinDef { SkinTextures = "2026_SniperWatery.png", SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = WaterBindings, IconTextures = "2026_SniperWatery_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } } },
		{ 2027, new SkinDef { SkinTextures = "2027_ShotgunWatery.png", SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = WaterBindings, IconTextures = "2027_ShotgunWatery_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } } },
		{ 2028, new SkinDef { SkinTextures = "2028_CannonWatery.png", SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = WaterBindings, IconTextures = "2028_CannonWatery_Icon.png" } },
		{ 2029, new SkinDef { SkinTextures = "2029_MGFrosted.png", SkinFlameModes = FlameMode.Surface, SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2029_MGFrosted_Icon.png" } },
		{ 2030, new SkinDef { SkinTextures = "2030_SniperFrosted.png", SkinFlameModes = FlameMode.Surface, SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2030_SniperFrosted_Icon.png" } },
		{ 2031, new SkinDef { SkinTextures = "2031_ShotgunFrosted.png", SkinFlameModes = FlameMode.Surface, SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2031_ShotgunFrosted_Icon.png" } },
		{ 2032, new SkinDef { SkinTextures = "2032_CannonFrosted.png", SkinFlameModes = FlameMode.Surface, SkinFlames = "2020_Frostbound_Flames.png", IconTextures = "2032_CannonFrosted_Icon.png" } },
		{ 2033, new SkinDef { SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = LavaBindings, IconTextures = "2033_MGLava_Icon.png" } },
		{ 2034, new SkinDef { SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = LavaBindings, IconTextures = "2034_SniperLava_Icon.png" } },
		{ 2035, new SkinDef { SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = LavaBindings, IconTextures = "2035_ShotgunLava_Icon.png" } },
		{ 2036, new SkinDef { SkinShaders = new string[] { WaterShader }, SkinShaderResources = WaterShaderPath, SkinMaterialBindings = LavaBindings, IconTextures = "2036_CannonLava_Icon.png" } },
		{ 2037, new SkinDef { SkinTextures = "2037_NeonCircuitBlack.png", IconTextures = "2037_NeonCircuitBlack_Icon.png" } },
		{ 2038, new SkinDef { SkinTextures = "2023_Permafrost.png", SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinMaterialBindings = GlassMatteBindings, IconTextures = "2038_AWPMatteGlass_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f), TintRenderers = new string[] { "SplatterTrail" } } } },
		{ 2039, new SkinDef { SkinTextures = "2024_Icebreaker.png", SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, SkinMaterialBindings = GlassMatteBindings, IconTextures = "2039_IcebreakerMatteGlass_Icon.png" } },
		{ 2040, new SkinDef { SkinTextures = "2040_FrostSerpent.png", IconTextures = "2040_FrostSerpent_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f), ParticleObjects = new string[] { "Sfx", "Spark" }, TintRenderers = new string[] { "SplatterTrail" } } } },
		{ 2041, new SkinDef { SkinTextures = "2023_Permafrost.png", SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, IconTextures = "2041_AWPClearIce_Icon.png" } },
		{ 2042, new SkinDef { SkinTextures = "2024_Icebreaker.png", SkinShaders = new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" }, IconTextures = "2042_IcebreakerClearIce_Icon.png" } },
		{ 2043, new SkinDef { SkinTextures = "2043_M4A1Gold.png", IconTextures = "2043_M4A1Gold_Icon.png" } },
		{ 2044, new SkinDef { SkinTextures = "2044_AK47Gold.png", IconTextures = "2044_AK47Gold_Icon.png" } },
		{ 2045, new SkinDef { SkinTextures = "2045_SPAS12Gold.png", IconTextures = "2045_SPAS12Gold_Icon.png" } },
		{ 2046, new SkinDef { SkinTextures = "2046_AWPGold.png", IconTextures = "2046_AWPGold_Icon.png" } },
		{ 2047, new SkinDef { SkinTextures = "2047_M4A1Chrome.png", IconTextures = "2047_M4A1Chrome_Icon.png" } },
		{ 2048, new SkinDef { SkinTextures = "2048_AK47Chrome.png", IconTextures = "2048_AK47Chrome_Icon.png" } },
		{ 2049, new SkinDef { SkinTextures = "2049_SPAS12Chrome.png", IconTextures = "2049_SPAS12Chrome_Icon.png" } },
		{ 2050, new SkinDef { SkinTextures = "2050_AWPChrome.png", IconTextures = "2050_AWPChrome_Icon.png" } },
		{ 2051, new SkinDef { SkinTextures = "2051_M4A1Damascus.png", IconTextures = "2051_M4A1Damascus_Icon.png" } },
		{ 2052, new SkinDef { SkinTextures = "2052_AK47Damascus.png", IconTextures = "2052_AK47Damascus_Icon.png" } },
		{ 2053, new SkinDef { SkinTextures = "2053_SPAS12Damascus.png", IconTextures = "2053_SPAS12Damascus_Icon.png" } },
		{ 2054, new SkinDef { SkinTextures = "2054_AWPDamascus.png", IconTextures = "2054_AWPDamascus_Icon.png" } },
		{ 2055, new SkinDef { SkinTextures = "2055_M4A1Carbon.png", IconTextures = "2055_M4A1Carbon_Icon.png" } },
		{ 2056, new SkinDef { SkinTextures = "2056_AK47Carbon.png", IconTextures = "2056_AK47Carbon_Icon.png" } },
		{ 2057, new SkinDef { SkinTextures = "2057_SPAS12Carbon.png", IconTextures = "2057_SPAS12Carbon_Icon.png" } },
		{ 2058, new SkinDef { SkinTextures = "2058_AWPCarbon.png", IconTextures = "2058_AWPCarbon_Icon.png" } },
		{ 2059, new SkinDef { SkinTextures = "2059_M4A1Tempered.png", IconTextures = "2059_M4A1Tempered_Icon.png" } },
		{ 2060, new SkinDef { SkinTextures = "2060_AK47Tempered.png", IconTextures = "2060_AK47Tempered_Icon.png" } },
		{ 2061, new SkinDef { SkinTextures = "2061_SPAS12Tempered.png", IconTextures = "2061_SPAS12Tempered_Icon.png" } },
		{ 2062, new SkinDef { SkinTextures = "2062_AWPTempered.png", IconTextures = "2062_AWPTempered_Icon.png" } },
		{ 2063, new SkinDef { SkinTextures = "2063_M4A1ChromeMax.png", IconTextures = "2063_M4A1ChromeMax_Icon.png" } },
		{ 2064, new SkinDef { SkinTextures = "2064_AK47ChromeMax.png", IconTextures = "2064_AK47ChromeMax_Icon.png" } },
		{ 2065, new SkinDef { SkinTextures = "2065_SPAS12ChromeMax.png", IconTextures = "2065_SPAS12ChromeMax_Icon.png" } },
		{ 2066, new SkinDef { SkinTextures = "2066_AWPChromeMax.png", IconTextures = "2066_AWPChromeMax_Icon.png" } },
		{ 2067, new SkinDef { SkinTextures = "2067_AWPUberverse.png", IconTextures = "2067_AWPUberverse_Icon.png", MuzzleTints = new MuzzleTintSpec { HasLight = true, LightColour = new Color(0.20f, 0.85f, 1.00f, 1f), HasParticles = true, ParticleTint = new Color(0.30f, 1.30f, 1.60f, 1f), ParticleObjects = new string[] { "Sfx", "Spark" }, TintRenderers = new string[] { "SplatterTrail" } } } },
		{ 2068, new SkinDef { SkinTextures = "2068_CyberNeon.png", IconTextures = "2068_CyberNeon_Icon.png" } },
		{ 2069, new SkinDef { SkinTextures = "2069_ToxicVenom.png", IconTextures = "2069_ToxicVenom_Icon.png" } },
		{ 2070, new SkinDef { SkinTextures = "2070_MoltenInferno.png", IconTextures = "2070_MoltenInferno_Icon.png" } },
		{ 2071, new SkinDef { SkinTextures = "2067_AWPUberverse.png", IconTextures = "2071_AWPUberverseV1_Icon.png" } },
		{ 2072, new SkinDef { SkinTextures = "2067_AWPUberverse.png", IconTextures = "2072_AWPUberverseV12_Icon.png" } },
		{ 2073, new SkinDef { SkinTextures = "2073_WreckerVoidglass.png", IconTextures = "2073_WreckerVoidglass_Icon.png" } },
		{ 2074, new SkinDef { SkinTextures = "2074_SplattergunPrismSplatter.png", IconTextures = "2074_SplattergunPrismSplatter_Icon.png" } },
		{ 2075, new SkinDef { SkinTextures = "2075_LauncherDragonsMaw.png", IconTextures = "2075_LauncherDragonsMaw_Icon.png" } },
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
