using SonsSdk;
using RedLoader;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    public static class TimeCommands
    {
        // Example: "season summer"
        public static void SetSeason(string season)
        {
             RLog.Msg($"Setting Season to: {season}");
             // Sons.Environment.SeasonManager.Self.SetSeason(...)
        }

        public static void SetTimeOfDay(float hour)
        {
             RLog.Msg($"Setting Time to: {hour}");
             // Sons.Environment.TimeOfDay.Self.SetTime(hour);
        }
    }
}
