# SOTF All-In-One Installer v3.0
# Automatically installs RedLoader + Mod Pack

$ErrorActionPreference = 'Stop'

function Write-ColorHost($text, $color = "White") {
    Write-Host $text -ForegroundColor $color
}

function Write-Header($text) {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Cyan
    Write-Host "  $text" -ForegroundColor Cyan
    Write-Host "============================================" -ForegroundColor Cyan
    Write-Host ""
}

Write-Header "SOTF All-In-One Installer v3.0"
Write-ColorHost "Complete setup: RedLoader + Mod Pack" "Gray"

# Find SOTF installation
$possiblePaths = @(
    "G:\SteamLibrary\steamapps\common\Sons Of The Forest",
    "C:\Program Files (x86)\Steam\steamapps\common\Sons Of The Forest",
    "D:\SteamLibrary\steamapps\common\Sons Of The Forest",
    "E:\SteamLibrary\steamapps\common\Sons Of The Forest"
)

$sotfPath = $null
foreach ($path in $possiblePaths) {
    if (Test-Path $path) {
        $sotfPath = $path
        break
    }
}

if (!$sotfPath) {
    Write-ColorHost "Could not auto-detect SOTF installation." "Yellow"
    Write-Host "Please enter your Sons Of The Forest path:"
    $sotfPath = Read-Host "Path"
    
    if (!(Test-Path $sotfPath)) {
        Write-ColorHost "ERROR: Path not found. Exiting." "Red"
        pause
        exit 1
    }
}

Write-ColorHost "Found SOTF at: $sotfPath" "Green"
Write-Host ""

# Check for RedLoader
Write-Header "Step 1: RedLoader Check"

$redloaderDll = Join-Path $sotfPath "version.dll"
$redloaderInstalled = Test-Path $redloaderDll

if ($redloaderInstalled) {
    Write-ColorHost "RedLoader is already installed!" "Green"
    Write-ColorHost "  Location: $redloaderDll" "Gray"
}
else {
    Write-ColorHost "RedLoader not found - installation required" "Yellow"
    Write-Host ""
    
    Write-ColorHost "RedLoader is the mod loader that makes mods work." "Gray"
    Write-ColorHost "This installer will download and install it for you." "Gray"
    Write-Host ""
    
    $install = Read-Host "Install RedLoader now? [Y/n]"
    
    if ($install -eq "" -or $install -eq "Y" -or $install -eq "y") {
        Write-Header "Installing RedLoader"
        
        # RedLoader download info
        $redloaderVersion = "0.8.6"
        $downloadUrl = "https://github.com/ToniMacaroni/RedLoader/releases/download/$redloaderVersion/RedLoader.zip"
        $tempZip = Join-Path $env:TEMP "RedLoader.zip"
        $tempExtract = Join-Path $env:TEMP "RedLoader_Extract"
        
        try {
            Write-ColorHost "Downloading RedLoader v$redloaderVersion..." "Yellow"
            Write-ColorHost "  From: GitHub/ToniMacaroni/RedLoader" "Gray"
            
            # Download with progress
            $ProgressPreference = 'SilentlyContinue'
            Invoke-WebRequest -Uri $downloadUrl -OutFile $tempZip -UseBasicParsing
            $ProgressPreference = 'Continue'
            
            Write-ColorHost "Downloaded successfully!" "Green"
            Write-Host ""
            
            Write-ColorHost "Extracting..." "Yellow"
            if (Test-Path $tempExtract) { Remove-Item $tempExtract -Recurse -Force }
            Expand-Archive -Path $tempZip -DestinationPath $tempExtract -Force
            
            Write-ColorHost "Installing to game directory..." "Yellow"
            
            # Copy RedLoader files
            $redloaderFiles = Get-ChildItem $tempExtract -Recurse -File
            foreach ($file in $redloaderFiles) {
                $relativePath = $file.FullName.Substring($tempExtract.Length + 1)
                $destPath = Join-Path $sotfPath $relativePath
                $destDir = Split-Path $destPath
                
                if (!(Test-Path $destDir)) {
                    New-Item -ItemType Directory -Path $destDir -Force | Out-Null
                }
                
                Copy-Item $file.FullName -Destination $destPath -Force
            }
            
            # Cleanup
            Remove-Item $tempZip -Force -ErrorAction SilentlyContinue
            Remove-Item $tempExtract -Recurse -Force -ErrorAction SilentlyContinue
            
            Write-Host ""
            Write-ColorHost "RedLoader installed successfully!" "Green"
            Write-ColorHost "  version.dll created" "Gray"
            
        }
        catch {
            Write-ColorHost "ERROR: Failed to install RedLoader" "Red"
            Write-ColorHost "  $($_.Exception.Message)" "Red"
            Write-Host ""
            Write-ColorHost "Manual installation required:" "Yellow"
            Write-ColorHost "  1. Download from: https://github.com/ToniMacaroni/RedLoader/releases" "Gray"
            Write-ColorHost "  2. Extract to: $sotfPath" "Gray"
            Write-Host ""
            pause
            exit 1
        }
    }
    else {
        Write-ColorHost "RedLoader installation skipped." "Yellow"
        Write-ColorHost "Note: Mods will not work without RedLoader!" "Red"
        Write-Host ""
    }
}

Write-Host ""

# Now proceed with mod installation (using the v2 installer code)
Write-Header "Step 2: Mod Pack Installation"

$modsPath = Join-Path $sotfPath "_Redloader\Mods"

if (!(Test-Path $modsPath)) {
    New-Item -ItemType Directory -Path $modsPath | Out-Null
    Write-ColorHost "Created _Redloader\Mods folder" "Green"
}

# Load manifest
$manifestPath = Join-Path $PSScriptRoot "modpack_manifest.json"
if (!(Test-Path $manifestPath)) {
    Write-ColorHost "ERROR: modpack_manifest.json not found!" "Red"
    pause
    exit 1
}

$manifest = Get-Content $manifestPath | ConvertFrom-Json

# Scan existing mods
Write-ColorHost "Scanning existing mods..." "Yellow"
$existingMods = @()
if (Test-Path $modsPath) {
    $existingModFolders = Get-ChildItem $modsPath -Directory
    foreach ($folder in $existingModFolders) {
        $existingMods += $folder.Name
    }
}

Write-ColorHost "Found $($existingMods.Count) existing mod(s)" "Gray"
Write-Host ""

# Get server mods
$serverMods = @()
Get-ChildItem $PSScriptRoot -Directory | ForEach-Object { $serverMods += $_.Name }

if ($existingMods.Count -eq 0) {
    # Fresh install - no need for mode selection
    Write-ColorHost "Fresh installation - installing server mods..." "Green"
    Write-Host ""
    
    $installed = 0
    foreach ($modFolder in Get-ChildItem $PSScriptRoot -Directory) {
        $destPath = Join-Path $modsPath $modFolder.Name
        
        try {
            Copy-Item -Path $modFolder.FullName -Destination $destPath -Recurse -Force
            $installed++
            Write-ColorHost "  $($modFolder.Name)" "Green"
        }
        catch {
            Write-ColorHost "  $($modFolder.Name) - FAILED" "Red"
        }
    }
    
    $mode = "FRESH"
}
else {
    # Existing mods - run full v2 installer logic
    $conflicts = $existingMods | Where-Object { $_ -notin $serverMods }
    $updates = $existingMods | Where-Object { $_ -in $serverMods }
    $newMods = $serverMods | Where-Object { $_ -notin $existingMods }
    
    Write-Header "Mod Analysis"
    
    if ($newMods.Count -gt 0) {
        Write-ColorHost "NEW mods to install: $($newMods.Count)" "Green"
        $newMods | ForEach-Object { Write-Host "  + $_" -ForegroundColor Green }
        Write-Host ""
    }
    
    if ($updates.Count -gt 0) {
        Write-ColorHost "EXISTING mods to update: $($updates.Count)" "Yellow"
        $updates | ForEach-Object { Write-Host "  ~ $_" -ForegroundColor Yellow }
        Write-Host ""
    }
    
    if ($conflicts.Count -gt 0) {
        Write-ColorHost "CONFLICTING mods (not on server): $($conflicts.Count)" "Red"
        $conflicts | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
        Write-Host ""
    }
    
    # Mode selection
    Write-Header "Sync Mode Selection"
    
    Write-Host "How would you like to sync with the server?"
    Write-Host ""
    Write-Host "[1] MERGE - Add server mods, keep my existing mods" -ForegroundColor Yellow
    Write-Host "    (Fastest, but may cause conflicts)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "[2] REPLACE - Match server exactly (RECOMMENDED)" -ForegroundColor Green
    Write-Host "    (Backs up existing mods, ensures perfect match)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "[3] SMART - Auto-resolve conflicts" -ForegroundColor Cyan
    Write-Host "    (Keeps compatible mods, removes conflicts)" -ForegroundColor Gray
    Write-Host ""
    
    $validChoice = $false
    $modeNum = 2
    
    while (!$validChoice) {
        $choice = Read-Host "Choice [1/2/3] (default: 2)"
        
        if ($choice -eq "" -or $choice -eq "2") {
            $modeNum = 2
            $validChoice = $true
        }
        elseif ($choice -eq "1") {
            $modeNum = 1
            $validChoice = $true
        }
        elseif ($choice -eq "3") {
            $modeNum = 3
            $validChoice = $true
        }
        else {
            Write-ColorHost "Invalid choice. Please enter 1, 2, or 3." "Red"
        }
    }
    
    Write-Host ""
    
    # Execute based on mode (simplified versions from v2)
    switch ($modeNum) {
        1 {
            Write-Header "MERGE Mode"
            $installed = 0
            foreach ($modFolder in Get-ChildItem $PSScriptRoot -Directory) {
                $destPath = Join-Path $modsPath $modFolder.Name
                if (Test-Path $destPath) { Remove-Item $destPath -Recurse -Force }
                Copy-Item -Path $modFolder.FullName -Destination $destPath -Recurse -Force -ErrorAction SilentlyContinue
                $installed++
                Write-ColorHost "  $($modFolder.Name)" "Green"
            }
            $mode = "MERGE"
        }
        2 {
            Write-Header "REPLACE Mode"
            if ($existingMods.Count -gt 0) {
                $backupName = "Mods_Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
                $backupPath = Join-Path (Split-Path $modsPath) $backupName
                Copy-Item -Path $modsPath -Destination $backupPath -Recurse -Force
                Write-ColorHost "Backed up to: $backupName" "Green"
            }
            Get-ChildItem $modsPath -Directory | Remove-Item -Recurse -Force
            $installed = 0
            foreach ($modFolder in Get-ChildItem $PSScriptRoot -Directory) {
                $destPath = Join-Path $modsPath $modFolder.Name
                Copy-Item -Path $modFolder.FullName -Destination $destPath -Recurse -Force
                $installed++
                Write-ColorHost "  $($modFolder.Name)" "Green"
            }
            $mode = "REPLACE"
        }
        3 {
            Write-Header "SMART Mode"
            if ($existingMods.Count -gt 0) {
                $backupName = "Mods_Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
                $backupPath = Join-Path (Split-Path $modsPath) $backupName
                Copy-Item -Path $modsPath -Destination $backupPath -Recurse -Force
                Write-ColorHost "Backed up!" "Green"
            }
            foreach ($conflict in $conflicts) {
                Remove-Item (Join-Path $modsPath $conflict) -Recurse -Force -ErrorAction SilentlyContinue
            }
            $installed = 0
            foreach ($modFolder in Get-ChildItem $PSScriptRoot -Directory) {
                $destPath = Join-Path $modsPath $modFolder.Name
                if (Test-Path $destPath) { Remove-Item $destPath -Recurse -Force }
                Copy-Item -Path $modFolder.FullName -Destination $destPath -Recurse -Force
                $installed++
                Write-ColorHost "  $($modFolder.Name)" "Green"
            }
            $mode = "SMART"
        }
    }
}

Write-Host ""
Write-Header "Installation Complete!"

Write-ColorHost "Summary:" "White"
Write-ColorHost "  RedLoader: Installed" "Green"
Write-ColorHost "  Mods installed: $($serverMods.Count)" "Green"
Write-ColorHost "  Mode: $mode" "Gray"
Write-Host ""

Write-ColorHost "Next Steps:" "Yellow"
Write-Host "  1. Launch Sons Of The Forest" -ForegroundColor Gray
Write-Host "  2. Press F1 to open mod menu" -ForegroundColor Gray
Write-Host "  3. Verify all mods loaded" -ForegroundColor Gray
Write-Host "  4. Join the server and play!" -ForegroundColor Gray
Write-Host ""

pause
