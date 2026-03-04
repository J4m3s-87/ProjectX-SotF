<p align="center">
  <h1 align="center">Project X</h1>
  <p align="center"><strong>A unified modding suite for Sons of the Forest</strong></p>
  <p align="center">
    One DLL • 20+ features • Dedicated server support • Multiplayer-ready
  </p>
</p>

---

## What Is Project X?

Project X combines the functionality of 20+ standalone mods into a single, unified package built from the ground up. Rather than juggling 15–20 separate mods with independent configs and update cycles, everything is managed from one assembly with one config system.

The key difference is **dedicated server support**. Most existing mods are built on the `SonsMod` base class, which depends on a visible game window with a player camera and `OnGUI()` callbacks. A headless dedicated server (`SonsOfTheForestDS.exe`) has no renderer and no UI — so mods that rely on `SonsMod` lifecycle callbacks, IMGUI rendering, or camera-dependent logic simply won't load. Simpler mods (like Hotbar or AmmoUi) that don't depend on these systems may work fine on servers, but the more complex feature mods — raid customisation, loot respawn, structure durability, admin tools — typically don't.

Project X was designed from the start to work across all environments: solo, co-op host, dedicated server, and client.

From a single shared codebase, three editions are compiled:

| Edition    | DLL                   | For                                                                                                                    |
| ---------- | --------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| **Owner**  | `ProjectX.Owner.dll`  | Server admin's game client — full dashboard + remote commands                                                          |
| **Server** | `ProjectX.Server.dll` | Headless dedicated server (`SonsOfTheForestDS.exe`)                                                                    |
| **Client** | `ProjectX.Client.dll` | Players joining the server — receives server-pushed config (economy, raids, durability, etc.), UI access gated by role |

## Features

<details>
<summary><strong>🎮 Player Cheats & Quality of Life</strong></summary>

- God Mode, Infinite Stamina, No Hunger/Thirst/Fatigue
- NoClip / Fly Mode with WASD + Space/Ctrl
- No Fall Damage, Season Control
- Unstuck Kelvin / Virginia (teleport companions to you)
</details>

<details>
<summary><strong>🏗️ Building & Construction</strong></summary>

- Free Form Placement — remove snap restrictions
- Instant Build — skip construction animations
- Structure Relocator — pick up and reposition buildings
- Structure Durability Multiplier (slider)
- Log Hack / Stone Hack — infinite building materials
</details>

<details>
<summary><strong>⚔️ Combat & Enemies</strong></summary>

- Weapon Damage Multiplier
- Per-type Enemy HP / Damage / Aggression multipliers (Cannibals, Creepies, Bosses)
- Kelvin & Virginia HP Persistence — survives first-aid revival
- Kill All / Burn All / Freeze AI
</details>

<details>
<summary><strong>🎒 Inventory & Items</strong></summary>

- 75+ individually configurable stack sizes across 14 categories
- Infinite Items toggle
- Builder Stacks — separate carry amounts for building materials
- Ammo UI — on-screen ammunition counter
</details>

<details>
<summary><strong>🌍 World & Environment</strong></summary>

- **Raid Customiser** — scheduling, spawn control, enemy limits, boss configuration
- Loot Respawn — configurable respawn timer
- Extended Ziplines & Rope Bridges (up to 15,000 units)
- Crafting Speed multiplier
- Water Collector heat radius
- Waterfall volume control
</details>

<details>
<summary><strong>🔥 Unique Features</strong></summary>

- **Scary Cross** — electrified cross structure with demon detection, fire effects, and progressive damage
- **Meat Dryer** — seasonal drying speeds with fire proximity detection
- **Discord Bridge** — player join/leave/death/raid alerts with rich embeds
- **Admin Bridge** — 40+ remote commands via in-game chat (`/px` prefix)
- **RBAC Permissions** — Owner/Admin roles via SteamID, menu adapts in real-time
- **Config Sync** — settings broadcast to all clients via Bolt, no restarts needed
</details>

## Quick Install (Client)

1. Download the `Installer/` folder
2. Double-click **`INSTALL.bat`**
3. Follow the prompts — the installer auto-detects your game via Steam
4. Launch Sons of the Forest and join the server

> **Requires:** [RedLoader 0.8.6](https://sotf-mods.com/redloader), .NET 6.0 Desktop Runtime, VC++ 2015-2019 x64

See [`Installer/payload/README.txt`](Installer/payload/README.txt) for manual install instructions and troubleshooting.

## Project Structure

```
Dev/
├── ProjectX.Master/       # Shared codebase (all modules)
│   ├── Modules/           # Feature modules (one folder each)
│   │   ├── RaidCustomizer/
│   │   ├── DedicatedSuperuser/
│   │   ├── ScaryCross/
│   │   ├── StoneGate/
│   │   ├── UI/
│   │   └── ...
│   └── Assets/            # Binary assets (icons, models)
├── ProjectX.Client/       # Client build config
├── ProjectX.Owner/        # Owner build config
└── ProjectX.Server/       # Server build config
Installer/                 # One-click client installer
```

## How Project X Differs from the Originals

Every feature in Project X was independently implemented. The original mods were studied to understand _which game APIs to target_ and what player-facing behaviour to achieve — but the implementations had to be completely rewritten because the architectures are incompatible.

The originals use `SonsMod` with `[HarmonyPatchAll]` attribute scanning. Project X uses static classes with manual `harmony.Patch()` per method. You cannot copy code between these patterns — a `[HarmonyPatch]` attribute doesn't work in a manual patching system, and a `SonsMod.OnInGameUpdate()` callback doesn't exist in static modules.

### Why Everything Had to Be Rebuilt

Most original mods are built on the `SonsMod` base class, which depends on Unity's renderer and lifecycle callbacks (`OnInGameUpdate()`, `OnGameStart()`). On a headless dedicated server, these callbacks never fire — the mod simply doesn't load. This isn't a minor incompatibility; it means:

- **No `SonsMod` lifecycle** — We built our own tick drivers and event hooks using manual Harmony patches
- **No GUI on servers** — We built a remote command system (`/px` commands via chat protocol) so admins can control everything from their game client
- **Server-authoritative stats** — Enemy HP/damage must be modified on the server using unsafe pointer arithmetic because IL2CPP strips the normal reflection APIs
- **Permissions** — On a public server you can't give everyone admin access, so we built role-based access control (Owner/Admin/Player) synced via Bolt

### Module-by-Module Comparison

Modules with an original counterpart — studied the concept, rebuilt from scratch:

| Module                 | Project X           | Original                                                                        | Key Architectural Difference                                                                                                                                                                                 |
| ---------------------- | ------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **RaidCustomizer**     | 99.6KB, 11 files    | 76KB, 16 files                                                                  | Server-authoritative scheduling, unsafe pointer writes at IL2CPP offsets (0x4CC/0x4D0) because reflection is stripped. 4 failed approaches before finding working solution. Multiplayer player-count scaling |
| **DedicatedSuperuser** | 102.6KB, 11 files   | 33KB                                                                            | 3.1× larger. Remote `/px` command bridge, RBAC permissions, chat protocol routing                                                                                                                            |
| **ScaryCross**         | 70.4KB, 3 files     | 16KB                                                                            | 4.4× larger. Custom `DemonDetector` MonoBehaviour, progressive heat/fire state machine, direct `VailActor.IgniteSelf()` bypass (stimuli system doesn't work from modded components)                          |
| **BroadcastMessage**   | 66.2KB, 4 files     | 7KB                                                                             | 9.5× larger. Discord rich embeds with zero-dependency JSON + in-game welcome system                                                                                                                          |
| **Loot Respawn**       | 10.4KB, 2 files     | Full C# on [GitHub](https://github.com/laserman120/SOTF-Mod-LootRespawnControl) | Integer hash replaced MD5 — 600+ `PickUp.Awake()` calls made MD5 a bottleneck                                                                                                                                |
| **Water Collectors**   | 15.2KB, 2 files     | ~3KB                                                                            | 5.1× larger. `Physics.OverlapSphere()` stripped by IL2CPP — replaced with Harmony `OnEnable` tracking + `Vector3.Distance`                                                                                   |
| **Meat Dryer**         | 18.1KB, 2 files     | ~5KB                                                                            | 3.6× larger. Complete rewrite with seasonal drying system and fire proximity detection                                                                                                                       |
| **Stack Sizes**        | 14.9KB, 1 file      | ~4KB                                                                            | 3.7× larger. Per-item config entries across 14 categories, reset-to-defaults, tier guards                                                                                                                    |
| **Enemy Stats**        | (in RaidCustomizer) | —                                                                               | 4 failed approaches: `AccessTools.Field` → null, `GetField(NonPublic)` → null, `GetStat(Type)` → MissingMethodException, then unsafe `IntPtr + offset` pointer arithmetic                                    |
| **Follower HP**        | (in RaidCustomizer) | —                                                                               | VailActor has 13 stats, multiple with `_max=100`. Match by `_baseValue` not `_max`. Post-revive 1-second delayed timer to avoid game HP reset race                                                           |

Modules that exist **only** in Project X — no original equivalent:

- **Permission System** (15.5KB) — RBAC synced between server and clients
- **Config Sync** (14.9KB) — Real-time server→client config broadcast
- **Admin Bridge** (20.3KB) — Remote `/px` commands via hybrid Bolt/chat protocol
- **Building Enhancements** (26.9KB) — Custom building tools with server-side instant build
- **Integrity / Anti-Cheat** (40.6KB) — Server-side cheat detection
- **Custom GUI** (144KB) — Permission-aware IMGUI with 6 tabbed panels
- **One-click Installer** — Auto-detects game via Steam registry, backs up existing mods

See [`CREDITS.md`](CREDITS.md) for full attribution of all studied mods and their authors.

## Technical Approach

Project X is built on a fundamentally different architecture from standard Sons of the Forest mods:

- **Static class modules** with manual `Init()` — no `SonsMod` lifecycle dependency
- **Manual Harmony patching** per method — no `[HarmonyPatchAll]` attribute scanning (IL2CPP vtable safety)
- **Unsafe memory access** via pointer arithmetic at IL2CPP offsets for stats and state
- **Hybrid networking** — Bolt (server→client) + ChatBox interception (client→server)
- **Server-safe tick driver** — `SeasonsManager.LateUpdate` postfix for headless servers
- **Four-tier conditional compilation** — `#if SERVER / OWNER / CLIENT` strips irrelevant code from each build

See [`community_technical_guide.md`](community_technical_guide.md) for the full IL2CPP pattern catalogue (23 patterns documented).

## Documentation

| Document                                                       | Description                         |
| -------------------------------------------------------------- | ----------------------------------- |
| [`WHAT_IS_PROJECT_X.md`](WHAT_IS_PROJECT_X.md)                 | Detailed feature breakdown          |
| [`MODULE_DETAILS.txt`](MODULE_DETAILS.txt)                     | Per-module technical reference      |
| [`community_technical_guide.md`](community_technical_guide.md) | IL2CPP modding patterns & solutions |
| [`CREDITS.md`](CREDITS.md)                                     | Attribution & mod inspirations      |
| [`LICENSES.md`](LICENSES.md)                                   | Third-party license details         |

## License

[AGPL-3.0](LICENSE) — see [`CREDITS.md`](CREDITS.md) for full attribution of studied mods.

---

_Project X — by J4m3s & Claude • March 2026_
