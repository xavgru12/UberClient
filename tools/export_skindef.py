#!/usr/bin/env python3
"""
Export a skin's config as a WeaponSkinDictionaries paste-in block.

The skin gun tool (or you) call skindef_block(id, fields) to turn a skin's options into the
one per-id block that pastes straight into WeaponSkinDictionaries.cs. Empty options are left out.

CLI:
  python export_skindef.py --id 2010                 # print skin 2010's block from the source
  python export_skindef.py --id 2010 --file ../Assembly-CSharp/WeaponSkinDictionaries.cs
  python export_skindef.py --list                    # every id in the source

Field order (matches SkinDef): SkinTextures, SkinShellScale, SkinFlameModes, SkinShaders,
SkinShaderResources, SkinMaterialBindings, SkinReflectTints, SkinFlameTints, SkinSleeves,
SkinFlames, IconTextures, TracerOverrides, MuzzleTints.
"""
import os, re, sys

FIELDS = ["SkinTextures","SkinShellScale","SkinFlameModes","SkinShaders","SkinShaderResources",
          "SkinMaterialBindings","SkinReflectTints","SkinFlameTints","SkinSleeves","SkinFlames",
          "IconTextures","TracerOverrides","MuzzleTints"]
DEFAULT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Assembly-CSharp", "WeaponSkinDictionaries.cs")

def skindef_block(item_id, fields):
    """fields: {name: value_expr_string}. Returns the paste-in block; omits empty/None."""
    parts = ["%s = %s" % (n, fields[n]) for n in FIELDS if fields.get(n) not in (None, "", [])]
    return "{ %d, new SkinDef { %s } }," % (int(item_id), ", ".join(parts))

def _strip_comments(s):
    out = []; i = 0; n = len(s)
    while i < n:
        c = s[i]; nx = s[i+1] if i+1 < n else ""
        if c == '"':
            out.append(c); i += 1
            while i < n:
                out.append(s[i])
                if s[i] == "\\" and i+1 < n: out.append(s[i+1]); i += 2; continue
                if s[i] == '"': i += 1; break
                i += 1
            continue
        if c == "/" and nx == "/":
            while i < n and s[i] != "\n": i += 1
            continue
        if c == "/" and nx == "*":
            i += 2
            while i < n and not (s[i] == "*" and i+1 < n and s[i+1] == "/"): i += 1
            i += 2; continue
        out.append(c); i += 1
    return "".join(out)

def load_source(path):
    """Return {id: block_string} parsed from a WeaponSkinDictionaries.cs Skins table."""
    b = _strip_comments(open(path, encoding="utf-8").read())
    m = re.search(r"Skins\s*=\s*new\s+Dictionary<int,\s*SkinDef>\s*", b)
    i = b.index("{", m.end()); depth = 0; j = i
    while j < len(b):
        c = b[j]
        if c == "{": depth += 1
        elif c == "}":
            depth -= 1
            if depth == 0: break
        j += 1
    body = b[i+1:j]; out = {}; k = 0; nn = len(body)
    while k < nn:
        if body[k] == "{":
            d = 1; s = k + 1; ins = False
            while s < nn and d > 0:
                cc = body[s]
                if ins:
                    if cc == "\\": s += 2; continue
                    if cc == '"': ins = False
                elif cc == '"': ins = True
                elif cc == "{": d += 1
                elif cc == "}":
                    d -= 1
                    if d == 0: break
                s += 1
            ent = body[k:s+1]
            mid = re.match(r"\{\s*(\d+)\s*,", ent)
            if mid: out[int(mid.group(1))] = re.sub(r"\s+", " ", ent).strip() + ","
            k = s + 1; continue
        k += 1
    return out

def main():
    a = sys.argv
    path = a[a.index("--file")+1] if "--file" in a else DEFAULT
    src = load_source(path)
    if "--list" in a:
        for i in sorted(src): print(i)
        return
    if "--id" in a:
        i = int(a[a.index("--id")+1])
        print(src.get(i, "// id %d not found in %s" % (i, path)))
        return
    print(__doc__)

if __name__ == "__main__":
    main()
