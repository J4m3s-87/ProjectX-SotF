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
        if (HasEnoughTimePassed(data.Timestamp))
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
- **Timer**: `RespawnDays × 1440` seconds (1 in-game day ≈ 1440 seconds)

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
