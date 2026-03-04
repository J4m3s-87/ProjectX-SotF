# What Is Project X?

Project X is a comprehensive modding suite for **Sons of the Forest** — a single DLL that replaces 20+ individual mods with one unified, server-aware package. It was built from the ground up by J4m3s with the help of Claude (AI pair programmer) over hundreds of hours, and is designed to work across **dedicated servers** and **multiplayer co-op** — environments that no other mod supports.

---

## The Problem It Solves

Sons of the Forest has a thriving modding scene, but every mod is built as a standalone package. If you want extended ziplines, customised raids, stack size overrides, a mod menu, structure durability, weapon damage tweaks, and admin tools, you need to install and maintain 15-20 separate mods — each with its own config file, its own update cycle, and its own potential to crash the game.

Worse, **none of these mods work on dedicated servers**. Every existing mod is built on the `SonsMod` base class, which requires a visible game window with a player camera. A headless dedicated server (`SonsOfTheForestDS.exe`) has no renderer, no UI, and no game loop callbacks. Install any standard mod on a dedicated server and it simply won't load.

Project X solves both problems. It merges the functionality of 20+ mods into a single assembly, rewrites everything to work without a game window, and adds an entire layer of networking, permissions, and remote administration that doesn't exist anywhere else.

---

## What It Actually Does

### Player Cheats & Quality of Life

Project X gives players (or admins) control over core survival mechanics:

- **God Mode** — Full invulnerability via the game's debug console API
- **Infinite Stamina** — Never run out of energy
- **No Hunger / No Thirst / No Fatigue** — Survival stats locked at full using hybrid reflection injection (bypassing protected IL2CPP methods)
- **No Fall Damage** — Safe exploration
- **NoClip / Fly Mode** — Full 3D flight with physics bypass, WASD movement plus Space/Ctrl for vertical control
- **Season Control** — Instantly switch between Spring, Summer, Autumn, and Winter

### Building & Construction

A full suite of building enhancements, all accessible from the in-game menu:

- **Free Form Placement** — Remove snapping restrictions for creative builds
- **Instant Build** — Skip the construction animation
- **No Wood Cuttings** — Trees don't leave stumps
- **Log Hack** — Infinite logs for building
- **Stone Hack** — Infinite stones
- **Structure Durability Multiplier** — Make buildings tougher (or weaker) with a slider
- **Structure Relocator** — Pick up and move placed structures instead of destroying them, with a backup dictionary that preserves original placement modes

### Weapons & Combat

- **Weapon Damage Multiplier** — Adjust how much damage your weapons deal via a configurable slider
- **Enemy HP & Damage Multipliers** — Server-side control over how tough enemies are, using direct unsafe memory writes at specific offsets (0x4CC for HP, 0x4D0 for damage) because IL2CPP strips the normal reflection APIs
- **Kill All Enemies / Animals** — Clear the area with radius-based filtering
- **Burn All Enemies** — Set all nearby enemies on fire
- **Freeze AI** — Pause the world simulation entirely

### Inventory & Items

- **Custom Stack Sizes** — 75+ individually configurable item stack limits across 14 categories (Crafting, Meds, Food, Armor, Ammo, and more)
- **Infinite Items** — Toggle unlimited item counts
- **Builder Stacks** — Separate carry amount overrides for building materials with category filtering and backup/restore
- **Ammo UI** — On-screen ammunition counter showing current weapon ammo

### World & Environment

- **Raid Customiser** — Full control over enemy raid scheduling, frequency, and composition. Configure which time slots allow raids (morning/day/evening/night), adjust spawn factors, set enemy limits, enable or disable specific enemy types (cannibals, creepy mutants, muddies), and control boss spawn counts. Quick-action buttons let admins trigger, clear, or requeue raids on demand
- **Loot Respawn** — Configurable loot respawn system based on in-game day tracking
- **Zipline Extender** — Massively extended zipline and rope bridge ranges with linear sliders (up to 15,000 units)
- **Waterfall Sound Control** — Adjust the volume of waterfalls via an audio polling system
- **Crafting Speed** — Multiplier for how fast items are crafted
- **Water Collector Heat Radius** — Extend the effective range of water collectors near heat sources

### Unique Features (No Other Mod Has These)

- **Kelvin & Virginia HP Persistence** — The first mod to keep companion HP at a set value even after first aid is given, preventing the need to kill and respawn early-game companions
- **Scary Cross** — Enhanced cross structures with demon detection (custom MonoBehaviour), fire effects, and player effigy stimuli harvesting
- **Meat Dryer Rewrite** — Complete rewrite of the original (which was bugged) with seasonal drying times, fire proximity detection, and configurable settings

---

## The Server Architecture

What makes Project X fundamentally different from any other Sons of the Forest mod is its **three-tier build system**. From a single shared codebase, three separate editions are compiled:

| Edition    | DLL                   | Who Uses It                                                     |
| ---------- | --------------------- | --------------------------------------------------------------- |
| **Owner**  | `ProjectX.Owner.dll`  | The server administrator's game client — full admin dashboard   |
| **Server** | `ProjectX.Server.dll` | Headless dedicated server — no UI, runs `SonsOfTheForestDS.exe` |
| **Client** | `ProjectX.Client.dll` | Players joining the server — RBAC-gated, vanilla by default     |

Each edition is built using `#if SERVER / OWNER / CLIENT` conditional compilation, meaning client-only code (like the GUI) is physically stripped from the server binary, and server-only code (like permission management) is stripped from the client build.

### The Admin Bridge

On a dedicated server, there's no screen to click on. Project X solves this with the **Admin Bridge** — a hybrid networking protocol that uses Bolt (server-to-client) and the game's chat system (client-to-server) to enable remote administration:

- Type `/px godmode on` in the game chat from your Owner client
- The chat message is intercepted before it reaches other players
- The command is routed to the server's `CommandBridge`
- The server executes the command and sends a confirmation back

This gives administrators full control over a headless server without needing direct console access — over 40 commands covering player cheats, world settings, raid control, and server management.

### Permission System (RBAC)

Project X implements **Role-Based Access Control** with two tiers:

- **Owner** — Full world-state control, all commands, all menu features
- **Admin** — Utilities, kick, revive, raid control

Roles are managed via a `roles.json` file using SteamID64 mappings. When a player joins, the server broadcasts their permission level via Bolt networking, and the client's menu adapts in real-time — showing or hiding features based on what they're authorised to do. Standard players see nothing; the mod is invisible to them.

### Config Sync

When the server owner adjusts settings (stack sizes, durability multipliers, raid frequency), those changes are broadcast to all connected clients in real-time via Bolt GlobalEvents. No config files to distribute, no restarts required.

---

## The Discord Integration

Project X includes a **Discord Bridge** that connects the game server to a Discord channel:

- **Player Joins** — Colour-coded rich embed announcing who joined, with Project X branding
- **Player Leaves** — Departure notification with session context
- **Player Deaths** — Death announcements for community engagement
- **Raid Alerts** — Notifications when enemy raids are triggered
- **Welcome Embeds** — Formatted welcome message with server rules and save mechanic education

All embeds are built using **zero-dependency JSON construction** — no external HTTP libraries, just manual JSON escaping for IL2CPP compatibility. The colour palette matches the Project X brand (black #1a1a1a + red #ff0000).

### In-Game Welcome System

Players joining the dedicated server also receive an **in-game orientation** via the chat box — a multi-line welcome message covering server rules, available mod features, and critical save mechanics (like the distinction between 5-minute world auto-saves and manual personal saves). This runs automatically for every new connection.

---

## The Custom GUI

Project X features an **83KB IMGUI menu system** (activated with the Insert key) built entirely on Unity's built-in GUI system — not the SUI framework used by other mods (which crashes on dedicated servers). The menu includes:

- **6 tabbed panels**: Player, Weather, World, System, Structures, Raids
- **Full slider and checkbox controls** with real-time feedback
- **Permission-aware rendering** — panels show/hide based on the player's RBAC role
- **Red and black branded theme** with the "PROJECT X MOD MENU" header
- **Resolution-independent layout** using parent-relative positioning
- **Save Config button** for explicit configuration persistence

---

## Installation

Project X ships with a **one-click installer** — a pure batch file that:

1. Auto-detects the game installation path via Steam registry and VDF file parsing
2. Backs up any existing mods to a timestamped directory
3. Deploys the correct DLL and assets to the game's `Mods/` folder
4. Detects whether RedLoader (the mod framework) is installed and provides instructions if it's missing

No PowerShell dependencies, no admin rights required, no manual file copying. One double-click and it's done.

---

## Technical Foundation

Project X is built on a completely different architecture from standard Sons of the Forest mods:

- **Static class architecture** with manual `Init()` calls instead of `SonsMod` lifecycle
- **Manual Harmony patching** (`harmony.Patch()` per method) instead of `[HarmonyPatchAll]` attribute scanning
- **Unsafe memory access** using pointer arithmetic for enemy stats because IL2CPP strips reflection metadata
- **Cached reflection** for protected methods (like `SetMin` on vitals) using `AccessTools`
- **DebugConsole dispatch** as a universal bypass for stripped internal APIs
- **Defensive polling patterns** instead of event-driven Harmony for maximum stability

The project comprises **72 source files totalling over 750KB of custom C# code**, with a development history spanning 200+ documented phases of iterative problem-solving against the IL2CPP runtime environment.

---

## Who It's For

- **Dedicated server operators** who had zero modding options before Project X
- **Friend groups** who want modded co-op without technical setup
- **Players** who want one mod instead of managing 20+
- **Community server owners** who need admin tools, permissions, and Discord integration

One file. One install. Everything works.

---

_Project X — by J4m3s & Claude_
_March 2026_
