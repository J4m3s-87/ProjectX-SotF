using System;
using System.Collections.Generic;
using RedLoader;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// PermissionSync - Manages client permissions synced from server
    /// Server broadcasts permissions on player join, clients receive and apply
    /// </summary>
    public static class PermissionSync
    {
        // Permission flags for this client
        public static bool HasMenuAccess { get; private set; }
        public static bool HasAdminAccess { get; private set; }
        public static string CurrentRole { get; private set; } = "player";
        
        // Server-side session settings
        private static bool _sessionCheatsEnabled;
        private static HashSet<string> _playersWithMenuAccess = new HashSet<string>();
        
        /// <summary>
        /// Initialize permission system
        /// </summary>
        public static void Init()
        {
#if SERVER
            RLog.Msg("[PermissionSync] Server mode - will broadcast permissions to clients");
            _sessionCheatsEnabled = false;
#elif OWNER
            // Owner always has full access
            HasMenuAccess = true;
            HasAdminAccess = true;
            CurrentRole = "owner";
            RLog.Msg("[PermissionSync] Owner mode - full access enabled");
#elif CLIENT
            // Client waits for server permissions
            HasMenuAccess = false;
            HasAdminAccess = false;
            CurrentRole = "player";
            RLog.Msg("[PermissionSync] Client mode - waiting for server permissions");
#else
            // Solo - full access
            HasMenuAccess = true;
            HasAdminAccess = true;
            CurrentRole = "owner";
#endif
        }
        
#if SERVER || OWNER
        /// <summary>
        /// [Server/Owner] Enable cheats for all players this session
        /// </summary>
        public static void SetSessionCheats(bool enabled)
        {
            _sessionCheatsEnabled = enabled;
            RLog.Msg($"[PermissionSync] Session cheats: {(enabled ? "ON" : "OFF")}");
            // TODO: Broadcast via Bolt GlobalEvent
            BroadcastPermissions();
        }
        
        /// <summary>
        /// [Server/Owner] Grant menu access to specific player
        /// </summary>
        public static void GrantMenuAccess(string playerName)
        {
            _playersWithMenuAccess.Add(playerName.ToLower());
            RLog.Msg($"[PermissionSync] Granted menu access to: {playerName}");
            // TODO: Send targeted Bolt event to player
        }
        
        /// <summary>
        /// [Server/Owner] Revoke menu access from specific player
        /// </summary>
        public static void RevokeMenuAccess(string playerName)
        {
            _playersWithMenuAccess.Remove(playerName.ToLower());
            RLog.Msg($"[PermissionSync] Revoked menu access from: {playerName}");
            // TODO: Send targeted Bolt event to player
        }
        
        /// <summary>
        /// [Server/Owner] Check if player has menu access
        /// </summary>
        public static bool PlayerHasMenuAccess(string playerName)
        {
            return _sessionCheatsEnabled || _playersWithMenuAccess.Contains(playerName.ToLower());
        }
        
        /// <summary>
        /// [Server] Broadcast current permissions to all clients
        /// </summary>
        private static void BroadcastPermissions()
        {
            if (PermissionEvent.Instance != null)
            {
                PermissionEvent.Instance.BroadcastSessionCheats(_sessionCheatsEnabled);
            }
        }
#endif

#if CLIENT
        /// <summary>
        /// [Client] Called when receiving permission update from server
        /// </summary>
        public static void OnReceivePermissions(bool menuAccess, bool adminAccess, string role)
        {
            HasMenuAccess = menuAccess;
            HasAdminAccess = adminAccess;
            CurrentRole = role;
            RLog.Msg($"[PermissionSync] Received permissions - Menu: {menuAccess}, Admin: {adminAccess}, Role: {role}");
        }
#endif
    }
}
