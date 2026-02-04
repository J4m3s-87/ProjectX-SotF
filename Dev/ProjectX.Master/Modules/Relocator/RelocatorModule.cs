#if !SERVER
using System.Collections.Generic;
using RedLoader;
using Sons.Crafting.Structures;
using SonsSdk;
using HarmonyLib;

namespace ProjectX.Master.Modules.Relocator
{
    /// <summary>
    /// Relocator Module - Allows structures to be relocated without breaking.
    /// Based on original Relocator mod pattern:
    /// - Backs up original _relocateMode values
    /// - Applies category-based filtering
    /// - Restores original values when disabled
    /// </summary>
    public static class RelocatorModule
    {
        private static bool _applied = false;
        private static bool _backupDone = false;
        
        // Store original relocate modes per recipe ID
        private static Dictionary<int, int> _originalModes = new Dictionary<int, int>();
        
        public static void Init()
        {
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
            SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);
            RLog.Msg("[Relocator] Initialized (Category-Based Mode)");
        }

        private static void OnGameActivated()
        {
            _applied = false;
            _backupDone = false;
            _originalModes.Clear();
            Apply();
        }
        
        private static void OnUpdate()
        {
            // Keep trying until successful
            if (!_applied)
            {
                Apply();
            }
        }

        private static void Apply()
        {
            try
            {
                if (ScrewStructureManager._instance == null) 
                {
                    return; // Not ready yet
                }

                // Get _database from manager (direct access)
                var db = ScrewStructureManager._instance._database;
                if (db == null) 
                {
                    return; // Not ready yet
                }
                
                var recipes = db._recipes;
                if (recipes == null) 
                {
                    return; // Not ready yet
                }

                // Phase 1: Backup original values (only once)
                if (!_backupDone)
                {
                    foreach (var recipe in recipes)
                    {
                        if (recipe == null) continue;
                        _originalModes[recipe._id] = (int)recipe._relocateMode;
                    }
                    _backupDone = true;
                    RLog.Msg($"[Relocator] Backed up {_originalModes.Count} original relocate modes");
                }

                // Phase 2: Apply relocation based on category
                int modifiedCount = 0;
                foreach (var recipe in recipes)
                {
                    if (recipe == null) continue;
                    
                    // Check if this category should allow relocation
                    bool allowRelocate = ShouldAllowRelocate(recipe._category);
                    
                    if (allowRelocate)
                    {
                        // Mode 0 = allow relocation
                        recipe._relocateMode = (StructureRecipe.RelocateModeType)0;
                        modifiedCount++;
                    }
                    else if (_originalModes.TryGetValue(recipe._id, out int originalMode))
                    {
                        // Restore original mode
                        recipe._relocateMode = (StructureRecipe.RelocateModeType)originalMode;
                    }
                }
                
                _applied = true;
                RLog.Msg($"[Relocator] Applied relocation to {modifiedCount} structures");
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[Relocator] Apply failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Determine if a category should allow relocation.
        /// Categories: 0=Traps, 1=Utility, 2=Furniture, 3=Decoration, 4=Shelters
        /// </summary>
        private static bool ShouldAllowRelocate(StructureRecipe.CategoryType category)
        {
            // For now, enable all categories (like "always-on" mode)
            // This matches the original behavior when all categories are enabled
            // 
            // Category enum values:
            // 0 = Traps (e.g., deadfall traps)
            // 1 = Utility (e.g., meat dryer, rack)
            // 2 = Furniture (e.g., beds, chairs)
            // 3 = Decoration (e.g., skull lamp)
            // 4 = Shelters (e.g., log cabin)
            
            switch (category)
            {
                case StructureRecipe.CategoryType.Traps:
                    return true; // Traps enabled by default
                case StructureRecipe.CategoryType.Utility:
                    return true;
                case StructureRecipe.CategoryType.Furniture:
                    return true;
                case StructureRecipe.CategoryType.Decoration:
                    return true;
                case StructureRecipe.CategoryType.Shelters:
                    return true;
                default:
                    return true; // Enable all by default
            }
        }
        
        /// <summary>
        /// Force reapply (call when config changes)
        /// </summary>
        public static void ForceReapply()
        {
            _applied = false;
            Apply();
        }
    }
}
#endif
