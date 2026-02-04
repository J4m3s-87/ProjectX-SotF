using Sons.Characters;
using UnityEngine;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Stores original event defaults for reset capability
    /// </summary>
    public class SearchPartyEventDefaults
    {
        public int SpawnCount { get; set; }
        public int SpawnMax { get; set; }
        public int CooldownDays { get; set; }
        public bool EndgameOnly { get; set; }
        public bool ForestOnly { get; set; }
        public Vector2Int MinMaxDay { get; set; }
        public Vector2 MinMaxAnger { get; set; }
        public VailWorldEventData.TimeOfEvent TimeOfEvent { get; set; }
        
        public SearchPartyEventDefaults(
            int spawnCount, 
            int spawnMax, 
            int cooldownDays, 
            bool endgameOnly, 
            bool forestOnly, 
            Vector2Int minMaxDay, 
            Vector2 minMaxAnger, 
            VailWorldEventData.TimeOfEvent timeOfEvent)
        {
            SpawnCount = spawnCount;
            SpawnMax = spawnMax;
            CooldownDays = cooldownDays;
            EndgameOnly = endgameOnly;
            ForestOnly = forestOnly;
            MinMaxDay = minMaxDay;
            MinMaxAnger = minMaxAnger;
            TimeOfEvent = timeOfEvent;
        }
    }
}
