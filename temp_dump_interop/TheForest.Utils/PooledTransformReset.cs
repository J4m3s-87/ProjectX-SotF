using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000027 RID: 39
	public class PooledTransformReset : MonoBehaviour
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00007B90 File Offset: 0x00005D90
		// Note: this type is marked as 'beforefieldinit'.
		static PooledTransformReset()
		{
			Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "PooledTransformReset");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr);
			PooledTransformReset.NativeFieldInfoPtr__localPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr, "_localPosition");
			PooledTransformReset.NativeFieldInfoPtr__localRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr, "_localRotation");
			PooledTransformReset.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr, 100663457);
			PooledTransformReset.NativeMethodInfoPtr_OnSpawned_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr, 100663458);
			PooledTransformReset.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr, 100663459);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007C24 File Offset: 0x00005E24
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499902, XrefRangeEnd = 1499910, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PooledTransformReset.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007C58 File Offset: 0x00005E58
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499910, XrefRangeEnd = 1499918, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnSpawned()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PooledTransformReset.NativeMethodInfoPtr_OnSpawned_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007C8C File Offset: 0x00005E8C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe PooledTransformReset()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PooledTransformReset>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PooledTransformReset.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00002A3C File Offset: 0x00000C3C
		public PooledTransformReset(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007CC8 File Offset: 0x00005EC8
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00002A45 File Offset: 0x00000C45
		public unsafe Vector3 _localPosition
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PooledTransformReset.NativeFieldInfoPtr__localPosition);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PooledTransformReset.NativeFieldInfoPtr__localPosition)) = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007CF0 File Offset: 0x00005EF0
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00002A60 File Offset: 0x00000C60
		public unsafe Quaternion _localRotation
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PooledTransformReset.NativeFieldInfoPtr__localRotation);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PooledTransformReset.NativeFieldInfoPtr__localRotation)) = value;
			}
		}

		// Token: 0x040000DC RID: 220
		private static readonly IntPtr NativeFieldInfoPtr__localPosition;

		// Token: 0x040000DD RID: 221
		private static readonly IntPtr NativeFieldInfoPtr__localRotation;

		// Token: 0x040000DE RID: 222
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x040000DF RID: 223
		private static readonly IntPtr NativeMethodInfoPtr_OnSpawned_Private_Void_0;

		// Token: 0x040000E0 RID: 224
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
