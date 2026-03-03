using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;
using RedLoader;
using RedLoader.Preferences;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    /// <summary>
    /// Live config commands — get, set, save, reload, list config values.
    /// Allows admins to change any setting in real-time without restart.
    /// </summary>
    public static class ConfigCommands
    {
        public static void Handle(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px config get|set|save|list [key] [value]");
                return;
            }

            switch (args[0].ToLower())
            {
                case "get":
                    if (args.Length >= 2)
                        GetConfigValue(args[1]);
                    else
                        Log("Usage: /px config get <key>");
                    break;

                case "set":
                    if (args.Length >= 3)
                        SetConfigValue(args[1], string.Join(" ", args.Skip(2)));
                    else
                        Log("Usage: /px config set <key> <value>");
                    break;

                case "save":
                    SaveConfig();
                    break;

                case "list":
                    string category = args.Length >= 2 ? string.Join(" ", args.Skip(1)) : null;
                    ListConfig(category);
                    break;

                default:
                    Log($"Unknown config command: {args[0]}");
                    break;
            }
        }

        /// <summary>
        /// Get the current value of a config entry by key name
        /// </summary>
        public static void GetConfigValue(string key)
        {
            try
            {
                var (entry, category) = FindConfigEntry(key);
                if (entry != null)
                {
                    Log($"{category}/{entry.DisplayName}: {entry.BoxedValue}");
                }
                else
                {
                    Log($"Config key '{key}' not found. Use '/px config list' to see available keys.");
                }
            }
            catch (Exception ex)
            {
                Log($"GetConfig failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Set a config value live, with auto-save
        /// </summary>
        public static void SetConfigValue(string key, string value)
        {
            try
            {
                var (entry, category) = FindConfigEntry(key);
                if (entry == null)
                {
                    Log($"Config key '{key}' not found. Use '/px config list' to see available keys.");
                    return;
                }

                // Convert string value to the entry's type
                var targetType = entry.BoxedValue?.GetType() ?? typeof(string);
                object newValue;

                if (targetType == typeof(bool))
                {
                    newValue = value.ToLower() == "true" || value == "1" || value.ToLower() == "on";
                }
                else if (targetType == typeof(float))
                {
                    if (!float.TryParse(value, out float f))
                    {
                        Log($"Invalid float value: {value}");
                        return;
                    }
                    newValue = f;
                }
                else if (targetType == typeof(int))
                {
                    if (!int.TryParse(value, out int i))
                    {
                        Log($"Invalid int value: {value}");
                        return;
                    }
                    newValue = i;
                }
                else if (targetType == typeof(double))
                {
                    if (!double.TryParse(value, out double d))
                    {
                        Log($"Invalid double value: {value}");
                        return;
                    }
                    newValue = d;
                }
                else
                {
                    newValue = value;
                }

                var oldValue = entry.BoxedValue;
                entry.BoxedValue = newValue;
                
                // Auto-save to disk
                Config.Save();
                
                // Server-side log only — no chat response to avoid flooding
                // when the admin panel sends 20+ config set commands at once
                RLog.Msg($"[Superuser] Set {category}/{entry.DisplayName}: {oldValue} -> {newValue} (saved)");
            }
            catch (Exception ex)
            {
                Log($"SetConfig failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Force save all config to disk
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                Config.Save();
                Log("Config saved to disk");
            }
            catch (Exception ex)
            {
                Log($"SaveConfig failed: {ex.Message}");
            }
        }

        /// <summary>
        /// List config categories and entries
        /// </summary>
        public static void ListConfig(string filterCategory = null)
        {
            try
            {
                // Use reflection to find all ConfigCategory properties in Config
                // NOTE: Config uses properties (get/set), not fields!
                var configType = typeof(Config);
                var props = configType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                
                var categoryProps = props.Where(p => typeof(ConfigCategory).IsAssignableFrom(p.PropertyType));
                
                int count = 0;
                foreach (var prop in categoryProps)
                {
                    var cat = prop.GetValue(null) as ConfigCategory;
                    if (cat == null) continue;
                    
                    string catName = cat.DisplayName ?? prop.Name;
                    
                    if (filterCategory != null && 
                        !catName.Contains(filterCategory, StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    Log($"─── {catName} ───");
                    
                    foreach (var entry in cat.Entries)
                    {
                        Log($"  {entry.DisplayName} [{entry.Identifier}]: {entry.BoxedValue}");
                        count++;
                    }
                }
                
                Log($"Total: {count} config entries");
            }
            catch (Exception ex)
            {
                Log($"ListConfig failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Find a config entry by key name (searches all categories)
        /// </summary>
        private static (ConfigEntry, string) FindConfigEntry(string key)
        {
            var configType = typeof(Config);
            // NOTE: Config uses properties (get/set), not fields!
            var props = configType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            
            // First try: exact match on entry property name in Config
            foreach (var prop in props)
            {
                if (prop.Name.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                    typeof(ConfigEntry).IsAssignableFrom(prop.PropertyType))
                {
                    var entry = prop.GetValue(null) as ConfigEntry;
                    if (entry != null)
                        return (entry, "Config");
                }
            }
            
            // Second try: search by Identifier or DisplayName across all categories
            var categoryProps = props.Where(p => typeof(ConfigCategory).IsAssignableFrom(p.PropertyType));
            foreach (var catProp in categoryProps)
            {
                var cat = catProp.GetValue(null) as ConfigCategory;
                if (cat == null) continue;
                
                foreach (var entry in cat.Entries)
                {
                    if (entry.Identifier?.Equals(key, StringComparison.OrdinalIgnoreCase) == true ||
                        entry.DisplayName?.Equals(key, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return (entry, cat.DisplayName ?? catProp.Name);
                    }
                }
            }
            
            return (null, null);
        }

        private static void Log(string msg)
        {
            // Send to all connected players' chat AND server log
            ChatResponse.Send(msg);
        }
    }
}
