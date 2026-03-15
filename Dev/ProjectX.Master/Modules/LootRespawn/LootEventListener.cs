using System;
using System.Collections.Generic;
using System.Text;
using Bolt;
using Il2CppInterop.Runtime.Injection;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// GlobalEventListener that handles debugCommand Bolt events for:
    ///   - C→S loot collection reporting (px:loot:collect)
    ///   - C→S status request (px:loot:status-req)  — server gathers data, sends response
    ///   - S→C status response (px:loot:status)     — client caches for GUI display
    ///   - C→S sync request  (px:loot:sync-req)     — client requests suppression list
    ///   - S→C sync response (px:loot:sync)         — server sends full suppression list
    ///   - S→C suppress      (px:loot:suppress)     — server broadcasts single new suppression
    /// 
    /// Pattern: SimpleNetworkEvents' CustomGlobalEventListener — proven on 7,800+ installs.
    /// Transport: debugCommand built-in Bolt event (bypasses Packets.NetEvent C→S drop).
    /// 
    /// CRITICAL: Must call BoltNetwork.AddGlobalEventListener() to register with Bolt.
    /// CRITICAL: Must NOT use [HideFromIl2Cpp] on OnEvent — IL2CPP needs the vtable entry.
    /// 
    /// Runs on BOTH server and client:
    ///   Server: handles px:loot:collect + px:loot:status-req + px:loot:sync-req
    ///   Client: handles px:loot:status + px:loot:sync + px:loot:suppress
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class LootEventListener : GlobalEventListener
    {
        private static LootEventListener _instance;

        /// <summary>
        /// Cached server status text — updated when server responds to status request.
        /// GUI reads this to display authoritative server data.
        /// Empty on local host (falls back to local GetStatus).
        /// </summary>
        private static string _serverStatusText = "";
        public static string ServerStatusText => _serverStatusText;

        /// <summary>
        /// Create and register the listener (called on both server and client).
        /// </summary>
        public static void Create()
        {
            if (_instance != null) return;

            var go = new GameObject("PX_LootEventListener");
            GameObject.DontDestroyOnLoad(go);
            _instance = go.AddComponent<LootEventListener>();
            RLog.Msg("[LootEventListener] Created and registered as GlobalEventListener");
        }

        /// <summary>
        /// Unity Start callback — register with Bolt's global event system.
        /// Same pattern as SimpleNetworkEvents.CustomGlobalEventListener.Start().
        /// </summary>
        public void Start()
        {
            BoltNetwork.AddGlobalEventListener((MonoBehaviour)(object)this);
            RLog.Msg("[LootEventListener] Registered with BoltNetwork.AddGlobalEventListener");
        }

        /// <summary>
        /// Bolt callback — fires when any debugCommand event is received.
        /// Routes by input prefix: px:loot:*
        /// 
        /// NOTE: No [HideFromIl2Cpp] — IL2CPP must see this override in the vtable.
        /// </summary>
        public override void OnEvent(debugCommand evnt)
        {
            try
            {
                string cmd = evnt.input;
                if (string.IsNullOrEmpty(cmd) || !cmd.StartsWith("px:loot:")) return;

                // ═══ Server-side: handle loot collection reports ═══
                if (BoltNetwork.isServer && cmd == "px:loot:collect")
                {
                    string payload = evnt.input2;
                    if (string.IsNullOrEmpty(payload)) return;

                    int lastColon = payload.LastIndexOf(':');
                    if (lastColon <= 0) return;

                    string hash = payload.Substring(0, lastColon);
                    string itemIdStr = payload.Substring(lastColon + 1);

                    if (!int.TryParse(itemIdStr, out int itemId)) return;

                    LootRespawnModule.RecordPickupFromClient(hash, itemId);
                    return;
                }

                // ═══ Server-side: handle status requests → send response ═══
                if (BoltNetwork.isServer && cmd == "px:loot:status-req")
                {
                    SendStatusResponse();
                    return;
                }

                // ═══ Server-side: handle suppression list request → send full list ═══
                if (BoltNetwork.isServer && cmd == "px:loot:sync-req")
                {
                    RLog.Msg("[LootEventListener] ★ Received sync request from client");
                    SendSyncResponse();
                    return;
                }

                // ═══ Client-side: handle status responses → cache for GUI ═══
                if (!BoltNetwork.isServer && cmd == "px:loot:status")
                {
                    _serverStatusText = evnt.input2 ?? "";
                    RLog.Msg($"[LootEventListener] ★ Received server loot status: {_serverStatusText}");
                    return;
                }

                // ═══ Client-side: handle full suppression list → apply to tracker ═══
                if (!BoltNetwork.isServer && cmd == "px:loot:sync")
                {
                    string payload = evnt.input2;
                    if (string.IsNullOrEmpty(payload))
                    {
                        // Empty list — server has 0 tracked items
                        LootRespawnModule.ApplyServerState(new List<(string, long, int)>());
                        return;
                    }

                    var entries = new List<(string hash, long timestamp, int itemId)>();
                    string[] items = payload.Split('|');
                    foreach (string item in items)
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        // Format: hash:timestamp:itemId
                        // Hash itself may contain colons (MD5 doesn't, but be safe)
                        // Use LastIndexOf twice to parse from the right
                        int lastSep = item.LastIndexOf(':');
                        if (lastSep <= 0) continue;
                        string itemIdStr = item.Substring(lastSep + 1);

                        string remainder = item.Substring(0, lastSep);
                        int secondLastSep = remainder.LastIndexOf(':');
                        if (secondLastSep <= 0) continue;
                        string timestampStr = remainder.Substring(secondLastSep + 1);
                        string hash = remainder.Substring(0, secondLastSep);

                        if (long.TryParse(timestampStr, out long timestamp) &&
                            int.TryParse(itemIdStr, out int itemId))
                        {
                            entries.Add((hash, timestamp, itemId));
                        }
                    }

                    RLog.Msg($"[LootEventListener] ★ Received suppression list: {entries.Count} items");
                    LootRespawnModule.ApplyServerState(entries);
                    return;
                }

                // ═══ Client-side: handle incremental suppression broadcast ═══
                if (!BoltNetwork.isServer && cmd == "px:loot:suppress")
                {
                    string payload = evnt.input2;
                    if (string.IsNullOrEmpty(payload)) return;

                    // Format: hash:timestamp:itemId
                    int lastSep = payload.LastIndexOf(':');
                    if (lastSep <= 0) return;
                    string itemIdStr = payload.Substring(lastSep + 1);

                    string remainder = payload.Substring(0, lastSep);
                    int secondLastSep = remainder.LastIndexOf(':');
                    if (secondLastSep <= 0) return;
                    string timestampStr = remainder.Substring(secondLastSep + 1);
                    string hash = remainder.Substring(0, secondLastSep);

                    if (long.TryParse(timestampStr, out long timestamp) &&
                        int.TryParse(itemIdStr, out int itemId))
                    {
                        RLog.Msg($"[LootEventListener] ★ Incremental suppress: {hash.Substring(0, System.Math.Min(8, hash.Length))}…");
                        LootRespawnModule.AddServerSuppression(hash, timestamp, itemId);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] Error processing event: {ex.Message}");
            }
        }

        // ==================== Server-Side Responses ====================

        /// <summary>
        /// Server-side: gather authoritative loot status and broadcast to all clients.
        /// </summary>
        private void SendStatusResponse()
        {
            try
            {
                string status = LootRespawnModule.GetStatus();
                var cmd = debugCommand.Raise(GlobalTargets.Everyone);
                cmd.input = "px:loot:status";
                cmd.input2 = status;
                ((Bolt.Event)cmd).Send();
                RLog.Msg($"[LootEventListener] Sent status response: {status}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] SendStatusResponse failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Server-side: serialize full suppression list and send via debugCommand.
        /// Format: hash:timestamp:itemId|hash:timestamp:itemId|...
        /// </summary>
        private void SendSyncResponse()
        {
            try
            {
                var collected = LootRespawnModule.GetCollectedEntries();
                
                if (collected.Count == 0)
                {
                    // Send empty sync so client knows to set _serverSyncReceived
                    var emptyCmd = debugCommand.Raise(GlobalTargets.Everyone);
                    emptyCmd.input = "px:loot:sync";
                    emptyCmd.input2 = "";
                    ((Bolt.Event)emptyCmd).Send();
                    RLog.Msg("[LootEventListener] Sent empty sync response (0 tracked items)");
                    return;
                }

                // Build serialized string — batch entries to stay under Bolt string limits
                // Each entry ~80 chars (32 hash + 10 timestamp + 5 itemId + separators)
                // Bolt string limit ~16KB → ~200 entries per batch
                const int MAX_ENTRIES_PER_BATCH = 150;
                int batchStart = 0;
                int batchNum = 0;

                while (batchStart < collected.Count)
                {
                    int batchEnd = System.Math.Min(batchStart + MAX_ENTRIES_PER_BATCH, collected.Count);
                    var sb = new StringBuilder(batchEnd - batchStart * 80);
                    
                    for (int i = batchStart; i < batchEnd; i++)
                    {
                        if (i > batchStart) sb.Append('|');
                        var entry = collected[i];
                        sb.Append(entry.Hash);
                        sb.Append(':');
                        sb.Append(entry.Timestamp);
                        sb.Append(':');
                        sb.Append(entry.ItemId);
                    }

                    var cmd = debugCommand.Raise(GlobalTargets.Everyone);
                    cmd.input = "px:loot:sync";
                    cmd.input2 = sb.ToString();
                    ((Bolt.Event)cmd).Send();
                    batchNum++;
                    batchStart = batchEnd;
                }

                RLog.Msg($"[LootEventListener] Sent sync response: {collected.Count} items in {batchNum} batch(es)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] SendSyncResponse failed: {ex.Message}");
            }
        }

        // ==================== Client-Side Requests ====================

        /// <summary>
        /// Client-side: send a status request to the server via debugCommand.
        /// Called from GUI "Show Loot Status" button.
        /// </summary>
        public static void RequestServerStatus()
        {
            try
            {
                var cmd = debugCommand.Create(GlobalTargets.Everyone);
                cmd.input = "px:loot:status-req";
                cmd.input2 = "";
                ((Bolt.Event)cmd).Send();
                RLog.Msg("[LootEventListener] Sent status request to server");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] RequestServerStatus failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Client-side: request the full suppression list from the server via debugCommand.
        /// Called on client join — replaces the broken LootSyncEvent.RequestState().
        /// </summary>
        public static void RequestServerSync()
        {
            try
            {
                if (!BoltNetwork.isRunning) return;

                var cmd = debugCommand.Create(GlobalTargets.Everyone);
                cmd.input = "px:loot:sync-req";
                cmd.input2 = "";
                ((Bolt.Event)cmd).Send();
                RLog.Msg("[LootEventListener] Sent sync request to server");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] RequestServerSync failed: {ex.Message}");
            }
        }

        // ==================== Server-Side Broadcasts ====================

        /// <summary>
        /// Server-side: broadcast a single new suppression to all clients via debugCommand.
        /// Called when any player collects an item — replaces LootSyncEvent.BroadcastNewSuppression().
        /// </summary>
        public static void BroadcastSuppression(string hash, long timestamp, int itemId)
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isServer) return;

                var cmd = debugCommand.Raise(GlobalTargets.Everyone);
                cmd.input = "px:loot:suppress";
                cmd.input2 = $"{hash}:{timestamp}:{itemId}";
                ((Bolt.Event)cmd).Send();
                RLog.Msg($"[LootEventListener] Broadcast suppress: {hash.Substring(0, System.Math.Min(8, hash.Length))}…");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootEventListener] BroadcastSuppression failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove the listener from Bolt on cleanup.
        /// </summary>
        public void OnDestroy()
        {
            try
            {
                BoltNetwork.RemoveGlobalEventListener((MonoBehaviour)(object)this);
                RLog.Msg("[LootEventListener] Removed from BoltNetwork");
            }
            catch { }
        }
    }
}
