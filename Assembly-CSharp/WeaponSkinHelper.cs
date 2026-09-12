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
//   9011 Hazardous Shotgun (was Natural Shotgun)   (base 1003 PaintShotty)   -- renamed from Toxic Splatter 2026-08-12
//   9012 Void Amethyst     (base 1004 PaintSniper)   -- also the only tracer override
//   9013 Bloodhound        (base 1003 PaintShotty)
//   9014 Abyssal Leviathan (base 1005 Cannon)
//   9015 Neon Circuit      (base 1002 MachineGun)
//   9016 Crimson Dragon    (base 6 MythicEdge-DE, premium melee)
//   9017 Frostbound        (base 6 MythicEdge-DE)    -- see-through ice, HELIX flames, blue as
//                                                       of 2026-08-17, on the second flame sheet
//   9018 Frostfire         (base 6 MythicEdge-DE)    -- shares 9017's art, SURFACE flames
//   9019 Bloodglass        (base 6 MythicEdge-DE)    -- see-through red, SURFACE flames
//   9020 AWP [Permafrost]  (base AWP_Roughed)        -- see-through ice, HELIX flames as of
//                                                       2026-08-17, plus a blue muzzle light.
//                                                       Its mesh is still not CPU-readable, so
//                                                       SURFACE remains impossible here.
//   9021 Icebreaker        (base DeathHammer)        -- see-through ice, HELIX flames, same
//                                                       caveat. DeathHammer is ItemClass 4, a
//                                                       SHOTGUN with 12 projectiles -- not a
//                                                       warhammer.
//   9022 MG [Watery]       (base 1002 MachineGun)
//   9023 Sniper [Watery]   (base 1004 PaintSniper)
//   9024 Shotgun [Watery]  (base 1003 PaintShotty)
//   9025 Cannon [Watery]   (base 1005 Cannon)
//   9026 MG [Frosted]      (base 1002 MachineGun)
//   9027 Sniper [Frosted]  (base 1004 PaintSniper)
//   9028 Shotgun [Frosted] (base 1003 PaintShotty)
//   9029 Cannon [Frosted]  (base 1005 Cannon)
//   9030 MG [Lava]         (base 1002 MachineGun)   -- the [Lava] set: same water shader and the
//   9031 Sniper [Lava]     (base 1004 PaintSniper)     same four water textures as [Watery],
//   9032 Shotgun [Lava]    (base 1003 PaintShotty)     three colours apart. NO painted art and
//   9033 Cannon [Lava]     (base 1005 Cannon)          NO flames -- see LavaBindings.
//   9034 Neon Circuit [Black] (base 1002 MachineGun) -- 9015's sibling: the SAME cyan linework,
//                                                       pixel-for-pixel, over a graphite body
//                                                       instead of a light one.
//   9035 AWP [Matte Glass]  (base AWP_Roughed)       -- 9020's variant. SHARES 9020's painted art
//                                                       and shader; the ONLY difference is
//                                                       _ReflectColor driven to near-black, which
//                                                       removes the white gloss term and leaves
//                                                       the see-through untouched. NO flames.
//   9036 Icebreaker [Matte Glass] (base DeathHammer) -- 9021's variant, same one-number change.
//   9038 AWP [Clear Ice]     (base AWP_Roughed)       -- the ORIGINAL pre-2026-08-17 look, kept as
//                                                       its own skin: blueish transparent with no
//                                                       glow. NO SkinMaterialBindings entry, which
//                                                       is the whole point -- it inherits
//                                                       ApplyShaderOverride's icy default.
//   9039 Icebreaker [Clear Ice] (base DeathHammer)    -- same, on the hammer.
//   9040 M4A1 [Gold]        (base 28 M4_Standard)      -- the [Gold] set. PROCEDURALLY DYED from
//   9041 AK-47 [Gold]       (base 38 AK47)                each weapon's own stock diffuse: a
//   9042 SPAS-12 [Gold]     (base 60 Automatic_Shotgun_Roughed)  luminance remap through a
//   9043 AWP [Gold]         (base 92 AWP_Roughed)         bronze->gold->specular ramp, so every
//                                                       panel line, screw and vent of the base
//                                                       survives. No AI pass and no hand art.
//                                                       All four are Bumped Specular, so their
//                                                       .alpha.png red channel is GLOSS.
//   9044-9063  FIVE PROCEDURAL SETS on M4A1 / AK-47 / SPAS-12 / AWP, generated by
//              webgl-skins-rendering/tools/make_skin_sets.py. Same principle as [Gold]: a dye
//              moves colour and never redraws geometry, so every panel line, screw and stamp
//              of the stock texture survives. All Bumped Specular, so .alpha.png red = GLOSS.
//                9044-9047 [Chrome]    mirror silver, tone percentile-pinned per weapon so all
//                                      four read as ONE plating job rather than four metals
//                9048-9051 [Damascus]  domain-warped banding -- folded steel, not a repeat
//                9056-9059 [Carbon]    twill weave on the dark panels, metal furniture kept
//                9060-9063 [Tempered]  heat-oxide gradient driven by POSITION ALONG THE WEAPON,
//                                      from a UV->3D map rasterised out of the mesh export.
//                                      The first skin here whose colour knows where a texel
//                                      sits on the gun rather than only what value it is.
//   9064-9067  [Chrome Max] on the same four weapons -- engraved chrome, and the ONE set tonight
//              that is NOT procedural. Engraving is new detail drawn onto the surface: scrollwork
//              and filigree exist nowhere in the base to be remapped, so a dye cannot produce it
//              and this went through Nano Banana Pro 2. Each weapon carries its own motif
//              (acanthus / eastern baroque / industrial deco / English rose-and-scroll) so the
//              four are not one idea repeated. That distinction -- treatments are dyed, designs
//              are generated -- is the line the [Gold] work established.
//   9052-9055, 9068-9078  [Venom] -- REMOVED 2026-08-18, ids retired, do not reuse.
//              Three rounds all rejected in game: the procedural version ("not very unique" -- it
//              was a filter, so it produced one idea four times), then an AI round, then a
//              15-variant shoot-out with every generation registered as its own skin. None won.
//              The brief itself needs rethinking, not the execution -- so nothing here is worth
//              inheriting. The prompts and every generated sheet are kept in
//              Desktop/UberStrike_Skins_2026-08-06/07_VenomV2 rather than in the client.
//   9037 AWP [Frozen Serpent] (base AWP_Roughed)     -- painted, an ORIGINAL blue/white serpent.
//                                                       Bumped Specular, so its .alpha.png red
//                                                       channel is GLOSS, not transparency. It is
//                                                       the first AWP skin whose gloss mask is
//                                                       DERIVED rather than inherited -- the base
//                                                       has none. Blue muzzle FX, no flames yet.
//
// Completed 2026-08-16 -- it had gone stale a THIRD time, stopping at 9019. Found by
// skin_studio, which derives each skin's base weapon from this block and reported 9020/9021 as
// having no derivable base, so the AWP and DeathHammer could not be previewed at all.
//
// 9030-9033 were added to this block IN THE SAME EDIT that added them to the tables below, which
// is the only way this list stays true. Three of the four times it went stale, the tables were
// right and only this block was wrong -- and because skin_studio reads THIS block to find each
// skin's base weapon, a skin missing here cannot be previewed at all even though it works in game.
//
// This does NOT check ownership/equip state beyond what the game itself already enforces
// via AssignWeapon (only ever called with an item the player has equipped in their
// loadout) -- once the server knows about these item ids and a player's loadout
// references one, this just changes what texture renders. No local bypass.
//
// WHERE "the server knows about these item ids" ACTUALLY STANDS, checked 2026-08-17 rather than
// assumed, because a comment that guesses at the other half of the delivery is how the [Lava]
// icons nearly shipped unembedded:
//   UberServer/src/UberStrok.WebServices.AspNetCore/assets/configs/game/items.json
//     171 WeaponItems, 9030-9034 all present. 9034 is {ID 9034, "Neon Circuit [Black]",
//     ItemClass 3, PrefabName MachineGun}, matching the base weapon this file assumes.
//   UberStrok.WebServices.AspNetCore/bin/Release/net6.0/assets/... (the BUILT copy the running
//     service reads) is still at 166 and has neither 9030-9033 nor 9034, and the older
//     UberStrok.WebServices/configs/game/items.json is at 142.
// So the catalog row EXISTS in source and is NOT yet in the deployed build. Nothing in this file
// depends on that -- these tables are keyed by item id and are inert for an id the server never
// hands out -- but do not read "the client is done" as "the shop will show it".

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
		// 9008 repainted 2026-08-11 against the 4.7.1 MachineGun base, same as 9015.
		// It was parked on the theory that the texture was rotated on this client's mesh;
		// that was wrong. The real cause was the UV layout, see the 9015 note below.
		// Measures 4.1% unpainted at brightness 153.0.
		{ 9008, "9008_InfernoMG.png" },
		{ 9009, "9009_CryoStrike.png" },
		{ 9010, "9010_SolarCannon.png" },
		{ 9011, "9011_HazardousShotgun.png" },
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
		// 9035/9036 SHARE the art above, exactly as 9018 shares 9017's. The matte variants differ
		// only in _ReflectColor, so a second copy of the same two textures would be 4 MB of
		// duplicate art and two more files to keep in sync when the ice grade is next touched.
		{ 9035, "9020_Permafrost.png" },
		{ 9036, "9021_Icebreaker.png" },
		{ 9038, "9020_Permafrost.png" },
		{ 9039, "9021_Icebreaker.png" },
		{ 9040, "9040_M4A1Gold.png" },
		{ 9041, "9041_AK47Gold.png" },
		{ 9042, "9042_SPAS12Gold.png" },
		{ 9043, "9043_AWPGold.png" },
		{ 9044, "9044_M4A1Chrome.png" },
		{ 9045, "9045_AK47Chrome.png" },
		{ 9046, "9046_SPAS12Chrome.png" },
		{ 9047, "9047_AWPChrome.png" },
		{ 9048, "9048_M4A1Damascus.png" },
		{ 9049, "9049_AK47Damascus.png" },
		{ 9050, "9050_SPAS12Damascus.png" },
		{ 9051, "9051_AWPDamascus.png" },
		{ 9056, "9056_M4A1Carbon.png" },
		{ 9057, "9057_AK47Carbon.png" },
		{ 9058, "9058_SPAS12Carbon.png" },
		{ 9059, "9059_AWPCarbon.png" },
		{ 9060, "9060_M4A1Tempered.png" },
		{ 9061, "9061_AK47Tempered.png" },
		{ 9062, "9062_SPAS12Tempered.png" },
		{ 9063, "9063_AWPTempered.png" },
		{ 9064, "9064_M4A1ChromeMax.png" },
		{ 9065, "9065_AK47ChromeMax.png" },
		{ 9066, "9066_SPAS12ChromeMax.png" },
		{ 9067, "9067_AWPChromeMax.png" },
		// 9037. NOT a Glass-Hangar skin -- it keeps AWP_Roughed's own Bumped Specular, so unlike
		// 9020/9021/9035/9036 on the same weapon, alpha here means GLOSS and not transparency.
		{ 9037, "9037_FrostSerpent.png" },
		{ 9079, "9079_AWPUberverse.png" },
		{ 9080, "9080_CyberNeon.png" },
		{ 9081, "9081_ToxicVenom.png" },
		{ 9082, "9082_MoltenInferno.png" },
		// V1 (9083) and V1.2 (9084) share the same Uberverse galaxy paint as V2 (9079); they
		// differ only in their orbital FX, so no new art is embedded for them.
		{ 9083, "9079_AWPUberverse.png" },
		{ 9084, "9079_AWPUberverse.png" },
		// New weapon skins on their own base weapons (not the AWP): Wrecker (116) + Splattergun (106).
		{ 9085, "9085_WreckerVoidglass.png" },
		{ 9086, "9086_SplattergunPrismSplatter.png" },
		{ 9087, "9087_LauncherDragonsMaw.png" }, // Grenade Launcher (base 111)
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
		// 2026-08-17. The black-base sibling of 9015 Neon Circuit, on the same MachineGun.
		//
		// Like the [Frosted]/[Watery] sets this is GENERATED from an existing sheet rather than
		// painted, and for the same reason: the circuit traces are the identity of the skin and
		// must not move a single texel. Every operation is pixel-wise on 9015's own texels, so
		// every UV island stays on its original pixel and coverage cannot drop -- which is the
		// failure that left the ORIGINAL 9015 with ~37% of its mesh unpainted.
		//
		// The transform, in order: isolate the linework by cyan-excess (G+B)/2 - R through a
		// smoothstep 38..52 gate (measured bimodal on 9015: p90 16.0 against p99 176.5, so the
		// gate sits in an empty gap); compress the BODY on HSV value only, holding hue and
		// saturation, to a graphite floor near luminance 22; raise the TRACES by each pixel's own
		// headroom so nothing clips; then a painted cyan bloom around them.
		//
		// TWO THINGS HERE WERE FOUND BY MEASUREMENT AND ARE WORTH KEEPING:
		//
		//   * The body floor is graphite, NOT #000000, and that is not timidity. This weapon's
		//     material is Normal-BumpSpec -- albedo plus gloss and nothing else. Unlike [Lava],
		//     which CAN sit on literal black because its _ReflectColor reflection term carries
		//     the whole skin, a black albedo here leaves the normal map with no diffuse term to
		//     shade and the gun becomes a flat silhouette on dark maps.
		//
		//   * The traces are brightened by PER-PIXEL headroom, not by a flat gain. A flat gain
		//     large enough to hit the contrast target clips the trace cores to (94,255,255),
		//     which equalises G and B and drags the hue off pure cyan -- measured hue mass in the
		//     cyan bin fell to 72.9%. Scaling each pixel by 255/max-channel reaches the same
		//     luminance with hue and saturation mathematically unchanged.
		//
		// ITS ALPHA IS 9015's, BYTE-FOR-BYTE, and that is deliberate. On this weapon _MainTex is
		// "Base (RGB) Gloss (A)" and the shader does o.Gloss = tex.a, so alpha is GLOSS, not
		// transparency (contrast 9017 below, where the shader swap makes it transparency). Alpha
		// says WHICH PANELS ARE POLISHED, the panels did not move, so neither did the mask.
		// 9015's and 9008's masks are already byte-identical to each other for the same reason.
		{ 9034, "9034_NeonCircuitBlack.png" },
	};

	/// <summary>
	/// How a skin's flame overlay is built.
	///
	/// Surface was the original concept: a copy of the weapon's own mesh drawn additively over
	/// it, so the fire sits ON the steel. Helix builds a separate sleeve of ribbons standing
	/// off the blade and winding along it, so the fire orbits OUTSIDE the sword.
	/// </summary>
	/// <summary>
	/// Surface = fire painted onto a COPY of the weapon's own geometry.
	/// Helix   = a ribbon sleeve built around the weapon's bounding box.
	/// Shell   = the weapon's own mesh REFERENCED (not copied) and scaled up slightly, so the
	///           fire is the exact silhouette of the gun, just bigger. An outer flame.
	///
	/// SHELL IS THE ONLY MODE THAT WORKS ON A NON-CPU-READABLE MESH, and that is the whole point
	/// of it. Surface calls WhiteVertexCopy, which reads `.vertices` and returns null on
	/// AWP.asset / Death_Hammer.asset / ShotGun.asset / Sniper.asset (all m_IsReadable: 0), so
	/// those weapons could never have surface fire. Shell assigns `sharedMesh = sourceMesh` -- a
	/// REFERENCE. Nothing is read back to the CPU, so readability is irrelevant. The only mesh
	/// data it touches is `.bounds`, which is the serialised m_LocalAABB and is always present.
	/// </summary>
	public enum FlameMode { Surface, Helix, Shell }

	/// <summary>
	/// How much bigger than the weapon a Shell overlay is drawn, per skin. 1.0 would sit exactly
	/// on the surface and z-fight-free but invisible; the useful range is small.
	/// </summary>
	public static readonly Dictionary<int, float> SkinShellScale = new Dictionary<int, float>
	{
		// 2026-08-17, THIRD attempt at these two, after helix was rejected twice in game as
		// "no flames at all, just lines". The brief that produced this: "the exact weapon shape
		// but just bigger, with a low overlay, so it's an outer blue flame."
		//
		// Scale is applied about the MESH BOUNDS CENTRE, not the transform pivot. That distinction
		// already cost a bug once -- the note at the localScale assignment records an earlier 1.015
		// that read as a second ghostly blade beside the real one, because these weapons' geometry
		// sits well off its pivot. The overlay is offset by bounds.center * (1 - scale) to cancel
		// exactly that, so the shell grows evenly around the gun instead of sliding off it.
		//
		// Why these numbers: the shell reads mostly as a RIM. Particles/Additive is ZWrite Off, so
		// the front and back faces of the shell both draw and both add -- at a grazing angle the
		// eye looks through more shell and the add doubles, which puts the brightest fire exactly
		// on the silhouette where it belongs. Over the middle of the gun it is a single soft pass.
		// That is why the tint can stay low and still read as fire on the edge.
		{ 9020, 1.09f },   // AWP: long and thin, so a larger factor is still a small absolute skin
		{ 9021, 1.07f },   // Death Hammer: chunkier body, the same factor would look inflated
	};

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
		//
		// 9020/9021 ARE HELIX, AND THESE TWO ROWS ARE THE MOST LOAD-BEARING IN THE TABLE.
		// AWP.asset and Death_Hammer.asset both carry m_IsReadable: 0, so Surface mode -- which
		// rebuilds the weapon's own geometry from source.vertices -- cannot work on them by any
		// route. It no longer CRASHES (Object.Instantiate is gone; the read is a catchable managed
		// error and WhiteVertexCopy probes it first), so the real cost of getting this wrong today
		// is a logged skip and no fire, not a dead session. But the default at :1550 is one line
		// away from being flipped, and this is exactly the pair that would pay for it, so the mode
		// is written down rather than inherited.
		// 2026-08-17, THIRD approach. Helix was judged in game TWICE and rejected both times --
		// "wire scratches lying on the receiver", then "no flames at all, just lines". Two ribbons
		// wound round a cylinder present almost no surface to the camera, and widening the arc and
		// re-tinting them blue did not change that verdict.
		//
		// Shell instead: the weapon's own mesh, referenced and scaled up, so the fire is the gun's
		// exact silhouette and cannot read as a wire. This is only possible at all because Shell
		// does not copy the mesh -- AWP.asset and Death_Hammer.asset are m_IsReadable: 0, which is
		// what ruled Surface out and sent the first two attempts to Helix in the first place.
		//
		// The SkinSleeves entries for 9020/9021 are deliberately LEFT IN PLACE. They are unused by
		// Shell, they are fully documented, and they are the record of what was tried -- deleting
		// them invites a fourth attempt at the same thing.
		// 9020/9021 removed -- see the note in SkinFlames. Shell mode itself stays; it is the
		// only mode that works on a non-CPU-readable mesh and the next such skin will need it.
		//
		// 9022-9025 [Watery] removed 2026-08-17, together with their SkinFlames entries -- the
		// full reasoning is there, at the sheet that actually turns flames on. This entry would
		// be dead weight without it, and a mode left behind for a skin that no longer burns is
		// exactly the sort of half-registration that gets copied into the next set.
		{ 9026, FlameMode.Surface },
		{ 9027, FlameMode.Surface },
		{ 9028, FlameMode.Surface },
		{ 9029, FlameMode.Surface },
	};

	// ---------------------------------------------------------------- the water shader
	//
	// The game's OWN flowing-water shader, used by the [Watery] set. Everything below was read
	// out of the shipped client rather than guessed, because a wrong name here fails silently:
	// Shader.Find returns null and the skin renders opaque with no error.
	//
	// Verified in the SHIPPED build, not in the source project:
	//   * shader text lives in UberStrike_Data/resources.assets at byte offset 307673488,
	//     length 61657, and its first line is Shader "CMune/Water/Opaque_Flowing".
	//   * that name occurs exactly ONCE in resources.assets, and a scan for "CMune/Water/*"
	//     returns no siblings -- so there is no second water shader to fall back to.
	//
	// Its Properties block, verbatim from that blob:
	//   _MainTex        ("Base (RGB) Gloss (A)", 2D)   = "white"
	//   _BumpMap        ("Normalmap", 2D)              = "bump"
	//   _Caustics       ("_Caustics", 2D)              = "black"
	//   _Cube           ("Reflection Cubemap", CUBE)   = "black"
	//   _Color          ("Main Color", Color)          = (0, 0.313726, 0.65098, 1)
	//   _WaterColor_Dark("Dark Water Color", Color)    = (1, 1, 1, 1)
	//   _ReflectColor   ("Reflection Color", Color)    = (0.72549, 0.992157, 1, 0.501961)
	//   _Specular ("_Specular", Float) = 2   _Gloss ("_Gloss", Float) = 1   _Tiling = 1.5
	//
	// A BARE SHADER BIND IS NOT ENOUGH, and this is the note that must survive: binding the
	// shader without assigning textures gives _Caustics = "black", so the caustics term
	// multiplies out to ZERO and the weapon has no caustics at all, while _Cube = "black"
	// collapses the cubemap lerp and _WaterColor_Dark defaults to WHITE water. Nothing errors;
	// it just renders wrong. That is the Bloodglass failure repeating -- see the 9019 note in
	// SkinShaders, where an unassigned _Cube sampled WHITE and turned a red blade grey-pink.
	// So SkinMaterialBindings below is load-bearing. Do not "simplify" those assignments away.
	//
	// The motion is free. The compiled program scrolls _MainTex at 0.050000001 and _BumpMap at
	// 0.07 off the shader's own time input, so the water flows with NO MonoBehaviour driving it
	// -- unlike the flame overlay, which needs WeaponFlameAnimator.
	public const string WaterShader = "CMune/Water/Opaque_Flowing";

	// Resources paths for the water assets, in the LOWERCASE form the build's index actually
	// stores. This client keeps its Resources index in UberStrike_Data/mainData, and every
	// entry there is lowercased even where the file on disk is mixed case -- e.g. the file is
	// Water_A_NM.png but the index reads "items/shared/textures/water_a_nm".
	//
	// Lowercase is used here because it is the strictly SAFER of the two spellings: if Unity
	// lowercases the query before lookup, a lowercase path is unchanged and matches; if it does
	// NOT, a lowercase path still matches the lowercase index while a mixed-case one would miss.
	// Mixed case is only safe under the first assumption, so it is not worth the risk.
	//
	// All five confirmed present in mainData's index (one hit each):
	//   items/shared/textures/water_a_nm      items/shared/textures/water_b_nm
	//   items/shared/textures/caustics_a_dm   items/shared/cubemaps/studio_a
	//   items/shared/shaders/water_flowing_a
	private const string WaterMainTexPath  = "items/shared/textures/water_a_nm";
	private const string WaterBumpMapPath  = "items/shared/textures/water_b_nm";
	private const string WaterCausticsPath = "items/shared/textures/caustics_a_dm";
	private const string WaterCubePath     = "items/shared/cubemaps/studio_a";
	private const string WaterShaderPath   = "items/shared/shaders/water_flowing_a";

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
		{ 9035, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		{ 9036, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		// 9038/9039 bind the SAME shader and deliberately have NO SkinMaterialBindings row, so
		// ApplyShaderOverride's icy default supplies _ReflectColor (0.55, 0.75, 0.95, 0.08) and
		// _Color stays the material's own (1,1,1,1). That combination IS the look these two
		// shipped with before 2026-08-17, and the team asked to keep it alongside the two new
		// treatments rather than replace it. Alpha 0.08 on the reflection is why there is no glow.
		{ 9038, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		{ 9039, new string[] { "Unique/Transparent/Glass-Hangar", "Transparent/Diffuse" } },
		// 9022-9025, the [Watery] set, bind the game's OWN water shader.
		//
		// This REPLACES their painted art, knowingly. _MainTex on this shader is not albedo --
		// the shipped materials put a NORMAL MAP in that slot (see SkinMaterialBindings) -- so
		// the glacier paint these four ship with is not sampled at all. They come out looking
		// like cut-crystal/ice weapons. That is the intended result, not a defect to fix.
		//
		// OPAQUE is what makes this safe here where Glass-Hangar was not. These four shipped
		// once with Glass-Hangar and every one of them rendered HOLLOW -- bright ice edges with
		// the body see-through to the wall behind -- because alpha-blended geometry does not
		// write depth, so the overlapping faces of one mesh sort arbitrarily. A katana blade is
		// a single thin shape and survives that; a machine gun is dozens of overlapping parts
		// and does not. This shader is tagged "RenderType" = "Opaque" in the shipped text, so
		// it writes depth and the hollow failure cannot recur.
		//
		// ONE name, not a chain to some other shader, and that is deliberate. There is exactly
		// one CMune/Water/* shader in the build, so any second NAME here would be a different
		// look entirely -- and worse, it would silently no-op every water binding below, which
		// is precisely the half-state this whole change exists to prevent. The genuine second
		// route is by Resources PATH to the SAME shader, in SkinShaderResources.
		{ 9022, new string[] { WaterShader } },
		{ 9023, new string[] { WaterShader } },
		{ 9024, new string[] { WaterShader } },
		{ 9025, new string[] { WaterShader } },
		// 9030-9033 [Lava] take the SAME shader. Molten rock is flowing liquid with a black body
		// and a hot reflection, which is this shader with three colours changed -- see
		// LavaBindings. Every argument above applies unchanged, including the OPAQUE one: these
		// are the same dozens-of-overlapping-parts firearms that rendered hollow under
		// Glass-Hangar, and this shader writes depth.
		{ 9030, new string[] { WaterShader } },
		{ 9031, new string[] { WaterShader } },
		{ 9032, new string[] { WaterShader } },
		{ 9033, new string[] { WaterShader } },
	};

	/// <summary>
	/// Second route to a skin's shader, by Resources path, tried ONLY after every name in
	/// SkinShaders has failed Shader.Find.
	///
	/// Not redundant with the name chain, and not a different look. Shader.Find only returns
	/// shaders that made it into the build, and this one is reachable BOTH ways: its text is in
	/// resources.assets, and the build's Resources index lists "items/shared/shaders/
	/// water_flowing_a". Being in Resources is also why it is certain to have survived shader
	/// stripping at all -- unlike Glass-Hangar, which no item material references and which the
	/// 9019 note above had to hedge against.
	///
	/// So this reaches the IDENTICAL asset by a second mechanism, which is the only kind of
	/// fallback worth having when the alternative would silently change what the skin looks like.
	/// </summary>
	public static readonly Dictionary<int, string> SkinShaderResources = new Dictionary<int, string>
	{
		{ 9022, WaterShaderPath },
		{ 9023, WaterShaderPath },
		{ 9024, WaterShaderPath },
		{ 9025, WaterShaderPath },
		// [Lava] gets the same second route, and it is not optional bookkeeping: these skins have
		// no painted art to fall back on, so a Shader.Find miss without this entry would leave
		// them rendering fully STOCK -- not "a bit wrong", but the base weapon.
		{ 9030, WaterShaderPath },
		{ 9031, WaterShaderPath },
		{ 9032, WaterShaderPath },
		{ 9033, WaterShaderPath },
	};

	// ------------------------------------------------- extra per-skin material bindings

	/// <summary>
	/// One texture to assign after the shader swap. <c>IsCubemap</c> is not cosmetic: a cubemap
	/// is NOT a Texture2D, so Resources.Load&lt;Texture2D&gt; on one returns NULL and the
	/// property would end up unassigned -- the exact silent half-state this file keeps hitting.
	/// </summary>
	public struct TextureBinding
	{
		public string Property;
		public string ResourcePath;
		public bool IsCubemap;

		public TextureBinding(string property, string resourcePath, bool isCubemap)
		{
			Property = property;
			ResourcePath = resourcePath;
			IsCubemap = isCubemap;
		}
	}

	public struct ColorBinding
	{
		public string Property;
		public Color Value;

		public ColorBinding(string property, Color value)
		{
			Property = property;
			Value = value;
		}
	}

	public struct FloatBinding
	{
		public string Property;
		public float Value;

		public FloatBinding(string property, float value)
		{
			Property = property;
			Value = value;
		}
	}

	/// <summary>
	/// Everything a skin needs assigned onto its material AFTER the shader is bound.
	///
	/// This did not exist before: ApplyToWeapon only ever set _MainTex, which is all a
	/// re-texture on the stock Bumped Specular shader needs. A shader with ten more properties
	/// needs all of them, and leaving any one unset is silent -- it takes the value from the
	/// shader's Properties block and renders something plausible but wrong.
	/// </summary>
	public class MaterialBindings
	{
		public TextureBinding[] Textures;
		public ColorBinding[] Colors;
		public FloatBinding[] Floats;
	}

	/// <summary>
	/// The [Watery] material, copied from the two materials that ALREADY ship with this shader
	/// rather than invented:
	///
	///   Resources/items/weapons/splattergun_manowar/res/res/SpatterGun_ManOWar_Water_A.mat
	///   Resources/items/gear/holo_hydra/res/res/Holo_Hydra.mat
	///
	/// Both bind the same four textures by GUID, resolved through their .meta files:
	///   _MainTex  010e39f5b75d51b479ea93966b1a5091 -> items/shared/textures/Water_A_NM.png
	///   _BumpMap  f951481b5ecdcba42b9582f464c29b11 -> items/shared/textures/Water_B_NM.png
	///   _Caustics fd207a9c99c672249975c3b055ede01a -> items/shared/textures/Caustics_A_DM.png
	///   _Cube     b63410db318971340a5fb77d191c6193 -> items/shared/cubemaps/Studio_A.png
	///
	/// Note _MainTex is a NORMAL MAP (Water_A_NM), not colour. That is not a mistake in the
	/// shipped materials -- it is how this shader works, and it is why binding it replaces the
	/// painted art instead of tinting it.
	///
	/// COLOURS COME FROM THE WEAPON MATERIAL, NOT FROM THE SHADER DEFAULTS, and the difference
	/// is large enough to matter:
	///   _WaterColor_Dark  shipped (0, 0.153, 0.478)  vs  shader default (1, 1, 1) -- the
	///                     default is WHITE water, i.e. no dark tone at all.
	///   _Color            shipped (0, 0.439, 1)      vs  default (0, 0.314, 0.651)
	///   _ReflectColor     shipped (0.420, 0.839, 1, 0.502) vs default (0.725, 0.992, 1, 0.502)
	/// </summary>
	/// <summary>
	/// The four textures this shader needs, shared by EVERY skin that binds it.
	///
	/// Hoisted out of WaterBindings when the [Lava] set arrived. Lava is the same shader driven by
	/// the same water assets with three colours changed, so a second copy of these four lines
	/// would be four more things to keep in sync and one more place for the _Cube subtlety below
	/// to be got wrong -- the same reasoning that has the four [Watery] skins share one
	/// MaterialBindings instance.
	///
	/// DECLARED BEFORE both tables that use it, and it must stay that way: C# runs static field
	/// initializers in textual order, so moving this below them would leave Textures NULL, and a
	/// null Textures array is silent -- ApplyShaderOverride's preflight (:1243) simply resolves
	/// nothing, ApplyMaterialBindings (:1374) binds nothing, and the weapon draws with _MainTex
	/// white, _Caustics black and _Cube black. Wrong-but-plausible, with nothing in the log.
	/// </summary>
	private static readonly TextureBinding[] WaterTextureSet = new TextureBinding[]
	{
		new TextureBinding("_MainTex",  WaterMainTexPath,  false),
		new TextureBinding("_BumpMap",  WaterBumpMapPath,  false),
		new TextureBinding("_Caustics", WaterCausticsPath, false),
		// CUBE, not 2D. Studio_A.png imports with textureType 5 / generateCubemap 5 and its
		// .meta recycles fileID 8900000 as "generatedCubemap", so the asset Resources.Load
		// returns is a Cubemap. Asking for a Texture2D here gets null and _Cube stays at
		// "black", which collapses the reflection lerp -- silently.
		new TextureBinding("_Cube",     WaterCubePath,     true),
	};

	private static readonly MaterialBindings WaterBindings = new MaterialBindings
	{
		Textures = WaterTextureSet,

		Colors = new ColorBinding[]
		{
			// Straight from SpatterGun_ManOWar_Water_A.mat, the WEAPON material.
			new ColorBinding("_Color",           new Color(0f, 0.4392157f, 1f, 1f)),
			new ColorBinding("_WaterColor_Dark", new Color(0f, 0.15294118f, 0.47843137f, 1f)),
			// This one must also survive the force-set further down -- see ApplyShaderOverride.
			new ColorBinding("_ReflectColor",    new Color(0.41960785f, 0.8392157f, 1f, 0.5019608f)),
		},

		Floats = new FloatBinding[]
		{
			// _Tiling 0.5 is SpatterGun_ManOWar_Water_A's value; Holo_Hydra uses 1.5. Taking the
			// weapon's number deliberately: 1.5 is authored against a character model's UVs and
			// 0.5 against a gun's, and 0.5 vs 1.5 is a 3x UV-scale difference, so it visibly
			// changes the size of the water's features on a weapon. These four skins are guns.
			new FloatBinding("_Tiling",   0.5f),
			// _Gloss 1.0 / _Specular 2.0, again the weapon's values. Holo_Hydra runs _Gloss 0.9.
			// The compiled program raises the specular exponent to 128 * _Gloss, so 1.0 is the
			// tightest, hardest highlight the shader offers -- which is what reads as wet.
			new FloatBinding("_Gloss",    1.0f),
			new FloatBinding("_Specular", 2.0f),

			// THESE TWO ARE NOT IN THE SHIPPED SHADER, and that is a measured finding, not a
			// guess: a scan of all 61657 bytes of the shader blob returns ZERO occurrences of
			// either name, while every real property (_Tiling, _Gloss, _Cube, ...) occurs 5-16
			// times. The caustics tiling is folded into the compiled code as the literal 3.375
			// (= 1.5 * 2.25) instead of being a uniform.
			//
			// They survive in Holo_Hydra.mat only as stale authoring-time leftovers -- Unity
			// keeps serialised properties a shader no longer declares. Kept here so the table
			// is a complete record of the authored material, and so that if this shader is ever
			// replaced by the authoring version they light up on their own. The HasProperty
			// guard in ApplyMaterialBindings skips them and says so ONCE, at Log rather than
			// LogWarning: a property the shader does not declare is expected here, and must not
			// be confused with an asset that failed to load, which is fatal.
			new FloatBinding("_CausticsTiling", 2.25f),
			new FloatBinding("_CausticsDeform", 0.1f),
		},
	};

	/// <summary>
	/// The [Lava] material -- "moltencore", ids 9030-9033. Same shader as [Watery], same four
	/// textures, THREE COLOURS APART. Nothing else differs, and that is the whole design.
	///
	/// It works because _MainTex on this shader is a NORMAL MAP, not albedo (see WaterBindings).
	/// The colour of the surface comes entirely from _Color, _WaterColor_Dark and _ReflectColor,
	/// so re-tinting those three turns the same flowing liquid from water into molten rock
	/// without touching a single texel. The motion, the caustics and the cubemap highlight are
	/// unchanged and still free -- the compiled program scrolls _MainTex and _BumpMap off its own
	/// time input, with no MonoBehaviour driving it.
	///
	/// The three colours, as authored by the user:
	///   _Color            #000000  (0, 0, 0)                    -- black rock between the cracks
	///   _WaterColor_Dark  #1E0600  (0.117647, 0.023529, 0)      -- barely-lit crust in the dark
	///   _ReflectColor     #FF6A0F  (1, 0.415686, 0.058824)      -- the filaments and grazing rim
	///
	/// _ReflectColor CARRIES THIS SKIN. Both other colours are at or near black, so the diffuse
	/// term contributes almost nothing; what the player sees is the reflection term,
	/// texCUBE(_Cube, worldRefl) * _ReflectColor, which fires where the surface turns away from
	/// the eye. That is why the orange lands as glowing filaments and a hot grazing rim over a
	/// black body rather than as an orange gun.
	///
	/// ITS ALPHA IS 0.5019608, NOT 1, and that is not a rounding of the user's #FF6A0F. Alpha is
	/// not opacity on this property -- ApplyShaderOverride quotes the shader as
	/// o.Albedo = c.rgb + reflcol.rgb * reflcol.a, so alpha is the STRENGTH of the reflection.
	/// 0.5019608 is the shipped SpatterGun_ManOWar_Water_A value that [Watery] uses, and "only
	/// the three colours differ" means the strength does not. Setting it to 1 would double the
	/// only term this skin has and clip the filaments to flat white-orange.
	///
	/// Binding _ReflectColor here also SUPPRESSES the icy default further down: the guard at
	/// :1303 asks whether the skin bound the property itself, and a skin that did not would be
	/// overwritten with (0.55, 0.75, 0.95, 0.08) -- a blue tint at a sixth of the strength, which
	/// on this palette would erase the skin entirely. That guard is load-bearing here, not
	/// incidental.
	///
	/// NO SkinFlames ENTRY, deliberately and by explicit decision: lava ships BARE so QA judges
	/// the colour treatment on its own. Do not "finish" this set by adding fire to it -- the
	/// fire's absence is the thing being tested. (Note also that the flame overlay would land on
	/// the same 3.9%-coverage / MAE 1.59 measurement recorded in SkinFlames for 9022, since these
	/// sit on the same four base weapons.)
	///
	/// NO SkinTextures ENTRY EITHER, and this needs no art on disk. ApplyToWeapon reads
	/// GetSkinTexture at :1109 and gets null for an unregistered id, but the early-return at :1111
	/// is `tex == null && !hasBindings` -- having an entry in SkinMaterialBindings is itself the
	/// qualification, so the method runs on to ApplyShaderOverride and binds normally. The only
	/// thing tex would have done is the _MainTex assignment at :1160-1162, and ApplyMaterialBindings
	/// overwrites that with Water_A_NM at :1384 regardless. A painted .jpg for these ids would be
	/// loaded, assigned, and thrown away in the same frame.
	/// </summary>
	private static readonly MaterialBindings LavaBindings = new MaterialBindings
	{
		// The identical four assets [Watery] binds -- shared, not copied. See WaterTextureSet.
		Textures = WaterTextureSet,

		Colors = new ColorBinding[]
		{
			// #000000. Black, exactly: the crust is unlit rock and any lift here greys the whole
			// weapon, because this is the base colour the water term is tinted by.
			new ColorBinding("_Color",           new Color(0f, 0f, 0f, 1f)),
			// #1E0600 = 30/255, 6/255, 0/255.
			new ColorBinding("_WaterColor_Dark", new Color(0.11764706f, 0.023529412f, 0f, 1f)),
			// #FF6A0F = 255/255, 106/255, 15/255, at [Watery]'s reflection STRENGTH -- see above.
			new ColorBinding("_ReflectColor",    new Color(1f, 0.41568628f, 0.05882353f, 0.5019608f)),
		},

		Floats = new FloatBinding[]
		{
			// Identical to [Watery]: same shader, same weapons, same UVs. _Tiling 0.5 is authored
			// against a gun rather than a character, and _Gloss 1.0 raises the specular exponent
			// to 128 -- the tightest highlight the shader offers, which reads as wet on water and
			// as a molten sheen here.
			new FloatBinding("_Tiling",   0.5f),
			new FloatBinding("_Gloss",    1.0f),
			new FloatBinding("_Specular", 2.0f),

			// _CausticsTiling and _CausticsDeform are NOT carried over from WaterBindings, where
			// they exist only as a record of the authored material. This shipped shader declares
			// neither (measured: zero occurrences in all 61657 bytes of the blob), so listing them
			// would bind nothing and only add a second skipped-property line to the log for a set
			// that has no story to tell about caustics.
		},
	};

	/// <summary>
	/// The icy glass material for 9020 AWP [Permafrost] and 9021 Icebreaker.
	///
	/// THIS TABLE, NOT THEIR .jpg FILES, IS THE ONLY THING THAT CAN CHANGE HOW THESE TWO LOOK,
	/// and that is the finding this round paid for. The request was "keep the transparency, make
	/// them icier", which sounds like an art job and is not one. Read out of the SHIPPED binary
	/// rather than the source project -- Glass-Hangar's text sits in sharedassets16.assets, and
	/// its ForwardBase fragment program is thirteen instructions:
	///
	///     TEX R1.x, fragment.texcoord[0], texture[0], 2D;   // _MainTex -- .x, RED, ONLY
	///     MUL R2.xyz, R2, c[2];                             // _LightColor0 * _Color  (no texture)
	///     ADD R0.w, -R1.x, c[4].x;                          // 1 - tex.r
	///     MUL R0.xyz, R0, c[3];                             // cube * _ReflectColor.rgb
	///     MAD result.color.xyz, R1.x, R0, R2;
	///     MUL result.color.w, R0, c[2];                     // alpha = (1 - tex.r) * _Color.a
	///
	/// So on this shader the painted RGB is NEVER the albedo; only the RED channel is sampled at
	/// all; the sheet's green, blue and alpha are dead; and transparency is 1 - RED. A colour
	/// re-grade of those two .jpg files is invisible in game. That was measured as well as
	/// derived: a deliberately extreme icy grade, written losslessly so red stayed bit-exact,
	/// changed 181 pixels of 786432 at max 7/255 -- against a control of the same config captured
	/// twice, which differs by 135 pixels at max 3. The grade is at the capture noise floor.
	///
	/// TWO CORRECTIONS TO WHAT THIS FILE SAID BEFORE, both from that program text:
	///
	///   * The note in ApplyShaderOverride quotes the shader as
	///     `o.Albedo = c.rgb + reflcol.rgb * reflcol.a`. That is the surface-shader SOURCE form.
	///     The shipped compiled program never reads _ReflectColor.a -- so the icy default's
	///     alpha 0.08 has always been inert, and the reflection wash has always been running at
	///     full (0.55, 0.75, 0.95). Nothing to fix; a lot to know before tuning alpha.
	///
	///   * Glass-Hangar's Properties block advertises _MainTex as "Base (RGB) Trans (A)". It
	///     lies. Alpha is unread and transparency comes from RED. This is very likely the real
	///     reason 9019 Bloodglass was wrong for five attempts: a red skin is exactly the one
	///     whose red channel is high everywhere, i.e. the one this shader renders nearly opaque
	///     and nearly unlit.
	///
	/// WHAT THE VALUES DO. _Color is the whole diffuse term, multiplied by the light and by
	/// nothing else; _ReflectColor tints the cubemap term, which is gated by tex.r so it lands on
	/// the bright painted areas. Together they are the entire palette of the skin.
	///   _Color        (0.78, 0.88, 0.96)  a cool near-white body, cooler than the (1,1,1) default
	///   _ReflectColor (0.35, 0.72, 1.00)  a genuinely blue reflection, against the icy default's
	///                                     much paler (0.55, 0.75, 0.95)
	/// Previewed: saturation 0.222 -> 0.344 on the AWP, blue-minus-red +41 -> +63, and the
	/// shotgun in particular starts reading as translucent blue ice rather than as a pale wash.
	///
	/// _Color.a IS 1.0 AND THAT IS SAFE, verified rather than assumed -- alpha is
	/// (1 - tex.r) * _Color.a, and Glass-Hangar's Properties block declares
	/// `_Color ("Main Color", Color) = (1,1,1,1)`, which is the value these skins have been
	/// running on. So binding 1.0 changes transparency by exactly nothing. Any OTHER value here
	/// would be the one way this entry could break the thing it must not break.
	///
	/// HONEST LIMIT, so nobody re-litigates it later: hue barely moves (202.3 -> 202.1 degrees);
	/// what moves is saturation. That is the signature of a filter, not of ice. Because _Cube is
	/// never assigned and samples WHITE, the wash is view-INDEPENDENT -- there is not one glint on
	/// either weapon at any angle. This is a well-chosen blue tint on a transparent gun. Real ice
	/// with depth needs _Cube bound to a cubemap (items/shared/cubemaps/studio_a is already used
	/// by [Watery], and TextureBinding.IsCubemap exists for exactly this), which is a bigger
	/// change and wants its own preview pass with a real cubemap sampler in the studio.
	///
	/// NO Textures ARRAY, deliberately: leaving it null makes ApplyShaderOverride's preflight
	/// resolve nothing and ApplyMaterialBindings skip the texture loop, which is correct -- these
	/// skins DO use their painted sheet, through its red channel, and must not have _MainTex
	/// replaced the way the [Watery] set does.
	/// </summary>
	private static readonly MaterialBindings GlassIceBindings = new MaterialBindings
	{
		Colors = new ColorBinding[]
		{
			new ColorBinding("_Color",        new Color(0.78f, 0.88f, 0.96f, 1f)),
			// Binding this also SUPPRESSES the icy default in ApplyShaderOverride, via the
			// BindsColor guard -- which is the intended mechanism, not a side effect.
			new ColorBinding("_ReflectColor", new Color(0.35f, 0.72f, 1.00f, 0.5019608f)),
		},
	};

	/// <summary>
	/// 9035 / 9036 -- "glassy look-through but not shiny". The QA brief for the matte variants.
	///
	/// THE SHINE AND THE TRANSPARENCY ARE DIFFERENT TERMS, which is why this is two numbers and
	/// not an art change. The shipped Glass-Hangar program, transcribed from the client's own
	/// compiled ARBfp1.0 (sharedassets16.assets, offset 17,882,095, length 144,424), is:
	///
	///     rgb   = _LightColor0 * _Color  +  tex.RED * (cube * _ReflectColor.rgb)
	///     alpha = (1 - tex.RED) * _Color.a
	///
	/// So the "shiny" is entirely the second rgb term, and it is not a reflection at all: _Cube is
	/// unassigned on these skins, an unassigned samplerCUBE reads WHITE, so it is a flat
	/// view-independent white gloss scaled by _ReflectColor. That is exactly the thing QA called
	/// shiny -- it does not move with the camera, so it reads as a sheen sitting ON the glass
	/// rather than as a reflection in it.
	///
	/// Transparency does not pass through _ReflectColor at any point. It is the red channel of the
	/// painted sheet, inverted, times _Color.a. So driving _ReflectColor to near-black removes the
	/// gloss and provably CANNOT affect how see-through the weapon is. One variable moved.
	///
	/// Not exactly zero: 0.04/0.06/0.08 leaves a trace of the term alive, so the bright-red areas
	/// of the sheet still separate very slightly from the dark ones and the glass keeps some
	/// internal structure. At a true 0 the body flattens to a single translucent tone.
	///
	/// THIS IS THE SAME KNOB BLOODGLASS SITS ON, at the other end. Every see-through skin in the
	/// file is one number apart, which is worth seeing as a ladder before anyone re-tunes:
	///     9019 Bloodglass   _ReflectColor 0.95 / 0.55 / 0.52   warm, strong gloss (SkinReflectTints)
	///     icy default       0.55 / 0.75 / 0.95   cool, strong gloss (ApplyShaderOverride fallback)
	///     9020/9021 GlassIce 0.35 / 0.72 / 1.00  cool, medium gloss
	///     9035/9036 (here)  0.04 / 0.06 / 0.08   gloss essentially off
	/// So "glassy but not shiny" is not a different technique from Bloodglass -- it is Bloodglass's
	/// mechanism with the gloss term turned down. The alpha on _ReflectColor is NOT part of this:
	/// the compiled program multiplies only .xyz, so that channel is dead on this shader and the
	/// 0.08 / 0.502 values scattered through the file are inert.
	///
	/// _Color.a stays at 1.0 -- the same as 9020/9021. The brief was about shine, not about
	/// transparency, and the see-through is already what the team wanted. If they ask for MORE
	/// see-through later, _Color.a is the knob, and it is independent of everything above.
	/// </summary>
	private static readonly MaterialBindings GlassMatteBindings = new MaterialBindings
	{
		Colors = new ColorBinding[]
		{
			// WHY _Color CARRIES THE MATTE, and not _ReflectColor. Proven in game 2026-08-17 with
			// a deliberately absurd test: _ReflectColor (1,0,0,1) turned the weapon visibly RED, so
			// the property is live and the bindings do land -- the log confirmed both values
			// reaching the material. The first matte attempt still looked identical to the glossy
			// one for a reason the arithmetic hid: _Cube is unassigned, so the reflection term is a
			// FLAT view-independent ADD, and the glossy value (0.35, 0.72, 1.00) is BLUE on a gun
			// that is already blue-white. Subtracting blue from blue moves the picture almost not
			// at all. The red test was visible only because red is a hue the weapon does not
			// already carry.
			//
			// So what reads as "shiny" here is not a highlight -- there is no cubemap to reflect
			// and nothing moves with the camera. It is FLAT BRIGHTNESS: a high _Color plus that
			// constant add, which together wash the surface toward uniform pale. Matte therefore
			// means turning the brightness down and letting the painted texture's own contrast
			// come back, which is what 0.78 -> 0.56 does.
			//
			// Alpha 1.0 -> 0.86 also answers the "glassy look through" half: alpha is
			// (1 - tex.RED) * _Color.a, so this is the one knob that makes the weapon more
			// see-through without touching the art.
			new ColorBinding("_Color",        new Color(0.56f, 0.64f, 0.72f, 0.86f)),
			new ColorBinding("_ReflectColor", new Color(0.05f, 0.07f, 0.09f, 0.5019608f)),
		},
	};

	/// <summary>
	/// Per-skin material bindings, applied after the shader override binds.
	///
	/// All four [Watery] skins share ONE instance rather than four copies: they differ only in
	/// which weapon they sit on, and four copies of the same numbers would only be four things
	/// to keep in sync -- the same reasoning as 9018 sharing 9017's texture.
	/// </summary>
	public static readonly Dictionary<int, MaterialBindings> SkinMaterialBindings = new Dictionary<int, MaterialBindings>
	{
		// The two Glass-Hangar firearms share one instance for the same reason the sets below do:
		// the same shader on the same kind of base, and two copies of two colours would only be
		// two things to keep in sync. Note these two ALSO have painted art -- unlike [Lava], they
		// do not need this entry to qualify as skins at all; it only changes their palette.
		{ 9020, GlassIceBindings },
		{ 9021, GlassIceBindings },
		// The matte variants. Same shader, same art, gloss term off -- see GlassMatteBindings.
		{ 9035, GlassMatteBindings },
		{ 9036, GlassMatteBindings },
		{ 9022, WaterBindings },
		{ 9023, WaterBindings },
		{ 9024, WaterBindings },
		{ 9025, WaterBindings },
		// The [Lava] set, on the same four base weapons and sharing one instance for the same
		// reason. This entry is also what QUALIFIES 9030-9033 as skins at all: they register no
		// painted texture, and ApplyToWeapon's guard at :1111 admits a skin with bindings but no
		// art precisely so a set like this one can exist.
		{ 9030, LavaBindings },
		{ 9031, LavaBindings },
		{ 9032, LavaBindings },
		{ 9033, LavaBindings },
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
		// 2026-08-17. BLUE, because the Helix default is not.
		//
		// The default (0.40, 0.47, 0.52, 0.5) peaks at 0.80 / 0.94 / 1.04, and the measured hue of
		// the light it actually adds is 0.854 : 1.000 : 0.973 -- WHITE, going faintly green. Two
		// things do that: every channel is at or above 0.8 so the ratios flatten, and blue CLIPS
		// (2 * 0.52 > 1), which throws away the small blue bias it had. "The flames are white" was
		// never a sheet problem; the sheet is near-neutral by design (see SkinFlames).
		//
		// 0.50 on blue is chosen exactly, not rounded: peak add = 2 * 0.50 = 1.000, sitting ON the
		// clip point, so the blue burns as hot as the shader allows WITHOUT saturating and washing
		// the hue back toward white -- which is precisely what the default does. Measured result:
		// added light 0.327 : 0.752 : 1.000, blue-dominant against an ice blade.
		//
		// THE ALPHA IS THE ONLY "TRANSPARENCY" KNOB THERE IS, and it is worth saying plainly
		// because the request asked for transparent flames. Particles/Additive is
		// Blend SrcAlpha One with ColorMask RGB: the overlay can only ADD light, it can never
		// occlude, so it is already completely see-through and no value here can make it more so.
		// What alpha buys is total ENERGY, linearly: col.a = saturate(2 * tint.a * tex.a), so 0.5
		// is full strength, values above 0.5 clamp and do nothing, and 0.35 would scale the whole
		// add to 0.70. Shipping at 0.5 because the new two-tongue sheet already lands at ~60% of
		// the old sheet's energy; drop this one number to 0.35f if it still reads too hot in game.
		{ 9017, new Color(0.13f, 0.28f, 0.50f, 0.5f) },
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
		// 2026-08-17, SECOND PASS. BLUE, not white -- rejected in game, and the reason is the blend
		// mode rather than the values.
		//
		// The first pass set these flat white at 0.34 and reasoned about clipping. It was judged in
		// game and read as thin white hairlines with NO fire at all, and the screenshots show why:
		// the strands are plainly visible where they overhang the floor and VANISH where they cross
		// the weapon.
		//
		// Particle Add is `Blend SrcAlpha One` with `ColorMask RGB` -- the overlay can only ADD
		// light, never remove it. So a flame is visible exactly to the extent that what it is drawn
		// over has headroom left in that channel. These two weapons are the worst case there:
		// Glass-Hangar over pale ice art, made paler still by the GlassIce re-grade, on a bright
		// white-grey map. R and G are already near 1.0 before the overlay contributes anything, so
		// 0.68 of flat white adds nothing over the gun and only the hottest core survives -- which
		// is precisely a hairline.
		//
		// THE FIRST PASS'S ARITHMETIC WAS SOUND AND ITS BACKGROUND WAS WRONG. It closed with "still
		// reads as clean white off-silhouette (the hangar background measures 0.10-0.13)". That is
		// the STUDIO's hangar. The map this was judged on is white-grey, roughly 8x brighter, and a
		// tint chosen against 0.10-0.13 has no chance there. Judge additive overlays against the
		// brightest surface they must burn over, never against the preview's backdrop.
		//
		// So the energy moves into the one channel the weapon has not already saturated. This is
		// exactly what 9017 does and why it reads as white-hot fire on a pale blue blade: the SHEET
		// is white, and the tint only decides how hard it burns. Blue tint does NOT mean blue fire.
		//   peak add, one strand : 0.26 / 0.56 / 1.00
		//   peak add, doubled    : 0.52 / 1.12 / 2.00
		// The doubling is real here -- Glass-Hangar is ZWrite Off, so nothing writes depth for the
		// overlay's ZTest to reject and the far half of the helix draws as well as the near half.
		// Blue clips where strands cross, deliberately: R and G stay moderate, so a crossing reads
		// as a hotter blue-white core rather than the flat white glare flat 0.46 would have given.
		//
		// If it still needs more in game, raise ALPHA (total energy) in 0.05 steps before touching
		// the rgb balance, and judge it over the weapon rather than off-silhouette.
		// THIRD PASS: lowered again for Shell. The blue is kept -- the reasoning above holds and is
		// corroborated by the game's own BlueLongSpark.mat, one of Jack O'Hat's four FX materials,
		// which tints (0.323, 0.370, 0.866): blue-dominant with R and G held low, exactly this
		// shape. But Shell covers the ENTIRE silhouette and double-adds at every grazing angle,
		// where Helix covered two thin ribbons, so the same numbers would glare.
		//   peak add, body : 0.20 / 0.34 / 0.60
		//   peak add, rim  : 0.40 / 0.68 / 1.20  (front and back faces both draw, ZWrite Off)
		// The rim is the effect; the body is meant to be a haze. If it needs more in game, raise
		// ALPHA in 0.05 steps and judge the RIM, then the shell scale in SkinShellScale -- in that
		// order, because scale changes the silhouette and alpha does not.
		// 9020/9021 removed with their flames -- see the note in SkinFlames.
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
		// EVERY ENTRY MUST SET ALL SEVEN FIELDS. This is a struct and the constant defaults at
		// :1811 are assigned ONLY in the not-found branch, so they are never merged into a partial
		// entry -- an omitted field is 0, not the file default. The failure modes are silent and
		// each one looks like a different bug:
		//   MaxLenFrac omitted -> Mathf.Clamp(thin*mult, len*0.025, 0) returns 0, because Unity
		//                         applies the max test last -> radius 0 -> the sleeve collapses
		//                         onto the centreline and reads as exactly the "lines" defect
		//                         these entries exist to fix.
		//   Ribbons omitted    -> nv = 0 -> a zero-vertex Mesh that is NOT null, so an empty
		//                         overlay GameObject is still built and parented.
		//   Start omitted      -> fire starts at the butt, wrapping the grip and the hand.
		//   VRepeat omitted    -> the ONE field that does fall back on its own, at :1682, because
		//                         it is read through a `> 0f` guard. Write it anyway.

		// ---- 9017 Frostbound. "Not line-looking", the geometry half of it.
		//
		// The line look had an arithmetic cause, not an aesthetic one. U runs edge-to-edge across
		// ONE ribbon, V runs along span/VRepeat, so at the shipped values the sheet is squashed
		//   ribbon width = RibbonArc 0.5 * radius 0.0451 = 0.0226 world
		//   V tile height = span 0.7520 / 2            = 0.3760 world
		// a 16.7 : 1 crush, and one of the shipped sheet's ~10 tongues lands 0.00226 wide by
		// 0.376 tall -- 1 : 167. Measured on screen: the overlay covered 1.19% of the frame in
		// 4244 separate horizontal runs whose MEDIAN WIDTH WAS 1 PIXEL. It was not line-LOOKING,
		// it was lines.
		//
		// Both levers were rendered separately at identical camera and tint, and the result is the
		// opposite of what you would guess -- GEOMETRY ALONE MAKES IT WORSE:
		//   today                     median run 1px, mean 1.80, 4244 runs/frame
		//   geometry only             median run 3px, mean 4.35, 7510 runs/frame  <- MORE lines
		//   sheet only                median run 3px, mean 4.02, 1606 runs/frame
		//   both (this entry)         median run 6px, mean 12.99, 2247 runs/frame <- tongues
		// So the new sheet in SkinFlames is the load-bearing half and this entry is the second
		// half; shipping the geometry without the sheet would be a regression.
		//
		// RadiusMult 2.0 IS INERT ON THIS WEAPON and is written down only so the entry is complete.
		// thin*2.0 = 0.0586 sits 30% ABOVE the MaxLenFrac ceiling, so the clamp always takes the
		// ceiling: radius = 1.0743 * 0.048 = 0.05157. Anyone tuning RadiusMult here will see
		// nothing change and conclude the table is broken. MaxLenFrac is the radius knob until it
		// exceeds 0.0546, where thin*RadiusMult starts binding again.
		//
		// Ribbons 3 x RibbonArc 1.5 = 4.500 rad of 6.283, i.e. 71.6% of the circumference with
		// 0.594 rad gaps -- deliberately NO self-overlap, because additive has no sorting and
		// overlapping strands would double the add into a white seam. Strip width becomes
		// 1.5 * 0.05157 = 0.0774, 3.4x today's. Twist drops 2.1 -> 1.4 so the tongues read as
		// fire winding up the blade rather than as thread wound round it. Start stays 0.30 so the
		// grip and the player's hand stay clear.
		{ 9017, new SleeveSpec { RadiusMult = 2.0f, Ribbons = 3, RibbonArc = 1.5f,
		                         Twist = 1.4f, Start = 0.30f, MaxLenFrac = 0.048f, VRepeat = 2f } },

		// ---- 9020 AWP [Permafrost] and 9021 Icebreaker. The reason the earlier attempt "looked
		// worse than none", and the fix.
		//
		// THE OLD DEFAULTS PUT THE SLEEVE INSIDE THE GUN. The radius clamp is calibrated for a
		// katana, and on a firearm len*0.042 lands well under the weapon's own cross-section:
		//                    radius (shipped)   other cross half-extent   ratio
		//   Ninja_Knife      0.045121           0.078539                  1.54  outside the steel
		//   AWP              0.062071           0.134681                  0.46  BURIED
		//   Death_Hammer     0.049263           0.115649                  0.43  BURIED
		// Rendered, the helix never once broke either weapon's outline -- it read as wire
		// scratches lying on the receiver, and on the AWP it crossed the scope. That is the
		// recorded failure, reproduced, and now explained. MaxLenFrac is raised until the sleeve
		// clears the silhouette: AWP 1.477879 * 0.068 = 0.100496 (the barrel is 0.055, and the
		// 0.137 receiver is behind Start so the sleeve never crosses it); Death Hammer
		// 0.068197 * 2.0 = 0.136394, which clears its 0.115 body and is the one case where
		// RadiusMult binds rather than MaxLenFrac.
		//
		// THE ARC IS WIDE, AND THAT IS ONLY SAFE BECAUSE OF THE SHEET. Read this together with the
		// SkinFlames note -- the two decisions are one decision.
		//
		// U tiling is a hard-coded 1f at :1685 with no SleeveSpec field, so whatever tongues the
		// sheet has ALWAYS spread across exactly RibbonArc radians. On the OLD shared sheet, with
		// ~9-10 tongues, that made arc LENGTH the thing that set tongue spacing, and the rule was
		// to hold radius * RibbonArc near the katana's 0.0226 or the strands combed apart. An
		// attempt at arc 1.1 on this rifle gave 0.111 m of arc, tongues 12 mm apart, and rendered
		// as a feather duster.
		//
		// THAT RULE DIED WITH THE SHEET. Two broad tongues cannot comb apart -- there is nothing
		// to comb. Held at the old 0.23 / 0.17 the new sheet still looked like hairlines, for the
		// simpler reason that a 0.023-wide ribbon on a 1.478-long rifle is 1.6% of the weapon and
		// is thin no matter what is painted on it. So the constraint that replaces it is the
		// ribbon's WIDTH RELATIVE TO THE WEAPON, matched to the katana that reads correctly:
		//   Ninja_Knife  arc 1.5  * r 0.051566 = 0.0774 on len 1.074301 = 7.2% of length
		//   AWP          arc 1.05 * r 0.100496 = 0.1055 on len 1.477879 = 7.1%
		//   Death_Hammer arc 0.62 * r 0.136394 = 0.0846 on len 1.172920 = 7.2%
		// SECOND PASS, 2026-08-17, after the first was judged IN GAME and rejected as "no flames at
		// all, just lines". The 7.1/7.2% match above was the right idea measured against the wrong
		// reference. It matched the katana's ribbon width as a fraction of weapon LENGTH -- but what
		// decides whether a strand reads as a tongue or a wire is how much BURNING SURFACE faces the
		// camera at a given moment, and on a 2-ribbon helix at 33% of the circumference most of that
		// surface is turned away. The katana gets away with it because it is a flat blade seen
		// broadside; a rifle is a cylinder and is not.
		//
		// So both weapons go to 3 ribbons and a wider arc. Width relative to length rises with it,
		// deliberately -- the previous pass's 7.2% ceiling was never a constraint, only a copy of
		// the katana:
		//   AWP          arc 1.30 * r 0.100496 = 0.1306 on len 1.477879 =  8.8%  (was 7.1)
		//   Death_Hammer arc 0.95 * r 0.136394 = 0.1296 on len 1.172920 = 11.0%  (was 7.2)
		//
		// STILL NO SELF-OVERLAP, which is the one hard limit -- additive has no sorting and crossing
		// strands double the add into a seam:
		//   AWP          3 * 1.30 = 3.90 rad of 6.283 = 62% of circumference, gaps 0.794 rad
		//   Death_Hammer 3 * 0.95 = 2.85 rad of 6.283 = 45% of circumference, gaps 1.144 rad
		// Sleeve cost goes 264v/420t -> 396v/630t (RINGS 22 * (ARC_SEGMENTS 5 + 1) * 3 ribbons).
		//
		// THE RECORDED RISK, so it is not rediscovered: an earlier attempt at arc 1.1 on this rifle
		// rendered as "a feather duster". That was on the OLD shared sheet, whose ~9-10 narrow
		// tongues combed apart as arc length grew. The new sheet has 2 broad lobes and nothing to
		// comb, which is what makes a wider arc safe now -- read this with the SkinFlames note, the
		// two decisions are still one decision. If it DOES read as a duster, the sheet is the thing
		// that changed, so suspect the sheet before the arc.
		//
		// Start is a silhouette decision on each weapon and is UNCHANGED: 0.55 on the AWP puts the
		// sleeve entirely FORWARD of the receiver (local z 0.4871, exactly where the barrel begins)
		// and ends it at the muzzle without overshoot -- do not lower it, it is what keeps the far
		// strand off the scope. 0.38 on the Death Hammer (z 0.1299) is forward of the pump and clear
		// of the hand. Widening the arc does not move either, because arc grows around the axis and
		// Start is along it. VRepeat 3 on both: shorter, more frequent tongues suit a 0.67-0.73 span
		// better than the katana's 2.
		{ 9020, new SleeveSpec { RadiusMult = 2.1f, Ribbons = 3, RibbonArc = 1.30f,
		                         Twist = 1.2f, Start = 0.55f, MaxLenFrac = 0.068f, VRepeat = 3f } },
		{ 9021, new SleeveSpec { RadiusMult = 2.0f, Ribbons = 3, RibbonArc = 0.95f,
		                         Twist = 1.0f, Start = 0.38f, MaxLenFrac = 0.118f, VRepeat = 3f } },

		// NOT REACHABLE FROM HERE, and every one of them would move 9017 too if edited: RINGS 22,
		// ARC_SEGMENTS 5, TIP_SCALE 0.55, MIN_LEN_FRAC 0.025 are file-level constants. Every value
		// above was deliberately kept inside the seven overridable fields for that reason.
		// ARC_SEGMENTS in particular is not worth a field: rendered at 5 vs 8 vs 10 with everything
		// else identical, the whole-frame MAE was 0.152/255 and 0.155/255.
	};

	public static readonly Dictionary<int, string> SkinFlames = new Dictionary<int, string>
	{
		// 2026-08-17. A SECOND FLAME SHEET, and the separate file is the point: the old one is
		// still named by 9018, 9019 and 9026-9029, so editing it in place would have changed all
		// seven. Do NOT "tidy" this back to the shared name.
		//
		// Shared by the three HELIX skins (9017 here, 9020/9021 below) and by nothing else -- the
		// split is by MODE, not by look. A helix samples U edge-to-edge across a ribbon a few
		// centimetres wide; a Surface overlay samples the weapon's own UV islands. The two want
		// opposite things from the same image, which is why one sheet could never serve both.
		//
		// The new sheet has TWO broad tongues across the 1024 width where the old one has about
		// ten narrow ones. That single number is what turns the overlay from thread into fire --
		// see the measurement in SkinSleeves, where the sheet lever beats the geometry lever
		// outright and the geometry lever ALONE makes things worse.
		//
		// Three properties of it are forced by how the sleeve samples, not by taste, and any
		// repaint has to keep them:
		//
		//   * COLUMNS 0 AND 1023 ARE BLACK. U runs edge-to-edge across ONE ribbon, so those two
		//     columns land exactly on the strand's two silhouette edges. The shared sheet is
		//     BRIGHT there (column means 0.2305 and 0.2157), which gives every ribbon two
		//     dead-straight bright edges -- a second, independent cause of the line look. Forced
		//     to 0.0000 here, so the strand's edge is the flame's own soft edge.
		//
		//   * NOTHING SMALLER THAN ~150 SHEET PIXELS. The sheet has no mipmaps and the strand
		//     lands on roughly 30-40 screen pixels, so a 50px feature is ~1.5 screen pixels --
		//     a line again. At this magnification fine filigree cannot be drawn, only aliased.
		//
		//   * IT IS PAINTED WHITE, not blue. Particles/Additive computes 2 * tint * texture, so
		//     SkinFlameTints does the colour; a blue sheet would multiply blue twice and go navy.
		//
		// Top and bottom rows are black so the V wrap (VRepeat 2) never shows as a band, matching
		// the shared sheet's behaviour. Ships as a plain .png with NO .jpg and NO .alpha.png twin,
		// deliberately: LoadSkinFile tries "<stem>.jpg" BEFORE the name asked for, so a jpg twin
		// would silently win, and an .alpha.png twin would be merged into RGBA and then dim the
		// flames per texel through Blend SrcAlpha One.
		{ 9017, "9017_Frostbound_BlueFlames.png" },
		// 9018 Frostfire deliberately KEEPS the shared sheet. It is SURFACE mode, where the
		// overlay samples the WEAPON's own UVs rather than a ribbon's -- so the two-tongue layout
		// above, which is calibrated to a 0.077-wide strip, means nothing there, and the request
		// that produced the new sheet named Frostbound. One line to change if that is wanted.
		{ 9018, "9017_Frostbound_Flames.png" },
		// Shares the flame sheet with the ice skins on purpose: it is generic white fire on pure
		// black that tiles vertically, and white fire over red glass is the contrast that sells
		// this skin. Red flames on a red blade would mostly disappear.
		{ 9019, "9017_Frostbound_Flames.png" },
		// 2026-08-17. 9020 and 9021 GAIN outer white flames, on the third attempt.
		//
		// Surface is still impossible on them -- AWP.asset and Death_Hammer.asset are both
		// m_IsReadable: 0, so the overlay cannot copy the weapon's geometry -- and their
		// SkinFlameModes rows say Helix explicitly rather than leaning on the default.
		//
		// The two earlier attempts failed on LOOKS, and both failures now have a measured cause
		// written down in SkinSleeves: the shipped clamp put the sleeve INSIDE the gun (radius
		// 0.46x and 0.43x the weapon's own cross-section, against 1.54x on the katana that works),
		// and widening the ribbon to compensate spread the shared sheet's ~9 tongues over 0.111 m
		// and produced the feather duster. Raising MaxLenFrac and NARROWING the arc fixes both,
		// and the per-skin SleeveSpec entries this table's note pointed at are what made it
		// expressible.
		//
		// THEY TAKE 9017's NEW SHEET, not the shared one, and this was decided at the preview
		// rather than assumed. Rendered on the shared sheet with the geometry above, both weapons
		// came back as a BUNDLE OF PARALLEL HAIRLINES lying along the barrel -- visibly the same
		// "five narrow strands read as combed fibre" this table already records as rejected, just
		// with the strands now outside the gun instead of inside it. The geometry fix alone was
		// not enough; the sheet was the other half here exactly as it was on 9017.
		//
		// The sheet is painted WHITE (see the note on 9017 above) and SkinFlameTints does the
		// colour, so these two get white fire from the same file that gives 9017 blue. The name
		// records which skin commissioned it, not what colour it is.
		// 2026-08-17. 9020 and 9021 REMOVED FROM FLAMES ALTOGETHER, and this is the third and
		// final rejection -- do not add a fourth. Helix was judged in game twice ("wire
		// scratches on the receiver", then "no flames at all, just lines"), and Shell was
		// built and judged after that. QA then changed direction entirely: what they asked for
		// instead is a matte see-through variant, which shipped as 9035/9036 and involves no
		// overlay at all. These two are back to the state they originally shipped in, which is
		// the state nobody has ever complained about.
		// The SkinSleeves and SkinShellScale entries are LEFT IN PLACE deliberately: they cost
		// nothing when unreferenced and they are the record of what was tried and rejected.
		// 9022-9025 [Watery] have NO flames. REMOVED 2026-08-17 -- they were registered here,
		// and in SkinFlameModes, from the day the set shipped. DO NOT RE-ADD THEM.
		//
		// They produced nothing in game, but only by accident, and that accident has just been
		// fixed. ApplyFlames used to enumerate MeshFilter, and MachineGun.prefab has none -- its
		// body is a single SkinnedMeshRenderer -- so the loop body never executed once for 9022
		// and the registration was invisible. ApplyFlames now enumerates Renderer (see the
		// measurement there), so on the next build 9022 WOULD have started burning. A skin that
		// ships fire-free because of a bug is not a decision; this is the decision.
		//
		// Two reasons, in order of weight:
		//
		//   1. A WATER skin configured for FIRE is a contradiction QA and players both have to
		//      explain away. These four bind CMune/Water/Opaque_Flowing and are named [Watery];
		//      flowing water with flames licking over it reads as a bug report, not as a look.
		//
		//   2. It was barely visible anyway, MEASURED rather than asserted: on 9022 the overlay
		//      touched 3.9% of the weapon's pixels at MAE 1.59. The cause is structural, not
		//      tuning -- 9022 has no SkinFlameTints entry, so it falls to the dim Surface default
		//      (0.18, 0.26, 0.32) at :1667, a peak add of 0.36/0.52/0.64 laid over a water
		//      surface that is already bright and already moving. Making it visible would mean a
		//      hot per-skin tint, which is reason 1 again but louder.
		//
		// 9026-9029 [Frosted] below deliberately KEEP theirs. Frost over fire is the language
		// 9018 Frostfire already ships, and 9026 is the skin that GAINS visible flames from the
		// Renderer fix -- that one is the point of the fix, not a casualty of it.
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
		{ 9011, "9011_HazardousShotgun_Icon.png" },
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
		//
		// Both FILES were replaced 2026-08-17 18:58 (the .pre-glass copies beside them are the
		// originals). The names and therefore these two rows did not change, which is exactly why
		// it is written down: nothing in this table can show that the bytes moved. The originals
		// were painted-RGB renders, and these two bind Unique/Transparent/Glass-Hangar, whose
		// program never samples the painted RGB at all -- so the old icons showed a surface the
		// player has never seen. The current ones render the shader the client actually runs,
		// including its alpha. A DLL built before 18:58 embeds the OLD bytes under the RIGHT names.
		{ 9020, "9020_Permafrost_Icon.png" },
		{ 9021, "9021_Icebreaker_Icon.png" },
		{ 9035, "9035_AWPMatteGlass_Icon.png" },
		{ 9036, "9036_IcebreakerMatteGlass_Icon.png" },
		{ 9038, "9038_AWPClearIce_Icon.png" },
		{ 9039, "9039_IcebreakerClearIce_Icon.png" },
		// The [Gold] set. Icons exist now: three manifest entries were authored in
		// uberstrike-patcher-workshop (framing inherited by ItemClass -- class 3 takes the
		// MachineGun camera, class 4 the ShotGun camera, class 5 the Sniper camera, the same rule
		// the 9020/9021 notes state) and the three missing meshes were copied into meshes471,
		// because no skin had ever existed on M4A1, AK-47 or SPAS-12.
		{ 9040, "9040_M4A1Gold_Icon.png" },
		{ 9041, "9041_AK47Gold_Icon.png" },
		{ 9042, "9042_SPAS12Gold_Icon.png" },
		{ 9043, "9043_AWPGold_Icon.png" },
		{ 9044, "9044_M4A1Chrome_Icon.png" },
		{ 9045, "9045_AK47Chrome_Icon.png" },
		{ 9046, "9046_SPAS12Chrome_Icon.png" },
		{ 9047, "9047_AWPChrome_Icon.png" },
		{ 9048, "9048_M4A1Damascus_Icon.png" },
		{ 9049, "9049_AK47Damascus_Icon.png" },
		{ 9050, "9050_SPAS12Damascus_Icon.png" },
		{ 9051, "9051_AWPDamascus_Icon.png" },
		{ 9056, "9056_M4A1Carbon_Icon.png" },
		{ 9057, "9057_AK47Carbon_Icon.png" },
		{ 9058, "9058_SPAS12Carbon_Icon.png" },
		{ 9059, "9059_AWPCarbon_Icon.png" },
		{ 9060, "9060_M4A1Tempered_Icon.png" },
		{ 9061, "9061_AK47Tempered_Icon.png" },
		{ 9062, "9062_SPAS12Tempered_Icon.png" },
		{ 9063, "9063_AWPTempered_Icon.png" },
		{ 9064, "9064_M4A1ChromeMax_Icon.png" },
		{ 9065, "9065_AK47ChromeMax_Icon.png" },
		{ 9066, "9066_SPAS12ChromeMax_Icon.png" },
		{ 9067, "9067_AWPChromeMax_Icon.png" },
		{ 9037, "9037_FrostSerpent_Icon.png" },
		{ 9079, "9079_AWPUberverse_Icon.png" },
		{ 9080, "9080_CyberNeon_Icon.png" },
		{ 9081, "9081_ToxicVenom_Icon.png" },
		{ 9082, "9082_MoltenInferno_Icon.png" },
		{ 9083, "9083_AWPUberverseV1_Icon.png" },   // V1: procedural gas-giants, wide orbit
		{ 9084, "9084_AWPUberverseV12_Icon.png" },  // V1.2: same worlds, tight orbit
		{ 9085, "9085_WreckerVoidglass_Icon.png" },
		{ 9086, "9086_SplattergunPrismSplatter_Icon.png" },
		{ 9087, "9087_LauncherDragonsMaw_Icon.png" },
		// Framing inherited per BASE weapon, not chosen: each of these takes the camera of the
		// shipped skin on the same weapon (9008 for the MG, 9012 sniper, 9013 shotgun, 9014
		// cannon), which is what keeps a family of icons looking like a set. The stock icons
		// work the same way -- silhouette overlap between variants of one weapon measures
		// 0.957-0.979 IoU, i.e. one camera per weapon and only the texture changes.
		{ 9022, "9022_MGWatery_Icon.png" },
		{ 9023, "9023_SniperWatery_Icon.png" },
		{ 9024, "9024_ShotgunWatery_Icon.png" },
		{ 9025, "9025_CannonWatery_Icon.png" },
		{ 9026, "9026_MGFrosted_Icon.png" },
		{ 9027, "9027_SniperFrosted_Icon.png" },
		{ 9028, "9028_ShotgunFrosted_Icon.png" },
		{ 9029, "9029_CannonFrosted_Icon.png" },
		// 9030-9033 [Lava]. THIS IS THE ONE TABLE THE [Lava] SET NEEDS ART FOR.
		//
		// They deliberately have no SkinTextures entry -- their material is bound from Resources
		// and their painted texture would be overwritten in the same frame (see LavaBindings).
		// An ICON is a different asset with a different job: a 48x48 render rather than the
		// skin's texture, reached by a separate path -- ProxyItem.cs:62 -> GetIconTexture, which
		// reads THIS dictionary and nothing else -- so no amount of correct material binding
		// produces one. "No art needed" is true of the weapon and false of the shop.
		//
		// All four verified present in WeaponSkins/ at 48x48 RGBA before being registered, and
		// registered only because they are: an entry naming a file that does not exist makes
		// LoadSkinFile log an error (:1037) on EVERY ProxyItem construction, because _iconCache
		// only ever caches a successful load.
		//
		// Framing is inherited per BASE weapon exactly as the [Watery]/[Frosted] rows above --
		// 9008's camera for the MG, 9012's sniper, 9013's shotgun, 9014's cannon.
		{ 9030, "9030_MGLava_Icon.png" },
		{ 9031, "9031_SniperLava_Icon.png" },
		{ 9032, "9032_ShotgunLava_Icon.png" },
		{ 9033, "9033_CannonLava_Icon.png" },
		// 9034 Neon Circuit [Black]. This row was WITHHELD for one round and is now registered;
		// the note is kept as the record of both halves, because the reason it was withheld is a
		// rule that still holds and the reason it is here now is a fact that can be re-checked.
		//
		// WITHHELD 2026-08-17 (earlier the same day): the icon had never been rendered. The only
		// renderer for these, tools/render_weapon_icon.py, lived in uberstrike-patcher-workshop,
		// outside that round's scope, so no file existed. Registering the row anyway would have
		// been strictly worse than leaving it out -- the rule a few lines up is not decoration: an
		// entry naming a file that does not exist makes LoadSkinFile log an error on EVERY
		// ProxyItem construction, forever, because _iconCache only ever caches a SUCCESSFUL load,
		// so the miss is retried for every item every time the shop builds. Absent, the skin fell
		// back to the base MachineGun's icon path, the same quiet behaviour the five stock weapons
		// have -- quiet being the problem: the shop showed a generic class icon and logged nothing.
		//
		// REGISTERED 2026-08-17 18:58, because the file now exists and was verified before the row
		// was added: WeaponSkins/9034_NeonCircuitBlack_Icon.png, 48x48 RGBA, 3100 bytes, alpha
		// (255,255) with 0 partial-alpha pixels, i.e. opaque as the convention above requires.
		//
		// It was rendered INSIDE a permitted repo, which is what changed: webgl-skins-rendering's
		// tools/render_premium_icons.py CALLS render_weapon_icon.render() rather than
		// reimplementing it, and supplies 9015's manifest entry with 9034's texture. So the framing
		// is inherited per BASE WEAPON exactly as every row above -- 9015's MachineGun camera, not
		// 9008's, though their framing keys are identical, so that a future re-tune moves both Neon
		// Circuits together. The one value chosen rather than fetched is a LIGHTING lift: 9034's
		// body measures mean luminance 30.6 against 9015's 133.1 on the same mesh at the same
		// camera, close enough to the plate's own 27-34 that at 9015's rig only 75.8% of the
		// footprint cleared the plate and the barrel and receiver read as absent. Raised with
		// ambient, not texture gain; the shipped skin's art is untouched.
		//
		// render_premium_icons.py --selftest re-checks THIS row by parsing this dictionary, and
		// fails loudly ("IconTextures[9034] NOT REGISTERED") if it is ever dropped again.
		{ 9034, "9034_NeonCircuitBlack_Icon.png" },
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

	/// <summary>
	/// Optional per-skin recolour of the weapon's own muzzle effects.
	///
	/// READ THE ISOLATION ARGUMENT BEFORE ADDING A ROW. Custom muzzle FX was tried across the
	/// whole set once before and reverted, and the recoverable history says the thing that was
	/// disliked was a BUG rather than a feature: ApplyToWeapon used to assign the skin texture to
	/// every child Renderer, so the gun's diffuse landed on the additive muzzle quad and the flash
	/// rendered as a bright rectangle of UV atlas. There is no prior per-skin mechanism to
	/// resurrect and no prior tuning to inherit -- this is new, and it is deliberately narrow.
	///
	/// Every field here is written through a PER-COMPONENT property, never through a material:
	///
	///   * Light.color is a component field. MuzzleLightShining.anim animates m_Intensity and
	///     m_Range and NEVER colour (classID 108, checked), so the animation and this assignment
	///     do not fight, and no shared asset is involved at all.
	///
	///   * ParticleSystem.startColor is a component property that multiplies the material
	///     per-particle. That matters more than it looks: FireBall.mat is referenced by THIRTY-ONE
	///     prefabs -- every AK47, M4, AWP, Beretta, ExplosiveShotgun, SniperMSR and the cannon
	///     explosion -- and Flare_Flare.mat by all five AWP variants. A sharedMaterial.SetColor
	///     here would repaint every one of them for every player in the match. startColor touches
	///     neither. If anyone ever "simplifies" this into a material write, that is the damage.
	///
	/// Applied once at skin time rather than at fire time, which is enough: startColor affects
	/// particles emitted AFTER the write, and this runs long before the first shot. It does mean
	/// the tint is lost if anything re-instantiates the particle system.
	/// </summary>
	public struct MuzzleTintSpec
	{
		public bool HasLight;
		public Color LightColour;
		public bool HasParticles;
		public Color ParticleTint;
		/// <summary>Exact child GameObject names to tint. Named, not "all", on purpose.</summary>
		public string[] ParticleObjects;
		/// <summary>
		/// Renderers to tint through their material's _TintColor, by exact GameObject name.
		///
		/// THIS IS THE ONE THAT ACTUALLY SHOWS ON THE AWP, and it took four builds and three
		/// in-game tests to establish, all logged. The prefab reading said the muzzle FX was
		/// "a light plus four particle systems", so the first three attempts tinted Sfx and
		/// Spark. Both exist, both are Particles/Additive, both took the tint -- and nothing
		/// changed on screen, because THE RUNNING GAME HAS NO MuzzleParticleSystem COMPONENT
		/// ON EITHER OF THEM. Enumerating BaseWeaponEffect under the live weapon returns only
		/// WeaponShootAnimation, MuzzleLight and BulletTrail, so nothing ever calls Play() on
		/// those two emitters and they never emit a particle.
		///
		/// BulletTrail (on SplatterTrail, material Particles/Additive) is the effect that
		/// actually fires: OnShoot enables its renderers, plays a clip, and a coroutine
		/// disables them ~0.1s later. That short additive flash at the muzzle is what a player
		/// sees and calls the muzzle flash.
		/// </summary>
		public string[] TintRenderers;
	}

	public static readonly Dictionary<int, MuzzleTintSpec> MuzzleTints = new Dictionary<int, MuzzleTintSpec>
	{
		// 9020 AWP [Permafrost]. AWP_Roughed has NO muzzle flash quad at all -- there is no
		// MuzzleFlash script anywhere on it. Its muzzle FX is one Light plus four particle
		// systems: Sfx (FireBall.mat), Spark (Flare_Flare.mat), AWPGunSmoke and AWPBigSmoke.
		//
		// The light is the highest-impact, lowest-risk half and probably carries the whole read on
		// its own: it is a point light at intensity 8 / range 2 that fires on every shot, and the
		// stock colour (1, 0.861, 0.731) is distinctly warm. Cooling it swings the whole muzzle
		// area blue for the duration of the flash.
		//
		// ONLY Sfx AND Spark ARE TINTED, and the omission is a decision. Blue smoke reads as a
		// rendering bug rather than as a skin, and AWPGunSmoke/AWPBigSmoke are also the two on the
		// most widely shared materials.
		//
		// THE BLUE EXCEEDS 1.0 BECAUSE THE TEXTURES ARE WARM, exactly as TracerSpec.MatTint above
		// documents for the trail: startColor MULTIPLIES the particle's texture, fire_ball.png
		// averages RGB (246, 202, 141) and Flare_Sparks_TexS.png (93, 64, 36), so an even-handed
		// blue comes out teal. Channels are compensated roughly target/texture. Unity allows >1
		// for colours. This is a starting point for tuning in the real client, not a measured
		// result -- see the note in ApplyMuzzleTint about why the studio cannot preview it.
		{ 9020, new MuzzleTintSpec {
			HasLight        = true,
			LightColour     = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles    = true,
			ParticleTint    = new Color(0.20f, 0.62f, 1.70f, 1f),
			ParticleObjects = new string[] { "Sfx", "Spark" },
			TintRenderers   = new string[] { "SplatterTrail" } } },
		// 9037 asks for the same blue muzzle FX, and it is the SAME base weapon (AWP_Roughed), so
		// the same object names and the same two levers apply. Written out rather than sharing
		// 9020's instance because these are two independent skins that happen to agree today --
		// tuning one must not silently move the other.
		// 2026-08-17, extended to the rest of the cold set. Requested: the two other AWP skins,
		// Cryo Strike, Sniper [Watery], Shotgun [Watery] and Icebreaker.
		//
		// All six share ONE tint deliberately -- they are the same idea on six weapons, and six
		// copies of two colours would be six things to keep in sync. ParticleObjects is left null
		// on these: the generic component-type pass finds each weapon's own effect renderers, and
		// naming children per weapon is exactly what did not scale.
		//
		// 9021 Icebreaker is the honest partial. DeathHammer's muzzle visual is a MuzzleFlash,
		// whose shared material has an ANIMATED alpha, so it is excluded by the pass above and
		// its flash stays stock. Its light and any trail still tint. Do not "fix" this by adding
		// MuzzleFlash to the include list -- that is commit 63a9776's regression, and the flash
		// disappears entirely rather than turning blue.
		{ 9009, new MuzzleTintSpec {   // Cryo Strike        (PaintSniper)
			HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } },
		{ 9021, new MuzzleTintSpec {   // Icebreaker         (DeathHammer -- light + trail only)
			HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } },
		{ 9023, new MuzzleTintSpec {   // Sniper [Watery]    (PaintSniper)
			HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } },
		{ 9024, new MuzzleTintSpec {   // Shotgun [Watery]   (PaintShotty)
			HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f) } },
		{ 9035, new MuzzleTintSpec {   // AWP [Matte Glass]  (AWP_Roughed)
			HasLight = true, LightColour = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles = true, ParticleTint = new Color(0.20f, 0.62f, 1.70f, 1f),
			TintRenderers = new string[] { "SplatterTrail" } } },
		{ 9037, new MuzzleTintSpec {
			HasLight        = true,
			LightColour     = new Color(0.45f, 0.72f, 1.00f, 1f),
			HasParticles    = true,
			ParticleTint    = new Color(0.20f, 0.62f, 1.70f, 1f),
			ParticleObjects = new string[] { "Sfx", "Spark" },
			TintRenderers   = new string[] { "SplatterTrail" } } },
		{ 9079, new MuzzleTintSpec {   // AWP [Uberverse]  (AWP_Roughed) -- CYAN muzzle
			HasLight        = true,
			LightColour     = new Color(0.20f, 0.85f, 1.00f, 1f),
			HasParticles    = true,
			ParticleTint    = new Color(0.30f, 1.30f, 1.60f, 1f),
			ParticleObjects = new string[] { "Sfx", "Spark" },
			TintRenderers   = new string[] { "SplatterTrail" } } },

		// 9021 Icebreaker IS DELIBERATELY ABSENT, and this is the record of why -- it was asked
		// for in the same breath as 9020 and is a completely different problem.
		//
		// DeathHammer's muzzle FX is ONLY a flash quad: SGMuzzleFlash20, a MeshRenderer plus an
		// Animation plus the MuzzleFlash script. No Light, no ParticleSystem. So neither lever
		// above exists on it, and the only thing left to change is the material -- which is where
		// it becomes dangerous:
		//
		//   * SGMuzzleFlash.mat is referenced by FOUR prefabs, and one of them is the STOCK
		//     ShotGun (PaintShotty, the base of skins 9011/9013/9024/9028/9032). A sharedMaterial
		//     write leaks blue onto every player's stock shotgun.
		//
		//   * A `.material` write reproduces a bug this file has already paid for twice. The
		//     Animation drives _TintColor.a on the SHARED material asset, from 1.0 to 0.0 over
		//     0.0833 s, and BaseWeaponDecorator's startup Hide() leaves it parked at 0. So reading
		//     .material clones a material FROZEN AT ALPHA 0 on an additive shader -- it contributes
		//     exactly nothing, forever. That is the "renders as NOTHING rather than the stock
		//     flash" note in ApplyToWeapon, and it is one careless line from coming back.
		//
		// The safe route is a MaterialPropertyBlock overriding _MainTex with a pre-graded blue copy
		// of MuzzleSmoke.png, which never changes WHICH Material object the renderer uses. Not
		// done, for two reasons: that 128x128 texture does not exist on disk, and a grep for
		// MaterialPropertyBlock/SetPropertyBlock across the whole client returns ZERO hits -- the
		// game never uses that API, and Unity 4.x property-block texture overrides were
		// historically the least reliable corner of it. "Present in UnityEngine.dll metadata" is
		// not "works on this renderer in this build", and it cannot be verified from the studio.
		// Ship it only after a smoke test in the real client, and if it disappoints, add a small
		// blue Light under MuzzlePosition mirroring the flash's enabled state -- do NOT escalate
		// to a material write.
	};

	private static readonly Dictionary<int, Texture2D> _skinCache = new Dictionary<int, Texture2D>();
	private static readonly Dictionary<int, Texture2D> _iconCache = new Dictionary<int, Texture2D>();

	/// <summary>
	/// Folder holding the skin PNGs, relative to <c>UberStrike_Data</c>.
	/// Distributed through the patcher's Entry.txt like any other game file.
	/// </summary>
	public const string SkinFolder = "Skins";

	/// <summary>Prefix for the embedded resource names, set via LogicalName in the csproj.</summary>
	private const string ResourcePrefix = "WeaponSkins.";

	/// <summary>Absolute path of a skin file, for logging and for the patcher's manifest.</summary>
	public static string SkinPath(string fileName)
	{
		// Application.dataPath is "<install>/UberStrike_Data" for a Windows player.
		return Path.Combine(Path.Combine(Application.dataPath, SkinFolder), fileName);
	}

	// ------------------------------------------------------------------ byte sources

	/// <summary>
	/// Read a skin file: loose file on disk first, then the copy embedded in this assembly.
	/// Returns null if neither exists.
	///
	/// Both paths are needed and they are not redundant.
	///
	/// The embedded copy is what makes "clone, compile, run" work with no deployment step.
	/// Shipping only the DLL was the defect behind "all skins PR doesn't work": the code
	/// landed, the art did not, and a missing skin file is SILENT - the weapon simply renders
	/// stock, which is indistinguishable from the patch not working at all.
	///
	/// The disk override still wins when present, which is what keeps the patcher useful:
	/// art can be updated without re-shipping code, and Deploy-WeaponSkins.ps1 keeps working
	/// exactly as before. It also means a build with the resources stripped
	/// (-p:EmbedSkins=false) is still fully functional against a deployed Skins folder.
	/// </summary>
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

				// Read in a loop rather than one Read call: Stream.Read is permitted to return
				// fewer bytes than asked for, and Stream.CopyTo does not exist on the .NET 3.5
				// profile this client compiles against.
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

	/// <summary>
	/// Load a skin texture, from a loose file if one is deployed and from the embedded copy
	/// otherwise. See ReadSkinBytes for why both exist.
	///
	/// The art was embedded once before and removed, because a plain EmbeddedResource list took
	/// Assembly-CSharp.dll from 1.4 MB to 51 MB - 97 percent of the file was PNG - so every
	/// code-only fix re-shipped 50 MB of art. That objection is answered here by the
	/// EmbedSkins property rather than by dropping the resources: build with
	/// -p:EmbedSkins=false and the assembly is code-only at ~1.4 MB, loading art from the
	/// deployed Skins folder exactly as before. The default build embeds, so a fresh clone
	/// compiles and runs with skins visible and no deployment step.
	///
	/// Texture2D.LoadImage handles PNG and JPEG on this client (Unity 4.6.5), and produces
	/// a texture with mipmaps disabled, matching the previous embedded behaviour exactly.
	/// </summary>
	private static Texture2D LoadSkinFile(string fileName)
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
		string jpegName = stem + ".jpg";
		string maskName = stem + ".alpha.png";

		byte[] colourBytes = ReadSkinBytes(jpegName);
		if (colourBytes != null)
		{
			Texture2D colour = DecodeTexture(colourBytes, jpegName);
			if (colour == null)
				return null;

			byte[] maskBytes = ReadSkinBytes(maskName);
			if (maskBytes == null)
				return colour; // colour-only skin, e.g. one with no specular mask

			Texture2D maskTex = DecodeTexture(maskBytes, maskName);
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
			// Uberverse (9079) is a starfield-heavy skin whose fine detail aliases/shimmers under
			// camera motion and scope zoom WITHOUT mipmaps. Enable mipmaps + trilinear for it ONLY,
			// so the other skins keep their exact shipped (mip-free) behaviour unchanged.
			bool uberverseMips = stem != null && stem.IndexOf("Uberverse", StringComparison.OrdinalIgnoreCase) >= 0;
			Texture2D merged = new Texture2D(colour.width, colour.height, TextureFormat.RGBA32, uberverseMips);
			Color[] rgb = colour.GetPixels();
			Color[] a = maskTex.GetPixels();
			for (int i = 0; i < rgb.Length; i++)
				rgb[i].a = a[i].r; // greyscale mask: any channel carries the value
			merged.SetPixels(rgb);
			merged.Apply(uberverseMips);
			if (uberverseMips)
				merged.filterMode = FilterMode.Trilinear;
			return merged;
		}

		byte[] single = ReadSkinBytes(fileName);
		if (single == null)
		{
			Debug.LogError("WeaponSkinHelper: skin file not found on disk (" + SkinPath(fileName)
				+ ") and not embedded as \"" + ResourcePrefix + fileName + "\"");
			return null;
		}
		return DecodeTexture(single, fileName);
	}

	private static Texture2D DecodeTexture(byte[] data, string label)
	{
		if (data == null)
			return null;

		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		if (!tex.LoadImage(data))
		{
			Debug.LogError("WeaponSkinHelper: Texture2D.LoadImage failed for " + label);
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

		Texture2D tex = LoadSkinFile(resourceName);
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

		Texture2D tex = LoadSkinFile(resourceName);
		if (tex != null)
			_iconCache[itemId] = tex;
		return tex;
	}

	// True for any renderer belonging to one of our procedural FX (Uberverse V1/V1.2/V2 and the
	// three themed auras). ApplyToWeapon must never repaint these with the gun's diffuse atlas.
	private static bool IsEffectRenderer(Renderer r)
	{
		return UberverseWeaponEffect.Owns(r) || UberverseV1Effect.Owns(r) || UberverseV12Effect.Owns(r)
			|| WeaponEmitterEffect.Owns(r);
	}

	// Called from Avatar.AssignWeapon right after a weapon is attached to a player.
	public static void ApplyToWeapon(GameObject weaponRoot, int itemId)
	{
		if (weaponRoot == null)
			return;

		// Independent of diffuse delivery: attaches the AWP [Uberverse] orbital-system FX for
		// item 9079, and removes it on pooled-root skin changes. Safe no-op for every other id.
		UberverseWeaponEffect.Apply(weaponRoot, itemId);
		// V1 (9083) and V1.2 (9084): the earlier procedural-sphere Uberverse FX, kept selectable
		// alongside V2 so the team can compare. Each is a no-op for every id but its own.
		UberverseV1Effect.Apply(weaponRoot, itemId);
		UberverseV12Effect.Apply(weaponRoot, itemId);
		// Themed auras for the three new AWP skins. Same contract: attach on match, tear down on
		// switch, no-op otherwise.
		CyberNeonWeaponEffect.Apply(weaponRoot, itemId);
		ToxicVenomWeaponEffect.Apply(weaponRoot, itemId);
		MoltenInfernoWeaponEffect.Apply(weaponRoot, itemId);
		// New weapons' auras (attach to their own meshes, not the AWP): Voidglass on the Wrecker,
		// Prism Splatter on the Splattergun. No-op for every other id.
		VoidglassWeaponEffect.Apply(weaponRoot, itemId);
		PrismSplatterWeaponEffect.Apply(weaponRoot, itemId);
		DragonsMawWeaponEffect.Apply(weaponRoot, itemId);

		// Before the texture check: the tracer is independent of whether this item has
		// a skin registered, so an item could have one without the other.
		ApplyTracer(weaponRoot, itemId);
		// Same placement, same reason: a muzzle recolour is independent of whether this item has
		// painted art, and both hooks -- WeaponSlot.cs:106 for first person and Avatar.cs:178 for
		// third -- come through here, so one line covers both. Forgetting one of those two is
		// exactly how a skin ends up correct for everyone except the player holding it.
		ApplyMuzzleTint(weaponRoot, itemId);

		// A skin qualifies if it has painted art OR a material-bindings table. The second half
		// exists for the [Watery] set: they bind the water shader, whose _MainTex is a normal
		// map from Resources, so they never sample their painted PNG at all.
		//
		// Without this they would be hostage to art they do not use -- a missing 9022_MGWatery
		// PNG would return null here, this method would return before ApplyShaderOverride ever
		// ran, and the weapon would render STOCK with the water bind never attempted. That is
		// the same silent failure as "the code landed, the art did not" in LoadSkinFile's note,
		// and it would be especially misleading here because the art is not the point.
		Texture2D tex = GetSkinTexture(itemId);
		bool hasBindings = SkinMaterialBindings.ContainsKey(itemId);
		if (tex == null && !hasBindings)
			return; // not one of our skins, leave the weapon alone

		Renderer[] renderers = weaponRoot.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in renderers)
		{
			if (r == null)
				continue;
			// Never repaint the procedural FX renderers with the gun's diffuse atlas.
			if (IsEffectRenderer(r))
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

			// Guarded only for the bindings-without-art case above. For all 18 painted skins
			// tex is non-null and this is byte-for-byte the behaviour it always had.
			if (tex != null)
			{
				r.material.mainTexture = tex;
				if (r.material.HasProperty("_MainTex"))
					r.material.SetTexture("_MainTex", tex);
			}

			ApplyShaderOverride(r, itemId);
		}

		ApplyFlames(weaponRoot, itemId);
	}

	/// <summary>
	/// Swap this renderer's shader, and assign whatever material state the new shader needs.
	///
	/// Renderer.material is already a per-instance copy, so assigning a shader here does not
	/// touch the shared material and cannot leak onto another player's weapon.
	///
	/// Falls through the candidate list and takes the first that resolves, because
	/// Shader.Find only finds shaders that actually made it into the build. A shader no
	/// material references may have been stripped, and the failure is silent: the skin would
	/// simply render opaque with no error. Logged so it is visible which one bound.
	///
	/// "Something other than opaque" was the original purpose and is no longer the whole of it.
	/// The [Watery] set binds an OPAQUE shader -- what those skins need is not transparency but
	/// a shader with ten properties instead of one, which is why SkinMaterialBindings and the
	/// preflight below exist.
	/// </summary>
	private static void ApplyShaderOverride(Renderer r, int itemId)
	{
		string[] candidates;
		if (!SkinShaders.TryGetValue(itemId, out candidates) || candidates == null || candidates.Length == 0)
			return; // Length check guards candidates[0] in the messages below -- an empty array
			        // would otherwise throw from inside the error path, of all places.

		// ---- resolve the shader, by name first and by Resources path second
		Shader s = null;
		string how = null;
		for (int i = 0; i < candidates.Length; i++)
		{
			s = Shader.Find(candidates[i]);
			if (s != null)
			{
				how = "Shader.Find('" + candidates[i] + "')"
					+ (i > 0 ? " (fell back; '" + candidates[0] + "' is not in this build)" : "");
				break;
			}
		}

		string shaderPath;
		if (s == null && SkinShaderResources.TryGetValue(itemId, out shaderPath) && !string.IsNullOrEmpty(shaderPath))
		{
			s = Resources.Load(shaderPath, typeof(Shader)) as Shader;
			if (s != null)
				how = "Resources.Load('" + shaderPath + "') -- Shader.Find missed '" + candidates[0] + "'";
		}

		if (s == null)
		{
			// LogError, not LogWarning. The old code warned here and let the weapon render
			// opaque, which is survivable for a glass katana but not for a skin whose ENTIRE
			// appearance is the shader.
			Debug.LogError("WeaponSkinHelper: skin " + itemId + " found NONE of its shaders in "
				+ "this build (tried Shader.Find on '" + string.Join("', '", candidates) + "'"
				+ (SkinShaderResources.ContainsKey(itemId)
					? " then Resources.Load on '" + SkinShaderResources[itemId] + "'" : "")
				+ "). Leaving the renderer on its previous material.");
			return;
		}

		// ---- PREFLIGHT the textures BEFORE touching the shader.
		//
		// Order is the whole point. Once r.material.shader is assigned, the renderer is
		// committed: a texture that fails to load after that leaves the property at its
		// Properties-block default -- _Caustics "black" means NO CAUSTICS, _Cube "black"
		// collapses the reflection -- and the weapon draws a plausible-looking half-state with
		// nothing in the log. Resolving everything first means a failure costs the skin its
		// shader swap and leaves the stock material intact, which is visibly "not applied"
		// rather than "applied wrong".
		MaterialBindings bind;
		if (!SkinMaterialBindings.TryGetValue(itemId, out bind))
			bind = null;

		Texture[] resolved = null;
		if (bind != null && bind.Textures != null)
		{
			resolved = new Texture[bind.Textures.Length];
			for (int i = 0; i < bind.Textures.Length; i++)
			{
				TextureBinding tb = bind.Textures[i];
				Texture t = LoadResourceTexture(tb);
				if (t == null)
				{
					// Names the PROPERTY and the PATH. "A texture failed to load" is not
					// actionable; "_Cube could not be loaded from items/shared/cubemaps/studio_a
					// as a Cubemap" is.
					Debug.LogError("WeaponSkinHelper: skin " + itemId + " NOT applied -- could not bind "
						+ tb.Property + " from Resources path '" + tb.ResourcePath + "' as a "
						+ (tb.IsCubemap ? "Cubemap" : "Texture2D") + ". Refusing to bind '"
						+ s.name + "' with " + tb.Property + " unassigned, because that renders "
						+ "a wrong-but-plausible weapon instead of an obviously unskinned one. "
						+ "Leaving the renderer on its previous material.");
					return;
				}
				resolved[i] = t;
			}
		}

		// ---- everything resolved; commit
		r.material.shader = s;

		if (bind != null)
			ApplyMaterialBindings(r.material, bind, resolved, itemId);

		{
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
			//
			// GUARDED, because this block is a DEFAULT for skins that do not author a
			// reflection colour, and the [Watery] set does author one -- (0.420, 0.839, 1,
			// 0.502), copied from the shipped weapon material. Left ungated it would overwrite
			// that with the icy (0.55, 0.75, 0.95, 0.08) two lines after the bindings assigned
			// it, and alpha 0.08 against the shipped 0.502 is a SIX-FOLD cut to the reflection
			// term -- the water's cubemap highlight would all but disappear.
			//
			// The guard asks "did this skin bind _ReflectColor itself?" rather than testing the
			// item id or reordering the two blocks. That is the least surprising mechanism of
			// the three: an id test would need editing every time a skin is added, and relying
			// on statement order leaves a silent trap for whoever next moves these lines. This
			// way the rule is stated where it applies and holds for any future skin.
			if (r.material.HasProperty("_ReflectColor") && !BindsColor(bind, "_ReflectColor"))
			{
				Color reflect;
				if (!SkinReflectTints.TryGetValue(itemId, out reflect))
					reflect = new Color(0.55f, 0.75f, 0.95f, 0.08f); // icy default
				r.material.SetColor("_ReflectColor", reflect);
			}

			// Report the VALUES, not just that a shader bound. 2026-08-17: 9020/9035 and
			// 9021/9036 were reported as looking identical in game, and the arithmetic says they
			// cannot be -- at this texture's mean red 0.277 the two _ReflectColor settings differ
			// by 32/57/75 out of 255, rising to 58/111/151 at p90. "Bound shader X" was never
			// evidence that the COLOURS landed; this line is.
			Debug.Log(string.Format(
				"WeaponSkinHelper: skin {0} bound shader '{1}' via {2}; bindings={3} _Color={4} _ReflectColor={5}",
				itemId, s.name, how, bind == null ? "NONE" : "yes",
				r.material.HasProperty("_Color") ? r.material.GetColor("_Color").ToString() : "(absent)",
				r.material.HasProperty("_ReflectColor") ? r.material.GetColor("_ReflectColor").ToString() : "(absent)"));
		}
	}

	/// <summary>Did this skin's bindings table assign <paramref name="property"/> itself?</summary>
	private static bool BindsColor(MaterialBindings bind, string property)
	{
		if (bind == null || bind.Colors == null)
			return false;
		for (int i = 0; i < bind.Colors.Length; i++)
		{
			if (bind.Colors[i].Property == property)
				return true;
		}
		return false;
	}

	/// <summary>
	/// Resources textures resolved once and reused. ApplyToWeapon runs ApplyShaderOverride for
	/// EVERY renderer under the weapon -- a machine gun is dozens -- and AssignWeapon runs again
	/// on every respawn, so without this the same four assets would be looked up hundreds of
	/// times per match.
	///
	/// Failures are cached too, as null, deliberately. A missing asset is a build/deployment
	/// fact that will not change mid-session, and the alternative is re-attempting a doomed load
	/// once per renderer per respawn and logging the same error every time.
	/// </summary>
	private static readonly Dictionary<string, Texture> _resourceTexCache = new Dictionary<string, Texture>();

	private static Texture LoadResourceTexture(TextureBinding tb)
	{
		Texture cached;
		if (_resourceTexCache.TryGetValue(tb.ResourcePath, out cached))
			return cached;

		// typeof(Cubemap) vs typeof(Texture2D) matters -- Resources.Load type-filters, so asking
		// for the wrong one returns null rather than converting. Both derive from Texture, which
		// is what Material.SetTexture takes, so one cache holds either.
		Texture t = Resources.Load(tb.ResourcePath, tb.IsCubemap ? typeof(Cubemap) : typeof(Texture2D)) as Texture;
		_resourceTexCache[tb.ResourcePath] = t;
		return t;
	}

	/// <summary>Properties already reported as absent, so the message is logged once, not once per renderer.</summary>
	private static readonly Dictionary<string, bool> _reportedMissingProps = new Dictionary<string, bool>();

	/// <summary>
	/// Assign a skin's full material state after its shader is bound.
	///
	/// Every assignment is guarded by HasProperty, and a property the shader does not declare is
	/// reported at Log level rather than LogError. That distinction is deliberate and is the one
	/// thing to preserve here: "this shader has no _CausticsDeform" is an expected, harmless fact
	/// about the shipped build (see the note in WaterBindings -- neither _CausticsTiling nor
	/// _CausticsDeform exists in it), whereas "this texture would not load" means the weapon is
	/// about to render wrong and is fatal. Collapsing the two would either drown the real errors
	/// in noise or hide them.
	/// </summary>
	private static void ApplyMaterialBindings(Material m, MaterialBindings bind, Texture[] resolved, int itemId)
	{
		if (m == null || bind == null)
			return;

		if (bind.Textures != null && resolved != null)
		{
			for (int i = 0; i < bind.Textures.Length; i++)
			{
				TextureBinding tb = bind.Textures[i];
				if (!m.HasProperty(tb.Property))
				{
					ReportMissingProperty(m, itemId, tb.Property, "texture '" + tb.ResourcePath + "'");
					continue;
				}
				m.SetTexture(tb.Property, resolved[i]);
			}
		}

		if (bind.Colors != null)
		{
			for (int i = 0; i < bind.Colors.Length; i++)
			{
				ColorBinding cb = bind.Colors[i];
				if (!m.HasProperty(cb.Property))
				{
					ReportMissingProperty(m, itemId, cb.Property, "colour " + cb.Value);
					continue;
				}
				m.SetColor(cb.Property, cb.Value);
			}
		}

		if (bind.Floats != null)
		{
			for (int i = 0; i < bind.Floats.Length; i++)
			{
				FloatBinding fb = bind.Floats[i];
				if (!m.HasProperty(fb.Property))
				{
					ReportMissingProperty(m, itemId, fb.Property, "float " + fb.Value);
					continue;
				}
				m.SetFloat(fb.Property, fb.Value);
			}
		}
	}

	private static void ReportMissingProperty(Material m, int itemId, string property, string what)
	{
		string shaderName = (m.shader != null ? m.shader.name : "<null shader>");
		string key = shaderName + "|" + property;
		if (_reportedMissingProps.ContainsKey(key))
			return;
		_reportedMissingProps[key] = true;

		// Named, so nobody has to guess which one was skipped -- but NOT an error, because the
		// shader simply does not expose it. Setting it would be a silent no-op; saying so is the
		// point.
		Debug.Log("WeaponSkinHelper: skin " + itemId + " skipped " + property + " (" + what
			+ ") -- shader '" + shaderName + "' does not declare that property, so assigning it "
			+ "would do nothing. The rest of the material was bound normally.");
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

		Texture2D flame = LoadSkinFile(sheet);
		if (flame == null)
			return;
		flame.wrapMode = TextureWrapMode.Repeat;   // it scrolls, so it must tile

		Shader additive = Shader.Find("Particles/Additive");
		if (additive == null)
		{
			Debug.LogWarning("WeaponSkinHelper: 'Particles/Additive' missing, no flames for " + itemId);
			return;
		}

		// ENUMERATE RENDERERS, NOT MeshFilters -- the same call ApplyToWeapon makes at :1114.
		//
		// MEASURED, not assumed. Item 1002 resolves to the prefab named "MachineGun"
		// (DefaultItemUtil.cs:152), which is Assets/GameObject/MachineGun.prefab, and that
		// prefab contains ZERO `!u!33 MeshFilter` components. Its body is a single
		// `!u!137 SkinnedMeshRenderer` on the "MachineGun" child, and a SkinnedMeshRenderer
		// carries its geometry on the RENDERER -- there is no MeshFilter to find. The old
		// GetComponentsInChildren<MeshFilter>() therefore came back EMPTY on that weapon and
		// this entire loop never executed once.
		//
		// That is why 9022 MG [Watery] and 9026 MG [Frosted] have shipped registered in
		// SkinFlames and producing nothing whatsoever in game: not a tuning problem, not a
		// missing sheet, the loop body simply never ran. It is also why the requested "neon
		// aura" for 9015 Neon Circuit -- another MachineGun skin -- could not have worked: an
		// aura is this overlay, and this overlay could not reach the weapon.
		//
		// The other four bases in this file DO have MeshFilters (SniperRifle, ShotGun and
		// Cannon carry three each, TheSplatbat one), so they were always reached and their
		// behaviour here is unchanged.
		Renderer[] renderers = weaponRoot.GetComponentsInChildren<Renderer>(true);

		// The overlay is a SLEEVE around the blade, not a copy of the blade. Copying the weapon
		// mesh paints fire onto the surface; a sleeve is separate geometry standing off the steel,
		// so the flames can orbit the sword in the air around it.
		//
		// Hoisted out of the loop: this is a property of the SKIN, not of the renderer, and the
		// size filter below needs to know it before the first renderer is examined.
		FlameMode mode;
		if (!SkinFlameModes.TryGetValue(itemId, out mode))
			mode = FlameMode.Helix;

		// HELIX ONLY: the longest mesh extent under this weapon, so a trivially small part can be
		// skipped below. Zero when not needed, which switches the filter off.
		float weaponLongest = (mode == FlameMode.Helix) ? LongestOverlayExtent(renderers) : 0f;

		foreach (Renderer src in renderers)
		{
			if (src == null)
				continue;
			if (IsEffectRenderer(src))
				continue;

			// INSPECT VIA sharedMaterial, NOT material. This is the 63a9776 fix that landed in
			// ApplyToWeapon (:1144) and that this method was still quietly undoing afterwards.
			//
			// Renderer.material is not a getter: the first access INSTANTIATES a private copy
			// of the shared material and rebinds this renderer to it. Reading `src.material`
			// and `src.material.shader` to decide whether to SKIP a renderer therefore forced
			// a material instance onto every muzzle flash on every flame-carrying skin, which
			// detaches the quad from the shared material the game's own effect code drives --
			// and the flash then renders as NOTHING rather than as the stock flash.
			// sharedMaterial inspects without instantiating, so a skipped renderer is left
			// genuinely untouched.
			Material shared = src.sharedMaterial;
			if (shared == null)
				continue;

			// Never overlay an effect renderer -- that is the muzzle flash, and stacking an
			// additive copy on an additive quad doubles it into a bright block.
			//
			// BY SHADER NAME, NOT BY RENDERER CLASS, and the two disagree in BOTH directions
			// across these prefabs, so a class test would be wrong on every weapon here:
			//
			//   MachineGun/Shell_Casing IS a ParticleSystemRenderer, but its material
			//   FX_Shell_Casing_A.mat sits on the same builtin shader as the gun body
			//   (MachineGun_0.mat) -- the game paints the skin onto it, so it must NOT be
			//   treated as an effect;
			//
			//   SniperRifle/SRMuzzleFlash, SniperRifle/SplatterTrail, Cannon/CNMuzzleFlash,
			//   ShotGun/SGMuzzleFlash and ShotGun/StandardBullet are plain MeshRenderers whose
			//   materials are on Particles shaders, and those MUST be skipped.
			Shader sh = shared.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;

			// WHERE THE MESH LIVES depends on the renderer. A MeshRenderer reads its geometry
			// from a sibling MeshFilter; a SkinnedMeshRenderer ignores any MeshFilter entirely
			// and draws its own sharedMesh, so it is asked first.
			//
			// Anything that yields neither is skipped, and that is the correct outcome for the
			// two ParticleSystemRenderers on the MachineGun: they keep their mesh on the
			// particle system, and painting a flame copy of a shell casing would be nonsense.
			SkinnedMeshRenderer skinned = src as SkinnedMeshRenderer;
			MeshFilter mf = src.GetComponent<MeshFilter>();
			Mesh sourceMesh = (skinned != null)
				? skinned.sharedMesh
				: (mf != null ? mf.sharedMesh : null);
			if (sourceMesh == null)
				continue;

			// AssignWeapon can run more than once for the same weapon instance; without this
			// each call would stack another overlay and the flames would get brighter every
			// respawn until the blade was a white blob.
			if (src.transform.FindChild(FlameChildName) != null)
				continue;

			// SKIP TRIVIALLY SMALL PARTS, IN HELIX MODE ONLY. This is what makes 9020 shippable.
			//
			// ApplyFlames filters on "yields a mesh" and "is not particle-shaded" and nothing
			// else, so on AWP_Roughed it finds TWO targets: the rifle (mesh AWP) and the pistol
			// grip (mesh Handle). A Surface overlay on the grip would be a correctly-placed copy
			// of the grip and harmless. A HELIX sleeve is not: BuildFlameSleeve synthesises it
			// from the part's OWN bounding box along the part's OWN longest axis, and Handle's
			// longest axis is X -- across the rifle. The result, rendered and confirmed on screen,
			// is a detached horizontal comb of white hairlines floating in mid-air below the
			// receiver, attached to nothing, at radius 0.004 over a span of 0.027.
			//
			// It cannot be suppressed through any existing table: SkinSleeves is keyed by ITEM ID,
			// not by renderer, so 9020's entry applies to both parts equally. Hence a filter here.
			//
			// 15% of the weapon's longest part, and the margin is enormous rather than tuned:
			//   AWP    1.477879   Handle 0.060465 -> 4.1%   SKIPPED
			//   Death_Hammer 1.172920, Ninja_Knife 1.074301, MachineGun 0.879688 -> each is its
			//   own weapon's maximum, i.e. 100%, so nothing that has flames today is anywhere
			//   near the threshold and no existing skin changes behaviour.
			if (weaponLongest > 0f)
			{
				Vector3 sz = sourceMesh.bounds.size;
				float longest = Mathf.Max(sz.x, Mathf.Max(sz.y, sz.z));
				if (longest < weaponLongest * FLAME_MIN_PART_FRAC)
				{
					ReportFlamePartSkip(itemId, weaponRoot, src, sourceMesh, longest, weaponLongest);
					continue;
				}
			}

			Vector3 axis = Vector3.up, centre = Vector3.zero;
			float shellScale = 1f;
			Mesh overlayMesh;
			if (mode == FlameMode.Surface)
			{
				// the original concept: fire painted onto the weapon's own geometry
				overlayMesh = WhiteVertexCopy(sourceMesh);
			}
			else if (mode == FlameMode.Shell)
			{
				// THE WEAPON'S OWN MESH, BY REFERENCE. No copy, no vertex read, so this is the one
				// mode that works on the m_IsReadable: 0 meshes -- which is exactly the set of
				// weapons that could never have surface fire.
				//
				// Do NOT "improve" this into a copy to give it white vertex colours the way Surface
				// does. Particles/Additive multiplies by vertex colour, and a mesh with no colour
				// channel is treated as white anyway, so a copy would buy nothing and would put
				// these two weapons straight back into the unreadable-mesh failure.
				overlayMesh = sourceMesh;
				if (!SkinShellScale.TryGetValue(itemId, out shellScale) || shellScale <= 1f)
					shellScale = 1.08f;
				// Scale about the MESH BOUNDS CENTRE rather than the pivot. A vertex v would render
				// at s*v; we want c + s*(v - c), so the child is offset by c*(1 - s). bounds is the
				// serialised m_LocalAABB and needs no CPU read.
				centre = sourceMesh.bounds.center * (1f - shellScale);
			}
			else
			{
				overlayMesh = BuildFlameSleeve(sourceMesh, itemId, out axis, out centre);
			}
			if (overlayMesh == null)
			{
				// LOUD AND NAMED. The usual cause is a mesh with m_IsReadable: 0, where
				// `.vertices` cannot be read on the CPU -- true of Sniper.asset, ShotGun.asset,
				// CannonBody/CannonHead.asset, AWP.asset and Death_Hammer.asset in the shipped
				// art, and false of MachineGun.asset and Ninja_Knife.asset. Saying WHICH weapon
				// and WHICH skin is the point: a skin that silently ships without its flames is
				// exactly the half-state this file keeps paying for.
				ReportFlameSkip(itemId, weaponRoot, src, sourceMesh);
				continue;
			}

			// SHELL CANNOT WORK ON A SKINNED RENDERER, and it must say so rather than draw
			// nothing. A SkinnedMeshRenderer with bones ignores its own transform entirely, so
			// localScale -- the only thing that makes a shell a shell -- is discarded and the
			// overlay would sit exactly on the weapon, invisible. Falling back to the static
			// MeshFilter path is worse, not better: the geometry would draw in the BIND POSE,
			// which on MachineGun.prefab is a quarter turn about Y (mesh AABB centre
			// (-0.0063, 0.0242, 0.1400) vs renderer AABB centre (-0.1400, 0.0242, -0.0063)) --
			// a gun-shaped ghost lying crosswise through the real weapon.
			// No skin does this today: 9020 (AWP_Roughed) and 9021 (DeathHammer) are both static
			// MeshRenderers. This exists so that adding a Shell skin to the MachineGun fails
			// LOUDLY instead of shipping an effect nobody can see.
			if (mode == FlameMode.Shell && skinned != null
				&& skinned.bones != null && skinned.bones.Length > 0)
			{
				Debug.LogWarning(string.Format(
					"WeaponSkinHelper: skin {0} on {1} asks for Shell flames, but '{2}' is a "
					+ "SkinnedMeshRenderer ({3} bones). A skinned renderer ignores transform "
					+ "scale, so the shell would be invisible. Skipping this renderer -- use "
					+ "Surface mode for skinned weapons, or scale the shell in the mesh itself.",
					itemId, weaponRoot != null ? weaponRoot.name : "?",
					src.gameObject.name, skinned.bones.Length));
				continue;
			}

			// Created only now that there is something to put in it. Building it earlier left
			// an orphan GameObject floating in the scene on every equip of a skin whose mesh
			// could not be copied -- never parented, never destroyed.
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
			go.layer = src.gameObject.layer;

			go.transform.parent = src.transform;
			// Vertices are authored in the weapon's own local space now that the sleeve bends
			// with the blade, so the child sits at the origin and never needs moving.
			go.transform.localPosition = centre;   // Vector3.zero from BuildFlameSleeve
			go.transform.localRotation = Quaternion.identity;
			// Scale stays at ONE for Surface and Helix. An earlier version used 1.015 "so it
			// never z-fights", which was wrong twice over: Particles/Additive already has ZWrite
			// Off so there is no depth fight to lose, and scaling happens about this transform's
			// PIVOT, not the mesh centroid. The katana's geometry sits well off its pivot, so
			// 1.5% became a visible translation and the overlay read as a second, ghostly blade
			// beside the real one.
			//
			// SHELL scales deliberately, and survives that same trap only because localPosition
			// above carries the bounds.center * (1 - scale) correction. If the shell ever reads as
			// a displaced ghost of the gun rather than a halo around it, that correction is the
			// first thing to check -- it is the identical failure, and the pivot offset is large
			// on exactly these weapons.
			go.transform.localScale = Vector3.one * shellScale;

			// A SKINNED source needs a SKINNED overlay, and this is measured rather than
			// cautious. On MachineGun.prefab the mesh's own m_LocalAABB is
			// centre (-0.0063, 0.0242, 0.1400) / extent (0.0375, 0.1224, 0.4398), while the
			// SkinnedMeshRenderer's m_AABB -- where the gun actually DRAWS -- is
			// centre (-0.1400, 0.0242, -0.0063) / extent (0.4398, 0.1224, 0.0375): the same
			// numbers with X and Z exchanged, i.e. a quarter turn about Y. The mesh's first
			// bindpose is exactly that rotation, and the root bone B_Root carries it. So a
			// static MeshFilter copy parented to this renderer would draw the bind-pose
			// geometry a quarter turn out -- a gun-shaped ghost of fire lying crosswise
			// through the weapon -- and would also sit still through the shoot animation that
			// drives B_Ejector and B_Hammer.
			//
			// Binding the copy to the SOURCE's own bones and bindposes makes the overlay
			// deform with the weapon, which is the only way "fire on the surface" means
			// anything on a skinned mesh. A SkinnedMeshRenderer with bones ignores its own
			// transform, so the localPosition/localRotation set above are simply unused here.
			//
			// The guard is on the COPY's bindposes, not on the source's: BuildFlameSleeve
			// produces fresh unweighted geometry, so a Helix skin on a skinned weapon falls to
			// the static branch below. No skin does that today -- every MachineGun skin in
			// SkinFlameModes is Surface -- but it would place the sleeve in the renderer's
			// space rather than the bind pose's, so it is worth knowing before adding one.
			Renderer or;
			if (skinned != null && skinned.bones != null && skinned.bones.Length > 0
				&& overlayMesh.bindposes != null && overlayMesh.bindposes.Length > 0)
			{
				SkinnedMeshRenderer osmr = go.AddComponent<SkinnedMeshRenderer>();
				osmr.sharedMesh = overlayMesh;
				osmr.bones = skinned.bones;
				osmr.rootBone = skinned.rootBone;
				osmr.quality = skinned.quality;
				// Same culling volume as the weapon it covers, so the overlay cannot be culled
				// while the gun is still on screen.
				osmr.localBounds = skinned.localBounds;
				or = osmr;
			}
			else
			{
				MeshFilter of = go.AddComponent<MeshFilter>();
				of.sharedMesh = overlayMesh;
				or = go.AddComponent<MeshRenderer>();
			}
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
				{
					if (mode == FlameMode.Surface)
						tint = new Color(0.18f, 0.26f, 0.32f, 0.5f);   // peak add 0.36/0.52/0.64
					else if (mode == FlameMode.Shell)
						// LOWER than Surface, on purpose. A shell covers the whole silhouette AND
						// double-adds at every grazing angle (ZWrite Off, both faces draw), so the
						// same value that reads as a wash on Surface reads as a glare here. The
						// brief was "a low overlay so it's an outer blue flame" -- the rim is
						// supposed to carry it, not the body.
						tint = new Color(0.10f, 0.17f, 0.30f, 0.5f);   // peak 0.20/0.34/0.60, x2 at the rim
					else
						tint = new Color(0.40f, 0.47f, 0.52f, 0.5f);   // peak add 0.80/0.94/1.04
				}
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
			// Shell samples the weapon's OWN UVs, exactly as Surface does -- it is the same
			// geometry -- so it takes the same tiling and the fire travels over the gun's shape
			// rather than around a ribbon.
			m.SetTextureScale("_MainTex", (mode == FlameMode.Surface || mode == FlameMode.Shell)
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

	/// <summary>
	/// A Helix overlay target must be at least this fraction of the weapon's longest part.
	/// See the note at the call site: the only thing this excludes in the shipped art is the
	/// AWP's pistol grip, at 4.1%.
	/// </summary>
	private const float FLAME_MIN_PART_FRAC = 0.15f;

	/// <summary>
	/// The longest bounding-box axis of any mesh under this weapon that ApplyFlames would treat as
	/// an overlay target.
	///
	/// Deliberately applies the SAME two filters the main loop does -- particle-shaded renderers
	/// skipped, SkinnedMeshRenderer asked for sharedMesh before any MeshFilter -- because a
	/// fraction measured against a set that includes muzzle flashes and shell casings would mean
	/// something different from the fraction the loop then tests against.
	/// </summary>
	private static float LongestOverlayExtent(Renderer[] renderers)
	{
		float best = 0f;
		if (renderers == null)
			return 0f;
		for (int i = 0; i < renderers.Length; i++)
		{
			Renderer r = renderers[i];
			if (r == null)
				continue;
			// sharedMaterial, never material -- see the note in the main loop. This prepass runs
			// over every renderer on the weapon including the effects, and reading .material here
			// would instantiate a copy on each one, which is the exact bug 63a9776 fixed.
			Material shared = r.sharedMaterial;
			if (shared == null)
				continue;
			Shader sh = shared.shader;
			if (sh != null && sh.name != null && sh.name.IndexOf("Particle", StringComparison.OrdinalIgnoreCase) >= 0)
				continue;
			SkinnedMeshRenderer skinned = r as SkinnedMeshRenderer;
			MeshFilter mf = r.GetComponent<MeshFilter>();
			Mesh m = (skinned != null) ? skinned.sharedMesh : (mf != null ? mf.sharedMesh : null);
			if (m == null)
				continue;
			Vector3 sz = m.bounds.size;
			float longest = Mathf.Max(sz.x, Mathf.Max(sz.y, sz.z));
			if (longest > best)
				best = longest;
		}
		return best;
	}

	private static readonly Dictionary<string, bool> _reportedFlameSkips = new Dictionary<string, bool>();

	/// <summary>
	/// Say out loud that a part was too small to carry a helix sleeve, with the numbers.
	///
	/// Separate from ReportFlameSkip below, and at Log rather than LogWarning, because the two
	/// mean opposite things: that one is "this skin lost fire it should have had", this one is
	/// "this skin correctly did not put fire somewhere silly". Sharing a message would make a
	/// working weapon look broken in the log.
	/// </summary>
	private static void ReportFlamePartSkip(int itemId, GameObject weaponRoot, Renderer src,
		Mesh sourceMesh, float longest, float weaponLongest)
	{
		string weapon = (weaponRoot != null ? weaponRoot.name : "<null weapon>");
		string part = (src != null ? src.name : "<null renderer>");
		string mesh = (sourceMesh != null ? sourceMesh.name : "<null mesh>");

		string key = "small|" + itemId + "|" + weapon + "|" + part + "|" + mesh;
		if (_reportedFlameSkips.ContainsKey(key))
			return;
		_reportedFlameSkips[key] = true;

		Debug.Log("WeaponSkinHelper: skin " + itemId + " gives no helix sleeve to " + weapon + "/"
			+ part + " (mesh '" + mesh + "', longest extent " + longest.ToString("F4") + " = "
			+ (longest / weaponLongest * 100f).ToString("F1") + "% of the weapon's "
			+ weaponLongest.ToString("F4") + ", under the " + (FLAME_MIN_PART_FRAC * 100f).ToString("F0")
			+ "% floor). A sleeve is built from the PART's own bounding box, so on a small "
			+ "off-axis part it detaches from the weapon entirely. The rest of the skin is "
			+ "unaffected.");
	}

	/// <summary>
	/// Say out loud that a renderer got no flame overlay, naming the SKIN and the WEAPON.
	///
	/// WhiteVertexCopy already logs the mesh it could not copy, but a mesh name on its own does
	/// not tell you which skin lost its fire or which weapon to look at in game -- and "the skin
	/// is registered in SkinFlames but nothing burns" is precisely the failure that let 9022 and
	/// 9026 ship broken and unnoticed.
	///
	/// Deduplicated per skin/weapon/mesh because ApplyToWeapon runs on every equip and every
	/// respawn, for every player in the room.
	/// </summary>
	private static void ReportFlameSkip(int itemId, GameObject weaponRoot, Renderer src, Mesh sourceMesh)
	{
		string weapon = (weaponRoot != null ? weaponRoot.name : "<null weapon>");
		string part = (src != null ? src.name : "<null renderer>");
		string mesh = (sourceMesh != null ? sourceMesh.name : "<null mesh>");

		string key = itemId + "|" + weapon + "|" + part + "|" + mesh;
		if (_reportedFlameSkips.ContainsKey(key))
			return;
		_reportedFlameSkips[key] = true;

		Debug.LogWarning("WeaponSkinHelper: skin " + itemId + " gets NO flame overlay on "
			+ weapon + "/" + part + " (mesh '" + mesh + "') -- the overlay geometry could not be "
			+ "built, almost always because the mesh is not readable on the CPU. The skin's "
			+ "texture and shader are unaffected; only its flames are missing.");
	}

	private static readonly Dictionary<Mesh, Mesh> _flameMeshCache = new Dictionary<Mesh, Mesh>();
	// KEYED BY (MESH, ITEM ID), NOT BY MESH. This was a latent bug the moment SkinSleeves stopped
	// being empty, and it would have been invisible: the cache is consulted at :1786, BEFORE the
	// SkinSleeves lookup at :1811, so two Helix skins sharing one source mesh would both get
	// whichever SleeveSpec happened to build FIRST that session -- order-dependent, per-session,
	// and with nothing in the log to say so.
	//
	// Not hypothetical any more. Five prefabs share one AWP mesh GUID (AWP_Roughed, AWP_Black,
	// AWP_Camo, AWP_Pimp, AWP-Snake), so a second AWP helix skin lands straight on it, and 9017's
	// katana is likewise shared by 9018/9019 (Surface today -- but a mode change is one word).
	//
	// A composite string rather than a nested dictionary because Mesh has no value equality and
	// GetInstanceID is stable for the object's lifetime, which is exactly the cache's lifetime.
	private static readonly Dictionary<string, Mesh> _sleeveCache = new Dictionary<string, Mesh>();
	private static readonly Dictionary<string, Vector3> _sleeveAxis = new Dictionary<string, Vector3>();
	private static readonly Dictionary<string, Vector3> _sleeveCentre = new Dictionary<string, Vector3>();

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

		string cacheKey = source.GetInstanceID() + "|" + itemId;

		Mesh cached;
		if (_sleeveCache.TryGetValue(cacheKey, out cached) && cached != null)
		{
			axis = _sleeveAxis[cacheKey];
			centre = _sleeveCentre[cacheKey];
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

		_sleeveCache[cacheKey] = mesh;
		_sleeveAxis[cacheKey] = axis;
		_sleeveCentre[cacheKey] = centre;

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

			// SKIN WEIGHTS, when the source has them. Normals and tangents are dead weight for
			// an unlit additive shader, but these are not: without bindposes and boneWeights
			// the overlay can only be drawn by a static MeshRenderer, and on a skinned weapon
			// that puts it in the wrong place -- see the measurement in ApplyFlames. Both are
			// plain managed array reads on an already-verified-readable mesh, so neither can
			// reintroduce the native crash that Object.Instantiate was.
			BoneWeight[] weights = source.boneWeights;
			Matrix4x4[] binds = source.bindposes;
			if (weights != null && weights.Length == copy.vertexCount
				&& binds != null && binds.Length > 0)
			{
				copy.boneWeights = weights;
				copy.bindposes = binds;
			}

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

	/// <summary>
	/// Recolour this weapon's own muzzle effects, for the one skin that asks for it.
	///
	/// THE FIRST LINE IS THE ISOLATION PROOF, and it is the whole reason this is safe to add to a
	/// file that already carries 28 skins. MuzzleTints has exactly one key. A stock weapon reaches
	/// ApplyToWeapon with its own item id (1002-1005, 6, ...), misses the lookup and returns
	/// having touched nothing; so does every other skin. And because nothing below writes a shared
	/// asset -- Light.color and ParticleSystem.startColor are both per-COMPONENT state on this
	/// weapon instance -- even a mis-gated call could not outlive the weapon it ran on.
	///
	/// NOT PREVIEWABLE IN skin_studio, and that is not a gap that can be closed cheaply: the
	/// studio draws a static weapon from the exporter's renderer list, has no light and no
	/// particle simulation, and does not model a fire event at all. Verification for this one is
	/// the real client, first person AND watching another player fire, per the note at the call
	/// site. Budget for a build; do not let a green studio stand in for it.
	/// </summary>
	/// <summary>
	/// Set true to have ApplyMuzzleTint dump the live weapon's renderers, materials, shaders and
	/// BaseWeaponEffect components to the player log. Off in shipping builds -- it is ~13 lines
	/// per weapon equip, twice (first person and third).
	/// </summary>
	// static readonly, NOT const: a const false lets the compiler fold the guarded blocks away
	// and emit CS0162 "unreachable code" for each one, and this file has built warning-clean
	// until now. A field read costs nothing here and keeps the diagnostic switchable.
	private static readonly bool MUZZLE_DEBUG = false;

	public static void ApplyMuzzleTint(GameObject weaponRoot, int itemId)
	{
		MuzzleTintSpec spec;
		if (!MuzzleTints.TryGetValue(itemId, out spec))
			return; // not one of ours -- leave every other skin and every stock weapon alone

		if (spec.HasLight)
		{
			// Every Light under the weapon, which on AWP_Roughed is exactly one
			// (MuzzleLightShining). Colour only: intensity and range are what the clip animates,
			// and writing them here would fight it.
			Light[] lights = weaponRoot.GetComponentsInChildren<Light>(true);
			for (int i = 0; i < lights.Length; i++)
			{
				if (lights[i] == null)
					continue;
				lights[i].color = spec.LightColour;
			}
		}

		// DIAGNOSTIC, 2026-08-17. The tint was verified in the extracted Unity project and then
		// reported invisible in game -- "just a very low opacity grey/white smoke and that native
		// orange muzzle". Both halves of this hook match BY NAME or BY COMPONENT on the runtime
		// hierarchy, and the runtime hierarchy is the one thing that was never checked: every
		// object name here came from
		// UberSteam-client-4-7-1-unity465/.../AWP_Roughed.prefab, NOT from the shipped client.
		// So this prints what is actually under the weapon at equip time. If "Sfx" and "Spark"
		// are not in that list, the names are wrong and the fix is a rename, not a colour.
		// Kept, not deleted, and OFF by default. This block is what finally identified the real
		// effect after three wrong builds, and the next weapon with a muzzle request will need it
		// again -- the prefab cannot tell you which effects anything actually plays. Flip
		// MUZZLE_DEBUG to true, equip the weapon, and read UberStrike_Data/output_log.txt.
		if (MUZZLE_DEBUG)
		{
			Light[] dbgL = weaponRoot.GetComponentsInChildren<Light>(true);
			ParticleSystem[] dbgP = weaponRoot.GetComponentsInChildren<ParticleSystem>(true);
			string pn = "";
			for (int i = 0; i < dbgP.Length; i++)
				pn += (i > 0 ? ", " : "") + dbgP[i].gameObject.name;
			string ln = "";
			for (int i = 0; i < dbgL.Length; i++)
				ln += (i > 0 ? ", " : "") + dbgL[i].gameObject.name;
			Debug.Log(string.Format(
				"WeaponSkinHelper: muzzle tint {0} on '{1}' -- {2} light(s) [{3}], {4} particle "
				+ "system(s) [{5}]; wanted [{6}]",
				itemId, weaponRoot.name, dbgL.Length, ln, dbgP.Length, pn,
				spec.ParticleObjects == null ? "" : string.Join(", ", spec.ParticleObjects)));

			// SECOND PASS, 2026-08-17. Tinting Sfx and Spark -- both found by name, both
			// Particles/Additive, both _TintColor written on a per-instance clone -- changed
			// NOTHING on screen. So the flash almost certainly is not those two emitters, and the
			// assumption that it was came from reading the prefab rather than the running game.
			// This lists EVERY renderer under the weapon with its shader, and every weapon-effect
			// component, so the thing that actually draws the orange flash has to appear here.
			Renderer[] dbgR = weaponRoot.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < dbgR.Length; i++)
			{
				Material sm = dbgR[i].sharedMaterial;
				Debug.Log(string.Format(
					"WeaponSkinHelper:   renderer '{0}' [{1}] mat '{2}' shader '{3}' enabled={4}",
					dbgR[i].gameObject.name, dbgR[i].GetType().Name,
					sm != null ? sm.name : "(null)",
					sm != null && sm.shader != null ? sm.shader.name : "(null)",
					dbgR[i].enabled));
			}
			BaseWeaponEffect[] dbgE = weaponRoot.GetComponentsInChildren<BaseWeaponEffect>(true);
			for (int i = 0; i < dbgE.Length; i++)
				Debug.Log(string.Format("WeaponSkinHelper:   effect '{0}' [{1}]",
					dbgE[i].gameObject.name, dbgE[i].GetType().Name));
		}

		// GENERIC PASS, by COMPONENT TYPE rather than by object name. Added when the tint was
		// extended from one weapon to six.
		//
		// Naming renderers per weapon does not scale and is not knowable from the prefabs:
		// AWP_Roughed.prefab declares only MuzzleLight, yet the RUNNING weapon also carries
		// BulletTrail on a nested SplatterTrail -- which is the effect that actually draws the
		// flash. Enumerating by component type finds it on every weapon without anyone having to
		// know its child's name in advance.
		//
		// WHAT IS INCLUDED: BulletTrail, MuzzleParticleSystem, MuzzleHeatWave -- the effects whose
		// renderers are muzzle visuals that fire on shoot.
		//
		// WHAT IS DELIBERATELY EXCLUDED, and each exclusion is a bug avoided:
		//   MuzzleFlash -- its material's _TintColor.ALPHA is driven by a legacy Animation on the
		//     SHARED material asset. Touching `.material` clones it frozen at whatever alpha the
		//     clip last wrote, which is 0 after Hide(), so the flash renders as NOTHING. That is
		//     exactly the regression commit 63a9776 fixed, and DeathHammer (9021 Icebreaker) is
		//     the weapon that has one. Its light still tints; its flash is left stock on purpose.
		//   MuzzleSmoke -- blue smoke reads as a bug rather than as a skin.
		if (spec.HasParticles)
		{
			Component[] fx = weaponRoot.GetComponentsInChildren<BaseWeaponEffect>(true);
			for (int i = 0; i < fx.Length; i++)
			{
				if (fx[i] == null)
					continue;
				string tn = fx[i].GetType().Name;
				if (tn != "BulletTrail" && tn != "MuzzleParticleSystem" && tn != "MuzzleHeatWave")
					continue;
				Renderer[] fr = fx[i].GetComponentsInChildren<Renderer>(true);
				for (int k = 0; k < fr.Length; k++)
				{
					if (fr[k] == null)
						continue;
					Material fm = fr[k].material;      // per-instance clone; stock weapon untouched
					if (fm == null || !fm.HasProperty("_TintColor"))
						continue;
					Color cur = fm.GetColor("_TintColor");
					fm.SetColor("_TintColor", new Color(
						spec.ParticleTint.r, spec.ParticleTint.g, spec.ParticleTint.b, cur.a));
					if (MUZZLE_DEBUG)
						Debug.Log(string.Format(
							"WeaponSkinHelper: muzzle {0} tinted {1} renderer '{2}'",
							itemId, tn, fr[k].gameObject.name));
				}
			}
		}

		// The renderers that actually draw something on this weapon. See TintRenderers' note:
		// Sfx/Spark below are inert on the AWP because nothing plays them, and this is the loop
		// that produces the visible change.
		if (spec.TintRenderers != null)
		{
			Renderer[] rends = weaponRoot.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < rends.Length; i++)
			{
				if (rends[i] == null)
					continue;
				for (int j = 0; j < spec.TintRenderers.Length; j++)
				{
					if (rends[i].gameObject.name != spec.TintRenderers[j])
						continue;
					// `.material`, per-instance, so the stock weapon is untouched.
					Material rm = rends[i].material;
					if (rm != null && rm.HasProperty("_TintColor"))
					{
						Color cur = rm.GetColor("_TintColor");
						rm.SetColor("_TintColor", new Color(
							spec.ParticleTint.r, spec.ParticleTint.g, spec.ParticleTint.b, cur.a));
						if (MUZZLE_DEBUG)
							Debug.Log(string.Format(
								"WeaponSkinHelper: muzzle {0} tinted renderer '{1}' (shader '{2}')",
								itemId, rends[i].gameObject.name,
								rm.shader != null ? rm.shader.name : "(null)"));
					}
					break;
				}
			}
		}

		if (spec.HasParticles && spec.ParticleObjects != null)
		{
			// BY NAME, not "every particle system on the weapon". The AWP also carries
			// AWPGunSmoke, AWPBigSmoke, AWPBulletShell and AWPBulletShellTail, and tinting smoke
			// or brass blue reads as a bug rather than as a skin -- see the table's note.
			ParticleSystem[] systems = weaponRoot.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < systems.Length; i++)
			{
				if (systems[i] == null)
					continue;
				for (int j = 0; j < spec.ParticleObjects.Length; j++)
				{
					if (systems[i].gameObject.name != spec.ParticleObjects[j])
						continue;
					// startColor, NOT the renderer's material. This is a Unity 4.x Shuriken
					// build -- there is no `main` module and no MinMaxGradient, startColor is a
					// plain Color, and it multiplies the shared material per-particle instead of
					// replacing it. See the leak counts in the table above.
					systems[i].startColor = spec.ParticleTint;

					// startColor ALONE DOES NOTHING VISIBLE HERE, proven in game on 2026-08-17:
					// the hook ran on both the first- and third-person weapons, found this exact
					// object by name, set startColor -- and the flash stayed stock orange.
					// Particles/Additive computes 2 * vertexColour * _TintColor * texture, so
					// vertex colour is only half the product; whatever this build does with
					// startColor, the material's own _TintColor is the term that survives.
					//
					// `.material` and NOT `.sharedMaterial`: sharedMaterial would leak into every
					// other weapon using FireBall/Flare, and `.material` clones per renderer
					// instance so the stock AWP is untouched. The clone trap that killed the
					// shotgun flash in 63a9776 does NOT apply here -- that was a material whose
					// alpha is driven by a legacy Animation, and nothing animates these two.
					Renderer pr = systems[i].GetComponent<Renderer>();
					if (pr != null)
					{
						Material pm = pr.material;
						string sh = pm != null && pm.shader != null ? pm.shader.name : "(null)";
						bool has = pm != null && pm.HasProperty("_TintColor");
						if (has)
						{
							// Preserve alpha, as WeaponTracerTinter does: on the additive
							// particle shaders alpha is the energy term and the effect's own
							// fade depends on it.
							Color cur = pm.GetColor("_TintColor");
							pm.SetColor("_TintColor", new Color(
								spec.ParticleTint.r, spec.ParticleTint.g, spec.ParticleTint.b, cur.a));
						}
						if (MUZZLE_DEBUG)
							Debug.Log(string.Format(
								"WeaponSkinHelper: muzzle {0} '{1}' shader '{2}' _TintColor={3}",
								itemId, systems[i].gameObject.name, sh,
								has ? "SET" : "ABSENT -- shader has no _TintColor"));
					}
					break;
				}
			}
		}
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
