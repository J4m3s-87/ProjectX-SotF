
@echo off
REM SOTF All-In-One Installer Launcher
REM Installs RedLoader + Mod Pack automatically

echo ============================================
echo   SOTF All-In-One Installer
echo   RedLoader + Mod Pack Setup
echo ============================================
echo.

REM Check if PowerShell exists
where powershell >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: PowerShell not found!
    echo.
    echo Please download PowerShell from: https://aka.ms/powershell
    echo.
    pause
    exit /b 1
)

REM Run the all-in-one installer
echo Launching complete setup...
echo This will install RedLoader (if needed) and all server mods
echo.
powershell.exe -ExecutionPolicy Bypass -File "%~dp0INSTALL_COMPLETE.ps1"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ============================================
    echo   Setup completed successfully!
    echo ============================================
    echo.
    echo RedLoader and all mods are installed!
    echo Launch Sons Of The Forest and press F1
) else (
    echo.
    echo ============================================
    echo   Setup failed - check errors above
    echo ============================================
)

echo.
pause
