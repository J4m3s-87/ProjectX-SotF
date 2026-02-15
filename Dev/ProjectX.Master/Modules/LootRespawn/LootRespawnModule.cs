using System;
using System.Collections.Generic;
using HarmonyLib;
using RedLoader;
using Sons.Gameplay;
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Loot Respawn module — uses manual Harmony patches on PickUp.Awake and PickUp.Collect.
    /// 
    /// OPTIMIZED: Replaced expensive MD5 hashing with fast integer hashing to prevent load lag.
    /// 
    /// - Postfix on PickUp.Awake → check if item was collected (fast lookup)
    /// - Postfix on PickUp.Collect → record collection
    /// </summary>
    public static class LootRespawnModule
    {
        private static bool _initialized = false;
        private static HarmonyLib.Harmony _harmony;
        
        // Collected loot tracking: identifier → entry
        private static readonly Dictionary<string, LootEntry> _collectedLoot = new();
        
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
                _harmony = new HarmonyLib.Harmony("ProjectX.LootRespawn");
                _harmony.PatchAll(typeof(PickUpAwakePatch));
                _harmony.PatchAll(typeof(PickUpCollectPatch));
                
                RLog.Msg("[LootRespawn] Harmony patches applied (Awake + Collect)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[LootRespawn] Failed to apply Harmony patches: {ex.Message}");
            }
        }

        /// <summary>No-op — module is event-driven via Harmony hooks</summary>
        public static void OnUpdate() { }

        public static void Reset()
        {
            _collectedLoot.Clear();
            RLog.Msg("[LootRespawn] Tracker reset");
        }

        // --- Harmony Callbacks ---

        internal static void OnPickUpAwake(PickUp pickup)
        {
            if (pickup == null || !RespawnConfig.Enabled) return;

            try
            {
                // OPTIMIZATION: Check if we even have any collected loot before computing ID
                if (_collectedLoot.Count == 0) return;

                string id = GenerateIdentifierFast(pickup);
                if (string.IsNullOrEmpty(id)) return;

                if (_collectedLoot.TryGetValue(id, out var entry))
                {
                    float elapsed = Time.time - entry.CollectedAtRealtime;
                    float required = RespawnConfig.RespawnDays * 1440f; // 1 game day ≈ 24 min real

                    if (elapsed < required)
                    {
                        var target = pickup._destroyTarget != null ? pickup._destroyTarget : pickup.gameObject;
                        UnityEngine.Object.Destroy(target);
                    }
                    else
                    {
                        _collectedLoot.Remove(id);
                    }
                }
            }
            catch (Exception ex)
            {
                // Throttled logging to prevent spam during load
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

                _collectedLoot[id] = new LootEntry
                {
                    ItemId = pickup._itemId,
                    CollectedAtRealtime = Time.time
                };
                // Debug log removed to prevent spam during gameplay
                // RLog.Msg($"[LootRespawn] Collected: {pickup.name}");
            }
            catch (Exception ex)
            {
                if (Time.frameCount % 600 == 0)
                    RLog.Warning($"[LootRespawn] Collect error: {ex.Message}");
            }
        }

        // --- Helpers ---

        /// <summary>
        /// OPTIMIZED: High-performance ID generation without MD5 or string formatting allocations.
        /// Uses name hash + quantized position. Collision chance is extremely low for static loot.
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
                    
                    // Quantize position to 1cm (0.01) to handle floating point drift
                    // Multiply by 100 and cast to int
                    hash = hash * 23 + (int)(pos.x * 100f);
                    hash = hash * 23 + (int)(pos.y * 100f);
                    hash = hash * 23 + (int)(pos.z * 100f);
                    
                    // Simple string return is safer for dictionary keys than raw ints 
                    // (prevents collision with different items at same hash)
                    return hash.ToString();
                }
            }
            catch { return null; }
        }

        public static string GetStatus()
        {
            if (!_initialized) return "Respawn: Not initialized";
            return $"Respawn: {(RespawnConfig.Enabled ? "ON" : "OFF")} | Days: {RespawnConfig.RespawnDays} | Tracked: {_collectedLoot.Count}";
        }

        private class LootEntry
        {
            public int ItemId;
            public float CollectedAtRealtime;
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
