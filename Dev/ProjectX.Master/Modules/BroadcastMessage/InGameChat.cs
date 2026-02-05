using System;
using System.Reflection;
using RedLoader;

namespace ProjectX.Master.Modules.BroadcastMessage
{
    /// <summary>
    /// Utility for sending messages to the in-game chat box.
    /// Uses reflection to safely call ChatBox.AddLine without Harmony patching.
    /// </summary>
    public static class InGameChat
    {
        private static Type _chatBoxType;
        private static MethodInfo _addLineMethod;
        private static object _chatBoxInstance;
        private static bool _initialized;
        private static bool _initFailed;

        /// <summary>
        /// Initialize the chat reflection. Called lazily on first use.
        /// </summary>
        private static bool EnsureInitialized()
        {
            if (_initialized) return !_initFailed;
            if (_initFailed) return false;

            _initialized = true;

            try
            {
                // Try to find the ChatBox type
                _chatBoxType = HarmonyLib.AccessTools.TypeByName("Sons.Gui.Chat.ChatBox") ??
                               HarmonyLib.AccessTools.TypeByName("TheForest.UI.Multiplayer.ChatBox") ??
                               HarmonyLib.AccessTools.TypeByName("ChatBox");

                if (_chatBoxType == null)
                {
                    RLog.Warning("[InGameChat] Could not find ChatBox type.");
                    _initFailed = true;
                    return false;
                }

                // Find AddLine method - signature: AddLine(NetworkId playerId, string message, bool system)
                _addLineMethod = HarmonyLib.AccessTools.Method(_chatBoxType, "AddLine");
                
                if (_addLineMethod == null)
                {
                    RLog.Warning("[InGameChat] Could not find AddLine method.");
                    _initFailed = true;
                    return false;
                }

                RLog.Msg($"[InGameChat] Initialized with {_chatBoxType.Name}.{_addLineMethod.Name}");
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[InGameChat] Init failed: {ex.Message}");
                _initFailed = true;
                return false;
            }
        }

        /// <summary>
        /// Get the ChatBox singleton instance.
        /// </summary>
        private static object GetChatBoxInstance()
        {
            if (_chatBoxInstance != null) return _chatBoxInstance;

            try
            {
                // Try to find Instance property or field
                var instanceProp = HarmonyLib.AccessTools.Property(_chatBoxType, "Instance");
                if (instanceProp != null)
                {
                    _chatBoxInstance = instanceProp.GetValue(null);
                }
                else
                {
                    var instanceField = HarmonyLib.AccessTools.Field(_chatBoxType, "_instance") ??
                                        HarmonyLib.AccessTools.Field(_chatBoxType, "instance");
                    if (instanceField != null)
                    {
                        _chatBoxInstance = instanceField.GetValue(null);
                    }
                }

                // If no singleton, try FindObjectOfType
                if (_chatBoxInstance == null)
                {
                    _chatBoxInstance = UnityEngine.Object.FindObjectOfType(_chatBoxType);
                }

                return _chatBoxInstance;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[InGameChat] Could not get ChatBox instance: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Send a system message to the in-game chat.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="asSystem">If true, shows as system message (no player name)</param>
        public static void SendMessage(string message, bool asSystem = true)
        {
            if (!EnsureInitialized()) return;

            try
            {
                var instance = GetChatBoxInstance();
                if (instance == null)
                {
                    // ChatBox might not exist yet (e.g., in main menu)
                    return;
                }

                // Get default NetworkId (0 for system messages)
                var networkIdType = HarmonyLib.AccessTools.TypeByName("Bolt.NetworkId") ?? 
                                    HarmonyLib.AccessTools.TypeByName("NetworkId");

                object networkId = null;
                if (networkIdType != null)
                {
                    // Create default NetworkId
                    networkId = Activator.CreateInstance(networkIdType);
                }

                // Call AddLine(NetworkId, string, bool)
                _addLineMethod.Invoke(instance, new object[] { networkId, message, asSystem });
            }
            catch (Exception ex)
            {
                RLog.Warning($"[InGameChat] SendMessage failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Send the welcome message to a connecting player.
        /// Breaks the message into multiple lines for readability.
        /// </summary>
        public static void SendWelcome(string playerName = null)
        {
            // Send welcome in multiple lines for better readability
            SendMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            SendMessage("🎮 WELCOME TO PROJECT X");
            if (!string.IsNullOrEmpty(playerName))
            {
                SendMessage($"Welcome, {playerName}!");
            }
            SendMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            SendMessage("");
            SendMessage("📦 CLIENT MOD PACK");
            SendMessage("Get the full experience with our client mods!");
            SendMessage("Check #mod-download on Discord for the latest version.");
            SendMessage("");
            SendMessage("⚙️ FEATURES");
            SendMessage("• Custom Stack Sizes");
            SendMessage("• Structure Durability & Repair");
            SendMessage("• Raid Customization");
            SendMessage("• Zipline Building");
            SendMessage("");
            SendMessage("💾 SAVE MECHANICS");
            SendMessage("• Server auto-saves every 5 minutes (bases, structures)");
            SendMessage("• Your personal progress (inventory, weapons) is saved");
            SendMessage("  when YOU use a bed or tent in-game");
            SendMessage("");
            SendMessage("📜 RULES");
            SendMessage("• Do NOT interfere with other players' bases");
            SendMessage("  without permission from the owner");
            SendMessage("");
            SendMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        /// <summary>
        /// Test if the chat system is working.
        /// </summary>
        public static void TestChat()
        {
            SendMessage("🔧 Project X In-Game Chat Test");
            RLog.Msg("[InGameChat] Test message sent to chat");
        }
    }
}
