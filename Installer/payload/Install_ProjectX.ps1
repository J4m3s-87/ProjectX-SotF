<#
.SYNOPSIS
    Project X Client Installer - Full auto-installer for Sons of the Forest modding.

.DESCRIPTION
    One-click installer that handles everything:
    1. Auto-detects your Sons of the Forest game directory via Steam registry + VDF parsing
    2. Downloads and installs .NET 6.0 Desktop Runtime if missing (silent)
    3. Downloads and installs VC++ 2015-2019 x64 Redistributable if missing (silent)
    4. Installs RedLoader 0.8.6 (mod loader) via RedModManager
    5. Deploys the Project X Client build (DLL, manifest, assets)

.NOTES
    Run this script by right-clicking -> "Run with PowerShell"
    or from a terminal: powershell -ExecutionPolicy Bypass -File Install_ProjectX.ps1
#>

# --- Configuration ---
$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$PayloadDir = $ScriptDir

# --- Helper Functions ---

function Write-Banner {
    Clear-Host
    Write-Host ""
    Write-Host "  =================================================" -ForegroundColor Red
    Write-Host "  =                                               =" -ForegroundColor Red
    Write-Host "  =       PROJECT X - Client Installer            =" -ForegroundColor Red
    Write-Host "  =       Sons of the Forest Mod Pack             =" -ForegroundColor Red
    Write-Host "  =                                               =" -ForegroundColor Red
    Write-Host "  =================================================" -ForegroundColor Red
    Write-Host ""
}

function Write-Step {
    param([string]$Message)
    Write-Host "  [>] " -ForegroundColor Cyan -NoNewline
    Write-Host $Message
}

function Write-Success {
    param([string]$Message)
    Write-Host "  [OK] " -ForegroundColor Green -NoNewline
    Write-Host $Message
}

function Write-Warn {
    param([string]$Message)
    Write-Host "  [!!] " -ForegroundColor Yellow -NoNewline
    Write-Host $Message
}

function Write-Fail {
    param([string]$Message)
    Write-Host "  [X] " -ForegroundColor Red -NoNewline
    Write-Host $Message
}

# --- Step 1: Find Sons of the Forest ---

function Find-GameDirectory {
    Write-Step "Searching for Sons of the Forest installation..."
    
    $steamPaths = @()
    
    # Check default Steam install locations
    $defaultSteamPaths = @(
        "C:\Program Files (x86)\Steam",
        "C:\Program Files\Steam",
        "D:\Steam",
        "D:\SteamLibrary",
        "E:\Steam",
        "E:\SteamLibrary",
        "G:\Steam",
        "G:\SteamLibrary"
    )
    
    # Check registry for Steam install path
    try {
        $regPath = Get-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Valve\Steam" -Name "InstallPath" -ErrorAction SilentlyContinue
        if ($regPath) {
            $steamPaths += $regPath.InstallPath
        }
    }
    catch { }
    
    try {
        $regPath = Get-ItemProperty -Path "HKCU:\Software\Valve\Steam" -Name "SteamPath" -ErrorAction SilentlyContinue
        if ($regPath) {
            $steamPaths += $regPath.SteamPath -replace '/', '\'
        }
    }
    catch { }
    
    $steamPaths += $defaultSteamPaths
    
    # Parse libraryfolders.vdf for additional library paths
    foreach ($steamPath in $steamPaths) {
        $vdfFile = Join-Path $steamPath "steamapps\libraryfolders.vdf"
        if (Test-Path $vdfFile) {
            $content = Get-Content $vdfFile -Raw
            $vdfMatches = [regex]::Matches($content, '"path"\s+"([^"]+)"')
            foreach ($m in $vdfMatches) {
                $libPath = $m.Groups[1].Value -replace '\\\\', '\'
                $steamPaths += $libPath
            }
        }
    }
    
    # Search for the game in all library paths
    $gameSubPath = "steamapps\common\Sons Of The Forest"
    $gamePaths = @()
    
    foreach ($path in ($steamPaths | Select-Object -Unique)) {
        $candidate = Join-Path $path $gameSubPath
        if (Test-Path (Join-Path $candidate "SonsOfTheForest.exe")) {
            $gamePaths += $candidate
        }
    }
    
    if ($gamePaths.Count -eq 0) {
        Write-Warn "Could not auto-detect game directory."
        Write-Host ""
        Write-Host "  Please enter the full path to your Sons of the Forest folder" -ForegroundColor Yellow
        Write-Host "  (the folder containing SonsOfTheForest.exe):" -ForegroundColor Yellow
        Write-Host ""
        $manual = Read-Host "  Path"
        $manual = $manual.Trim('"', ' ')
        
        if (Test-Path (Join-Path $manual "SonsOfTheForest.exe")) {
            return $manual
        }
        else {
            Write-Fail "SonsOfTheForest.exe not found at: $manual"
            Write-Fail "Installation cancelled."
            return $null
        }
    }
    
    if ($gamePaths.Count -eq 1) {
        Write-Success "Found game at: $($gamePaths[0])"
        return $gamePaths[0]
    }
    
    # Multiple installations found
    Write-Warn "Multiple installations found:"
    for ($i = 0; $i -lt $gamePaths.Count; $i++) {
        Write-Host "    [$($i+1)] $($gamePaths[$i])" -ForegroundColor White
    }
    $choice = Read-Host "  Select installation (1-$($gamePaths.Count))"
    $idx = [int]$choice - 1
    if ($idx -ge 0 -and $idx -lt $gamePaths.Count) {
        return $gamePaths[$idx]
    }
    
    Write-Fail "Invalid selection."
    return $null
}

# --- Step 2: Check Prerequisites ---

function Test-DotNet6 {
    Write-Step "Checking for .NET 6.0 Desktop Runtime..."
    
    try {
        $output = & dotnet --list-runtimes 2>$null
        if ($output -match "Microsoft\.WindowsDesktop\.App 6\.") {
            Write-Success ".NET 6.0 Desktop Runtime is installed."
            return $true
        }
    }
    catch { }
    
    # Fallback: check if runtime folder exists
    try {
        $runtimePath = "C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App"
        if (Test-Path $runtimePath) {
            $versions = Get-ChildItem $runtimePath -Directory | Where-Object { $_.Name -match "^6\." }
            if ($versions) {
                Write-Success ".NET 6.0 Desktop Runtime found: $($versions[-1].Name)"
                return $true
            }
        }
    }
    catch { }
    
    Write-Warn ".NET 6.0 Desktop Runtime not detected."
    return $false
}

function Test-VCRedist {
    Write-Step "Checking for Visual C++ 2015-2019 Redistributable (x64)..."
    
    $installed = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64" -ErrorAction SilentlyContinue
    if ($installed -and $installed.Installed -eq 1) {
        Write-Success "VC++ Redistributable is installed."
        return $true
    }
    
    # Also check via uninstall registry
    $uninstall = Get-ItemProperty "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*" -ErrorAction SilentlyContinue |
    Where-Object { $_.DisplayName -match "Microsoft Visual C\+\+ 201[5-9].*x64" -or $_.DisplayName -match "Microsoft Visual C\+\+ 202[0-9].*x64" }
    
    if ($uninstall) {
        Write-Success "VC++ Redistributable is installed."
        return $true
    }
    
    Write-Warn "VC++ 2015-2019 Redistributable (x64) not detected."
    return $false
}

function Install-Prerequisites {
    param([bool]$NeedDotNet, [bool]$NeedVC)
    
    if (-not $NeedDotNet -and -not $NeedVC) { return $true }
    
    Write-Host ""
    Write-Host "  --- Installing Missing Prerequisites ---" -ForegroundColor Cyan
    Write-Host ""
    
    # Create temp directory for downloads
    $tempDir = Join-Path $env:TEMP "ProjectX_Install"
    if (-not (Test-Path $tempDir)) {
        New-Item -Path $tempDir -ItemType Directory -Force | Out-Null
    }
    
    $allOk = $true
    
    # --- VC++ 2015-2019 Redistributable ---
    if ($NeedVC) {
        $vcUrl = "https://aka.ms/vs/16/release/vc_redist.x64.exe"
        $vcPath = Join-Path $tempDir "vc_redist.x64.exe"
        
        Write-Step "Downloading Visual C++ 2015-2019 Redistributable (x64)..."
        try {
            Invoke-WebRequest -Uri $vcUrl -OutFile $vcPath -UseBasicParsing
            Write-Success "Downloaded VC++ Redistributable"
            
            Write-Step "Installing VC++ Redistributable (this may take a moment)..."
            $proc = Start-Process -FilePath $vcPath -ArgumentList "/quiet /norestart" -Wait -PassThru
            if ($proc.ExitCode -eq 0 -or $proc.ExitCode -eq 3010) {
                Write-Success "VC++ Redistributable installed successfully."
            }
            else {
                Write-Warn "VC++ installer exited with code: $($proc.ExitCode)"
                Write-Warn "You may need to install it manually: $vcUrl"
                $allOk = $false
            }
        }
        catch {
            Write-Fail "Failed to download VC++ Redistributable: $_"
            Write-Warn "Opening download page in browser instead..."
            Start-Process "https://aka.ms/vs/16/release/vc_redist.x64.exe"
            Write-Host "  Please install VC++ manually, then run this installer again." -ForegroundColor Yellow
            $allOk = $false
        }
    }
    
    # --- .NET 6.0 Desktop Runtime ---
    if ($NeedDotNet) {
        $dotnetUrl = "https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe"
        $dotnetPath = Join-Path $tempDir "windowsdesktop-runtime-6.0-win-x64.exe"
        
        Write-Step "Downloading .NET 6.0 Desktop Runtime..."
        try {
            Invoke-WebRequest -Uri $dotnetUrl -OutFile $dotnetPath -UseBasicParsing
            Write-Success "Downloaded .NET 6.0 Desktop Runtime"
            
            Write-Step "Installing .NET 6.0 Desktop Runtime (this may take a moment)..."
            $proc = Start-Process -FilePath $dotnetPath -ArgumentList "/quiet /norestart" -Wait -PassThru
            if ($proc.ExitCode -eq 0 -or $proc.ExitCode -eq 3010) {
                Write-Success ".NET 6.0 Desktop Runtime installed successfully."
            }
            else {
                Write-Warn ".NET installer exited with code: $($proc.ExitCode)"
                Write-Warn "You may need to install it manually."
                $allOk = $false
            }
        }
        catch {
            Write-Fail "Failed to download .NET 6.0 Desktop Runtime: $_"
            Write-Warn "Opening download page in browser instead..."
            Start-Process "https://dotnet.microsoft.com/en-us/download/dotnet/6.0"
            Write-Host "  Please install .NET 6.0 Desktop Runtime manually, then run this installer again." -ForegroundColor Yellow
            $allOk = $false
        }
    }
    
    # Cleanup temp files
    try { Remove-Item $tempDir -Recurse -Force -ErrorAction SilentlyContinue } catch { }
    
    if (-not $allOk) {
        Write-Host ""
        Write-Warn "Some prerequisites could not be installed automatically."
        Write-Host "  Press any key to exit..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
        return $false
    }
    
    return $true
}

# --- Step 3: Install RedLoader ---

function Install-RedLoader {
    param([string]$GameDir)
    
    Write-Step "Installing RedLoader (mod loader)..."
    
    # Check if RedLoader is already installed
    $versionDll = Join-Path $GameDir "version.dll"
    $redloaderDir = Join-Path $GameDir "_Redloader"
    
    if ((Test-Path $versionDll) -and (Test-Path $redloaderDir)) {
        Write-Success "RedLoader is already installed - skipping."
        return $true
    }
    
    # Launch RedModManager
    $redManager = Join-Path $PayloadDir "RedModManager.exe"
    
    if (-not (Test-Path $redManager)) {
        Write-Fail "RedModManager.exe not found in payload directory!"
        return $false
    }
    
    Write-Host ""
    Write-Host "  --- RedLoader Installation ---" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  The Red Mod Manager will now open." -ForegroundColor White
    Write-Host "  Click " -NoNewline
    Write-Host "Install" -ForegroundColor Green -NoNewline
    Write-Host " to set up RedLoader, then close the manager."
    Write-Host ""
    Write-Host "  Press any key to launch Red Mod Manager..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    
    try {
        $proc = Start-Process -FilePath $redManager -PassThru
        Write-Step "Waiting for Red Mod Manager to close..."
        $proc.WaitForExit()
        
        # Verify it worked
        if ((Test-Path $versionDll) -or (Test-Path $redloaderDir)) {
            Write-Success "RedLoader installed successfully."
            return $true
        }
        else {
            Write-Warn "Could not verify RedLoader installation."
            Write-Warn "If you already installed it, press Y to continue."
            $response = Read-Host "  Continue anyway? (y/n)"
            if ($response -eq 'y' -or $response -eq 'Y') { return $true }
            return $false
        }
    }
    catch {
        Write-Fail "Failed to launch Red Mod Manager: $_"
        return $false
    }
}

# --- Step 4: Install Project X Client ---

function Install-ProjectXClient {
    param([string]$GameDir)
    
    Write-Step "Installing Project X Client mod..."
    
    $modsDir = Join-Path $GameDir "Mods"
    $modSubDir = Join-Path $modsDir "ProjectX.Client"
    
    # Create Mods directory if needed
    if (-not (Test-Path $modsDir)) {
        New-Item -Path $modsDir -ItemType Directory -Force | Out-Null
    }
    
    # Create mod subdirectory
    if (-not (Test-Path $modSubDir)) {
        New-Item -Path $modSubDir -ItemType Directory -Force | Out-Null
    }
    
    # Copy the DLL to Mods root
    $dllSource = Join-Path $PayloadDir "ProjectX.Client.dll"
    if (-not (Test-Path $dllSource)) {
        Write-Fail "ProjectX.Client.dll not found in payload!"
        return $false
    }
    Copy-Item $dllSource -Destination $modsDir -Force
    Write-Success "Copied ProjectX.Client.dll"
    
    # Copy manifest.json
    $manifestSource = Join-Path $PayloadDir "ProjectX.Client\manifest.json"
    if (Test-Path $manifestSource) {
        Copy-Item $manifestSource -Destination $modSubDir -Force
        Write-Success "Copied manifest.json"
    }
    else {
        Write-Warn "manifest.json not found in payload - skipping"
    }
    
    # Copy Assets folder
    $assetsSource = Join-Path $PayloadDir "ProjectX.Client\Assets"
    if (Test-Path $assetsSource) {
        $assetsDest = Join-Path $modSubDir "Assets"
        if (-not (Test-Path $assetsDest)) {
            New-Item -Path $assetsDest -ItemType Directory -Force | Out-Null
        }
        Copy-Item "$assetsSource\*" -Destination $assetsDest -Recurse -Force
        Write-Success "Copied Assets (hotbar, StoneGate)"
    }
    else {
        Write-Warn "Assets folder not found in payload - skipping"
    }
    
    return $true
}

# --- Step 5: Verify Installation ---

function Test-Installation {
    param([string]$GameDir)
    
    Write-Host ""
    Write-Step "Verifying installation..."
    
    $checks = @(
        @{ Path = (Join-Path $GameDir "version.dll"); Name = "RedLoader bootstrapper (version.dll)" },
        @{ Path = (Join-Path $GameDir "_Redloader"); Name = "RedLoader directory (_Redloader/)" },
        @{ Path = (Join-Path $GameDir "Mods\ProjectX.Client.dll"); Name = "Project X Client DLL" },
        @{ Path = (Join-Path $GameDir "Mods\ProjectX.Client\manifest.json"); Name = "Mod manifest" }
    )
    
    $allGood = $true
    foreach ($check in $checks) {
        if (Test-Path $check.Path) {
            Write-Success "$($check.Name)"
        }
        else {
            Write-Fail "$($check.Name) - MISSING"
            $allGood = $false
        }
    }
    
    return $allGood
}

# --- Main ---

Write-Banner

# Verify payload exists
if (-not (Test-Path (Join-Path $PayloadDir "ProjectX.Client.dll"))) {
    Write-Fail "ProjectX.Client.dll not found in: $PayloadDir"
    Write-Fail "Make sure the mod files are in the same directory as this script."
    Write-Host ""
    Write-Host "  Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# Step 1: Find game
$gameDir = Find-GameDirectory
if (-not $gameDir) {
    Write-Host ""
    Write-Host "  Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

Write-Host ""

# Step 2: Check prerequisites
$hasDotNet = Test-DotNet6
$hasVC = Test-VCRedist

if (-not $hasDotNet -or -not $hasVC) {
    $canContinue = Install-Prerequisites -NeedDotNet (-not $hasDotNet) -NeedVC (-not $hasVC)
    if (-not $canContinue) {
        Write-Host ""
        Write-Host "  Press any key to exit..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
        exit 1
    }
}

Write-Host ""

# Step 3: Install RedLoader
$redloaderOk = Install-RedLoader -GameDir $gameDir
if (-not $redloaderOk) {
    Write-Host ""
    Write-Host "  Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# Step 4: Install Project X Client
$clientOk = Install-ProjectXClient -GameDir $gameDir
if (-not $clientOk) {
    Write-Host ""
    Write-Host "  Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# Step 5: Verify
$verified = Test-Installation -GameDir $gameDir

Write-Host ""
if ($verified) {
    Write-Host "  =================================================" -ForegroundColor Green
    Write-Host "  =                                               =" -ForegroundColor Green
    Write-Host "  =       Installation Complete!                  =" -ForegroundColor Green
    Write-Host "  =       Launch Sons of the Forest and enjoy!    =" -ForegroundColor Green
    Write-Host "  =                                               =" -ForegroundColor Green
    Write-Host "  =================================================" -ForegroundColor Green
}
else {
    Write-Host "  =================================================" -ForegroundColor Yellow
    Write-Host "  =                                               =" -ForegroundColor Yellow
    Write-Host "  =       Installation completed with warnings.   =" -ForegroundColor Yellow
    Write-Host "  =       Some files may be missing - check above.=" -ForegroundColor Yellow
    Write-Host "  =                                               =" -ForegroundColor Yellow
    Write-Host "  =================================================" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "  Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
