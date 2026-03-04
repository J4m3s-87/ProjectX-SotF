<p align="center">
  <h1 align="center">Project X</h1>
  <p align="center"><strong>A unified modding suite for Sons of the Forest</strong></p>
  <p align="center">
    One DLL • 20+ features • Dedicated server support • Multiplayer-ready
  </p>
</p>

---

## What Is Project X?

Project X replaces 20+ standalone mods with a single, unified package built from the ground up. It's the **only mod for Sons of the Forest that works on dedicated servers** — headless environments where every other mod fails because there's no game window, no renderer, and no UI.

From a single shared codebase, three editions are compiled:

| Edition    | DLL                   | For                                                               |
| ---------- | --------------------- | ----------------------------------------------------------------- |
| **Owner**  | `ProjectX.Owner.dll`  | Server admin's game client — full dashboard + remote commands     |
| **Server** | `ProjectX.Server.dll` | Headless dedicated server (`SonsOfTheForestDS.exe`)               |
| **Client** | `ProjectX.Client.dll` | Players joining the server — permission-gated, vanilla by default |

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

## Technical Approach

Project X is built on a fundamentally different architecture from standard Sons of the Forest mods:

- **Static class modules** with manual `Init()` — no `SonsMod` lifecycle dependency
- **Manual Harmony patching** per method — no `[HarmonyPatchAll]` attribute scanning (IL2CPP vtable safety)
- **Unsafe memory access** via pointer arithmetic at IL2CPP offsets for stats and state
- **Hybrid networking** — Bolt (server→client) + ChatBox interception (client→server)
- **Server-safe tick driver** — `SeasonsManager.LateUpdate` postfix for headless servers

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
