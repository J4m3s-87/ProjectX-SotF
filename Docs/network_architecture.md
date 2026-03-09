# Loot Respawn Module — Network Architecture

## Overview

```mermaid
flowchart TB
    subgraph CLIENT ["🎮 CLIENT (Owner/Client Edition)"]
        PA["PickUp.Awake\n(suppresses if server-synced)"]
        PC["PickUp.Collect"]
        BA["BreakableObject.Awake\n(suppresses if server-synced)"]
        BB["BreakableObject.OnBreak"]
        CO["ContainerItemSpawner.OpenContainer"]
        CS["ContainerItemSpawner.Start"]
        CSYNC["Client joins →\nRequestState (0x10)"]
    end

    subgraph NETWORK ["📡 NETWORK LAYER (LootSyncEvent)"]
        LSE["LootSyncEvent\n(Packets.NetEvent)\nProjectX.LootSync"]
    end

    subgraph SERVER ["🖥️ DEDICATED SERVER"]
        TRACK["_collected Dictionary\n(hash → timestamp + itemId)"]
        SAVE["SaveToDisk()\nloot_collected.dat\n(UserData/ via LoaderEnvironment)"]
        TICK["ServerTickPostfix\n(SeasonsManager.LateUpdate)"]
        BCAST["BroadcastNewSuppression\n(0x21 → all tracked clients)"]
        FULL["SendSuppressionList\n(0x20 → requesting client)"]
    end

    PC -->|"0x01 Collect"| LSE
    BB -->|"0x02 Container Break"| LSE
    CO -->|"0x03 Container Open"| LSE
    LSE -->|"Server records"| TRACK
    TRACK --> BCAST
    BCAST -->|"0x21 to all clients"| PA
    BCAST -->|"0x21 to all clients"| BA

    CSYNC -->|"0x10 Request"| LSE
    LSE -->|"0x20 Full list"| FULL
    FULL -->|"ApplyServerState"| PA

    TRACK --> SAVE
    TICK -->|"Every 5s if dirty"| SAVE

    style LSE fill:#51cf66,color:#fff
    style TRACK fill:#339af0,color:#fff
    style SAVE fill:#339af0,color:#fff
    style BCAST fill:#ff922b,color:#fff
    style FULL fill:#ff922b,color:#fff
```

## LootSyncEvent Protocol

| Msg    | Direction       | Purpose                            |
| ------ | --------------- | ---------------------------------- |
| `0x01` | Client → Server | Pickup collected (hash + itemId)   |
| `0x02` | Client → Server | Container broken (hash)            |
| `0x03` | Client → Server | Container opened (hash)            |
| `0x10` | Client → Server | Request full suppression list      |
| `0x20` | Server → Client | Full suppression list (all hashes) |
| `0x21` | Server → Client | Incremental suppress (single hash) |

## Per-Loot-Type Flow

### Openable Containers (ammo cases, storage crates)

```mermaid
sequenceDiagram
    participant G as Game Engine
    participant S as Server (Start Hook)
    participant P as Server (PREFIX)
    participant T as Tracker (_collected)
    participant D as Disk (loot_collected.dat)

    Note over G,D: === FIRST OPEN (Player loots container) ===
    G->>P: OpenContainer(spawnItems=True)
    P->>P: Not tracked → ALLOW
    P->>G: return true (proceed)
    G->>T: POSTFIX records hash + timestamp
    T->>D: SaveToDisk (via ServerTickPostfix)

    Note over G,D: === SERVER RESTART (timer NOT expired) ===
    D->>T: LoadFromDisk (items loaded)

    Note over G,D: === CONTAINER STREAMS IN ===
    G->>S: ContainerItemSpawner.Start()
    S->>T: Check: hash in _collected?
    T-->>S: YES, timer NOT expired
    S->>G: set__isOpen(true) + ToggleIcon(false)
    Note over G: Container looks OPENED ✅

    Note over G,D: === PLAYER TRIES TO INTERACT ===
    G->>P: OpenContainer(spawnItems=True)
    P->>T: Check: hash in _collected?
    T-->>P: YES, timer NOT expired
    P->>G: return false (SUPPRESSED)
    Note over G: Interaction blocked ✅

    Note over G,D: === SERVER RESTART (timer EXPIRED) ===
    D->>T: LoadFromDisk (items loaded)

    Note over G,D: === CONTAINER STREAMS IN ===
    G->>P: OpenContainer (streaming replay)
    P->>T: Check: hash in _collected?
    T-->>P: YES, timer EXPIRED
    P->>T: Remove from _collected
    P->>G: return true (ALLOW — fresh container)
    Note over G: Container is RE-LOOTABLE ✅
```

### Breakable Containers (coffins, crates)

```mermaid
sequenceDiagram
    participant G as Game Engine
    participant A as Server (Awake)
    participant P as Server (PREFIX)
    participant T as Tracker (_collected)

    Note over G,T: === FIRST BREAK (Player breaks crate) ===
    G->>P: OnBreak fires
    P->>P: Not in _breakableLoadFrame → ALLOW
    G->>T: POSTFIX records hash + timestamp

    Note over G,T: === SERVER RESTART + STREAM IN ===
    G->>A: BreakableObject.Awake
    A->>T: Check: hash in _collected?
    Note over A: Timer NOT expired → Destroy()
    A->>G: Object destroyed (stays intact visually)

    Note over G,T: === TIMER EXPIRES + STREAM IN ===
    G->>A: BreakableObject.Awake
    A->>T: Check: hash in _collected?
    Note over A: Timer EXPIRED → remove + allow
    G->>P: OnBreak (streaming replay)
    P->>P: Expired timer → BLOCK + ClearStateSync
    Note over G: Container respawns FRESH ✅
```

### Ground Pickups (via LootSyncEvent)

```mermaid
sequenceDiagram
    participant C as Client
    participant N as LootSyncEvent (0x01)
    participant S as Server
    participant T as Tracker
    participant B as Broadcast (0x21)

    Note over C,B: === PICKUP COLLECTED ===
    C->>C: PickUp.Collect fires (client-side)
    C->>N: ReportPickupCollected(hash, itemId)
    N->>S: OnRemoteLootCollected(hash, itemId)
    S->>T: Add to _collected
    S->>B: BroadcastNewSuppression(hash)
    B->>C: All clients receive 0x21
    Note over C: AddServerSuppression(hash) ✅
```

## Client Sync Flow

```mermaid
sequenceDiagram
    participant C as Client
    participant N as LootSyncEvent
    participant S as Server
    participant T as Tracker

    Note over C,T: === CLIENT JOINS GAME ===
    C->>C: OnGameStarted()
    C->>C: _collected.Clear() (discard local data)
    C->>N: RequestState (0x10)
    N->>S: ReadMessageServer
    S->>S: _trackedClients.Add(connection)
    S->>T: Get all hashes
    T-->>S: All entries
    S->>N: SendSuppressionList (0x20)
    N->>C: ApplyServerState()
    C->>C: _serverSyncReceived = true
    Note over C: OnPickUpAwake now suppresses ✅

    Note over C,T: === ANY PLAYER COLLECTS ===
    C->>N: ReportPickupCollected (0x01)
    N->>S: RecordPickupFromClient()
    S->>T: Add hash
    S->>N: BroadcastNewSuppression (0x21)
    N->>C: AddServerSuppression(hash)
    Note over C: Future Awakes will suppress ✅
```

## Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| **Server-authoritative** | All tracking stored on server; clients receive via sync |
| **LootSyncEvent** | Dedicated NetEvent replaces debugCommand for reliability |
| **Incremental broadcast (0x21)** | Avoids full-list resend; one hash per collection event |
| **Timer-expired removal** | Entries removed from `_collected` when timer expires — container becomes truly fresh |
| **_recentlyRespawned streaming-only** | Only blocks during save-load phase; players interact normally after |
| **Dual-layer suppression** | `Start` hook (proactive) + `PREFIX` (reactive) for containers |
| **ServerTickPostfix driver** | `SdkEvents.OnInWorldUpdate` dead on headless (Pattern #19) |
| **LoaderEnvironment path** | `../../UserData` relative to DLL breaks on hosted servers |
| **Client clear on join** | `_collected.Clear()` in `OnGameStarted()` discards stale local data |
