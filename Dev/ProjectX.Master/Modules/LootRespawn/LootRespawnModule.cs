using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using RedLoader;
using Sons.Environment;
using Sons.Gameplay;
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Loot Respawn module — prevents collected loot from respawning until enough
    /// in-game days have passed. Based on GLaD0S's LootRespawnControl v2.0 approach.
    ///
    /// Harmony patches:
    /// - Postfix on PickUp.Awake   → suppress item if not yet due for respawn
    /// - Postfix on PickUp.OnEnable → same check for pooled/recycled items
    /// - Prefix  on PickUp.Collect  → record collection day before item is consumed
    ///
    /// Server-authoritative: only tracks loot when running as server or singleplayer.
    /// </summary>
    public static class LootRespawnModule
    {
        private static bool _initialized = false;
        private static HarmonyLib.Harmony _harmony;

        // Collected loot tracking: identifier → game-day it was collected
        private static readonly Dictionary<string, int> _collectedLoot = new();

        // Persistence file path (set during Init)
        private static string _saveFilePath;

        // Throttle saves: don't write to disk more than once every 5 seconds
        private static float _lastSaveTime;
        private const float SaveCooldown = 5f;
        private static bool _dirty;

        // Diagnostic counters
        private static int _suppressedCount;
        private static int _respawnedCount;

        public static bool Enabled
        {
            get => RespawnConfig.Enabled;
            set => RespawnConfig.Enabled = value;
        }

        public static int TrackedCount => _collectedLoot.Count;

        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                // Save file in UserData folder (survives mod updates, appropriate location)
                string userDataDir = Path.Combine(
                    Path.GetDirectoryName(typeof(LootRespawnModule).Assembly.Location) ?? "",
                    "..", "..", "UserData");
                Directory.CreateDirectory(userDataDir);
                _saveFilePath = Path.Combine(userDataDir, "loot_collected.dat");

                // Migrate old save file if it exists next to the DLL
                string oldPath = Path.Combine(
                    Path.GetDirectoryName(typeof(LootRespawnModule).Assembly.Location) ?? "",
                    "loot_collected.dat");
                if (File.Exists(oldPath) && !File.Exists(_saveFilePath))
                {
                    try { File.Move(oldPath, _saveFilePath); }
                    catch { /* non-critical */ }
                }

                // Load persisted data
                LoadFromDisk();

                // Apply Harmony patches
                _harmony = new HarmonyLib.Harmony("ProjectX.LootRespawn");
                _harmony.PatchAll(typeof(PickUpAwakePatch));
                try { _harmony.PatchAll(typeof(PickUpOnEnablePatch)); }
                catch (Exception ex) { RLog.Warning($"[LootRespawn] OnEnable patch failed (non-critical): {ex.Message}"); }
                _harmony.PatchAll(typeof(PickUpCollectPatch));

                RLog.Msg($"[LootRespawn] ★ Initialized — {_collectedLoot.Count} tracked items loaded, days={RespawnConfig.RespawnDays}");
            }
            catch (Exception ex)
            {
                RLog.Error($"[LootRespawn] Failed to initialize: {ex.Message}");
            }
        }

        /// <summary>Flush dirty data to disk periodically</summary>
        public static void OnUpdate()
        {
            if (_dirty && Time.time - _lastSaveTime >= SaveCooldown)
            {
                SaveToDisk();
            }
        }

        public static void Reset()
        {
            int count = _collectedLoot.Count;
            _collectedLoot.Clear();
            _dirty = true;
            _suppressedCount = 0;
            _respawnedCount = 0;
            SaveToDisk();
            RLog.Msg($"[LootRespawn] ★ Tracker reset — cleared {count} items, all loot will respawn on next load");
        }

        // --- Harmony Callbacks ---

        /// <summary>
        /// Called when a PickUp spawns (Awake) or re-enables (OnEnable).
        /// If we've collected this item and not enough days have passed, destroy it.
        /// If enough days have passed, let it spawn and stop tracking it.
        /// Only runs on server/singleplayer (server-authoritative).
        /// </summary>
        internal static void OnPickUpSpawned(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;

            try
            {
                // Server-authoritative: only track when we are the server or singleplayer
                if (IsMultiplayerClient()) return;

                // Fast exit if nothing is tracked
                if (_collectedLoot.Count == 0) return;

                // Skip player-placed items (clones)
                string objName = pickup.name;
                if (string.IsNullOrEmpty(objName) || objName.Contains("Clone")) return;

                // Skip items without a PickupGui (non-interactable world objects)
                if (pickup.transform.Find("_PickupGui_") == null)
                {
                    // Exception: radios are valid pickups without PickupGui
                    if (!objName.StartsWith("Radio") || objName.Contains("FromStructure"))
                        return;
                }

                string id = GenerateIdentifier(pickup);
                if (string.IsNullOrEmpty(id)) return;

                if (_collectedLoot.TryGetValue(id, out int collectedDay))
                {
                    int currentDay = GetCurrentDay();
                    int elapsed = currentDay - collectedDay;

                    if (elapsed < RespawnConfig.RespawnDays)
                    {
                        // Not enough days have passed — destroy the pickup
                        var target = pickup._destroyTarget != null ? pickup._destroyTarget : pickup.gameObject;
                        UnityEngine.Object.Destroy(target);
                        _suppressedCount++;

                        if (RespawnConfig.ConsoleLogging && _suppressedCount <= 50)
                            RLog.Msg($"[LootRespawn] Suppressed: {objName} (day {collectedDay}, need {RespawnConfig.RespawnDays - elapsed} more)");
                    }
                    else
                    {
                        // Respawn timer expired — let it spawn and remove tracking
                        _collectedLoot.Remove(id);
                        _dirty = true;
                        _respawnedCount++;

                        if (RespawnConfig.ConsoleLogging)
                            RLog.Msg($"[LootRespawn] Respawned: {objName} (elapsed {elapsed} days)");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Spawn check error: {ex.Message}");
            }
        }

        /// <summary>
        /// Called BEFORE a pickup is collected (Prefix on Collect).
        /// Records the collection day. Skips clones and non-standard pickups.
        /// Only records on server/singleplayer (server-authoritative).
        /// </summary>
        internal static bool OnPickUpCollecting(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return true; // continue collection

            try
            {
                // Server-authoritative: only track when we are the server or singleplayer
                if (IsMultiplayerClient()) return true;

                // Skip player-placed items (clones)
                string objName = pickup.name;
                if (string.IsNullOrEmpty(objName) || objName.Contains("Clone")) return true;

                string id = GenerateIdentifier(pickup);
                if (string.IsNullOrEmpty(id)) return true;

                int day = GetCurrentDay();
                _collectedLoot[id] = day;
                _dirty = true;

                if (RespawnConfig.ConsoleLogging)
                    RLog.Msg($"[LootRespawn] Collected: {objName} (id={pickup._itemId}, day={day}, tracked={_collectedLoot.Count})");
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Collect error: {ex.Message}");
            }

            return true; // always allow collection to proceed
        }

        // --- Game Day ---

        /// <summary>
        /// Get the current game day number via TimeOfDayHolder.GetTimeOfDay().Days.
        /// Falls back to 0 if the call fails.
        /// </summary>
        private static int GetCurrentDay()
        {
            try
            {
                var timeOfDay = Sons.Environment.TimeOfDayHolder.GetTimeOfDay();
                return timeOfDay.Days;
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] GetCurrentDay failed: {ex.Message}");
            }

            return 0;
        }

        // --- Server Authority ---

        /// <summary>
        /// Returns true if we are a multiplayer CLIENT (not the server/host).
        /// Uses the same pattern as StoneGate/Network modules:
        ///   BoltNetwork.isRunning && BoltNetwork.isClient
        /// Avoids BoltNetwork.isServerOrNotRunning which doesn't exist in IL2CPP interop.
        /// </summary>
        private static bool IsMultiplayerClient()
        {
            try
            {
                return BoltNetwork.isRunning && BoltNetwork.isClient;
            }
            catch { return false; } // If Bolt isn't available, treat as singleplayer
        }

        // --- Identifier Generation ---

        /// <summary>
        /// Generate a stable identifier for a pickup using its name, position, and rotation.
        /// Includes rotation to reduce hash collisions (items at similar positions but
        /// different orientations are different loot spawns).
        /// </summary>
        private static string GenerateIdentifier(PickUp pickup)
        {
            try
            {
                var t = pickup.transform;
                if (t == null) return null;

                var pos = t.position;
                var rot = t.rotation;
                string name = pickup.name;

                if (string.IsNullOrEmpty(name)) return null;

                // Use first 3 chars of name (matches original mod approach)
                string namePrefix = name.Length >= 3 ? name.Substring(0, 3) : name;

                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + namePrefix.GetHashCode();
                    hash = hash * 31 + (int)(pos.x * 100f);
                    hash = hash * 31 + (int)(pos.y * 100f);
                    hash = hash * 31 + (int)(pos.z * 100f);
                    hash = hash * 31 + (int)(rot.x * 100f);
                    hash = hash * 31 + (int)(rot.y * 100f);
                    hash = hash * 31 + (int)(rot.z * 100f);
                    hash = hash * 31 + (int)(rot.w * 100f);
                    return hash.ToString();
                }
            }
            catch { return null; }
        }

        // --- Persistence ---

        /// <summary>
        /// Save collected loot to disk as a simple flat file.
        /// Format: one "id:day" pair per line.
        /// </summary>
        private static void SaveToDisk()
        {
            _dirty = false;
            _lastSaveTime = Time.time;

            try
            {
                if (string.IsNullOrEmpty(_saveFilePath)) return;

                using var writer = new StreamWriter(_saveFilePath, false);
                foreach (var kvp in _collectedLoot)
                {
                    writer.WriteLine($"{kvp.Key}:{kvp.Value}");
                }

                if (RespawnConfig.ConsoleLogging)
                    RLog.Msg($"[LootRespawn] Saved {_collectedLoot.Count} items to disk");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] SaveToDisk failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Load collected loot from disk.
        /// </summary>
        private static void LoadFromDisk()
        {
            try
            {
                if (string.IsNullOrEmpty(_saveFilePath) || !File.Exists(_saveFilePath)) return;

                var lines = File.ReadAllLines(_saveFilePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int sep = line.LastIndexOf(':');
                    if (sep <= 0) continue;

                    string id = line.Substring(0, sep);
                    string dayStr = line.Substring(sep + 1);

                    if (int.TryParse(dayStr, out int day))
                    {
                        _collectedLoot[id] = day;
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] LoadFromDisk failed: {ex.Message}");
            }
        }

        // --- Status ---

        public static string GetStatus()
        {
            if (!_initialized) return "Respawn: Not initialized";
            return $"Respawn: {(RespawnConfig.Enabled ? "ON" : "OFF")} | Days: {RespawnConfig.RespawnDays} | Tracked: {_collectedLoot.Count} | Day: {GetCurrentDay()} | Suppressed: {_suppressedCount} | Respawned: {_respawnedCount}";
        }
    }

    // ===== Harmony Patches (manually applied, NOT auto-discovered) =====

    [HarmonyPatch(typeof(Sons.Gameplay.PickUp), "Awake")]
    internal static class PickUpAwakePatch
    {
        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.PickUp __instance)
        {
            LootRespawnModule.OnPickUpSpawned(__instance);
        }
    }

    [HarmonyPatch(typeof(Sons.Gameplay.PickUp), "OnEnable")]
    internal static class PickUpOnEnablePatch
    {
        [HarmonyPostfix]
        private static void Postfix(Sons.Gameplay.PickUp __instance)
        {
            LootRespawnModule.OnPickUpSpawned(__instance);
        }
    }

    [HarmonyPatch(typeof(Sons.Gameplay.PickUp), "Collect")]
    internal static class PickUpCollectPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(Sons.Gameplay.PickUp __instance)
        {
            return LootRespawnModule.OnPickUpCollecting(__instance);
        }
    }
}
