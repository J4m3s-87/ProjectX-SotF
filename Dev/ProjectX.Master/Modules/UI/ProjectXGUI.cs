#if !SERVER
using System;
using System.Collections.Generic;
using RedLoader;
using Il2CppInterop.Runtime.Injection;
using SonsSdk;
using TheForest.Utils;
using UnityEngine;
using ProjectX.Master.Modules.Player;
using ProjectX.Master.Modules.Network;

namespace ProjectX.Master.Modules.UI
{
    /// <summary>
    /// Main GUI MonoBehaviour for Project X Menu
    /// Uses Unity IMGUI (OnGUI) instead of SUI to avoid IL2CPP crashes
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class ProjectXGUI : MonoBehaviour
    {
        public static ProjectXGUI Instance { get; private set; }
        
        // State
        private bool _showMenu = false;
        private int _currentPanel = 0;
        private readonly string[] _panelNames = { "Player", "Environment", "Teleport", "System", "Misc", "Raids" };
        
        // Layout dimensions (2x size for better readability)
        private const float PANEL_WIDTH = 900f;
        private const float PANEL_HEIGHT = 900f;
        private const float SELECTOR_WIDTH = 600f;
        private const float SELECTOR_HEIGHT = 60f;
        
        private Rect _selectorRect;
        private Rect _panelRect;
        private Vector2 _scrollPosition;
        
        // Teleport input
        private string _teleportInput = "";
        private string _timeInput = "12:00";
        private string _speedInput = "1";
        private string _stackInput = "999";
        
        // Teleport locations (matches Axel's)
        private static readonly Dictionary<string, Vector3> TeleportLocations = new Dictionary<string, Vector3>
        {
            { "Shotgun Grave", new Vector3(-1340, 102, 1412) },
            { "Pistol Location", new Vector3(-1797, 16, 578) },
            { "Hang Glider", new Vector3(-1307, 87, 1732) },
            { "Knight V", new Vector3(-1026, 231, -625) },
            { "Mountain Top", new Vector3(4, 716, -459) },
            { "Rebreather Cave", new Vector3(-418, 19, 1532) },
            { "Flashlight", new Vector3(-630, 142, 391) },
            { "Modern Axe", new Vector3(-704, 108, 450) },
            { "Machete", new Vector3(-65, 20, 1458) },
            { "Stun Baton", new Vector3(-1142, 134, -157) },
            { "Shovel Cave", new Vector3(-531, 200, 124) },
            { "Rope Gun Cave", new Vector3(-1113, 132, -171) },
            { "Crossbow Bunker", new Vector3(-1014, 102, 1024) },
            { "End Game Bunker", new Vector3(1756, 45, 553) }
        };

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Position: panel on left, selector centered above panel
            _panelRect = new Rect(
                20f,
                100f,
                PANEL_WIDTH,
                PANEL_HEIGHT
            );
            
            _selectorRect = new Rect(
                _panelRect.x + (PANEL_WIDTH - SELECTOR_WIDTH) / 2f,
                40f,
                SELECTOR_WIDTH,
                SELECTOR_HEIGHT
            );
            
            RLog.Msg("[ProjectXGUI] Awake complete");
        }

        void Update()
        {
            // Keybind is handled via ModInputCache.Notify in ProjectXUI
            // No need to check here
        }
        
        public void ToggleMenuPublic()
        {
            ToggleMenu();
        }
        
        private void ToggleMenu()
        {
#if CLIENT
            // Client edition: check permission before allowing menu
            if (!PermissionSync.HasMenuAccess)
            {
                RLog.Msg("[ProjectXGUI] Menu access denied by server");
                return;
            }
#endif
            _showMenu = !_showMenu;
            
            // Use SonsTools.MenuMode for proper input blocking (same as SUI panels)
            SonsTools.MenuMode(_showMenu);
            
            if (_showMenu)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void OnGUI()
        {
            if (!_showMenu) return;
#if CLIENT
            // Safety fallback: hide menu if permission revoked mid-session
            if (!PermissionSync.HasMenuAccess)
            {
                _showMenu = false;
                return;
            }
#endif
            
            try
            {
                // Initialize styles on first OnGUI call
                ProjectXStyles.Initialize();
                
                // Draw dark backgrounds using Texture2D.whiteTexture (IL2CPP safe)
                var prevColor = GUI.color;
                GUI.color = ProjectXStyles.PanelColor;
                GUI.DrawTexture(_selectorRect, Texture2D.whiteTexture);
                GUI.DrawTexture(_panelRect, Texture2D.whiteTexture);
                GUI.color = prevColor;
                
                // Draw selector bar (top center)
                DrawSelectorBar();
                
                // Draw active panel (left side)
                DrawPanel();
            }
            catch (Exception ex)
            {
                // Log only once to avoid spam
                if (!_loggedError)
                {
                    RLog.Error($"[ProjectXGUI] OnGUI Error: {ex.Message}");
                    _loggedError = true;
                }
            }
        }
        
        private bool _loggedError = false;
        
        private void DrawSelectorBar()
        {
            GUILayout.BeginArea(_selectorRect, ProjectXStyles.SelectorBar);
            GUILayout.BeginHorizontal();
            
            // Left arrow
            if (GUILayout.Button("←", ProjectXStyles.ArrowButton))
            {
                if (_currentPanel > 0)
                {
                    _currentPanel--;
                    _scrollPosition = Vector2.zero;
                }
            }
            
            GUILayout.FlexibleSpace();
            
            // Panel name
            GUILayout.Label(_panelNames[_currentPanel], ProjectXStyles.HeaderLabel);
            
            GUILayout.FlexibleSpace();
            
            // Right arrow
            if (GUILayout.Button("→", ProjectXStyles.ArrowButton))
            {
                if (_currentPanel < _panelNames.Length - 1)
                {
                    _currentPanel++;
                    _scrollPosition = Vector2.zero;
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        
        private void DrawPanel()
        {
            GUILayout.BeginArea(_panelRect, ProjectXStyles.PanelBackground);
            
            // Menu title
            GUILayout.Label("PROJECT X MOD MENU", ProjectXStyles.TitleLabel);
            GUILayout.Space(5);
            
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, ProjectXStyles.ScrollView);
            
            switch (_currentPanel)
            {
                case 0: DrawPlayerPanel(); break;
                case 1: DrawEnvironmentPanel(); break;
                case 2: DrawTeleportPanel(); break;
                case 3: DrawSystemPanel(); break;
                case 4: DrawMiscPanel(); break;
                case 5: DrawRaidsPanel(); break;
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        #region Panel Drawings
        
        private void DrawPlayerPanel()
        {
            // Stats section
            DrawDivider("STATS");
            
            // Checkboxes that trigger actions when changed
            bool newGodMode = DrawCheckbox("God Mode", Config.IsGodMode.Value);
            if (newGodMode != Config.IsGodMode.Value) PlayerActions.ToggleGodMode(newGodMode);
            
            bool newInfStamina = DrawCheckbox("Infinite Stamina", Config.IsInfStamina.Value);
            if (newInfStamina != Config.IsInfStamina.Value) PlayerActions.ToggleInfStamina(newInfStamina);
            
            bool newNoHunger = DrawCheckbox("No Hunger", Config.IsNoHungry.Value);
            if (newNoHunger != Config.IsNoHungry.Value) PlayerActions.ToggleNoHunger(newNoHunger);
            
            bool newNoThirst = DrawCheckbox("No Thirst", Config.IsNoDehydration.Value);
            if (newNoThirst != Config.IsNoDehydration.Value) PlayerActions.ToggleNoDehydration(newNoThirst);
            
            bool newNoSleep = DrawCheckbox("No Sleep", Config.IsNoSleep.Value);
            if (newNoSleep != Config.IsNoSleep.Value) PlayerActions.ToggleNoSleep(newNoSleep);
            
            bool newInfAmmo = DrawCheckbox("Infinite Ammo (NYI)", Config.IsInfiniteAmmo.Value);
            if (newInfAmmo != Config.IsInfiniteAmmo.Value) PlayerActions.ToggleInfAmmo(newInfAmmo);
            
            bool newNoFall = DrawCheckbox("No Fall Damage", Config.IsNoFallDamage.Value);
            if (newNoFall != Config.IsNoFallDamage.Value) PlayerActions.ToggleNoFallDamage(newNoFall);
            
            // Infinite Items - apply immediately when changed
            bool prevInfinite = Config.InfiniteInventory.Value;
            Config.InfiniteInventory.Value = DrawCheckbox("Infinite Items", Config.InfiniteInventory.Value);
            if (Config.InfiniteInventory.Value != prevInfinite)
            {
                Modules.Stack.StackModule.Apply();
                RLog.Msg($"[ProjectXGUI] Infinite Items: {Config.InfiniteInventory.Value}");
            }
            
            // NoClip section
            DrawDivider("NO CLIP");
            
            bool newNoClip = DrawCheckbox("NoClip / Fly Hack", Config.IsNoClip.Value);
            if (newNoClip != Config.IsNoClip.Value) PlayerActions.ToggleNoClip(newNoClip);
            Config.NoClipSpeed.Value = DrawSlider("Speed", Config.NoClipSpeed.Value, 1f, 100f);
            Config.NoClipUpDownSpeed.Value = DrawSlider("UpDown Speed", Config.NoClipUpDownSpeed.Value, 0.1f, 5f);
            
            // Movement section
            DrawDivider("MOVEMENT");
            
            float walkSpeed = DrawSlider("Walk Speed", Config.WalkSpeed.Value, 1f, 50f);
            if (Math.Abs(walkSpeed - Config.WalkSpeed.Value) > 0.01f)
            {
                Config.WalkSpeed.Value = walkSpeed;
                PlayerActions.SetWalkSpeed(walkSpeed);
            }
            
            float runSpeed = DrawSlider("Run Speed", Config.RunSpeed.Value, 1f, 50f);
            if (Math.Abs(runSpeed - Config.RunSpeed.Value) > 0.01f)
            {
                Config.RunSpeed.Value = runSpeed;
                PlayerActions.SetRunSpeed(runSpeed);
            }
            
            float swimSpeed = DrawSlider("Swim Speed", Config.SwimSpeed.Value, 1f, 50f);
            if (Math.Abs(swimSpeed - Config.SwimSpeed.Value) > 0.01f)
            {
                Config.SwimSpeed.Value = swimSpeed;
                PlayerActions.SetSwimSpeed(swimSpeed);
            }
            
            float jumpMult = DrawSlider("Jump Multiplier", Config.JumpMultiplier.Value, 1f, 20f);
            if (Math.Abs(jumpMult - Config.JumpMultiplier.Value) > 0.01f)
            {
                Config.JumpMultiplier.Value = jumpMult;
                PlayerActions.SetJumpMultiplier(jumpMult);
            }
            
            // Companions section
            DrawDivider("COMPANIONS");
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Unstuck Kelvin", ProjectXStyles.Button))
            {
                PlayerModule.UnstuckKelvin();
            }
            if (GUILayout.Button("Unstuck Virginia", ProjectXStyles.Button))
            {
                PlayerModule.UnstuckVirginia();
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawEnvironmentPanel()
        {
            DrawDivider("TERRAIN");
            
            // Gravity toggle
            bool newNoGravity = DrawCheckbox("No Gravity", Config.IsNoGravity.Value);
            if (newNoGravity != Config.IsNoGravity.Value) PlayerActions.ToggleNoGravity(newNoGravity);
            
            // Grass/Forest buttons (toggles, no state tracking)
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Grass", ProjectXStyles.Button)) PlayerActions.ToggleNoGrass();
            if (GUILayout.Button("Toggle Forest", ProjectXStyles.Button)) PlayerActions.ToggleNoForest();
            GUILayout.EndHorizontal();
            
            DrawDivider("SEASONS");
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spring", ProjectXStyles.Button)) PlayerActions.SetSeason("Spring");
            if (GUILayout.Button("Summer", ProjectXStyles.Button)) PlayerActions.SetSeason("Summer");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Autumn", ProjectXStyles.Button)) PlayerActions.SetSeason("Autumn");
            if (GUILayout.Button("Winter", ProjectXStyles.Button)) PlayerActions.SetSeason("Winter");
            GUILayout.EndHorizontal();
            
            DrawDivider("TIME");
            
            // Set Time of Day input (e.g., "12:00", "18:30")
            GUILayout.BeginHorizontal();
            GUILayout.Label("Set Time:", ProjectXStyles.NormalLabel, GUILayout.Width(70));
            _timeInput = GUILayout.TextField(_timeInput, ProjectXStyles.InputField, GUILayout.Width(80));
            if (GUILayout.Button("Set", ProjectXStyles.Button, GUILayout.Width(60)))
            {
                PlayerActions.SetTimeOfDay(_timeInput);
            }
            GUILayout.EndHorizontal();
            
            // Lock Time toggle
            bool newLockTime = DrawCheckbox("Lock Time of Day", Config.IsLockTime.Value);
            if (newLockTime != Config.IsLockTime.Value) PlayerActions.ToggleLockTime(newLockTime);
            
            // Daytime Speed - button-based to avoid rapid reflection calls
            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed:", ProjectXStyles.NormalLabel, GUILayout.Width(50));
            _speedInput = GUILayout.TextField(_speedInput, ProjectXStyles.InputField, GUILayout.Width(50));
            if (GUILayout.Button("Set", ProjectXStyles.Button, GUILayout.Width(40)))
            {
                if (float.TryParse(_speedInput, out float speed))
                {
                    PlayerActions.SetDaytimeSpeed(speed);
                }
            }
            // Quick preset buttons
            if (GUILayout.Button("0.5x", ProjectXStyles.Button, GUILayout.Width(40))) PlayerActions.SetDaytimeSpeed(0.5f);
            if (GUILayout.Button("1x", ProjectXStyles.Button, GUILayout.Width(35))) PlayerActions.SetDaytimeSpeed(1f);
            if (GUILayout.Button("2x", ProjectXStyles.Button, GUILayout.Width(35))) PlayerActions.SetDaytimeSpeed(2f);
            if (GUILayout.Button("5x", ProjectXStyles.Button, GUILayout.Width(35))) PlayerActions.SetDaytimeSpeed(5f);
            GUILayout.EndHorizontal();
            
            DrawDivider("ENVIRONMENT");
            
            // Tree Regrow and Wind disabled due to JIT crash on rapid slider updates
            GUILayout.Label("Tree/Wind: Use config file", ProjectXStyles.NormalLabel);
        }
        
        private void DrawTeleportPanel()
        {
            DrawDivider("QUICK TELEPORT");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Coords:", ProjectXStyles.NormalLabel, GUILayout.Width(60));
            _teleportInput = GUILayout.TextField(_teleportInput, ProjectXStyles.InputField, GUILayout.Width(200));
            if (GUILayout.Button("Go", ProjectXStyles.Button, GUILayout.Width(50)))
            {
                TryTeleportToCoords(_teleportInput);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(5);
            
            // Show current position
            if (LocalPlayer.IsInWorld && LocalPlayer.Transform != null)
            {
                var pos = LocalPlayer.Transform.position;
                GUILayout.Label($"Current: {pos.x:F0}, {pos.y:F0}, {pos.z:F0}", ProjectXStyles.ValueLabel);
            }
            
            DrawDivider("LOCATIONS");
            
            // Draw location buttons in 2-column grid
            int col = 0;
            GUILayout.BeginHorizontal();
            foreach (var loc in TeleportLocations)
            {
                if (GUILayout.Button(loc.Key, ProjectXStyles.Button, GUILayout.Width(230)))
                {
                    TeleportTo(loc.Value);
                }
                col++;
                if (col >= 2)
                {
                    col = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawSystemPanel()
        {
            DrawDivider("MODULES");
            
            Config.RelocatorEnabled.Value = DrawCheckbox("Relocator (C to move)", Config.RelocatorEnabled.Value);
            Config.OpenSesameEnabled.Value = DrawCheckbox("Open Sesame", Config.OpenSesameEnabled.Value);
            Config.PrefabRepairEnabled.Value = DrawCheckbox("Prefab Repair", Config.PrefabRepairEnabled.Value);
            
            GUILayout.Space(20);
            
            GUILayout.Label("Per-item stacks: Settings > Mods > ProjectX", ProjectXStyles.NormalLabel);
        }
        
        private void DrawMiscPanel()
        {
            DrawDivider("STRUCTURES");
            
            float durabilityOld = Config.StructureDurabilityMultiplier.Value;
            float durabilityNew = DrawSlider("Durability Multi", durabilityOld, 1f, 100f);
            if (Math.Abs(durabilityNew - durabilityOld) > 0.05f)
            {
                Config.StructureDurabilityMultiplier.Value = durabilityNew;
                RLog.Msg($"[ProjectX] Structure Durability: {durabilityNew:F1}x");
            }
            
            float heatOld = Config.WaterCollectorHeatRadius.Value;
            float heatNew = DrawSlider("WC Heat Radius", heatOld, 1f, 10f);
            if (Math.Abs(heatNew - heatOld) > 0.05f)
            {
                Config.WaterCollectorHeatRadius.Value = heatNew;
                RLog.Msg($"[ProjectX] Water Collector Heat Radius: {heatNew:F1}");
            }
            
            DrawDivider("ENEMY ACTIONS");
            
            // Re-enabled with per-actor exception handling for IL2CPP stability
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill All Enemies", ProjectXStyles.Button))
            {
                PlayerActions.KillAllEnemies();
            }
            if (GUILayout.Button("Burn All Enemies", ProjectXStyles.Button))
            {
                PlayerActions.BurnAllEnemies();
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Kill All Animals", ProjectXStyles.Button))
            {
                PlayerActions.KillAllAnimals();
            }
            
            // Kill Radius slider (0 = all loaded)
            Config.KillRadius.Value = DrawSlider("Kill Radius (0=ALL)", Config.KillRadius.Value, 0f, 500f);
            
            // Freeze AI toggle
            bool newFreezeAI = DrawCheckbox("Freeze AI (Stop Spawning)", Config.FreezeAI.Value);
            if (newFreezeAI != Config.FreezeAI.Value) PlayerActions.ToggleFreezeAI(newFreezeAI);
            
            DrawDivider("DISCORD");
            
            Config.EnableDiscordBridge.Value = DrawCheckbox("Discord Bridge", Config.EnableDiscordBridge.Value);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Test Message", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestDiscordConnection();
            }
            if (GUILayout.Button("Test Welcome", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestWelcomeEmbed();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Test Join", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestPlayerJoin();
            }
            if (GUILayout.Button("Test Leave", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestPlayerLeave();
            }
            if (GUILayout.Button("Test Death", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestDeath();
            }
            GUILayout.EndHorizontal();
            
            DrawDivider("IN-GAME CHAT");
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Test Chat", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.InGameChat.TestChat();
            }
            if (GUILayout.Button("Test Welcome", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.InGameChat.SendWelcome("TestPlayer");
            }
            GUILayout.EndHorizontal();
            
            DrawDivider("DEBUG");
            
            if (GUILayout.Button("Dump Item IDs", ProjectXStyles.Button))
            {
                // Debug.DumpItemsId();
                RLog.Msg("[ProjectX] Item ID dump triggered");
            }
            
            GUILayout.Space(20);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Config", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                Config.Save();
                RLog.Msg("[ProjectX] Config saved!");
            }
            if (GUILayout.Button("Close Menu", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                ToggleMenu();
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawRaidsPanel()
        {
            DrawDivider("RAID SCHEDULE");
            
            // Time of day toggles
            GUILayout.BeginHorizontal();
            bool morning = DrawCheckbox("Morning", RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value);
            if (morning != RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value) RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value = morning;
            bool day = DrawCheckbox("Day", RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value);
            if (day != RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value) RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value = day;
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            bool evening = DrawCheckbox("Evening", RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value);
            if (evening != RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value) RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value = evening;
            bool night = DrawCheckbox("Night", RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value);
            if (night != RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value) RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value = night;
            GUILayout.EndHorizontal();
            
            // Raids per day
            RaidCustomizer.RaidConfig.RaidsPerDay.Value = (int)DrawSlider("Raids/Day (-1=Def)", RaidCustomizer.RaidConfig.RaidsPerDay.Value, -1, 24);
            
            DrawDivider("ENEMY TYPES");
            
            bool cannibals = DrawCheckbox("Allow Cannibals", RaidCustomizer.RaidConfig.AllowCannibals.Value);
            if (cannibals != RaidCustomizer.RaidConfig.AllowCannibals.Value) RaidCustomizer.RaidConfig.AllowCannibals.Value = cannibals;
            
            bool creepy = DrawCheckbox("Allow Creepy", RaidCustomizer.RaidConfig.AllowCreepy.Value);
            if (creepy != RaidCustomizer.RaidConfig.AllowCreepy.Value) RaidCustomizer.RaidConfig.AllowCreepy.Value = creepy;
            
            bool muddies = DrawCheckbox("Allow Muddies", RaidCustomizer.RaidConfig.AllowMuddies.Value);
            if (muddies != RaidCustomizer.RaidConfig.AllowMuddies.Value) RaidCustomizer.RaidConfig.AllowMuddies.Value = muddies;
            
            DrawDivider("SPAWN SETTINGS");
            
            RaidCustomizer.RaidConfig.SpawnCountFactor.Value = DrawSlider("Min Spawn Factor", RaidCustomizer.RaidConfig.SpawnCountFactor.Value, 0.1f, 10f);
            RaidCustomizer.RaidConfig.MaxSpawnCountFactor.Value = DrawSlider("Max Spawn Factor", RaidCustomizer.RaidConfig.MaxSpawnCountFactor.Value, 0.1f, 10f);
            RaidCustomizer.RaidConfig.EnemyLimit.Value = (int)DrawSlider("Enemy Limit", RaidCustomizer.RaidConfig.EnemyLimit.Value, 1, 30);
            RaidCustomizer.RaidConfig.BossSpawnCount.Value = (int)DrawSlider("Boss Spawn Count", RaidCustomizer.RaidConfig.BossSpawnCount.Value, 1, 30);
            
            DrawDivider("STAT MULTIPLIERS");
            
            bool enableStats = DrawCheckbox("Enable Stat Overrides", RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value);
            if (enableStats != RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
            {
                RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value = enableStats;
                RLog.Msg($"[RaidCustomizer] Stat Overrides: {(enableStats ? "ENABLED" : "DISABLED")}");
            }
            
            if (RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
            {
                RaidCustomizer.RaidConfig.CannibalHealthMultiplier.Value = DrawSlider("Cannibal HP", RaidCustomizer.RaidConfig.CannibalHealthMultiplier.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.CannibalDamageMultiplier.Value = DrawSlider("Cannibal DMG", RaidCustomizer.RaidConfig.CannibalDamageMultiplier.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.CreepHealthMultiplier.Value = DrawSlider("Creep HP", RaidCustomizer.RaidConfig.CreepHealthMultiplier.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.CreepDamageMultiplier.Value = DrawSlider("Creep DMG", RaidCustomizer.RaidConfig.CreepDamageMultiplier.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.BossHealthMultiplier.Value = DrawSlider("Boss HP", RaidCustomizer.RaidConfig.BossHealthMultiplier.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.BossDamageMultiplier.Value = DrawSlider("Boss DMG", RaidCustomizer.RaidConfig.BossDamageMultiplier.Value, 0.1f, 10f);
            }
            
            DrawDivider("ANNOUNCEMENTS");
            
            bool announce = DrawCheckbox("Announce Raids", RaidCustomizer.RaidConfig.AnnounceIncomingSearchParties.Value);
            if (announce != RaidCustomizer.RaidConfig.AnnounceIncomingSearchParties.Value)
            {
                RaidCustomizer.RaidConfig.AnnounceIncomingSearchParties.Value = announce;
                RLog.Msg($"[RaidCustomizer] Announce Raids: {(announce ? "ENABLED" : "DISABLED")}");
            }
            
            DrawDivider("QUICK ACTIONS");
            
            // Row 1: Main actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Run Random Raid", ProjectXStyles.Button, GUILayout.Height(35)))
            {
                RaidCustomizer.RaidActions.RunRandomRaid();
            }
            if (GUILayout.Button("Clear Queued Raids", ProjectXStyles.Button, GUILayout.Height(35)))
            {
                RaidCustomizer.RaidActions.ClearQueuedRaids();
            }
            GUILayout.EndHorizontal();
            
            // Row 2: Clear actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear All Events", ProjectXStyles.Button, GUILayout.Height(30)))
            {
                RaidCustomizer.RaidActions.ClearAllEvents();
            }
            if (GUILayout.Button("Clear Cooldowns", ProjectXStyles.Button, GUILayout.Height(30)))
            {
                RaidCustomizer.RaidActions.ClearRaidCooldowns();
            }
            GUILayout.EndHorizontal();
            
            // Row 3: Utility actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Requeue Raids", ProjectXStyles.Button, GUILayout.Height(30)))
            {
                RaidCustomizer.RaidActions.RequeueRaids();
            }
            if (GUILayout.Button("Print Queued (Log)", ProjectXStyles.Button, GUILayout.Height(30)))
            {
                RaidCustomizer.RaidActions.PrintQueuedRaids();
            }
            GUILayout.EndHorizontal();
        }
        
        #endregion
        
        #region Helper Methods
        
        private void DrawDivider(string text)
        {
            GUILayout.Space(12);
            GUILayout.Label($"═══ {text} ═══", ProjectXStyles.DividerLabel);
            GUILayout.Space(4);
        }
        
        private bool DrawCheckbox(string label, bool value)
        {
            return GUILayout.Toggle(value, "  " + label, ProjectXStyles.Toggle);
        }
        
        private float DrawSlider(string label, float value, float min, float max)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, ProjectXStyles.NormalLabel, GUILayout.Width(200));
            float newVal = GUILayout.HorizontalSlider(value, min, max, 
                ProjectXStyles.HorizontalSlider, ProjectXStyles.HorizontalSliderThumb, 
                GUILayout.Width(500));
            GUILayout.Label(newVal.ToString("F1"), ProjectXStyles.ValueLabel, GUILayout.Width(60));
            GUILayout.EndHorizontal();
            return newVal;
        }
        
        private void TryTeleportToCoords(string input)
        {
            try
            {
                var parts = input.Replace(" ", "").Split(',');
                if (parts.Length >= 3)
                {
                    float x = float.Parse(parts[0]);
                    float y = float.Parse(parts[1]);
                    float z = float.Parse(parts[2]);
                    TeleportTo(new Vector3(x, y, z));
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ProjectX] Invalid coords: {ex.Message}");
            }
        }
        
        private void TeleportTo(Vector3 position)
        {
            if (LocalPlayer.IsInWorld && LocalPlayer.Transform != null)
            {
                LocalPlayer.Transform.position = position;
                RLog.Msg($"[ProjectX] Teleported to {position}");
            }
        }
        
        #endregion
    }
}
#endif
