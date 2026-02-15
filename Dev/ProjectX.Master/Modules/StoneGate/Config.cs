using System.Collections.Generic;
using RedLoader;
using Sons.Gui;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Testing;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate
{
    public static class Config
    {
        public static ConfigCategory Category { get; private set; }
        public static ConfigEntry<bool> LoggingToConsole { get; private set; }
        public static KeybindConfigEntry PrimaryAction { get; private set; }
        public static KeybindConfigEntry CycleAction { get; private set; }
        public static KeybindConfigEntry FinishAction { get; private set; }
        
        // Original allowed hits — these are actual GameObject root names in the game
        public static HashSet<string> allowedHits = new HashSet<string> { "RockWall", "RockPilar", "RockBeam" };

        public static void Init()
        {
            Category = ConfigSystem.CreateFileCategory("StoneGate", "StoneGate Settings", "StoneGate.cfg");
            LoggingToConsole = Category.CreateEntry<bool>("stone_gate_logging", false, "Enable Console Logs", "Enable Console Logs To Console", false, false, null, null);
            LoggingToConsole.DefaultValue = false;

#if !SERVER
            // Keybind: Primary Action (Hit Tool Key)
            PrimaryAction = Category.CreateKeybindEntry("stone_gate_primary", "<Mouse>/leftButton", "Hit Tool Key", "Key that makes the Gate Tool useable (DEFAULT Left Click).", false, false, null, null);
            PrimaryAction.DefaultValue = "<Mouse>/leftButton";
            ModInputCache.Notify(PrimaryAction, () => ActiveItem.OnKeyPress(), null);

            // Keybind: Cycle Mode
            CycleAction = Category.CreateKeybindEntry("stone_gate_cycle", "c", "Change Tool Mode", "Key changes tool mode (DEFAULT C).", false, false, null, null);
            CycleAction.DefaultValue = "c";
            ModInputCache.Notify(CycleAction, () => UiController.ChangeMode(), null);

            // Keybind: Finish / Open-Close Door
            FinishAction = Category.CreateKeybindEntry("stone_gate_finish", "e", "Open-Close Door / Finish Gate Key", "Open-Close Door / Finish Gate Key (DEFAULT E) NOTE: Finish UI Key Does Not Update, but key works", false, false, null, null);
            FinishAction.DefaultValue = "e";
            ModInputCache.Notify(FinishAction, () =>
            {
                if (!LocalPlayer.IsInWorld || LocalPlayer.IsInInventory || PauseMenu.IsActive || LocalPlayer.InWater) return;

                if (ActiveItem.active != null)
                {
                    ActiveItem.active.Complete();
                    if (Settings.logOnFinishOpenCloseDoorKey)
                        Misc.Msg("[Config] [FinishAction] ActiveItem.active.Complete()", false);
                }
                else
                {
                    var storedParent = CreateGateParent.Instance.StoredParent;
                    if (storedParent != null)
                    {
                        var children = SonsSdk.CommonExtensions.GetChildren(storedParent);
                        foreach (var transform in children)
                        {
                            var component = transform.GetComponent<Mono.StoneGateStoreMono>();
                            if (component != null && component.LinkUiElement != null && component.LinkUiElement.IsActive)
                            {
                                component.ToggleGate(true);
                                break;
                            }
                        }
                    }
                }
            }, null);
#endif
        }

        public static void OnSettingsUiClosed()
        {
        }
    }
}
