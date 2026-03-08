using Bolt;
using Il2CppInterop.Runtime.Injection;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Server-side GlobalEventListener that receives debugCommand Bolt events
    /// for silent C→S loot collection reporting.
    /// 
    /// Pattern: SimpleNetworkEvents' CustomGlobalEventListener — proven on 7,800+ installs.
    /// Transport: debugCommand built-in Bolt event (bypasses Packets.NetEvent C→S drop).
    /// Routing: input="px:loot:collect", input2="{hash}:{itemId}"
    /// 
    /// CRITICAL: Must call BoltNetwork.AddGlobalEventListener() to register with Bolt.
    /// CRITICAL: Must NOT use [HideFromIl2Cpp] on OnEvent — IL2CPP needs the vtable entry.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class LootEventListener : GlobalEventListener
    {
        private static LootEventListener _instance;

        /// <summary>
        /// Create and register the listener (server-only, called once).
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
        /// We filter by input prefix to only handle our events.
        /// 
        /// NOTE: No [HideFromIl2Cpp] — IL2CPP must see this override in the vtable.
        /// </summary>
        public override void OnEvent(debugCommand evnt)
        {
            // Only process on server
            if (!BoltNetwork.isServer) return;

            // Only handle our events
            if (evnt.input != "px:loot:collect") return;

            try
            {
                string payload = evnt.input2;
                if (string.IsNullOrEmpty(payload)) return;

                // Parse "{hash}:{itemId}"
                int lastColon = payload.LastIndexOf(':');
                if (lastColon <= 0) return;

                string hash = payload.Substring(0, lastColon);
                string itemIdStr = payload.Substring(lastColon + 1);

                if (!int.TryParse(itemIdStr, out int itemId)) return;

                // Record the collection on the server
                LootRespawnModule.RecordPickupFromClient(hash, itemId);
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[LootEventListener] Error processing loot event: {ex.Message}");
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
