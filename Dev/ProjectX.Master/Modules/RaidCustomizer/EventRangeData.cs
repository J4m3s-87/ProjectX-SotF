using System;
using System.Collections.Generic;
using Sons.Characters;
using UnityEngine;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Caches hour ranges for event times
    /// </summary>
    internal class EventRangeData
    {
        private readonly Dictionary<VailWorldEventData.TimeOfEvent, Tuple<int, int>> _ranges = new();
        private int _lastDay = -1;
        
        public Tuple<int, int> GetHourRange(VailWorldEventData.TimeOfEvent timeOfEvent, int currentDay)
        {
            if (_lastDay != currentDay)
            {
                _ranges.Clear();
                _lastDay = currentDay;
            }
            
            if (_ranges.TryGetValue(timeOfEvent, out var tuple))
            {
                return tuple;
            }
            
            var hourRange = VailWorldEventData.GetHourRange(timeOfEvent, currentDay);
            int start = hourRange.x;
            int end = hourRange.y < hourRange.x ? hourRange.y + 24 : hourRange.y;
            
            tuple = new Tuple<int, int>(start, end);
            _ranges.Add(timeOfEvent, tuple);
            return tuple;
        }
    }
}
