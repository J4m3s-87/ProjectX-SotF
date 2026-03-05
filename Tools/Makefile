# ============================================================
# Project X - Multi-Edition Build System
# ============================================================
# Three editions: Owner, Server, Client
#
# Usage (from Project X root):
#   make -f Tools/Makefile          - Build + deploy Owner + Server + Client
#   make -f Tools/Makefile owner    - Build Owner (your MP client)
#   make -f Tools/Makefile server   - Build Server (headless)
#   make -f Tools/Makefile client   - Build Client (friends)
#   make -f Tools/Makefile help     - Show all targets
# ============================================================

SHELL := pwsh.exe
.SHELLFLAGS := -NoProfile -NoLogo -NonInteractive -Command

# Load local path overrides if present (gitignored)
-include Tools/Makefile.local

CONFIGURATION := Release
VERBOSITY := minimal

.PHONY: all owner server client clean restore help info deploy deploy-owner deploy-server deploy-client discord-pack lint test

# ============================================================
# Default Target
# ============================================================
all: deploy
	@Write-Host "[MAKE] All editions built and deployed!" -ForegroundColor Green

# ============================================================
# Build Targets
# ============================================================

## Build Owner edition (ProjectX.Owner.dll) - Your multiplayer client
owner: restore lint
	@Write-Host "[MAKE] Building Owner (ProjectX.Owner.dll)..." -ForegroundColor Cyan
	@dotnet build "Dev/ProjectX.Owner/ProjectX.Owner.csproj" -c $(CONFIGURATION) -v $(VERBOSITY)
	@Write-Host "[MAKE] Owner built" -ForegroundColor Green

## Build Server edition (ProjectX.Server.dll) - Headless dedicated server
server: restore lint
	@Write-Host "[MAKE] Building Server (ProjectX.Server.dll)..." -ForegroundColor Cyan
	@dotnet build "Dev/ProjectX.Server/ProjectX.Server.csproj" -c $(CONFIGURATION) -v $(VERBOSITY)
	@Write-Host "[MAKE] Server built" -ForegroundColor Green

## Build Client edition (ProjectX.Client.dll) - Friends joining server
client: restore lint
	@Write-Host "[MAKE] Building Client (ProjectX.Client.dll)..." -ForegroundColor Cyan
	@dotnet build "Dev/ProjectX.Client/ProjectX.Client.csproj" -c $(CONFIGURATION) -v $(VERBOSITY)
	@Write-Host "[MAKE] Client built" -ForegroundColor Green

## Restore NuGet packages for all projects
restore:
	@Write-Host "[MAKE] Restoring NuGet packages..." -ForegroundColor Cyan
	@dotnet restore "Dev/ProjectX.Owner/ProjectX.Owner.csproj" -v $(VERBOSITY)
	@dotnet restore "Dev/ProjectX.Server/ProjectX.Server.csproj" -v $(VERBOSITY) 2>$$null
	@dotnet restore "Dev/ProjectX.Client/ProjectX.Client.csproj" -v $(VERBOSITY) 2>$$null
	@Write-Host "[MAKE] Packages restored" -ForegroundColor Green

# ============================================================
# Deploy Paths — override these for your local setup:
#   make -f Tools/Makefile GAME_MODS="D:/Steam/..." deploy
# Or set them as environment variables.
# ============================================================
# Each edition is a SINGLE self-contained DLL (Master source files
# are compiled directly into each via <Compile Include>).
# ONLY ONE client edition must exist in the Mods folder at a time,
# otherwise duplicate types cause silent conflicts.

GAME_MODS ?= $(USERPROFILE)/SteamLibrary/steamapps/common/Sons Of The Forest/Mods
SERVER_DEPLOY ?= $(USERPROFILE)/Desktop/ProjectX-Server
CLIENT_DEPLOY ?= $(USERPROFILE)/Desktop/ProjectX-Client

# ============================================================
# Deploy Targets (build + deploy + remove conflicts)
# ============================================================

## Build Owner + Server + Client and deploy all (most common dev workflow)
deploy: deploy-owner deploy-server deploy-client
	@Write-Host "[MAKE] Owner + Server + Client deployed!" -ForegroundColor Green

## Build and deploy Owner edition to game Mods folder
## Structure: Mods/ProjectX.Owner.dll + Mods/ProjectX.Owner/{manifest.json, Assets/}
deploy-owner: owner
	@New-Item -ItemType Directory -Path "$(GAME_MODS)/ProjectX.Owner" -Force | Out-Null
	@Remove-Item "$(GAME_MODS)/ProjectX.Master.dll" -Force -ErrorAction SilentlyContinue; Remove-Item "$(GAME_MODS)/ProjectX.Client.dll" -Force -ErrorAction SilentlyContinue; $$true
	@Copy-Item "Dev/ProjectX.Owner/bin/Release/net6.0/ProjectX.Owner.dll" "$(GAME_MODS)/ProjectX.Owner.dll" -Force
	@Copy-Item "Dev/ProjectX.Owner/manifest.json" "$(GAME_MODS)/ProjectX.Owner/manifest.json" -Force
	@if (Test-Path "Dev/ProjectX.Owner/bin/Release/net6.0/Assets") { Remove-Item "$(GAME_MODS)/ProjectX.Owner/Assets" -Recurse -Force -ErrorAction SilentlyContinue; Copy-Item "Dev/ProjectX.Owner/bin/Release/net6.0/Assets" "$(GAME_MODS)/ProjectX.Owner/Assets" -Recurse -Force }
	@Get-Item "$(GAME_MODS)/ProjectX.Owner.dll" | ForEach-Object { Write-Host "[DEPLOY] Owner: $$($$_.Length) bytes @ $$($$_.LastWriteTime.ToString('HH:mm:ss'))" -ForegroundColor Green }

## Build and deploy Server edition
## Structure: for server/ProjectX.Server.dll + for server/ProjectX.Server/manifest.json
deploy-server: server
	@New-Item -ItemType Directory -Path "$(SERVER_DEPLOY)/ProjectX.Server" -Force | Out-Null
	@Copy-Item "Dev/ProjectX.Server/bin/Release/net6.0/ProjectX.Server.dll" "$(SERVER_DEPLOY)/ProjectX.Server.dll" -Force
	@Copy-Item "Dev/ProjectX.Server/manifest.json" "$(SERVER_DEPLOY)/ProjectX.Server/manifest.json" -Force
	@Get-Item "$(SERVER_DEPLOY)/ProjectX.Server.dll" | ForEach-Object { Write-Host "[DEPLOY] Server: $$($$_.Length) bytes @ $$($$_.LastWriteTime.ToString('HH:mm:ss'))" -ForegroundColor Green }

## Build and deploy Client edition to For Client folder (never to local game Mods)
## Structure: For Client/ProjectX.Client.dll + For Client/ProjectX.Client/{manifest.json, Assets/}
deploy-client: client
	@New-Item -ItemType Directory -Path "$(CLIENT_DEPLOY)/ProjectX.Client" -Force | Out-Null
	@Copy-Item "Dev/ProjectX.Client/bin/Release/net6.0/ProjectX.Client.dll" "$(CLIENT_DEPLOY)/ProjectX.Client.dll" -Force
	@Copy-Item "Dev/ProjectX.Client/manifest.json" "$(CLIENT_DEPLOY)/ProjectX.Client/manifest.json" -Force
	@if (Test-Path "Dev/ProjectX.Client/bin/Release/net6.0/Assets") { Remove-Item "$(CLIENT_DEPLOY)/ProjectX.Client/Assets" -Recurse -Force -ErrorAction SilentlyContinue; Copy-Item "Dev/ProjectX.Client/bin/Release/net6.0/Assets" "$(CLIENT_DEPLOY)/ProjectX.Client/Assets" -Recurse -Force }
	@Get-Item "$(CLIENT_DEPLOY)/ProjectX.Client.dll" | ForEach-Object { Write-Host "[DEPLOY] Client: $$($$_.Length) bytes @ $$($$_.LastWriteTime.ToString('HH:mm:ss'))" -ForegroundColor Green }

# ============================================================
# Testing
# ============================================================

## Run integration tests: build checks, log analysis, module health, crash detection, lint
test: restore
	@pwsh -NoProfile -File "Tools/Test-ProjectX.ps1"

# ============================================================
# IL2CPP Lint (Banned API Check)
# ============================================================

## Check source files for known IL2CPP-stripped APIs (advisory, non-blocking)
lint:
	@pwsh -NoProfile -File "Tools/Lint-IL2CPP.ps1"; $$true

# ============================================================
# Clean
# ============================================================
clean:
	@Write-Host "[MAKE] Cleaning build artifacts..." -ForegroundColor Yellow
	@Get-ChildItem "Dev/*/bin","Dev/*/obj" -Directory -ErrorAction SilentlyContinue | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
	@Write-Host "[MAKE] Clean complete" -ForegroundColor Green

# ============================================================
# Help
# ============================================================
help:
	@Write-Host "============================================" -ForegroundColor Cyan
	@Write-Host "  Project X Multi-Edition Build System" -ForegroundColor Cyan
	@Write-Host "============================================" -ForegroundColor Cyan
	@Write-Host ""
	@Write-Host "EDITIONS:" -ForegroundColor Yellow
	@Write-Host "  owner   - You on server, full menu + admin"
	@Write-Host "  server  - Headless dedicated server"
	@Write-Host "  client  - Friends joining, dynamic menu access"
	@Write-Host ""
	@Write-Host "BUILD TARGETS:" -ForegroundColor Yellow
	@Write-Host "  make owner    - Build Owner (ProjectX.Owner.dll)"
	@Write-Host "  make server   - Build Server (ProjectX.Server.dll)"
	@Write-Host "  make client   - Build Client (ProjectX.Client.dll)"
	@Write-Host ""
	@Write-Host "DEPLOY TARGETS (build + deploy + remove conflicts):" -ForegroundColor Yellow
	@Write-Host "  make deploy          - Build + deploy Owner + Server + Client (dev workflow)"
	@Write-Host "  make deploy-owner    - Build + deploy Owner to game Mods"
	@Write-Host "  make deploy-server   - Build + deploy Server to for-server folder"
	@Write-Host "  make deploy-client   - Build + deploy Client to For Client folder"
	@Write-Host ""
	@Write-Host "DISTRIBUTION:" -ForegroundColor Yellow
	@Write-Host "  make discord-pack  - Build Client + ZIP one-click installer for Discord"
	@Write-Host ""
	@Write-Host "TESTING:" -ForegroundColor Yellow
	@Write-Host "  make test     - Run integration tests (80% pass rate required)"
	@Write-Host "  make lint     - Check for IL2CPP-stripped API usage"
	@Write-Host ""
	@Write-Host "UTILITY:" -ForegroundColor Yellow
	@Write-Host "  make clean    - Remove build artifacts"
	@Write-Host "  make info     - Show output paths"
	@Write-Host ""
	@Write-Host "USAGE:" -ForegroundColor Yellow
	@Write-Host "  make -f Tools/Makefile deploy"
	@Write-Host "============================================" -ForegroundColor Cyan

info:
	@Write-Host "============================================" -ForegroundColor Cyan
	@Write-Host "  Output Locations" -ForegroundColor Cyan
	@Write-Host "============================================" -ForegroundColor Cyan
	@Write-Host "Owner:    $(GAME_MODS)/ProjectX.Owner.dll"
	@Write-Host "Server:   $(SERVER_DEPLOY)/ProjectX.Server.dll"
	@Write-Host "Client:   $(CLIENT_DEPLOY)/ProjectX.Client.dll"
	@Write-Host "Discord:  $(DISCORD_OUT)/ProjectX_Installer.zip"
	@Write-Host "============================================" -ForegroundColor Cyan

# ============================================================
# Discord Distribution (One-Click Installer ZIP)
# ============================================================
# Builds Client, refreshes Installer/payload/, and creates a
# ready-to-upload ZIP for Discord distribution.
#
# The Installer/ directory contains:
#   INSTALL.bat              <- Entry point for friends
#   payload/
#     Install_ProjectX.ps1   <- Auto-detect game, check prereqs, install
#     RedModManager.exe      <- Installs RedLoader if needed
#     ProjectX.Client.dll    <- The mod DLL (refreshed by this target)
#     ProjectX.Client/       <- manifest.json + Assets/
#     README.txt

DISCORD_OUT ?= $(USERPROFILE)/Desktop/ProjectX-Discord
DISCORD_ZIP := $(DISCORD_OUT)/ProjectX_Installer.zip
INSTALLER_DIR := Installer

## Build Client + refresh installer payload + ZIP for Discord
discord-pack: client
	@Write-Host "[DISCORD] Refreshing installer payload..." -ForegroundColor Magenta
	@Copy-Item "Dev/ProjectX.Client/bin/Release/net6.0/ProjectX.Client.dll" "$(INSTALLER_DIR)/payload/ProjectX.Client.dll" -Force
	@Write-Host "[DISCORD] Updated ProjectX.Client.dll" -ForegroundColor Green
	@New-Item -ItemType Directory -Path "$(INSTALLER_DIR)/payload/ProjectX.Client" -Force | Out-Null
	@Copy-Item "Dev/ProjectX.Client/manifest.json" "$(INSTALLER_DIR)/payload/ProjectX.Client/manifest.json" -Force
	@Write-Host "[DISCORD] Updated manifest.json" -ForegroundColor Green
	@if (Test-Path "Dev/ProjectX.Client/bin/Release/net6.0/Assets") { Remove-Item "$(INSTALLER_DIR)/payload/ProjectX.Client/Assets" -Recurse -Force -ErrorAction SilentlyContinue; Copy-Item "Dev/ProjectX.Client/bin/Release/net6.0/Assets" "$(INSTALLER_DIR)/payload/ProjectX.Client/Assets" -Recurse -Force; Write-Host "[DISCORD] Updated Assets" -ForegroundColor Green }
	@New-Item -ItemType Directory -Path "$(DISCORD_OUT)" -Force | Out-Null
	@Remove-Item "$(DISCORD_ZIP)" -Force -ErrorAction SilentlyContinue; $$true
	@Compress-Archive -Path "$(INSTALLER_DIR)/*" -DestinationPath "$(DISCORD_ZIP)" -Force
	@$$zip = Get-Item "$(DISCORD_ZIP)"; $$mb = [math]::Round($$zip.Length / 1MB, 1); Write-Host "[DISCORD] Created: $$($$zip.Name) ($$mb MB)" -ForegroundColor Green
	@Write-Host "[DISCORD] Ready to upload to Discord!" -ForegroundColor Cyan
