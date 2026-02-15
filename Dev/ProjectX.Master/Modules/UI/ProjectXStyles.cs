#if !SERVER
using System;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.UI
{
    /// <summary>
    /// IL2CPP-Safe GUIStyles for Project X Menu
    /// Uses Unity's built-in GUI.skin - no custom texture creation
    /// </summary>
    public static class ProjectXStyles
    {
        // Styles - will use Unity's default skin (no texture creation)
        public static GUIStyle PanelBackground { get; private set; }
        public static GUIStyle SelectorBar { get; private set; }
        public static GUIStyle TitleLabel { get; private set; }
        public static GUIStyle HeaderLabel { get; private set; }
        public static GUIStyle DividerLabel { get; private set; }
        public static GUIStyle NormalLabel { get; private set; }
        public static GUIStyle ValueLabel { get; private set; }
        public static GUIStyle Button { get; private set; }
        public static GUIStyle ArrowButton { get; private set; }
        public static GUIStyle GreenButton { get; private set; }
        public static GUIStyle RedButton { get; private set; }
        public static GUIStyle Toggle { get; private set; }
        public static GUIStyle HorizontalSlider { get; private set; }
        public static GUIStyle HorizontalSliderThumb { get; private set; }
        public static GUIStyle InputField { get; private set; }
        public static GUIStyle ScrollView { get; private set; }
        
        public static bool IsInitialized { get; private set; }
        
        // Colors for runtime tinting - Red/Black Theme
        public static readonly Color PanelColor = new Color(0.02f, 0.02f, 0.02f, 0.97f); // Deep black
        public static readonly Color ButtonColor = new Color(0.12f, 0.08f, 0.08f, 1f); // Dark red-black
        public static readonly Color ButtonHoverColor = new Color(0.3f, 0.1f, 0.1f, 1f); // Red hover
        public static readonly Color ButtonActiveColor = new Color(0.6f, 0.15f, 0.15f, 1f); // Bright red active
        public static readonly Color GreenButtonColorVal = new Color(0.04f, 0.5f, 0.04f, 0.9f);
        public static readonly Color AccentRed = new Color(0.85f, 0.15f, 0.15f, 1f); // Main red accent
        public static readonly Color DimRed = new Color(0.6f, 0.2f, 0.2f, 1f); // Dimmer red for labels
        
        public static void Initialize()
        {
            if (IsInitialized) return;
            
            try
            {
                CreateStyles();
                IsInitialized = true;
                RLog.Msg("[ProjectXStyles] Styles initialized (IL2CPP-safe, no textures)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[ProjectXStyles] Failed to initialize: {ex}");
            }
        }
        
        private static void CreateStyles()
        {
            // Panel background - use box style
            PanelBackground = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(20, 20, 20, 20)
            };
            
            // Selector bar
            SelectorBar = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(12, 12, 10, 10)
            };
            
            // Title - "Project X Mod Menu" (large, red, bold)
            TitleLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 34,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = AccentRed }
            };
            
            // Header - Panel title (large, white, bold)
            HeaderLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 26,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            
            // Divider labels (red accent, like "═══ SECTION ═══")
            DividerLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = AccentRed },
                margin = new RectOffset(0, 0, 16, 10)
            };
            
            // Normal text label
            NormalLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft
            };
            
            // Value display (right-aligned numbers, red tinted)
            ValueLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                normal = { textColor = new Color(1f, 0.7f, 0.7f) },
                alignment = TextAnchor.MiddleRight
            };
            
            // Standard button
            Button = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                fontStyle = FontStyle.Normal,
                normal = { textColor = Color.white },
                hover = { textColor = Color.white },
                active = { textColor = Color.white },
                padding = new RectOffset(16, 16, 14, 14),
                margin = new RectOffset(4, 4, 5, 5)
            };
            
            // Arrow buttons (← →)
            ArrowButton = new GUIStyle(GUI.skin.button)
            {
                fontSize = 32,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                fixedWidth = 70,
                fixedHeight = 55
            };
            
            // Green button (for special actions)
            GreenButton = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                fontStyle = FontStyle.Normal,
                normal = { textColor = Color.white },
                hover = { textColor = Color.white },
                padding = new RectOffset(16, 16, 14, 14),
                margin = new RectOffset(4, 4, 5, 5)
            };
            
            // Red button (accent actions)
            RedButton = new GUIStyle(GUI.skin.button)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                normal = { textColor = AccentRed },
                hover = { textColor = Color.white },
                padding = new RectOffset(16, 16, 12, 12),
                margin = new RectOffset(4, 4, 4, 4)
            };
            
            // Toggle/Checkbox (red when on)
            Toggle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 20,
                normal = { textColor = Color.white },
                hover = { textColor = new Color(1f, 0.85f, 0.85f) },
                onNormal = { textColor = AccentRed },
                padding = new RectOffset(32, 0, 6, 6),
                margin = new RectOffset(8, 8, 6, 6)
            };
            
            // Slider background
            HorizontalSlider = new GUIStyle(GUI.skin.horizontalSlider)
            {
                fixedHeight = 18
            };
            
            // Slider thumb
            HorizontalSliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb)
            {
                fixedWidth = 22,
                fixedHeight = 22
            };
            
            // Text input field
            InputField = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 18,
                normal = { textColor = Color.white },
                focused = { textColor = Color.white },
                padding = new RectOffset(10, 10, 8, 8)
            };
            
            // Scroll view
            ScrollView = new GUIStyle(GUI.skin.scrollView);
        }
        
        /// <summary>
        /// Helper to draw a colored box background (call before drawing content)
        /// Uses GUI.color for tinting
        /// </summary>
        public static void DrawColoredBox(Rect rect, Color color)
        {
            try
            {
                var prevColor = GUI.color;
                GUI.color = color;
                GUI.Box(rect, GUIContent.none);
                GUI.color = prevColor;
            }
            catch { /* IL2CPP safety */ }
        }
    }
}
#endif
