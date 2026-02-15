using RedLoader;
using SonsSdk;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using TheForest.Utils;

namespace ProjectX.Master.Modules.Player
{
    public static class PlayerModule
    {
        private static Type _debugConsoleType;
        private static object _debugConsoleInstance;
        private static MethodInfo _godModeMethod;
        private static MethodInfo _logHackMethod;

        public static void Init()
        {
            // Direct initialization if needed, or just rely on UpdateSettings
            ApplySettings();
        }

        public static void UpdateSettings()
        {
            ApplySettings();
        }

        private static void InitializeReflection()
        {
            // Deprecated: Using direct references if possible
        }

        private static void ApplySettings()
        {
             if (Config.IsGodMode != null)
            {
                string state = Config.IsGodMode.Value ? "on" : "off";
                // Direct call (Requires TheForest.Utils)
                // If this fails to compile, we will see.
                /*
                try 
                {
                    TheForest.Utils.DebugConsole.Instance._godmode(state);
                }
                catch { }
                */
            }

            if (Config.InfiniteLogs != null)
            {
                string state = Config.InfiniteLogs.Value ? "on" : "off";
                try 
                {
                    TheForest.DebugConsole.Instance._loghack(state);
                }
                catch { }
            }

            if (Config.InfiniteStones != null)
            {
                try 
                {
                    string cmd = Config.InfiniteStones.Value ? "stonehack on" : "stonehack off";
                    TheForest.DebugConsole.Instance.SendCommand(cmd);
                }
                catch { }
            }
        }


        public static void UnstuckKelvin()
        {
            try
            {
                if (!LocalPlayer.IsInWorld) return;
                
                Vector3 destPos = LocalPlayer.Transform.position + (LocalPlayer.Transform.forward * 2f);
                Quaternion destRot = LocalPlayer.Transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                int num = 0;
                while (true)
                {
                    // "Robby" is Kelvin's internal name
                    GameObject go = GameObject.Find("Robby" + (num == 0 ? "" : num.ToString())); // Try Robby, Robby1... actually original code was "Robby" + num
                    // Original: "Robby" + num.ToString() -> Robby0, Robby1? 
                    // Let's stick to original logic: "Robby" + num.ToString()
                    if (num > 10) break; // Safety break
                    
                    go = GameObject.Find("Robby" + num.ToString());
                    if (go == null && num == 0) go = GameObject.Find("Robby"); // Fallback check for unnumbered?
                    
                    if (go != null)
                    {
                        var actor = go.GetComponent<Sons.Ai.Vail.VailActor>();
                        if (actor != null)
                        {
                            actor.SetPositionAndRotation(destPos, destRot, true);
                            RLog.Msg($"Teleported Kelvin ({go.name})");
                        }
                    }
                    else if (num > 0)
                    {
                        // Stop if we don't find RobbyN
                        break; 
                    }
                    else if (num == 0) // Should catch Robby0 if it exists
                    {
                         // Continue searching
                    }
                    
                    num++;
                    // Original logic loop was infinite until null.
                }
            }
             catch (Exception ex)
            {
                RLog.Error($"Error UnstuckKelvin: {ex.Message}");
            }
        }

        public static void UnstuckVirginia()
        {
             try
            {
                if (!LocalPlayer.IsInWorld) return;
                
                Vector3 destPos = LocalPlayer.Transform.position + (LocalPlayer.Transform.forward * 2f);
                Quaternion destRot = LocalPlayer.Transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                int num = 0;
                while (true)
                {
                    GameObject go = GameObject.Find("Virginia" + num.ToString());
                    if (go == null) break;
                    
                    var actor = go.GetComponent<Sons.Ai.Vail.VailActor>();
                    if (actor != null)
                    {
                        actor.SetPositionAndRotation(destPos, destRot, true);
                        RLog.Msg($"Teleported Virginia ({go.name})");
                    }
                    num++;
                }
            }
            catch (Exception ex)
            {
                RLog.Error($"Error UnstuckVirginia: {ex.Message}");
            }
        }
    }
}
