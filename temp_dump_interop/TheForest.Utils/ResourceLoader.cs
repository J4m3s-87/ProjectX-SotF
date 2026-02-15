using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000028 RID: 40
	public class ResourceLoader : MonoBehaviour
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00007D18 File Offset: 0x00005F18
		// Note: this type is marked as 'beforefieldinit'.
		static ResourceLoader()
		{
			Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "ResourceLoader");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr);
			ResourceLoader.NativeFieldInfoPtr__type = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, "_type");
			ResourceLoader.NativeFieldInfoPtr__assetPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, "_assetPath");
			ResourceLoader.NativeFieldInfoPtr__target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, "_target");
			ResourceLoader.NativeFieldInfoPtr__asset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, "_asset");
			ResourceLoader.NativeFieldInfoPtr_InUseAssetsCounters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, "InUseAssetsCounters");
			ResourceLoader.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, 100663460);
			ResourceLoader.NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, 100663461);
			ResourceLoader.NativeMethodInfoPtr_AssetLoad_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, 100663462);
			ResourceLoader.NativeMethodInfoPtr_AssetUnload_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, 100663463);
			ResourceLoader.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr, 100663464);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007E10 File Offset: 0x00006010
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499918, XrefRangeEnd = 1499940, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnEnable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourceLoader.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007E44 File Offset: 0x00006044
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499940, XrefRangeEnd = 1499958, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDisable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourceLoader.NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007E78 File Offset: 0x00006078
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1499967, RefRangeEnd = 1499968, XrefRangeStart = 1499958, XrefRangeEnd = 1499967, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void AssetLoad()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourceLoader.NativeMethodInfoPtr_AssetLoad_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007EAC File Offset: 0x000060AC
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1499974, RefRangeEnd = 1499975, XrefRangeStart = 1499968, XrefRangeEnd = 1499974, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void AssetUnload(bool resourceUnload)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref resourceUnload;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourceLoader.NativeMethodInfoPtr_AssetUnload_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007EEC File Offset: 0x000060EC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ResourceLoader()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ResourceLoader>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ResourceLoader.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00002A7B File Offset: 0x00000C7B
		public ResourceLoader(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007F28 File Offset: 0x00006128
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00002A84 File Offset: 0x00000C84
		public unsafe ResourceLoader.AssetTypes _type
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__type);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__type)) = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007F50 File Offset: 0x00006150
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00002A9F File Offset: 0x00000C9F
		public unsafe string _assetPath
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__assetPath);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__assetPath), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007F78 File Offset: 0x00006178
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00002ABE File Offset: 0x00000CBE
		public unsafe Object _target
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__target);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Object>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__target), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007FA8 File Offset: 0x000061A8
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00002ADD File Offset: 0x00000CDD
		public unsafe Object _asset
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__asset);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Object>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(ResourceLoader.NativeFieldInfoPtr__asset), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007FD8 File Offset: 0x000061D8
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00002AFC File Offset: 0x00000CFC
		public unsafe static Dictionary<string, int> InUseAssetsCounters
		{
			get
			{
				IntPtr intPtr;
				IL2CPP.il2cpp_field_static_get_value(ResourceLoader.NativeFieldInfoPtr_InUseAssetsCounters, (void*)(&intPtr));
				IntPtr intPtr2 = intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Dictionary<string, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(ResourceLoader.NativeFieldInfoPtr_InUseAssetsCounters, IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000E1 RID: 225
		private static readonly IntPtr NativeFieldInfoPtr__type;

		// Token: 0x040000E2 RID: 226
		private static readonly IntPtr NativeFieldInfoPtr__assetPath;

		// Token: 0x040000E3 RID: 227
		private static readonly IntPtr NativeFieldInfoPtr__target;

		// Token: 0x040000E4 RID: 228
		private static readonly IntPtr NativeFieldInfoPtr__asset;

		// Token: 0x040000E5 RID: 229
		private static readonly IntPtr NativeFieldInfoPtr_InUseAssetsCounters;

		// Token: 0x040000E6 RID: 230
		private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

		// Token: 0x040000E7 RID: 231
		private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

		// Token: 0x040000E8 RID: 232
		private static readonly IntPtr NativeMethodInfoPtr_AssetLoad_Public_Void_0;

		// Token: 0x040000E9 RID: 233
		private static readonly IntPtr NativeMethodInfoPtr_AssetUnload_Public_Void_Boolean_0;

		// Token: 0x040000EA RID: 234
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x02000045 RID: 69
		public enum AssetTypes
		{
			// Token: 0x04000178 RID: 376
			Mesh,
			// Token: 0x04000179 RID: 377
			Texture
		}
	}
}
