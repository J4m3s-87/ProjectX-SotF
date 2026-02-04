using System;
using RedLoader;
using Il2CppInterop.Runtime.Injection;
using SonsSdk;
using SonsSdk.Attributes;
using UnityEngine;

namespace ProjectX.Master.Modules.UI
{
    /// <summary>
    /// ProjectXUI - Entry point for Project X custom menu
    /// Initializes the IMGUI-based ProjectXGUI MonoBehaviour
    /// </summary>
    public static class ProjectXUI
    {
        private static bool _initialized;
        private static GameObject _guiObject;

        public static void Init()
        {
            if (_initialized) return;

            try
            {
                RLog.Msg("[ProjectXUI] Initializing IMGUI-based menu system...");
                
                // Register our MonoBehaviour type with IL2CPP
                if (!ClassInjector.IsTypeRegisteredInIl2Cpp<ProjectXGUI>())
                {
                    ClassInjector.RegisterTypeInIl2Cpp<ProjectXGUI>();
                    RLog.Msg("[ProjectXUI] Registered ProjectXGUI type");
                }

                // Create persistent GameObject for GUI
                _guiObject = new GameObject("ProjectX_GUI");
                _guiObject.AddComponent<ProjectXGUI>();
                GameObject.DontDestroyOnLoad(_guiObject);

                RLog.Msg("[ProjectXUI] IMGUI Menu System Initialized!");
                RLog.Msg("[ProjectXUI] Press INSERT to toggle menu");
                
                // Register keybind callback for menu toggle
                try
                {
                    ModInputCache.Notify(Config.OpenKey, () => ToggleMenu(), null);
                    RLog.Msg("[ProjectXUI] Keybind registered for INSERT key");
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[ProjectXUI] Could not register keybind: {ex.Message}");
                }
                
                _initialized = true;
            }
            catch (Exception ex)
            {
                RLog.Error($"[ProjectXUI] Failed to initialize: {ex}");
            }
        }

        /// <summary>
        /// Toggles the menu visibility from external code
        /// </summary>
        public static void ToggleMenu()
        {
            if (ProjectXGUI.Instance != null)
            {
                ProjectXGUI.Instance.ToggleMenuPublic();
            }
        }

        /// <summary>
        /// Cleanup on mod unload
        /// </summary>
        public static void Cleanup()
        {
            if (_guiObject != null)
            {
                GameObject.Destroy(_guiObject);
                _guiObject = null;
            }
            _initialized = false;
        }
    }
}
