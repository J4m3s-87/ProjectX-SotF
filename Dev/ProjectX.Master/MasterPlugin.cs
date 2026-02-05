using RedLoader;
using RedLoader.Utils;
using SonsSdk;
using UnityEngine;
#if !SERVER
using SonsAxLib;
using SUI;
#endif

namespace ProjectX.Master
{
    public class ProjectXMaster : SonsMod
    {
        public static ProjectXMaster Instance { get; private set; }

        public ProjectXMaster()
        {
            // DISABLED: HarmonyPatchAll causing CLR crash in IL2CPP
            this.HarmonyPatchAll = false;
        }

        protected override void OnInitializeMod()
        {
            Instance = this;
            Config.Init();
            LoggerInstance.Msg($"Project X Master Mod Loaded!");
            SdkEvents.OnInWorldUpdate.Subscribe(ManagedOnUpdate);
        }

        private bool _settingsDirty = false;

        protected override void OnSdkInitialized()
        {
            // Register settings with the in-game Mods menu (client only)
#if !SERVER
            // Using a unique ID (NOT null) to avoid the "Period Trap" - manifest ID contains a period
            // which causes the [ProjectX] Master = { } ghost stub when using null
            SettingsRegistry.CreateSettings(this, "ProjectX_Settings", typeof(Config), false, null);
#endif
            
            // Detect Dedicated Server (Headless)
            bool isDedicated = UnityEngine.Application.isBatchMode;
            LoggerInstance.Msg($"Mode: {(isDedicated ? "Dedicated Server (Headless)" : "Client (Graphical)")}");

            // =============================================
            // FRESH MERGE: Enable modules one at a time
            // =============================================
            
            // 1. UI/AxelModMenu (IMGUI rewrite - original SUI crashes IL2CPP)
#if !SERVER
            Modules.UI.ProjectXUI.Init();
#endif
            
            // 2. StoneGate
            Modules.StoneGate.StoneGateModule.Init();
            Modules.StoneGate.StoneGateModule.OnSdkInitialized();
            
#if !SERVER
            // 3. Hotbar (client only) - DISABLED: Causes Tab crash
            // if (!isDedicated)
            // {
            //     Modules.Hotbar.HotbarModule.Init();
            // }
#endif
            
#if !SERVER
            // 4. Relocator (client only - structure relocation)
            Modules.Relocator.RelocatorModule.Init();
#endif
            
            // 5. Zipline (always-on)
            Modules.Zipline.ZiplineModule.Init();
            
            // 6. PrefabRepair (polling mode)
            Modules.PrefabRepair.PrefabRepairModule.Init();
            
            // 7. StructureDurability
            Modules.StructureDurability.StructureDurabilityModule.Init();
            
            // 8. WaterCollectors
            Modules.WaterCollectors.WaterCollectorsModule.Init();
            
            // 9. MeatDryer (season-based drying boost)
            Modules.MeatDryer.MeatDryerModule.Init();
            
            // 10. ScaryCross 
            Modules.ScaryCross.ScaryCrossModule.Init();
            
            // 11. OpenSesame
            Modules.OpenSesame.OpenSesameModule.Init();
            
            // 12. DedicatedSuperuser
            Modules.DedicatedSuperuser.DedicatedSuperuserModule.Init();
            
#if !SERVER
            // 13. Stack (client only - inventory stack sizes)
            Modules.Stack.StackModule.Init();
#endif
            
            // 14. RaidCustomizer (raid control, enemy stats)
            // Note: RaidConfig is now a proxy to Config.cs - no separate SettingsRegistry needed
            Modules.RaidCustomizer.RaidCustomizerModule.Initialize();
            
            // 15. BroadcastMessage (Discord bridge, chat logging)
            Modules.BroadcastMessage.BroadcastMessageModule.Init();
            
            LoggerInstance.Msg("Fresh Merge: All modules enabled");
        }

        protected override void OnGameStart()
        {
            LoggerInstance.Msg("Game Started - Project X Active (Fresh Merge Mode)");
            Modules.StoneGate.StoneGateModule.OnGameStart();
#if !SERVER
            // Modules.Hotbar.HotbarModule.OnGameStart();  // DISABLED: Causes Tab crash
#endif
        }

        private void ManagedOnUpdate()
        {
            if (UnityEngine.Application.isBatchMode) return;
            
#if !SERVER
            // Hotbar update loop - DISABLED: Causes Tab crash
            // Modules.Hotbar.HotbarModule.OnUpdate();
            
            // Player actions update (NoClip fly movement)
            Modules.UI.PlayerActions.OnUpdate();
            
            // StoneGate UI safety check - ensure UI is hidden when tool not equipped
            Modules.StoneGate.StoneGateModule.EnsureUIHidden();
#endif
        }
    }
}
