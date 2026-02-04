using RedLoader;
using System.Collections.Generic;

namespace ProjectX.Master.Modules.StoneGate
{
    public static class Config
    {
        public static ConfigEntry<bool> LoggingToConsole;
        public static List<string> allowedHits = new List<string> { "Structure", "Construction", "Foundation", "Wall", "Floor", "Roof", "Stairs", "Ramp" };

        static Config()
        {
            var category = ConfigSystem.CreateFileCategory("StoneGate", "StoneGate Settings", "StoneGate.cfg");
            LoggingToConsole = category.CreateEntry("LoggingToConsole", true, "Enable Console Logging", "Log debug messages to console");
        }
    }
}
