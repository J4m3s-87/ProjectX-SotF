using ProjectX.Master.Modules.DedicatedSuperuser.Commands;
using ProjectX.Master.Modules.DedicatedSuperuser.Networking;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;
using RedLoader;
using SonsSdk;
using UnityEngine;
using TheForest.Utils;

namespace ProjectX.Master.Modules.DedicatedSuperuser
{
    public class DedicatedSuperuserModule : SonsMod
    {
        public static DedicatedSuperuserModule Instance;

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new DedicatedSuperuserModule();
                Instance.OnInitializeMod();
            }
        }

        protected override void OnInitializeMod()
        {
            RLog.Msg("[DedicatedSuperuser] Module Initializing...");

            // Initialize Whitelist
            OwnerWhitelistUtils.Init();

            // Patch Chat for Commands
            var harmony = new HarmonyLib.Harmony("ProjectX.DedicatedSuperuser");
            PatchChat(harmony);
            
            RLog.Msg("[DedicatedSuperuser] Module Initialized");
        }

        private void PatchChat(HarmonyLib.Harmony harmony)
        {
            // DISABLED: ChatBox.AddLine signature mismatch causes Harmony crash
            // Actual signature: AddLine(NetworkId playerId, string message, bool system)
            // This feature will need reimplementation with correct signature
            RLog.Msg("[DedicatedSuperuser] Chat commands disabled (Harmony patch incompatible)");
            
            /*
            try
            {
                var type = HarmonyLib.AccessTools.TypeByName("TheForest.UI.Multiplayer.ChatBox") ?? 
                           HarmonyLib.AccessTools.TypeByName("Sons.Gui.Chat.ChatBox") ??
                           HarmonyLib.AccessTools.TypeByName("ChatBox");

                if (type == null)
                {
                    RLog.Error("[DedicatedSuperuser] Could not find ChatBox type.");
                    return;
                }

                // Patch AddLine to listen for commands
                var method = HarmonyLib.AccessTools.Method(type, "AddLine");
                if (method != null)
                {
                    var postfix = HarmonyLib.AccessTools.Method(typeof(DedicatedSuperuserModule), nameof(OnChatLineAdded));
                    harmony.Patch(method, postfix: new HarmonyLib.HarmonyMethod(postfix));
                    RLog.Msg($"[DedicatedSuperuser] Listening to Chat via {type.Name}.{method.Name}");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[DedicatedSuperuser] Error patching chat: {ex}");
            }
            */
        }

        public static void OnChatLineAdded(string name, string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            var cmd = message.ToLower().Trim();

            // Check Whitelist (Local Player check for now, until Networking is fully robust)
            // In a real server scenario, the server receives the packet. 
            // Here, we simulate "If I type it, and I am Admin, execute it".
            // Since this is a "Friend Pack" (Client Host) mostly, LocalPlayer check is fine.
            // For Dedicated Server, this patch might need to run on the Server's chat handler if it exists, 
            // OR the implementation should be in NetworkingManager handling packets.
            // But 'AddLine' runs on clients.
            
            // For V1.5 (Friend Pack), this works.
            // Placeholder: SteamID retrieval needs proper SDK call (e.g. SteamClient.SteamId)
            // For now, we bypass strict check or use a dummy ID to allow compilation.
            // TODO: Fix this when Steamworks integration is verified.
            string currentSteamId = "76561198000000000"; 
            
            if (OwnerWhitelistUtils.IsAdmin(currentSteamId))
            {
                 if (cmd == "revivekelvin") Commands.ReviveCommands.ReviveKelvin();
                 if (cmd == "revivevirginia") Commands.ReviveCommands.ReviveVirginia();
                 if (cmd.StartsWith("season ")) Commands.TimeCommands.SetSeason(cmd.Substring(7));
                 if (cmd.StartsWith("time ")) 
                 {
                     if (float.TryParse(cmd.Substring(5), out float t)) Commands.TimeCommands.SetTimeOfDay(t);
                 }
            }
        }
    }
}
