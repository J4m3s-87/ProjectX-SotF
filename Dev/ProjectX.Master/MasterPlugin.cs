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

#if SERVER || OWNER
        // Player-presence tracking for server tick idling
        // Updated by WelcomeHandler connect/disconnect hooks
        private static int _activePlayerCount = 0;
        internal static bool _hasActivePlayers => _activePlayerCount > 0;
        
        public static void OnPlayerConnected()
        {
            _activePlayerCount++;
            Instance?.LoggerInstance.Msg($"[MasterPlugin] Player connected (active: {_activePlayerCount})");
        }
        
        public static void OnPlayerDisconnected()
        {
            _activePlayerCount = System.Math.Max(0, _activePlayerCount - 1);
            Instance?.LoggerInstance.Msg($"[MasterPlugin] Player disconnected (active: {_activePlayerCount})");
        }
#endif

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
            
#if !CLIENT
            // 4c. AudioControl (owner/solo only — Harmony tracking for waterfall volume)
            Modules.Audio.AudioControl.Init();
#endif
            
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
            
            // 7. StructureDurability (client/owner: Harmony patch on GetStructureInfo)
#if !SERVER
            Modules.StructureDurability.StructureDurabilityModule.Init();
#endif
            
            // 8. WaterCollectors (server-safe: Harmony + distance checks)
            Modules.WaterCollectors.WaterCollectorsModule.Init();
            
            // 9. MeatDryer (server-safe: Harmony + Marshal offsets)
            Modules.MeatDryer.MeatDryerModule.Init();
            
            // 10. ScaryCross (server-safe: Harmony + distance checks + IgniteSelf)
            Modules.ScaryCross.ScaryCrossModule.Init();
            
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
            
            // 16c. BuilderEnhancements Harmony patch (InstantBuild via ConfigSync)
            Modules.Building.BuilderEnhancements.InitPatches();
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
            LootSyncEvent.Register();
            IntegrityConfig.Init();
            IntegrityEvent.Register();
            
            // 17. LootRespawn (server-safe: Harmony + tracking)
            // TEMP DISABLED for crash isolation
            try { Modules.LootRespawn.LootRespawnModule.Init(); }
            catch (Exception ex) { LoggerInstance.Error($"[LootRespawn] Init CRASHED: {ex}"); }
            
            // 18. WeaponDamage (client/owner: damage multipliers, inspect, solafite)
#if !SERVER
            Modules.WeaponDamage.WeaponDamageModule.Init();
#endif
            
            LoggerInstance.Msg("Fresh Merge: All modules enabled");
#if SERVER || OWNER
            ConfigSyncPayload.EnableBroadcast();
#endif
        }

        protected override void OnSonsSceneInitialized(SonsSdk.ESonsScene sonsScene)
        {
            // Reset LootRespawn deferred check state BEFORE PickUp.Awake fires during scene load
            try { Modules.LootRespawn.LootRespawnModule.OnSceneInit(); }
            catch (Exception ex) { LoggerInstance.Error($"[LootRespawn] OnSceneInit FAILED: {ex}"); }
        }

        protected override void OnGameStart()
        {
            LoggerInstance.Msg("Game Started - Project X Active (Fresh Merge Mode)");
            // Modules.StoneGate.StoneGateModule.OnGameStart(); // REMOVED
#if !SERVER
            Modules.Hotbar.HotbarModule.OnGameStart();
            Modules.AmmoUI.AmmoUiModule.ApplyHarmonyPatch(HarmonyInstance);
            
            // WeaponDamage: apply all damage settings on game start
            Modules.WeaponDamage.WeaponDamageModule.ApplyAllDamageSettings();
#endif
            // LootRespawn: process deferred items now that save data is loaded
            try { Modules.LootRespawn.LootRespawnModule.OnGameStarted(); }
            catch (Exception ex) { LoggerInstance.Error($"[LootRespawn] OnGameStarted FAILED: {ex}"); }

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
            
            // StructureDurability is now client/owner only (#if !SERVER)
            // try { Modules.StructureDurability.StructureDurabilityModule.Init(); } 
            // catch (Exception ex) { LoggerInstance.Error($"[Server] StructureDurability init failed: {ex.Message}"); }
            
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
            
            try { LootSyncEvent.Register(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] LootSyncEvent init failed: {ex.Message}"); }
            
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
            
            try { Modules.ScaryCross.ScaryCrossModule.Init(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] ScaryCross init failed: {ex.Message}"); }
            
            try { Modules.ScaryCross.ScaryCrossModule.OnGameActivated(); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] ScaryCross OnGameActivated failed: {ex.Message}"); }
            
            try { Modules.WeaponDamage.WeaponDamagePatches.ApplyPatch(new HarmonyLib.Harmony("ProjectX.WeaponDamage.Server")); } 
            catch (Exception ex) { LoggerInstance.Error($"[Server] WeaponDamage patch failed: {ex.Message}"); }
            
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
                    LootSyncEvent.Instance?.RequestState();
                    LoggerInstance.Msg("[Client] Sent config + permission + loot state requests to server (deferred)");
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
            
            // IntegrityEvent: process deferred client response (ChatBox may not be ready during loading)
            try { IntegrityEvent.ProcessPendingResponse(); } catch { }
            
            // WeaponDamage: per-frame held weapon damage adjustment + inspect keybind
            try { Modules.WeaponDamage.WeaponDamageModule.OnWorldUpdate(); } catch { }
            
            // StoneGate - REMOVED
            // Modules.StoneGate.StoneGateModule.EnsureUIHidden();
#endif
        }

#if SERVER || OWNER
        /// <summary>
        /// Server-side tick via SeasonsManager.LateUpdate (Harmony postfix).
        /// SdkEvents.OnInWorldUpdate does NOT fire on headless servers.
        /// SeasonsManager.LateUpdate is proven to run every frame on all tiers.
        /// 
        /// Player-presence guard: skip ticking when no players are connected
        /// to prevent 24/7 world simulation (NPC buildup, wasted resources).
        /// </summary>
        private static float _lastInstantBuildPoll = 0f;
        
        private static void ServerTickPostfix()
        {
            // Skip server tick when no players are connected — let the server idle
            // NOTE: BoltNetwork.connections is IL2CPP — foreach/Linq don't work reliably
            // Use _hasPlayers flag updated by WelcomeHandler connect/disconnect hooks
            try
            {
                if (!BoltNetwork.isRunning) return;
                if (!_hasActivePlayers) return;
            }
            catch { return; }
            
            try { ConfigSyncPayload.Update(); } catch { }
            try { IntegrityEvent.CheckTimeouts(); } catch { }
            try { Modules.ScaryCross.ScaryCrossModule.ServerTick(); } catch { }
            
            // InstantBookBuild: auto-complete blueprints every 0.5s via finishblueprints
            try
            {
                if (Config.InstantBookBuild?.Value == true)
                {
                    float now = UnityEngine.Time.time;
                    if (now - _lastInstantBuildPoll >= 0.5f)
                    {
                        _lastInstantBuildPoll = now;
                        Modules.DedicatedSuperuser.Commands.WorldCommands.TrySendDebugCommand("finishblueprints", quiet: true);
                    }
                }
            }
            catch { }
        }
#endif
    }
}
