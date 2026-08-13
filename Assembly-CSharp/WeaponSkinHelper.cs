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
// Item ID -> base weapon mapping (from unity_2022_tg/skin-framework commit 850893ee).
// Keep this list complete: it went stale once at 9012-9015 and again at 9018, which makes it
// look authoritative while being wrong.
//   9007 Plasma Bat        (base 1000 TheSplatbat)
//   9008 Inferno MG        (base 1002 MachineGun)
//   9009 Cryo Strike       (base 1004 PaintSniper)
//   9010 Solar Cannon      (base 1005 Cannon)
//   9011 Natural Shotgun   (base 1003 PaintShotty)   -- renamed from Toxic Splatter 2026-08-12
//   9012 Void Amethyst     (base 1004 PaintSniper)   -- also the only tracer override
//   9013 Bloodhound        (base 1003 PaintShotty)
//   9014 Abyssal Leviathan (base 1005 Cannon)
//   9015 Neon Circuit      (base 1002 MachineGun)
//   9016 Crimson Dragon    (base 6 MythicEdge-DE, premium melee)
//   9017 Frostbound        (base 6 MythicEdge-DE)    -- see-through ice, HELIX flames
//   9018 Frostfire         (base 6 MythicEdge-DE)    -- shares 9017's art, SURFACE flames
//   9019 Bloodglass        (base 6 MythicEdge-DE)    -- see-through red, SURFACE flames
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
		{ 9011, "9011_NaturalShotgun.png" },
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
		// 2026-08-12. Same glass sword as 9017, deliberately sharing its texture files rather
		// than duplicating them -- the two skins differ only in HOW the fire moves, so two
		// copies of the same art would only be two things to keep in sync. What differs is the
		// flame mode below, the catalog name, and the icon.
		{ 9018, "9017_Frostbound.png" },
		// 2026-08-13. Third skin on this katana and the second see-through one: red glass rather
		// than ice. It has its OWN art, unlike 9018, because the palette is the whole point.
		//
		// Getting red to work here took three generation passes, and the reason is worth keeping.
		// This base already carries a dragon etched down the blade and a diamond-weave grip, and
		// 9016 Crimson Dragon is already the red katana on it, so colour alone could never
		// separate the two. What separates this one is that it is TRANSPARENT: the dragon reads
		// as a denser form suspended INSIDE the glass rather than as a glowing inlay, which is
		// 9016's language.
		//
		// The shipped art needed a luminance-only correction (gamma 0.50 + 0.04 lift, hue and
		// saturation untouched). Every red generation came back underexposed -- around brightness
		// 38 against the 55-80 target -- while the same prompt asking for ice came back correctly
		// exposed. A plain RGB gamma fixes the exposure but washes the red out to R/B 1.70, under
		// the 2.0 gate; lifting only the value channel holds R/B at 2.52, just above 9016's 2.57.
		{ 9019, "9019_Bloodglass.png" },
		// 2026-08-13. First see-through skins on a FIREARM and on a blunt melee, and the first
		// whose transparency is NOT authored from a gloss mask -- neither base has one, their
		// alpha is fully opaque (measured). So the translucency comes from painted BRIGHTNESS
		// instead: the art was briefed to keep held parts dark (<80) and glass parts bright
		// (>140), and the ramp sits across that measured gap. AWP came back 35% dark / 31%
		// bright, the hammer 30% / 39%, both with a thin middle -- separable.
		{ 9020, "9020_Permafrost.png" },
		{ 9021, "9021_Icebreaker.png" },
		// 2026-08-13. The glacier set: the first skins in this file that are GENERATED rather
		// than painted. tools/make_glacier_skin.py transforms each base pixel-wise -- luminance
		// through an ice ramp, procedural fractures and frost scaled by a glass weight, edge
		// light from the base's own gradient.
		//
		// Chosen over generation because an ice treatment is a recolour, not an invention: the
		// panel seams, ribs and knurling must stay exactly where they are, and that is what a
		// generator cannot be told. It also makes the failures we kept hitting impossible --
		// UV islands cannot move and coverage cannot drop, because every output pixel comes
		// from the input pixel at the same coordinate.
		//
		// Bases identified by correlating against the skins that already render correctly, not
		// by filename: MachineGun_DM 0.662, Sniper-diffuse 0.325, Shotgun-diffuse 0.321,
		// Cannon-diffuse 0.295, each well clear of its runner-up. Picking a base by name is
		// what left 9015 with ~37% of its mesh unpainted.
		{ 9022, "9022_MGWatery.png" },
		{ 9023, "9023_SniperWatery.png" },
		{ 9024, "9024_ShotgunWatery.png" },
		{ 9025, "9025_CannonWatery.png" },
		// The same generator, second palette. Frost SCATTERS light where water TRANSMITS it,
		// so the two ramps differ in more than hue: water holds its colour as it brightens
		// (B/R 2.0-2.8), frost climbs to a neutral near-white (B/R 1.2-1.4) and keeps its
		// cyan bias almost off, because a blue cast on a white ramp reads as plastic.
		// Frost also gets far less liquid smoothing -- 0.35 against 0.70 -- since snow is a
		// granular surface and should not flow.
		{ 9026, "9026_MGFrosted.png" },
		{ 9027, "9027_SniperFrosted.png" },
		{ 9028, "9028_ShotgunFrosted.png" },
		{ 9029, "9029_CannonFrosted.png" },
	};

	/// <summary>
	/// How a skin's flame overlay is built.
	///
	/// Surface was the original concept: a copy of the weapon's own mesh drawn additively over
	/// it, so the fire sits ON the steel. Helix builds a separate sleeve of ribbons standing
	/// off the blade and winding along it, so the fire orbits OUTSIDE the sword.
	/// </summary>
	public enum FlameMode { Surface, Helix }

	public static readonly Dictionary<int, FlameMode> SkinFlameModes = new Dictionary<int, FlameMode>
	{
		{ 9017, FlameMode.Helix },
		{ 9018, FlameMode.Surface },
		// Surface, matching 9018 rather than 9017: the brief was flames INSIDE the body, and the
		// helix sleeve stands the fire off the blade instead.
		{ 9019, FlameMode.Surface },
		// Surface: fire on the weapon itself, matching 9018 Frostfire.
		//
		// This crashed the client when it first shipped, inside WhiteVertexCopy's
		// Object.Instantiate of the weapon mesh. That call is gone -- the overlay mesh is now
		// rebuilt by hand from the source arrays, so a mesh the CPU cannot read raises a
		// catchable managed error and costs the weapon its flames rather than the session.
		// 9020/9021 removed: they carry no flame sheet, so the mode is unused.
		{ 9022, FlameMode.Surface },
		{ 9023, FlameMode.Surface },
		{ 9024, FlameMode.Surface },
		{ 9025, FlameMode.Surface },
		{ 9026, FlameMode.Surface },
		{ 9027, FlameMode.Surface },
		{ 9028, FlameMode.Surface },
		{ 9029, FlameMode.Surface },
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
		{ 9018, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		// 9019 deliberately SKIPS Glass-Hangar and takes Transparent/Diffuse directly.
		//
		// Glass-Hangar binds fine here -- the client logs it -- but it adds a cubemap reflection
		// whose _Cube we never assign, so it samples WHITE. On the ice skins that wash is
		// invisible or even flattering; on red it turns the blade pale grey-pink. Measured
		// against the shipped texture, which is unambiguously red (mean 109/15/18, blade
		// highlights 225/59/61 at R/B 3.69) and still rendered white in game.
		//
		// Transparent/Diffuse has no reflection term at all: albedo is the texture, alpha is
		// the texture's alpha. It costs the faint cubemap sparkle and keeps the colour, which
		// is the right trade for a skin whose entire identity is that it is red.
		{ 9019, new string[] { "Transparent/Diffuse" } },
		{ 9020, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		{ 9021, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		// 9022-9025 (the glacier set) are deliberately ABSENT: they keep the stock opaque
		// shader. Shipped once with Glass-Hangar and every one of them rendered HOLLOW --
		// bright ice edges visible and the body see-through to the wall behind. Alpha-blended
		// geometry does not write depth, so overlapping faces of one mesh sort arbitrarily.
		// A katana blade is a single thin shape and survives that; a machine gun is dozens of
		// overlapping parts and does not. Their alpha carries GLOSS for Bumped Specular
		// instead, which is what gives frozen water its hard wet highlight.
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
	/// <summary>
	/// Per-skin tint for Glass-Hangar's cubemap reflection term. Absent = the icy default.
	///
	/// Exists because 9019 Bloodglass rendered BLUE in game despite a red texture: the single
	/// hardcoded cool tint was laying a blue cast over every see-through skin, which is
	/// invisible on the ice ones and fatal on a red one.
	/// </summary>
	public static readonly Dictionary<int, Color> SkinReflectTints = new Dictionary<int, Color>
	{
		// Warm, so the reflection term reinforces the red instead of fighting it.
		{ 9019, new Color(0.95f, 0.55f, 0.52f, 0.08f) },
	};

	/// <summary>
	/// Per-skin flame tint, overriding the per-MODE default. Absent = the mode default.
	///
	/// Particles/Additive computes 2 * vertexColour * tint * texture, so the peak add is twice
	/// these numbers.
	/// </summary>
	public static readonly Dictionary<int, Color> SkinFlameTints = new Dictionary<int, Color>
	{
		// Near-neutral white with a slight warm bias: the brief for this skin is WHITE flames
		// over red glass, so it must not be tinted red (they would vanish into the blade) and
		// must not keep the blue-dominant Surface default (which is what made it look blue).
		// Peak add 0.56 / 0.48 / 0.48.
		// Peak add 0.22 / 0.18 / 0.18 -- deliberately much dimmer than the ice default.
		// First attempt used 0.28/0.24/0.24 and the blade came back WHITE: Surface mode
		// paints the whole weapon, so an additive wash of ~0.5 per channel over a blade
		// sitting at brightness 69 buries the red entirely. The flames still read as white
		// fire because they are white in the SHEET; the tint only sets how hard they burn.
		{ 9019, new Color(0.11f, 0.09f, 0.09f, 0.5f) },
	};

	/// <summary>
	/// Per-skin overrides for the flame sleeve's geometry.
	///
	/// Exists for the weapons whose meshes are NOT CPU-readable -- the AWP and the Death
	/// Hammer. Surface mode copies the weapon's own geometry, and on those two `.vertices`
	/// throws, so it is impossible there by any route rather than merely broken. The sleeve
	/// is built from the bounding box, which IS readable, so it is the only way to put fire
	/// on those weapons at all.
	///
	/// The defaults orbit a katana at 2x its half-thickness with two narrow strands. Pulled
	/// in tight with more, wider strands, the same geometry stops reading as fire circling
	/// the weapon and starts reading as fire clinging to it -- which is what Surface mode
	/// gives on the weapons that can support it.
	/// </summary>
	public struct SleeveSpec
	{
		public float RadiusMult;   // multiple of the mesh's half-thickness
		public int Ribbons;        // how many strands around the circumference
		public float RibbonArc;    // radians of arc each strand covers
		public float Twist;        // turns along the weapon's length
		public float Start;        // 0 = butt, 1 = tip: where the sleeve begins
		public float MaxLenFrac;   // radius ceiling as a fraction of length
		public float VRepeat;      // how many times the flame sheet tiles ALONG the sleeve
	}

	public static readonly Dictionary<int, SleeveSpec> SkinSleeves = new Dictionary<int, SleeveSpec>
	{
		// Hugging: radius under 1x half-thickness so the strands sit ON the barrel, five of
		// them so they wrap it, a wide arc so each is a sheet rather than a thread, and a low
		// twist so they run ALONG the weapon instead of spiralling round it.
		// (no entries: the two skins this was built for ship without flames -- see the
		// note in SkinFlames. Defaults below apply to anything that does use a sleeve.)
	};

	public static readonly Dictionary<int, string> SkinFlames = new Dictionary<int, string>
	{
		{ 9017, "9017_Frostbound_Flames.png" },
		{ 9018, "9017_Frostbound_Flames.png" },
		// Shares the flame sheet with the ice skins on purpose: it is generic white fire on pure
		// black that tiles vertically, and white fire over red glass is the contrast that sells
		// this skin. Red flames on a red blade would mostly disappear.
		{ 9019, "9017_Frostbound_Flames.png" },
		// 9020 and 9021 have NO flames, deliberately, and this is where the attempt stopped
		// rather than where it succeeded.
		//
		// Surface is impossible on them: their meshes are not CPU-readable, so the overlay
		// cannot copy the weapon's geometry -- the client logs "cannot be copied on the CPU"
		// for AWP and Death_Hammer. The fallback is a sleeve built from the bounding box,
		// which does work, but two shapes of it were tried in game and both looked worse than
		// no fire at all: five narrow strands read as combed fibre, and two broad ones tiled
		// 9x were no better. The skins are good without them, so they ship clean.
		//
		// The SleeveSpec machinery below is left in place for whoever picks this up: it makes
		// the sleeve's radius, strand count, arc, twist and sheet tiling per-skin, which is
		// the vocabulary needed to tune this properly rather than by guessing.
		{ 9022, "9017_Frostbound_Flames.png" },
		{ 9023, "9017_Frostbound_Flames.png" },
		{ 9024, "9017_Frostbound_Flames.png" },
		{ 9025, "9017_Frostbound_Flames.png" },
		{ 9026, "9017_Frostbound_Flames.png" },
		{ 9027, "9017_Frostbound_Flames.png" },
		{ 9028, "9017_Frostbound_Flames.png" },
		{ 9029, "9017_Frostbound_Flames.png" },
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
		{ 9011, "9011_NaturalShotgun_Icon.png" },
		{ 9012, "9012_VoidAmethyst_Icon.png" },
		{ 9013, "9013_Bloodhound_Icon.png" },
		{ 9014, "9014_AbyssalLeviathan_Icon.png" },
		{ 9015, "9015_NeonCircuit_Icon.png" },
		{ 9016, "9016_CrimsonDragon_Icon.png" },
		{ 9017, "9017_Frostbound_Icon.png" },
		{ 9018, "9018_Frostfire_Icon.png" },
		{ 9019, "9019_Bloodglass_Icon.png" },
		// Rendered once the mesh reader learned to decode COMPRESSED meshes. The AWP and
		// Death Hammer store their geometry quantised under m_CompressedMesh with an empty
		// _typelessdata, unlike every weapon the icon pipeline had handled before, so these
		// two shipped with stock icons until that decoder existed.
		{ 9020, "9020_Permafrost_Icon.png" },
		{ 9021, "9021_Icebreaker_Icon.png" },
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
			if (r == null)
				continue;

			// Skip effect renderers. A weapon's children include its muzzle flash and shell
			// casing, which use "Particle Add" and friends, and painting the gun's diffuse
			// onto an additive quad makes the flash render as a bright rectangle showing a
			// slab of the UV atlas.
			//
			// This stayed invisible for a long time by luck: the earlier skins carried a
			// specular mask that is 82-84% near-zero alpha, so the additive quad multiplied
			// out to nothing. The 4.7.1 MachineGun base's mask is 74.6% MID-range, so the
			// same bug finally showed up as a visible square. Measured, not guessed.
			//
			// INSPECT VIA sharedMaterial, NOT material. This is the whole reason the flash
			// used to vanish rather than fall back to stock.
			//
			// Renderer.material is not a getter: the first access INSTANTIATES a private copy
			// of the shared material and rebinds this renderer to it. The old code read
			// `r.material == null` and `r.material.shader` before deciding to skip, so every
			// effect renderer on the weapon got a material instance forced onto it even though
			// we then skipped it. That detaches the quad from the shared material the game's
			// own effect code drives, and the flash renders as NOTHING.
			//
			// That is exactly the "renders as nothing rather than as the stock flash, which is
			// more than skipping alone should do" note that sat here unexplained. Reading
			// sharedMaterial inspects without instantiating, so a skipped renderer is left
			// genuinely untouched and keeps its stock behaviour.
			Material shared = r.sharedMaterial;
			if (shared == null)
				continue;
			Shader sh = shared.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;

			// Past this point the renderer IS being skinned, so instantiating its material
			// is intended -- and keeps the change per-instance, off the shared asset.
			if (r.material == null)
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

			// Glass-Hangar adds a cubemap reflection on top of the albedo:
			//
			//     reflcol  = texCUBE(_Cube, worldRefl) * _ReflectColor
			//     o.Albedo = c.rgb + reflcol.rgb * reflcol.a
			//
			// We only swap the shader, so _Cube is never assigned and samples WHITE, while
			// _ReflectColor defaults to (1,1,1,0.5). That adds a flat +0.5 white to every
			// pixel and washes the blade out to a featureless pale slab -- which is exactly
			// what it did in game, with all the ice detail gone.
			//
			// With no cubemap to reflect there is nothing meaningful for this term to say, so
			// it is turned down to a faint cool tint instead of being left at its default.
			// PER SKIN, not shared. This started as one hardcoded icy value because every
			// see-through skin was blue. 9019 Bloodglass is red, and inheriting a cool tint
			// laid a blue cast over the whole blade -- in game it read as a BLUE sword, which
			// is the one thing that skin must not be.
			if (r.material.HasProperty("_ReflectColor"))
			{
				Color reflect;
				if (!SkinReflectTints.TryGetValue(itemId, out reflect))
					reflect = new Color(0.55f, 0.75f, 0.95f, 0.08f); // icy default
				r.material.SetColor("_ReflectColor", reflect);
			}

			Debug.Log("WeaponSkinHelper: skin " + itemId + " bound shader '" + candidates[i] + "'"
				+ (i > 0 ? " (fell back; '" + candidates[0] + "' is not in this build)" : ""));
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

			// INHERIT THE LAYER. `new GameObject` always starts on layer 0 (Default) -- it
			// does not take its parent's layer, and nothing later fixes it, because
			// Avatar.AssignWeapon does its SetLayerRecursively FIVE LINES BEFORE it calls
			// into this file (Avatar.cs:173 vs :178). WeaponSlot.cs:187 is the same shape.
			//
			// That one missing line is what produced every "glitch" in testing. The
			// first-person weapon lives on a layer only the weapon camera draws, so an
			// overlay left on Default is picked up by the MAIN world camera instead: a blade
			// hanging in the world at the first-person weapon's position, which sits right in
			// front of the camera and therefore renders enormous. It read as a second sword,
			// as a giant translucent slab, and as wrong positioning -- and it survived fixes
			// to scale, tint and vertex colours because none of them were the cause.
			go.layer = mf.gameObject.layer;

			// The overlay is a SLEEVE around the blade, not a copy of the blade. Copying the
			// weapon mesh paints fire onto the surface; a sleeve is separate geometry standing
			// off the steel, so the flames can orbit the sword in the air around it.
			FlameMode mode;
			if (!SkinFlameModes.TryGetValue(itemId, out mode))
				mode = FlameMode.Helix;

			Vector3 axis = Vector3.up, centre = Vector3.zero;
			Mesh overlayMesh;
			if (mode == FlameMode.Surface)
			{
				// the original concept: fire painted onto the weapon's own geometry
				overlayMesh = WhiteVertexCopy(mf.sharedMesh);
			}
			else
			{
				overlayMesh = BuildFlameSleeve(mf.sharedMesh, itemId, out axis, out centre);
			}
			if (overlayMesh == null)
				continue;

			go.transform.parent = mf.transform;
			// Vertices are authored in the weapon's own local space now that the sleeve bends
			// with the blade, so the child sits at the origin and never needs moving.
			go.transform.localPosition = centre;   // Vector3.zero from BuildFlameSleeve
			go.transform.localRotation = Quaternion.identity;
			// Scale stays at ONE. An earlier version used 1.015 "so it never z-fights", which
			// was wrong twice over: Particles/Additive already has ZWrite Off so there is no
			// depth fight to lose, and scaling happens about this transform's PIVOT, not the
			// mesh centroid. The katana's geometry sits well off its pivot, so 1.5% became a
			// visible translation and the overlay read as a second, ghostly blade beside the
			// real one.
			go.transform.localScale = Vector3.one;

			MeshFilter of = go.AddComponent<MeshFilter>();
			of.sharedMesh = overlayMesh;

			MeshRenderer or = go.AddComponent<MeshRenderer>();
			Material m = new Material(additive);
			m.mainTexture = flame;
			if (m.HasProperty("_TintColor"))
			{
				// Particles/Additive computes 2.0 * vertexColour * _TintColor * texture, and a
				// MeshRenderer has no vertex colours so that term is white. The factor of TWO
				// is the part worth remembering: a tint of 0.42/0.62/0.78 is not "60% strength",
				// it peaks at 0.84/1.24/1.56 and clips -- brighter than the blade underneath.
				// Particles/Additive computes 2 * vertexColour * tint * texture, so the peak
				// add is twice these numbers. Deliberately split by MODE rather than shared:
				// the helix is two narrow ribbons covering very little of the frame, so it can
				// run hot and read as white-hot fire, while Surface paints the whole weapon and
				// the same value there would wash the ice out to a flat glare.
				//
				// Also PER SKIN. Both defaults below are blue-dominant because they were tuned
				// on the ice skins, and Surface mode paints the WHOLE weapon -- so on a red
				// blade that wash is a second blue cast on top of the reflection one.
				Color tint;
				if (!SkinFlameTints.TryGetValue(itemId, out tint))
					tint = mode == FlameMode.Surface
						? new Color(0.18f, 0.26f, 0.32f, 0.5f)   // peak add 0.36 / 0.52 / 0.64
						: new Color(0.40f, 0.47f, 0.52f, 0.5f);  // peak add 0.80 / 0.94 / 1.04
				m.SetColor("_TintColor", tint);
			}
			// Tile the sheet ALONG the blade. The overlay samples with the weapon's own UVs,
			// where the blade is one long thin island, so at 1x tiling a single flame tongue
			// is stretched over the entire length and reads as a wash rather than as fire.
			// Repeating it down the island gives distinct tongues travelling up the blade.
			// On the sleeve, U runs AROUND the circumference and V runs ALONG the blade, so
			// these two numbers mean something different than they did on the mesh copy:
			// 2 flame columns around the sword, repeating 3 times down its length.
			float vrep = 2f;
			SleeveSpec tsp;
			if (SkinSleeves.TryGetValue(itemId, out tsp) && tsp.VRepeat > 0f)
				vrep = tsp.VRepeat;
			m.SetTextureScale("_MainTex", mode == FlameMode.Surface
				? new Vector2(1f, 4f)      // across the weapon UVs, as the original did
				: new Vector2(1f, vrep));  // one band per ribbon, tiled along it
			m.renderQueue = 3100;               // after the glass at 3000
			or.material = m;
			or.castShadows = false;
			or.receiveShadows = false;

			WeaponFlameAnimator anim = go.AddComponent<WeaponFlameAnimator>();
			anim.SpinAxis = axis;
		}
	}

	private const string FlameChildName = "__SkinFlameOverlay";

	private static readonly Dictionary<Mesh, Mesh> _flameMeshCache = new Dictionary<Mesh, Mesh>();
	private static readonly Dictionary<Mesh, Mesh> _sleeveCache = new Dictionary<Mesh, Mesh>();
	private static readonly Dictionary<Mesh, Vector3> _sleeveAxis = new Dictionary<Mesh, Vector3>();
	private static readonly Dictionary<Mesh, Vector3> _sleeveCentre = new Dictionary<Mesh, Vector3>();

	// Sleeve shape. Rings along the blade, segments around it.
	private const int SLEEVE_RINGS = 22;
	private const int SLEEVE_RIBBONS = 2;          // two flame strands
	private const int SLEEVE_ARC_SEGMENTS = 5;     // quads across one strand
	private const float SLEEVE_RIBBON_ARC = 0.5f;  // radians of arc each strand covers
	// How far out the flames stand off the steel, as a multiple of the blade's own half
	// THICKNESS -- the thinner cross-axis, not the thicker one. A katana is curved, so its
	// bounding box in the curve plane measures the bend (0.157 on this mesh), not the steel.
	// Sizing off that gave a sleeve 61% as wide as it was long: a spinning umbrella.
	private const float SLEEVE_RADIUS_MULT = 2.0f;
	// Belt and braces on the above: whatever the cross-section says, keep the envelope inside
	// a sane fraction of the blade's LENGTH, which is the measurement that cannot be fooled
	// by curvature or by a stray vertex.
	private const float SLEEVE_MIN_LEN_FRAC = 0.025f;
	private const float SLEEVE_MAX_LEN_FRAC = 0.042f;
	// How much the envelope narrows at the TIP only. The reference art is a flame helix that
	// hugs the blade for its whole length and closes at the point -- not an hourglass. An
	// earlier version pinched the waist and flared both ends, which read as a bowtie and
	// pushed fire out past the tip.
	private const float SLEEVE_TIP_SCALE = 0.55f;
	// Turns of twist from guard to tip. This is what makes it a HELIX rather than a tube with
	// a pattern on it, so it is the single most important number for matching the reference.
	private const float SLEEVE_TWIST_TURNS = 2.1f;
	// Where the sleeve starts along the weapon, as a fraction from butt to tip. The katana's
	// grip is roughly the first third and a player's hand is there, so fire wrapping it looks
	// wrong; the flames begin above the guard.
	private const float SLEEVE_START = 0.30f;

	/// <summary>
	/// Build a cylindrical shell standing off the blade, for flames that ORBIT the sword
	/// rather than being painted on it.
	///
	/// Everything is derived from the mesh's own bounds rather than hardcoded, so this works
	/// on any weapon we later point it at:
	///
	///   * the blade axis is simply the LONGEST of the three bounds extents
	///   * the radius comes from the other two, so a thick weapon gets a wider sleeve
	///   * the grip end is the one nearer the weapon's local origin, because
	///     Avatar.AssignWeapon parents the weapon to the attach point and then zeroes its
	///     local position -- so the hand sits at approximately zero and the blade extends away
	///
	/// UVs are laid out U-around, V-along, which is what lets the flame sheet's vertical
	/// tongues run down the length of the blade while the mesh spins about it.
	/// </summary>
	private static Mesh BuildFlameSleeve(Mesh source, int itemId, out Vector3 axis, out Vector3 centre)
	{
		axis = Vector3.up;
		centre = Vector3.zero;
		if (source == null)
			return null;

		Mesh cached;
		if (_sleeveCache.TryGetValue(source, out cached) && cached != null)
		{
			axis = _sleeveAxis[source];
			centre = _sleeveCentre[source];
			return cached;
		}

		Bounds b = source.bounds;
		Vector3 size = b.size;

		int ai = 0;
		if (size.y > size.x) ai = 1;
		if (size.z > size[ai]) ai = 2;
		axis = ai == 0 ? Vector3.right : (ai == 1 ? Vector3.up : Vector3.forward);

		// the two axes perpendicular to the blade
		Vector3 pu = ai == 0 ? Vector3.up : Vector3.right;
		Vector3 pv = ai == 2 ? Vector3.up : Vector3.forward;

		float halfLen = size[ai] * 0.5f;
		if (halfLen <= 1e-5f)
			return null;

		// per-skin geometry, falling back to the katana-tuned defaults
		SleeveSpec sp;
		if (!SkinSleeves.TryGetValue(itemId, out sp))
		{
			sp.RadiusMult = SLEEVE_RADIUS_MULT; sp.Ribbons = SLEEVE_RIBBONS;
			sp.RibbonArc = SLEEVE_RIBBON_ARC;   sp.Twist = SLEEVE_TWIST_TURNS;
			sp.Start = SLEEVE_START;            sp.MaxLenFrac = SLEEVE_MAX_LEN_FRAC;
			sp.VRepeat = 2f;                    // the katana's original tiling
		}

		// MIN, not max: on a curved blade the wider cross-axis is the bend, not the steel.
		float thin = Mathf.Min(size[(ai + 1) % 3], size[(ai + 2) % 3]) * 0.5f;
		float len = halfLen * 2f;
		float radius = Mathf.Clamp(thin * sp.RadiusMult,
			len * SLEEVE_MIN_LEN_FRAC, len * sp.MaxLenFrac);

		// Which way the blade points from the hand.
		float sign = b.center[ai] >= 0f ? 1f : -1f;
		float butt = b.center[ai] - sign * halfLen;
		float tip = b.center[ai] + sign * halfLen;
		float start = Mathf.Lerp(butt, tip, sp.Start);
		float end = tip;

		Vector3 b_centre = b.center;
		centre = b.center;
		centre[ai] = (start + end) * 0.5f;

		// FOLLOW THE BLADE'S CURVE. A single centre cannot work here, and that is measured:
		// across the sleeve's span this katana sweeps 0.126 in X as it rises, against a flame
		// radius of 0.045. So even a perfect average leaves the sleeve nearly three flame-widths
		// off the steel at the ends -- strands running parallel to the blade but beside it,
		// which is exactly what centring on the bounding box, and then on the vertex mean,
		// both produced.
		//
		// Instead bin the vertices by height and take each slice's own centre, giving a
		// centreline that bends with the blade. Falls back to a straight line down the bounding
		// box if the mesh is not readable.
		Vector3[] ring = new Vector3[SLEEVE_RINGS];
		{
			Vector3[] sv = null;
			try { sv = source.vertices; } catch { sv = null; }
			double[] su = new double[SLEEVE_RINGS];
			double[] sw = new double[SLEEVE_RINGS];
			int[] cnt = new int[SLEEVE_RINGS];
			float lo = Mathf.Min(start, end), hi = Mathf.Max(start, end);
			if (sv != null && sv.Length > 0 && hi > lo)
			{
				for (int q = 0; q < sv.Length; q++)
				{
					float p = sv[q][ai];
					if (p < lo || p > hi)
						continue;
					int bin = Mathf.Clamp((int)((p - lo) / (hi - lo) * (SLEEVE_RINGS - 1) + 0.5f),
						0, SLEEVE_RINGS - 1);
					su[bin] += Vector3.Dot(sv[q], pu);
					sw[bin] += Vector3.Dot(sv[q], pv);
					cnt[bin]++;
				}
			}
			// fill each ring, carrying the last known slice through any empty bin
			float lastU = Vector3.Dot(b_centre, pu), lastV = Vector3.Dot(b_centre, pv);
			for (int r = 0; r < SLEEVE_RINGS; r++)
			{
				if (cnt[r] > 2)
				{
					lastU = (float)(su[r] / cnt[r]);
					lastV = (float)(sw[r] / cnt[r]);
				}
				ring[r] = pu * lastU + pv * lastV;
			}
			// one smoothing pass, so a thin slice cannot kink the centreline
			Vector3[] sm = new Vector3[SLEEVE_RINGS];
			for (int r = 0; r < SLEEVE_RINGS; r++)
			{
				Vector3 acc = ring[r] * 2f;
				float wsum = 2f;
				if (r > 0) { acc += ring[r - 1]; wsum += 1f; }
				if (r < SLEEVE_RINGS - 1) { acc += ring[r + 1]; wsum += 1f; }
				sm[r] = acc / wsum;
			}
			ring = sm;
		}

		// The sleeve now bends, so it can no longer be spun as a rigid body about the axis --
		// that would swing the curve away from the blade. Motion comes from the texture
		// travelling ALONG the helix instead, which reads as flame winding around the sword.
		centre = Vector3.zero;

		int across = SLEEVE_ARC_SEGMENTS + 1;
		int perRibbon = SLEEVE_RINGS * across;
		int nv = perRibbon * sp.Ribbons;
		Vector3[] verts = new Vector3[nv];
		Vector2[] uvs = new Vector2[nv];
		Color[] cols = new Color[nv];

		for (int rib = 0; rib < sp.Ribbons; rib++)
		{
			float phase = (float)rib / sp.Ribbons * Mathf.PI * 2f;
			for (int r = 0; r < SLEEVE_RINGS; r++)
			{
				float t = (float)r / (SLEEVE_RINGS - 1);
				float along = Mathf.Lerp(start, end, t);
				float profile = Mathf.Lerp(1f, SLEEVE_TIP_SCALE, Mathf.Pow(t, 2.5f));
				float twist = sp.Twist * Mathf.PI * 2f * t + phase;
				for (int s = 0; s < across; s++)
				{
					float w = (float)s / SLEEVE_ARC_SEGMENTS - 0.5f;
					float ang = twist + w * sp.RibbonArc;
					int idx = rib * perRibbon + r * across + s;
					verts[idx] = axis * along + ring[r]
						+ pu * (Mathf.Cos(ang) * radius * profile)
						+ pv * (Mathf.Sin(ang) * radius * profile);
					uvs[idx] = new Vector2((float)s / SLEEVE_ARC_SEGMENTS, t);
					cols[idx] = Color.white;
				}
			}
		}

		int[] tris = new int[sp.Ribbons * (SLEEVE_RINGS - 1) * SLEEVE_ARC_SEGMENTS * 6];
		int k = 0;
		for (int rib = 0; rib < sp.Ribbons; rib++)
		{
			int b0 = rib * perRibbon;
			for (int r = 0; r < SLEEVE_RINGS - 1; r++)
			{
				for (int s = 0; s < SLEEVE_ARC_SEGMENTS; s++)
				{
					int i0 = b0 + r * across + s;
					int i1 = i0 + 1;
					int i2 = i0 + across;
					int i3 = i2 + 1;
					tris[k++] = i0; tris[k++] = i2; tris[k++] = i1;
					tris[k++] = i1; tris[k++] = i2; tris[k++] = i3;
				}
			}
		}

		Mesh mesh = new Mesh();
		mesh.name = source.name + "__flameSleeve";
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.colors = cols;
		mesh.triangles = tris;
		mesh.RecalculateBounds();

		_sleeveCache[source] = mesh;
		_sleeveAxis[source] = axis;
		_sleeveCentre[source] = centre;

		Debug.Log("WeaponSkinHelper: flame sleeve for " + source.name
			+ " axis=" + axis + " radius=" + radius.ToString("F3")
			+ " (thin half=" + thin.ToString("F3") + ", len=" + len.ToString("F3") + ")"
			+ " span=" + (end - start).ToString("F3")
			+ " curve=" + (ring[SLEEVE_RINGS - 1] - ring[0]).magnitude.ToString("F3"));
		return mesh;
	}

	/// <summary>
	/// A copy of the mesh with every vertex colour set to white.
	///
	/// This is what makes the flame overlay work at all. "Particles/Additive" is written for
	/// particle systems, which always supply vertex colours, and its fragment is
	///
	///     2.0 * i.color * _TintColor * tex        with  o.color = v.color
	///
	/// A weapon mesh has no colour channel, so v.color is UNDEFINED -- and the tint is being
	/// multiplied by whatever garbage happens to be in that register. That is why the first
	/// in-game test blew out to a white smear, and why turning the tint down afterwards
	/// changed nothing: the tint was never the term in control.
	///
	/// The copy matters as much as the colours. mf.sharedMesh is shared with the base weapon
	/// and with every other player holding one, so writing colours into it would corrupt the
	/// stock Mythic Edge for the whole session. Cached per source mesh so a respawn does not
	/// allocate a new copy every time.
	/// </summary>
	/// <summary>
	/// Can this mesh's geometry be read back on the CPU? Reading .vertices raises a managed
	/// error for a mesh that is not CPU-readable, which is recoverable; Instantiate on the same
	/// mesh is a native crash, which is not. So probe with the safe call before the unsafe one.
	/// </summary>
	private static bool CanCopyMesh(Mesh m)
	{
		try
		{
			if (m.vertexCount <= 0)
				return false;
			Vector3[] v = m.vertices;
			return v != null && v.Length > 0;
		}
		catch (Exception e)
		{
			Debug.LogWarning("WeaponSkinHelper: mesh '" + m.name + "' is not CPU-readable: " + e.Message);
			return false;
		}
	}

	private static Mesh WhiteVertexCopy(Mesh source)
	{
		if (source == null)
			return null;

		Mesh cached;
		if (_flameMeshCache.TryGetValue(source, out cached) && cached != null)
			return cached;

		// HARD CRASH GUARD. Object.Instantiate on a mesh whose vertex data the CPU cannot read
		// takes the whole client down with a native access violation -- not a managed exception,
		// so nothing downstream can catch it. It killed the client on equipping the AWP and
		// Death Hammer skins while the katana copied fine.
		//
		// Touching .vertices first turns that into a catchable managed error, so an unreadable
		// mesh costs the weapon its flames instead of costing the player their session. A skin
		// with no flames is a disappointment; a skin that crashes on equip is a broken build.
		if (!CanCopyMesh(source))
		{
			Debug.LogWarning("WeaponSkinHelper: mesh '" + source.name + "' cannot be copied on "
				+ "the CPU, so no flame overlay for it. The skin itself is unaffected.");
			_flameMeshCache[source] = null;
			return null;
		}

		// BUILT BY HAND, not Instantiated.
		//
		// Object.Instantiate(mesh) is the call that crashed: on the AWP and Death Hammer it
		// took the client down with a native access violation, which no managed catch can
		// trap. Reconstructing the mesh from its own arrays does exactly the same job, and
		// every read here is a managed call that either succeeds or throws something
		// catchable -- so the worst case is a weapon without flames, never a dead session.
		//
		// The overlay only needs geometry, UVs and white vertex colours: Particles/Additive
		// computes 2 * vertexColour * _TintColor * texture and does not light the surface, so
		// normals and tangents are dead weight and are deliberately not copied.
		Mesh copy;
		try
		{
			copy = new Mesh();
			copy.name = source.name + "__flameOverlay";
			copy.vertices = source.vertices;
			copy.triangles = source.triangles;
			Vector2[] uv = source.uv;
			if (uv != null && uv.Length == copy.vertexCount)
				copy.uv = uv;
			Color[] colours = new Color[copy.vertexCount];
			for (int i = 0; i < colours.Length; i++)
				colours[i] = Color.white;
			copy.colors = colours;
			copy.RecalculateBounds();
		}
		catch (Exception e)
		{
			Debug.LogWarning("WeaponSkinHelper: could not rebuild '" + source.name
				+ "' for the flame overlay (" + e.Message + "); skipping flames for it.");
			_flameMeshCache[source] = null;
			return null;
		}

		_flameMeshCache[source] = copy;
		return copy;
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
	public float Speed = 0.17f;          // sheet heights per second, along the blade
	public float SwaySpeed = 0.13f;      // slight drift so it does not look rigid
	public float SwayAmount = 0.015f;

	/// <summary>
	/// Axis the sleeve spins about, in the parent's local space -- the blade's long axis, as
	/// measured from the mesh bounds. Set by ApplyFlames; without it the flames would tumble
	/// about an arbitrary axis instead of orbiting the sword.
	/// </summary>
	public Vector3 SpinAxis = Vector3.up;
	public float SpinDegreesPerSecond = 0f;

	private Renderer _renderer;
	private float _v;
	private float _spin;

	private void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	private void LateUpdate()
	{
		if (_renderer == null || _renderer.material == null)
			return;

		// Two independent motions, which is what stops it reading as a rigid spinning tube:
		// the sleeve ORBITS the blade, while the fire itself travels ALONG it.
		// Only spins when explicitly asked. A sleeve that follows a CURVED blade cannot be
		// rotated as a rigid body -- the curve would swing off the steel, which is the bug
		// this replaced. Motion comes from the texture travelling along the helix instead.
		if (SpinDegreesPerSecond != 0f)
		{
			_spin = Mathf.Repeat(_spin + SpinDegreesPerSecond * Time.deltaTime, 360f);
			transform.localRotation = Quaternion.AngleAxis(_spin, SpinAxis);
		}

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
