using System;
using System.Collections.Generic;
using RedLoader;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// CommandBridge - Routes /px chat commands to handlers
    /// Intercepts chat messages and dispatches to appropriate modules
    /// </summary>
    public static class CommandBridge
    {
        private static Dictionary<string, Action<string, string[]>> _commands = new Dictionary<string, Action<string, string[]>>();
        private static bool _initialized;
        
        /// <summary>
        /// Initialize command system and register handlers
        /// </summary>
        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;
            
            RegisterCommands();
            RLog.Msg("[CommandBridge] Command system initialized");
        }
        
        /// <summary>
        /// Register a command handler from external modules (e.g. DedicatedSuperuser)
        /// </summary>
        public static void RegisterCommand(string command, Action<string, string[]> handler)
        {
            _commands[command.ToLower()] = handler;
        }
        
        /// <summary>
        /// Register all /px commands
        /// </summary>
        private static void RegisterCommands()
        {
#if SERVER || OWNER
            // Session commands (Owner only)
            _commands["session"] = HandleSessionCommand;
            
            // Player permission commands
            _commands["grant"] = HandleGrantCommand;
            _commands["revoke"] = HandleRevokeCommand;
            
            // Effect commands
            _commands["effect"] = HandleEffectCommand;
            
            // Broadcast command
            _commands["broadcast"] = HandleBroadcastCommand;
            
            // Info commands
            _commands["help"] = HandleHelpCommand;
            _commands["players"] = HandlePlayersCommand;
#endif

#if CLIENT
            // Clients can only use help
            _commands["help"] = HandleHelpCommand;
#endif
        }
        
        /// <summary>
        /// Process incoming chat message - returns true if handled
        /// </summary>
        public static bool ProcessMessage(string senderSteamId, string message)
        {
            if (string.IsNullOrEmpty(message)) return false;
            if (!message.StartsWith("/px ") && message != "/px") return false;
            
            try
            {
                // Parse command: /px <command> [args...]
                string[] parts = message.Substring(4).Trim().Split(' ');
                if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
                {
                    parts = new[] { "help" };
                }
                
                string cmd = parts[0].ToLower();
                string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();
                
                if (_commands.TryGetValue(cmd, out var handler))
                {
                    handler(senderSteamId, args);
                    return true;
                }
                else
                {
                    RLog.Msg($"[CommandBridge] Unknown command: {cmd}");
                    return true; // Still consumed the /px message
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[CommandBridge] Error processing command: {ex.Message}");
                return true;
            }
        }
        
#if SERVER || OWNER
        /// <summary>
        /// /px session cheats on|off
        /// </summary>
        private static void HandleSessionCommand(string steamId, string[] args)
        {
            if (!RoleManager.IsOwner(steamId))
            {
                RLog.Msg($"[CommandBridge] {steamId} tried session command without owner role");
                return;
            }
            
            if (args.Length < 2)
            {
                RLog.Msg("[CommandBridge] Usage: /px session cheats on|off");
                return;
            }
            
            if (args[0].ToLower() == "cheats")
            {
                bool enabled = args[1].ToLower() == "on";
                PermissionSync.SetSessionCheats(enabled);
            }
        }
        
        /// <summary>
        /// /px grant menu <player>
        /// </summary>
        private static void HandleGrantCommand(string steamId, string[] args)
        {
            if (!RoleManager.IsAdmin(steamId))
            {
                RLog.Msg($"[CommandBridge] {steamId} tried grant without admin role");
                return;
            }
            
            if (args.Length < 2)
            {
                RLog.Msg("[CommandBridge] Usage: /px grant menu <player>");
                return;
            }
            
            if (args[0].ToLower() == "menu")
            {
                PermissionSync.GrantMenuAccess(args[1]);
            }
        }
        
        /// <summary>
        /// /px revoke menu <player>
        /// </summary>
        private static void HandleRevokeCommand(string steamId, string[] args)
        {
            if (!RoleManager.IsAdmin(steamId))
            {
                RLog.Msg($"[CommandBridge] {steamId} tried revoke without admin role");
                return;
            }
            
            if (args.Length < 2)
            {
                RLog.Msg("[CommandBridge] Usage: /px revoke menu <player>");
                return;
            }
            
            if (args[0].ToLower() == "menu")
            {
                PermissionSync.RevokeMenuAccess(args[1]);
            }
        }
        
        /// <summary>
        /// /px effect <type> <player> on|off
        /// </summary>
        private static void HandleEffectCommand(string steamId, string[] args)
        {
            if (!RoleManager.IsAdmin(steamId))
            {
                RLog.Msg($"[CommandBridge] {steamId} tried effect without admin role");
                return;
            }
            
            if (args.Length < 3)
            {
                RLog.Msg("[CommandBridge] Usage: /px effect godmode|stamina|noclip <player> on|off");
                return;
            }
            
            string effectType = args[0].ToLower();
            string playerName = args[1];
            bool enabled = args[2].ToLower() == "on";
            
            RLog.Msg($"[CommandBridge] Effect {effectType} for {playerName}: {(enabled ? "ON" : "OFF")}");
            // TODO: Send Bolt event to target player
        }
        
        /// <summary>
        /// /px players - list connected players
        /// </summary>
        private static void HandlePlayersCommand(string steamId, string[] args)
        {
            if (!RoleManager.IsAdmin(steamId))
            {
                RLog.Msg($"[CommandBridge] {steamId} tried players without admin role");
                return;
            }
            
            RLog.Msg("[CommandBridge] Player list: (TODO: Bolt player enumeration)");
        }
#endif
        
        /// <summary>
        /// /px broadcast <message> - send a message to all connected players
        /// </summary>
        private static void HandleBroadcastCommand(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                RLog.Msg("[ProjectX] Usage: /px broadcast <message>");
                return;
            }
            
            string message = string.Join(" ", args);
            DedicatedSuperuser.Utility.ChatResponse.Send(message);
            RLog.Msg($"[ProjectX] Broadcast sent: {message}");
        }

        /// <summary>
        /// /px help
        /// </summary>
        private static void HandleHelpCommand(string steamId, string[] args)
        {
            RLog.Msg("[ProjectX] Available commands:");
#if SERVER || OWNER
            RLog.Msg("  /px session cheats on|off  - Enable/disable cheats for all");
            RLog.Msg("  /px grant menu <player>    - Grant menu access");
            RLog.Msg("  /px revoke menu <player>   - Revoke menu access");
            RLog.Msg("  /px effect <type> <player> on|off");
            RLog.Msg("  /px players                - List connected players");
            RLog.Msg("  /px broadcast <message>    - Send message to all players");
#endif
            // Superuser commands (registered dynamically)
            RLog.Msg("  /px raid start|boss|clear|cooldown|requeue|status");
            RLog.Msg("  /px world time|season|freeze|trees|revive|weather");
            RLog.Msg("  /px config get|set|save|list [key] [value]");
            RLog.Msg("  /px loot status|reset|toggle");
            RLog.Msg("  /px server save|status|admin");
            RLog.Msg("  /px save      - Quick save");
            RLog.Msg("  /px status    - Quick status");
            RLog.Msg("  /px time <hr> - Quick time set");
        }
    }
}
