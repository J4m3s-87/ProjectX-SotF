╔══════════════════════════════════════════════════╗
║         PROJECT X — Client Installer             ║
║         Sons of the Forest Mod Pack              ║
╚══════════════════════════════════════════════════╝

QUICK INSTALL
─────────────
1. Right-click "Install_ProjectX.ps1" → "Run with PowerShell"
2. Follow the on-screen prompts
3. Launch Sons of the Forest and join the server!

If "Run with PowerShell" is not available, open PowerShell and run:
   powershell -ExecutionPolicy Bypass -File Install_ProjectX.ps1


WHAT THIS INSTALLS
──────────────────
• RedLoader 0.8.6 — the mod loader for Sons of the Forest
• Project X Client — our custom mod pack with enhanced features

The installer will auto-detect your game directory via Steam.


PREREQUISITES
─────────────
The installer will check for these and guide you if they're missing:

• .NET 6.0 Desktop Runtime
  https://dotnet.microsoft.com/en-us/download/dotnet/6.0

• Visual C++ 2015-2019 Redistributable (x64)
  https://aka.ms/vs/16/release/vc_redist.x64.exe


MANUAL INSTALL (if the script doesn't work)
───────────────────────────────────────────
1. Extract "payload/RedLoader.zip" into your game folder
   (the _Redloader folder should be next to SonsOfTheForest.exe)

2. Copy "payload/ProjectX.Client.dll" into your game's "Mods/" folder

3. Copy the "payload/ProjectX.Client/" folder into your game's "Mods/" folder

4. Your Mods folder should look like:
   Mods/
   ├── ProjectX.Client.dll
   └── ProjectX.Client/
       ├── manifest.json
       └── Assets/
           ├── hotbar_assets
           └── StoneGate/


TROUBLESHOOTING
───────────────
Q: PowerShell says "running scripts is disabled"
A: Run this in PowerShell first:
   Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope CurrentUser

Q: The installer can't find my game
A: You'll be prompted to enter the path manually. Find the folder
   containing SonsOfTheForest.exe (usually in Steam/steamapps/common/).

Q: Game crashes on launch after install
A: Make sure .NET 6.0 Desktop Runtime (not just the regular runtime)
   and VC++ 2015-2019 x64 are both installed.
