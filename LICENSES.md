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

### SonsAxLib

- **Author:** Toni Macaroni
- **License:** LGPL-2.1 (bundled with RedLoader)
- **Source:** https://github.com/ToniMacaroni/RedLoader
- **Usage:** Extended game utility library (SUI, helpers).

### HarmonyLib (0Harmony)

- **Author:** Andreas Pardeike (pardeike)
- **License:** MIT
- **Source:** https://github.com/pardeike/Harmony
- **Usage:** Runtime method patching for game hooks (PREFIX/POSTFIX patches).

### Il2CppInterop

- **Author:** BepInEx Team
- **License:** LGPL-3.0 (GNU Lesser General Public License v3.0)
- **Source:** https://github.com/BepInEx/Il2CppInterop
- **Usage:** Interoperability layer between IL2CPP and .NET CoreCLR.

### Photon Bolt (bolt, bolt.user, udpkit)

- **Author:** Exit Games / Photon Engine
- **License:** Proprietary (game-bundled, runtime-linked only)
- **Source:** Distributed with Sons of the Forest
- **Usage:** Multiplayer networking. Project X references these for server
  command routing and Bolt event interception. Not redistributed.

### Steamworks.NET

- **Author:** Riley Labrecque (rlabrecque)
- **License:** MIT
- **Source:** https://github.com/rlabrecque/Steamworks.NET
- **Usage:** Steam API bindings for multiplayer and authentication.

### Unity Engine Modules

- **Author:** Unity Technologies
- **License:** Unity Companion License
- **Source:** https://unity.com
- **Usage:** Core game engine (UnityEngine, CoreModule, UI, InputSystem,
  TextMeshPro). Referenced at compile time via Il2CppDumper DummyDlls.

### Game Assemblies (Sons of the Forest)

- **Author:** Endnight Games
- **License:** Proprietary (game-bundled, runtime-linked only)
- **Source:** Distributed with Sons of the Forest
- **Usage:** Project X references game assemblies (Sons, Sons.Ai.Vail,
  Sons.Item, Sons.Weapon, Sons.StatSystem, Sons.Construction, Sons.Gui,
  Sons.Debug, Sons.Multiplayer, Sons.Electricity, Sons.Atmosphere,
  Endnight, Endnight.Utilities) for type information and hooks.
  Not redistributed.

## Development Tools

### Il2CppDumper

- **Author:** Perfare
- **License:** MIT
- **Source:** https://github.com/Perfare/Il2CppDumper
- **Usage:** Game type and method discovery during development. Generates
  DummyDlls for compile-time type references.

### ICSharpCode.Decompiler (ILSpy)

- **Author:** ICSharpCode Team
- **License:** MIT
- **Source:** https://github.com/icsharpcode/ILSpy
- **Usage:** .NET assembly decompilation for mod analysis.

## NuGet Packages

### Il2CppInterop.Runtime (v1.4.5)

- **License:** LGPL-3.0
- **Source:** https://www.nuget.org/packages/Il2CppInterop.Runtime

### UnityEngine.Modules (v2021.3.29)

- **License:** Unity Companion License
- **Source:** https://www.nuget.org/packages/UnityEngine.Modules

## Studied Mods (Reference Material)

The following community mods were studied as reference material during
development to understand which game APIs to target. These mods are not
linked, bundled, or redistributed with Project X. All Project X modules
were independently implemented on a different architecture.

### GLaD0S (laserman120) — AGPL-3.0

- **ScaryCross** — https://github.com/laserman120/ScaryCross
- **DedicatedSuperuser** — https://github.com/laserman120/DedicatedSuperuser
- **LootRespawnControl** — https://github.com/laserman120/SOTF-Mod-LootRespawnControl
- **PrefabRepair** — https://github.com/laserman120/PrefabRepair
- **VailSpawnControl** — https://github.com/laserman120/VailSpawnControl

### GLaD0S (laserman120) — GPL-3.0

- **Realistic Water Collectors** — https://github.com/laserman120/Realistic-Water-Collectors

### Other Studied Mods (No License Found)

The following mods were downloaded as compiled DLLs from sotf-mods.com.
No license files were bundled with the downloads and no public source
repositories were identified at the time of development:

- RaidCustomizer (codengine), StackMod (Terroducky), AmmoUi (ImAxel0),
  AxelModMenu (ImAxel0), ZiplineExtender (SmokyAce), BroadcastMessage
  (SmokyAce & ThirtyTwelve), ItemCarryAmount (SmokyAce),
  WaterfallSoundControl (Toni Macaroni), Relocator (Toni Macaroni),
  RealisticMeatDryer (badboy7), SonsHotbar (AEDEV),
  LessUselessGuns (sknthelisper), OpenSesame (tempbito),
  StructureDurability (tempbito)

## LGPL Compliance Notice

Project X uses RedLoader, SonsSdk, SonsAxLib, and Il2CppInterop under the
terms of the LGPL. These libraries are dynamically linked at runtime and have
not been modified. Users retain the right to replace these libraries with
compatible versions as permitted by the LGPL.

## MIT License Summary

The MIT License permits free use, modification, and distribution of the
licensed software, provided the original copyright notice and license text
are preserved. Full license texts are available at the source repositories
linked above.

## Proprietary Notice

Sons of the Forest game assemblies and Photon Bolt networking libraries are
proprietary software owned by Endnight Games and Exit Games respectively.
Project X references these libraries at runtime only and does not redistribute
them. Users must own a legitimate copy of Sons of the Forest.
