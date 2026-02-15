using System;
using System.Drawing;
using System.Reflection;
using Bolt;
using RedLoader;
using SonsSdk.Networking;
using UdpKit;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// AdminCommandEvent - Routes admin panel commands to the server.
    ///
    /// Uses ChatBox.SendLine() — the exact same method the game calls when a player
    /// types a chat message and hits Enter. This is guaranteed to reach the server
    /// because multiplayer chat works.
    ///
    /// Server-side: DedicatedSuperuserModule hooks CoopPlayerCallbacks.OnEvent(ChatEvent)
    /// to intercept "/px" prefixed messages and route through CommandBridge.
    ///
    /// Approach history:
    ///   ❌ DebugConsole.SendCommand — local only, server overrides changes
    ///   ❌ Packets.NetEvent (custom Bolt) — silently dropped on dedicated servers
    ///   ❌ AdminCommand Bolt event — client→server direction blocked by Bolt security
    ///   ❌ ChatEvent.Create(GlobalTargets.OnlyServer) — server never receives OnEvent
    ///   ✅ ChatBox.SendLine() — game's own chat send method, proven to reach server
    /// </summary>
    public class AdminCommandEvent : Packets.NetEvent
    {
        public static AdminCommandEvent Instance;
        
        // ChatBox.SendLine via reflection
        private static Type _chatBoxType;
        private static MethodInfo _chatBoxSendLine;
        private static bool _chatBoxResolved;
        
        // ChatEvent via reflection (backup/alternate path)
        private static Type _chatEventType;
        private static MethodInfo _chatEventCreate;
        private static PropertyInfo _chatEventMessage;
        private static MethodInfo _chatEventSend;
        private static bool _chatEventResolved;
        
        public override string Id => "ProjectX.AdminCommand";
        
        public static void Register()
        {
            Instance = new AdminCommandEvent();
            Packets.Register(Instance);
            ResolveChatBox();
            ResolveChatEvent();
            RLog.Msg(Color.GreenYellow, "[AdminCommandEvent] Registered");
        }
        
        /// <summary>
        /// Resolve ChatBox type and SendLine method.
        /// ChatBox.SendLine(string line) is the game's native chat send method.
        /// It's private, but we call it via reflection.
        /// </summary>
        private static void ResolveChatBox()
        {
            try
            {
                string[] chatBoxNames = { "ChatBox", "Sons.Gui.ChatBox", "TheForest.UI.Multiplayer.ChatBox" };
                foreach (var name in chatBoxNames)
                {
                    _chatBoxType = HarmonyLib.AccessTools.TypeByName(name);
                    if (_chatBoxType != null) break;
                }
                
                if (_chatBoxType == null)
                {
                    RLog.Warning("[AdminCommandEvent] ChatBox type not found");
                    return;
                }
                
                // SendLine(string line) — private method
                _chatBoxSendLine = _chatBoxType.GetMethod("SendLine",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[] { typeof(string) }, null);
                
                _chatBoxResolved = _chatBoxSendLine != null;
                RLog.Msg($"[AdminCommandEvent] ChatBox resolved: {_chatBoxResolved} (Type={_chatBoxType.Name}, SendLine={_chatBoxSendLine != null})");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] ChatBox resolution error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Resolve ChatEvent (kept for reference/fallback).
        /// </summary>
        private static void ResolveChatEvent()
        {
            try
            {
                _chatEventType = HarmonyLib.AccessTools.TypeByName("ChatEvent");
                if (_chatEventType == null) return;
                
                _chatEventCreate = _chatEventType.GetMethod("Create",
                    BindingFlags.Static | BindingFlags.Public,
                    null, new Type[] { typeof(GlobalTargets) }, null);
                
                _chatEventMessage = _chatEventType.GetProperty("Message");
                
                _chatEventSend = _chatEventType.GetMethod("Send",
                    BindingFlags.Instance | BindingFlags.Public);
                if (_chatEventSend == null && _chatEventType.BaseType != null)
                    _chatEventSend = _chatEventType.BaseType.GetMethod("Send",
                        BindingFlags.Instance | BindingFlags.Public);
                
                _chatEventResolved = _chatEventCreate != null && _chatEventMessage != null && _chatEventSend != null;
                RLog.Msg($"[AdminCommandEvent] ChatEvent resolved: {_chatEventResolved}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] ChatEvent resolution error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Find the ChatBox instance in the scene.
        /// ChatBox is a MonoBehaviour — there should be exactly one in the scene.
        /// </summary>
        private static object FindChatBoxInstance()
        {
            try
            {
                if (_chatBoxType == null) return null;
                
                // Use UnityEngine.Object.FindObjectOfType
                var findMethod = typeof(UnityEngine.Object).GetMethod("FindObjectOfType",
                    BindingFlags.Static | BindingFlags.Public,
                    null, new Type[0], null);
                
                if (findMethod == null)
                {
                    // Try generic version
                    var findGeneric = typeof(UnityEngine.Object).GetMethod("FindObjectOfType",
                        BindingFlags.Static | BindingFlags.Public,
                        null, new Type[] { typeof(Type) }, null);
                    if (findGeneric != null)
                        return findGeneric.Invoke(null, new object[] { _chatBoxType });
                    return null;
                }
                
                var genericFind = findMethod.MakeGenericMethod(_chatBoxType);
                return genericFind.Invoke(null, null);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] FindChatBox error: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Send via ChatBox.SendLine — the same method the game calls when a player chats.
        /// Guaranteed to reach the server because multiplayer chat works.
        /// </summary>
        private static bool SendViaChatBox(string pxCommand)
        {
            try
            {
                if (!_chatBoxResolved || !BoltNetwork.isRunning)
                    return false;
                
                // Check DedicatedSuperuserModule's cached instance first
                var cachedInstance = Modules.DedicatedSuperuser.DedicatedSuperuserModule.CachedChatBoxInstance;
                if (cachedInstance == null)
                {
                    // Try finding the ChatBox in the scene
                    cachedInstance = FindChatBoxInstance();
                    if (cachedInstance != null)
                    {
                        Modules.DedicatedSuperuser.DedicatedSuperuserModule.CachedChatBoxInstance = cachedInstance;
                        RLog.Msg("[AdminCommandEvent] ChatBox instance found in scene");
                    }
                }
                
                if (cachedInstance == null)
                {
                    RLog.Warning("[AdminCommandEvent] No ChatBox instance available");
                    return false;
                }
                
                // Call ChatBox.SendLine(pxCommand)
                _chatBoxSendLine.Invoke(cachedInstance, new object[] { pxCommand });
                RLog.Msg($"[AdminCommandEvent] Sent via ChatBox.SendLine: {pxCommand}");
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] ChatBox.SendLine error: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Fallback: Send via ChatEvent.Create
        /// </summary>
        private static bool SendViaChatEvent(string pxCommand)
        {
            try
            {
                if (!_chatEventResolved || !BoltNetwork.isRunning)
                    return false;
                
                var evt = _chatEventCreate.Invoke(null, new object[] { GlobalTargets.Everyone });
                if (evt == null) return false;
                
                _chatEventMessage.SetValue(evt, pxCommand);
                _chatEventSend.Invoke(evt, null);
                
                RLog.Msg($"[AdminCommandEvent] Sent via ChatEvent.Create: {pxCommand}");
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] ChatEvent send error: {ex.Message}");
                return false;
            }
        }
        
        // Packets.NetEvent stub
        public override void Read(UdpPacket packet, BoltConnection fromConnection) { }
        
        /// <summary>
        /// Send a command to the server.
        /// PRIMARY: ChatBox.SendLine (game's own chat send — uses ChatBox instance in scene)
        /// SECONDARY: ChatEvent.Create(GlobalTargets.Everyone)
        /// FALLBACK: Local execution
        /// 
        /// NOTE: Local visual effects (season PREFIX, weather ToggleForceRain) are handled
        /// by LocalSetSeason/LocalSetWeather BEFORE this method is called.
        /// This method ONLY sends the command to the server — no local processing here.
        /// </summary>
        public static void SendToServer(string pxCommand)
        {
            try
            {
                if (!pxCommand.StartsWith("/px "))
                    pxCommand = "/px " + pxCommand;
                
                // If we ARE the server, execute locally
                if (BoltNetwork.isServer)
                {
                    RLog.Msg($"[AdminCommandEvent] Host mode — local: {pxCommand}");
                    CommandBridge.ProcessMessage("owner", pxCommand);
                    return;
                }
                
                // PRIMARY: ChatBox.SendLine
                if (SendViaChatBox(pxCommand))
                    return;
                
                // SECONDARY: ChatEvent.Create  
                if (SendViaChatEvent(pxCommand))
                    return;
                
                // FALLBACK: Local execution
                RLog.Warning($"[AdminCommandEvent] No delivery path — local: {pxCommand}");
                CommandBridge.ProcessMessage("owner", pxCommand);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AdminCommandEvent] SendToServer error: {ex.Message}");
            }
        }
        
    }
}

