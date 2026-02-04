using System;
using Il2CppInterop.Runtime;
using Sons.Characters;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Utilities for detecting and classifying raid events
    /// </summary>
    public static class EventTools
    {
        private static Il2CppSystem.Type _searchPartyEventType;
        
        /// <summary>
        /// Get the SearchPartyEvent type lazily
        /// </summary>
        private static Il2CppSystem.Type SearchPartyEventType
        {
            get
            {
                if (_searchPartyEventType == null)
                    _searchPartyEventType = Il2CppType.Of<VailWorldEventData.SearchPartyEvent>();
                return _searchPartyEventType;
            }
        }
        
        /// <summary>
        /// Check if object is a SearchPartyEvent type
        /// </summary>
        private static bool IsSearchPartyEventType(Il2CppSystem.Object evt)
        {
            if (evt == null) return false;
            try
            {
                return evt.GetIl2CppType().Equals(SearchPartyEventType);
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
