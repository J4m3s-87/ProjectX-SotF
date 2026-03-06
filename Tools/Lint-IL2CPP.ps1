# ============================================================
# IL2CPP Banned API Linter for Project X
# ============================================================
# Reads il2cpp_banned_apis.txt and scans all .cs files in
# Dev/ProjectX.Master/ for known-stripped API usage.
#
# Usage: pwsh Tools/Lint-IL2CPP.ps1
# Exit code: 1 if any ERROR-level violations found, 0 otherwise
# ============================================================

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$bannedFile = Join-Path $scriptDir "il2cpp_banned_apis.txt"
$sourceDir = Join-Path (Split-Path -Parent $scriptDir) "Dev" "ProjectX.Master"

if (-not (Test-Path $bannedFile)) {
    Write-Host "[LINT] Banned APIs file not found: $bannedFile" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $sourceDir)) {
    Write-Host "[LINT] Source directory not found: $sourceDir" -ForegroundColor Red
    exit 1
}

Write-Host "[LINT] IL2CPP Banned API Check" -ForegroundColor Cyan
Write-Host "[LINT] Scanning: $sourceDir" -ForegroundColor DarkGray

# Parse banned APIs (skip comments and empty lines)
$rules = @()
Get-Content $bannedFile | ForEach-Object {
    $line = $_.Trim()
    if ($line -and -not $line.StartsWith("#")) {
        $parts = $line -split "\|", 3
        if ($parts.Count -eq 3) {
            $rules += [PSCustomObject]@{
                Pattern     = $parts[0].Trim()
                Severity    = $parts[1].Trim()
                Description = $parts[2].Trim()
            }
        }
    }
}

Write-Host "[LINT] Loaded $($rules.Count) rules from banned_apis.txt" -ForegroundColor DarkGray

$errorCount = 0
$warnCount = 0
$violations = @()

# Directories excluded from compilation in .csproj (decompiled 3rd-party code)
$excludedDirs = @("StoneGate", "OpenSesame", "PrefabRepair")

# Scan all .cs files (excluding modules removed from build)
$csFiles = Get-ChildItem -Path $sourceDir -Filter "*.cs" -Recurse | Where-Object {
    $rel = $_.FullName.Replace($sourceDir, "").TrimStart("\", "/")
    -not ($excludedDirs | Where-Object { $rel.StartsWith("Modules\$_\") })
}

foreach ($file in $csFiles) {
    $lines = Get-Content $file.FullName
    $lineNum = 0
    foreach ($codeLine in $lines) {
        $lineNum++
        
        # Skip comments
        $trimmed = $codeLine.TrimStart()
        if ($trimmed.StartsWith("//") -or $trimmed.StartsWith("*") -or $trimmed.StartsWith("///")) {
            continue
        }
        
        foreach ($rule in $rules) {
            if ($codeLine -match $rule.Pattern) {
                $relPath = $file.FullName.Replace($sourceDir, "").TrimStart("\", "/")
                $violations += [PSCustomObject]@{
                    File        = $relPath
                    Line        = $lineNum
                    Severity    = $rule.Severity
                    Pattern     = $rule.Pattern
                    Description = $rule.Description
                    Code        = $codeLine.Trim()
                }
                if ($rule.Severity -eq "ERROR") { $errorCount++ }
                else { $warnCount++ }
            }
        }
    }
}

# Report results
if ($violations.Count -eq 0) {
    Write-Host "[LINT] ✓ No banned API usage found" -ForegroundColor Green
    exit 0
}

Write-Host ""
Write-Host "[LINT] Found $($violations.Count) violation(s):" -ForegroundColor Yellow
Write-Host ""

foreach ($v in $violations) {
    $color = if ($v.Severity -eq "ERROR") { "Red" } else { "Yellow" }
    $icon = if ($v.Severity -eq "ERROR") { "✗" } else { "⚠" }
    Write-Host "  $icon [$($v.Severity)] $($v.File):$($v.Line)" -ForegroundColor $color
    Write-Host "    Code: $($v.Code)" -ForegroundColor DarkGray
    Write-Host "    Fix:  $($v.Description)" -ForegroundColor DarkGray
    Write-Host ""
}

Write-Host "[LINT] Summary: $errorCount error(s), $warnCount warning(s)" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Yellow" })

if ($errorCount -gt 0) {
    exit 1
}
else {
    exit 0
}
