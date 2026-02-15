using System;
using System.Reflection;
using ProjectX.Master.Modules.DedicatedSuperuser.Commands;
using ProjectX.Master.Modules.DedicatedSuperuser.Networking;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;
using ProjectX.Master.Modules.Network;
using RedLoader;
using HarmonyLib;
using SonsSdk;

namespace ProjectX.Master.Modules.DedicatedSuperuser
{
    /// <summary>
    /// DedicatedSuperuser module — intercepts chat messages on the server
    /// and routes /px commands to CommandBridge for processing.
    /// Also provides admin whitelist authentication.
    /// </summary>
    public class DedicatedSuperuserModule
    {
        public static DedicatedSuperuserModule Instance;
        private static HarmonyLib.Harmony _harmony;
        
        /// <summary>
        /// Cached ChatBox instance from Harmony hook — used by AdminCommandEvent for SendLine
        /// </summary>
        public static object CachedChatBoxInstance;
        
        /// <summary>
        /// Deduplication: prevent dual ChatBox/ChatEvent from executing the same command twice
        /// </summary>
        private static string _lastProcessedCommand;
        private static float _lastProcessedTime;

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new DedicatedSuperuserModule();
                Instance.OnInitializeMod();
            }
        }

        private void OnInitializeMod()
        {
            RLog.Msg("[DedicatedSuperuser] Module Initializing...");

            // Initialize admin whitelist
            OwnerWhitelistUtils.Init();
            
            // Initialize server-to-client chat responses
            Utility.ChatResponse.Init();

            // Register command handlers in CommandBridge
            RegisterSuperuserCommands();

            // Attempt to hook chat for server-side command interception
            _harmony = new HarmonyLib.Harmony("ProjectX.DedicatedSuperuser");
            PatchChat(_harmony);

            RLog.Msg("[DedicatedSuperuser] Module Initialized");
        }

        /// <summary>
        /// Register all superuser command handlers into CommandBridge
        /// </summary>
        private void RegisterSuperuserCommands()
        {
            CommandBridge.RegisterCommand("raid", RaidCommands.Handle);
            CommandBridge.RegisterCommand("world", WorldCommands.Handle);
            CommandBridge.RegisterCommand("config", ConfigCommands.Handle);
            CommandBridge.RegisterCommand("loot", LootCommands.Handle);
            CommandBridge.RegisterCommand("server", ServerCommands.Handle);

            // Shortcut aliases for common actions
            CommandBridge.RegisterCommand("save", (sid, args) => ServerCommands.ForceSave());
            CommandBridge.RegisterCommand("status", (sid, args) => ServerCommands.ShowStatus());
            CommandBridge.RegisterCommand("time", (sid, args) =>
            {
                if (args.Length >= 1 && float.TryParse(args[0], out float hour))
                    WorldCommands.SetTimeOfDay(hour);
                else
                    RLog.Msg("[Superuser] Usage: /px time <0-24>");
            });

            RLog.Msg("[DedicatedSuperuser] Registered superuser commands: raid, world, config, loot, server + aliases");
        }

        /// <summary>
        /// Patch event handlers for server-side command interception.
        /// 
        /// PRIMARY: CoopPlayerCallbacks.OnEvent(ChatEvent) — [BoltGlobalBehaviour] singleton
        ///          that handles chat events on ALL peers including dedicated servers.
        ///          ChatEvent is the game's native bidirectional chat event.
        ///
        /// SECONDARY: CoopServerCallbacks.OnEvent(AdminCommand) — native admin channel
        /// TERTIARY: ChatBox.AddLine — for client-hosted servers with GUI
        /// </summary>
        private void PatchChat(HarmonyLib.Harmony harmony)
        {
            bool hooked = false;
            
            // PRIMARY: Hook CoopPlayerCallbacks.OnEvent(ChatEvent)
            // This is a [BoltGlobalBehaviour] that exists on ALL peers including headless servers.
            // ChatEvent is the game's bidirectional chat system.
            try
            {
                var coopPlayerType = HarmonyLib.AccessTools.TypeByName("CoopPlayerCallbacks");
                var chatEventType = HarmonyLib.AccessTools.TypeByName("ChatEvent");
                
                if (coopPlayerType != null && chatEventType != null)
                {
                    var methods = coopPlayerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                    MethodInfo onEventMethod = null;
                    
                    foreach (var m in methods)
                    {
                        if (m.Name != "OnEvent") continue;
                        var parms = m.GetParameters();
                        if (parms.Length == 1 && parms[0].ParameterType == chatEventType)
                        {
                            onEventMethod = m;
                            break;
                        }
                    }
                    
                    if (onEventMethod != null)
                    {
                        var postfix = typeof(DedicatedSuperuserModule).GetMethod(
                            nameof(OnChatEventReceived),
                            BindingFlags.Static | BindingFlags.Public);
                        harmony.Patch(onEventMethod, postfix: new HarmonyLib.HarmonyMethod(postfix));
                        RLog.Msg("[DedicatedSuperuser] Hooked CoopPlayerCallbacks.OnEvent(ChatEvent) ★");
                        hooked = true;
                    }
                    else
                    {
                        RLog.Warning("[DedicatedSuperuser] CoopPlayerCallbacks.OnEvent(ChatEvent) not found");
                    }
                }
                else
                {
                    RLog.Warning($"[DedicatedSuperuser] Type resolution: CoopPlayerCallbacks={coopPlayerType != null}, ChatEvent={chatEventType != null}");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] CoopPlayerCallbacks hook failed: {ex.Message}");
            }
            
            // SECONDARY: Also hook CoopServerCallbacks.OnEvent(AdminCommand) in case AdminCommand events work
            try
            {
                var coopServerType = HarmonyLib.AccessTools.TypeByName("CoopServerCallbacks");
                var adminCmdType = HarmonyLib.AccessTools.TypeByName("AdminCommand");
                
                if (coopServerType != null && adminCmdType != null)
                {
                    var methods = coopServerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var m in methods)
                    {
                        if (m.Name != "OnEvent") continue;
                        var parms = m.GetParameters();
                        if (parms.Length == 1 && parms[0].ParameterType == adminCmdType)
                        {
                            var postfix = typeof(DedicatedSuperuserModule).GetMethod(
                                nameof(OnAdminCommandEvent), BindingFlags.Static | BindingFlags.Public);
                            harmony.Patch(m, postfix: new HarmonyLib.HarmonyMethod(postfix));
                            RLog.Msg("[DedicatedSuperuser] Hooked CoopServerCallbacks.OnEvent(AdminCommand)");
                            hooked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] AdminCommand hook failed: {ex.Message}");
            }
            
            // TERTIARY: ChatBox.AddLine for client-hosted servers
            try
            {
                string[] chatTypeNames = { "Sons.Gui.ChatBox", "TheForest.UI.Multiplayer.ChatBox", "ChatBox" };
                Type chatType = null;
                foreach (var name in chatTypeNames)
                {
                    chatType = HarmonyLib.AccessTools.TypeByName(name);
                    if (chatType != null) break;
                }
                
                if (chatType != null)
                {
                    var methods = chatType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var method in methods)
                    {
                        if (method.Name != "AddLine") continue;
                        var parms = method.GetParameters();
                        if (parms.Length >= 2)
                        {
                            bool hasString = false;
                            foreach (var p in parms)
                                if (p.ParameterType == typeof(string)) { hasString = true; break; }
                            if (hasString)
                            {
                                var postfix = typeof(DedicatedSuperuserModule).GetMethod(
                                    nameof(OnChatMessageReceived), BindingFlags.Static | BindingFlags.Public);
                                harmony.Patch(method, postfix: new HarmonyLib.HarmonyMethod(postfix));
                                RLog.Msg($"[DedicatedSuperuser] Hooked chat: {chatType.Name}.{method.Name}");
                                hooked = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] ChatBox hook failed: {ex.Message}");
            }
            
            if (!hooked)
            {
                RLog.Warning("[DedicatedSuperuser] No hooks installed — commands only via menu buttons");
            }
        }

        /// <summary>
        /// Harmony postfix — intercepts chat messages and routes /px commands
        /// </summary>
        public static void OnChatMessageReceived(object __instance, string message)
        {
            // Cache the ChatBox instance — used by AdminCommandEvent for SendLine
            if (__instance != null && CachedChatBoxInstance == null)
            {
                CachedChatBoxInstance = __instance;
                RLog.Msg("[DedicatedSuperuser] ChatBox instance cached for admin commands");
            }
            
            // Also catches overloads with extra params — Harmony ignores unused params
            if (string.IsNullOrEmpty(message)) return;
            if (!message.StartsWith("/px")) return;
            
            // Dedup guard: skip if same command was processed within 1 second
            float now = UnityEngine.Time.unscaledTime;
            if (message == _lastProcessedCommand && (now - _lastProcessedTime) < 1.0f)
            {
                RLog.Msg($"[DedicatedSuperuser] Dedup: skipping duplicate ChatBox command: {message}");
                return;
            }
            _lastProcessedCommand = message;
            _lastProcessedTime = now;

            try
            {
                string steamId = "server";

                if (UnityEngine.Application.isBatchMode)
                {
                    CommandBridge.ProcessMessage(steamId, message);
                }
                else
                {
                    if (OwnerWhitelistUtils.IsAdmin(steamId))
                    {
                        CommandBridge.ProcessMessage(steamId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] Error handling chat command: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Harmony postfix — intercepts ChatEvent via CoopPlayerCallbacks.
        /// ChatEvent has properties: Message (string) and Sender (NetworkId).
        /// When a client sends a ChatEvent with "/px" prefix, route through CommandBridge.
        /// This runs on the dedicated server.
        /// </summary>
        public static void OnChatEventReceived(object __instance, object evnt)
        {
            if (evnt == null) return;
            
            // CRITICAL: Only process commands on the SERVER.
            if (!BoltNetwork.isServer) return;
            
            try
            {
                var msgProp = evnt.GetType().GetProperty("Message");
                if (msgProp == null) return;
                
                string message = msgProp.GetValue(evnt) as string;
                if (string.IsNullOrEmpty(message)) return;
                
                // Anti-cheat integrity response — route to IntegrityEvent directly
                // (not through CommandBridge — this is automated, not a user command)
#if SERVER || OWNER
                if (message.StartsWith("/px integrity "))
                {
                    RLog.Msg($"[DedicatedSuperuser] Integrity response via ChatEvent — routing to IntegrityEvent");
                    IntegrityEvent.HandleChatResponse(message, evnt);
                    return;
                }
#endif
                
                RLog.Msg($"[DedicatedSuperuser] ChatEvent received: {message}");
                
                if (!message.StartsWith("/px")) return;
                
                // Dedup guard: skip if same command was processed within 1 second
                float now = UnityEngine.Time.unscaledTime;
                if (message == _lastProcessedCommand && (now - _lastProcessedTime) < 1.0f)
                {
                    RLog.Msg($"[DedicatedSuperuser] Dedup: skipping duplicate ChatEvent command: {message}");
                    return;
                }
                _lastProcessedCommand = message;
                _lastProcessedTime = now;
                
                // Execute on server via CommandBridge
                RLog.Msg($"[DedicatedSuperuser] Processing /px command from ChatEvent: {message}");
                CommandBridge.ProcessMessage("admin", message);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] ChatEvent handler error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Harmony postfix — intercepts the game's native AdminCommand Bolt events.
        /// CoopServerCallbacks.OnEvent(AdminCommand evnt) fires when a client sends 
        /// an AdminCommand event to the server.
        /// 
        /// AdminCommand has properties: Command (string), Data (string)
        /// We check if Command starts with "/px" and route through CommandBridge.
        /// </summary>
        public static void OnAdminCommandEvent(object __instance, object evnt)
        {
            if (evnt == null) return;
            
            try
            {
                // Get the Command property from the AdminCommand event
                var commandProp = evnt.GetType().GetProperty("Command");
                if (commandProp == null) return;
                
                string command = commandProp.GetValue(evnt) as string;
                if (string.IsNullOrEmpty(command)) return;
                
                RLog.Msg($"[DedicatedSuperuser] AdminCommand received: {command}");
                
                // Only intercept /px commands, let the game handle its own admin commands
                if (!command.StartsWith("/px")) return;
                
                // Route through CommandBridge
                string steamId = "admin"; // From AdminCommand event, identity is implicit
                CommandBridge.ProcessMessage(steamId, command);
                RLog.Msg($"[DedicatedSuperuser] Processed: {command}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DedicatedSuperuser] AdminCommand handler error: {ex.Message}");
            }
        }
    }
}
