using RedLoader;
using SonsSdk;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ProjectX.Master.Modules.Compatibility
{
    public static class CompatibilityModule
    {
        public static void Init()
        {
            SyncManager.Init();
        }

        public static void UpdateSettings()
        {
            // Re-sync logic if needed
        }
    }

    public static class SyncManager
    {
        public static bool IsFrankyModMenuInstalled { get; private set; }
        public static bool IsTrueSurvivalInstalled { get; private set; }

        public static void Init()
        {
            DetectMods();
            SyncTrueSurvival();
        }

        private static void DetectMods()
        {
            foreach (var mod in ModTypeBase<SonsMod>.RegisteredMods)
            {
                if (mod.ID == "FrankyModMenu") IsFrankyModMenuInstalled = true;
                if (mod.ID == "TrueSurvival") IsTrueSurvivalInstalled = true;
            }
            ProjectXMaster.Instance.LoggerInstance.Msg($"Compatibility: FrankyModMenu({IsFrankyModMenuInstalled}), TrueSurvival({IsTrueSurvivalInstalled})");
        }

        private static void SyncTrueSurvival()
        {
            if (IsTrueSurvivalInstalled)
            {
                // Example: If TrueSurvival wants to override stacks, we might want to respect or overwrite
                // For now, we apply basic logging
                ProjectXMaster.Instance.LoggerInstance.Msg("Applying TrueSurvival Compatibility Rules...");
            }
        }
    }
}
