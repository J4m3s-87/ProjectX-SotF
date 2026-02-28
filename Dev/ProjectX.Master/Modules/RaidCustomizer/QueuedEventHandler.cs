
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Endnight.Types;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
#if !SERVER
using SonsSdk;
#endif
using TheForest.Utils;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Handles raid event scheduling and timing
    /// </summary>
    public static class QueuedEventHandler
    {
        private static readonly Random Rnd = new();
        private static readonly EventRangeData EventRangeData = new();
        private static SortedSet<int> _possibleHours;
        private static readonly HashSet<int> UsedHours = new();
        
        /// <summary>
        /// Set start time for a queued event or refuse if no valid times
        /// </summary>
        public static VailWorldEvents.QueuedEvent SetStartTimeOrRefuse(
            VailWorldEvents.QueuedEvent queuedEvent, 
            VailWorldEvents.EventChoiceData eventChoiceData)
        {
            try
            {
                int day = eventChoiceData.Day;
                
                // Get current hour from simulation
                float hourOfDay = 12f; // Default to noon
                try
                {
                    var simFunc = VailWorldSimulation.Instance;
                    if (simFunc != null)
                    {
                        var sim = simFunc();
                        if (sim != null)
                            hourOfDay = sim.HourOfDay;
                    }
                }
                catch { }
                
                int currentHour = (int)hourOfDay;
                
                if (_possibleHours == null)
                {
                    var morningRange = EventRangeData.GetHourRange(VailWorldEventData.TimeOfEvent.Morning, day);
                    var dayRange = EventRangeData.GetHourRange(VailWorldEventData.TimeOfEvent.Day, day);
                    var eveningRange = EventRangeData.GetHourRange(VailWorldEventData.TimeOfEvent.Evening, day);
                    var nightRange = EventRangeData.GetHourRange(VailWorldEventData.TimeOfEvent.Night, day);
                    _possibleHours = CalculatePossibleHours(morningRange, dayRange, eveningRange, nightRange);
                }
                
                queuedEvent.GetTimeString();
                
                if (!ChooseNewStartTime(queuedEvent, currentHour, day))
                    return null;
                
                return queuedEvent;
            }
            catch
            {
                return queuedEvent;
            }
        }
        
        private static bool ChooseNewStartTime(VailWorldEvents.QueuedEvent queuedEvent, int currentHour, int currentDay)
        {
            int count = _possibleHours.Count;
            if (count == 0) return false;
            
            if (count == 1)
            {
                AssignNewStartTime(queuedEvent, currentDay, _possibleHours.First());
                return true;
            }
            
            bool success = RaidConfig.RaidDistribution.Value switch
            {
                "Evenly" => ChooseNewStartTimeEvenly(queuedEvent, currentHour, currentDay),
                "Stacked" => ChooseNewStartTimeStacked(queuedEvent, currentHour, currentDay),
                _ => ChooseNewStartTimeRandomly(queuedEvent, currentHour, currentDay)
            };
            
            return success;
        }
        
        private static bool ChooseNewStartTimeEvenly(VailWorldEvents.QueuedEvent queuedEvent, int currentHour, int currentDay)
        {
            var available = _possibleHours
                .Where(hour => !UsedHours.Contains(hour) && (!RaidConfig.ConsiderCurrentTime.Value || hour > currentHour))
                .ToList();
            
            if (available.Count == 0) return false;
            
            // Find largest gap between used hours
            var usedWithBounds = UsedHours.Concat(new[] { currentHour, _possibleHours.Max() }).Distinct().OrderBy(h => h).ToList();
            
            int maxGap = 0, gapStart = 0, gapEnd = 0;
            for (int i = 1; i < usedWithBounds.Count; i++)
            {
                int gap = usedWithBounds[i] - usedWithBounds[i - 1];
                if (gap > maxGap && usedWithBounds[i - 1] >= currentHour)
                {
                    maxGap = gap;
                    gapStart = usedWithBounds[i - 1];
                    gapEnd = usedWithBounds[i];
                }
            }
            
            int bestHour = (gapStart + gapEnd) / 2;
            
            if (!available.Contains(bestHour))
            {
                // Find closest available hour
                var closest = available.OrderBy(h => Math.Abs(h - bestHour)).FirstOrDefault();
                if (closest != 0)
                    bestHour = closest;
            }
            
            if (!available.Contains(bestHour)) return false;
            
            AssignNewStartTime(queuedEvent, currentDay, bestHour);
            return true;
        }
        
        private static bool ChooseNewStartTimeStacked(VailWorldEvents.QueuedEvent queuedEvent, int currentHour, int currentDay)
        {
            var effective = GetEffectivePossibleHours(currentHour);
            if (effective.Count == 0) return false;
            
            AssignNewStartTime(queuedEvent, currentDay, effective.First());
            return true;
        }
        
        private static bool ChooseNewStartTimeRandomly(VailWorldEvents.QueuedEvent queuedEvent, int currentHour, int currentDay)
        {
            var effective = GetEffectivePossibleHours(currentHour);
            if (effective.Count == 0) return false;
            
            int chosen = effective[Rnd.Next(effective.Count)];
            AssignNewStartTime(queuedEvent, currentDay, chosen);
            return true;
        }
        
        private static List<int> GetEffectivePossibleHours(int currentHour)
        {
            if (!RaidConfig.ConsiderCurrentTime.Value)
                return new List<int>(_possibleHours);
            return _possibleHours.Where(hour => hour > currentHour).ToList();
        }
        
        private static void AssignNewStartTime(VailWorldEvents.QueuedEvent queuedEvent, int currentDay, int chosenHour)
        {
            queuedEvent._startTimeInHours = currentDay * 24 + chosenHour;
            
            if (RaidConfig.RaidDistribution.Value == "Randomly")
                return;
            
            UsedHours.Add(chosenHour);
            _possibleHours.Remove(chosenHour);
        }
        
        private static SortedSet<int> CalculatePossibleHours(
            Tuple<int, int> morningRange, 
            Tuple<int, int> dayRange, 
            Tuple<int, int> eveningRange, 
            Tuple<int, int> nightRange)
        {
            var hours = new SortedSet<int>();
            
            if (RaidConfig.SearchPartiesAtMorning.Value)
                AddHoursForRange(hours, morningRange);
            if (RaidConfig.SearchPartiesAtDay.Value)
                AddHoursForRange(hours, dayRange);
            if (RaidConfig.SearchPartiesAtEvening.Value)
                AddHoursForRange(hours, eveningRange);
            if (RaidConfig.SearchPartiesAtNight.Value)
                AddHoursForRange(hours, nightRange);
            
            return hours;
        }
        
        private static void AddHoursForRange(ISet<int> possibleHours, Tuple<int, int> range)
        {
            for (int i = range.Item1; i <= range.Item2; i++)
            {
                possibleHours.Add(i);
            }
        }
        
        /// <summary>
        /// Reset for new day
        /// </summary>
        public static void Reset()
        {
            _possibleHours = null;
            UsedHours.Clear();
        }
        
        /// <summary>
        /// Clear all queued raid events
        /// </summary>
        public static void ClearQueuedRaidEvents()
        {
            try
            {
                var world = SingletonBehaviour<VailWorldEvents>._instance;
                if (world == null) return;
                
                var toRemove = new List<VailWorldEvents.QueuedEvent>();
                foreach (var evt in world._queuedEvents)
                {
                    if (EventTools.IsActualSearchParty(evt._event))
                        toRemove.Add(evt);
                }
                
                if (toRemove.Count == 0)
                {
                    RLog.Msg(Color.Orange, "[RaidCustomizer] No events queued");
                    return;
                }
                
                foreach (var evt in toRemove)
                {
                    world._queuedEvents.Remove(evt);
                }
                
                RLog.Msg(Color.Orange, $"[RaidCustomizer] Cleared {toRemove.Count} events");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ClearQueuedRaidEvents failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Get structured list of all queued raids, sorted by start time.
        /// Returns (day, hour, period, name, isBoss) tuples.
        /// </summary>
        public static List<Tuple<int, int, string, string, bool>> GetQueuedRaidList()
        {
            var result = new List<Tuple<int, int, string, string, bool>>();
            try
            {
                var world = SingletonBehaviour<VailWorldEvents>._instance;
                if (world == null) return result;
                
                var sorted = new List<Tuple<float, VailWorldEvents.QueuedEvent>>();
                foreach (var evt in world._queuedEvents)
                {
                    if (EventTools.IsActualSearchParty(evt._event))
                    {
                        sorted.Add(new Tuple<float, VailWorldEvents.QueuedEvent>(evt._startTimeInHours, evt));
                    }
                }
                
                foreach (var (startTime, qEvt) in sorted.OrderBy(t => t.Item1))
                {
                    int day = (int)(startTime / 24f);
                    int hour = (int)(startTime % 24f);
                    string period = GetTimePeriod(hour);
                    string name = qEvt._event?.name ?? "Unknown";
                    bool isBoss = EventTools.IsBossEvent(qEvt._event);
                    result.Add(new Tuple<int, int, string, string, bool>(day, hour, period, name, isBoss));
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] GetQueuedRaidList failed: {ex.Message}");
            }
            return result;
        }
        
        /// <summary>
        /// Map hour to time-of-day period name
        /// </summary>
        private static string GetTimePeriod(int hour)
        {
            if (hour >= 6 && hour < 12) return "Morning";
            if (hour >= 12 && hour < 17) return "Day";
            if (hour >= 17 && hour < 21) return "Evening";
            return "Night";
        }
        
        /// <summary>
        /// Print all queued raids to console with detailed formatting
        /// </summary>
        public static void PrintQueuedRaids()
        {
            try
            {
                var raids = GetQueuedRaidList();
                
                if (raids.Count == 0)
                {
                    RLog.Msg(Color.Orange, "[RaidCustomizer] No raids queued");
#if !SERVER
                    SonsTools.ShowMessage("No raids queued", 5f);
#endif
                    return;
                }
                
                RLog.Msg(Color.Orange, $"[RaidCustomizer] === Queued Raids ({raids.Count}) ===");
                foreach (var (day, hour, period, name, isBoss) in raids)
                {
                    string bossTag = isBoss ? " [BOSS]" : "";
                    string msg = $"Day {day}, {hour:D2}:00 ({period}) — {name}{bossTag}";
                    RLog.Msg(Color.Orange, $"[RaidCustomizer]   {msg}");
#if !SERVER
                    SonsTools.ShowMessage(msg, 10f);
#endif
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] PrintQueuedRaids failed: {ex.Message}");
            }
        }
    }
}
