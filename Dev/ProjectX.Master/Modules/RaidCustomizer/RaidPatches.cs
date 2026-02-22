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
                
                // === CRITICAL: Event queuing patches ===
                // Without these, the game's ChooseEventsForDay returns 0 events
                // because anger=0 and no config overrides are applied.
                PatchEventsForTime();
                PatchChooseEventsForDay();
                PatchInCooldown();
                
                // Patch VailActor.OnEnable - apply stat multipliers when actors spawn
                PatchVailActorOnEnable();
                
                // PERMANENTLY DISABLED: RunEvent patch causes IL Compile Error which
                // corrupts the CLR method table, leading to Tab/backpack crash.
                // PatchRunEventSimplified();
                
                RLog.Msg("[RaidCustomizer] Patches applied (event queuing + stat multipliers)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[RaidCustomizer] Failed to apply patches: {ex.Message}");
            }
        }
        
        // =====================================================================
        // EVENT QUEUING PATCHES — make ChooseEventsForDay produce correct results
        // =====================================================================
        
        /// <summary>
        /// Postfix on VailWorldEventData.EventsForTime — overrides how many events
        /// are chosen per day period. Without this, the game returns 0 events when
        /// anger is low. This is the single most important patch for raid queuing.
        /// </summary>
        private static void PatchEventsForTime()
        {
            try
            {
                var original = AccessTools.Method(typeof(VailWorldEventData), "EventsForTime");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] EventsForTime method not found");
                    return;
                }
                
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(EventsForTime_Postfix));
                _harmony.Patch(original, postfix: new HarmonyMethod(postfix));
                RLog.Msg("[RaidCustomizer] Patched EventsForTime (event count override)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] EventsForTime patch failed: {ex.Message}");
            }
        }
        
        private static void EventsForTime_Postfix(ref int __result, VailWorldEventData.TimeOfEvent __1, int __2)
        {
            try
            {
                // If no event time ranges are selected in config, return 0
                if (!RaidConfig.HasAnyEventRangesSelected())
                {
                    __result = 0;
                    return;
                }
                
                // If we have a custom events-per-day setting, override the result.
                // CRITICAL: Only apply the custom count for TimeOfEvent.Day (value 0).
                // The game calls EventsForTime for each period (Day=0, Night=1, Any=2).
                // The original mod returns the count for Day only, and 0 for others.
                // Without this filter, raids are tripled (count × 3 time periods).
                int eventsPerDay;
                if (RaidConfig.HasCustomEventsPerDay(__2, out eventsPerDay))
                {
                    // __1 == 0 means TimeOfEvent.Day — the only period that gets custom count
                    __result = ((int)__1 == 0) ? eventsPerDay : 0;
                }
                // When RaidsPerDay=-1, HasCustomEventsPerDay returns false,
                // so __result keeps the vanilla value (passthrough).
            }
            catch { }
        }
        
        /// <summary>
        /// Prefix on VailWorldEvents.ChooseEventsForDay — applies our config modifications
        /// (spawn counts, anger/day overrides) to the event data before the game selects events.
        /// </summary>
        private static void PatchChooseEventsForDay()
        {
            try
            {
                var original = AccessTools.Method(typeof(VailWorldEvents), "ChooseEventsForDay");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] ChooseEventsForDay method not found");
                    return;
                }
                
                var prefix = AccessTools.Method(typeof(RaidPatches), nameof(ChooseEventsForDay_Prefix));
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(ChooseEventsForDay_Postfix));
                _harmony.Patch(original, prefix: new HarmonyMethod(prefix), postfix: new HarmonyMethod(postfix));
                RLog.Msg("[RaidCustomizer] Patched ChooseEventsForDay (prefix + postfix)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ChooseEventsForDay patch failed: {ex.Message}");
            }
        }
        
        private static bool ChooseEventsForDay_Prefix(VailWorldEventData __0)
        {
            try
            {
                // No server-side forced overrides — vanilla behavior when config is default.
                // Users can explicitly set RaidsPerDay, IgnoreMinMaxAnger, etc. via config
                // if they want non-vanilla raid behavior on the server.
                
                QueuedEventHandler.Reset();
                SearchPartyEventConfigurator.PrepareForNewDay(__0);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ChooseEventsForDay prefix failed: {ex.Message}");
            }
            return true; // Continue to original method
        }
        
        /// <summary>
        /// Postfix on ChooseEventsForDay — hook point for future post-processing.
        /// __1 is eventList (List<QueuedEvent>), __4 is cannibalAngerLevel, __6 is playerCount
        /// </summary>
        private static void ChooseEventsForDay_Postfix(
            Il2CppSystem.Collections.Generic.List<VailWorldEvents.QueuedEvent> __1,
            int __3, float __4, int __6)
        {
            // Postfix reserved for future use (e.g., event filtering, announcements)
        }
        
        /// <summary>
        /// Prefix on EventStatsDictionary.InCooldown — overrides cooldown checks for
        /// search party events to respect our custom cooldown settings.
        /// </summary>
        private static void PatchInCooldown()
        {
            try
            {
                var dictType = typeof(VailWorldEvents).GetNestedType("EventStatsDictionary", 
                    BindingFlags.Public | BindingFlags.NonPublic);
                if (dictType == null)
                {
                    RLog.Warning("[RaidCustomizer] EventStatsDictionary type not found");
                    return;
                }
                
                var original = AccessTools.Method(dictType, "InCooldown");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] InCooldown method not found");
                    return;
                }
                
                var prefix = AccessTools.Method(typeof(RaidPatches), nameof(InCooldown_Prefix));
                _harmony.Patch(original, prefix: new HarmonyMethod(prefix));
                RLog.Msg("[RaidCustomizer] Patched InCooldown (cooldown override)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] InCooldown patch failed: {ex.Message}");
            }
        }
        
        private static bool InCooldown_Prefix(VailWorldEventData.EventBase __0, ref bool __result)
        {
            try
            {
                if (__0 == null || !EventTools.IsActualSearchParty(__0))
                    return true; // Not a search party, let original run
                
                if (SearchPartyEventConfigurator.ShouldDisable(__0))
                {
                    __result = true; // Disabled events are always "in cooldown"
                    return false;
                }
                
                bool isBoss = EventTools.IsBossEvent(__0);
                if ((isBoss && RaidConfig.BossRaidsCooldown.Value == 0) ||
                    (!isBoss && RaidConfig.NormalRaidsCooldown.Value == 0))
                {
                    __result = false; // Zero cooldown = never in cooldown
                    return false;
                }
            }
            catch { }
            return true; // Let original handle it
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
                // Note: reflection fallback for _worldEventData typically fails in IL2CPP
                // (GetFields only returns wrapper fields). The typed access path at
                // worldEvents._worldEventData always works — this is just a secondary cache.
                
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
        /// Uses typed field access first (IL2CPP safe), then reflection fallback
        /// </summary>
        public static VailWorldEventData GetWorldEventData(VailWorldEvents worldEvents)
        {
            if (worldEvents == null) return null;
            
            try
            {
                // Typed field access — direct IL2CPP memory read, always works
                // _worldEventData is at offset 0x28 in the IL2CPP dump
                var data = worldEvents._worldEventData;
                if (data != null) return data;
            }
            catch { }
            
            try
            {
                // Fallback: cached field reflection (GetValue generally works in IL2CPP)
                if (_vailWorldEventDataField != null)
                {
                    var result = _vailWorldEventDataField.GetValue(worldEvents);
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
        
        
#if !SERVER
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
#endif
        
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

