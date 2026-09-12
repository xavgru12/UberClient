# Weapon skins — deploying the art

There are **two** ways the skin art reaches a player, and which one applies depends on how many
skins are in the build.

## The failure this exists to prevent

The skins load their art at runtime from:

    <install>/UberStrike_Data/Skins/

The repo keeps it in:

    Assembly-CSharp/WeaponSkins/

**Nothing in the build copies between those, and the folder names differ.** Building
`Assembly-CSharp.dll` and shipping it alone produces **no skins and no error** — the loader
logs "skin file not found" and every weapon renders stock. That is indistinguishable from "the
patch does not work", and it is exactly what happened on first integration.

---

## The art is embedded, and a deployed file still wins

Every skin's art ships as an `EmbeddedResource` in `Assembly-CSharp.dll`, and
`WeaponSkinHelper.ReadSkinBytes` checks `UberStrike_Data/Skins/` **first**, falling back to the
embedded copy. So:

  * **Clone, compile, run** works with no deployment step. This is what #10 did for one skin and
    what #8 now does for all of them. Shipping the DLL alone was the whole of "all skins PR
    doesn't work" -- the code arrived, the art did not, and the failure is silent.
  * **The patcher path is unchanged.** A deployed loose file overrides the embedded copy, so art
    can still be updated without re-shipping code.

    default build              28.41 MB   67 embedded resources
    -p:EmbedSkins=false         1.36 MB   code only, loads from the deployed folder

The size is the reason this was removed once before: a plain resource list took the assembly to
50 MB and every code-only fix re-shipped the art. `-p:EmbedSkins=false` answers that without
taking the working default away from anyone who just wants to build and play.

## The full set: deploy as game files

    .\tools\Deploy-WeaponSkins.ps1
    .\tools\Deploy-WeaponSkins.ps1 -GamePath "D:\Games\UberStrike" -ManifestOut skins.entry.txt
    .\tools\Deploy-WeaponSkins.ps1 -WhatIf          # verify without copying

It does three things:

1. **Copies** the art into `<install>/UberStrike_Data/Skins/`, skipping files that are already
   byte-identical so re-running is cheap.
2. **Verifies** every skin `WeaponSkinHelper` references actually resolves in the install, and
   **fails with a non-zero exit** naming the exact item id if one is missing. This is the whole
   point: a missing file is otherwise silent.
3. **Emits a patcher manifest** — `Skins/<name><TAB><bytes><TAB><sha256>` per line — so the
   patcher can ship the same files to players instead of each tester copying by hand.

Needs no administrator rights.

---

## What the loader actually reads

For each registered skin it resolves, in order:

    <stem>.jpg   +   <stem>.alpha.png     preferred: JPEG colour, lossless specular mask
    <stem>.png                            fallback: single RGBA PNG
    <stem>_Icon.png                       shop icon, read by ProxyItem

The colour is JPEG q92 with **chroma subsampling off** — 4:2:0 smears colour and is exactly what
wrecks textures. It measures 41.9–45.2 dB PSNR against the original, roughly 1% average
per-channel error, for 5–7× less data.

The **alpha is never compressed**. On most skins it carries the specular mask composited from
the base weapon, which is what makes them read as metal rather than flat paint. On the
see-through skins (9017–9021) it is transparency instead. Either way it stays bit-for-bit
lossless in its own greyscale PNG.

---

## Two traps worth knowing

**Alpha means different things per shader.** `Bumped Specular` reads `o.Gloss = tex.a`, so alpha
is a *gloss* mask on every opaque skin. The alpha-blended glass shader reads it as
*transparency*. Compositing a specular mask onto a see-through skin produces an opacity map and
a half-dissolved weapon.

**Alpha-blended geometry does not write depth.** The glacier packs first shipped on the glass
shader and rendered hollow — bright edges with the body see-through to the wall behind, because
overlapping faces of one mesh sort arbitrarily. A katana blade is a single thin shape and
survives it; a machine gun is dozens of overlapping parts and does not. Anything with
overlapping geometry ships opaque.
