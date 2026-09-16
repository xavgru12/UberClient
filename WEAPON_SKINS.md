# Weapon skins

Skin art ships as `EmbeddedResource` in `Assembly-CSharp.dll`. Embedding is the only load path —
clone, compile, run and skins render, no deployment step.

Repo art lives in `Assembly-CSharp/WeaponSkins/`. Each file is an `<EmbeddedResource>` with
`LogicalName` `WeaponSkins.<file>` in `Assembly-CSharp.csproj`. Add a skin = drop the files in
that folder and add the resource entries.

## What the loader reads

Per registered skin, in order:

    <stem>.jpg   +   <stem>.alpha.png     JPEG colour + lossless mask (preferred)
    <stem>.png                            single RGBA PNG (fallback)
    <stem>_Icon.png                       shop icon (ProxyItem)

Colour is JPEG q92, chroma subsampling OFF (4:2:0 smears colour), ~42-45 dB PSNR vs original.
Alpha is never compressed: specular mask on opaque skins, transparency on see-through skins
(9017-9021). Stays lossless greyscale PNG.

## Two traps

**Alpha means different things per shader.** `Bumped Specular` reads `o.Gloss = tex.a` (gloss
mask). The alpha-blended glass shader reads alpha as transparency. A specular mask on a glass
skin renders half-dissolved.

**Alpha-blended geometry does not write depth.** Overlapping faces of one mesh sort arbitrarily,
so a multi-part weapon on the glass shader renders hollow. A single thin blade survives; a
machine gun does not. Overlapping geometry ships opaque.
