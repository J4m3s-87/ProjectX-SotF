using Sons.Items.Core;
using RedLoader;
using ProjectX.Master;

namespace ProjectX.Master.Modules.Inventory
{
    public static class InventoryModule
    {
        public static void Init()
        {
            // Initial application
            UpdateSettings();
        }

        public static void UpdateSettings()
        {
            // Only apply if the Infinite Inventory setting is enabled
            if (Config.InfiniteInventory.Value)
            {
                ApplyInfiniteItems();
            }
        }

        private static void ApplyInfiniteItems()
        {
            try 
            {
                if (ItemDatabaseManager.Items == null)
                {
                    RLog.Error("InventoryModule: ItemDatabaseManager.Items is NULL!");
                    return;
                }

                int count = 0;
                foreach (var item in ItemDatabaseManager.Items)
                {
                    if (item == null) continue;
                    
                    try 
                    {
                        // Safe modification
                        item.MaxAmount = 999999999; // Delegate to StackModule for per-item logic 
                        count++;
                    }
                    catch (System.Exception innerEx)
                    {
                        RLog.Warning($"Failed to set stack for item {item.Id}: {innerEx.Message}");
                    }
                }
                RLog.Msg($"Inventory Module: Infinite Items Applied to {count} items.");
            }
            catch (System.Exception ex)
            {
                RLog.Error($"CRITICAL CRASH PREVENTED in InventoryModule: {ex.Message}");
                RLog.Error(ex.StackTrace);
            }
        }
    }
}
