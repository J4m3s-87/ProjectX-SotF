using System;
using System.Drawing;
using System.Reflection;
using Endnight.Types;
using RedLoader;
using Sons.Characters;
using SonsSdk;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Raid Customizer Module
    /// Controls raid frequency, timing, and enemy spawn counts
    /// </summary>
    public static class RaidCustomizerModule
    {
        // MustRequeueRaids flag is now in Config.cs to allow OnValueChanged subscriptions
        
        /// <summary>
        /// Initialize the module
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Initialize config
                RaidConfig.Init();
                
                // Apply Harmony patches
                RaidPatches.Apply();
                
                // Register update callback
                SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);
                
                // Cleanup on scene change
                SdkEvents.OnWorldExited.Subscribe(OnWorldExited);
                
                RLog.Msg(Color.LimeGreen, "[RaidCustomizer] Module Initialized");
            }
            catch (Exception ex)
            {
                RLog.Error($"[RaidCustomizer] Failed to initialize: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Shutdown the module
        /// </summary>
        public static void Shutdown()
        {
            try
            {
                RaidPatches.Remove();
                RLog.Msg("[RaidCustomizer] Module Shutdown");
            }
            catch { }
        }
        
        private static void OnUpdate()
        {
            try
            {
                if (!Config.MustRequeueRaids) return;
                Config.MustRequeueRaids = false;
                
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null) return;
                
                // Clear queued raids
                QueuedEventHandler.ClearQueuedRaidEvents();
                
                // Try to requeue via QueueMorningEvents or equivalent
                TryRequeueEvents(worldEvents);
                
                RLog.Msg(Color.Orange, "[RaidCustomizer] Raids requeued with new settings");
            }
            catch { }
        }
        
        private static void TryRequeueEvents(VailWorldEvents worldEvents)
        {
            try
            {
                // Try calling QueueMorningEvents via reflection
                var method = typeof(VailWorldEvents).GetMethod("QueueMorningEvents", 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                
                if (method != null)
                {
                    method.Invoke(worldEvents, null);
                    return;
                }
                
                // Alternative: Try QueueEvents or similar
                var altMethod = typeof(VailWorldEvents).GetMethod("QueueEvents",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                
                if (altMethod != null)
                {
                    altMethod.Invoke(worldEvents, null);
                }
            }
            catch { }
        }
        
        private static void OnWorldExited()
        {
            try
            {
                QueuedEventHandler.Reset();
                SearchPartyEventConfigurator.Reset();
                Config.MustRequeueRaids = false;
            }
            catch { }
        }
    }
}
