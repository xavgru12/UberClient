<#
.SYNOPSIS
    Copy the weapon skin art into a game install, and emit a patcher manifest for it.

.DESCRIPTION
    The skins load their art from "<install>/UberStrike_Data/Skins/" at runtime, but the repo
    keeps it in "Assembly-CSharp/WeaponSkins/". NOTHING in the build copies between the two,
    and the folder names differ, so a build of Assembly-CSharp.dll alone produces no skins and
    no error -- the loader logs "skin file not found" and every weapon renders stock.

    That is not hypothetical: it is exactly what happened on first integration, and it is
    indistinguishable from "the patch does not work". This script is the missing step.

    It also prints an Entry.txt-style manifest (relative path, size, SHA256) so the patcher can
    ship the same files to players rather than each tester copying them by hand.

.PARAMETER GamePath
    The UberStrike install. Defaults to the usual Steam location.

.PARAMETER ManifestOut
    Optional path to write the patcher manifest to. Printed to the console regardless.

.PARAMETER WhatIf
    Show what would be copied without copying it.

.EXAMPLE
    .\tools\Deploy-WeaponSkins.ps1
    .\tools\Deploy-WeaponSkins.ps1 -GamePath "D:\Games\UberStrike" -ManifestOut skins.entry.txt
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [string]$GamePath = "C:\Program Files (x86)\Steam\steamapps\common\UberStrike",
    [string]$ManifestOut
)

$ErrorActionPreference = "Stop"
function Say($m) { Write-Host "  $m" }
function Die($m) { Write-Host "`n  FAILED: $m" -ForegroundColor Red; exit 1 }

$repoRoot = Split-Path -Parent $PSScriptRoot
$src = Join-Path $repoRoot "Assembly-CSharp\WeaponSkins"
if (-not (Test-Path -LiteralPath $src)) { Die "no art folder at '$src'" }

$dataDir = Get-ChildItem -LiteralPath $GamePath -Directory -Filter "*_Data" -ErrorAction SilentlyContinue |
           Select-Object -First 1
if (-not $dataDir) { Die "no *_Data folder under '$GamePath' -- is that the game install?" }

# Must match WeaponSkinHelper.SkinFolder. It is "Skins", NOT "WeaponSkins": the repo folder and
# the runtime folder deliberately have different names, which is half of why this step was
# missed in the first place.
$dst = Join-Path $dataDir.FullName "Skins"
Say "source : $src"
Say "target : $dst"

# Only what the loader actually reads. It resolves "<stem>.jpg" plus "<stem>.alpha.png", falling
# back to a single "<stem>.png", and ProxyItem reads "<stem>_Icon.png".
$wanted = Get-ChildItem -LiteralPath $src -File |
          Where-Object { $_.Name -match '\.(jpg|png)$' -and $_.Name -notmatch '\.superseded$' }
if (-not $wanted) { Die "no art files found in '$src'" }

if ($PSCmdlet.ShouldProcess($dst, "copy $($wanted.Count) skin files")) {
    if (-not (Test-Path -LiteralPath $dst)) { New-Item -ItemType Directory -Path $dst | Out-Null }
    $copied = 0; $skipped = 0
    foreach ($f in $wanted) {
        $target = Join-Path $dst $f.Name
        # skip byte-identical files so re-running is cheap and does not churn timestamps
        if (Test-Path -LiteralPath $target) {
            $a = (Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash
            $b = (Get-FileHash -LiteralPath $target   -Algorithm SHA256).Hash
            if ($a -eq $b) { $skipped++; continue }
        }
        Copy-Item -LiteralPath $f.FullName -Destination $target -Force
        $copied++
    }
    Say "copied $copied, already current $skipped, total $($wanted.Count)"
}

# --- verify every skin the helper references actually resolves -----------------------------
Say ""
Say "verifying against WeaponSkinHelper..."
$helper = Get-Content -LiteralPath (Join-Path $repoRoot "Assembly-CSharp\WeaponSkinHelper.cs") -Raw
$refs = [regex]::Matches($helper, '\{\s*(90\d\d)\s*,\s*"([^"]+\.png)"\s*\}')
$missing = @()
$seen = @{}
foreach ($m in $refs) {
    $file = $m.Groups[2].Value
    if ($seen.ContainsKey($file)) { continue }
    $seen[$file] = $true
    $stem = [IO.Path]::GetFileNameWithoutExtension($file)
    # a skin is satisfied by the jpeg pair OR by the single rgba png
    $ok = (Test-Path -LiteralPath (Join-Path $dst "$stem.jpg")) -or
          (Test-Path -LiteralPath (Join-Path $dst $file))
    if (-not $ok) { $missing += "$($m.Groups[1].Value) -> $file" }
}
if ($missing) {
    Write-Host "`n  MISSING ART -- these skins would render stock:" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
    Die "$($missing.Count) referenced file(s) not present in the install"
}
Say "all $($seen.Count) referenced skins resolve"

# --- patcher manifest -----------------------------------------------------------------------
$lines = foreach ($f in ($wanted | Sort-Object Name)) {
    $h = (Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash.ToLower()
    "Skins/{0}`t{1}`t{2}" -f $f.Name, $f.Length, $h
}
$total = ($wanted | Measure-Object Length -Sum).Sum
Say ""
Say ("patcher manifest -- {0} files, {1:N1} MB" -f $wanted.Count, ($total / 1MB))
Say "format: <relative path><TAB><bytes><TAB><sha256>"
$lines | Select-Object -First 6 | ForEach-Object { Write-Host "    $_" }
if ($lines.Count -gt 6) { Say "    ... and $($lines.Count - 6) more" }
if ($ManifestOut) {
    Set-Content -Path $ManifestOut -Value $lines -Encoding utf8NoBOM
    Say "written to $ManifestOut"
}
Write-Host ""
