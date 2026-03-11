# Loot Respawn System — Comprehensive Technical Reference

Last Updated: March 11, 2026

The Loot Respawn system tracks collected loot items, opened containers, and broken crates, allowing them to respawn after a configurable number of in-game days. This document covers how the system works in **both local play (solo/P2P)** and **dedicated server** scenarios.

---

## 1. Overview

When a player collects a pickup, opens a container, or breaks a crate, the mod records a **unique positional hash** and a **game-time timestamp**. While the timer is active, the item is **suppressed** — hidden, non-interactable, and invisible to the player. Once the respawn timer expires, the item returns to the world as if never collected.

### Supported Item Types

| Type | Examples | Tracking Method |
|---|---|---|
| **Ground Pickups** | MREs, duct tape, ammo boxes, weapons, meds | `PickUp.Collect` Harmony Postfix |
| **Openable Containers** | Ammo cases, pelican cases, suitcases, meds crates | `ContainerItemSpawner.OpenContainer` Harmony PREFIX+POSTFIX |
| **Breakable Containers** | Wooden crates, coffins, gore pots | `BreakableObject.OnBreak` Harmony PREFIX+POSTFIX |

### Item Identification

- **Ground Pickups**: Integer hash combining `name.GetHashCode()` + quantized position (replacing MD5 for performance — 600+ `PickUp.Awake` calls on world load made `MD5.Create()` + `ComputeHash()` a bottleneck)
- **Openable Containers**: Hierarchical container name (e.g., `OceanCrashPickups.Meds.AAD`) — stable across sessions
- **Breakable Containers**: Same hash as pickups but with a special pseudo-ID (`9999`)

---

## 2. How It Works — Local Play (Solo / P2P Host)

In local play, everything runs on the same machine. The player's game is both the **authority** and the **renderer**.

### 2.1 Collection Flow

```
Player picks up item
    → PickUp.Collect fires (Harmony Postfix)
    → ShouldTrackItem(itemId) check — skip building materials, blacklisted, disabled categories
    → Hash retrieved from _hashCache (generated during Awake)
    → Record { hash, timestamp, itemId } in _collected dictionary
    → Set _dirty = true
    → Save to loot_collected.dat within 5 seconds
```

### 2.2 Suppression Flow (World Load)

When the world loads, every pickup and container fires its `Awake` method. The mod intercepts these:

```
PickUp.Awake fires
    → If save data not loaded yet → queue in _pendingPickups (deferred)
    → If hash found in _collected:
        → Timer expired? → Remove from _collected, add to _recentlyRespawned, call RestorePickup
        → Timer active?  → Call SuppressPickup (hide + disable)
    → If hash NOT in _collected → item spawns normally
```

**Deferred Processing**: Items that `Awake` before save data loads are queued. Once `OnGameStarted` fires and sets `_saveDataLoaded = true`, all pending items are processed in bulk.

### 2.3 Suppression Mechanics

When an item is suppressed, four things happen:

| Step | What | Why |
|---|---|---|
| `transform.localScale = Vector3.zero` | Hides the 3D model | Works even when child meshes haven't loaded yet |
| `_PickupGui_` child `.SetActive(false)` | Hides the floating interaction icon | Separate world-space GUI element not affected by parent scale |
| `DisableCollidersRecursive()` | Disables all colliders on the hierarchy | Prevents phantom interactions (invisible item still clickable) |
| `pickup.enabled = false` | Disables the PickUp component | Prevents the collection system from detecting it |

### 2.4 Restoration Mechanics

When a timer expires **mid-session** (player is still in the world and walks into a streaming zone), the item needs to be visually restored without reloading:

| Step | What |
|---|---|
| `transform.localScale = Vector3.one` | Restores the 3D model to normal size |
| `_PickupGui_` child `.SetActive(true)` | Re-enables the floating interaction icon |
| `EnableCollidersRecursive()` | Re-enables all colliders |
| `pickup.enabled = true` | Re-enables the PickUp component |

**Important Guard**: `RestorePickup` is only called if `localScale == Vector3.zero`. On game reload, items are **fresh instances** with their correct original scale — calling RestorePickup on these would cause giant icon bugs.

### 2.5 Container-Specific Behaviour

#### Openable Containers (Ammo Cases, Pelican Cases)

The PREFIX on `ContainerItemSpawner.OpenContainer` handles four states:

1. **`_recentlyRespawned` + streaming** (`!_saveDataLoaded`): Block (streaming replay — game is replaying saved state)
2. **`_recentlyRespawned` + player** (`_saveDataLoaded`): Allow through, remove from set (player can loot it)
3. **In `_collected` + timer expired**: Remove from tracker → return `true` (container is re-lootable)
4. **In `_collected` + timer active**: Mark visually opened → return `false` (suppressed, can't interact)

**Visual State**: The `ContainerItemSpawner.Start` hook applies opened appearance to suppressed containers immediately on load, so players see them as already-opened rather than mysteriously un-interactable.

#### Breakable Containers (Wooden Crates, Coffins)

Breakable containers are more complex because the game replays `OnBreak` during streaming:

- **Frame-based blocking**: `_breakableLoadFrame` dictionary tracks which frame each breakable's `Awake` fired. If `OnBreak` fires within 120 frames of `Awake`, it's treated as a streaming replay and blocked
- **ClearContainerSaveState**: When a breakable respawns, the child `ContainerItemSpawner` has its save data wiped (`ClearStateSync` + `set_isOpen(false)`) so fresh loot spawns from the `contentsSeed`
- **Event ordering race**: Sometimes `OnBreak` fires before `Awake` POSTFIX on the same frame during streaming — the PREFIX checks `_collected` directly as a fallback

---

## 3. How It Works — Dedicated Server

On a dedicated server, the **server is authoritative** — it maintains the central tracker and decides which items exist. Clients receive state via the `LootSyncEvent` network protocol.

### 3.1 Key Differences from Local Play

| Aspect | Local Play | Dedicated Server |
|---|---|---|
| **Authority** | Local machine is truth | Server is truth |
| **Collection** | Direct Harmony hooks record locally | Client sends LootSyncEvent to server |
| **Persistence** | Local `UserData/loot_collected.dat` | Server `UserData/loot_collected.dat` |
| **Suppression sync** | N/A (single machine) | Server broadcasts to all clients |
| **Timer checks** | Run locally on load + streaming | Run on server |
| **Container events** | Fire locally | Fire server-side via Harmony (no C→S needed) |

### 3.2 Client Collection Flow

```
Player picks up item on client
    → PickUp.Collect fires locally
    → Client sends LootSyncEvent 0x01 (hash + itemId) to server
    → Server records { hash, timestamp, itemId } in _collected
    → Server broadcasts 0x21 (incremental suppress) to all tracked clients
    → All clients add hash to their local _collected
```

### 3.3 Client Join / Sync Flow

```
Client joins server
    → OnGameStarted fires → _collected.Clear() (start fresh)
    → Client sends LootSyncEvent 0x10 (request full state)
    → Server sends 0x20 (full suppression list — all tracked hashes)
    → Client applies state via ApplyServerState()
    → _serverSyncReceived = true
    → All subsequent PickUp.Awake calls now check against server data ✅
```

**Before sync is received**: The client's `PickUp.Awake` hook returns immediately (no suppression). This prevents false positives during the brief window before the server responds.

### 3.4 Container Events on Server

Container breaks and opens fire **server-side directly** via Harmony hooks — no client→server message is needed for these since the game already handles container state on the server:

- `BreakableObject.OnBreak` — fires on the server, records break directly
- `ContainerItemSpawner.OpenContainer` — fires on the server, records open directly

### 3.5 Suppression on Clients

When a client receives suppression data (via 0x20 or 0x21), it adds the hash to its local `_collected`. The next time a matching `PickUp.Awake` fires (e.g., player enters a streaming zone), the item is suppressed locally using the same `SuppressPickup` mechanics.

### 3.6 Lifecycle Fix

On dedicated servers, `OnSdkInitialized` does NOT fire. `Init()` runs inside `OnGameStart()` — **after** `OnGameStarted()` has already set `_saveDataLoaded = true`. The fix: `Init()` preserves `_saveDataLoaded` if already true, and `OnSceneInit()` skips reset if it's already true.

---

## 4. LootSyncEvent Network Protocol

All dedicated server sync uses `LootSyncEvent` (`Packets.NetEvent`, scope `ProjectX.LootSync`).

### Message Types

| Code | Direction | Payload | Purpose |
|---|---|---|---|
| `0x01` | C→S | hash (string) + itemId (int) | Client collected a ground pickup |
| `0x02` | C→S | hash (string) | Client broke a container |
| `0x03` | C→S | hash (string) | Client opened a container |
| `0x10` | C→S | _(empty)_ | Client requests full suppression list |
| `0x20` | S→C | List of all tracked hashes | Full state sync (response to 0x10) |
| `0x21` | S→C | Single hash (string) | Incremental suppression (broadcast to all) |

### Connection Tracking

The server tracks connected clients in a `_trackedClients` HashSet, populated when a client sends `0x10`. Broadcasts iterate this set with dead-connection cleanup. Direct `BoltNetwork.connections` iteration does **not** work in IL2CPP, so the tracked set is necessary.

### Fallback Path

`LootEventListener` remains registered as a `GlobalEventListener` for the `debugCommand` C→S path. Pickup collections (`0x01`) still use this as a proven fallback. Container events (`0x02`/`0x03`) use `LootSyncEvent` directly.

---

## 5. Timer System

### How Timers Work

- **Timestamp**: When an item is collected, the current game time is recorded as a Unix-style timestamp (`GetGameTimestamp()`)
- **Check**: `HasEnoughTimePassed(timestamp, itemId)` calculates `now - collectedTimestamp` and compares against the threshold
- **Threshold**: `GetRespawnDaysForItem(itemId)` × 86,400 seconds (1 in-game day = 86,400 seconds in game time)
- **Real-time equivalent**: 1 in-game day ≈ 24 real minutes at default game speed. At 3-day default: ~72 real minutes

### Per-Category Timers

Each of the 12 item categories can have its own respawn timer, overriding the global `LootRespawnDays`:

| Setting | Category | Items Include |
|---|---|---|
| `LR_MeleeDays` | Melee Weapons | Modern Axe, Katana, Chainsaw, Machete |
| `LR_RangedDays` | Ranged Weapons | Pistol, Shotgun, Crossbow, Slingshot |
| `LR_WeaponModsDays` | Weapon Mods | Silencer, Laser Sight, Scope, Rails |
| `LR_MaterialsDays` | Crafting Materials | Rope, Duct Tape, Cloth, Coins, Solar Panel, Radio |
| `LR_FoodDays` | Food | MREs, Ramen, Canned Food, Cat Food |
| `LR_MedsDays` | Medicine & Energy | Pills, Energy Bar, Energy Drink, GPS Locator (529) |
| `LR_PlantsDays` | Plants | Aloe Vera, Mushrooms, Berries, Chicory |
| `LR_AmmoDays` | Ammunition | Bullets, Arrows, Bolts, Zipline Rope |
| `LR_ThrowablesDays` | Throwables | Grenades, Sticky Bombs, Golf Balls, Flares |
| `LR_ExpendablesDays` | Expendables | Printer Resin, Air Tanks, Loot Pouch |
| `LR_BreakablesDays` | Breakable Containers | Wooden Crates, Coffins (pseudo-ID 9999) |
| `LR_OpenablesDays` | Openable Containers | Ammo Cases, Pelican Cases, Suitcases (pseudo-ID 9998) |

**Values**: `-1` = use global `LootRespawnDays` (default), `0` = instant respawn, `1–100` = category-specific days.

**Implementation**: `RespawnConfig.GetRespawnDaysForItem(int itemId)` looks up the category for the item ID (including pseudo-IDs 9999/9998 for containers) and returns the category-specific timer or the global fallback. The `LootTypeOverride` toggle must be enabled for per-category overrides to take effect.

---

## 6. Item Filtering

### Filter Priority (ShouldTrackItem)

Items pass through this priority chain to determine if they should be tracked:

1. **Building Materials** → always `false` (logs, planks, stones — never tracked, handled by BuilderStacks)
2. **Blacklist** → if item ID is in `LR_ItemBlacklist` → `false` (overrides everything)
3. **Whitelist** → if item ID is in `LR_ItemWhitelist` → `true` (overrides category toggles)
4. **Category Toggles** → per-category on/off (`TrackMelee`, `TrackAmmo`, etc.)
5. **Unknown Items** → `true` (tracked by default — items not in any known category)

### Clone Filtering

Items spawned from containers have `(Clone)` in their name. These are **always skipped** — containers are tracked via their own hooks (`BreakableObject.OnBreak` / `ContainerItemSpawner.OpenContainer`), not via the individual items inside them.

### Per-Item Overrides

| Config Entry | Purpose | Example |
|---|---|---|
| `LR_ItemBlacklist` | Items to **NEVER** track (always available) | `340,356` = Axe & Machete always present |
| `LR_ItemWhitelist` | Items to **ALWAYS** track (even if category off) | `437,441` = Meds & Energy Bar always tracked |

---

## 7. Persistence

| Detail | Value |
|---|---|
| **File** | `loot_collected.dat` (binary format) |
| **Location** | `UserData/` via `LoaderEnvironment.UserDataDirectory` |
| **Save Trigger** | `_dirty` flag + 5-second cooldown timer |
| **Save Driver** | `ServerTickPostfix` on `SeasonsManager.LateUpdate` (Harmony Postfix) |
| **Dedicated Server** | Saved in server's `UserData/` — accessible via SFTP |
| **Format** | Binary: count + (hash string + timestamp long + itemId int) per entry |

The persistence file survives game save corruption since it's independent of the game's own save system.

---

## 8. Harmony Patches

All patches use **manual Harmony patching** (`HarmonyPatchAll = false`) for IL2CPP vtable safety.

| Target Method | Patch Type | Purpose |
|---|---|---|
| `PickUp.Awake` | Postfix | Queue/check/suppress ground pickups on spawn |
| `PickUp.Collect` | Postfix | Record pickup collection with timestamp |
| `BreakableObject.Awake` | Postfix | Queue/check breakable containers on spawn |
| `BreakableObject.OnBreak` | PREFIX + Postfix | PREFIX blocks streaming replays; Postfix records breaks |
| `ContainerItemSpawner.OpenContainer` | PREFIX + Postfix | PREFIX blocks interaction while timer active; Postfix records opens |
| `ContainerItemSpawner.Start` | Postfix | Proactive visual state — shows suppressed containers as already-opened |
| `SeasonsManager.LateUpdate` | Postfix | Server tick driver for save persistence |

---

## 9. Admin Controls

### GUI

- **Loot Respawn toggle**: Enable/disable the entire system
- **Respawn Days**: Text input + "Set" button in Server Admin panel (not a slider — slider spammed `ServerCmd` on every drag tick)
- **Reset Loot**: Clears all tracked items, forcing everything to respawn

### Chat Commands

- `/px loot status` — Show tracked count, suppressed count, respawned count
- `/px config set LootRespawnDays {value}` — Set global respawn timer
- `/px config get LootRespawnDays` — Read current value

---

## 10. Comparison: Authority Model by Hook

| Hook | Local Play | Dedicated Server (Server) | Dedicated Server (Client) |
|---|---|---|---|
| `PickUp.Awake` | Check `_collected`, suppress/respawn | Same | Suppress using server-synced `_collected` (after 0x20) |
| `PickUp.Collect` | Record directly | Record when receiving 0x01 from client | Send 0x01 to server |
| `Container.Start` | Visual state if tracked | Same | Visual state using synced data |
| `Container.Open` | PREFIX blocks/allows | PREFIX blocks/allows (authoritative) | Receives world state from engine |
| `Container.Break` | Frame-based blocking + record | Same (fires server-side via Harmony) | Send 0x02 to server |

---

## 11. Known Behaviours & Edge Cases

| Behaviour | Explanation |
|---|---|
| **Items respawn on area reload, not instantly** | The game streams world chunks. Items respawn when their chunk reloads (player leaves and returns) OR via `RestorePickup` mid-session |
| **Containers appear opened while suppressed** | By design — the `Start` hook applies visual opened state so players understand why a container is empty |
| **Clone items are never tracked** | Items spawned from containers (name contains `(Clone)`) are ephemeral and excluded |
| **Breakable containers spawn fresh loot** | `ClearContainerSaveState` wipes the child spawner's save data so a new `contentsSeed` generates random loot on respawn |
| **Giant icon bug (fixed)** | Calling `RestorePickup` on fresh game instances caused icons to scale up. Fixed with `localScale == Vector3.zero` guard |
| **Negative elapsed times** | Can occur if items were collected during accelerated game time (5x speed) and timestamps ended up in the "future" relative to normal game clock. These items will never expire until the game time catches up |
| **GPS Locator in Meds** | ItemId 529 is classified in the Meds category for timer purposes (uses per-category override, not global fallback) |

---

## 12. File Reference

| File | Size | Purpose |
|---|---|---|
| `LootRespawnModule.cs` | ~75KB | Core module — all hooks, suppression, restoration, timer logic, sync |
| `RespawnConfig.cs` | ~10KB | Item ID lists, category toggles, filter logic, timer overrides |
| `LootSyncEvent.cs` | ~10KB | Network protocol for dedicated server sync |
| `LootEventListener.cs` | ~3KB | GlobalEventListener for debugCommand fallback path |
