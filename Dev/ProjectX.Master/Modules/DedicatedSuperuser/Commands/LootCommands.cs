using System;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;
using RedLoader;
using ProjectX.Master.Modules.LootRespawn;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    /// <summary>
    /// Loot respawn control commands — status, reset, toggle.
    /// </summary>
    public static class LootCommands
    {
        public static void Handle(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px loot status|reset|toggle");
                return;
            }

            switch (args[0].ToLower())
            {
                case "status": ShowStatus(); break;
                case "reset":  ResetTracker(); break;
                case "toggle": ToggleRespawn(); break;
                default:
                    Log($"Unknown loot command: {args[0]}");
                    break;
            }
        }

        public static void ShowStatus()
        {
            try
            {
                string status = LootRespawnModule.GetStatus();
                Log(status);
            }
            catch (Exception ex)
            {
                Log($"Status failed: {ex.Message}");
            }
        }

        public static void ResetTracker()
        {
            try
            {
                LootRespawnModule.Reset();
                Log("Loot tracker reset — all items will respawn");
            }
            catch (Exception ex)
            {
                Log($"Reset failed: {ex.Message}");
            }
        }

        public static void ToggleRespawn()
        {
            try
            {
                LootRespawnModule.Enabled = !LootRespawnModule.Enabled;
                Config.Save();
                Log($"Loot Respawn: {(LootRespawnModule.Enabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                Log($"Toggle failed: {ex.Message}");
            }
        }

        private static void Log(string msg)
        {
            // Send to all connected players' chat AND server log
            ChatResponse.Send(msg);
        }
    }
}
