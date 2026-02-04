using System;
using System.Collections.Generic;
using System.Linq;
using Endnight.Types;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using SonsSdk;
using TheForest.Utils;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Static helper class providing raid action methods for UI buttons
    /// Based on original RaidCustomizer console commands
    /// </summary>
    public static class RaidActions
    {
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
                    SonsTools.ShowMessage("Raids not ready - load a save first");
                    return;
                }
                
                var eventData = RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    SonsTools.ShowMessage("Cannot access raid data");
                    return;
                }
                
                // Get all valid search party events
                var searchParties = new List<VailWorldEventData.SearchPartyEvent>();
                foreach (var evt in eventData._searchPartyEvents)
                {
                    if (EventTools.IsActualSearchParty(evt) && !SearchPartyEventConfigurator.ShouldDisable(evt))
                    {
                        searchParties.Add(evt);
                    }
                }
                
                if (searchParties.Count == 0)
                {
                    SonsTools.ShowMessage("No valid raids available");
                    return;
                }
                
                // Pick random raid and trigger it
                var random = new Random();
                var selectedRaid = searchParties[random.Next(0, searchParties.Count)];
                selectedRaid.RunEventNow();
                
                SonsTools.ShowMessage($"Triggered raid: {selectedRaid.name}");
                RLog.Msg($"[RaidCustomizer] Triggered raid: {selectedRaid.name}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] RunRandomRaid failed: {ex.Message}");
                SonsTools.ShowMessage("Failed to trigger raid");
            }
        }
        
        /// <summary>
        /// Clear all queued raid events
        /// </summary>
        public static void ClearQueuedRaids()
        {
            try
            {
                QueuedEventHandler.ClearQueuedRaidEvents();
                SonsTools.ShowMessage("Queued raids cleared");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ClearQueuedRaids failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Clear ALL events (not just raids)
        /// </summary>
        public static void ClearAllEvents()
        {
            try
            {
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    SonsTools.ShowMessage("VailWorldEvents not ready");
                    return;
                }
                
                worldEvents.ClearEvents();
                SonsTools.ShowMessage("All events cleared");
                RLog.Msg("[RaidCustomizer] Cleared all events");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ClearAllEvents failed: {ex.Message}");
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
                    SonsTools.ShowMessage("VailWorldEvents not ready");
                    return;
                }
                
                var eventData = RaidPatches.GetWorldEventData(worldEvents);
                if (eventData == null)
                {
                    SonsTools.ShowMessage("Cannot access event data");
                    return;
                }
                
                int cleared = 0;
                foreach (var searchPartyEvent in eventData._searchPartyEvents)
                {
                    if (EventTools.IsActualSearchParty(searchPartyEvent))
                    {
                        // Reset cooldown via reflection on event run stats
                        // worldEvents._eventRunStats._statsDict[name]._runCount = 0, _lastDay = -1
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
                
                SonsTools.ShowMessage($"Cleared {cleared} raid cooldowns");
                RLog.Msg($"[RaidCustomizer] Cleared cooldowns for {cleared} raids");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ClearRaidCooldowns failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Requeue raids with current settings
        /// </summary>
        public static void RequeueRaids()
        {
            try
            {
                QueuedEventHandler.ClearQueuedRaidEvents();
                QueuedEventHandler.Reset();
                SearchPartyEventConfigurator.Reset();
                
                var worldEvents = SingletonBehaviour<VailWorldEvents>._instance;
                if (worldEvents == null)
                {
                    SonsTools.ShowMessage("VailWorldEvents not ready");
                    return;
                }
                
                var eventData = RaidPatches.GetWorldEventData(worldEvents);
                if (eventData != null)
                {
                    SearchPartyEventConfigurator.PrepareForNewDay(eventData);
                }
                
                // Reset the last queued day to force requeue
                worldEvents._lastQueuedDay = -1;
                
                SonsTools.ShowMessage("Raids requeued");
                RLog.Msg("[RaidCustomizer] Raids requeued with new settings");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] RequeueRaids failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Print queued raids to log
        /// </summary>
        public static void PrintQueuedRaids()
        {
            try
            {
                QueuedEventHandler.PrintQueuedRaids();
                SonsTools.ShowMessage("Queued raids printed to log");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] PrintQueuedRaids failed: {ex.Message}");
            }
        }
    }
}
