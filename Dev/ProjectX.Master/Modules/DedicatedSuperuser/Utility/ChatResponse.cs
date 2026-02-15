using System;
using System.Reflection;
using Bolt;
using RedLoader;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Utility
{
    /// <summary>
    /// Sends chat responses from the server back to all connected clients.
    /// 
    /// Two sending paths:
    /// - Send() uses ChatEvent.Create(GlobalTargets.Everyone) — server-only visibility
    /// - SendLine() uses ChatBox.SendLine() — the game's native chat, reaches ALL players
    /// </summary>
    public static class ChatResponse
    {
        // ChatEvent path (existing)
        private static Type _chatEventType;
        private static MethodInfo _chatEventCreate;
        private static PropertyInfo _chatEventMessage;
        private static MethodInfo _chatEventSend;
        private static bool _resolved;
        
        // ChatBox.SendLine path (new — actually reaches client chat UIs)
        private static Type _chatBoxType;
        private static MethodInfo _chatBoxSendLine;
        private static bool _chatBoxResolved;
        
        public static void Init()
        {
            try
            {
                _chatEventType = HarmonyLib.AccessTools.TypeByName("ChatEvent");
                if (_chatEventType == null)
                {
                    RLog.Warning("[ChatResponse] ChatEvent type not found");
                    return;
                }
                
                // ChatEvent.Create(GlobalTargets) — static factory
                _chatEventCreate = _chatEventType.GetMethod("Create",
                    BindingFlags.Static | BindingFlags.Public,
                    null, new Type[] { typeof(GlobalTargets) }, null);
                
                // ChatEvent.Message — string property
                _chatEventMessage = _chatEventType.GetProperty("Message");
                
                // ChatEvent.Send() — instance method (inherited from Event)
                _chatEventSend = _chatEventType.GetMethod("Send",
                    BindingFlags.Instance | BindingFlags.Public,
                    null, Type.EmptyTypes, null);
                    
                if (_chatEventSend == null && _chatEventType.BaseType != null)
                    _chatEventSend = _chatEventType.BaseType.GetMethod("Send",
                        BindingFlags.Instance | BindingFlags.Public,
                        null, Type.EmptyTypes, null);
                
                _resolved = _chatEventCreate != null && _chatEventMessage != null && _chatEventSend != null;
                RLog.Msg($"[ChatResponse] Initialized: {_resolved} (Create={_chatEventCreate != null}, Message={_chatEventMessage != null}, Send={_chatEventSend != null})");
                
                // Resolve ChatBox.SendLine — the game's native chat sender
                ResolveChatBoxSendLine();
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ChatResponse] Init failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Resolve ChatBox type and SendLine method for the native chat pipeline.
        /// ChatBox.SendLine(string) goes through the same path as player chat → reaches everyone.
        /// </summary>
        private static void ResolveChatBoxSendLine()
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
                    RLog.Warning("[ChatResponse] ChatBox type not found for SendLine");
                    return;
                }
                
                // SendLine(string line) — private method
                _chatBoxSendLine = _chatBoxType.GetMethod("SendLine",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[] { typeof(string) }, null);
                
                _chatBoxResolved = _chatBoxSendLine != null;
                RLog.Msg($"[ChatResponse] ChatBox.SendLine resolved: {_chatBoxResolved}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ChatResponse] ChatBox.SendLine resolution error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Send a [ProjectX] prefixed message to all connected players' chat
        /// using the GAME'S NATIVE chat pipeline (ChatBox.SendLine).
        /// This is the method that actually appears in everyone's chat UI.
        /// Falls back to ChatEvent.Create if ChatBox is unavailable.
        /// </summary>
        public static void SendLine(string message)
        {
            string formatted = $"[ProjectX] {message}";
            
            // Always log server-side
            RLog.Msg($"[Superuser] {message}");
            
            if (!BoltNetwork.isRunning) return;
            
            // PRIMARY: Try ChatBox.SendLine (reaches all connected players)
            if (_chatBoxResolved)
            {
                try
                {
                    var chatBoxInstance = DedicatedSuperuserModule.CachedChatBoxInstance;
                    
                    // If no cached instance, try to find it in the scene
                    if (chatBoxInstance == null)
                    {
                        chatBoxInstance = FindChatBoxInstance();
                        if (chatBoxInstance != null)
                        {
                            DedicatedSuperuserModule.CachedChatBoxInstance = chatBoxInstance;
                            RLog.Msg("[ChatResponse] ChatBox instance found via FindObjectOfType");
                        }
                    }
                    
                    if (chatBoxInstance != null)
                    {
                        _chatBoxSendLine.Invoke(chatBoxInstance, new object[] { formatted });
                        RLog.Msg($"[ChatResponse] Sent via ChatBox.SendLine: {formatted}");
                        return;
                    }
                    else
                    {
                        RLog.Warning("[ChatResponse] No ChatBox instance — falling back to ChatEvent");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[ChatResponse] ChatBox.SendLine error: {ex.Message}");
                }
            }
            
            // FALLBACK: Use ChatEvent.Create (may not render on clients)
            Send(message);
        }
        
        /// <summary>
        /// Send a [ProjectX] prefixed message via ChatEvent.Create(GlobalTargets.Everyone).
        /// NOTE: This may not render in vanilla client chat UIs.
        /// Prefer SendLine() for messages that need to be visible to players.
        /// Also logs to server console via RLog.
        /// </summary>
        public static void Send(string message)
        {
            string formatted = $"[ProjectX] {message}";
            
            // Always log server-side
            RLog.Msg($"[Superuser] {message}");
            
            // Send to all clients via ChatEvent
            if (!_resolved || !BoltNetwork.isRunning)
                return;
                
            try
            {
                var evt = _chatEventCreate.Invoke(null, new object[] { GlobalTargets.Everyone });
                if (evt == null)
                {
                    RLog.Warning("[ChatResponse] ChatEvent.Create returned null");
                    return;
                }
                
                _chatEventMessage.SetValue(evt, formatted);
                _chatEventSend.Invoke(evt, null);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ChatResponse] Send failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Find the ChatBox instance in the scene using UnityEngine.Object.FindObjectOfType.
        /// ChatBox is a MonoBehaviour — there should be exactly one in the scene.
        /// </summary>
        private static object FindChatBoxInstance()
        {
            try
            {
                if (_chatBoxType == null) return null;
                
                // Use UnityEngine.Object.FindObjectOfType (generic)
                var findMethod = typeof(UnityEngine.Object).GetMethod("FindObjectOfType",
                    BindingFlags.Static | BindingFlags.Public,
                    null, new Type[0], null);
                
                if (findMethod != null)
                {
                    var genericFind = findMethod.MakeGenericMethod(_chatBoxType);
                    return genericFind.Invoke(null, null);
                }
                
                // Try the non-generic version with Type parameter
                var findByType = typeof(UnityEngine.Object).GetMethod("FindObjectOfType",
                    BindingFlags.Static | BindingFlags.Public,
                    null, new Type[] { typeof(Type) }, null);
                if (findByType != null)
                    return findByType.Invoke(null, new object[] { _chatBoxType });
                
                return null;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ChatResponse] FindChatBoxInstance error: {ex.Message}");
                return null;
            }
        }
    }
}
