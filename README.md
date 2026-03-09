<p align="center">
  <h1 align="center">Project X</h1>
  <p align="center"><strong>A unified modding suite for Sons of the Forest</strong></p>
  <p align="center">
    One DLL • 20+ features • Dedicated server support • Multiplayer-ready
  </p>
</p>

---

## What Is Project X?

Project X combines the functionality of 20+ standalone mods into a single, unified package. Instead of installing and maintaining 15–20 separate DLLs — each with its own update cycle, each registering its own Harmony patches, and each a potential source of conflicts or crashes — everything runs from one assembly with shared infrastructure.

A key difference is **dedicated server support**. Most existing mods are built on the `SonsMod` base class, which depends on a visible game window with a player camera and `OnGUI()` callbacks. A headless dedicated server (`SonsOfTheForestDS.exe`) has no renderer and no UI — so mods that rely on `SonsMod` lifecycle callbacks, IMGUI rendering, or camera-dependent logic simply won't load. Simpler mods (like Hotbar or AmmoUi) that don't depend on these systems may work fine, but the more complex feature mods — raid customisation, loot respawn, admin tools — typically don't.

Project X was designed from the start to work across all environments: solo, co-op host, dedicated server, and client.

### Three Editions, One Codebase

From a single shared codebase, three separate editions are compiled using `#if SERVER / OWNER / CLIENT` conditional compilation — code that doesn't belong in an edition is physically stripped at build time:

| Edition    | DLL                   | For                                                                                                                    |
| ---------- | --------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| **Owner**  | `ProjectX.Owner.dll`  | Server admin's game client — full dashboard + remote commands                                                          |
| **Server** | `ProjectX.Server.dll` | Headless dedicated server (`SonsOfTheForestDS.exe`)                                                                    |
| **Client** | `ProjectX.Client.dll` | Players joining the server — receives server-pushed config (economy, raids, durability, etc.), UI access gated by role |

No other Sons of the Forest mod uses multi-tier conditional compilation. The originals are single-build, client-only.

---

## Features

### Player Cheats & Quality of Life

- **God Mode** — Full invulnerability via the game's debug console API
- **Infinite Stamina** — Never run out of energy
- **No Hunger / No Thirst / No Fatigue** — Survival stats locked at full. _Unlike simple toggle mods, this uses hybrid reflection injection to bypass protected IL2CPP methods that are normally inaccessible_
- **No Fall Damage** — Safe exploration from any height
- **NoClip / Fly Mode** — Full 3D flight with physics bypass, WASD + Space/Ctrl for vertical control
- **Season Control** — Instantly switch between Spring, Summer, Autumn, and Winter
- **Unstuck Kelvin / Virginia** — Teleport companions 2m in front of you when they get stuck

### Building & Construction

- **Free Form Placement** — Remove snapping restrictions. _We discovered that the commonly referenced console commands (`freeformplace`, `skipwoodcuttings`) don't actually exist — we found the real API: `GameSetupManager.SetFreeFormForcePlaceFullLoadSetting()`_
- **Instant Build** — Skip construction animations. _On dedicated servers, a one-shot `finishblueprints` fires when toggled on (for existing blueprints), and a Harmony PREFIX on `PlaceStructureNode` handles all new placements with `instantBuild=true`. Previous polling approach removed — the game logs every `finishblueprints` call_
- **Structure Relocator** — Pick up and reposition placed structures instead of destroying them, with a backup dictionary that preserves original placement modes
- **Structure Durability Multiplier** — Configurable slider. _Rebuilt using Harmony postfix on `GetStructureInfo()` because IL2CPP strips direct field access on `ScrewStructure._hp`_
- **Log Hack / Stone Hack** — Infinite building materials

### Weapons & Combat

- **Weapon Damage Multiplier** — Adjustable slider for player weapon damage
- **Enemy HP & Damage Multipliers** — Per-type control over Cannibals, Creepies, and Bosses. _This required developing an entirely new approach: after 4 failed attempts (AccessTools.Field → null, GetField(NonPublic) → null, GetStat(Type) → MissingMethodException, property setter → wrong type), we landed on unsafe pointer arithmetic — direct memory writes at offsets 0x4CC (HP), 0x4D0 (damage), and 0x218+0x18 (aggression). IL2CPP strips all normal reflection paths_
- **Kelvin & Virginia HP Persistence** — _The first mod to keep companion HP at a set value even after first aid revival._ VailActor has 13 stats, many sharing `_max=100`. We identify the correct stat by matching `_baseValue` (not `_max`), and use a 1-second delayed timer after revival to avoid a race condition with the game's `DyingRecoverHealth` recovery logic
- **Kill All / Burn All Enemies** — Radius-based with entity filtering
- **Freeze AI** — Pause world simulation entirely

### Inventory & Items

- **Custom Stack Sizes** — 75+ individually configurable items across 14 categories (Crafting, Meds, Food, Armor, Ammo, and more). _3.7× larger than the original StackMod — adds per-item config entries, reset-to-defaults crash prevention, and tier guards_
- **Infinite Items** — Toggle to set all items to max
- **Builder Stacks** — Separate carry amount overrides for building materials with category filtering and backup/restore
- **Ammo UI** — On-screen ammunition counter showing current weapon ammo. _Condensed from 6 files (58KB) to 1 file (10KB) with sprite caching and frame throttling_

### World & Environment

- **Raid Customiser** — Full control over enemy raid scheduling, frequency, and composition. Configure time-of-day windows (morning/day/evening/night), spawn factors, enemy limits, enable/disable specific enemy types, and control boss spawn counts. **Raid announcements** display on-screen messages showing raid type and enemy count when incoming. Quick-action buttons for triggering, clearing, or requeuing raids on demand. _Server-authoritative — the server controls all raid scheduling, with multiplayer player-count scaling that adjusts difficulty based on how many players are connected_
- **Loot Respawn** — Configurable respawn timer for ground pickups, openable containers (ammo cases, pelican cases), and breakable containers (coffins, crates). _Server-authoritative tracking via LootSyncEvent (custom Packets.NetEvent protocol with 6 message types). Container respawn is entirely new — the original mod only handles ground pickups. Non-loot breakables (stick piles, stumps) are filtered via `IsNonLootBreakable()`. Replaced MD5 hashing with integer hash after profiling showed 600+ `PickUp.Awake()` calls made `MD5.Create()` a bottleneck_
- **Zipline Extender** — Extended zipline and rope bridge ranges up to 15,000 units (vanilla ~40m). _Uses a polling pattern instead of Harmony attributes for IL2CPP compatibility_
- **Water Collector Heat Radius** — Keep rain catchers unfrozen in winter when near fire. _5.1× larger than the original — `Physics.OverlapSphere()` is stripped by IL2CPP, so we built a Harmony `OnEnable` tracking system for heat sources with `Vector3.Distance` checks and state-cached `SetFrozen()` calls_
- **Crafting Speed** — Multiplier for crafting animation speed
- **Waterfall Sound Control** — Audio volume slider via FMOD emitter tracking
- **Force Rain** — Toggle heavy rain on/off instantly
- **No World Gravity** — Toggle world gravity via debug console (`gravity 0` / `gravity -9.81`)

### Unique Features — No Other Mod Has These

- **Scary Cross** — Electrified cross structure with a custom `DemonDetector` MonoBehaviour. When powered, it detects nearby enemies, progressively increases light intensity (4096–32768), heats up, catches fire, and burns enemies within range. Progressive lightbulb failure based on HP damage. _4.4× larger than the original (70KB vs 16KB) — the original's stimuli system doesn't work from modded components, so we bypass it entirely with direct `VailActor.IgniteSelf()` calls_
- **Meat Dryer** — Complete rewrite with seasonal drying speeds (Spring 30×, Summer 25×, Autumn 15×, Winter 5×) and fire proximity detection with per-season temperature thresholds. _3.6× larger than the original — the original was bugged and had no seasonal awareness_
- **Discord Bridge** — Player join/leave/death/raid alerts as colour-coded rich embeds with Project X branding (#1a1a1a + #ff0000). _Built with zero-dependency JSON construction — no external HTTP libraries, just manual JSON escaping for IL2CPP compatibility. 9.5× larger than the original BroadcastMessage (66KB vs 7KB)_
- **In-Game Welcome System** — Players joining the server receive a multi-line orientation via chat covering server rules, features, and save mechanics (world auto-saves vs manual personal saves)
- **One-Click Installer** — Pure batch/PowerShell installer that auto-detects the game via Steam registry and VDF parsing, backs up existing mods to timestamped directories, validates prerequisites (.NET 6.0, VC++), and deploys with zero dependencies

---

## The Admin Bridge

On a dedicated server, there's no screen to click on. Project X solves this with a **hybrid networking protocol** — Bolt for server-to-client messaging, and the game's chat system for client-to-server commands:

1. Type `/px godmode on` in the game chat from your Owner client
2. The chat message is intercepted before it reaches other players
3. The command is routed to the server's `CommandBridge`
4. The server executes the command and sends a confirmation back

Over 40 commands covering player cheats, world settings, raid control, config management, and server administration — all without needing direct console access to the server.

### Permission System (RBAC)

Project X implements **Role-Based Access Control** with Owner and Admin tiers, managed via `roles.json` using SteamID64 mappings. When a player joins, the server broadcasts their permission level via Bolt, and the client's menu adapts in real-time — showing or hiding features based on authorisation. Standard players see nothing; the mod is invisible to them.

### Config Sync — Live Server Economy Control

When the server owner or admin adjusts settings, those changes are broadcast to all connected clients in real-time via Bolt GlobalEvents. No config files to distribute, no server restarts required. The admin sets the server economy and every player receives those settings automatically.

**What can be changed live while the server is running:**

**Raid Economy** — Full control over how raids behave on the server:

- Raid scheduling: raids per day, time-of-day windows (morning/day/evening/night)
- Enemy types: enable/disable Cannibals, Creepies, and Muddies independently
- Spawn control: min/max spawn factors, enemy limits, boss spawn counts
- Stat multipliers: per-type HP and damage multipliers for Cannibals, Creepies, and Bosses (0.1×–10×)
- Cooldowns: separate cooldown timers for normal and boss raids
- Day/anger gate overrides: bypass minimum day and anger requirements
- Multiplayer scaling: extra raids, spawns, and bosses per connected player

**World Economy** — Server-wide settings that affect all players:

- Structure durability multiplier (1×–100×)
- Stack sizes across 75+ items in 14 categories
- Loot respawn enable/disable and respawn interval (1–30 in-game days)
- Crafting speed multiplier (1×–10×)
- Water collector heat radius
- Weapon damage multiplier
- Building cheats: free form placement, instant build, log/stone hacks

**Companion Settings:**

- Kelvin and Virginia HP multipliers

All settings are applied server-side and pushed to clients — individual players cannot override the server's economy. Admins adjust values via the GUI panels or `/px config set <key> <value>` commands.

---

## The Custom GUI

An **83KB IMGUI menu system** built entirely on Unity's built-in GUI system — not the SUI framework used by other mods (which crashes on dedicated servers). Activated with the Insert key:

- **7 tabbed panels**: Player, Environment, Teleport, Building+, Raids, Server Admin, Server Raids
- **Full slider and checkbox controls** with real-time feedback
- **Permission-aware rendering** — panels show/hide based on RBAC role
- **Red and black branded theme** with resolution-independent layout

---

## Quick Install (Client)

1. Download the `Installer/` folder
2. Double-click **`INSTALL.bat`**
3. Follow the prompts — the installer handles everything:
   - Auto-detects your game directory via Steam registry and VDF parsing
   - Downloads and installs .NET 6.0 Desktop Runtime if missing
   - Downloads and installs VC++ 2015-2019 Redistributable if missing
   - Installs RedLoader 0.8.6 via RedModManager
   - Deploys Project X Client (DLL, manifest, assets)
4. Launch Sons of the Forest and join the server

See [`Installer/payload/README.txt`](Installer/payload/README.txt) for manual install instructions and troubleshooting.

---

## How Project X Differs from the Originals

Every feature was independently implemented. The original mods were studied to understand _which game APIs to target_ — but the implementations had to be completely rewritten because the architectures are incompatible.

The originals use `SonsMod` with `[HarmonyPatchAll]` attribute scanning. Project X uses static classes with manual `harmony.Patch()` per method. You cannot copy code between these patterns — a `[HarmonyPatch]` attribute doesn't work in a manual patching system, and a `SonsMod.OnInGameUpdate()` callback doesn't exist in static modules.

### Why Everything Had to Be Rebuilt

Most original mods depend on Unity's renderer and lifecycle callbacks (`OnInGameUpdate()`, `OnGameStart()`). On a headless dedicated server, these callbacks never fire. This isn't a minor incompatibility:

- **No `SonsMod` lifecycle** — We built our own tick drivers using Harmony postfixes on `SeasonsManager.LateUpdate`
- **No GUI on servers** — We built a remote command system (`/px` via chat protocol) for headless admin control
- **Server-authoritative stats** — Enemy stats must be modified on the server via unsafe pointer arithmetic because IL2CPP strips reflection APIs
- **Permissions** — Public servers need access control, so we built RBAC synced between server and clients via Bolt

### Module-by-Module Comparison

| Module                 | Project X         | Original                                                             | Key Difference                                                  |
| ---------------------- | ----------------- | -------------------------------------------------------------------- | --------------------------------------------------------------- |
| **RaidCustomizer**     | 99.6KB, 11 files  | 76KB, 16 files                                                       | Server-authoritative, unsafe pointer stats, multiplayer scaling |
| **DedicatedSuperuser** | 102.6KB, 11 files | 33KB                                                                 | 3.1× larger — remote command bridge, RBAC, chat routing         |
| **ScaryCross**         | 70.4KB, 3 files   | 16KB                                                                 | 4.4× larger — custom DemonDetector, heat/fire state machine     |
| **BroadcastMessage**   | 66.2KB, 4 files   | 7KB                                                                  | 9.5× larger — Discord rich embeds + in-game welcome             |
| **MeatDryer**          | 18.1KB, 2 files   | ~5KB                                                                 | 3.6× larger — seasonal system, fire proximity                   |
| **WaterCollectors**    | 15.2KB, 2 files   | ~3KB                                                                 | 5.1× larger — IL2CPP-safe heat tracking                         |
| **Stack**              | 14.9KB, 1 file    | ~4KB                                                                 | 3.7× larger — per-item configs, tier guards                     |
| **LootRespawn**        | 95KB, 4 files     | [GitHub](https://github.com/laserman120/SOTF-Mod-LootRespawnControl) | Container respawn + LootSyncEvent protocol (entirely new)       |

**Modules with no original equivalent (~260KB+ of unique code):**

- **Permission System** (15.5KB) — RBAC synced between server and clients
- **Config Sync** (14.9KB) — Real-time server→client config broadcast
- **Admin Bridge** (20.3KB) — Remote `/px` commands via hybrid Bolt/chat protocol
- **Building Enhancements** (26.9KB) — Server-side instant build + correct API discovery
- **Integrity / Anti-Cheat** (40.6KB) — Server-side cheat detection
- **Custom GUI** (144KB) — Permission-aware IMGUI with 6 tabbed panels

See [`CREDITS.md`](CREDITS.md) for full attribution of all studied mods and their authors.

---

## Technical Approach

- **Static class modules** with manual `Init()` — no `SonsMod` lifecycle dependency
- **Manual Harmony patching** per method — no `[HarmonyPatchAll]` (IL2CPP vtable safety)
- **Unsafe memory access** via pointer arithmetic at IL2CPP offsets
- **Hybrid networking** — Bolt (server→client) + ChatBox interception (client→server)
- **Server-safe tick driver** — `SeasonsManager.LateUpdate` postfix for headless servers
- **Four-tier conditional compilation** — `#if SERVER / OWNER / CLIENT` strips irrelevant code
- **Defensive polling patterns** instead of event-driven Harmony for maximum stability
- **Cached reflection** for protected methods using `AccessTools`
- **DebugConsole dispatch** as a universal bypass for stripped internal APIs

72 source files, 750KB+ of custom C# code, 200+ documented development phases.

See [`community_technical_guide.md`](community_technical_guide.md) for the full IL2CPP pattern catalogue (23 patterns documented).

---

## Project Structure

```
Dev/
├── ProjectX.Master/       # Shared codebase (all modules)
│   ├── Modules/           # Feature modules (one folder each)
│   │   ├── RaidCustomizer/
│   │   ├── DedicatedSuperuser/
│   │   ├── ScaryCross/
│   │   ├── UI/
│   │   └── ...
│   └── Assets/            # Binary assets (icons, models)
├── ProjectX.Client/       # Client build config
├── ProjectX.Owner/        # Owner build config
└── ProjectX.Server/       # Server build config
Installer/                 # One-click client installer
```

## Documentation

| Document                                                       | Description                                      |
| -------------------------------------------------------------- | ------------------------------------------------ |
| [`MODULE_DETAILS.txt`](MODULE_DETAILS.txt)                     | Per-module technical reference with usage guides |
| [`community_technical_guide.md`](community_technical_guide.md) | IL2CPP modding patterns & solutions              |
| [`CREDITS.md`](CREDITS.md)                                     | Attribution & mod inspirations                   |
| [`LICENSES.md`](LICENSES.md)                                   | Third-party license details                      |

## Who It's For

- **Dedicated server operators** who had limited modding options before Project X
- **Friend groups** who want modded co-op without managing 20+ separate mods
- **Community server owners** who need admin tools, permissions, config sync, and Discord integration
- **Players** who want one install instead of troubleshooting 15 mod conflicts

One file. One install. Everything works.

## License

[AGPL-3.0](LICENSE) — see [`CREDITS.md`](CREDITS.md) for full attribution of studied mods.

---

_Project X — by J4m3s & Claude • March 2026_
