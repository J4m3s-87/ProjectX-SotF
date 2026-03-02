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
    /// Loot Respawn module — uses Harmony patches on PickUp.Awake and PickUp.Collect.
    ///
    /// Tracks collected loot by game-day number (via TimeOfDayHolder.GetDayNumber).
    /// Persists tracking data to disk so respawn timers survive server restarts.
    ///
    /// - Postfix on PickUp.Awake → suppress item if not yet due for respawn
    /// - Postfix on PickUp.Collect → record collection day and persist
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

        public static bool Enabled
        {
            get => RespawnConfig.Enabled;
            set => RespawnConfig.Enabled = value;
        }

        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                // Determine save file path next to the mod DLL
                string modDir = Path.GetDirectoryName(typeof(LootRespawnModule).Assembly.Location) ?? "";
                _saveFilePath = Path.Combine(modDir, "loot_collected.dat");

                // Load persisted data
                LoadFromDisk();

                // Apply Harmony patches
                _harmony = new HarmonyLib.Harmony("ProjectX.LootRespawn");
                _harmony.PatchAll(typeof(PickUpAwakePatch));
                _harmony.PatchAll(typeof(PickUpCollectPatch));

                RLog.Msg($"[LootRespawn] Initialized — {_collectedLoot.Count} items loaded from disk");
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
            _collectedLoot.Clear();
            _dirty = true;
            SaveToDisk();
            RLog.Msg("[LootRespawn] Tracker reset — all items will respawn");
        }

        // --- Harmony Callbacks ---

        internal static void OnPickUpAwake(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;

            try
            {
                // Fast exit if nothing is tracked
                if (_collectedLoot.Count == 0) return;

                string id = GenerateIdentifierFast(pickup);
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
                    }
                    else
                    {
                        // Respawn timer expired — let it spawn and remove tracking
                        _collectedLoot.Remove(id);
                        _dirty = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Awake error: {ex.Message}");
            }
        }

        internal static void OnPickUpCollected(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;

            try
            {
                string id = GenerateIdentifierFast(pickup);
                if (string.IsNullOrEmpty(id)) return;

                int day = GetCurrentDay();
                _collectedLoot[id] = day;
                _dirty = true;
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Collect error: {ex.Message}");
            }
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

        // --- Identifier Generation ---

        /// <summary>
        /// Fast ID generation: name hash + quantized position.
        /// </summary>
        private static string GenerateIdentifierFast(PickUp pickup)
        {
            try
            {
                var t = pickup.transform;
                if (t == null) return null;

                var pos = t.position;
                string name = pickup.name;

                if (string.IsNullOrEmpty(name)) return null;

                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + name.GetHashCode();
                    hash = hash * 23 + (int)(pos.x * 100f);
                    hash = hash * 23 + (int)(pos.y * 100f);
                    hash = hash * 23 + (int)(pos.z * 100f);
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
            return $"Respawn: {(RespawnConfig.Enabled ? "ON" : "OFF")} | Days: {RespawnConfig.RespawnDays} | Tracked: {_collectedLoot.Count} | Day: {GetCurrentDay()}";
        }
    }

    // ===== Harmony Patches (manually applied, NOT auto-discovered) =====

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
}
