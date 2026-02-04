using System;
using SonsSdk.Attributes;
using UnityEngine;

namespace ProjectX.Master.Modules.Hotbar
{
    /// <summary>
    /// Asset definitions for Hotbar - uses SonsSdk attributes for automatic loading.
    /// This matches the original SonsHotbar mod exactly.
    /// </summary>
    [AssetBundle("Assets/hotbar_assets")]
    public static class ModAssets
    {
        [AssetReference("HotbarCanvas")]
        public static GameObject HotbarPrefab { get; set; }

        [AssetReference("missingIcon")]
        public static Sprite missingIcon { get; set; }
    }
}
