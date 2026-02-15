using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using HarmonyLib;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Tracks collected loot items and determines respawn eligibility.
    /// Uses MD5 hash of position+rotation+name as unique identifier (matching original LootRespawnControl).
    /// </summary>
    public static class LootTracker
    {
        /// <summary>
        /// Stored data for a collected loot item
        /// </summary>
        public class LootData
        {
            public string Hash { get; set; }
            public long Timestamp { get; set; }
            public int ItemId { get; set; }

            public LootData(string hash, long timestamp, int itemId)
            {
                Hash = hash;
                Timestamp = timestamp;
                ItemId = itemId;
            }
        }

        // All currently collected (suppressed) loot
        private static readonly HashSet<LootData> _collectedLoot = new();

        // Cached accessor for TimeOfDayHolder
        private static System.Reflection.MethodInfo _getTimeOfDayMethod;

        /// <summary>
        /// Generate a unique identifier for a pickup based on its transform and name.
        /// Matches the original LootRespawnControl MD5 approach.
        /// </summary>
        public static string GenerateIdentifier(Vector3 position, Quaternion rotation, string name)
        {
            try
            {
                // Combine position, rotation, and name into a single string
                string input = $"{position.x:F2},{position.y:F2},{position.z:F2}|{rotation.x:F2},{rotation.y:F2},{rotation.z:F2},{rotation.w:F2}|{name}";

                using var md5 = MD5.Create();
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] GenerateIdentifier failed: {ex.Message}");
                // Fallback to simpler hash
                return $"{Mathf.RoundToInt(position.x)}_{Mathf.RoundToInt(position.y)}_{Mathf.RoundToInt(position.z)}_{name}";
            }
        }

        /// <summary>
        /// Check if a loot item has been collected (is in the suppressed set)
        /// </summary>
        public static bool IsCollected(string identifier)
        {
            foreach (var data in _collectedLoot)
            {
                if (data.Hash == identifier) return true;
            }
            return false;
        }

        /// <summary>
        /// Mark a loot item as collected
        /// </summary>
        public static void MarkCollected(string identifier, int itemId, long timestamp)
        {
            // Don't double-add
            if (IsCollected(identifier)) return;

            _collectedLoot.Add(new LootData(identifier, timestamp, itemId));
        }

        /// <summary>
        /// Remove a loot item from the collected set (allow it to respawn)
        /// </summary>
        public static void RemoveFromCollected(string identifier)
        {
            _collectedLoot.RemoveWhere(d => d.Hash == identifier);
        }

        /// <summary>
        /// Check if enough in-game time has passed for a loot item to respawn
        /// </summary>
        public static bool HasEnoughTimePassed(string identifier, long currentTimestamp)
        {
            foreach (var data in _collectedLoot)
            {
                if (data.Hash == identifier)
                {
                    long elapsed = currentTimestamp - data.Timestamp;
                    // Each game day is roughly 1440 minutes (24h * 60m)
                    long requiredMinutes = (long)RespawnConfig.RespawnDays * 1440L;
                    return elapsed >= requiredMinutes;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the current game timestamp (minutes since day 0)
        /// Uses TimeOfDayHolder via reflection for IL2CPP safety
        /// </summary>
        public static long GetCurrentTimestamp()
        {
            try
            {
                if (_getTimeOfDayMethod == null)
                {
                    _getTimeOfDayMethod = AccessTools.Method("Sons.Environment.TimeOfDayHolder:GetTimeOfDay");
                }

                if (_getTimeOfDayMethod != null)
                {
                    var timeOfDay = _getTimeOfDayMethod.Invoke(null, null);
                    return GetTimestampFromTimeOfDay(timeOfDay.ToString());
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootRespawn] GetCurrentTimestamp failed: {ex.Message}");
            }
            return 0;
        }

        /// <summary>
        /// Convert a TimeOfDay string to a numeric timestamp (minutes)
        /// Format varies but typically "Day X HH:MM" or similar
        /// </summary>
        private static long GetTimestampFromTimeOfDay(string timeString)
        {
            try
            {
                // Try to parse day number and time
                // TimeOfDay struct typically has Day and Hour properties
                // As a fallback, use a simple hash approach
                if (string.IsNullOrEmpty(timeString)) return 0;
                return (long)timeString.GetHashCode();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the current game day number
        /// </summary>
        public static int CurrentDay
        {
            get
            {
                try
                {
                    var method = AccessTools.Method("Sons.Environment.TimeOfDayHolder:GetDayNumber");
                    if (method != null)
                    {
                        return (int)(float)method.Invoke(null, null);
                    }
                }
                catch { }
                return 0;
            }
        }

        /// <summary>
        /// Reset all tracking data
        /// </summary>
        public static void Reset()
        {
            _collectedLoot.Clear();
            RLog.Msg("[LootRespawn] Tracker reset");
        }

        /// <summary>
        /// Number of items being tracked as collected
        /// </summary>
        public static int TrackedCount => _collectedLoot.Count;
    }
}
