using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000029 RID: 41
	public static class ResourcesHelper : global::Il2CppSystem.Object
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008000 File Offset: 0x00006200
		// Note: this type is marked as 'beforefieldinit'.
		static ResourcesHelper()
		{
			Il2CppClassPointerStore<ResourcesHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "ResourcesHelper");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ResourcesHelper>.NativeClassPtr);
			ResourcesHelper.NativeMethodInfoPtr_UnloadUnusedAssets_Public_Static_AsyncOperation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourcesHelper>.NativeClassPtr, 100663466);
			ResourcesHelper.NativeMethodInfoPtr_GCCollect_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourcesHelper>.NativeClassPtr, 100663467);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008058 File Offset: 0x00006258
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 1499985, RefRangeEnd = 1499990, XrefRangeStart = 1499975, XrefRangeEnd = 1499985, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static AsyncOperation UnloadUnusedAssets()
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourcesHelper.NativeMethodInfoPtr_UnloadUnusedAssets_Public_Static_AsyncOperation_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			IntPtr intPtr3 = intPtr;
			return (intPtr3 != 0) ? Il2CppObjectPool.Get<AsyncOperation>(intPtr3) : null;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000808C File Offset: 0x0000628C
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 1500000, RefRangeEnd = 1500004, XrefRangeStart = 1499990, XrefRangeEnd = 1500000, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void GCCollect()
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourcesHelper.NativeMethodInfoPtr_GCCollect_Public_Static_Void_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00002B0E File Offset: 0x00000D0E
		public ResourcesHelper(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x040000EB RID: 235
		private static readonly IntPtr NativeMethodInfoPtr_UnloadUnusedAssets_Public_Static_AsyncOperation_0;

		// Token: 0x040000EC RID: 236
		private static readonly IntPtr NativeMethodInfoPtr_GCCollect_Public_Static_Void_0;
	}
}
