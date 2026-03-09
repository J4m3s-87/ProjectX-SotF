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
    /// 
    /// Pattern: SimpleNetworkEvents' CustomGlobalEventListener — proven on 7,800+ installs.
    /// Transport: debugCommand built-in Bolt event (bypasses Packets.NetEvent C→S drop).
    /// 
    /// CRITICAL: Must call BoltNetwork.AddGlobalEventListener() to register with Bolt.
    /// CRITICAL: Must NOT use [HideFromIl2Cpp] on OnEvent — IL2CPP needs the vtable entry.
    /// 
    /// Runs on BOTH server and client:
    ///   Server: handles px:loot:collect + px:loot:status-req
    ///   Client: handles px:loot:status (cached for GUI)
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
        /// Routes by input prefix: px:loot:collect, px:loot:status-req, px:loot:status.
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

                // ═══ Client-side: handle status responses → cache for GUI ═══
                if (!BoltNetwork.isServer && cmd == "px:loot:status")
                {
                    _serverStatusText = evnt.input2 ?? "";
                    RLog.Msg($"[LootEventListener] ★ Received server loot status: {_serverStatusText}");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[LootEventListener] Error processing event: {ex.Message}");
            }
        }

        /// <summary>
        /// Server-side: gather authoritative loot status and broadcast to all clients.
        /// </summary>
        private void SendStatusResponse()
        {
            try
            {
                string status = LootRespawnModule.GetStatus();
                var cmd = debugCommand.Create(GlobalTargets.Everyone);
                cmd.input = "px:loot:status";
                cmd.input2 = status;
                cmd.Send();
                RLog.Msg($"[LootEventListener] Sent status response: {status}");
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[LootEventListener] SendStatusResponse failed: {ex.Message}");
            }
        }

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
                cmd.Send();
                RLog.Msg("[LootEventListener] Sent status request to server");
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[LootEventListener] RequestServerStatus failed: {ex.Message}");
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
