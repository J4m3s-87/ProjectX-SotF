# Project X — Third-Party Licenses

Project X depends on the following open-source libraries and tools.
No modifications have been made to any of these libraries.

## Runtime Dependencies

### RedLoader & SonsSdk

- **Author:** Toni Macaroni & codengine
- **License:** LGPL-2.1 (GNU Lesser General Public License v2.1)
- **Source:** https://github.com/ToniMacaroni/RedLoader
- **Usage:** Mod loader framework and game SDK. Project X links against these
  libraries at runtime without modification.

### HarmonyLib

- **Author:** Andreas Pardeike (pardeike)
- **License:** MIT
- **Source:** https://github.com/pardeike/Harmony
- **Usage:** Runtime method patching for game hooks.

### Il2CppInterop

- **Author:** BepInEx Team
- **License:** LGPL-3.0 (GNU Lesser General Public License v3.0)
- **Source:** https://github.com/BepInEx/Il2CppInterop
- **Usage:** Interoperability layer between IL2CPP and .NET CoreCLR.

## Development Tools

### Il2CppDumper

- **Author:** Perfare
- **License:** MIT
- **Source:** https://github.com/Perfare/Il2CppDumper
- **Usage:** Game type and method discovery during development.

### ICSharpCode.Decompiler (ILSpy)

- **Author:** ICSharpCode Team
- **License:** MIT
- **Source:** https://github.com/icsharpcode/ILSpy
- **Usage:** .NET assembly decompilation for mod analysis.

## LGPL Compliance Notice

Project X uses RedLoader, SonsSdk, and Il2CppInterop under the terms of the
LGPL. These libraries are dynamically linked at runtime and have not been
modified. Users retain the right to replace these libraries with compatible
versions as permitted by the LGPL.

## MIT License Summary

The MIT License permits free use, modification, and distribution of the
licensed software, provided the original copyright notice and license text
are preserved. Full license texts are available at the source repositories
linked above.
