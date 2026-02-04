using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using Endnight.Types;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Harmony patches for raid customization
    /// Simplified to avoid IL2CPP type marshaling issues
    /// </summary>
    public static class RaidPatches
    {
        private static HarmonyLib.Harmony _harmony;
        
        // Cached reflection for private fields
        private static FieldInfo _vailWorldEventDataField;
        private static FieldInfo _eventField; // _event field on QueuedEvent
        
        /// <summary>
        /// Apply all patches
        /// </summary>
        public static void Apply()
        {
            try
            {
                _harmony = new HarmonyLib.Harmony("ProjectX.RaidCustomizer");
                
                // Cache reflection for private fields
                CacheReflection();
                
                // Patch VailActor.OnEnable - apply stat multipliers when actors spawn
                // This is the most stable patch - prioritize it
                PatchVailActorOnEnable();
                
                // Simplified patches - use object parameters to avoid IL Compile Errors
                PatchRunEventSimplified();
                
                // Note: EventsForTime and ChooseEventsForDay patches disabled
                // due to IL Compile Errors with complex nested types
                // The IMGUI Raids tab now handles manual requeue instead
                
                RLog.Msg("[RaidCustomizer] Patches applied (stat multipliers + announcements)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[RaidCustomizer] Failed to apply patches: {ex.Message}");
            }
        }
        
        private static void CacheReflection()
        {
            try
            {
                // Try multiple possible field names for world event data
                string[] fieldNames = { "_worldEventData", "_vailWorldEventData", "worldEventData" };
                
                foreach (var name in fieldNames)
                {
                    _vailWorldEventDataField = typeof(VailWorldEvents).GetField(name, 
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (_vailWorldEventDataField != null)
                    {
                        RLog.Msg($"[RaidCustomizer] Found world event data field: {name}");
                        break;
                    }
                }
                
                // Diagnostic: dump all fields on VailWorldEvents if we didn't find it
                if (_vailWorldEventDataField == null)
                {
                    RLog.Warning("[RaidCustomizer] World event data field not found by name, dumping all fields:");
                    var allFields = typeof(VailWorldEvents).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (var f in allFields)
                    {
                        RLog.Msg($"[RaidCustomizer]   Field: {f.Name} ({f.FieldType.Name})");
                    }
                }
                
                // Cache _event field on QueuedEvent for announcements
                var queuedEventType = typeof(VailWorldEvents).GetNestedType("QueuedEvent", BindingFlags.Public | BindingFlags.NonPublic);
                if (queuedEventType != null)
                {
                    _eventField = queuedEventType.GetField("_event", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Reflection cache failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Get the VailWorldEventData from VailWorldEvents instance
        /// Uses multiple approaches: method call, property, then field reflection
        /// </summary>
        public static VailWorldEventData GetWorldEventData(VailWorldEvents worldEvents)
        {
            if (worldEvents == null) return null;
            
            try
            {
                // First, try calling the private GetWorldEventData() method via reflection
                // This is more reliable in IL2CPP than field access
                var getMethod = typeof(VailWorldEvents).GetMethod("GetWorldEventData", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (getMethod != null)
                {
                    var result = getMethod.Invoke(worldEvents, null);
                    if (result != null)
                    {
                        return result as VailWorldEventData;
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Method invoke failed: {ex.Message}");
            }
            
            try
            {
                // Then try cached field
                if (_vailWorldEventDataField != null)
                {
                    var result = _vailWorldEventDataField.GetValue(worldEvents);
                    if (result != null)
                    {
                        return result as VailWorldEventData;
                    }
                }
                
                // Try finding field directly
                var field = typeof(VailWorldEvents).GetField("_worldEventData", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (field != null)
                {
                    var result = field.GetValue(worldEvents);
                    if (result != null)
                    {
                        return result as VailWorldEventData;
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Field access failed: {ex.Message}");
            }
            
            return null;
        }

        
        /// <summary>
        /// Remove all patches
        /// </summary>
        public static void Remove()
        {
            try
            {
                _harmony?.UnpatchSelf();
                RLog.Msg("[RaidCustomizer] Patches removed");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Failed to remove patches: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Simplified RunEvent patch - uses reflection to access event data
        /// Avoids complex IL2CPP type parameters
        /// </summary>
        private static void PatchRunEventSimplified()
        {
            try
            {
                var original = AccessTools.Method(typeof(VailWorldEvents), "RunEvent");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] RunEvent method not found");
                    return;
                }
                
                var prefix = AccessTools.Method(typeof(RaidPatches), nameof(RunEvent_Prefix_Simplified));
                _harmony.Patch(original, prefix: new HarmonyMethod(prefix));
                RLog.Msg("[RaidCustomizer] RunEvent patch applied (announcements enabled)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] RunEvent patch failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Simplified prefix - uses object parameter to avoid IL2CPP nested type issues
        /// </summary>
        private static void RunEvent_Prefix_Simplified(VailWorldEvents __instance, object queuedEvent)
        {
            try
            {
                if (queuedEvent == null || _eventField == null) return;
                
                // Get the _event field via reflection
                var eventObj = _eventField.GetValue(queuedEvent);
                if (eventObj == null) return;
                
                // Cast to VailWorldEventData.EventBase and check if it's a search party
                if (eventObj is VailWorldEventData.EventBase eventBase)
                {
                    if (EventTools.IsActualSearchParty(eventBase))
                    {
                        var searchParty = eventBase.Cast<VailWorldEventData.SearchPartyEvent>();
                        EventAnnouncer.Announce(searchParty);
                    }
                }
            }
            catch { }
        }
        
        private static void PatchVailActorOnEnable()
        {
            try
            {
                // Try to find OnEnable or similar activation method on VailActor
                var original = AccessTools.Method(typeof(VailActor), "OnEnable");
                if (original == null)
                {
                    // Try alternative method names
                    original = AccessTools.Method(typeof(VailActor), "Awake");
                }
                if (original == null)
                {
                    original = AccessTools.Method(typeof(VailActor), "Start");
                }
                
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] VailActor activation method not found - stat multipliers disabled");
                    return;
                }
                
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(VailActor_OnEnable_Postfix));
                _harmony.Patch(original, postfix: new HarmonyMethod(postfix));
                RLog.Msg($"[RaidCustomizer] Patched VailActor.{original.Name} for stat multipliers");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] VailActor patch failed: {ex.Message}");
            }
        }
        
        private static void VailActor_OnEnable_Postfix(VailActor __instance)
        {
            try
            {
                // Check if stat modification is enabled
                if (!RaidConfig.StatMultiplierModificationEnabled.Value) return;
                
                // Apply damage and aggression multipliers
                StatsMultiplierModifier.ApplyMultipliers(__instance);
            }
            catch { }
        }
    }
}

