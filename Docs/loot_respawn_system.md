# Server-Authoritative Loot Respawn & Suppression

The Loot Respawn system in Project X tracks collected loot and enforces respawn timers. On dedicated servers, the server is **authoritative** — it decides which items exist. Clients receive server state via `LootSyncEvent` and suppress locally.

## 1. Authority Model

| Component           | Dedicated Server                                                                             | Multiplayer Client                                                             |
| :------------------ | :------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------- |
| **PickUp.Awake**    | **Suppressor**: Check `_collected`. If tracked + timer active → `Destroy(gameObject)`.       | **Suppressor**: Same logic using server-synced `_collected` (after 0x20 sync). |
| **PickUp.Collect**  | **Recorder**: Records collection in `_collected`, broadcasts 0x21 to all clients.            | **Reporter**: Sends `LootSyncEvent` 0x01 (hash + itemId) to server.            |
| **Container.Start** | **Visual State**: If tracked + timer active → `set_isOpen(true)` + `ToggleIcon(false)`.      | **Visual State**: Same logic using server-synced data.                         |
| **Container.Open**  | **PREFIX**: Timer active → block (return false). Timer expired → remove from tracker, allow. | **Visual Result**: Receives world-state from game engine.                      |
| **Container.Break** | **PREFIX**: Frame-based blocking for streaming replays. Expired → BLOCK + ClearStateSync.    | **Reporter**: Sends LootSyncEvent 0x02.                                        |

## 2. Server-Side Lifecycle

### 2.1 Suppression (Pickups)

```csharp
internal static void OnPickUpAwake(PickUp pickup)
{
    // Client branch: suppress using server-synced _collected
    if (IsMultiplayerClient())
    {
        if (!_serverSyncReceived) return; // Haven't received 0x20 yet
        string hash = GetOrGenerateHash(pickup.transform, pickup.GetInstanceID());
        if (hash != null && _collected.ContainsKey(hash))
        {
            UnityEngine.Object.Destroy(pickup.gameObject);
            _suppressedCount++;
        }
        return;
    }

    // Server/host branch: check tracker
    string hash2 = GetOrGenerateHash(pickup.transform, pickup.GetInstanceID());
    if (_collected.TryGetValue(hash2, out var data))
    {
        if (HasEnoughTimePassed(data.Timestamp, data.ItemId))
        {
            _collected.Remove(hash2);       // Timer expired → respawn
            _recentlyRespawned.Add(hash2);   // Block re-recording during streaming
        }
        else
        {
            UnityEngine.Object.Destroy(pickup.gameObject);  // Suppress
        }
    }
}
```

### 2.2 Container Respawn (Openable)

The PREFIX on `ContainerItemSpawner.OpenContainer` handles three states:

1. **`_recentlyRespawned` + streaming** (`!_saveDataLoaded`): Block (streaming replay)
2. **`_recentlyRespawned` + player** (`_saveDataLoaded`): Allow through, remove from set
3. **In `_collected` + timer expired**: Remove from tracker, return `true` (re-lootable)
4. **In `_collected` + timer active**: Mark visually opened, return `false` (suppressed)

### 2.3 Game Time

- **Tick Driver**: Harmony Postfix on `SeasonsManager.LateUpdate` — only guaranteed per-frame tick on headless (Pattern #19)
- **Timer**: Per-category `GetRespawnDaysForItem(itemId)` × 86400 seconds. Each category can override the global `RespawnDays` with its own timer (-1 = use global).

## 3. LootSyncEvent Protocol

All sync uses `LootSyncEvent` (`Packets.NetEvent`, scope `ProjectX.LootSync`):

| Msg    | Direction | Purpose                            |
| ------ | --------- | ---------------------------------- |
| `0x01` | C→S       | Pickup collected (hash + itemId)   |
| `0x02` | C→S       | Container broken (hash)            |
| `0x03` | C→S       | Container opened (hash)            |
| `0x10` | C→S       | Request full suppression list      |
| `0x20` | S→C       | Full suppression list (all hashes) |
| `0x21` | S→C       | Incremental suppress (single hash) |

### Client Sync Flow

```
[Client joins] → OnGameStarted → _collected.Clear() → RequestState (0x10)
    → Server sends 0x20 (all hashes) → ApplyServerState()
    → _serverSyncReceived = true → OnPickUpAwake now suppresses ✅

[Any player collects] → LootSyncEvent 0x01 → Server records
    → BroadcastNewSuppression 0x21 → All clients add hash
```

### Connection Tracking

Server tracks clients in `_trackedClients` HashSet (populated on 0x10). Broadcasts iterate this set with dead-connection cleanup. Direct `BoltNetwork.connections` iteration does **not** work in IL2CPP.

## 4. Persistence

| Item         | Details                                               |
| ------------ | ----------------------------------------------------- |
| **File**     | `loot_collected.dat` (binary)                         |
| **Location** | `UserData/` via `LoaderEnvironment.UserDataDirectory` |
| **Save**     | `ServerTickPostfix` every 5s if dirty flag set        |
| **SFTP**     | Visible alongside `ProjectX.Master.cfg`               |

## 5. Lifecycle & Initialization Fixes

On dedicated servers, `OnSdkInitialized` does NOT fire. `Init()` runs inside `OnGameStart()` — AFTER `OnGameStarted()` sets `_saveDataLoaded=true`. Guards in `Init()` and `OnSceneInit()` preserve `_saveDataLoaded` if already true.

## 6. Admin Controls

Respawn Days in Server Admin panel uses text input + "Set" button (not slider — slider spammed `ServerCmd` on every drag tick). Routes via `/px config set LootRespawnDays {value}`.

## 7. Per-Item Whitelist / Blacklist

Two comma-separated config entries allow per-item override of category toggles:

| Setting            | Purpose                                                  | Example   |
| :----------------- | :------------------------------------------------------- | :-------- |
| `LR_ItemBlacklist` | Items to **NEVER** track (always respawn immediately)    | `340,356` |
| `LR_ItemWhitelist` | Items to **ALWAYS** track (even if category is disabled) | `437,441` |

**Filter priority in `ShouldTrackItem()`:**

1. **Building materials** → always `false` (logs, planks, stones — never tracked)
2. **Blacklist** → if item ID is in blacklist, `false` (overrides everything)
3. **Whitelist** → if item ID is in whitelist, `true` (overrides category toggles)
4. **Category toggles** → existing per-category on/off (TrackMelee, TrackAmmo, etc.)
5. **Unknown items** → `true` (tracked by default)

Both entries are visible on **all builds** (solo, owner, server, client) and are pushed to clients via ConfigSync on dedicated servers.

## 8. Per-Category Respawn Timers

Each of the 12 categories can have its own respawn timer (in days), overriding the global `LootRespawnDays`:

| Setting              | Category             | Default |
| :------------------- | :------------------- | :------ |
| `LR_MeleeDays`       | Melee Weapons        | -1      |
| `LR_RangedDays`      | Ranged Weapons       | -1      |
| `LR_WeaponModsDays`  | Weapon Mods          | -1      |
| `LR_MaterialsDays`   | Crafting Materials   | -1      |
| `LR_FoodDays`        | Food                 | -1      |
| `LR_MedsDays`        | Medicine & Energy    | -1      |
| `LR_PlantsDays`      | Plants               | -1      |
| `LR_AmmoDays`        | Ammunition           | -1      |
| `LR_ThrowablesDays`  | Throwables           | -1      |
| `LR_ExpendablesDays` | Expendables          | -1      |
| `LR_BreakablesDays`  | Breakable Containers | -1      |
| `LR_OpenablesDays`   | Openable Containers  | -1      |

**Values**: `-1` = use global `LootRespawnDays`, `0` = instant respawn, `1-100` = category-specific days.

**Implementation**: `RespawnConfig.GetRespawnDaysForItem(int itemId)` looks up the category for the stored item ID (including pseudo-IDs 9999/9998 for containers) and returns the category-specific timer or the global fallback.
