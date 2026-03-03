using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using Endnight.Types;
using UnityEngine;

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
                
                // Patch VailActor.OnActorEnabled — apply damage, aggression, and HP multipliers
                // Uses unsafe IntPtr + IL2CPP offset arithmetic (bypasses all reflection issues)
                PatchVailActorOnActorEnabled();
                
                // Patch ReviveVailActor.Revive — re-apply follower HP after first-aid revival
                // (OnActorEnabled doesn't fire when a downed companion is revived)
                PatchReviveVailActor();
                
                // PERMANENTLY DISABLED: RunEvent patch causes IL Compile Error which
                // corrupts the CLR method table, leading to Tab/backpack crash.
                // PatchRunEventSimplified();
                
                RLog.Msg("[RaidCustomizer] Patches applied (event queuing + stat multipliers via unsafe offsets)");
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
        /// On dedicated servers the server process is counted as a "player",
        /// inflating playerCount by 1. This corrects it so vanilla raid logic
        /// uses the real human-player count. Only applies when Application.isBatchMode.
        /// </summary>
        private static int CorrectPlayerCount(int rawCount)
        {
            if (UnityEngine.Application.isBatchMode && rawCount > 1)
                return rawCount - 1;
            return rawCount;
        }
        
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
                
                var prefix = AccessTools.Method(typeof(RaidPatches), nameof(EventsForTime_Prefix));
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(EventsForTime_Postfix));
                _harmony.Patch(original, prefix: new HarmonyMethod(prefix), postfix: new HarmonyMethod(postfix));
                RLog.Msg("[RaidCustomizer] Patched EventsForTime (prefix + postfix)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] EventsForTime patch failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prefix: correct the playerCount parameter before vanilla EventsForTime runs.
        /// On dedicated servers the server process inflates the count by 1.
        /// </summary>
        private static void EventsForTime_Prefix(ref int __2)
        {
            __2 = CorrectPlayerCount(__2);
        }
        
        private static void EventsForTime_Postfix(ref int __result, VailWorldEventData.TimeOfEvent __1, int __2)
        {
            try
            {
                int originalResult = __result;
                
                // If no event time ranges are selected in config, return 0
                if (!RaidConfig.HasAnyEventRangesSelected())
                {
                    __result = 0;
                    RLog.Msg($"[RaidCustomizer] EventsForTime: NO ranges selected → result=0 (was {originalResult})");
                    return;
                }
                
                // If we have a custom events-per-day setting, override the result.
                int eventsPerDay;
                if (RaidConfig.HasCustomEventsPerDay(__2, out eventsPerDay))
                {
                    __result = ((int)__1 == 0) ? eventsPerDay : 0;
                    RLog.Msg($"[RaidCustomizer] EventsForTime: custom={eventsPerDay}, period={(int)__1}, playerCount={__2} → result={__result} (was {originalResult})");
                }
                else
                {
                    RLog.Msg($"[RaidCustomizer] EventsForTime: passthrough, period={(int)__1}, playerCount={__2} → result={__result}");
                }
            }
            catch (Exception ex) { RLog.Warning($"[RaidCustomizer] EventsForTime_Postfix error: {ex.Message}"); }
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
        
        private static bool ChooseEventsForDay_Prefix(VailWorldEventData __0, ref int __6)
        {
            try
            {
                // Correct player count on dedicated servers (server process inflates by 1)
                __6 = CorrectPlayerCount(__6);
                
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
        /// Postfix on ChooseEventsForDay — diagnostic logging + hook point.
        /// __1 is eventList (List<QueuedEvent>), __3 is day, __4 is cannibalAngerLevel, __6 is playerCount
        /// </summary>
        private static void ChooseEventsForDay_Postfix(
            Il2CppSystem.Collections.Generic.List<VailWorldEvents.QueuedEvent> __1,
            int __3, float __4, int __6)
        {
            try
            {
                int eventCount = __1?.Count ?? 0;
                RLog.Msg($"[RaidCustomizer] ChooseEventsForDay RESULT: {eventCount} events selected (day={__3}, anger={__4:F1}, players={__6})");
                
                // Log config state at selection time
                RLog.Msg($"[RaidCustomizer]   Config: RaidsPerDay={RaidConfig.RaidsPerDay.Value}, " +
                    $"AllowCreepy={RaidConfig.AllowCreepy.Value}, AllowCannibals={RaidConfig.AllowCannibals.Value}, " +
                    $"AllowMuddies={RaidConfig.AllowMuddies.Value}, HasRanges={RaidConfig.HasAnyEventRangesSelected()}");
                
                if (eventCount > 0)
                {
                    for (int i = 0; i < eventCount; i++)
                    {
                        var evt = __1[i];
                        RLog.Msg($"[RaidCustomizer]   Event[{i}]: day={evt.day}, hour={evt._startTimeInHours:F1}, name={evt._event?.name}");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ChooseEventsForDay_Postfix error: {ex.Message}");
            }
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
        
        /// <summary>
        /// Patch VailActor.OnActorEnabled — applies damage and aggression multipliers
        /// using typed IL2CPP field access (mirrors original RaidCustomizer mod).
        /// </summary>
        private static void PatchVailActorOnActorEnabled()
        {
            try
            {
                var original = AccessTools.Method(typeof(VailActor), "OnActorEnabled");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] VailActor.OnActorEnabled not found - damage/aggression multipliers disabled");
                    return;
                }
                
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(VailActor_OnActorEnabled_Postfix));
                _harmony.Patch(original, postfix: new HarmonyMethod(postfix));
                RLog.Msg("[RaidCustomizer] Patched VailActor.OnActorEnabled for damage/aggression multipliers");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] VailActor.OnActorEnabled patch failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Patch VailActor.Revive — re-apply follower HP multiplier after first-aid revival.
        /// OnActorEnabled does NOT fire when a downed companion is revived, so this hook
        /// ensures the HP multiplier persists through down/revive cycles.
        /// Unlike ReviveVailActor.Revive (client-only), VailActor.Revive fires server-side.
        /// </summary>
        private static void PatchReviveVailActor()
        {
            try
            {
                var original = AccessTools.Method(typeof(VailActor), "Revive");
                if (original == null)
                {
                    RLog.Warning("[RaidCustomizer] VailActor.Revive not found — follower HP won't persist through revives");
                    return;
                }
                
                var postfix = AccessTools.Method(typeof(RaidPatches), nameof(VailActor_Revive_Postfix));
                _harmony.Patch(original, postfix: new HarmonyMethod(postfix));
                RLog.Msg("[RaidCustomizer] Patched VailActor.Revive for follower HP persistence");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] VailActor.Revive patch failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Postfix: modify _gameSettingsDamageMultiplier, _gameSettingsAngerMultiplier, and _health
        /// via unsafe IL2CPP pointer + offset arithmetic (bypasses all reflection/DummyDll issues).
        /// 
        /// Known offsets from Il2CppDumper dump.cs:
        ///   VailActor._gameSettingsDamageMultiplier = 0x4CC (float)
        ///   VailActor._gameSettingsAngerMultiplier  = 0x4D0 (float)
        ///   VailActor._healthSettings               = 0x218 (ActorHealthSettings ref)
        ///   ActorHealthSettings._health              = 0x18  (float - base HP)
        /// </summary>
        private const int OFFSET_DAMAGE_MULT = 0x4CC;
        private const int OFFSET_ANGER_MULT  = 0x4D0;
        private const int OFFSET_HEALTH_SETTINGS = 0x218;
        private const int OFFSET_HEALTH_VALUE = 0x18;
        
        private static System.Reflection.PropertyInfo _cachedPointerProp;
        private static bool _pointerPropCached = false;
        private static bool _loggedFirstCall = false;
        
        // Cache the ORIGINAL base HP per healthSettings pointer.
        // This makes HP modification idempotent: always baseHP × multiplier,
        // so re-applying gives the same result (1000, not 10000, not 100000).
        private static readonly System.Collections.Generic.Dictionary<IntPtr, float> _baseHealthCache 
            = new System.Collections.Generic.Dictionary<IntPtr, float>();
        
        private static unsafe void VailActor_OnActorEnabled_Postfix(VailActor __instance)
        {
            try
            {
                bool enabled = RaidConfig.StatMultiplierModificationEnabled.Value;
                
                if (!_loggedFirstCall)
                {
                    _loggedFirstCall = true;
                    RLog.Msg($"[RaidCustomizer] OnActorEnabled postfix first call — StatOverrides={enabled}");
                }
                
                if (!enabled) return;
                
                if (!_pointerPropCached)
                {
                    _pointerPropCached = true;
                    _cachedPointerProp = __instance.GetType().GetProperty("Pointer",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    RLog.Msg($"[RaidCustomizer] Pointer property cached: {_cachedPointerProp != null}");
                }
                
                if (_cachedPointerProp == null) return;
                IntPtr actorPtr = (IntPtr)_cachedPointerProp.GetValue(__instance);
                if (actorPtr == IntPtr.Zero) return;
                
                var typeId = __instance.TypeId;
                
                // --- Damage multiplier at offset 0x4CC (idempotent — replaces value) ---
                float* damagePtr = (float*)((byte*)actorPtr.ToPointer() + OFFSET_DAMAGE_MULT);
                float currentDmg = *damagePtr;
                float newDmg = StatsMultiplierModifier.GetDamageMultiplier(typeId, currentDmg);
                if (Math.Abs(newDmg - currentDmg) > 0.001f)
                {
                    *damagePtr = newDmg;
                }
                
                // --- Aggression multiplier at offset 0x4D0 (idempotent — replaces value) ---
                float* angerPtr = (float*)((byte*)actorPtr.ToPointer() + OFFSET_ANGER_MULT);
                float currentAnger = *angerPtr;
                float newAnger = StatsMultiplierModifier.GetAggressionMultiplier(typeId, currentAnger);
                if (Math.Abs(newAnger - currentAnger) > 0.001f)
                {
                    *angerPtr = newAnger;
                }
                
                // --- HP multiplier via _healthSettings._health (IDEMPOTENT via base cache) ---
                IntPtr healthSettingsPtr = *(IntPtr*)((byte*)actorPtr.ToPointer() + OFFSET_HEALTH_SETTINGS);
                if (healthSettingsPtr != IntPtr.Zero)
                {
                    float* healthPtr = (float*)((byte*)healthSettingsPtr.ToPointer() + OFFSET_HEALTH_VALUE);
                    float currentHealth = *healthPtr;
                    float hpMultiplier = StatsMultiplierModifier.GetHealthMultiplier(typeId, 1.0f);
                    
                    if (Math.Abs(hpMultiplier - 1.0f) > 0.01f)
                    {
                        // Cache the original base HP on first encounter.
                        if (!_baseHealthCache.TryGetValue(healthSettingsPtr, out float baseHealth))
                        {
                            baseHealth = currentHealth;
                            _baseHealthCache[healthSettingsPtr] = baseHealth;
                        }
                        
                        float targetHealth = baseHealth * hpMultiplier;
                        
                        // 1. Write to template (affects future stat reads)
                        if (Math.Abs(targetHealth - currentHealth) > 0.5f)
                        {
                            *healthPtr = targetHealth;
                        }
                        
                        // 2. Write to RUNTIME stat system via pointer chain:
                        //    VailActor._statsManager (0x8E0) → VailStatsManager._statsManager (0x28)
                        //    → StatsManager._stats (0x18) → List<Stat> items
                        //    Stat._currentValue (0x10), Stat._max (0x24)
                        try
                        {
                            const int OFF_VAIL_STATS_MGR = 0x8E0;
                            const int OFF_STATS_MGR = 0x28;
                            const int OFF_STATS_LIST = 0x18;
                            const int OFF_STAT_CURRENT = 0x10;
                            const int OFF_STAT_MAX = 0x24;
                            
                            IntPtr vailStatsMgr = *(IntPtr*)((byte*)actorPtr.ToPointer() + OFF_VAIL_STATS_MGR);
                            if (vailStatsMgr != IntPtr.Zero)
                            {
                                IntPtr statsMgr = *(IntPtr*)((byte*)vailStatsMgr.ToPointer() + OFF_STATS_MGR);
                                if (statsMgr != IntPtr.Zero)
                                {
                                    IntPtr statsList = *(IntPtr*)((byte*)statsMgr.ToPointer() + OFF_STATS_LIST);
                                    if (statsList != IntPtr.Zero)
                                    {
                                        // Il2Cpp List<T>: _items at 0x10 (array ref), _size at 0x18
                                        IntPtr itemsArray = *(IntPtr*)((byte*)statsList.ToPointer() + 0x10);
                                        int listSize = *(int*)((byte*)statsList.ToPointer() + 0x18);
                                        
                                        if (itemsArray != IntPtr.Zero && listSize > 0)
                                        {
                                            // Il2Cpp Array: length at 0x18, first element at 0x20
                                            int arrLen = *(int*)((byte*)itemsArray.ToPointer() + 0x18);
                                            int count = Math.Min(listSize, arrLen);
                                            bool foundHealth = false;
                                            
                                            for (int i = 0; i < count; i++)
                                            {
                                                IntPtr statObj = *(IntPtr*)((byte*)itemsArray.ToPointer() + 0x20 + i * IntPtr.Size);
                                                if (statObj == IntPtr.Zero) continue;
                                                
                                                // Identify HealthStat by _baseValue matching template base HP
                                                // (stat[12] has base=100 for Kelvin, base=120 for Virginia)
                                                float statBaseVal = *(float*)((byte*)statObj.ToPointer() + 0x14); // _baseValue
                                                float statMax = *(float*)((byte*)statObj.ToPointer() + OFF_STAT_MAX);
                                                
                                                if (Math.Abs(statBaseVal - baseHealth) < 1f || Math.Abs(statMax - targetHealth) < 1f)
                                                {
                                                    *(float*)((byte*)statObj.ToPointer() + OFF_STAT_MAX) = targetHealth;
                                                    *(float*)((byte*)statObj.ToPointer() + OFF_STAT_CURRENT) = targetHealth;
                                                    *(float*)((byte*)statObj.ToPointer() + 0x14) = targetHealth; // _baseValue
                                                    foundHealth = true;
                                                    RLog.Msg($"[RaidCustomizer] {typeId} stats: dmg={newDmg:F1} HP={baseHealth:F0}*{hpMultiplier:F1}={targetHealth:F0} (stat[{i}] updated: base={statBaseVal:F0} max={statMax:F0}→{targetHealth:F0})");
                                                    break;
                                                }
                                            }
                                            
                                            if (!foundHealth)
                                            {
                                                RLog.Msg($"[RaidCustomizer] {typeId} stats: dmg={newDmg:F1} HP={baseHealth:F0}*{hpMultiplier:F1}={targetHealth:F0} (template only — no matching runtime stat found)");
                                            }
                                        }
                                        else
                                        {
                                            RLog.Msg($"[RaidCustomizer] {typeId} stats: dmg={newDmg:F1} HP={baseHealth:F0}*{hpMultiplier:F1}={targetHealth:F0} (template only — stats list empty)");
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Fallback: log template-only modification
                            RLog.Msg($"[RaidCustomizer] {typeId} stats: dmg={newDmg:F1} HP={baseHealth:F0}*{hpMultiplier:F1}={targetHealth:F0} (template only — stat chain error)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] OnActorEnabled postfix error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Postfix for VailActor.Revive — re-apply HP multiplier to the revived companion.
        /// __instance is the VailActor being revived, so we can read TypeId and reuse
        /// the same pointer chain as OnActorEnabled.
        /// </summary>
        private static unsafe void VailActor_Revive_Postfix(VailActor __instance)
        {
            try
            {
                if (!RaidConfig.StatMultiplierModificationEnabled.Value) return;
                if (!_pointerPropCached || _cachedPointerProp == null) return;
                
                var typeId = __instance.TypeId;
                var classId = VailTypes.GetActorClass(typeId);
                if ((int)classId != 4) return; // Only followers (class 4)
                
                IntPtr actorPtr = (IntPtr)_cachedPointerProp.GetValue(__instance);
                if (actorPtr == IntPtr.Zero) return;
                
                IntPtr healthSettingsPtr = *(IntPtr*)((byte*)actorPtr.ToPointer() + OFFSET_HEALTH_SETTINGS);
                if (healthSettingsPtr == IntPtr.Zero) return;
                
                if (!_baseHealthCache.TryGetValue(healthSettingsPtr, out float baseHealth))
                {
                    float* healthPtr = (float*)((byte*)healthSettingsPtr.ToPointer() + OFFSET_HEALTH_VALUE);
                    baseHealth = *healthPtr;
                    _baseHealthCache[healthSettingsPtr] = baseHealth;
                }
                
                float hpMultiplier = StatsMultiplierModifier.GetHealthMultiplier(typeId, 1.0f);
                if (Math.Abs(hpMultiplier - 1.0f) < 0.01f) return;
                
                float targetHealth = baseHealth * hpMultiplier;
                
                // Capture values for delayed application
                // The game's revive logic resets HP AFTER Revive() returns,
                // so we schedule the HP re-application 1 second later.
                var capturedActorPtr = actorPtr;
                var capturedHealthSettingsPtr = healthSettingsPtr;
                var capturedTypeId = typeId;
                var capturedTarget = targetHealth;
                var capturedBase = baseHealth;
                
                System.Threading.Timer delayTimer = null;
                delayTimer = new System.Threading.Timer(_ =>
                {
                    try
                    {
                        ApplyFollowerHP(capturedActorPtr, capturedHealthSettingsPtr, capturedTarget, capturedBase, capturedTypeId, "Revive(delayed)");
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[RaidCustomizer] Delayed revive HP error: {ex.Message}");
                    }
                    finally
                    {
                        delayTimer?.Dispose();
                    }
                }, null, 1000, System.Threading.Timeout.Infinite);
                
                RLog.Msg($"[RaidCustomizer] Revive detected: {typeId} — scheduled HP restore ({capturedTarget:F0}) in 1s");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Revive postfix error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Shared helper: writes target HP to the template and runtime stat system.
        /// Used by both OnActorEnabled (immediate) and Revive (delayed).
        /// </summary>
        private static unsafe void ApplyFollowerHP(IntPtr actorPtr, IntPtr healthSettingsPtr, float targetHealth, float baseHealth, object typeId, string source)
        {
            const int OFF_VAIL_STATS_MGR = 0x8E0;
            const int OFF_STATS_MGR = 0x28;
            const int OFF_STATS_LIST = 0x18;
            const int OFF_STAT_CURRENT = 0x10;
            const int OFF_STAT_MAX = 0x24;
            
            // 1. Update template
            float* healthPtr = (float*)((byte*)healthSettingsPtr.ToPointer() + OFFSET_HEALTH_VALUE);
            *healthPtr = targetHealth;
            
            // 2. Apply to runtime stat system
            IntPtr vailStatsMgr = *(IntPtr*)((byte*)actorPtr.ToPointer() + OFF_VAIL_STATS_MGR);
            if (vailStatsMgr == IntPtr.Zero) return;
            
            IntPtr statsMgr = *(IntPtr*)((byte*)vailStatsMgr.ToPointer() + OFF_STATS_MGR);
            if (statsMgr == IntPtr.Zero) return;
            
            IntPtr statsList = *(IntPtr*)((byte*)statsMgr.ToPointer() + OFF_STATS_LIST);
            if (statsList == IntPtr.Zero) return;
            
            IntPtr itemsArray = *(IntPtr*)((byte*)statsList.ToPointer() + 0x10);
            int listSize = *(int*)((byte*)statsList.ToPointer() + 0x18);
            
            if (itemsArray == IntPtr.Zero || listSize <= 0) return;
            
            int arrLen = *(int*)((byte*)itemsArray.ToPointer() + 0x18);
            int count = Math.Min(listSize, arrLen);
            
            for (int i = 0; i < count; i++)
            {
                IntPtr statObj = *(IntPtr*)((byte*)itemsArray.ToPointer() + 0x20 + i * IntPtr.Size);
                if (statObj == IntPtr.Zero) continue;
                
                float statBaseVal = *(float*)((byte*)statObj.ToPointer() + 0x14);
                float statMax = *(float*)((byte*)statObj.ToPointer() + OFF_STAT_MAX);
                float statCur = *(float*)((byte*)statObj.ToPointer() + OFF_STAT_CURRENT);
                
                if (Math.Abs(statBaseVal - baseHealth) < 1f || Math.Abs(statBaseVal - targetHealth) < 1f
                    || Math.Abs(statMax - targetHealth) < 1f)
                {
                    *(float*)((byte*)statObj.ToPointer() + OFF_STAT_MAX) = targetHealth;
                    *(float*)((byte*)statObj.ToPointer() + OFF_STAT_CURRENT) = targetHealth;
                    *(float*)((byte*)statObj.ToPointer() + 0x14) = targetHealth;
                    RLog.Msg($"[RaidCustomizer] {source} HP applied: {typeId} stat[{i}] cur={statCur:F0}→{targetHealth:F0} max={statMax:F0}→{targetHealth:F0}");
                    break;
                }
            }
        }
    }
}

