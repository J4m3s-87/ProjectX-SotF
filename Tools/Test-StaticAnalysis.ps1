# ============================================================
# Project X — Static Analysis Test Suite
# ============================================================
# Validates source code structure without running the game.
# Tests:  1. Tier guard coverage (#if SERVER/OWNER/!SERVER)
#         2. Namespace consistency
#         3. No hardcoded paths
#         4. Config documentation
#         5. Project file health
#         6. Credits & licenses
#         7. Module coverage in CREDITS.md
#
# Usage:  pwsh Tools/Test-StaticAnalysis.ps1
# Exit:   0 = all pass, 1 = failures found
# ============================================================

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$modulesDir = Join-Path $projectRoot "Dev\ProjectX.Master\Modules"

# ============================================================
# Test Infrastructure (shared pattern with Test-ProjectX.ps1)
# ============================================================
$script:passCount = 0
$script:failCount = 0
$script:warnCount = 0
$script:skipCount = 0
$script:results = @()

function Test-Pass($name, $detail) {
    $script:passCount++
    $script:results += [PSCustomObject]@{ Status = "PASS"; Name = $name; Detail = $detail }
    Write-Host "  [PASS] $name" -ForegroundColor Green
    if ($detail) { Write-Host "         $detail" -ForegroundColor DarkGray }
}

function Test-Fail($name, $detail) {
    $script:failCount++
    $script:results += [PSCustomObject]@{ Status = "FAIL"; Name = $name; Detail = $detail }
    Write-Host "  [FAIL] $name" -ForegroundColor Red
    if ($detail) { Write-Host "         $detail" -ForegroundColor DarkGray }
}

function Test-Warn($name, $detail) {
    $script:warnCount++
    $script:results += [PSCustomObject]@{ Status = "WARN"; Name = $name; Detail = $detail }
    Write-Host "  [WARN] $name" -ForegroundColor Yellow
    if ($detail) { Write-Host "         $detail" -ForegroundColor DarkGray }
}

function Test-Skip($name, $detail) {
    $script:skipCount++
    $script:results += [PSCustomObject]@{ Status = "SKIP"; Name = $name; Detail = $detail }
    Write-Host "  [SKIP] $name" -ForegroundColor DarkGray
    if ($detail) { Write-Host "         $detail" -ForegroundColor DarkGray }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Project X - Static Analysis Suite" -ForegroundColor Cyan
Write-Host "  $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor DarkGray
Write-Host "============================================" -ForegroundColor Cyan

# ============================================================
# Get all module .cs files (excluding disabled modules)
# ============================================================
$allCsFiles = Get-ChildItem -Recurse -Path $modulesDir -Include "*.cs" |
    Where-Object { $_.FullName -notmatch "\\StoneGate\\" -and $_.FullName -notmatch "\\OpenSesame\\" -and $_.FullName -notmatch "\\PrefabRepair\\" }

# ============================================================
# SECTION 1: Namespace Consistency
# ============================================================
Write-Host ""
Write-Host "[1/7] Namespace Consistency" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$badNamespace = @()
foreach ($file in $allCsFiles) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match "namespace\s+(\S+)") {
        $ns = $matches[1]
        if ($ns -notmatch "^ProjectX\.(Master|Owner|Server|Client)") {
            $badNamespace += "$($file.Name): $ns"
        }
    }
}

if ($badNamespace.Count -eq 0) {
    Test-Pass "All namespaces use ProjectX.* prefix" "$($allCsFiles.Count) files checked"
}
else {
    Test-Fail "Non-standard namespaces found" "$($badNamespace.Count): $($badNamespace -join '; ')"
}

# ============================================================
# SECTION 2: Tier Guard Coverage
# ============================================================
Write-Host ""
Write-Host "[2/7] Tier Guard Coverage" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$moduleDirs = Get-ChildItem -Directory $modulesDir |
    Where-Object { $_.Name -notin @("StoneGate", "OpenSesame", "PrefabRepair") }

$guarded = 0
$unguarded = @()
foreach ($dir in $moduleDirs) {
    $files = Get-ChildItem -Path $dir.FullName -Include "*.cs" -Recurse
    $hasGuard = $false
    foreach ($f in $files) {
        $content = Get-Content $f.FullName -Raw
        if ($content -match "#if\s+(SERVER|OWNER|!SERVER|!OWNER|CLIENT|!CLIENT)") {
            $hasGuard = $true
            break
        }
    }
    if ($hasGuard) {
        $guarded++
    }
    else {
        $unguarded += $dir.Name
    }
}

$guardPct = if ($moduleDirs.Count -gt 0) { [math]::Round(($guarded / $moduleDirs.Count) * 100) } else { 0 }
if ($unguarded.Count -eq 0) {
    Test-Pass "All modules have tier guards" "$guarded/$($moduleDirs.Count) modules"
}
elseif ($guardPct -ge 70) {
    Test-Warn "Some modules lack tier guards" "$guardPct% covered. Missing: $($unguarded -join ', ')"
}
else {
    Test-Fail "Low tier guard coverage" "$guardPct%. Missing: $($unguarded -join ', ')"
}

# ============================================================
# SECTION 3: Hardcoded Paths
# ============================================================
Write-Host ""
Write-Host "[3/7] No Hardcoded Paths" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$hardcoded = @()
foreach ($file in $allCsFiles) {
    $lines = Get-Content $file.FullName
    $lineNum = 0
    foreach ($line in $lines) {
        $lineNum++
        # Match Windows absolute paths (C:\, D:\, G:\) but ignore comments about paths
        if ($line -match '[A-Z]:\\\\[A-Za-z]' -and $line -notmatch "^\s*//" -and $line -notmatch "^\s*\*" -and $line -notmatch "///") {
            $hardcoded += "$($file.Name):$lineNum"
        }
    }
}

if ($hardcoded.Count -eq 0) {
    Test-Pass "No hardcoded paths in source" "$($allCsFiles.Count) files checked"
}
else {
    Test-Warn "Hardcoded paths found" "$($hardcoded.Count) occurrence(s): $($hardcoded -join ', ')"
}

# ============================================================
# SECTION 4: Config Entry Documentation
# ============================================================
Write-Host ""
Write-Host "[4/7] Config Documentation" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$totalConfigs = 0
$documentedConfigs = 0
foreach ($file in $allCsFiles) {
    $lines = Get-Content $file.FullName
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match "ConfigEntry<") {
            $totalConfigs++
            # Check if previous 1-5 lines have XML doc or comment
            $hasDoc = $false
            for ($j = [Math]::Max(0, $i - 5); $j -lt $i; $j++) {
                if ($lines[$j] -match "///|/\*\*|// ") {
                    $hasDoc = $true
                    break
                }
            }
            if ($hasDoc) { $documentedConfigs++ }
        }
    }
}

$docPct = if ($totalConfigs -gt 0) { [math]::Round(($documentedConfigs / $totalConfigs) * 100) } else { 100 }
if ($docPct -ge 80) {
    Test-Pass "Config entries documented" "$documentedConfigs/$totalConfigs ($docPct%)"
}
elseif ($docPct -ge 50) {
    Test-Warn "Some config entries lack docs" "$documentedConfigs/$totalConfigs ($docPct%)"
}
else {
    Test-Fail "Most config entries undocumented" "$documentedConfigs/$totalConfigs ($docPct%)"
}

# ============================================================
# SECTION 5: Project File Health
# ============================================================
Write-Host ""
Write-Host "[5/7] Project File Health" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

# manifest.json exists
$manifestPath = Join-Path $projectRoot "Dev\ProjectX.Owner\manifest.json"
if (-not (Test-Path $manifestPath)) {
    $manifestPath = Join-Path $projectRoot "Dev\ProjectX.Master\manifest.json"
}
$manifestPaths = Get-ChildItem -Recurse -Path (Join-Path $projectRoot "Dev") -Filter "manifest.json"
if ($manifestPaths.Count -gt 0) {
    Test-Pass "manifest.json found" "$($manifestPaths.Count) manifest file(s)"
}
else {
    Test-Fail "manifest.json missing" "No manifest.json in Dev/"
}

# .csproj files compile
$csprojFiles = Get-ChildItem -Recurse -Path (Join-Path $projectRoot "Dev") -Filter "*.csproj"
$validCsproj = 0
foreach ($proj in $csprojFiles) {
    $content = Get-Content $proj.FullName -Raw
    if ($content -match "<TargetFramework>net6.0</TargetFramework>") {
        $validCsproj++
    }
}
if ($validCsproj -eq $csprojFiles.Count) {
    Test-Pass "All .csproj target net6.0" "$validCsproj/$($csprojFiles.Count)"
}
else {
    Test-Warn "Some .csproj have wrong target" "$validCsproj/$($csprojFiles.Count) target net6.0"
}

# ============================================================
# SECTION 6: Credits & Licenses
# ============================================================
Write-Host ""
Write-Host "[6/7] Credits & Licenses" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$creditsPath = Join-Path $projectRoot "CREDITS.md"
$licensesPath = Join-Path $projectRoot "LICENSES.md"

if (Test-Path $creditsPath) {
    $creditsContent = Get-Content $creditsPath -Raw
    $creditsLines = (Get-Content $creditsPath).Count
    Test-Pass "CREDITS.md exists" "$creditsLines lines"
}
else {
    Test-Fail "CREDITS.md missing" "Create CREDITS.md at project root"
}

if (Test-Path $licensesPath) {
    $licensesContent = Get-Content $licensesPath -Raw
    # Check for key licenses
    $hasLGPL = $licensesContent -match "LGPL"
    $hasMIT = $licensesContent -match "MIT"
    if ($hasLGPL -and $hasMIT) {
        Test-Pass "LICENSES.md covers LGPL and MIT" ""
    }
    else {
        Test-Warn "LICENSES.md may be incomplete" "LGPL: $hasLGPL, MIT: $hasMIT"
    }
}
else {
    Test-Fail "LICENSES.md missing" "Required for LGPL compliance"
}

# ============================================================
# SECTION 7: Module Coverage in Credits
# ============================================================
Write-Host ""
Write-Host "[7/7] Module Coverage in Credits" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

if ((Test-Path $creditsPath)) {
    $creditsText = Get-Content $creditsPath -Raw
    $uncredited = @()
    foreach ($dir in $moduleDirs) {
        $modName = $dir.Name
        # Check if module name appears in CREDITS.md (case insensitive)
        if ($creditsText -notmatch "(?i)$modName") {
            $uncredited += $modName
        }
    }
    if ($uncredited.Count -eq 0) {
        Test-Pass "All modules mentioned in CREDITS.md" "$($moduleDirs.Count) modules"
    }
    elseif ($uncredited.Count -le 3) {
        Test-Warn "Some modules missing from CREDITS.md" "$($uncredited -join ', ')"
    }
    else {
        Test-Fail "Many modules missing from CREDITS.md" "$($uncredited.Count): $($uncredited -join ', ')"
    }
}
else {
    Test-Skip "Module credit check" "CREDITS.md not found"
}

# ============================================================
# SECTION 8: Code Metrics
# ============================================================
Write-Host ""
Write-Host "[+] Code Metrics" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$totalLines = 0
$totalFiles = $allCsFiles.Count
foreach ($f in $allCsFiles) {
    $totalLines += (Get-Content $f.FullName | Measure-Object).Count
}
Write-Host "    Source files:  $totalFiles" -ForegroundColor DarkGray
Write-Host "    Lines of C#:  $totalLines" -ForegroundColor DarkGray
Write-Host "    Modules:      $($moduleDirs.Count)" -ForegroundColor DarkGray

# ============================================================
# Summary
# ============================================================
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
$total = $script:passCount + $script:failCount + $script:warnCount + $script:skipCount
$active = $total - $script:skipCount
$passRate = if ($active -gt 0) { [math]::Round(($script:passCount / $active) * 100) } else { 0 }

Write-Host "  Results: $total tests ($active active, $($script:skipCount) skipped)" -ForegroundColor Cyan
Write-Host "    PASS: $($script:passCount)" -ForegroundColor Green
Write-Host "    FAIL: $($script:failCount)" -ForegroundColor $(if ($script:failCount -gt 0) { "Red" } else { "Green" })
Write-Host "    WARN: $($script:warnCount)" -ForegroundColor $(if ($script:warnCount -gt 0) { "Yellow" } else { "Green" })
Write-Host "    SKIP: $($script:skipCount)" -ForegroundColor DarkGray
Write-Host "    Pass Rate: $passRate% (target: 80%)" -ForegroundColor $(if ($passRate -ge 80) { "Green" } elseif ($passRate -ge 60) { "Yellow" } else { "Red" })
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

if ($script:failCount -gt 0) {
    Write-Host "  FAILED - $($script:failCount) test(s) need attention" -ForegroundColor Red
    exit 1
}
elseif ($script:warnCount -gt 0) {
    Write-Host "  PASSED with warnings ($passRate%)" -ForegroundColor Yellow
    exit 0
}
else {
    Write-Host "  ALL TESTS PASSED ($passRate%)" -ForegroundColor Green
    exit 0
}
