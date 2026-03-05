# ============================================================
# Project X — Integration Test & Health Report
# ============================================================
# Analyzes the latest game log to verify module health.
# Tests:  1. Build verification (DLL exists, recent)
#         2. Module initialization (each module logged startup)
#         3. Crash detection (MissingMethodException, etc.)
#         4. Runtime health (no repeated errors)
#         5. IL2CPP lint (banned API check)
#
# Usage:  pwsh Tools/Test-ProjectX.ps1
#         make -f Tools/Makefile test
# Exit:   0 = all pass, 1 = failures found
# ============================================================

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$buildDll = Join-Path $projectRoot "Dev\ProjectX.Owner\bin\Release\net6.0\ProjectX.Owner.dll"

# Auto-detect game directory via Steam registry + common paths
$gameDir = $null
$steamPaths = @()
try { $reg = Get-ItemProperty "HKLM:\SOFTWARE\WOW6432Node\Valve\Steam" -Name "InstallPath" -EA SilentlyContinue; if ($reg) { $steamPaths += $reg.InstallPath } } catch { }
try { $reg = Get-ItemProperty "HKCU:\Software\Valve\Steam" -Name "SteamPath" -EA SilentlyContinue; if ($reg) { $steamPaths += $reg.SteamPath -replace '/', '\' } } catch { }
$steamPaths += @("C:\Program Files (x86)\Steam", "D:\SteamLibrary", "G:\SteamLibrary")
foreach ($sp in ($steamPaths | Select-Object -Unique)) {
    $vdf = Join-Path $sp "steamapps\libraryfolders.vdf"
    if (Test-Path $vdf) { [regex]::Matches((Get-Content $vdf -Raw), '"path"\s+"([^"]+)"') | ForEach-Object { $steamPaths += $_.Groups[1].Value -replace '\\\\', '\' } }
}
foreach ($sp in ($steamPaths | Select-Object -Unique)) {
    $candidate = Join-Path $sp "steamapps\common\Sons Of The Forest"
    if (Test-Path (Join-Path $candidate "SonsOfTheForest.exe")) { $gameDir = $candidate; break }
}
if (-not $gameDir) { Write-Host "[WARN] Could not auto-detect game directory. Log/deploy checks will be skipped." -ForegroundColor Yellow }

$logFile = if ($gameDir) { Join-Path $gameDir "_Redloader\Latest.log" } else { $null }
$ownerDll = if ($gameDir) { Join-Path $gameDir "Mods\ProjectX.Owner.dll" } else { $null }

# ============================================================
# Test Infrastructure
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
Write-Host "  Project X - Integration Test Suite" -ForegroundColor Cyan
Write-Host "  $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor DarkGray
Write-Host "============================================" -ForegroundColor Cyan

# ============================================================
# SECTION 1: Build Verification
# ============================================================
Write-Host ""
Write-Host "[1/5] Build Verification" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

# Test: Build output exists
if (Test-Path $buildDll) {
    $buildInfo = Get-Item $buildDll
    $age = (Get-Date) - $buildInfo.LastWriteTime
    $sizeKB = [math]::Round($buildInfo.Length / 1KB)
    Test-Pass "Build output exists" "$sizeKB KB, built $($buildInfo.LastWriteTime.ToString('HH:mm:ss'))"

    if ($age.TotalHours -lt 24) {
        Test-Pass "Build is recent" "$([math]::Round($age.TotalMinutes)) minutes old"
    }
    else {
        Test-Warn "Build is stale" "$([math]::Round($age.TotalHours)) hours old"
    }
}
else {
    Test-Fail "Build output exists" "Not found: $buildDll"
}

# Test: Deployed DLL exists and matches build
if (Test-Path $ownerDll) {
    $deployInfo = Get-Item $ownerDll
    $deploySizeKB = [math]::Round($deployInfo.Length / 1KB)

    if ((Test-Path $buildDll)) {
        $buildInfo = Get-Item $buildDll
        if ($deployInfo.Length -eq $buildInfo.Length) {
            Test-Pass "Deployed DLL matches build" "$deploySizeKB KB"
        }
        else {
            Test-Warn "Deployed DLL differs from build" "Deploy: $deploySizeKB KB vs Build: $([math]::Round($buildInfo.Length / 1KB)) KB"
        }
    }
    else {
        Test-Pass "Deployed DLL exists" "$deploySizeKB KB"
    }
}
else {
    Test-Fail "Deployed DLL exists" "Not found: $ownerDll"
}

# ============================================================
# SECTION 2: Log File Health
# ============================================================
Write-Host ""
Write-Host "[2/5] Log Availability" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$hasLog = Test-Path $logFile
if (-not $hasLog) {
    Test-Fail "Game log exists" "Not found: $logFile"
    Write-Host ""
    Write-Host "  [SKIP] Sections 3-4 require game log" -ForegroundColor DarkGray
}
else {
    $logContent = Get-Content $logFile -Encoding UTF8
    $logLines = $logContent.Count
    $logInfo = Get-Item $logFile
    $logAge = (Get-Date) - $logInfo.LastWriteTime

    Test-Pass "Game log exists" "$logLines lines, last modified $($logInfo.LastWriteTime.ToString('HH:mm:ss'))"

    if ($logAge.TotalHours -lt 4) {
        Test-Pass "Log is from recent session" "$([math]::Round($logAge.TotalMinutes)) min ago"
    }
    else {
        Test-Warn "Log is from old session" "$([math]::Round($logAge.TotalHours)) hours ago - results may be stale"
    }
}

# ============================================================
# SECTION 3: Module Initialization
# ============================================================
Write-Host ""
Write-Host "[3/5] Module Initialization" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

if (-not $hasLog) {
    Test-Skip "Module init checks" "No game log available"
}
else {
    $modules = @(
        @{ Name = "ProjectX Core"; Tag = "ProjectX_Owner_Edition"; Pattern = "Game Started" }
        @{ Name = "Hotbar"; Tag = "Hotbar"; Pattern = "active|Controller found" }
        @{ Name = "AmmoUI"; Tag = "AmmoUI"; Pattern = "Harmony patch|Ammo type" }
        @{ Name = "ScaryCross"; Tag = "ScaryCross"; Pattern = "Harmony patch|Initialize|ElectricLight" }
        @{ Name = "MeatDryer"; Tag = "MeatDryer"; Pattern = "Tracked rack|Season=" }
        @{ Name = "WaterCollectors"; Tag = "WaterCollectors"; Pattern = "Tracked catcher|Tracked heat" }
        @{ Name = "BuilderStacks"; Tag = "BuilderStacks"; Pattern = "Pointer resolved|Stack" }
        @{ Name = "LootRespawn"; Tag = "LootRespawn"; Pattern = "respawn|Loot" }
        @{ Name = "Zipline"; Tag = "Zipline"; Pattern = "Zipline" }
        @{ Name = "CraftingSpeed"; Tag = "CraftingSpeed"; Pattern = "Crafting" }
        @{ Name = "Relocator"; Tag = "Relocator"; Pattern = "Relocator" }
        @{ Name = "RaidCustomizer"; Tag = "RaidCustomizer"; Pattern = "Raid" }
        @{ Name = "Stack"; Tag = "Stack"; Pattern = "Stack" }
        @{ Name = "BroadcastMessage"; Tag = "BroadcastMessage"; Pattern = "Broadcast|Message" }
        @{ Name = "IntegrityConfig"; Tag = "IntegrityConfig"; Pattern = "Integrity|hash|verified" }
        @{ Name = "ProjectXGUI"; Tag = "ProjectXGUI"; Pattern = "GUI|menu|registered" }
    )

    foreach ($mod in $modules) {
        $tagLines = $logContent | Where-Object { $_ -match "\[$([regex]::Escape($mod.Tag))\]" }
        if ($tagLines.Count -gt 0) {
            $initLine = $tagLines | Where-Object { $_ -match $mod.Pattern } | Select-Object -First 1
            if ($initLine) {
                $ts = ""
                if ($initLine -match "^\[(\d{2}:\d{2}:\d{2}\.\d{3})\]") { $ts = $matches[1] }
                Test-Pass "$($mod.Name) initialized" "at $ts ($($tagLines.Count) log entries)"
            }
            else {
                Test-Warn "$($mod.Name) has logs but no init" "$($tagLines.Count) entries"
            }
        }
        else {
            Test-Skip "$($mod.Name) not loaded" "No [$($mod.Tag)] entries in log"
        }
    }
}

# ============================================================
# SECTION 4: Crash & Error Detection
# ============================================================
Write-Host ""
Write-Host "[4/5] Crash & Error Detection" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

if (-not $hasLog) {
    Test-Skip "Crash detection" "No game log available"
}
else {
    # MissingMethodException (IL2CPP stripping)
    $missingMethod = $logContent | Where-Object { $_ -match "MissingMethodException|Method not found" }
    if ($missingMethod.Count -eq 0) {
        Test-Pass "No MissingMethodException" "IL2CPP stripping issues: none"
    }
    else {
        $uniqueMethods = @()
        foreach ($line in $missingMethod) {
            if ($line -match "Method not found: '([^']+)'") {
                $method = $matches[1]
                if ($uniqueMethods -notcontains $method) { $uniqueMethods += $method }
            }
        }
        Test-Fail "MissingMethodException detected" "$($missingMethod.Count) occurrence(s): $($uniqueMethods -join '; ')"
    }

    # CRASHED pattern (our try/catch crash logs)
    $crashed = $logContent | Where-Object { $_ -match "CRASHED" }
    if ($crashed.Count -eq 0) {
        Test-Pass "No CRASHED patterns" "All ManualUpdate loops healthy"
    }
    else {
        $crashModules = @()
        foreach ($line in $crashed) {
            if ($line -match "\[([^\]]+)\].*CRASHED") {
                if ($crashModules -notcontains $matches[1]) { $crashModules += $matches[1] }
            }
        }
        Test-Fail "CRASHED pattern detected" "$($crashed.Count) crash(es) in: $($crashModules -join ', ')"
    }

    # NullReferenceException
    $nullRef = $logContent | Where-Object { $_ -match "NullReferenceException" }
    if ($nullRef.Count -eq 0) {
        Test-Pass "No NullReferenceException" "Clean execution"
    }
    else {
        Test-Warn "NullReferenceException detected" "$($nullRef.Count) occurrence(s)"
    }

    # Harmony patch failures
    $harmonyFail = $logContent | Where-Object { $_ -match "Harmony.*fail|patch.*fail|TargetInvocationException" }
    if ($harmonyFail.Count -eq 0) {
        Test-Pass "No Harmony patch failures" "All patches applied cleanly"
    }
    else {
        Test-Fail "Harmony patch failure" "$($harmonyFail.Count) failure(s)"
    }

    # Error spam check
    $errorLines = $logContent | Where-Object { $_ -match "ERROR|Exception" -and $_ -notmatch "ErrorAction|SilentlyContinue|0 Error" }
    if ($errorLines.Count -gt 10) {
        Test-Warn "Error spam detected" "$($errorLines.Count) error-like lines"
    }
    elseif ($errorLines.Count -gt 0) {
        Test-Pass "Minimal errors" "$($errorLines.Count) error-like line(s)"
    }
    else {
        Test-Pass "No errors in log" "Clean session"
    }

    # ============================================================
    # SECTION 4B: Feature Verification (from logs)
    # ============================================================
    Write-Host ""
    Write-Host "[4B] Feature Verification" -ForegroundColor Yellow
    Write-Host "---" -ForegroundColor DarkGray

    # Screw structure durability
    $screwAwake = $logContent | Where-Object { $_ -match "ScrewAwake:" }
    if ($screwAwake.Count -gt 0) {
        Test-Pass "Screw durability applied" "$($screwAwake.Count) screw structures tracked"
    }
    else {
        Test-Skip "Screw durability" "No ScrewAwake entries (not tested this session)"
    }

    # Screw structure repair
    $screwRepair = $logContent | Where-Object { $_ -match "Screw structures: repaired" }
    if ($screwRepair.Count -gt 0) {
        $lastRepair = $screwRepair | Select-Object -Last 1
        Test-Pass "Screw repair triggered" "$($screwRepair.Count) repair event(s)"
    }
    else {
        Test-Skip "Screw repair" "No repair events (not tested this session)"
    }

    # Config sync
    $configSync = $logContent | Where-Object { $_ -match "\[ConfigSync" }
    if ($configSync.Count -gt 0) {
        Test-Pass "ConfigSync active" "$($configSync.Count) sync entries"
    }
    else {
        Test-Skip "ConfigSync" "No ConfigSync entries"
    }

    # Permission events
    $permEvents = $logContent | Where-Object { $_ -match "\[Permission" }
    if ($permEvents.Count -gt 0) {
        Test-Pass "Permission system active" "$($permEvents.Count) permission entries"
    }
    else {
        Test-Skip "Permission system" "No permission entries (single player?)"
    }

    # Duplicate module init detection
    $initCounts = @{}
    foreach ($mod in $modules) {
        $initLines = $logContent | Where-Object { $_ -match "\[$([regex]::Escape($mod.Tag))\]" -and $_ -match "Initialized|Module Init|Init\b" }
        if ($initLines.Count -gt 1) {
            $initCounts[$mod.Name] = $initLines.Count
        }
    }
    if ($initCounts.Count -eq 0) {
        Test-Pass "No duplicate module inits" "All modules initialized once"
    }
    else {
        $dupes = ($initCounts.GetEnumerator() | ForEach-Object { "$($_.Key)($($_.Value)x)" }) -join ", "
        Test-Warn "Duplicate module init detected" $dupes
    }

    # Session stability (lasted >60 seconds)
    $firstLine = $logContent | Select-Object -First 1
    $lastLine = $logContent | Select-Object -Last 1
    $sessionStable = $false
    if ($firstLine -match "\[(\d{2}):(\d{2}):(\d{2})" -and $lastLine -match "\[(\d{2}):(\d{2}):(\d{2})") {
        # Extract timestamps
        $allTimestamps = @()
        if ($firstLine -match "\[(\d{2}:\d{2}:\d{2})") { $startTs = $matches[1] }
        if ($lastLine -match "\[(\d{2}:\d{2}:\d{2})") { $endTs = $matches[1] }
        if ($startTs -and $endTs) {
            $startSec = ([TimeSpan]::Parse($startTs)).TotalSeconds
            $endSec = ([TimeSpan]::Parse($endTs)).TotalSeconds
            $duration = $endSec - $startSec
            if ($duration -lt 0) { $duration += 86400 } # wrapped midnight
            if ($duration -gt 60) {
                Test-Pass "Session stable (>60s)" "$([math]::Round($duration / 60, 1)) minutes"
                $sessionStable = $true
            }
            else {
                Test-Warn "Short session" "$([math]::Round($duration)) seconds"
            }
        }
    }
    if (-not $sessionStable -and -not ($firstLine -match "\[\d{2}:\d{2}:\d{2}")) {
        Test-Skip "Session duration" "Could not parse timestamps"
    }
}

# ============================================================
# SECTION 5: IL2CPP Lint
# ============================================================
Write-Host ""
Write-Host "[5/5] IL2CPP Lint (Banned APIs)" -ForegroundColor Yellow
Write-Host "---" -ForegroundColor DarkGray

$lintScript = Join-Path $projectRoot "Tools\Lint-IL2CPP.ps1"
if (Test-Path $lintScript) {
    $lintOutput = & pwsh -NoProfile -File $lintScript 2>&1 | Out-String
    if ($lintOutput -match "(\d+) error\(s\), (\d+) warning\(s\)") {
        $lintErrors = [int]$matches[1]
        $lintWarnings = [int]$matches[2]
        if ($lintErrors -eq 0 -and $lintWarnings -eq 0) {
            Test-Pass "IL2CPP lint clean" "No banned API usage"
        }
        elseif ($lintErrors -gt 0) {
            Test-Warn "IL2CPP lint violations" "$lintErrors error(s), $lintWarnings warning(s) - run 'make lint'"
        }
        else {
            Test-Pass "IL2CPP lint (warnings only)" "$lintWarnings warning(s)"
        }
    }
    elseif ($lintOutput -match "No banned API usage") {
        Test-Pass "IL2CPP lint clean" "No violations"
    }
    else {
        Test-Warn "IL2CPP lint output unclear" "Run 'make lint' manually"
    }
}
else {
    Test-Skip "IL2CPP lint" "Script not found"
}

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

if ($passRate -lt 80) {
    Write-Host "  BELOW 80% THRESHOLD - $passRate% pass rate" -ForegroundColor Red
    exit 1
}
elseif ($script:failCount -gt 0) {
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
