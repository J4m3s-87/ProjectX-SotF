using System;
using System.Reflection;
using Sons.Characters;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Utilities for detecting and classifying raid events
    /// Refactored for server compatibility — uses reflection for IL2CPP type checks
    /// that fail on DummyDll (internal members lose Il2CppObjectBase inheritance).
    /// </summary>
    public static class EventTools
    {
        private static Il2CppSystem.Type _searchPartyEventType;
        
        private static Il2CppSystem.Type SearchPartyEventType
        {
            get
            {
                if (_searchPartyEventType == null)
                    _searchPartyEventType = Il2CppInterop.Runtime.Il2CppType.Of<VailWorldEventData.SearchPartyEvent>();
                return _searchPartyEventType;
            }
        }
        
        /// <summary>
        /// Check if object is a SearchPartyEvent type (IL2CPP type check — works on both client and server)
        /// NOTE: C# 'is' operator DOES NOT WORK on IL2CPP proxy types — always returns false.
        /// Must use Il2CppType comparison instead.
        /// </summary>
        private static bool IsSearchPartyEventType(VailWorldEventData.EventBase evt)
        {
            if (evt == null) return false;
            try
            {
                return ((Il2CppSystem.Object)evt).GetIl2CppType().Equals(SearchPartyEventType);
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Check if event is an actual raid (search party)
        /// TypeOfEvent: 0=Cannibal, 1=Creepy, 3=Muddy
        /// </summary>
        public static bool IsActualSearchParty(VailWorldEventData.EventBase evt)
        {
            if (!IsSearchPartyEventType(evt)) return false;
            
            var type = evt.type;
            // TypeOfEvent.Cannibal = 0, Creepy = 1, Muddy = 3
            return (int)type <= 1 || (int)type == 3;
        }
        
        /// <summary>
        /// Check if event is a boss raid (single spawn = boss)
        /// </summary>
        public static bool IsBossEvent(VailWorldEventData.EventBase evt)
        {
            if (!IsSearchPartyEventType(evt)) return false;
            try
            {
                // Use .Cast<> for IL2CPP type conversion — 'as' operator doesn't work on IL2CPP proxy types
                var defaults = SearchPartyEventConfigurator.GetOrCreateDefaults(evt.Cast<VailWorldEventData.SearchPartyEvent>());
                return IsBossEvent(defaults);
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Check if defaults represent a boss event
        /// </summary>
        public static bool IsBossEvent(SearchPartyEventDefaults defaults)
        {
            return defaults.SpawnCount == 1 && defaults.SpawnMax == 1;
        }
    }
}
