using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// Serializes/deserializes config entries for network sync between server and clients.
    /// Server serializes all registered entries → sends via ConfigSyncEvent → client applies.
    /// </summary>
    public static class ConfigSyncPayload
    {
        // Registered config entries to sync (key → getter/setter)
        private static readonly List<SyncEntry> _entries = new List<SyncEntry>();
        
        // Broadcast tracking
        private static bool _broadcastEnabled;
        private static bool _dirty;
        private static float _lastBroadcastTime;
        private const float BROADCAST_DEBOUNCE = 2.0f; // seconds between re-broadcasts
        
        public static int EntryCount => _entries.Count;
        
        /// <summary>
        /// Register all config entries that should be synced to clients.
        /// Called once at end of Config.Init().
        /// </summary>
        public static void RegisterEntries()
        {
            _entries.Clear();
            
            // Register key gameplay-affecting config values that clients need
            TryRegister("WelcomeMessageDelay", () => Config.WelcomeMessageDelay?.Value.ToString(), v => { if (float.TryParse(v, out var f)) Config.WelcomeMessageDelay.Value = f; });
            
            RLog.Msg($"[ConfigSyncPayload] Registered {_entries.Count} entries for network sync");
        }
        
        /// <summary>
        /// Enable broadcast mode (server/owner only).
        /// After this, config changes trigger re-broadcast to all clients.
        /// </summary>
        public static void EnableBroadcast()
        {
            _broadcastEnabled = true;
            RLog.Msg("[ConfigSyncPayload] Broadcast enabled");
        }
        
        /// <summary>
        /// Called every frame on server/owner. Checks if a debounced re-broadcast is needed.
        /// </summary>
        public static void Update()
        {
            if (!_broadcastEnabled || !_dirty) return;
            
            if (Time.time - _lastBroadcastTime < BROADCAST_DEBOUNCE) return;
            
            _dirty = false;
            _lastBroadcastTime = Time.time;
            
            try
            {
#if SERVER || OWNER
                ConfigSyncEvent.Instance?.BroadcastConfig();
#endif
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSyncPayload] Broadcast failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Mark config as dirty so next Update() triggers a broadcast.
        /// </summary>
        public static void MarkDirty()
        {
            _dirty = true;
        }
        
        /// <summary>
        /// Serialize all registered entries to a pipe-delimited string.
        /// Format: key=value|key=value|...
        /// </summary>
        public static string Serialize()
        {
            var sb = new StringBuilder();
            foreach (var entry in _entries)
            {
                try
                {
                    string val = entry.Getter();
                    if (val != null)
                    {
                        if (sb.Length > 0) sb.Append('|');
                        sb.Append(entry.Key).Append('=').Append(val);
                    }
                }
                catch { }
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Apply a serialized config payload received from server.
        /// Returns number of entries successfully applied.
        /// </summary>
        public static int Apply(string payload)
        {
            if (string.IsNullOrEmpty(payload)) return 0;
            
            int applied = 0;
            var pairs = payload.Split('|');
            
            var entryMap = _entries.ToDictionary(e => e.Key, e => e);
            
            foreach (var pair in pairs)
            {
                var eqIdx = pair.IndexOf('=');
                if (eqIdx <= 0) continue;
                
                var key = pair.Substring(0, eqIdx);
                var value = pair.Substring(eqIdx + 1);
                
                if (entryMap.TryGetValue(key, out var entry))
                {
                    try
                    {
                        entry.Setter(value);
                        applied++;
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[ConfigSyncPayload] Failed to apply {key}: {ex.Message}");
                    }
                }
            }
            
            return applied;
        }
        
        private static void TryRegister(string key, Func<string> getter, Action<string> setter)
        {
            try
            {
                _entries.Add(new SyncEntry { Key = key, Getter = getter, Setter = setter });
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSyncPayload] Failed to register {key}: {ex.Message}");
            }
        }
        
        private class SyncEntry
        {
            public string Key;
            public Func<string> Getter;
            public Action<string> Setter;
        }
    }
}
