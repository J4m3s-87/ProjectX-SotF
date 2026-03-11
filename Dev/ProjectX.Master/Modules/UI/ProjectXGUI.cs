#if !SERVER
using System;
using System.Collections.Generic;
using RedLoader;
using Il2CppInterop.Runtime.Injection;
using SonsSdk;
using TheForest.Utils;
using UnityEngine;
using TheForest;
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
        private readonly string[] _panelNames = { "Player", "Environment", "Teleport", "Building+", "Raids", "Server Admin", "Server Raids" };
        
        // Layout dimensions (enlarged for better readability)
        private const float PANEL_WIDTH = 1100f;
        private const float PANEL_HEIGHT = 1150f;
        private const float SELECTOR_WIDTH = 800f;
        private const float SELECTOR_HEIGHT = 65f;
        
        private Rect _selectorRect;
        private Rect _panelRect;
        private Vector2 _scrollPosition;
        private Vector2 _raidQueueScroll;
        
        // Teleport input
        private string _teleportInput = "";
        private string _timeInput = "12:00";
        private string _respawnDaysInput = "3";
        private string _speedInput = "1";
        private string _stackInput = "999";
        private bool _forceRainActive = false;
        private bool _noWorldGravity = false;
        private bool _noGrassActive = false;
        private bool _noForestActive = false;
        private string _activeSeason = "";
        
        // Console commands state
        private int _aiAngerLevel = 50;
        
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
            { "End Game Bunker", new Vector3(1756, 45, 553) },
            // Modern Bow removed — teleport doesn't work as intended
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
            // Client edition: menu is locked — all settings controlled by server ConfigSync.
            // Future: server-wide cheats toggle could unlock specific panels.
            RLog.Msg("[ProjectXGUI] Client build: menu disabled (server-authoritative)");
            return;
#endif
            // Don't open mod menu when the game's Esc/pause menu is active
            if (Sons.Gui.PauseMenu.IsActive) return;
            
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
            // Piggyback MeatDryer scanner on this proven OnGUI loop
            try { MeatDryer.MeatDryerModule.OnGuiTick(); } catch { }
            // Piggyback WaterCollectors fire proximity check
            try { WaterCollectors.WaterCollectorsModule.OnGuiTick(); } catch { }
            
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
                case 3: DrawMiscPanel(); break;
                case 4: DrawRaidsPanel(); break;
                case 5: DrawServerAdminPanel(); break;
                case 6: DrawServerRaidsPanel(); break;
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        #region Panel Drawings
        
        private void DrawPlayerPanel()
        {
            // Stats section
            DrawDivider("STATS");
            float colW = (PANEL_WIDTH - 60f) / 2f; // half the panel width minus padding (20+20+margins)
            
            // Row 1: God Mode | Infinite Stamina
            GUILayout.BeginHorizontal();
            bool newGodMode = GUILayout.Toggle(Config.IsGodMode.Value, "  God Mode", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newGodMode != Config.IsGodMode.Value) PlayerActions.ToggleGodMode(newGodMode);
            bool newInfStamina = GUILayout.Toggle(Config.IsInfStamina.Value, "  Infinite Stamina", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newInfStamina != Config.IsInfStamina.Value) PlayerActions.ToggleInfStamina(newInfStamina);
            GUILayout.EndHorizontal();
            
            // Row 2: No Hunger | No Thirst
            GUILayout.BeginHorizontal();
            bool newNoHunger = GUILayout.Toggle(Config.IsNoHungry.Value, "  No Hunger", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoHunger != Config.IsNoHungry.Value) PlayerActions.ToggleNoHunger(newNoHunger);
            bool newNoThirst = GUILayout.Toggle(Config.IsNoDehydration.Value, "  No Thirst", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoThirst != Config.IsNoDehydration.Value) PlayerActions.ToggleNoDehydration(newNoThirst);
            GUILayout.EndHorizontal();
            
            // Row 3: No Sleep | No Fall Damage
            GUILayout.BeginHorizontal();
            bool newNoSleep = GUILayout.Toggle(Config.IsNoSleep.Value, "  No Sleep", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoSleep != Config.IsNoSleep.Value) PlayerActions.ToggleNoSleep(newNoSleep);
            bool newNoFall = GUILayout.Toggle(Config.IsNoFallDamage.Value, "  No Fall Damage", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoFall != Config.IsNoFallDamage.Value) PlayerActions.ToggleNoFallDamage(newNoFall);
            GUILayout.EndHorizontal();
            
            // Row 4: No Gravity | Infinite Stacks
            GUILayout.BeginHorizontal();
            bool newNoGravity = GUILayout.Toggle(Config.IsNoGravity.Value, "  No Gravity", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoGravity != Config.IsNoGravity.Value) PlayerActions.ToggleNoGravity(newNoGravity);
            bool prevInfinite = Config.InfiniteInventory.Value;
            Config.InfiniteInventory.Value = GUILayout.Toggle(Config.InfiniteInventory.Value, "  Infinite Stacks", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (Config.InfiniteInventory.Value != prevInfinite)
            {
                Modules.Stack.StackModule.Apply();
                RLog.Msg($"[ProjectXGUI] Infinite Stacks: {Config.InfiniteInventory.Value}");
            }
            GUILayout.EndHorizontal();
            
            // Row 5: AI Ghost Player | Fill Inventory
            GUILayout.BeginHorizontal();
            bool newGhostPlayer = GUILayout.Toggle(Modules.Building.BuilderEnhancements.AIGhostPlayer, "  AI Ghost Player", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newGhostPlayer != Modules.Building.BuilderEnhancements.AIGhostPlayer)
                Modules.Building.BuilderEnhancements.AIGhostPlayer = newGhostPlayer;
            if (GUILayout.Toggle(false, "  Fill Inventory", ProjectXStyles.Toggle, GUILayout.Width(colW)))
            {
                try
                {
                    // ALWAYS reset stacks to safe defaults before fill.
                    // Prevents 999M fills if Infinite Stacks was toggled on then off
                    // (ApplyPerItemConfig only covers 118/308 items — the rest retain 999M).
                    bool wasInfinite = Config.InfiniteInventory.Value;
                    Config.InfiniteInventory.Value = false;
                    Modules.Stack.StackModule.ResetToSafeDefaults();
                    Modules.Stack.StackModule.Apply(); // restore per-item config caps BEFORE fill
                    
                    SonsSdk.SonsTools.ShowMessage("Filling inventory...");
                    DebugConsole.Instance.SendCommand("addallitems");
                    RLog.Msg("[ProjectXGUI] Fill Inventory triggered via addallitems");
                    
                    // Restore previous state and re-apply stack config
                    if (wasInfinite)
                        Config.InfiniteInventory.Value = true;
                    Modules.Stack.StackModule.Apply();
                }
                catch (System.Exception ex)
                {
                    RLog.Warning($"[ProjectXGUI] Fill Inventory failed: {ex.Message}");
                }
            }
            GUILayout.EndHorizontal();
            
            // Row 6: Max Strength
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(false, "  Max Strength", ProjectXStyles.Toggle, GUILayout.Width(colW)))
            {
                try
                {
                    DebugConsole.Instance.SendCommand("setstrengthlevel 100");
                    SonsSdk.SonsTools.ShowMessage("Strength set to max!");
                    RLog.Msg("[ProjectXGUI] Max Strength triggered via setstrengthlevel 100");
                }
                catch (System.Exception ex)
                {
                    RLog.Warning($"[ProjectXGUI] Max Strength failed: {ex.Message}");
                }
            }
            GUILayout.EndHorizontal();
            
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
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(false, "  Reset Movement", ProjectXStyles.Toggle, GUILayout.Width(colW)))
            {
                PlayerActions.CaptureVanillaDefaults();
                float vw = PlayerActions.VanillaWalkSpeed;
                float vr = PlayerActions.VanillaRunSpeed;
                float vs = PlayerActions.VanillaSwimSpeed;
                float vj = PlayerActions.VanillaJumpMultiplier;
                Config.WalkSpeed.Value = vw;
                Config.RunSpeed.Value = vr;
                Config.SwimSpeed.Value = vs;
                Config.JumpMultiplier.Value = vj;
                PlayerActions.SetWalkSpeed(vw);
                PlayerActions.SetRunSpeed(vr);
                PlayerActions.SetSwimSpeed(vs);
                PlayerActions.SetJumpMultiplier(vj);
                RLog.Msg($"[GUI] Reset Movement to vanilla: walk={vw:F2}, run={vr:F2}, swim={vs:F2}, jump={vj:F2}");
            }
            GUILayout.EndHorizontal();
            
            // Enemy Actions section (moved from Misc panel)
            DrawDivider("ENEMY ACTIONS");
            
            // Re-enabled with per-actor exception handling for IL2CPP stability
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill All Enemies", ProjectXStyles.Button, GUILayout.Width(colW)))
            {
                PlayerActions.KillAllEnemies();
            }
            if (GUILayout.Button("Burn All Enemies", ProjectXStyles.Button, GUILayout.Width(colW)))
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
            
            // Freeze AI toggle — route to server via /px world freeze
            // (PlayerActions.ToggleFreezeAI only calls SetPaused locally, doesn't reach dedicated server)
            bool newFreezeAI = DrawCheckbox("Freeze AI (Stop Spawning)", Config.FreezeAI.Value);
            if (newFreezeAI != Config.FreezeAI.Value)
            {
                ServerCmd("world freeze");
            }
            
            // Spawn NPC section
            DrawDivider("SPAWN NPC");
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("End Boss", ProjectXStyles.Button, GUILayout.Width(colW)))
            {
                DebugConsole.Instance.SendCommand("addcharacter mutantboss");
                RLog.Msg("[GUI] Spawn: addcharacter mutantboss");
            }
            if (GUILayout.Button("Armsy", ProjectXStyles.Button, GUILayout.Width(colW)))
            {
                DebugConsole.Instance.SendCommand("addcharacter armsy");
                RLog.Msg("[GUI] Spawn: addcharacter armsy");
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Cannibal Group (x3)", ProjectXStyles.Button))
            {
                DebugConsole.Instance.SendCommand("addcharacter cannibal 3");
                RLog.Msg("[GUI] Spawn: addcharacter cannibal 3");
            }
        }
        
        private void DrawEnvironmentPanel()
        {
            float colW = (PANEL_WIDTH - 60f) / 2f;
            DrawDivider("TERRAIN");
            
            // Grass/Forest toggles
            GUILayout.BeginHorizontal();
            bool newNoGrass = GUILayout.Toggle(_noGrassActive, "  No Grass", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoGrass != _noGrassActive)
            {
                _noGrassActive = newNoGrass;
                PlayerActions.ToggleNoGrass();
            }
            bool newNoForest = GUILayout.Toggle(_noForestActive, "  No Forest", ProjectXStyles.Toggle, GUILayout.Width(colW));
            if (newNoForest != _noForestActive)
            {
                _noForestActive = newNoForest;
                PlayerActions.ToggleNoForest();
            }
            GUILayout.EndHorizontal();
            
            DrawDivider("SEASONS");
            
            GUILayout.BeginHorizontal();
            bool springActive = _activeSeason == "Spring";
            if (GUILayout.Toggle(springActive, "  Spring", ProjectXStyles.Toggle, GUILayout.Width(colW)) && !springActive)
            { _activeSeason = "Spring"; PlayerActions.SetSeason("Spring"); }
            bool summerActive = _activeSeason == "Summer";
            if (GUILayout.Toggle(summerActive, "  Summer", ProjectXStyles.Toggle, GUILayout.Width(colW)) && !summerActive)
            { _activeSeason = "Summer"; PlayerActions.SetSeason("Summer"); }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            bool autumnActive = _activeSeason == "Autumn";
            if (GUILayout.Toggle(autumnActive, "  Autumn", ProjectXStyles.Toggle, GUILayout.Width(colW)) && !autumnActive)
            { _activeSeason = "Autumn"; PlayerActions.SetSeason("Autumn"); }
            bool winterActive = _activeSeason == "Winter";
            if (GUILayout.Toggle(winterActive, "  Winter", ProjectXStyles.Toggle, GUILayout.Width(colW)) && !winterActive)
            { _activeSeason = "Winter"; PlayerActions.SetSeason("Winter"); }
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
            GUILayout.Label("Speed:", ProjectXStyles.NormalLabel, GUILayout.Width(70));
            _speedInput = GUILayout.TextField(_speedInput, ProjectXStyles.InputField, GUILayout.Width(60));
            if (GUILayout.Button("Set", ProjectXStyles.Button, GUILayout.Width(75), GUILayout.Height(45)))
            {
                if (float.TryParse(_speedInput, out float speed))
                {
                    PlayerActions.SetDaytimeSpeed(speed);
                }
            }
            // Quick preset buttons
            if (GUILayout.Button("0.5x", ProjectXStyles.Button, GUILayout.Width(70), GUILayout.Height(45))) PlayerActions.SetDaytimeSpeed(0.5f);
            if (GUILayout.Button("1x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) PlayerActions.SetDaytimeSpeed(1f);
            if (GUILayout.Button("2x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) PlayerActions.SetDaytimeSpeed(2f);
            if (GUILayout.Button("5x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) PlayerActions.SetDaytimeSpeed(5f);
            GUILayout.EndHorizontal();
            
            DrawDivider("ENVIRONMENT");
            
            Config.TreeRegrowRate.Value = DrawSlider("Tree Regrow Rate", Config.TreeRegrowRate.Value, 0f, 10f);
            float newWind = DrawSlider("Wind Intensity (-1=Auto)", Config.WindIntensity.Value, -1f, 10f);
            if (Math.Abs(newWind - Config.WindIntensity.Value) > 0.01f)
            {
                PlayerActions.SetWindIntensity(newWind);
            }
            
            // Waterfall Volume slider
            float newWaterfallVol = DrawSlider("Waterfall Volume", Config.WaterfallVolume.Value, 0f, 3f);
            if (Math.Abs(newWaterfallVol - Config.WaterfallVolume.Value) > 0.01f)
            {
                Config.WaterfallVolume.Value = newWaterfallVol;
            }
            
            // Force Rain
            bool newRain = DrawCheckbox("Force Rain", _forceRainActive);
            if (newRain != _forceRainActive)
            {
                _forceRainActive = newRain;
                PlayerActions.ToggleForceRain(newRain);
            }
            
            // No World Gravity
            bool newWorldGrav = DrawCheckbox("No World Gravity", _noWorldGravity);
            if (newWorldGrav != _noWorldGravity)
            {
                _noWorldGravity = newWorldGrav;
                if (_noWorldGravity)
                {
                    DebugConsole.Instance.SendCommand("gravity 0");
                    RLog.Msg("[ProjectXGUI] World gravity disabled (gravity 0)");
                }
                else
                {
                    DebugConsole.Instance.SendCommand("gravity -9.81");
                    RLog.Msg("[ProjectXGUI] World gravity restored (gravity -9.81)");
                }
            }
            
            DrawDivider("LOOT RESPAWN");
            
            // Enable toggle
            bool newLootRespawn = DrawCheckbox("Enable Loot Respawn", Config.LootRespawnEnabled.Value);
            if (newLootRespawn != Config.LootRespawnEnabled.Value) Config.LootRespawnEnabled.Value = newLootRespawn;
            
            // Days slider
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Respawn Days: {Config.LootRespawnDays.Value}", ProjectXStyles.NormalLabel, GUILayout.Width(160));
            int newDays = (int)GUILayout.HorizontalSlider(Config.LootRespawnDays.Value, 1, 100, GUILayout.Width(200));
            if (newDays != Config.LootRespawnDays.Value) Config.LootRespawnDays.Value = newDays;
            GUILayout.EndHorizontal();
            
            // Loot Type Override toggle — reveals per-category day sliders
            bool newLootOverride = DrawCheckbox("Loot Type Override", Config.LR_LootTypeOverride.Value);
            if (newLootOverride != Config.LR_LootTypeOverride.Value) Config.LR_LootTypeOverride.Value = newLootOverride;
            
            if (Config.LR_LootTypeOverride.Value)
            {
                Config.LR_MeleeDays.Value = DrawIntSlider("Melee Days (-1=Global)", Config.LR_MeleeDays.Value, -1, 100);
                Config.LR_RangedDays.Value = DrawIntSlider("Ranged Days (-1=Global)", Config.LR_RangedDays.Value, -1, 100);
                Config.LR_WeaponModsDays.Value = DrawIntSlider("Weapon Mods Days (-1=Global)", Config.LR_WeaponModsDays.Value, -1, 100);
                Config.LR_MaterialsDays.Value = DrawIntSlider("Materials Days (-1=Global)", Config.LR_MaterialsDays.Value, -1, 100);
                Config.LR_FoodDays.Value = DrawIntSlider("Food Days (-1=Global)", Config.LR_FoodDays.Value, -1, 100);
                Config.LR_MedsDays.Value = DrawIntSlider("Meds Days (-1=Global)", Config.LR_MedsDays.Value, -1, 100);
                Config.LR_PlantsDays.Value = DrawIntSlider("Plants Days (-1=Global)", Config.LR_PlantsDays.Value, -1, 100);
                Config.LR_AmmoDays.Value = DrawIntSlider("Ammo Days (-1=Global)", Config.LR_AmmoDays.Value, -1, 100);
                Config.LR_ThrowablesDays.Value = DrawIntSlider("Throwables Days (-1=Global)", Config.LR_ThrowablesDays.Value, -1, 100);
                Config.LR_ExpendablesDays.Value = DrawIntSlider("Expendables Days (-1=Global)", Config.LR_ExpendablesDays.Value, -1, 100);
                Config.LR_BreakablesDays.Value = DrawIntSlider("Breakables Days (-1=Global)", Config.LR_BreakablesDays.Value, -1, 100);
                Config.LR_OpenablesDays.Value = DrawIntSlider("Openables Days (-1=Global)", Config.LR_OpenablesDays.Value, -1, 100);
            }
            
            // Reset + Debug (same pattern as server panel — two buttons, Height 45)
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Loot Tracker", ProjectXStyles.Button, GUILayout.Height(45)))
                Modules.LootRespawn.LootRespawnModule.Reset();
            if (GUILayout.Button(Modules.LootRespawn.RespawnConfig.ConsoleLogging ? "Debug: ON" : "Debug: OFF", ProjectXStyles.Button, GUILayout.Height(45)))
                Modules.LootRespawn.RespawnConfig.ConsoleLogging = !Modules.LootRespawn.RespawnConfig.ConsoleLogging;
            GUILayout.EndHorizontal();
            
            // Status display
            GUILayout.Label(Modules.LootRespawn.LootRespawnModule.GetStatus(), ProjectXStyles.NormalLabel);
        }
        
        private void DrawTeleportPanel()
        {
            DrawDivider("QUICK TELEPORT");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Coords:", ProjectXStyles.NormalLabel, GUILayout.Width(80));
            _teleportInput = GUILayout.TextField(_teleportInput, ProjectXStyles.InputField, GUILayout.Width(350));
            if (GUILayout.Button("Go", ProjectXStyles.Button, GUILayout.Width(60)))
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
                if (GUILayout.Button(loc.Key, ProjectXStyles.Button, GUILayout.Width(300)))
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
            
            // Companions section (moved from Player panel)
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
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Revive Kelvin", ProjectXStyles.Button))
            {
                try
                {
                    DebugConsole.Instance.SendCommand("removedead");
                    DebugConsole.Instance.SendCommand("addcharacter robby");
                }
                catch {}
            }
            if (GUILayout.Button("Revive Virginia", ProjectXStyles.Button))
            {
                try
                {
                    DebugConsole.Instance.SendCommand("removedead");
                    DebugConsole.Instance.SendCommand("addcharacter virginia");
                }
                catch {}
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Virginia Visit", ProjectXStyles.Button))
            {
                try { DebugConsole.Instance.SendCommand("Virginiavisit"); }
                catch {}
            }
            if (GUILayout.Button("Happy Virginia", ProjectXStyles.Button))
            {
                try { DebugConsole.Instance.SendCommand("virginiasentiment 100"); }
                catch {}
            }
            GUILayout.EndHorizontal();
            
            bool statsEnabled = DrawCheckbox("Enable Stat Overrides", RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value);
            if (statsEnabled != RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
                RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value = statsEnabled;
            
            float kelvinOld = RaidCustomizer.RaidConfig.KelvinHealthMultiplier.Value;
            float kelvinNew = DrawSlider("Kelvin HP Multi", kelvinOld, 1f, 200f);
            if (Math.Abs(kelvinNew - kelvinOld) > 0.05f)
            {
                RaidCustomizer.RaidConfig.KelvinHealthMultiplier.Value = kelvinNew;
                if (kelvinNew > 1.05f && !RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
                    RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value = true;
            }
            
            float virginiaOld = RaidCustomizer.RaidConfig.VirginiaHealthMultiplier.Value;
            float virginiaNew = DrawSlider("Virginia HP Multi", virginiaOld, 1f, 200f);
            if (Math.Abs(virginiaNew - virginiaOld) > 0.05f)
            {
                RaidCustomizer.RaidConfig.VirginiaHealthMultiplier.Value = virginiaNew;
                if (virginiaNew > 1.05f && !RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
                    RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value = true;
            }
        }
        
        private void DrawMiscPanel()
        {
            
            DrawDivider("STRUCTURES");
            
            float durabilityOld = Config.StructureDurabilityMultiplier.Value;
            float durabilityNew = DrawSlider("Durability Multi", durabilityOld, 1f, 100f);
            if (Math.Abs(durabilityNew - durabilityOld) > 0.05f)
            {
                Config.StructureDurabilityMultiplier.Value = durabilityNew;
                Modules.StructureDurability.StructureDurabilityModule.Apply();
            }
            
            float heatOld = Config.WaterCollectorHeatRadius.Value;
            float heatNew = DrawSlider("WC Heat Radius", heatOld, 1f, 10f);
            if (Math.Abs(heatNew - heatOld) > 0.05f)
            {
                Config.WaterCollectorHeatRadius.Value = heatNew;
                RLog.Msg($"[ProjectX] Water Collector Heat Radius: {heatNew:F1}");
            }
            
            DrawDivider("BUILDING CHEATS");
            
            // FreeForm placement - restored with GameSetupManager API
            bool newFreeForm = DrawCheckbox("Free Form Placement", Modules.Building.BuilderEnhancements.FreeFormPlacement);
            if (newFreeForm != Modules.Building.BuilderEnhancements.FreeFormPlacement)
            {
                Modules.Building.BuilderEnhancements.FreeFormPlacement = newFreeForm;
            }
            
            // Instant Build - uses game's instantbookbuild command
            bool newInstantBuild = DrawCheckbox("Instant Build", Modules.Building.BuilderEnhancements.InstantBuild);
            if (newInstantBuild != Modules.Building.BuilderEnhancements.InstantBuild)
            {
                Modules.Building.BuilderEnhancements.InstantBuild = newInstantBuild;
            }
            
            // No Cuttings - confirmed working
            bool newNoCuttings = DrawCheckbox("No Wood Cuttings", Modules.Building.BuilderEnhancements.NoCuttingsSpawn);
            if (newNoCuttings != Modules.Building.BuilderEnhancements.NoCuttingsSpawn)
            {
                Modules.Building.BuilderEnhancements.NoCuttingsSpawn = newNoCuttings;
            }
            
            // Log Hack - uses _loghack command
            bool newLogHack = DrawCheckbox("Log Hack", Modules.Building.BuilderEnhancements.LogHack);
            if (newLogHack != Modules.Building.BuilderEnhancements.LogHack)
            {
                Modules.Building.BuilderEnhancements.LogHack = newLogHack;
            }
            
            // Stone Hack - uses stonehack command
            bool newStoneHack = DrawCheckbox("Stone Hack", Modules.Building.BuilderEnhancements.StoneHack);
            if (newStoneHack != Modules.Building.BuilderEnhancements.StoneHack)
            {
                Modules.Building.BuilderEnhancements.StoneHack = newStoneHack;
            }
            
            // Slap Chop - uses _slapchop command (fast wood chopping)
            bool newSlapChop = DrawCheckbox("Slap Chop", Modules.Building.BuilderEnhancements.SlapChop);
            if (newSlapChop != Modules.Building.BuilderEnhancements.SlapChop)
            {
                Modules.Building.BuilderEnhancements.SlapChop = newSlapChop;
            }
            
            // Blueprint actions (one-shot buttons)
            float colW2 = (PANEL_WIDTH - 60f) / 2f;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel Blueprints", ProjectXStyles.Button, GUILayout.Width(colW2)))
            {
                Modules.Building.BuilderEnhancements.CancelBlueprints();
            }
            if (GUILayout.Button("Finish Blueprints", ProjectXStyles.Button, GUILayout.Width(colW2)))
            {
                Modules.Building.BuilderEnhancements.FinishBlueprints();
            }
            GUILayout.EndHorizontal();
            
            // Repair All button — iterates all Structure objects and resets HP
            if (GUILayout.Button("Repair All Structures", ProjectXStyles.Button))
            {
                Modules.Building.BuilderEnhancements.RepairAllStructures();
            }
            
            DrawDivider("CRAFTING");
            
            // Faster Crafting - patches CraftingCog.OnCraftBeginEvent for backpack crafting
            bool newFasterCrafting = DrawCheckbox("Faster Crafting", Modules.Crafting.CraftingSpeed.Enabled);
            if (newFasterCrafting != Modules.Crafting.CraftingSpeed.Enabled)
            {
                Modules.Crafting.CraftingSpeed.Enabled = newFasterCrafting;
            }
            
            // Speed multiplier (only shown when enabled)
            if (Modules.Crafting.CraftingSpeed.Enabled)
            {
                float speedOld = Modules.Crafting.CraftingSpeed.SpeedMultiplier;
                float speedNew = DrawSlider("Max Craft Speed", speedOld, 1f, 10f);
                if (Math.Abs(speedNew - speedOld) > 0.1f)
                {
                    Modules.Crafting.CraftingSpeed.SpeedMultiplier = speedNew;
                }
            }
            
            // Add All Book Pages button — unlocks all blueprint book pages
            if (GUILayout.Button("Add All Book Pages", ProjectXStyles.Button))
            {
                Modules.Building.BuilderEnhancements.AddAllBookPages();
            }
            
            
        }
        
        private void DrawDiscordPanel()
        {
            DrawDivider("DISCORD BRIDGE");
            
            Config.EnableDiscordBridge.Value = DrawCheckbox("Discord Bridge", Config.EnableDiscordBridge.Value);
            
            GUILayout.BeginHorizontal();
#if !CLIENT
            if (GUILayout.Button("Test Message", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestDiscordConnection();
            }
            if (GUILayout.Button("Test Welcome", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.BroadcastMessageModule.TestWelcomeEmbed();
            }
#endif
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
#if !CLIENT
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
#endif
            GUILayout.EndHorizontal();
            
            DrawDivider("IN-GAME CHAT");
            
            GUILayout.BeginHorizontal();
#if !CLIENT
            if (GUILayout.Button("Test Chat", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.InGameChat.TestChat();
            }
            if (GUILayout.Button("Test Welcome", ProjectXStyles.Button))
            {
                Modules.BroadcastMessage.InGameChat.SendWelcome("TestPlayer");
            }
#endif
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
            
            // Raid Distribution (radio-style: only one active at a time)
            GUILayout.BeginHorizontal();
            string dist = RaidCustomizer.RaidConfig.RaidDistribution.Value;
            bool isEvenly = DrawCheckbox("Evenly", dist == "Evenly");
            if (isEvenly && dist != "Evenly") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Evenly";
            bool isRandomly = DrawCheckbox("Randomly", dist == "Randomly");
            if (isRandomly && dist != "Randomly") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Randomly";
            bool isStacked = DrawCheckbox("Stacked", dist == "Stacked");
            if (isStacked && dist != "Stacked") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Stacked";
            GUILayout.EndHorizontal();
            
            // Consider Current Time
            bool considerTime = DrawCheckbox("Consider Current Time", RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value);
            if (considerTime != RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value)
                RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value = considerTime;
            
            DrawDivider("ENEMY TYPES");
            
            GUILayout.BeginHorizontal();
            bool cannibals = DrawCheckbox("Allow Cannibals", RaidCustomizer.RaidConfig.AllowCannibals.Value);
            if (cannibals != RaidCustomizer.RaidConfig.AllowCannibals.Value) RaidCustomizer.RaidConfig.AllowCannibals.Value = cannibals;
            
            bool creepy = DrawCheckbox("Allow Creepy", RaidCustomizer.RaidConfig.AllowCreepy.Value);
            if (creepy != RaidCustomizer.RaidConfig.AllowCreepy.Value) RaidCustomizer.RaidConfig.AllowCreepy.Value = creepy;
            
            bool muddies = DrawCheckbox("Allow Muddies", RaidCustomizer.RaidConfig.AllowMuddies.Value);
            if (muddies != RaidCustomizer.RaidConfig.AllowMuddies.Value) RaidCustomizer.RaidConfig.AllowMuddies.Value = muddies;
            GUILayout.EndHorizontal();
            
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
            
            DrawDivider("QUICK ACTIONS");
            
            // Row 1: Main actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Run Random Raid", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.RunRandomRaid();
            }
            if (GUILayout.Button("Clear Queued Raids", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.ClearQueuedRaids();
            }
            GUILayout.EndHorizontal();
            
            // Row 2: Clear actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear All Events", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.ClearAllEvents();
            }
            if (GUILayout.Button("Clear Cooldowns", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.ClearRaidCooldowns();
            }
            GUILayout.EndHorizontal();
            
            // Row 3: Utility actions
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Requeue Raids", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.RequeueRaids();
            }
            if (GUILayout.Button("Print Queued (Log)", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                RaidCustomizer.RaidActions.PrintQueuedRaids();
            }
            GUILayout.EndHorizontal();
            
            DrawDivider("AI AGGRESSION");
            
            _aiAngerLevel = (int)DrawSlider("Anger Level", _aiAngerLevel, 0, 100);
            if (GUILayout.Button("Apply Anger Level", ProjectXStyles.Button, GUILayout.Height(45)))
            {
                DebugConsole.Instance.SendCommand($"aiangerlevel {_aiAngerLevel}");
                RLog.Msg($"[GUI] AI anger level → {_aiAngerLevel}");
            }
        }
        
        /// <summary>
        /// Send a /px command to the server for execution via game chat network.
        /// Commands should be in CommandBridge format (e.g. "world time 12", "save").
        /// The /px prefix is added automatically by AdminCommandEvent.SendToServer().
        /// </summary>
        private void ServerCmd(string command)
        {
            try { Network.AdminCommandEvent.SendToServer(command); }
            catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] Command error: {ex.Message}"); }
        }
        
        /// <summary>
        /// Batch-send all current raid config values to the server, then trigger a single requeue.
        /// This replaces the old per-slider auto-requeue approach that caused config race conditions.
        /// </summary>
        private void ApplyRaidConfigAndRequeue()
        {
            try
            {
                // Batch all raid config values to the server
                ServerCmd($"config set XR_AllowCreepy {RaidCustomizer.RaidConfig.AllowCreepy.Value}");
                ServerCmd($"config set XR_AllowCannibals {RaidCustomizer.RaidConfig.AllowCannibals.Value}");
                ServerCmd($"config set XR_AllowMuddies {RaidCustomizer.RaidConfig.AllowMuddies.Value}");
                ServerCmd($"config set XR_AnnounceRaids {RaidCustomizer.RaidConfig.AnnounceIncomingSearchParties.Value}");
                ServerCmd($"config set XR_IncludeEndgame {RaidCustomizer.RaidConfig.AlwaysIncludeEndgameRaids.Value}");
                ServerCmd($"config set XR_IncludeForest {RaidCustomizer.RaidConfig.AlwaysIncludeForestOnlyRaids.Value}");
                ServerCmd($"config set XR_RaidsPerDay {RaidCustomizer.RaidConfig.RaidsPerDay.Value}");
                ServerCmd($"config set XR_ConsiderTime {RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value}");
                ServerCmd($"config set XR_Distribution {RaidCustomizer.RaidConfig.RaidDistribution.Value}");
                
                // Time ranges
                ServerCmd($"config set XR_RaidAtMorning {RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value}");
                ServerCmd($"config set XR_RaidAtDay {RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value}");
                ServerCmd($"config set XR_RaidAtEvening {RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value}");
                ServerCmd($"config set XR_RaidAtNight {RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value}");
                
                // Spawn settings  
                ServerCmd($"config set XR_MinSpawnFactor {RaidCustomizer.RaidConfig.SpawnCountFactor.Value}");
                ServerCmd($"config set XR_MaxSpawnFactor {RaidCustomizer.RaidConfig.MaxSpawnCountFactor.Value}");
                ServerCmd($"config set XR_EnemyLimit {RaidCustomizer.RaidConfig.EnemyLimit.Value}");
                ServerCmd($"config set XR_BossCount {RaidCustomizer.RaidConfig.BossSpawnCount.Value}");
                
                // Overrides
                ServerCmd($"config set XR_IgnoreDayLimit {RaidCustomizer.RaidConfig.IgnoreMinMaxDay.Value}");
                ServerCmd($"config set XR_IgnoreAngerLimit {RaidCustomizer.RaidConfig.IgnoreMinMaxAnger.Value}");
                ServerCmd($"config set XR_BossIgnoreDay {RaidCustomizer.RaidConfig.BossIgnoreMinMaxDay.Value}");
                ServerCmd($"config set XR_BossIgnoreAnger {RaidCustomizer.RaidConfig.BossIgnoreMinMaxAnger.Value}");
                
                // Multiplayer
                ServerCmd($"config set XR_AdjustByPlayers {RaidCustomizer.RaidConfig.AdjustOnPlayerCount.Value}");
                ServerCmd($"config set XR_ExtraRaids {RaidCustomizer.RaidConfig.ExtraRaidsPerPlayer.Value}");
                ServerCmd($"config set XR_ExtraSpawns {RaidCustomizer.RaidConfig.ExtraSpawnsPerPlayer.Value}");
                ServerCmd($"config set XR_ExtraBosses {RaidCustomizer.RaidConfig.ExtraBossesPerPlayer.Value}");
                
                // Cooldowns
                ServerCmd($"config set XR_Cooldown {RaidCustomizer.RaidConfig.NormalRaidsCooldown.Value}");
                ServerCmd($"config set XR_BossCooldown {RaidCustomizer.RaidConfig.BossRaidsCooldown.Value}");
                
                // Stat multipliers
                ServerCmd($"config set XR_StatMultiplierEnabled {RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value}");
                ServerCmd($"config set XR_CannibalHealth {RaidCustomizer.RaidConfig.CannibalHealthMultiplier.Value}");
                ServerCmd($"config set XR_CannibalDamage {RaidCustomizer.RaidConfig.CannibalDamageMultiplier.Value}");
                ServerCmd($"config set XR_CreepHealth {RaidCustomizer.RaidConfig.CreepHealthMultiplier.Value}");
                ServerCmd($"config set XR_CreepDamage {RaidCustomizer.RaidConfig.CreepDamageMultiplier.Value}");
                ServerCmd($"config set XR_BossHealth {RaidCustomizer.RaidConfig.BossHealthMultiplier.Value}");
                ServerCmd($"config set XR_BossDamage {RaidCustomizer.RaidConfig.BossDamageMultiplier.Value}");
                
                // Single requeue after all config is set
                ServerCmd("raid requeue");
                
                // Save local config too
                try { Config.Save(); } catch { }
                
                SonsTools.ShowMessage("Raid config applied & requeued");
                RLog.Msg("[RaidCustomizer] Applied all raid config to server and requeued");
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] ApplyRaidConfigAndRequeue failed: {ex.Message}");
            }
        }
        
        // ═══════════════════════════════════════════════════════════════
        // Season/Weather: IL2CPP-safe overrides using Marshal offsets
        // AccessTools.Field is STRIPPED for SeasonsManager (confirmed in logs).
        // Must use Marshal.WriteInt32/ReadInt32 on IL2CPP object pointer.
        // Offsets from dump.cs:
        //   SeasonsManager._activeSeason   = 0x70 (int32 enum)
        //   SeasonsManager._seasonIsLocked  = 0x75 (bool)
        // Season uses PREFIX on LateUpdate (not postfix!) to write BEFORE
        // the game's season calculation runs, so it sees our locked state.
        // Weather uses WeatherSystem direct APIs (ForceRain/StopRaining).
        // ═══════════════════════════════════════════════════════════════
        
        private const int SEASON_ACTIVE_OFFSET = 0x70;
        private const int SEASON_LOCKED_OFFSET = 0x75;
        private const int SEASON_PREVIOUS_OFFSET = 0x7C;
        
        private static int? _forcedSeason = null; // null=no override, 0=Spring,1=Summer,2=Fall,3=Winter
        private static bool _seasonPatchApplied = false;
        private static System.Reflection.PropertyInfo _smPointerProp;
        private static HarmonyLib.Harmony _seasonHarmony;
        
        // Weather PREFIX override (same pattern as seasons)
        private static bool? _forcedRain = null; // null=no override, true=rain, false=sunny
        private static bool _weatherPatchApplied = false;
        private static HarmonyLib.Harmony _weatherHarmony;
        private static System.Reflection.PropertyInfo _wsPointerProp;
        private static System.Reflection.MethodInfo _wsStartRaining;
        private static System.Reflection.MethodInfo _wsStopRaining;
        
        /// <summary>
        /// Force season via Harmony PREFIX using Marshal offsets.
        /// PREFIX runs BEFORE LateUpdate so the game sees _seasonIsLocked=true
        /// and skips recalculation. Postfix was too late (confirmed: 1-second flash).
        /// </summary>
        private void LocalSetSeason(string season)
        {
            try
            {
                season = season.ToLower();
                if (season == "fall") season = "autumn";
                
                int seasonVal = season switch
                {
                    "spring" => 0,
                    "summer" => 1,
                    "autumn" => 2,
                    "winter" => 3,
                    _ => -1
                };
                
                if (seasonVal < 0) { RLog.Warning($"[ServerAdmin] Unknown season: {season}"); return; }
                
                // Apply Harmony patch on first use
                if (!_seasonPatchApplied)
                {
                    try
                    {
                        var smType = HarmonyLib.AccessTools.TypeByName("SeasonsManager");
                        if (smType == null) { RLog.Warning("[ServerAdmin] SeasonsManager type not found"); return; }
                        
                        // Get SeasonsManager instance to cache Pointer property
                        var sm = HarmonyLib.AccessTools.Property(smType, "Instance")?.GetValue(null);
                        if (sm != null)
                        {
                            _smPointerProp = sm.GetType().GetProperty("Pointer",
                                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy);
                            
                            RLog.Msg($"[ServerAdmin] Pointer property: {(_smPointerProp != null ? "found" : "NULL")}");
                            
                            if (_smPointerProp != null)
                            {
                                System.IntPtr ptr = (System.IntPtr)_smPointerProp.GetValue(sm);
                                int currentSeason = System.Runtime.InteropServices.Marshal.ReadInt32(ptr + SEASON_ACTIVE_OFFSET);
                                byte currentLocked = System.Runtime.InteropServices.Marshal.ReadByte(ptr + SEASON_LOCKED_OFFSET);
                                RLog.Msg($"[ServerAdmin] OFFSET VERIFY: _activeSeason={currentSeason}, _seasonIsLocked={currentLocked}");
                            }
                        }
                        else
                        {
                            RLog.Warning("[ServerAdmin] SeasonsManager.Instance is null");
                        }
                        
                        _seasonHarmony = new HarmonyLib.Harmony("ProjectX.SeasonOverride");
                        
                        // PREFIX on LateUpdate — writes our season value every frame
                        var lateUpdate = HarmonyLib.AccessTools.Method(smType, "LateUpdate");
                        if (lateUpdate != null)
                        {
                            var prefix = new HarmonyLib.HarmonyMethod(typeof(ProjectXGUI), nameof(SeasonsManagerLateUpdatePrefix));
                            _seasonHarmony.Patch(lateUpdate, prefix: prefix);
                            RLog.Msg("[ServerAdmin] Harmony PREFIX on SeasonsManager.LateUpdate applied");
                        }
                        
                        // PREFIX on UpdateTime — BLOCKS the game's time-based season recalculation
                        // UpdateTime is a virtual override from TimeOfDayBehaviour, called on a
                        // separate code path from LateUpdate. Without blocking this, it overrides
                        // our Marshal writes between frames.
                        var updateTime = HarmonyLib.AccessTools.Method(smType, "UpdateTime");
                        if (updateTime != null)
                        {
                            var utPrefix = new HarmonyLib.HarmonyMethod(typeof(ProjectXGUI), nameof(SeasonsManagerUpdateTimePrefix));
                            _seasonHarmony.Patch(updateTime, prefix: utPrefix);
                            RLog.Msg("[ServerAdmin] Harmony PREFIX on SeasonsManager.UpdateTime applied");
                        }
                        
                        // PREFIX on SetSeasonPostDeserialize — BLOCKS Bolt network season sync
                        // The server replicates season state to clients via this method,
                        // which overrides the visual systems even though our PREFIX holds _activeSeason.
                        // This is the primary cause of the "summer flickers back to winter" bug.
                        var postDeserialize = HarmonyLib.AccessTools.Method(smType, "SetSeasonPostDeserialize");
                        if (postDeserialize != null)
                        {
                            var pdPrefix = new HarmonyLib.HarmonyMethod(typeof(ProjectXGUI), nameof(SetSeasonPostDeserializePrefix));
                            _seasonHarmony.Patch(postDeserialize, prefix: pdPrefix);
                            RLog.Msg("[ServerAdmin] Harmony PREFIX on SeasonsManager.SetSeasonPostDeserialize applied");
                        }
                        
                        _seasonPatchApplied = (lateUpdate != null || updateTime != null);
                    }
                    catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] Harmony patch failed: {ex.Message}\n{ex.StackTrace}"); }
                }
                
                // Update forced season FIRST — PREFIX runs every frame and would
                // overwrite the transition back to the old season if we don't
                _forcedSeason = seasonVal;
                
                // CLEAR any active weather override — season takes priority
                // If user had "Sunny" active, _forcedRain=false blocks CheckForRain
                // which prevents the new season's natural weather (e.g. snow in winter)
                if (_forcedRain != null)
                {
                    RLog.Msg($"[ServerAdmin] Clearing weather override (_forcedRain was {_forcedRain}) — season takes priority");
                    _forcedRain = null;
                    // Re-enable rain system in case Sunny disabled it via enablerain off
                    PlayerActions.ToggleForceRain(false); // StopRaining to reset state
                    // Let the season's natural weather system take over
                }
                
                RLog.Msg($"[ServerAdmin] _forcedSeason set to {seasonVal} ({season}), calling PlayerActions.SetSeason...");
                
                // Trigger the season change via typed IL2CPP call
                // Set bypass flag so our SetSeasonPostDeserialize PREFIX allows our own call through
                _bypassDeserializeBlock = true;
                PlayerActions.SetSeason(season);
                
                // Readback verification — confirm memory state after typed IL2CPP call
                try
                {
                    var smType = HarmonyLib.AccessTools.TypeByName("SeasonsManager");
                    var sm = HarmonyLib.AccessTools.Property(smType, "Instance")?.GetValue(null);
                    if (sm != null && _smPointerProp != null)
                    {
                        System.IntPtr ptr = (System.IntPtr)_smPointerProp.GetValue(sm);
                        int readBack = System.Runtime.InteropServices.Marshal.ReadInt32(ptr + SEASON_ACTIVE_OFFSET);
                        byte readLocked = System.Runtime.InteropServices.Marshal.ReadByte(ptr + SEASON_LOCKED_OFFSET);
                        int readPrev = System.Runtime.InteropServices.Marshal.ReadInt32(ptr + SEASON_PREVIOUS_OFFSET);
                        RLog.Msg($"[ServerAdmin] POST-SetSeason readback: active={readBack}, locked={readLocked}, prev={readPrev}, forced={_forcedSeason}");
                    }
                }
                catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] Readback failed: {ex.Message}"); }
                
                RLog.Msg($"[ServerAdmin] ✓ Season FORCED to {season} (val={seasonVal}) — PREFIX holds every frame");
                
                // Also send to server — season change clears weather override
                ServerCmd($"world season {season}");
            }
            catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] LocalSetSeason error: {ex.Message}"); }
        }
        
        /// <summary>
        /// Harmony PREFIX on SeasonsManager.LateUpdate — runs BEFORE the game's
        /// season calculation. Writes _activeSeason and _seasonIsLocked=FALSE via
        /// Marshal offsets. _seasonIsLocked=false allows LateUpdate to detect
        /// _activeSeason != _previousSeason and fire the native SetSeason() call
        /// which notifies all visual receivers (ground, trees, snow, lighting).
        /// UpdateTime is blocked separately to prevent game-clock recalculation.
        /// (Postfix was too late — caused 1-second flash, confirmed in testing.)
        /// </summary>
        private static int _prefixLogCounter = 0;
        private static void SeasonsManagerLateUpdatePrefix(object __instance)
        {
            if (_forcedSeason == null || _smPointerProp == null) return;
            
            try
            {
                System.IntPtr ptr = (System.IntPtr)_smPointerProp.GetValue(__instance);
                if (ptr == System.IntPtr.Zero) return;
                
                // Read CURRENT value before we write (for diagnostics)
                int currentSeason = System.Runtime.InteropServices.Marshal.ReadInt32(ptr + SEASON_ACTIVE_OFFSET);
                
                // Write _activeSeason and _seasonIsLocked=FALSE
                // KEY INSIGHT: _seasonIsLocked=false lets LateUpdate detect _activeSeason != _previousSeason
                // and call the game's native SetSeason() which fires UpdateReceiversSeason() to notify
                // ALL visual systems (ground snow, trees, lighting, weather).
                // This is the same typed IL2CPP call pattern that makes the Sunny toggle work.
                // UpdateTime is blocked separately to prevent game-clock recalculation.
                // We do NOT write _previousSeason — let LateUpdate see the mismatch and fire receivers.
                System.Runtime.InteropServices.Marshal.WriteInt32(ptr + SEASON_ACTIVE_OFFSET, _forcedSeason.Value);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr + SEASON_LOCKED_OFFSET, 0);
                
                // Log once every ~300 frames (approx every 5 seconds) to reduce spam
                _prefixLogCounter++;
                if (_prefixLogCounter % 300 == 1 || currentSeason != _forcedSeason.Value)
                {
                    RLog.Msg($"[ServerAdmin] LateUpdate PREFIX: was={currentSeason}, wrote={_forcedSeason.Value}, drift={currentSeason != _forcedSeason.Value}");
                }
            }
            catch { } // Silent — runs every frame
        }
        
        /// <summary>
        /// Harmony PREFIX on SeasonsManager.UpdateTime — BLOCKS the game's
        /// time-based season recalculation when a forced season is active.
        /// UpdateTime is called from TimeOfDayBehaviour on a separate path
        /// from LateUpdate, and was overriding our Marshal writes.
        /// </summary>
        private static int _updateTimeBlockCount = 0;
        private static bool SeasonsManagerUpdateTimePrefix()
        {
            if (_forcedSeason == null) return true; // No override, run original
            
            // Block UpdateTime — it recalculates season from game clock.
            // Receiver notifications are fired via typed SetSeasonPostDeserialize call in LocalSetSeason.
            _updateTimeBlockCount++;
            if (_updateTimeBlockCount % 300 == 1)
            {
                RLog.Msg($"[ServerAdmin] UpdateTime BLOCKED (forced={_forcedSeason.Value}, blocks={_updateTimeBlockCount})");
            }
            return false;
        }
        
        /// <summary>
        /// Harmony PREFIX on SeasonsManager.SetSeasonPostDeserialize — serves dual purpose:
        /// 1. ALLOWS our own typed IL2CPP call through (via _bypassDeserializeBlock flag)
        ///    to fire the full visual pipeline including UpdateReceiversSeason().
        /// 2. BLOCKS Bolt network season sync from overriding our forced visual state.
        /// </summary>
        private static int _postDeserializeBlockCount = 0;
        private static bool _bypassDeserializeBlock = false; // Allow our own typed call through
        private static bool SetSeasonPostDeserializePrefix()
        {
            if (_forcedSeason == null) return true; // No override, run original
            
            // Allow our own typed call through
            if (_bypassDeserializeBlock)
            {
                _bypassDeserializeBlock = false;
                RLog.Msg($"[ServerAdmin] SetSeasonPostDeserialize ALLOWED (our own typed call, forced={_forcedSeason.Value})");
                return true;
            }
            
            // Block Bolt's network season sync — it would override our forced visual state
            _postDeserializeBlockCount++;
            if (_postDeserializeBlockCount % 300 == 1)
            {
                RLog.Msg($"[ServerAdmin] SetSeasonPostDeserialize BLOCKED (forced={_forcedSeason.Value}, blocks={_postDeserializeBlockCount})");
            }
            return false;
        }
        
        /// <summary>
        /// Change weather using typed API + Harmony PREFIX blocker.
        /// PlayerActions.ToggleForceRain uses typed IL2CPP calls (ForceRain/StopRaining)
        /// which work. PREFIX blocks CheckForRain from overriding our forced state.
        /// Reflection Invoke() fails silently in IL2CPP — do NOT use it.
        /// </summary>
        private void LocalSetWeather(string mode)
        {
            try
            {
                bool rainOn = (mode == "on" || mode == "rain");
                
                // Apply Harmony patch on first use — blocks CheckForRain dice roll
                if (!_weatherPatchApplied)
                {
                    try
                    {
                        var wsType = HarmonyLib.AccessTools.TypeByName("TheForest.World.WeatherSystem");
                        if (wsType == null) { RLog.Warning("[ServerAdmin] WeatherSystem type not found"); return; }
                        
                        var checkForRain = HarmonyLib.AccessTools.Method(wsType, "CheckForRain");
                        if (checkForRain != null)
                        {
                            _weatherHarmony = new HarmonyLib.Harmony("ProjectX.WeatherOverride");
                            var prefix = new HarmonyLib.HarmonyMethod(typeof(ProjectXGUI), nameof(WeatherSystemCheckForRainPrefix));
                            _weatherHarmony.Patch(checkForRain, prefix: prefix);
                            _weatherPatchApplied = true;
                            RLog.Msg("[ServerAdmin] \u2713 Harmony PREFIX on WeatherSystem.CheckForRain applied!");
                        }
                        else
                        {
                            RLog.Warning("[ServerAdmin] WeatherSystem.CheckForRain not found");
                        }
                    }
                    catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] Weather patch failed: {ex.Message}\n{ex.StackTrace}"); }
                }
                
                // Set the forced weather — PREFIX will BLOCK CheckForRain from overriding
                _forcedRain = rainOn;
                
                // Typed IL2CPP call for initial trigger (reflection Invoke fails silently!)
                PlayerActions.ToggleForceRain(rainOn);
                RLog.Msg($"[ServerAdmin] \u2713 Weather FORCED to {(rainOn ? "Rain" : "Sunny")} — typed call + PREFIX blocker");
                
                // Also send to server for other clients
                string weatherName = rainOn ? "rain" : "sunny";
                ServerCmd($"world weather {weatherName}");
            }
            catch (System.Exception ex) { RLog.Warning($"[ServerAdmin] LocalSetWeather error: {ex.Message}"); }
        }
        
        /// <summary>
        /// Harmony PREFIX on WeatherSystem.CheckForRain — BLOCKER ONLY.
        /// When _forcedRain is set, skips the original dice roll so the game
        /// can't override the weather state we set via PlayerActions.ToggleForceRain.
        /// Does NOT invoke methods via reflection (fails silently in IL2CPP).
        /// </summary>
        private static bool WeatherSystemCheckForRainPrefix(object __instance)
        {
            if (_forcedRain == null) return true; // No override, run original
            return false; // Block CheckForRain — preserve forced weather state
        }
        
        private void DrawServerAdminPanel()
        {
            try
            {
                DrawDivider("WORLD CONTROL");
                
                // Time of day — sent to server via /px world time
                GUILayout.BeginHorizontal();
                GUILayout.Label("Time of Day:", ProjectXStyles.NormalLabel, GUILayout.Width(120));
                _timeInput = GUILayout.TextField(_timeInput ?? "12:00", ProjectXStyles.InputField, GUILayout.Width(80));
                if (GUILayout.Button("Set Time", ProjectXStyles.Button, GUILayout.Height(45), GUILayout.Width(140)))
                {
                    // Strip HH:MM to just hours — server expects float (e.g., "12" not "12:00")
                    string timeForCmd = _timeInput?.Trim() ?? "12";
                    if (timeForCmd.Contains(":"))
                    {
                        var parts = timeForCmd.Split(':');
                        if (float.TryParse(parts[0], out float h))
                        {
                            float mins = parts.Length > 1 && float.TryParse(parts[1], out float m) ? m / 60f : 0;
                            timeForCmd = (h + mins).ToString("F1");
                        }
                    }
                    ServerCmd($"world time {timeForCmd}");
                }
                GUILayout.EndHorizontal();
                
                // Lock Time of Day — routed to server via /px world locktime
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Lock Time", ProjectXStyles.Button, GUILayout.Height(45), GUILayout.Width(140)))
                    ServerCmd("world locktime on");
                if (GUILayout.Button("Unlock Time", ProjectXStyles.Button, GUILayout.Height(45), GUILayout.Width(140)))
                    ServerCmd("world locktime off");
                GUILayout.EndHorizontal();
                
                // Daytime Speed — routed to server via /px world speed
                GUILayout.BeginHorizontal();
                GUILayout.Label("Speed:", ProjectXStyles.NormalLabel, GUILayout.Width(70));
                _speedInput = GUILayout.TextField(_speedInput ?? "1", ProjectXStyles.InputField, GUILayout.Width(60));
                if (GUILayout.Button("Set", ProjectXStyles.Button, GUILayout.Width(75), GUILayout.Height(45)))
                {
                    if (float.TryParse(_speedInput, out float spd))
                        ServerCmd($"world speed {spd}");
                }
                if (GUILayout.Button("0.5x", ProjectXStyles.Button, GUILayout.Width(70), GUILayout.Height(45))) ServerCmd("world speed 0.5");
                if (GUILayout.Button("1x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) ServerCmd("world speed 1");
                if (GUILayout.Button("2x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) ServerCmd("world speed 2");
                if (GUILayout.Button("5x", ProjectXStyles.Button, GUILayout.Width(55), GUILayout.Height(45))) ServerCmd("world speed 5");
                GUILayout.EndHorizontal();
                
                // Season buttons — execute LOCALLY (season is a client-side visual system)
                // Server-side execution (SeasonsManager, _season(), SendCommand) all fail silently
                // Client direct call proven working in PlayerActions.cs:285
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Spring", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetSeason("spring");
                if (GUILayout.Button("Summer", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetSeason("summer");
                if (GUILayout.Button("Fall", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetSeason("autumn");
                if (GUILayout.Button("Winter", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetSeason("winter");
                if (GUILayout.Button("Unlock", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    _forcedSeason = null; // Release local PREFIX lock
                    _forcedRain = null;   // Also clear weather override
                    ServerCmd("world season unlock");
                }
                GUILayout.EndHorizontal();
                
                // Weather buttons — execute LOCALLY (weather is a client-side visual system)
                // Uses _forcerain() on DebugConsole (discovered via method dump)
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Sunny", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetWeather("off");
                if (GUILayout.Button("Rain", ProjectXStyles.Button, GUILayout.Height(45)))
                    LocalSetWeather("on");
                if (GUILayout.Button("Snow", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    // Set local overrides directly (no ServerCmd from each helper)
                    _forcedSeason = 3; // winter
                    _forcedRain = true; // PREFIX blocker
                    PlayerActions.ToggleForceRain(true); // typed IL2CPP trigger
                    // Single server command handles both season + rain
                    ServerCmd("world weather snow");
                }
                GUILayout.EndHorizontal();
                
                // AI Freeze toggle — /px world freeze
                if (GUILayout.Button("Toggle AI Freeze", ProjectXStyles.Button, GUILayout.Height(45)))
                    ServerCmd("world freeze");
                
                // Tree regrow — /px world trees
                if (GUILayout.Button("Force Tree Regrow", ProjectXStyles.Button, GUILayout.Height(45)))
                    ServerCmd("world trees");
                
                DrawDivider("BUILDING CHEATS");
                
                // Instant Book Build toggle — execute locally + dispatch to server
                bool newInstantBuild = DrawCheckbox("Instant Book Build", Modules.Building.BuilderEnhancements.InstantBuild);
                if (newInstantBuild != Modules.Building.BuilderEnhancements.InstantBuild)
                {
                    Modules.Building.BuilderEnhancements.InstantBuild = newInstantBuild;
                    ServerCmd($"building instantbuild {(newInstantBuild ? "on" : "off")}");
                }
                
                // AI Ghost Player toggle — execute locally + dispatch to server
                bool newGhostAdmin = DrawCheckbox("AI Ghost Player", Modules.Building.BuilderEnhancements.AIGhostPlayer);
                if (newGhostAdmin != Modules.Building.BuilderEnhancements.AIGhostPlayer)
                {
                    Modules.Building.BuilderEnhancements.AIGhostPlayer = newGhostAdmin;
                    ServerCmd($"building ghost {(newGhostAdmin ? "on" : "off")}");
                }
                
                // Blueprint action buttons — execute locally + dispatch to server
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Cancel Blueprints", ProjectXStyles.Button, GUILayout.Height(42)))
                {
                    Modules.Building.BuilderEnhancements.CancelBlueprints();
                    ServerCmd("building cancel");
                }
                if (GUILayout.Button("Finish Blueprints", ProjectXStyles.Button, GUILayout.Height(42)))
                {
                    Modules.Building.BuilderEnhancements.FinishBlueprints();
                    ServerCmd("building finish");
                }
                GUILayout.EndHorizontal();
                
                DrawDivider("COMPANIONS");
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Revive Kelvin", ProjectXStyles.Button, GUILayout.Height(42)))
                    ServerCmd("world revive kelvin");
                if (GUILayout.Button("Revive Virginia", ProjectXStyles.Button, GUILayout.Height(42)))
                    ServerCmd("world revive virginia");
                GUILayout.EndHorizontal();
                
                // Follower health multiplier sliders
                float kelvinOld = RaidCustomizer.RaidConfig.KelvinHealthMultiplier.Value;
                float kelvinNew = DrawSlider("Kelvin HP Multi", kelvinOld, 1f, 200f);
                if (Math.Abs(kelvinNew - kelvinOld) > 0.05f)
                    RaidCustomizer.RaidConfig.KelvinHealthMultiplier.Value = kelvinNew;
                
                float virginiaOld = RaidCustomizer.RaidConfig.VirginiaHealthMultiplier.Value;
                float virginiaNew = DrawSlider("Virginia HP Multi", virginiaOld, 1f, 200f);
                if (Math.Abs(virginiaNew - virginiaOld) > 0.05f)
                    RaidCustomizer.RaidConfig.VirginiaHealthMultiplier.Value = virginiaNew;
                

                
                DrawDivider("LOOT RESPAWN");
                
                // Loot toggle — local mod state
                try
                {
                    bool lootEnabled = DrawCheckbox("Loot Respawn Enabled", LootRespawn.LootRespawnModule.Enabled);
                    if (lootEnabled != LootRespawn.LootRespawnModule.Enabled)
                    {
                        LootRespawn.LootRespawnModule.Enabled = lootEnabled;
                        Config.Save();
                    }
                }
                catch { GUILayout.Label("Loot module not loaded", ProjectXStyles.NormalLabel); }
                
                // Respawn days — text input + button (single update, no slider spam)
                GUILayout.BeginHorizontal();
                GUILayout.Label("Respawn Days:", ProjectXStyles.NormalLabel, GUILayout.Width(120));
                _respawnDaysInput = GUILayout.TextField(_respawnDaysInput ?? "3", ProjectXStyles.InputField, GUILayout.Width(60));
                if (GUILayout.Button("Set", ProjectXStyles.Button, GUILayout.Height(45), GUILayout.Width(80)))
                {
                    string val = _respawnDaysInput?.Trim() ?? "3";
                    if (int.TryParse(val, out int days) && days >= 1 && days <= 100)
                    {
                        ServerCmd($"config set LootRespawnDays {days}");
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset Loot Tracker", ProjectXStyles.Button, GUILayout.Height(45)))
                    ServerCmd("loot reset");
                if (GUILayout.Button("Refresh Server Status", ProjectXStyles.Button, GUILayout.Height(45)))
                    LootRespawn.LootEventListener.RequestServerStatus();
                GUILayout.EndHorizontal();
                
                // Status display — prefer cached server data, fall back to local
                try {
                    string serverStatus = LootRespawn.LootEventListener.ServerStatusText;
                    string status = !string.IsNullOrEmpty(serverStatus) 
                        ? $"Server: {serverStatus}" 
                        : LootRespawn.LootRespawnModule.GetStatus();
                    GUILayout.Label(status, ProjectXStyles.NormalLabel);
                } catch { /* module not loaded */ }
                
                DrawDivider("SERVER");
                
                // Structure Durability — local config
                if (Config.StructureDurabilityMultiplier != null)
                {
                    Config.StructureDurabilityMultiplier.Value = DrawSlider("Structure Durability", Config.StructureDurabilityMultiplier.Value, 0.1f, 100f);
                }
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Force Save World", ProjectXStyles.Button, GUILayout.Height(45)))
                    ServerCmd("save");
                if (GUILayout.Button("Save Config", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    try
                    {
                        // Send follower health multipliers to server
                        ServerCmd($"config set XR_KelvinHealth {RaidCustomizer.RaidConfig.KelvinHealthMultiplier.Value}");
                        ServerCmd($"config set XR_VirginiaHealth {RaidCustomizer.RaidConfig.VirginiaHealthMultiplier.Value}");
                        Config.Save();
                        RLog.Msg("[ServerAdmin] Config saved");
                    }
                    catch {}
                }
                GUILayout.EndHorizontal();
                
                if (GUILayout.Button("Show Server Status", ProjectXStyles.Button, GUILayout.Height(45)))
                    ServerCmd("status");
            }
            catch (System.Exception ex)
            {
                GUILayout.Label($"Server Admin panel error: {ex.Message}", ProjectXStyles.NormalLabel);
                RLog.Warning($"[ServerAdmin] Panel render error: {ex}");
            }
        }
        
        private void DrawServerRaidsPanel()
        {
            try
            {
                DrawDivider("RAID SCHEDULE");
                
                // Time of day toggles — local only, sent to server on Apply
                GUILayout.BeginHorizontal();
                bool morning = DrawCheckbox("Morning", RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value);
                if (morning != RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value)
                    RaidCustomizer.RaidConfig.SearchPartiesAtMorning.Value = morning;
                bool dayTime = DrawCheckbox("Day", RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value);
                if (dayTime != RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value)
                    RaidCustomizer.RaidConfig.SearchPartiesAtDay.Value = dayTime;
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                bool evening = DrawCheckbox("Evening", RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value);
                if (evening != RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value)
                    RaidCustomizer.RaidConfig.SearchPartiesAtEvening.Value = evening;
                bool night = DrawCheckbox("Night", RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value);
                if (night != RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value)
                    RaidCustomizer.RaidConfig.SearchPartiesAtNight.Value = night;
                GUILayout.EndHorizontal();
                
                // Raids per day — local only
                RaidCustomizer.RaidConfig.RaidsPerDay.Value = (int)DrawSlider("Raids/Day (-1=Def)", RaidCustomizer.RaidConfig.RaidsPerDay.Value, -1, 24);
                
                // Raid Distribution (radio-style: only one active at a time)
                GUILayout.BeginHorizontal();
                string srvDist = RaidCustomizer.RaidConfig.RaidDistribution.Value;
                bool srvEvenly = DrawCheckbox("Evenly", srvDist == "Evenly");
                if (srvEvenly && srvDist != "Evenly") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Evenly";
                bool srvRandomly = DrawCheckbox("Randomly", srvDist == "Randomly");
                if (srvRandomly && srvDist != "Randomly") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Randomly";
                bool srvStacked = DrawCheckbox("Stacked", srvDist == "Stacked");
                if (srvStacked && srvDist != "Stacked") RaidCustomizer.RaidConfig.RaidDistribution.Value = "Stacked";
                GUILayout.EndHorizontal();
                
                // Consider Current Time
                bool srvConsiderTime = DrawCheckbox("Consider Current Time", RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value);
                if (srvConsiderTime != RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value)
                    RaidCustomizer.RaidConfig.ConsiderCurrentTime.Value = srvConsiderTime;
                
                DrawDivider("ENEMY TYPES");
                
                GUILayout.BeginHorizontal();
                bool cannibals = DrawCheckbox("Allow Cannibals", RaidCustomizer.RaidConfig.AllowCannibals.Value);
                if (cannibals != RaidCustomizer.RaidConfig.AllowCannibals.Value)
                    RaidCustomizer.RaidConfig.AllowCannibals.Value = cannibals;
                bool creepy = DrawCheckbox("Allow Creepy", RaidCustomizer.RaidConfig.AllowCreepy.Value);
                if (creepy != RaidCustomizer.RaidConfig.AllowCreepy.Value)
                    RaidCustomizer.RaidConfig.AllowCreepy.Value = creepy;
                bool muddies = DrawCheckbox("Allow Muddies", RaidCustomizer.RaidConfig.AllowMuddies.Value);
                if (muddies != RaidCustomizer.RaidConfig.AllowMuddies.Value)
                    RaidCustomizer.RaidConfig.AllowMuddies.Value = muddies;
                GUILayout.EndHorizontal();
                
                DrawDivider("SPAWN SETTINGS");
                
                RaidCustomizer.RaidConfig.SpawnCountFactor.Value = DrawSlider("Min Spawn Factor", RaidCustomizer.RaidConfig.SpawnCountFactor.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.MaxSpawnCountFactor.Value = DrawSlider("Max Spawn Factor", RaidCustomizer.RaidConfig.MaxSpawnCountFactor.Value, 0.1f, 10f);
                RaidCustomizer.RaidConfig.EnemyLimit.Value = (int)DrawSlider("Enemy Limit", RaidCustomizer.RaidConfig.EnemyLimit.Value, 1, 30);
                RaidCustomizer.RaidConfig.BossSpawnCount.Value = (int)DrawSlider("Boss Spawn Count", RaidCustomizer.RaidConfig.BossSpawnCount.Value, 1, 30);
                
                DrawDivider("COOLDOWNS");
                
                RaidCustomizer.RaidConfig.NormalRaidsCooldown.Value = (int)DrawSlider("Raid Cooldown (-1=Def)", RaidCustomizer.RaidConfig.NormalRaidsCooldown.Value, -1, 50);
                RaidCustomizer.RaidConfig.BossRaidsCooldown.Value = (int)DrawSlider("Boss Cooldown (-1=Def)", RaidCustomizer.RaidConfig.BossRaidsCooldown.Value, -1, 50);
                
                DrawDivider("STAT MULTIPLIERS");
                
                bool enableStats = DrawCheckbox("Enable Stat Overrides", RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value);
                if (enableStats != RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value)
                    RaidCustomizer.RaidConfig.StatMultiplierModificationEnabled.Value = enableStats;
                
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
                    RaidCustomizer.RaidConfig.AnnounceIncomingSearchParties.Value = announce;
                
                DrawDivider("RAID ACTIONS");
                
                // Quick actions — execute locally for feedback + dispatch to dedicated server
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Start Random Raid", ProjectXStyles.Button, GUILayout.Height(42)))
                {
                    try { RaidCustomizer.RaidActions.RunRandomRaid(); } catch { }
                    ServerCmd("raid start");
                }
                if (GUILayout.Button("Clear Queued Raids", ProjectXStyles.Button, GUILayout.Height(42)))
                {
                    try { RaidCustomizer.RaidActions.ClearQueuedRaids(); } catch { }
                    ServerCmd("raid clear");
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear All Events", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    try { RaidCustomizer.RaidActions.ClearAllEvents(); } catch { }
                    ServerCmd("raid clearall");
                }
                if (GUILayout.Button("Clear Cooldowns", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    try { RaidCustomizer.RaidActions.ClearRaidCooldowns(); } catch { }
                    ServerCmd("raid cooldown");
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply & Requeue", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    ApplyRaidConfigAndRequeue();
                }
                if (GUILayout.Button("Show Raid Status", ProjectXStyles.Button, GUILayout.Height(45)))
                {
                    try { RaidCustomizer.RaidActions.PrintQueuedRaids(); } catch { }
                    ServerCmd("raid status");
                }
                GUILayout.EndHorizontal();
            }
            catch (System.Exception ex)
            {
                GUILayout.Label($"Server Raids panel error: {ex.Message}", ProjectXStyles.NormalLabel);
                RLog.Warning($"[ServerRaids] Panel render error: {ex}");
            }
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
            float sliderLabelW = PANEL_WIDTH * 0.35f;
            float sliderBarW = PANEL_WIDTH * 0.42f;
            float sliderValW = PANEL_WIDTH * 0.08f;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, ProjectXStyles.NormalLabel, GUILayout.Width(sliderLabelW));
            float newVal = GUILayout.HorizontalSlider(value, min, max, 
                ProjectXStyles.HorizontalSlider, ProjectXStyles.HorizontalSliderThumb, 
                GUILayout.Width(sliderBarW));
            GUILayout.Label(newVal.ToString("F1"), ProjectXStyles.ValueLabel, GUILayout.Width(sliderValW));
            GUILayout.EndHorizontal();
            return newVal;
        }
        
        private int DrawIntSlider(string label, int value, int min, int max)
        {
            float sliderLabelW = PANEL_WIDTH * 0.35f;
            float sliderBarW = PANEL_WIDTH * 0.42f;
            float sliderValW = PANEL_WIDTH * 0.08f;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, ProjectXStyles.NormalLabel, GUILayout.Width(sliderLabelW));
            int newVal = (int)Math.Round(GUILayout.HorizontalSlider(value, min, max, 
                ProjectXStyles.HorizontalSlider, ProjectXStyles.HorizontalSliderThumb, 
                GUILayout.Width(sliderBarW)));
            GUILayout.Label(newVal.ToString(), ProjectXStyles.ValueLabel, GUILayout.Width(sliderValW));
            GUILayout.EndHorizontal();
            return newVal;
        }
        
        private bool DrawButton(string label)
        {
            return GUILayout.Button(label, ProjectXStyles.Button, GUILayout.Height(32));
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
