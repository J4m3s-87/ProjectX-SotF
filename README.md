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

A key design goal is **dedicated server support**. Most existing mods are built on the `SonsMod` base class, which depends on a visible game window with a player camera and `OnGUI()` callbacks. A headless dedicated server (`SonsOfTheForestDS.exe`) has no renderer and no UI — so mods that rely on `SonsMod` lifecycle callbacks, IMGUI rendering, or camera-dependent logic simply won't load. Project X was designed from the start to work across all environments: solo, co-op host, dedicated server, and client.

For a detailed breakdown of each module's features, configuration options, and usage instructions, see [`Docs/MODULE_DETAILS.txt`](Docs/MODULE_DETAILS.txt).

---

## Architecture

### Three Editions, One Codebase

From a single shared codebase, three separate editions are compiled using `#if SERVER / OWNER / CLIENT` conditional compilation — code that doesn't belong in an edition is physically stripped at build time:

| Edition    | DLL                   | For                                                                          |
| ---------- | --------------------- | ---------------------------------------------------------------------------- |
| **Owner**  | `ProjectX.Owner.dll`  | Server admin's game client — full dashboard + remote commands                |
| **Server** | `ProjectX.Server.dll` | Headless dedicated server (`SonsOfTheForestDS.exe`)                          |
| **Client** | `ProjectX.Client.dll` | Players joining the server — receives server-pushed config, UI gated by role |

No other Sons of the Forest mod uses multi-tier conditional compilation. The originals are single-build, client-only.

### Modular Static Architecture

Each feature is a self-contained static class under `Modules/`. Modules are initialised manually via `Init()` calls — there is no dependency on the `SonsMod` lifecycle (`OnGameStart`, `OnInGameUpdate`), which doesn't exist on headless servers. This design also avoids `[HarmonyPatchAll]` attribute scanning, which can cause IL2CPP vtable corruption across 70+ source files.

```
Dev/ProjectX.Master/
├── MasterPlugin.cs          # Entry point — Init + tick driver
├── Config.cs                # Unified config (27 categories, 200+ settings)
├── Modules/
│   ├── Network/             # ConfigSync, RBAC, LootSyncEvent protocol
│   ├── DedicatedSuperuser/  # CommandBridge, chat routing, admin tools
│   ├── RaidCustomizer/      # Raid scheduling, stat overrides, announcements
│   ├── LootRespawn/         # Server-authoritative loot tracking + container respawn
│   ├── ScaryCross/          # DemonDetector MonoBehaviour, heat/fire state machine
│   ├── WeaponDamage/        # Per-weapon multipliers + solafite plating
│   ├── BroadcastMessage/    # Discord bridge + in-game welcome system
│   ├── UI/                  # 83KB IMGUI menu (7 tabs, permission-aware)
│   ├── Stack/               # 75+ per-item stack overrides
│   ├── BuilderStacks/       # Log/plank/stone carry capacity
│   └── ... (25 module directories total)
└── Assets/                  # Binary assets (icons, models)
```

### Server-Safe Tick Driver

On a dedicated server there is no `Update()` or `OnGUI()` callback. Project X runs its server-side logic via a Harmony postfix on `SeasonsManager.LateUpdate` — a method that runs every frame on both client and server. This postfix drives the `OnServerTick()` loop, which handles raid scheduling, loot persistence, config sync broadcasts, and periodic housekeeping.

---

## Networking & Admin Infrastructure

### The Admin Bridge

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

Config categories synced live include:

- **Raid economy** — scheduling, spawn control, enemy stat multipliers, multiplayer scaling
- **World economy** — structure durability, stack sizes, loot respawn interval, crafting speed
- **Weapon damage** — per-weapon multipliers for all ranged and melee weapons
- **Companion settings** — Kelvin and Virginia HP multipliers

All settings are applied server-side and pushed to clients — individual players cannot override the server's economy.

### LootSyncEvent Protocol

Loot tracking on dedicated servers uses `LootSyncEvent` (a custom `Packets.NetEvent` scope) with 6 binary message types:

| Code | Direction | Purpose                           |
| ---- | --------- | --------------------------------- |
| 0x01 | C→S       | Ground pickup collected           |
| 0x02 | C→S       | Openable container opened         |
| 0x03 | C→S       | Breakable container broken        |
| 0x10 | C→S       | Client requests full sync on join |
| 0x20 | S→C       | Full suppression list broadcast   |
| 0x21 | S→C       | Incremental suppression update    |

The server persists the tracker to `loot_collected.dat` in `UserData/` (SFTP-visible for server operators).

---

## IL2CPP Technical Approach

Sons of the Forest uses IL2CPP, which strips many standard .NET reflection paths. Project X uses a set of established patterns to work around these limitations:

- **Manual Harmony patching** per method — no `[HarmonyPatchAll]` (avoids IL2CPP vtable corruption)
- **Unsafe memory access** via pointer arithmetic at IL2CPP offsets (e.g., `0x4CC` for HP, `0x4D0` for damage) — used when all standard reflection paths return null
- **Cached reflection** using `AccessTools` for protected methods
- **DebugConsole dispatch** as a universal bypass for stripped internal APIs
- **Defensive polling patterns** instead of event-driven Harmony for maximum stability (e.g., Zipline module)
- **Integer hashing** replacing `MD5.Create()` for performance-critical paths (600+ `PickUp.Awake()` calls)

See [`community_technical_guide.md`](community_technical_guide.md) for the full IL2CPP pattern catalogue (23 patterns documented).

---

## Configuration System

Project X uses RedLoader's `ConfigSystem` with **27 config categories** and **200+ individual settings**, all manageable through:

1. **In-game native settings UI** — Settings → Mods → ProjectX sections
2. **IMGUI GUI panels** — 7 tabbed panels with sliders, checkboxes, and buttons
3. **Chat commands** — `/px config set <key> <value>` for headless server admin
4. **Config file** — `ProjectX.Master.cfg` (editable offline via the [Config Editor tool](Tools/ConfigEditor.html))

All config entries use `OnValueChanged` subscriptions for live application — no restart required.

---

## The Custom GUI

An **83KB IMGUI menu system** built entirely on Unity's built-in GUI system — not the SUI framework used by other mods (which crashes on dedicated servers). Activated with the Insert key:

- **7 tabbed panels**: Player, Environment, Teleport, Building+, Raids, Server Admin, Server Raids
- **Full slider and checkbox controls** with real-time feedback
- **Permission-aware rendering** — panels show/hide based on RBAC role
- **Red and black branded theme** with resolution-independent layout

---

## Project Structure

```
Dev/
├── ProjectX.Master/       # Shared codebase (all modules)
│   ├── MasterPlugin.cs    # Entry point + tick driver
│   ├── Config.cs          # 27 categories, 200+ settings
│   ├── Modules/           # 25 feature module directories
│   └── Assets/            # Binary assets (icons, models)
├── ProjectX.Client/       # Client build config
├── ProjectX.Owner/        # Owner build config
└── ProjectX.Server/       # Server build config
Tools/
├── ConfigEditor.html      # Offline config file editor (browser-based)
Installer/                 # One-click client installer
Docs/
├── MODULE_DETAILS.txt     # Per-module feature reference & usage guides
├── NEW_PLAYER_GUIDE.txt   # Player-facing quickstart guide
├── loot_respawn_system.md # Loot system technical reference
```

## Documentation

| Document                                                       | Description                                       |
| -------------------------------------------------------------- | ------------------------------------------------- |
| [`Docs/MODULE_DETAILS.txt`](Docs/MODULE_DETAILS.txt)           | Per-module feature reference & usage guides       |
| [`Docs/NEW_PLAYER_GUIDE.txt`](Docs/NEW_PLAYER_GUIDE.txt)       | Player-facing quickstart                          |
| [`community_technical_guide.md`](community_technical_guide.md) | IL2CPP modding patterns & solutions (23 patterns) |
| [`Tools/ConfigEditor.html`](Tools/ConfigEditor.html)           | Browser-based config file editor                  |
| [`CREDITS.md`](CREDITS.md)                                     | Attribution & mod inspirations                    |
| [`LICENSES.md`](LICENSES.md)                                   | Third-party license details                       |

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
