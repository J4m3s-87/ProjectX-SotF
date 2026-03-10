# Project X — Originality Report

---

## Overview

Project X is a unified modding suite for Sons of the Forest, combining the functionality of 20+ standalone mods into a single package with dedicated server support. It is **73 source files totalling over 770KB of custom C# code**, representing over 200 development phases.

This document provides a transparent account of what was built, what was studied, how each module relates to existing mods, and where the implementations diverge.

## Architecture

Project X is built on a fundamentally different architecture from standard Sons of the Forest mods:

| What            | Project X                                     | Typical SotF Mod                       |
| --------------- | --------------------------------------------- | -------------------------------------- |
| Architecture    | Static classes, manual `Init()`               | `SonsMod` subclass with lifecycle      |
| Harmony patches | Manual `harmony.Patch()` per method           | `[HarmonyPatchAll]` attribute scanning |
| Multi-tier      | `#if SERVER / OWNER / CLIENT` compilation     | Single build, no tier support          |
| Server support  | Headless dedicated server edition             | Client-only                            |
| Admin system    | Remote `/px` command bridge via chat protocol | None                                   |

These are not cosmetic differences — **code cannot be copy-pasted between these architectures**. A `[HarmonyPatch]` attribute doesn't work in a manual patching system. A `SonsMod.OnInGameUpdate()` callback doesn't exist in the static module pattern. Every line had to be written to work within this framework.

## Methodology

Every feature in Project X followed the same development process:

1. **Study** — Existing mods were studied to understand _which game APIs to target_ and what the desired player-facing behaviour should be
2. **Identify the API** — Determine which game types, methods, and fields need to be accessed (e.g. `ScrewStructure._hp` for durability, `ItemDatabaseManager.ItemById()` for stacks)
3. **Implement** — Write a new implementation within Project X's static module architecture, with manual Harmony patching, tier guards, and server compatibility
4. **Solve IL2CPP problems** — Many standard .NET approaches fail under IL2CPP (reflection is stripped, types are remapped, methods go missing). These required novel workarounds documented below

Every mod for Sons of the Forest targets the **same game APIs** — there is only one way to modify structure HP, one way to change stack sizes, one way to hook raid events. Two mods doing the same thing will inevitably reference the same types and fields, because those are the only types and fields the game exposes.

## Modules With No Original Equivalent

These modules exist only in Project X — there was nothing to study or reference:

| Module                                                                 | Size        | Files | What It Does                                                                                                                                         |
| ---------------------------------------------------------------------- | ----------- | ----- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Integrity / Anti-Cheat** (IntegrityEvent + IntegrityConfig)          | **40.6KB**  | 2     | Server-side cheat detection and validation                                                                                                           |
| **Permission System** (PermissionEvent + PermissionSync + RoleManager) | **15.5KB**  | 3     | Role-based access control (Owner / Admin / Player) synced via Bolt                                                                                   |
| **Config Sync** (ConfigSyncEvent + ConfigSyncPayload)                  | **14.9KB**  | 2     | Real-time server-to-client config broadcast — live economy control                                                                                   |
| **Loot Sync** (LootSyncEvent + LootEventListener)                      | **10.5KB**  | 2     | Server-authoritative loot tracking — dedicated NetEvent protocol (0x01–0x21) for client→server collection reports and server→client suppression sync |
| **Admin Command Bridge** (AdminCommandEvent + CommandBridge)           | **20.3KB**  | 2     | Remote `/px` commands via hybrid Bolt/ChatBox protocol                                                                                               |
| **Building Enhancements** (BuilderEnhancements)                        | **26.9KB**  | 1     | Custom building tools with correct API discovery and server-side support                                                                             |
| **Player Module** (PlayerModule + Actions)                             | **5.2KB**   | 1     | Player controls (Max Strength, vanilla speed capture for reset), companion management                                                                |
| **Custom GUI** (ProjectXGUI + Styles)                                  | **148.8KB** | 4     | Permission-aware IMGUI with 7 tabbed panels — not based on any existing mod                                                                          |
| **One-Click Installer**                                                | **16.5KB**  | 2     | Auto-detects game via Steam, downloads prerequisites, deploys mod                                                                                    |
| **In-Game Welcome System**                                             | —           | —     | Multi-line orientation chat for players joining the server                                                                                           |

**Total unique code with no original equivalent: ~288KB+**

## Modules With an Original Counterpart

Each module below was _studied_ from an existing mod to understand the concept and target API, then independently implemented within Project X's architecture.

| Module                  | Project X          | Original                                                                                                                                      | Size Difference  | Key Architectural Difference                                                                                                                                                                                                                                                                                                                                 |
| ----------------------- | ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **UI**                  | 148.8KB (4 files)  | 113.4KB (17 files)                                                                                                                            | +31%             | Studied AxelModMenu for concepts. Completely different IMGUI rendering approach, permission-aware panels                                                                                                                                                                                                                                                     |
| **RaidCustomizer**      | 99.6KB (11 files)  | 76KB (16 files)                                                                                                                               | +31%             | Most closely based on original. Retained core event logic, added 58KB of new code (RaidActions, RaidPatches, RaidConfig). Rewrote for manual Harmony and server support                                                                                                                                                                                      |
| **DedicatedSuperuser**  | 102.6KB (11 files) | [33KB (GitHub)](https://github.com/laserman120/DedicatedSuperuser) — Commands/, HarmonyPatches/, Manager/, Networking/, Utility/              | **3.1× larger**  | GLaDOS's version uses `SonsMod` lifecycle + `[HarmonyPatch]` attributes + SUI settings panel. Project X adds RBAC roles, permission-aware GUI (no debug console needed), remote `/px` command bridge via chat protocol, and config sync                                                                                                                      |
| **ScaryCross**          | 70.4KB (3 files)   | [16KB (GitHub)](https://github.com/laserman120/ScaryCross) — `MakeCrossScary` MonoBehaviour + `DemonDetector` + effigy stimuli                | **4.4× larger**  | GLaDOS's version uses `RegisterTypeInIl2Cpp` `MakeCrossScary` + `DemonDetector` + `ScaryObject.BurnDemon` + bonfire prefab fire cloning. Project X similarly uses `DemonDetector` but with `VailActor.IgniteSelf()` bypass and configurable target NPC types                                                                                                 |
| **BroadcastMessage**    | 66.2KB (4 files)   | 7KB                                                                                                                                           | **9.5× larger**  | Discord rich embeds with zero-dependency JSON + in-game welcome system                                                                                                                                                                                                                                                                                       |
| **WeaponDamage**        | 21.2KB (2 files)   | ~9KB                                                                                                                                          | **2.4× larger**  | Same concept, different Harmony approach, server sync                                                                                                                                                                                                                                                                                                        |
| **MeatDryer**           | 18.1KB (2 files)   | ~5KB                                                                                                                                          | **3.6× larger**  | Complete rewrite with seasonal drying system and fire proximity detection. Original was bugged                                                                                                                                                                                                                                                               |
| **WaterCollectors**     | 15.2KB (2 files)   | [~3KB (GitHub)](https://github.com/laserman120/Realistic-Water-Collectors) — `FireProximityTrigger` MonoBehaviour + `SphereCollider` triggers | **5.1× larger**  | GLaDOS's version uses `RegisterTypeInIl2Cpp` `FireProximityTrigger` with `SphereCollider` + `OnTriggerEnter/Exit` for fire detection. Project X uses Harmony `OnEnable` tracking + `Vector3.Distance` polling instead of collider triggers                                                                                                                   |
| **Stack**               | 14.9KB (1 file)    | ~4KB                                                                                                                                          | **3.7× larger**  | Per-item configs across 14 categories, reset-to-defaults, tier guards                                                                                                                                                                                                                                                                                        |
| **StructureDurability** | 11.7KB (1 file)    | ~4KB                                                                                                                                          | **2.9× larger**  | Base-health tracking, Harmony postfix on `GetStructureInfo()` — direct field access stripped by IL2CPP                                                                                                                                                                                                                                                       |
| **BuilderStacks**       | 10.8KB (1 file)    | 14.4KB (4 files)                                                                                                                              | condensed        | Studied ItemCarryAmount — rebuilt with per-material capacity (logs/planks/stones), independent buffers, vanilla visual stacking preserved, icon-based HUD, text entry config                                                                                                                                                                                 |
| **LootRespawn**         | 50KB+ (3 files)    | [Full C# (GitHub)](https://github.com/laserman120/SOTF-Mod-LootRespawnControl) — Harmony/, Networking/, `PickUp.cs`, `BreakableObject.cs`     | **5× larger**    | GLaDOS's version tracks pickups + breakables via `PickUp.Collect`/`BreakableObject.OnBreak`, has chunked networking with ACK flow control (`ConfigDataEvent`/`LootDataEvent`). Project X adds openable container tracking (`ContainerItemSpawner.OpenContainer`), integer hash replacing MD5, server-authoritative loot sync via dedicated NetEvent protocol |
| **AmmoUI**              | 10.1KB (1 file)    | 58.3KB (6 files)                                                                                                                              | **5.8× smaller** | Condensed 6-file mod into 1 file with sprite caching and frame throttling                                                                                                                                                                                                                                                                                    |
| **Hotbar**              | 8KB (2 files)      | ~3.7KB                                                                                                                                        | **2.2× larger**  | Sprite caching, frame throttling, IL2CPP reflection workaround for DummyDll texture casting                                                                                                                                                                                                                                                                  |
| **Relocator**           | 5.5KB (1 file)     | ~2KB                                                                                                                                          | **2.8× larger**  | Polling + category filter + backup dictionary                                                                                                                                                                                                                                                                                                                |
| **Zipline**             | 4.2KB (2 files)    | ~3KB                                                                                                                                          | +40%             | Polling pattern vs Harmony attributes for IL2CPP safety                                                                                                                                                                                                                                                                                                      |
| **CraftingSpeed**       | 4.2KB (1 file)     | ~2KB                                                                                                                                          | **2.1× larger**  | Static module + tier guards rewrite                                                                                                                                                                                                                                                                                                                          |
| **WaterfallSound**      | 4.5KB (1 file)     | ~1KB                                                                                                                                          | **4.5× larger**  | Complete FMOD rewrite: Harmony Postfix on `SonsFMODEventEmitter.Awake` replaces stripped `GetRootGameObjects` + `GetComponentsInChildren`. Volume via `SetVolume()` API                                                                                                                                                                                      |
| **Inventory**           | 1.8KB (1 file)     | 14.4KB (4 files)                                                                                                                              | condensed        | Studied ItemCarryAmount — carry amounts rebuilt as minimal static module                                                                                                                                                                                                                                                                                     |

**Every single module with a counterpart is architecturally different** — most are 2–9× the size. Code does not grow by copying.

## IL2CPP Challenges — Why Standard Approaches Fail

Many features required novel solutions because IL2CPP strips, remaps, or hides standard .NET APIs:

| Problem                            | Standard Approach                                                         | What Actually Happens                                                        | Project X Solution                                                                 |
| ---------------------------------- | ------------------------------------------------------------------------- | ---------------------------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| **Enemy HP/Damage**                | `AccessTools.Field("_health")`                                            | Returns `null` — field metadata stripped                                     | Unsafe pointer arithmetic at offsets 0x4CC, 0x4D0                                  |
| **Enemy Aggression**               | `GetStat(System.Type)`                                                    | `MissingMethodException` — IL2CPP maps `System.Type` to `Il2CppSystem.Type`  | Bridge through `dedicatedserver.cfg` native settings                               |
| **Structure Durability**           | Direct field access on `ScrewStructure._hp`                               | Field is stripped                                                            | Harmony postfix on `GetStructureInfo()`                                            |
| **Water Collector fire detection** | `SphereCollider` trigger (GLaDOS's approach) or `Physics.OverlapSphere()` | Both work at runtime; Project X avoids injecting MonoBehaviours on prefabs   | Harmony `OnEnable` tracking with `Vector3.Distance` — no collider injection needed |
| **Follower HP**                    | Set `_max` on HealthStat                                                  | VailActor has 13 stats, many sharing `_max=100`                              | Match by `_baseValue` not `_max`, 1-second post-revive delay                       |
| **Season control**                 | `SeasonsManager._activeSeason` field                                      | Field stripped, network sync overrides writes                                | Marshal.WriteInt32 at offset 0x70 + triple Harmony PREFIX blocker                  |
| **Texture casting**                | Standard type cast                                                        | DummyDll types don't cast to IL2CPP types                                    | `Il2CppInterop` runtime cast workaround                                            |
| **Scary Cross stimuli**            | Standard stimuli system                                                   | Stimuli don't fire from modded MonoBehaviours                                | Direct `VailActor.IgniteSelf()` bypass                                             |
| **Container respawn (breakable)**  | Reset save data via game API                                              | `OnBreak` fires before `Awake` during streaming; save data forces empty loot | Frame-based PREFIX blocking + `ClearStateSync` on child spawner                    |
| **Waterfall volume**               | `scene.GetRootGameObjects()` + `GetComponentsInChildren`                  | Both methods stripped by IL2CPP (scene graph + generic plural)               | Harmony Postfix on `SonsFMODEventEmitter.Awake` + root parent name filter          |

These are not theoretical problems — each one caused actual build failures or runtime crashes during development, often requiring multiple failed approaches before finding a working solution.

## LootRespawn — Deep Dive (46KB+ vs ~30KB Original)

LootRespawn is now the second-largest module and had the most original engineering work. It deserves more than a table row.

### What Was Kept From The Original

- **Core concept** — tracking collected loot by positional hash and suppressing respawn
- **MD5 hash algorithm** — `GenerateLootID()` using position + rotation + first 3 chars of name (retained for containers; replaced for pickups)
- **General hook targets** — Harmony hooks on `PickUp.Awake` and `PickUp.Collect`

### What Was Changed

| Aspect                  | Original (GlaDOS)                                 | Project X                                                                                                                                    |
| ----------------------- | ------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| **Hashing (pickups)**   | MD5 on every `PickUp.Awake`                       | Integer hash (`name.GetHashCode()` + quantized pos) — MD5 was a bottleneck with 600+ Awake calls                                             |
| **Harmony patching**    | `HarmonyPatchAll = true`                          | Manual `harmony.Patch()` per method — IL2CPP vtable safety                                                                                   |
| **Persistence**         | Game save system (`ICustomSaveable`)              | Standalone `loot_collected.dat` (binary) — survives save corruption                                                                          |
| **Config**              | 50+ entries (per-category allow/block/timed/sync) | 26+ entries (per-category toggles, per-category respawn days, whitelist/blacklist, global timer) — uses -1 sentinel for "use global" pattern |
| **Clone filtering**     | Not clearly handled                               | Explicit skip for `(Clone)` items — container-spawned pickups are ephemeral                                                                  |
| **Deferred processing** | `PickupsPendingCheck` list + double-check flag    | `_pendingItems` queue with `OnGameStarted` flush                                                                                             |

### What's Entirely New (Not In The Original)

Container respawn — openable, breakable, and GrabBag — is **100% original engineering** with no equivalent in the original mod:

| New Feature                     | Description                                                                                          |
| ------------------------------- | ---------------------------------------------------------------------------------------------------- |
| **Openable container respawn**  | PREFIX on `ContainerItemSpawner.OpenContainer` blocks streaming replays (`spawnItems=false`)         |
| **Breakable container respawn** | PREFIX on `BreakableObject.OnBreak` with frame-based blocking (≤120 frames) + expired timer fallback |
| **ClearContainerSaveState**     | Wipes child `ContainerItemSpawner` save data so breakable crates spawn fresh loot (`contentsSeed`)   |
| **Frame-based blocking**        | `_breakableLoadFrame` dictionary distinguishes streaming replays from player interactions            |
| **Event ordering fallback**     | Handles race where `OnBreak` fires before `Awake` POSTFIX on the same frame during streaming         |
| **`_recentlyRespawned` guard**  | Blocks streaming replays only; player interaction allowed after `_saveDataLoaded=true`               |
| **GrabBag/container tracking**  | Tracks suitcases and pelican cases via `containerId` as stable unique key                            |

This required **8 iterations** over a single session to solve — each fixing one layer of the problem (blocked all breaks → frame-based → event ordering race → empty loot → ClearStateSync).

### What The Original Has That We Don't

For transparency — features in the original that we intentionally did not include:

- ~~**Per-category granular control**~~ — **Now implemented** (Phase 329+): 12 category toggles + 12 per-category respawn day overrides (default -1 = use global `LootRespawnDays`). Each category can have its own timer — e.g., ammo respawns in 1 day, weapons in 7 days
- ~~**Multiplayer loot sync**~~ — **Now implemented** (March 09, 2026): Server-authoritative tracking via dedicated `LootSyncEvent` NetEvent protocol. Clients report collections (0x01 pickup, 0x02 break, 0x03 open); server broadcasts suppressions (0x20 full sync, 0x21 incremental). Container events fire server-side directly. Server maintains central tracker and persists to `loot_collected.dat`

Our multiplayer sync uses a dedicated `Packets.NetEvent` (scope `ProjectX.LootSync`) with a binary protocol — replacing the earlier `debugCommand` relay approach.

- ~~**Custom whitelist/blacklist**~~ — **Now implemented**: `LR_ItemWhitelist` / `LR_ItemBlacklist` config entries accept comma-separated item IDs. Blacklist overrides everything (never tracked), whitelist overrides category toggles (always tracked)
- **`LootIdentifier` MonoBehaviour** — `RegisterTypeInIl2Cpp` component injected on each pickup (we use a simpler instance-ID cache)
- **SUI settings panel** — dedicated in-game settings UI (we use native settings integration)

## Licensing

Project X is released under **AGPL-3.0**, consistent with the licenses of the mods that were studied.

The following mods with published source code were studied during development:

- **GLaD0S (laserman120)** — [ScaryCross](https://github.com/laserman120/ScaryCross) (AGPL-3.0), [DedicatedSuperuser](https://github.com/laserman120/DedicatedSuperuser) (AGPL-3.0), [Realistic Water Collectors](https://github.com/laserman120/Realistic-Water-Collectors) (GPL-3.0), [PrefabRepair](https://github.com/laserman120/PrefabRepair) (AGPL-3.0), [Loot Respawn Control](https://github.com/laserman120/SOTF-Mod-LootRespawnControl) (AGPL-3.0), [VailSpawnControl](https://github.com/laserman120/VailSpawnControl) (AGPL-3.0)

Dependencies:

- **RedLoader / SonsSdk / SonsAxLib** — LGPL-2.1, dynamically linked at runtime, unmodified
- **HarmonyLib** — MIT licensed
- **Il2CppInterop** — LGPL-3.0, dynamically linked, unmodified
- **Steamworks.NET** — MIT licensed
- **Game assemblies** — Referenced at runtime only, not redistributed

Full license details are maintained in [`LICENSES.md`](LICENSES.md).

## Acknowledgements

Project X wouldn't exist without the work of the modders who came before. Their mods showed what was possible, which game APIs to target, and what features the community wanted:

- **Toni Macaroni** — RedLoader, SonsSdk, SonsAxLib, WaterfallSoundControl, and Relocator. The entire modding framework is his work
- **GLaD0S (laserman120)** — ScaryCross, DedicatedSuperuser, Realistic Water Collectors, PrefabRepair, Loot Respawn Control, VailSpawnControl. Source code publicly available on GitHub
- **codengine** — RaidCustomizer. The most closely studied reference during development
- **TerroDucky** — StackMod. Per-item stack configuration concept
- **ImAxel** — AmmoUi and AxelModMenu. UI and ammo display concepts
- **SmokyAce** — ZiplineExtender, BroadcastMessage, and ItemCarryAmount
- **ThirtyTwelve** — BroadcastMessage co-author
- **badboy7** — Realistic Meat Dryer. Concept studied, rebuilt with seasonal system
- **AEDEV** — SonsHotbar. Hotbar implementation studied
- **sknthelisper** — Less Useless Guns. Weapon damage concept
- **Laughingcat** — Extended Rope Bridges
- **tempbito** — OpenSesame and StructureDurability
- **RegiToXic** — CoopServerTools. The inspiration for dedicated server support

Full attribution is maintained in [`CREDITS.md`](CREDITS.md).

---

_Project X — by J4m3s & Claude_
_73 source files • 770KB+ custom code • 200+ development phases_
_March 2026_
