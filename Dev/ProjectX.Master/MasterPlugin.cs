using System;
using RedLoader;
using RedLoader.Utils;
using SonsSdk;
using UnityEngine;
using Bolt;
using HarmonyLib;
using ProjectX.Master.Modules.Network;
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
            Config.Save(); // Flush config to disk immediately so admins can edit it
            LoggerInstance.Msg($"Project X Master Mod Loaded!");
            SdkEvents.OnInWorldUpdate.Subscribe(ManagedOnUpdate);
        }

#pragma warning disable CS0414 // Field is assigned but its value is never used
        private bool _settingsDirty = false;
#pragma warning restore CS0414

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
            
            // 2. StoneGate - REMOVED from build
            // Modules.StoneGate.StoneGateModule.Init();
            // Modules.StoneGate.StoneGateModule.OnSdkInitialized();
            
#if !SERVER
            // 3. Hotbar (client only) — re-enabled with sprite caching + frame throttling
            if (!isDedicated)
            {
                Modules.Hotbar.HotbarModule.Init();
            }
            
            // 4a. AmmoUI (client only)
            Modules.AmmoUI.AmmoUiModule.Init();
            
            // 4b. BuilderStacks (client only — extra log/plank/stone carrying)
            Modules.BuilderStacks.BuilderStacksModule.Init();
#endif
            
#if !SERVER
            // 4. Relocator (client only - structure relocation)
            Modules.Relocator.RelocatorModule.Init();
#endif
            
#if !SERVER
            // 5. Zipline (client only — uses LocalPlayer)
            Modules.Zipline.ZiplineModule.Init();
            
            // 6. PrefabRepair — REMOVED (HarmonyPatchAll vtable corruption)
            // Modules.PrefabRepair.PrefabRepairModule.Init();
#endif
            
            // 7. StructureDurability (server-safe: Harmony patch)
            Modules.StructureDurability.StructureDurabilityModule.Init();
            
            // 8. WaterCollectors (server-safe: Harmony + distance checks)
            Modules.WaterCollectors.WaterCollectorsModule.Init();
            
            // 9. MeatDryer (server-safe: Harmony + Marshal offsets)
            Modules.MeatDryer.MeatDryerModule.Init();
            
#if !SERVER
            // 10. ScaryCross (client only)
            Modules.ScaryCross.ScaryCrossModule.Init();
#endif
            
            // 11. OpenSesame — REMOVED (HarmonyPatchAll vtable corruption)
            // Modules.OpenSesame.OpenSesameModule.Init();
            
            // 12. DedicatedSuperuser (SERVER ONLY — hooks ChatEvent/AddLine on dedicated server)
            //      On the Owner client, these hooks cause local command processing that gets
            //      overridden by the server. The Server DLL initializes this in OnGameStart.
#if SERVER
            Modules.DedicatedSuperuser.DedicatedSuperuserModule.Init();
#endif
            
#if !SERVER
            // 13. Stack (client only - inventory stack sizes)
            Modules.Stack.StackModule.Init();
            
            // 16b. CraftingSpeed - DISABLED (CraftingCog IL2CPP incompatible)
            Modules.Crafting.CraftingSpeed.Init();
#endif
            
            
            // 14. RaidCustomizer (raid control, enemy stats)
            // Note: RaidConfig is now a proxy to Config.cs - no separate SettingsRegistry needed
            Modules.RaidCustomizer.RaidCustomizerModule.Initialize();
            
            // 15. BroadcastMessage (Discord bridge, chat logging)
#if !CLIENT
            Modules.BroadcastMessage.BroadcastMessageModule.Init();
#endif
            
            // 16. Network modules (multi-deployment architecture)
            PermissionSync.Init();
            RoleManager.Init();
            CommandBridge.Init();
            PermissionEvent.Register();
            AdminCommandEvent.Register();
            ConfigSyncEvent.Register();
            IntegrityConfig.Init();
            IntegrityEvent.Register();
            
            // 17. LootRespawn (server-safe: Harmony + tracking)
            Modules.LootRespawn.LootRespawnModule.Init();
            
            LoggerInstance.Msg("Fresh Merge: All modules enabled");
#if SERVER || OWNER
            ConfigSyncPayload.EnableBroadcast();
#endif
        }

        protected override void OnGameStart()
        {
            LoggerInstance.Msg("Game Started - Project X Active (Fresh Merge Mode)");
            // Modules.StoneGate.StoneGateModule.OnGameStart(); // REMOVED
#if !SERVER
            Modules.Hotbar.HotbarModule.OnGameStart();
            Modules.AmmoUI.AmmoUiModule.ApplyHarmonyPatch(HarmonyInstance);
#endif

#if CLIENT
            // Defer config/permission requests — Bolt isn't connected yet at OnGameStart.
            // ManagedOnUpdate will poll until BoltNetwork is ready.
            _clientNeedsSync = true;
            LoggerInstance.Msg("[Client] Config sync deferred — waiting for Bolt connection");
#endif

#if SERVER
            // =============================================
            // SERVER-ONLY: OnSdkInitialized does NOT fire on headless servers.
            // All server-safe modules must initialize here in OnGameStart instead.
            // =============================================
            LoggerInstance.Msg("[Server] Initializing server modules in OnGameStart...");
            
            try { Modules.StructureDurability.StructureDurabilityModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] StructureDurability init failed: {ex.Message}"); }
            
            try { Modules.WaterCollectors.WaterCollectorsModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] WaterCollectors init failed: {ex.Message}"); }
            
            try { Modules.MeatDryer.MeatDryerModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] MeatDryer init failed: {ex.Message}"); }
            
            try { Modules.DedicatedSuperuser.DedicatedSuperuserModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] DedicatedSuperuser init failed: {ex.Message}"); }
            
            try { Modules.RaidCustomizer.RaidCustomizerModule.Initialize(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] RaidCustomizer init failed: {ex.Message}"); }
            
            try { Modules.BroadcastMessage.BroadcastMessageModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] BroadcastMessage init failed: {ex.Message}"); }
            
            try { PermissionSync.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] PermissionSync init failed: {ex.Message}"); }
            
            try { RoleManager.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] RoleManager init failed: {ex.Message}"); }
            
            try { CommandBridge.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] CommandBridge init failed: {ex.Message}"); }
            
            try { PermissionEvent.Register(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] PermissionEvent init failed: {ex.Message}"); }
            
            try { AdminCommandEvent.Register(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] AdminCommandEvent init failed: {ex.Message}"); }
            
            try { ConfigSyncEvent.Register(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] ConfigSyncEvent init failed: {ex.Message}"); }
            
            try { IntegrityConfig.Init(); IntegrityEvent.Register(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] IntegrityEvent init failed: {ex.Message}"); }
            
            // Server-side tick driver: SdkEvents.OnInWorldUpdate does NOT fire on headless servers
            // (documented in MeatDryerModule). Use SeasonsManager.LateUpdate as proven alternative.
            try
            {
                var lateUpdate = AccessTools.Method(typeof(Sons.Atmosphere.SeasonsManager), "LateUpdate");
                var tickPostfix = AccessTools.Method(typeof(ProjectXMaster), nameof(ServerTickPostfix));
                if (lateUpdate != null && tickPostfix != null)
                {
                    var harmony = new HarmonyLib.Harmony("ProjectX.ServerTick");
                    harmony.Patch(lateUpdate, postfix: new HarmonyMethod(tickPostfix));
                    RLog.Msg("[MasterPlugin] Hooked SeasonsManager.LateUpdate for server tick");
                }
                else
                {
                    RLog.Warning("[MasterPlugin] SeasonsManager.LateUpdate hook failed — server polling disabled");
                }
            }
            catch (Exception ex) { LoggerInstance.Error($"[Server] ServerTick hook failed: {ex.Message}"); }
            
            try { Modules.LootRespawn.LootRespawnModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] LootRespawn init failed: {ex.Message}"); }
            
            ConfigSyncPayload.EnableBroadcast();
            
            LoggerInstance.Msg("[Server] All server modules initialized");
#endif
        }

#if CLIENT
        private static bool _clientNeedsSync = false;
#endif

        private void ManagedOnUpdate()
        {
            try
            {
                // LootRespawn polls on ALL tiers (before batch mode check)
                try { Modules.LootRespawn.LootRespawnModule.OnUpdate(); } catch { }
                
                // MeatDryer polls on ALL tiers — OnGUI() doesn't fire on headless servers
                try { Modules.MeatDryer.MeatDryerModule.OnGuiTick(); } catch { }
                
#if SERVER || OWNER
                // Server-side polling — must run before isBatchMode guard
                // (headless servers return early at that check)
                try { ConfigSyncPayload.Update(); } catch { }
                try { IntegrityEvent.CheckTimeouts(); }
                catch (System.Exception ex)
                {
                    RLog.Warning($"[MasterPlugin] CheckTimeouts crashed: {ex}");
                }
#endif
                
                if (UnityEngine.Application.isBatchMode) return;
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[MasterPlugin] ManagedOnUpdate error: {ex.Message}");
            }

#if CLIENT
            // Deferred config/permission request — polls until Bolt is connected
            if (_clientNeedsSync && BoltNetwork.isRunning && BoltNetwork.isClient)
            {
                _clientNeedsSync = false;
                try
                {
                    PermissionEvent.Instance?.RequestPermissions();
                    ConfigSyncEvent.Instance?.RequestConfig();
                    LoggerInstance.Msg("[Client] Sent config + permission requests to server (deferred)");
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warning($"[Client] Deferred sync failed: {ex.Message}");
                    _clientNeedsSync = true; // Retry next frame
                }
            }
#endif
            

            
#if !SERVER
            // Hotbar update loop
            Modules.Hotbar.HotbarModule.OnUpdate();
            
            // AmmoUI update loop
            Modules.AmmoUI.AmmoUiModule.OnUpdate();
            
            // BuilderStacks update loop
            Modules.BuilderStacks.BuilderStacksModule.OnUpdate();
            
            // Player actions update (NoClip fly movement)
            Modules.UI.PlayerActions.OnUpdate();
            
            // StoneGate - REMOVED
            // Modules.StoneGate.StoneGateModule.EnsureUIHidden();
#endif
        }

#if SERVER || OWNER
        /// <summary>
        /// Server-side tick via SeasonsManager.LateUpdate (Harmony postfix).
        /// SdkEvents.OnInWorldUpdate does NOT fire on headless servers.
        /// SeasonsManager.LateUpdate is proven to run every frame on all tiers.
        /// </summary>
        private static void ServerTickPostfix()
        {
            try { ConfigSyncPayload.Update(); } catch { }
            try { IntegrityEvent.CheckTimeouts(); } catch { }
        }
#endif
    }
}
