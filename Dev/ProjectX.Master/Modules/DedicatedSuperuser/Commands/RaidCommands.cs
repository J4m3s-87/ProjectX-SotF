using System;
using System.Collections.Generic;
using System.Reflection;
using Endnight.Types;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using SonsSdk;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    /// <summary>
    /// Server-safe raid commands — works on both client and dedicated server.
    /// Uses VailWorldEvents directly (server-authoritative).
    /// </summary>
    public static class RaidCommands
    {
        public static void Handle(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px raid start|boss|clear|clearall|cooldown|requeue|status");
                return;
            }

            switch (args[0].ToLower())
            {
                case "start":   RunRandomRaid(); break;
                case "boss":    RunBossRaid(); break;
                case "clear":   ClearQueuedRaids(); break;
                case "clearall": ClearAllEvents(); break;
                case "cooldown": ClearRaidCooldowns(); break;
                case "requeue": RequeueRaids(); break;
                case "status":  PrintRaidStatus(); break;
                default:
                    Log($"Unknown raid command: {args[0]}");
                    break;
            }
        }

        /// <summary>
        /// Trigger a random raid immediately
        /// </summary>
        public static void RunRandomRaid()
        {
            try
            {
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("Raids not ready — world not loaded");
                    return;
                }

                var eventData = RaidCustomizer.RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    Log("Cannot access raid data — field lookup failed");
                    return;
                }

                var searchParties = new List<VailWorldEventData.SearchPartyEvent>();
                foreach (var evt in eventData._searchPartyEvents)
                {
                    if (RaidCustomizer.EventTools.IsActualSearchParty(evt))
                    {
                        // Only pick Creepy (1) and Muddy (3) raids — these path aggressively
                        // toward the player's base. Cannibal (0) raids are patrol/scout events
                        // that may spawn far away and never reach the cross.
                        int typeVal = (int)evt.type;
                        if (typeVal == 1 || typeVal == 3)
                            searchParties.Add(evt);
                    }
                }

                RLog.Msg($"[RaidCustomizer] RunRandomRaid: {searchParties.Count} valid creepy/muddy raids (cannibal patrols excluded)");

                if (searchParties.Count == 0)
                {
                    Log("No valid raids available");
                    return;
                }

                var random = new Random();
                var selectedRaid = searchParties[random.Next(0, searchParties.Count)];
                
                // Force permissive constraints for manual trigger — on dedicated servers
                // anger is always 0 and day may not meet vanilla thresholds, which causes
                // RunEventNow() to silently fail even though the call succeeds.
                selectedRaid.minMaxAnger = new UnityEngine.Vector2(0f, float.MaxValue);
                selectedRaid.minMaxDay = new UnityEngine.Vector2Int(0, int.MaxValue);
                selectedRaid.RunEventNow();
                
                Log($"Triggered raid: {selectedRaid.name} (type={(int)selectedRaid.type})");
            }
            catch (Exception ex)
            {
                Log($"RunRandomRaid failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Trigger a boss raid specifically
        /// </summary>
        public static void RunBossRaid()
        {
            try
            {
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("Raids not ready — world not loaded");
                    return;
                }

                var eventData = RaidCustomizer.RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    Log("Cannot access raid data — field lookup failed");
                    return;
                }

                var bossRaids = new List<VailWorldEventData.SearchPartyEvent>();
                foreach (var evt in eventData._searchPartyEvents)
                {
                    if (RaidCustomizer.EventTools.IsBossEvent(evt))
                    {
                        bossRaids.Add(evt.Cast<VailWorldEventData.SearchPartyEvent>());
                    }
                }

                if (bossRaids.Count == 0)
                {
                    Log("No boss raids available");
                    return;
                }

                var random = new Random();
                var selectedBoss = bossRaids[random.Next(0, bossRaids.Count)];
                
                // Force permissive constraints — see RunRandomRaid() comment
                selectedBoss.minMaxAnger = new UnityEngine.Vector2(0f, float.MaxValue);
                selectedBoss.minMaxDay = new UnityEngine.Vector2Int(0, int.MaxValue);
                selectedBoss.RunEventNow();
                
                Log($"Triggered BOSS raid: {selectedBoss.name}");
            }
            catch (Exception ex)
            {
                Log($"RunBossRaid failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear all queued raid events
        /// </summary>
        public static void ClearQueuedRaids()
        {
            try
            {
                RaidCustomizer.QueuedEventHandler.ClearQueuedRaidEvents();
                Log("Queued raids cleared");
            }
            catch (Exception ex)
            {
                Log($"ClearQueuedRaids failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear ALL queued events (not just raids)
        /// </summary>
        public static void ClearAllEvents()
        {
            try
            {
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("VailWorldEvents not ready");
                    return;
                }

                worldEvents.ClearEvents();
                Log("All events cleared");
            }
            catch (Exception ex)
            {
                Log($"ClearAllEvents failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear raid cooldowns to allow raids to trigger again
        /// </summary>
        public static void ClearRaidCooldowns()
        {
            try
            {
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("VailWorldEvents not ready");
                    return;
                }

                var eventData = RaidCustomizer.RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    Log("Cannot access event data — field lookup failed");
                    return;
                }

                int cleared = 0;
                foreach (var searchPartyEvent in eventData._searchPartyEvents)
                {
                    if (RaidCustomizer.EventTools.IsActualSearchParty(searchPartyEvent))
                    {
                        try
                        {
                            var statsDict = worldEvents._eventRunStats?._statsDict;
                            if (statsDict != null && statsDict.ContainsKey(searchPartyEvent.name))
                            {
                                var history = statsDict[searchPartyEvent.name];
                                if (history != null)
                                {
                                    history._runCount = 0;
                                    history._lastDay = -1;
                                    cleared++;
                                }
                            }
                        }
                        catch { }
                    }
                }

                Log($"Cleared {cleared} raid cooldowns");
            }
            catch (Exception ex)
            {
                Log($"ClearRaidCooldowns failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Requeue raids with current settings.
        /// Works the same as the local RaidActions.RequeueRaids():
        /// 1. Clear existing queued raids
        /// 2. Reset handler and configurator
        /// 3. Apply config via PrepareForNewDay
        /// 4. Set _lastQueuedDay = -1 so the game's Update() triggers ChooseEventsForDay
        ///    on the next tick, with our Harmony patches (EventsForTime_Postfix,
        ///    ChooseEventsForDay_Prefix) applying the user's configured settings.
        /// </summary>
        public static void RequeueRaids()
        {
            try
            {
                RaidCustomizer.QueuedEventHandler.ClearQueuedRaidEvents();
                RaidCustomizer.QueuedEventHandler.Reset();
                RaidCustomizer.SearchPartyEventConfigurator.Reset();

                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("VailWorldEvents not ready");
                    return;
                }

                var eventData = RaidCustomizer.RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    Log("Cannot access event data — field lookup failed");
                    return;
                }

                // Apply our search party configuration to the event data
                RaidCustomizer.SearchPartyEventConfigurator.PrepareForNewDay(eventData);

                // Clear event run stats (cooldowns) for search parties —
                // without this, InCooldown returns true for previously selected events,
                // causing ChooseEventsForDay to select 0 events on subsequent requeues.
                try
                {
                    var statsDict = worldEvents._eventRunStats?._statsDict;
                    if (statsDict != null)
                    {
                        int cleared = 0;
                        foreach (var searchEvt in eventData._searchPartyEvents)
                        {
                            if (searchEvt != null && RaidCustomizer.EventTools.IsActualSearchParty(searchEvt))
                            {
                                try
                                {
                                    if (statsDict.ContainsKey(searchEvt.name))
                                    {
                                        var history = statsDict[searchEvt.name];
                                        if (history != null)
                                        {
                                            history._runCount = 0;
                                            history._lastDay = -1;
                                            cleared++;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        if (cleared > 0)
                            RLog.Msg($"[RaidCustomizer] Cleared cooldowns for {cleared} events");
                    }
                }
                catch (Exception ex3)
                {
                    RLog.Warning($"[RaidCustomizer] Cooldown reset failed: {ex3.Message}");
                }

                // Reset the last queued day — the game's Update() loop detects the
                // day mismatch and calls ChooseEventsForDay on the next tick.
                // Our Harmony patches ensure the right settings are applied.
                int prevDay = worldEvents._lastQueuedDay;
                worldEvents._lastQueuedDay = -1;
                
                Log($"Raids requeued (_lastQueuedDay: {prevDay} → -1, game will select events on next tick)");
            }
            catch (Exception ex)
            {
                Log($"RequeueRaids failed: {ex.Message}");
            }
        }


        /// <summary>
        /// Print queued raid status to log
        /// </summary>
        public static void PrintRaidStatus()
        {
            try
            {
                RaidCustomizer.QueuedEventHandler.PrintQueuedRaids();

                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    Log("VailWorldEvents not loaded");
                    return;
                }

                var eventData = RaidCustomizer.RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    Log("No event data — field lookup failed");
                    return;
                }

                int total = 0;
                int active = 0;
                foreach (var evt in eventData._searchPartyEvents)
                {
                    if (RaidCustomizer.EventTools.IsActualSearchParty(evt))
                    {
                        total++;
                        if (!RaidCustomizer.SearchPartyEventConfigurator.ShouldDisable(evt))
                            active++;
                    }
                }
                Log($"Raids: {active}/{total} active | Last queued day: {worldEvents._lastQueuedDay}");
            }
            catch (Exception ex)
            {
                Log($"PrintRaidStatus failed: {ex.Message}");
            }
        }

        private static void Log(string msg)
        {
            // Send to all connected players' chat AND server log
            ChatResponse.Send(msg);
        }
    }
}
