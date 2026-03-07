using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading; // Added by user instruction
using HarmonyLib;
using RedLoader;
using Sons.Environment;
using Sons.Gameplay;
using Sons.Gameplay.GrabBag; // Added by user instruction
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Loot Respawn module — tracks collected loot and prevents it from respawning
    /// until enough in-game time has passed.
    ///
    /// Architecture (ported from GlaDOS's LootRespawnControl v2):
    /// - Each world PickUp / BreakableObject gets a stable MD5 hash from
    ///   position + rotation + name prefix
    /// - When collected, the item's hash + game timestamp + item ID are recorded
    /// - On load, items whose hash is in the collected set are destroyed (suppressed)
    ///   UNLESS enough game-time has elapsed → then the entry is removed (respawned)
    /// - Deferred checking: items that Awake before save data loads are queued
    ///   and re-checked once save data is available
    ///
    /// Harmony patches (all Postfix, IL2CPP-safe):
    /// - PickUp.Awake           → queue/check pickup
    /// - PickUp.Collect         → record collection
    /// - BreakableObject.Awake  → queue/check container
    /// - BreakableObject.OnBreak → record container break
    /// </summary>
    public static class LootRespawnModule
    {
        // ── State ────────────────────────────────────────────────────

        private static bool _initialized;
        private static HarmonyLib.Harmony _harmony;

        /// <summary>Hash → LootData for collected items</summary>
        private static readonly Dictionary<string, LootData> _collected = new();

        /// <summary>Deferred check queues — populated before save data loads</summary>
        private static readonly List<PickUp> _pendingPickups = new();
        private static readonly List<BreakableObject> _pendingContainers = new();
        private static bool _saveDataLoaded;

        /// <summary>Cache: instance ID → MD5 hash (so Collect can look up the hash)</summary>
        private static readonly Dictionary<int, string> _hashCache = new();

        /// <summary>
        /// Hashes of items we just respawned this load.
        /// Prevents the game's save system from re-recording them via OnBreak
        /// (the game fires OnBreak during load to restore opened container state).
        /// </summary>
        private static readonly HashSet<string> _recentlyRespawned = new();

        /// <summary>
        /// Tracks the Unity frame number when each breakable's Awake fired.
        /// Used by OnBreak PREFIX to distinguish streaming replays (same/next frame)
        /// from player interactions (many frames later).
        /// </summary>
        private static readonly Dictionary<string, int> _breakableLoadFrame = new();

        // Persistence
        private static string _saveFilePath;
        private static float _lastSaveTime;
        private const float SaveCooldown = 5f;
        private static bool _dirty;
        private static System.Reflection.MethodInfo _clearStateSyncMethod;
        private static System.Reflection.MethodInfo _setIsOpenMethod;
        private static System.Reflection.MethodInfo _toggleIconMethod;

        // Diagnostics
        private static int _suppressedCount;
        private static int _respawnedCount;
        private static int _breakableAwakeCount;
        private static int _pickupAwakeCount;

        // ── Public API ───────────────────────────────────────────────

        public static bool Enabled
        {
            get => RespawnConfig.Enabled;
            set => RespawnConfig.Enabled = value;
        }

        public static int TrackedCount => _collected.Count;

        // ── Init ─────────────────────────────────────────────────────

        public static void Init()
        {
            if (_initialized) return;

            try
            {
                // Save file path
                string userDataDir = Path.Combine(
                    Path.GetDirectoryName(typeof(LootRespawnModule).Assembly.Location) ?? "",
                    "..", "..", "UserData");
                Directory.CreateDirectory(userDataDir);
                _saveFilePath = Path.Combine(userDataDir, "loot_collected.dat");

                // Migrate old save file
                string oldPath = Path.Combine(
                    Path.GetDirectoryName(typeof(LootRespawnModule).Assembly.Location) ?? "",
                    "loot_collected.dat");
                if (File.Exists(oldPath) && !File.Exists(_saveFilePath))
                {
                    try { File.Move(oldPath, _saveFilePath); }
                    catch { /* non-critical */ }
                }

                // Start with save data NOT loaded — items will queue until OnGameStarted
                _saveDataLoaded = false;

                // Load persisted data
                LoadFromDisk();

                // Apply Harmony patches
                _harmony = new HarmonyLib.Harmony("ProjectX.LootRespawn");

                try { _harmony.PatchAll(typeof(PickUpAwakePatch)); RLog.Msg("[LootRespawn] Harmony POSTFIX on PickUp.Awake — PATCHED OK"); }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] PickUp.Awake patch FAILED: {ex.Message}"); }

                try { _harmony.PatchAll(typeof(PickUpCollectPatch)); RLog.Msg("[LootRespawn] Harmony POSTFIX on PickUp.Collect — PATCHED OK"); }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] PickUp.Collect patch FAILED: {ex.Message}"); }

                try { _harmony.PatchAll(typeof(BreakableObjectAwakePatch)); RLog.Msg("[LootRespawn] Harmony POSTFIX on BreakableObject.Awake — PATCHED OK"); }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] BreakableObject.Awake patch FAILED: {ex.Message}"); }

                try { _harmony.PatchAll(typeof(BreakableObjectOnBreakPatch)); RLog.Msg("[LootRespawn] Harmony POSTFIX on BreakableObject.OnBreak — PATCHED OK"); }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] BreakableObject.OnBreak patch FAILED: {ex.Message}"); }

                // Manual patch for OpenContainer — search ALL types in Sons assembly
                try
                {
                    System.Reflection.MethodInfo openMethod = null;
                    
                    // First try GrabBagController directly
                    openMethod = AccessTools.Method(typeof(GrabBagController), "OpenContainer");
                    
                    if (openMethod == null)
                    {
                        // Search ALL types in the Sons assembly for OpenContainer
                        var asm = typeof(GrabBagController).Assembly;
                        foreach (var t in asm.GetTypes())
                        {
                            try
                            {
                                var m = AccessTools.Method(t, "OpenContainer");
                                if (m != null)
                                {
                                    RLog.Msg($"[LootRespawn] Found OpenContainer on {t.FullName} — params: {string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))}");
                                    openMethod = m;
                                    break;
                                }
                            }
                            catch { /* skip types that fail reflection */ }
                        }
                    }
                    
                    if (openMethod != null)
                    {
                        var prefix = new HarmonyMethod(AccessTools.Method(typeof(LootRespawnModule), nameof(OnOpenContainerPrefix)));
                        var postfix = new HarmonyMethod(AccessTools.Method(typeof(LootRespawnModule), nameof(OnGrabBagOpenedPostfix)));
                        _harmony.Patch(openMethod, prefix: prefix, postfix: postfix);
                        RLog.Msg($"[LootRespawn] Harmony PREFIX+POSTFIX on {openMethod.DeclaringType.FullName}.OpenContainer — PATCHED OK ✅");

                        // Cache methods for container state reset
                        var containerType = openMethod.DeclaringType;
                        _clearStateSyncMethod = AccessTools.Method(containerType, "ClearStateSync");
                        _setIsOpenMethod = AccessTools.Method(containerType, "set__isOpen");
                        _toggleIconMethod = AccessTools.Method(containerType, "ToggleIcon");
                        RLog.Msg($"[LootRespawn] Reset methods: ClearStateSync={(_clearStateSyncMethod != null ? "✅" : "❌")} set_isOpen={(_setIsOpenMethod != null ? "✅" : "❌")} ToggleIcon={(_toggleIconMethod != null ? "✅" : "❌")}");
                    }
                    else
                    {
                        RLog.Warning("[LootRespawn] OpenContainer method not found on ANY type in Sons assembly");
                    }
                }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] OpenContainer manual patch FAILED: {ex.Message}"); }

                _initialized = true;
                RLog.Msg($"[LootRespawn] ★ Initialized — {_collected.Count} tracked items loaded, days={RespawnConfig.RespawnDays}");
            }
            catch (Exception ex)
            {
                RLog.Error($"[LootRespawn] Failed to initialize: {ex.Message}");
            }
        }

        /// <summary>Called each frame — flushes dirty data to disk</summary>
        public static void OnUpdate()
        {
            if (_dirty && Time.time - _lastSaveTime >= SaveCooldown)
            {
                SaveToDisk();
            }
        }

        // ── Reset ────────────────────────────────────────────────────

        public static void Reset()
        {
            int count = _collected.Count;
            _collected.Clear();
            _dirty = true;
            SaveToDisk();
            _suppressedCount = 0;
            _respawnedCount = 0;
            RLog.Msg($"[LootRespawn] ★ Tracker reset — cleared {count} items, all loot will respawn on next load");
        }

        // ── Game Lifecycle ───────────────────────────────────────────

        /// <summary>
        /// Called BEFORE each game load to reset deferred-check state.
        /// This ensures items Awake'd during scene init get queued.
        /// </summary>
        public static void OnSceneInit()
        {
            _saveDataLoaded = false;
            _pendingPickups.Clear();
            _pendingContainers.Clear();
            _hashCache.Clear();
            _recentlyRespawned.Clear();
            _suppressedCount = 0;
            _respawnedCount = 0;
            _breakableAwakeCount = 0;
            _pickupAwakeCount = 0;
            RLog.Msg($"[LootRespawn] Scene init — deferred check armed, {_collected.Count} items in tracker");
        }

        /// <summary>
        /// Called when the game starts (after save data finishes loading).
        /// Processes any items that were queued during Awake before the save
        /// data was available.
        /// </summary>
        public static void OnGameStarted()
        {
            _saveDataLoaded = true;
            ProcessPendingItems();
        }

        /// <summary>
        /// Called when the world is exited — clears runtime state.
        /// </summary>
        public static void OnWorldExited()
        {
            _saveDataLoaded = false;
            _pendingPickups.Clear();
            _pendingContainers.Clear();
            _hashCache.Clear();
            _suppressedCount = 0;
            _respawnedCount = 0;
        }

        // ── Deferred Processing ──────────────────────────────────────

        private static void ProcessPendingItems()
        {
            int suppressed = 0;
            int respawned = 0;

            // Process pending pickups
            foreach (var pickup in _pendingPickups)
            {
                if (pickup == null) continue;
                try
                {
                    string hash = GetOrGenerateHash(pickup.transform, pickup.GetInstanceID());
                    if (hash == null) continue;

                    if (_collected.TryGetValue(hash, out var data))
                    {
                        // Category disabled — release item
                        if (!RespawnConfig.ShouldTrackItem(data.ItemId))
                        {
                            _collected.Remove(hash);
                            _dirty = true;
                            continue;
                        }

                        if (HasEnoughTimePassed(data.Timestamp))
                        {
                            // DO NOT remove from _collected — PREFIX needs entry for future loads
                            _recentlyRespawned.Add(hash);
                            respawned++;
                        }
                        else
                        {
                            UnityEngine.Object.Destroy(pickup.gameObject);
                            suppressed++;
                        }
                    }
                }
                catch { /* item may have been destroyed during load */ }
            }

            // Process pending containers
            foreach (var container in _pendingContainers)
            {
                if (container == null) continue;
                try
                {
                    string hash = GetOrGenerateHash(container.transform, container.GetInstanceID());
                    if (hash == null) continue;

                    if (_collected.TryGetValue(hash, out var data))
                    {
                        if (HasEnoughTimePassed(data.Timestamp))
                        {
                            // DO NOT remove from _collected — PREFIX needs entry for future loads
                            _recentlyRespawned.Add(hash);
                            _breakableLoadFrame[hash] = Time.frameCount;
                            respawned++;
                        }
                        else
                        {
                            UnityEngine.Object.Destroy(container.gameObject);
                            suppressed++;
                        }
                    }
                }
                catch { /* container may have been destroyed during load */ }
            }

            // Process openable containers (tracked via name: prefix by ContainerItemSpawner)
            // These don't have game objects queued — we just check timers in _collected
            var nameKeysToRelease = new List<string>();  // category disabled — remove entirely
            var nameKeysToRespawn = new List<string>();  // timer expired — add to recentlyRespawned
            foreach (var kvp in _collected)
            {
                if (kvp.Key.StartsWith("name:"))
                {
                    // Category disabled — release entry
                    if (!RespawnConfig.ShouldTrackItem(kvp.Value.ItemId))
                    {
                        nameKeysToRelease.Add(kvp.Key);
                        _dirty = true;
                        continue;
                    }

                    if (HasEnoughTimePassed(kvp.Value.Timestamp))
                    {
                        nameKeysToRespawn.Add(kvp.Key);
                        respawned++;
                    }
                    // If timer hasn't passed, the container stays in _collected
                    // and the idempotent check in OnGrabBagOpenedPostfix will
                    // prevent re-recording when the game opens it during load
                }
            }
            // Category disabled — fully remove from _collected
            foreach (var key in nameKeysToRelease)
            {
                _collected.Remove(key);
                string containerName = key.Substring(5);
                RLog.Msg($"[LootRespawn] Released container (category disabled): {containerName}");
            }
            // Timer expired — keep in _collected, add to _recentlyRespawned
            foreach (var key in nameKeysToRespawn)
            {
                // DO NOT remove from _collected — the PREFIX needs the entry to block
                // on future loads. Just add to _recentlyRespawned so PREFIX blocks streaming.
                _recentlyRespawned.Add(key);
                string containerName = key.Substring(5);
                RLog.Msg($"[LootRespawn] Respawned container: {containerName}");
            }

            RLog.Msg($"[LootRespawn] ★ Deferred check — queuedPickups={_pendingPickups.Count}, queuedContainers={_pendingContainers.Count}, suppressed={suppressed}, respawned={respawned}, tracked={_collected.Count}, breakableAwakes={_breakableAwakeCount}, pickupAwakes={_pickupAwakeCount}");

            _pendingPickups.Clear();
            _pendingContainers.Clear();

            _suppressedCount += suppressed;
            _respawnedCount += respawned;

            if (_dirty) SaveToDisk();
        }

        // ── PickUp Callbacks ─────────────────────────────────────────

        /// <summary>
        /// Called when a PickUp spawns. Generates hash and either checks
        /// immediately (if save is loaded) or queues for deferred check.
        /// Skips Clone items (container-spawned) — those are ephemeral.
        /// </summary>
        internal static void OnPickUpAwake(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;
            if (IsMultiplayerClient()) return;

            try
            {
                // Skip clone items — they are spawned from containers and have unstable positions
                string objName = pickup.name;
                if (!string.IsNullOrEmpty(objName) && objName.Contains("(Clone)")) return;

                _pickupAwakeCount++;
                // Log first 5 non-clone pickups for diagnostics
                if (_pickupAwakeCount <= 5)
                    RLog.Msg($"[LootRespawn] PickUp.Awake #{_pickupAwakeCount}: {objName} (saveLoaded={_saveDataLoaded})");

                string hash = GetOrGenerateHash(pickup.transform, pickup.GetInstanceID());
                if (hash == null) return;

                if (!_saveDataLoaded)
                {
                    _pendingPickups.Add(pickup);
                    return;
                }

                // Immediate check (items that Awake after save data is loaded)
                if (_collected.TryGetValue(hash, out var data))
                {
                    // Category disabled — release item immediately
                    if (!RespawnConfig.ShouldTrackItem(data.ItemId))
                    {
                        _collected.Remove(hash);
                        _dirty = true;
                        return;
                    }

                    if (HasEnoughTimePassed(data.Timestamp))
                    {
                        _collected.Remove(hash);
                        _recentlyRespawned.Add(hash);
                        _dirty = true;
                        _respawnedCount++;
                        RLog.Msg($"[LootRespawn] Respawned pickup: {objName} (hash={hash.Substring(0, 8)}…)");
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(pickup.gameObject);
                        _suppressedCount++;
                        if (_suppressedCount <= 50)
                            RLog.Msg($"[LootRespawn] Suppressed pickup: {objName} (hash={hash.Substring(0, 8)}…)");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] PickUp Awake error: {ex.Message}");
            }
        }

        /// <summary>
        /// Called AFTER a pickup is collected. Records the collection timestamp.
        /// </summary>
        internal static void OnPickUpCollected(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;
            if (IsMultiplayerClient()) return;

            try
            {
                // Skip clone items — container-spawned items have unstable positions.
                // Containers are tracked via BreakableObject.OnBreak instead.
                string objName = pickup.name;
                if (!string.IsNullOrEmpty(objName) && objName.Contains("(Clone)")) return;

                int itemId = pickup._itemId;

                // Check per-category filter
                if (!RespawnConfig.ShouldTrackItem(itemId)) return;

                // Retrieve cached hash (generated during Awake)
                int instanceId = pickup.GetInstanceID();
                if (!_hashCache.TryGetValue(instanceId, out string hash))
                {
                    // Fallback: generate now (item may have been pooled)
                    hash = GenerateLootHash(pickup.transform.position, pickup.transform.rotation, pickup.transform.name);
                }

                if (string.IsNullOrEmpty(hash)) return;

                long timestamp = GetGameTimestamp();
                _collected[hash] = new LootData(hash, timestamp, itemId);
                _dirty = true;

                RLog.Msg($"[LootRespawn] Collected pickup: {objName} (itemId={itemId}, hash={hash.Substring(0, 8)}…, ts={timestamp})");
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Collect error: {ex.Message}");
            }
        }

        // ── BreakableObject (Container) Callbacks ────────────────────

        /// <summary>
        /// Called when a BreakableObject spawns. Queue or check.
        /// </summary>
        internal static void OnContainerAwake(BreakableObject container)
        {
            if (container == null || !RespawnConfig.Enabled) return;
            if (IsMultiplayerClient()) return;
            if (!RespawnConfig.TrackBreakables) return;

            try
            {
                string objName = container.name;
                if (string.IsNullOrEmpty(objName)) return;
                if (objName.Contains("BreakableSticksInteraction")) return;

                _breakableAwakeCount++;
                // Log first 10 breakable objects for diagnostics
                if (_breakableAwakeCount <= 10)
                    RLog.Msg($"[LootRespawn] BreakableObject.Awake #{_breakableAwakeCount}: {objName} (saveLoaded={_saveDataLoaded})");

                string hash = GetOrGenerateHash(container.transform, container.GetInstanceID());
                if (hash == null) return;

                if (!_saveDataLoaded)
                {
                    _pendingContainers.Add(container);
                    return;
                }

                // Immediate check
                if (_collected.TryGetValue(hash, out var data))
                {
                    if (HasEnoughTimePassed(data.Timestamp))
                    {
                        // DO NOT remove from _collected — PREFIX needs entry for future loads
                        _recentlyRespawned.Add(hash);
                        _breakableLoadFrame[hash] = Time.frameCount;
                        _respawnedCount++;
                        RLog.Msg($"[LootRespawn] Respawned container: {objName} (frame={Time.frameCount})");
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(container.gameObject);
                        _suppressedCount++;
                        if (_suppressedCount <= 50)
                            RLog.Msg($"[LootRespawn] Suppressed container: {objName}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Container Awake error: {ex.Message}");
            }
        }

        /// <summary>
        /// PREFIX for BreakableObject.OnBreak.
        /// Blocks the game from replaying break events during streaming/loading
        /// for containers whose respawn timer has expired.
        /// </summary>
        internal static bool OnContainerBreakPrefix(BreakableObject container)
        {
            try
            {
                if (container == null || !RespawnConfig.Enabled) return true;
                if (IsMultiplayerClient()) return true;
                if (!RespawnConfig.TrackBreakables) return true;

                string objName = container.name;
                if (string.IsNullOrEmpty(objName)) return true;
                if (objName.Contains("Clone")) return true;
                if (objName.Contains("BreakableSticksInteraction")) return true;

                int instanceId = container.GetInstanceID();
                string hash;
                if (!_hashCache.TryGetValue(instanceId, out hash))
                {
                    hash = GenerateLootHash(container.transform.position, container.transform.rotation, container.transform.name);
                    if (!string.IsNullOrEmpty(hash))
                        _hashCache[instanceId] = hash;
                }

                if (string.IsNullOrEmpty(hash)) return true;

                // FRAME-BASED BLOCKING: Only block OnBreak if it fires within 120 frames
                // (~2 seconds at 60fps) of the BreakableObject.Awake call.
                // This distinguishes streaming replays (same/next frame) from
                // player interactions (many frames later).
                if (_breakableLoadFrame.TryGetValue(hash, out int loadFrame))
                {
                    int frameDelta = Time.frameCount - loadFrame;
                    if (frameDelta <= 120)
                    {
                        ClearContainerSaveState(container, objName);
                        RLog.Msg($"[LootRespawn] ★ BLOCKED OnBreak (streaming replay, Δframe={frameDelta}): {objName}");
                        return false;
                    }
                    // Many frames later → player interaction → allow through
                    // Remove from tracking so we don't check again
                    _breakableLoadFrame.Remove(hash);
                }
                // FALLBACK: First OnBreak fires BEFORE Awake records the frame.
                // Check if the entry is in _collected with expired timer.
                // If so, this is the first streaming replay — block it and record frame.
                else if (_collected.TryGetValue(hash, out var data) && HasEnoughTimePassed(data.Timestamp))
                {
                    _breakableLoadFrame[hash] = Time.frameCount;
                    _recentlyRespawned.Add(hash);
                    ClearContainerSaveState(container, objName);
                    RLog.Msg($"[LootRespawn] ★ BLOCKED OnBreak (expired timer, first replay): {objName}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] OnBreak prefix error: {ex.Message}");
            }
            return true;
        }

        /// <summary>
        /// Clears the save state of a BreakableObject's child ContainerItemSpawner.
        /// Called when blocking OnBreak replays so the game forgets the container
        /// was opened and will spawn fresh items when the player breaks it later.
        /// </summary>
        private static void ClearContainerSaveState(BreakableObject container, string objName)
        {
            try
            {
                var spawner = container.GetComponentInChildren<Sons.Gameplay.ContainerItemSpawner>();
                if (spawner == null) return;

                if (_clearStateSyncMethod != null)
                    _clearStateSyncMethod.Invoke(spawner, null);

                if (_setIsOpenMethod != null)
                    _setIsOpenMethod.Invoke(spawner, new object[] { false });

                RLog.Msg($"[LootRespawn] ★ Cleared save state for container spawner on: {objName}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] ClearContainerSaveState failed on {objName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when a BreakableObject is broken/opened.
        /// Records the container as collected.
        /// </summary>
        internal static void OnContainerBroken(BreakableObject container)
        {
            // DIAGNOSTIC: Log immediately before ANY filter checks
            RLog.Msg($"[LootRespawn] ★ OnBreak FIRED: {container?.name ?? "null"} (enabled={RespawnConfig.Enabled}, trackBreak={RespawnConfig.TrackBreakables})");

            if (container == null || !RespawnConfig.Enabled) return;
            if (IsMultiplayerClient()) return;
            if (!RespawnConfig.TrackBreakables) return;

            try
            {
                string objName = container.name;
                if (string.IsNullOrEmpty(objName)) return;
                if (objName.Contains("Clone")) return;
                if (objName.Contains("BreakableSticksInteraction")) return;

                // Check breakable blacklist
                var brokenPrefab = container._brokenPrefab;
                if (brokenPrefab != null)
                {
                    var innerPickup = brokenPrefab.transform.GetComponent<PickUp>();
                    if (innerPickup != null && RespawnConfig.BreakableBlacklist.Contains(innerPickup._itemId))
                        return;
                }

                int instanceId = container.GetInstanceID();
                if (!_hashCache.TryGetValue(instanceId, out string hash))
                {
                    hash = GenerateLootHash(container.transform.position, container.transform.rotation, container.transform.name);
                }

                if (string.IsNullOrEmpty(hash)) return;

                // Skip if this container was just respawned — the game fires OnBreak
                // during loading to restore the opened state of previously-broken containers
                if (_recentlyRespawned.Contains(hash))
                {
                    RLog.Msg($"[LootRespawn] OnBreak SKIPPED (recently respawned): {objName}");
                    return;
                }

                // IDEMPOTENT: Skip if already tracked — prevents WoodenCrateItems
                // from resetting their timestamp every time their area streams in
                if (_collected.ContainsKey(hash))
                {
                    return; // already tracked, don't update timestamp
                }

                long timestamp = GetGameTimestamp();
                _collected[hash] = new LootData(hash, timestamp, RespawnConfig.BreakableId);
                _dirty = true;

                RLog.Msg($"[LootRespawn] Container broken: {objName} (hash={hash.Substring(0, 8)}…, ts={timestamp}, tracked={_collected.Count})");
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Container break error: {ex.Message}");
            }
        }

        // ── GrabBag (Openable Container) Callbacks ───────────────────

        /// <summary>
        /// Called when a GrabBagController.OpenContainer fires (suitcases, pelican cases, etc.).
        /// Uses the containerId as a stable unique key (each openable container in the world has one).
        /// </summary>
        internal static void OnGrabBagOpened(GrabBagController controller, bool open, int containerId)
        {
            // DIAGNOSTIC: always log
            RLog.Msg($"[LootRespawn] ★ GrabBag.OpenContainer: open={open}, containerId={containerId}, obj={controller?.name ?? "null"}");

            if (!open) return; // Only track opening, not closing
            if (controller == null || !RespawnConfig.Enabled) return;
            if (IsMultiplayerClient()) return;
            if (!RespawnConfig.TrackOpenables) return;

            try
            {
                // Use a stable key based on containerId (unique per openable container in the world)
                string hash = $"grab:{containerId}";

                // Skip if recently respawned
                if (_recentlyRespawned.Contains(hash)) 
                {
                    RLog.Msg($"[LootRespawn] GrabBag open SKIPPED (recently respawned): containerId={containerId}");
                    return;
                }

                long timestamp = GetGameTimestamp();
                _collected[hash] = new LootData(hash, timestamp, RespawnConfig.OpenableId);
                _dirty = true;

                RLog.Msg($"[LootRespawn] GrabBag opened: {controller.name} (containerId={containerId}, ts={timestamp}, tracked={_collected.Count})");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] GrabBag open error: {ex.Message}");
            }
        }

        /// <summary>
        /// Harmony PREFIX for ContainerItemSpawner.OpenContainer.
        /// Blocks the game from replaying the "opened" state during deserialization/streaming
        /// when the container's respawn timer has expired.
        /// IMPORTANT: Does NOT remove from _collected — entry persists so PREFIX
        /// can block on every subsequent load until the player re-opens the container.
        /// </summary>
        internal static bool OnOpenContainerPrefix(object __instance, bool spawnItems, int contentsSeed)
        {
            try
            {
                if (spawnItems) return true; // let content-spawn calls through
                if (!RespawnConfig.Enabled) return true;
                if (IsMultiplayerClient()) return true;
                if (!RespawnConfig.TrackOpenables) return true; // category disabled — let through

                string objName = "unknown";
                if (__instance is UnityEngine.Component comp)
                    objName = comp.name ?? "null";

                string hash = $"name:{objName}";

                // If already respawned this session, block streaming replays too
                if (_recentlyRespawned.Contains(hash))
                {
                    RLog.Msg($"[LootRespawn] ★ BLOCKED OpenContainer (recently respawned): {objName}");
                    return false;
                }

                // Check if this container is tracked and timer has expired
                if (_collected.TryGetValue(hash, out var data) && HasEnoughTimePassed(data.Timestamp))
                {
                    // Timer expired — block the open call so the container stays closed/full
                    // DO NOT remove from _collected — keeps the entry so we block on future loads
                    _recentlyRespawned.Add(hash);
                    RLog.Msg($"[LootRespawn] ★ BLOCKED OpenContainer (timer expired, respawning): {objName}");
                    return false; // skip original method
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] OpenContainer prefix error: {ex.Message}");
            }
            return true; // run original method
        }

        /// <summary>
        /// Harmony postfix for ContainerItemSpawner.OpenContainer.
        /// Tracks new container opens during gameplay.
        /// When a player opens a previously-respawned container, removes the old
        /// expired entry and records a fresh timestamp.
        /// </summary>
        internal static void OnGrabBagOpenedPostfix(object __instance, bool spawnItems, int contentsSeed)
        {
            try
            {
                string objName = "unknown";
                if (__instance is UnityEngine.Component comp)
                    objName = comp.name ?? "null";

                // DIAGNOSTIC: always log
                RLog.Msg($"[LootRespawn] ★ ContainerItemSpawner.OpenContainer: spawnItems={spawnItems}, contentsSeed={contentsSeed}, obj={objName}");

                if (!RespawnConfig.Enabled) return;
                if (IsMultiplayerClient()) return;
                if (spawnItems) return; // only track the initial open call

                string hash = $"name:{objName}";

                // Skip during deserialization — don't record replays as new opens
                if (!_saveDataLoaded)
                {
                    RLog.Msg($"[LootRespawn] Container open SKIPPED (save not loaded yet): {objName}");
                    return;
                }

                // If recently respawned, PREFIX already blocked this call.
                // But with IL2CPP, POSTFIX still runs after PREFIX returns false!
                if (_recentlyRespawned.Contains(hash))
                {
                    RLog.Msg($"[LootRespawn] Container open SKIPPED (recently respawned): {objName}");
                    return;
                }

                // Skip WoodenCrateItems — tracked by BreakableObject.OnBreak
                if (objName.Contains("WoodenCrateItems"))
                    return;

                // Already tracked with unexpired timer — don't update timestamp
                if (_collected.ContainsKey(hash))
                    return;

                // NEW OPEN — record it
                long timestamp = GetGameTimestamp();
                _collected[hash] = new LootData(hash, timestamp, RespawnConfig.BreakableId);
                _dirty = true;

                RLog.Msg($"[LootRespawn] Container opened: {objName} (key={hash}, ts={timestamp}, tracked={_collected.Count})");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] Container postfix error: {ex.Message}");
            }
        }

        // ── Hash Generation (MD5, matches original mod) ──────────────

        /// <summary>
        /// Get or generate the MD5 hash for a transform, caching by instance ID.
        /// </summary>
        private static string GetOrGenerateHash(Transform t, int instanceId)
        {
            if (_hashCache.TryGetValue(instanceId, out string cached))
                return cached;

            string hash = GenerateLootHash(t.position, t.rotation, t.name);
            if (!string.IsNullOrEmpty(hash))
                _hashCache[instanceId] = hash;

            return hash;
        }

        /// <summary>
        /// Generate MD5 hash from position + rotation + first 3 chars of name.
        /// This is the exact same algorithm as LootIdentifier.GenerateIdentifier()
        /// in the original LootRespawnControl mod.
        /// </summary>
        private static string GenerateLootHash(Vector3 position, Quaternion rotation, string name)
        {
            try
            {
                byte[] posBytes = new byte[12];
                Buffer.BlockCopy(BitConverter.GetBytes(position.x), 0, posBytes, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(position.y), 0, posBytes, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(position.z), 0, posBytes, 8, 4);

                byte[] rotBytes = new byte[16];
                Buffer.BlockCopy(BitConverter.GetBytes(rotation.x), 0, rotBytes, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(rotation.y), 0, rotBytes, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(rotation.z), 0, rotBytes, 8, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(rotation.w), 0, rotBytes, 12, 4);

                string prefix = "";
                if (!string.IsNullOrEmpty(name))
                    prefix = name.Length >= 3 ? name.Substring(0, 3) : name;
                byte[] nameBytes = Encoding.UTF8.GetBytes(prefix);

                using var md5 = MD5.Create();
                using var stream = new MemoryStream();
                stream.Write(posBytes, 0, posBytes.Length);
                stream.Write(rotBytes, 0, rotBytes.Length);
                stream.Write(nameBytes, 0, nameBytes.Length);

                byte[] hashBytes = md5.ComputeHash(stream.ToArray());
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("x2"));

                return sb.ToString();
            }
            catch
            {
                return null;
            }
        }

        // ── Timestamp / Time ─────────────────────────────────────────

        /// <summary>
        /// Get current game time as total seconds (days * 86400 + time-of-day seconds).
        /// Same algorithm as LootRespawnControl.GetTimestampFromGameTime().
        /// </summary>
        private static long GetGameTimestamp()
        {
            try
            {
                var tod = TimeOfDayHolder.GetTimeOfDay();
                string todStr = tod.ToString();
                // Format: "Day X HH:MM:SS" — parse the same way as the original
                string[] parts = todStr.Split(' ');
                int day = int.Parse(parts[1]);
                TimeSpan time = TimeSpan.Parse(parts[2]);
                return (long)(day * 24 * 60 * 60) + (long)time.TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Check if enough game-time has passed since the item was collected.
        /// </summary>
        private static bool HasEnoughTimePassed(long collectedTimestamp)
        {
            long now = GetGameTimestamp();
            long threshold = (long)RespawnConfig.RespawnDays * 86400L;
            return (now - collectedTimestamp) >= threshold;
        }

        // ── Server Authority ─────────────────────────────────────────

        private static bool IsMultiplayerClient()
        {
            try
            {
                return BoltNetwork.isRunning && BoltNetwork.isClient;
            }
            catch { return false; }
        }

        // ── Persistence ──────────────────────────────────────────────

        private static void SaveToDisk()
        {
            try
            {
                using var writer = new BinaryWriter(File.Open(_saveFilePath, FileMode.Create));
                writer.Write(2); // version 2 — LootData format
                writer.Write(_collected.Count);
                foreach (var kv in _collected)
                {
                    writer.Write(kv.Value.Hash);
                    writer.Write(kv.Value.Timestamp);
                    writer.Write(kv.Value.ItemId);
                }
                _dirty = false;
                _lastSaveTime = Time.time;
                RLog.Msg($"[LootRespawn] Saved {_collected.Count} items to disk");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] SaveToDisk failed: {ex.Message}");
            }
        }

        private static void LoadFromDisk()
        {
            _collected.Clear();
            if (!File.Exists(_saveFilePath)) return;

            try
            {
                using var reader = new BinaryReader(File.Open(_saveFilePath, FileMode.Open));
                int version = reader.ReadInt32();

                if (version == 2)
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string hash = reader.ReadString();
                        long timestamp = reader.ReadInt64();
                        int itemId = reader.ReadInt32();
                        _collected[hash] = new LootData(hash, timestamp, itemId);
                    }
                }
                else if (version == 1)
                {
                    // Legacy format: hash → day (int)
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string key = reader.ReadString();
                        int day = reader.ReadInt32();
                        // Convert old day to approximate timestamp (day * 86400)
                        _collected[key] = new LootData(key, (long)day * 86400L, 0);
                    }
                    RLog.Msg($"[LootRespawn] Migrated {_collected.Count} items from v1 format");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] LoadFromDisk failed: {ex.Message}");
            }
        }

        // ── Status ───────────────────────────────────────────────────

        public static string GetStatus()
        {
            if (!_initialized) return "Respawn: Not initialized";
            long ts = GetGameTimestamp();
            return $"Respawn: {(RespawnConfig.Enabled ? "ON" : "OFF")} | Days: {RespawnConfig.RespawnDays} | Tracked: {_collected.Count} | GameTime: {ts}s | Suppressed: {_suppressedCount} | Respawned: {_respawnedCount}";
        }
    }

    // ── Data Model ───────────────────────────────────────────────────

    /// <summary>
    /// Recorded data for a single collected item.
    /// </summary>
    internal class LootData
    {
        public string Hash;
        public long Timestamp;  // game-time in total seconds
        public int ItemId;      // game item ID (or 9999 for breakable containers)

        public LootData(string hash, long timestamp, int itemId)
        {
            Hash = hash;
            Timestamp = timestamp;
            ItemId = itemId;
        }
    }

    // ── Harmony Patches ──────────────────────────────────────────────

    [HarmonyPatch(typeof(Sons.Gameplay.PickUp), "Awake")]
    internal static class PickUpAwakePatch
    {
        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.PickUp __instance)
        {
            LootRespawnModule.OnPickUpAwake(__instance);
        }
    }

    [HarmonyPatch(typeof(Sons.Gameplay.PickUp), "Collect")]
    internal static class PickUpCollectPatch
    {
        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.PickUp __instance)
        {
            LootRespawnModule.OnPickUpCollected(__instance);
        }
    }

    [HarmonyPatch(typeof(Sons.Gameplay.BreakableObject), "Awake")]
    internal static class BreakableObjectAwakePatch
    {
        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.BreakableObject __instance)
        {
            LootRespawnModule.OnContainerAwake(__instance);
        }
    }

    [HarmonyPatch(typeof(Sons.Gameplay.BreakableObject), "OnBreak")]
    internal static class BreakableObjectOnBreakPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(Sons.Gameplay.BreakableObject __instance)
        {
            return LootRespawnModule.OnContainerBreakPrefix(__instance);
        }

        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.BreakableObject __instance)
        {
            LootRespawnModule.OnContainerBroken(__instance);
        }
    }

    // GrabBag patch is done manually in Init() via AccessTools
}
