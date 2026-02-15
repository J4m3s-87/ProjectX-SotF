using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000026 RID: 38
	public class PlayerTrigger : MonoBehaviour
	{
		// Token: 0x06000168 RID: 360 RVA: 0x000079FC File Offset: 0x00005BFC
		// Note: this type is marked as 'beforefieldinit'.
		static PlayerTrigger()
		{
			Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "PlayerTrigger");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr);
			PlayerTrigger.NativeFieldInfoPtr__message = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr, "_message");
			PlayerTrigger.NativeFieldInfoPtr__target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr, "_target");
			PlayerTrigger.NativeFieldInfoPtr__destroyAfter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr, "_destroyAfter");
			PlayerTrigger.NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr, 100663455);
			PlayerTrigger.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr, 100663456);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007A90 File Offset: 0x00005C90
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499889, XrefRangeEnd = 1499902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnTriggerEnter(Collider other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(other);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PlayerTrigger.NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007AD4 File Offset: 0x00005CD4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe PlayerTrigger()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlayerTrigger>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PlayerTrigger.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000029DA File Offset: 0x00000BDA
		public PlayerTrigger(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007B10 File Offset: 0x00005D10
		// (set) Token: 0x0600016D RID: 365 RVA: 0x000029E3 File Offset: 0x00000BE3
		public unsafe string _message
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__message);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__message), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007B38 File Offset: 0x00005D38
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00002A02 File Offset: 0x00000C02
		public unsafe GameObject _target
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__target);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<GameObject>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__target), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007B68 File Offset: 0x00005D68
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00002A21 File Offset: 0x00000C21
		public unsafe bool _destroyAfter
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__destroyAfter);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PlayerTrigger.NativeFieldInfoPtr__destroyAfter)) = value;
			}
		}

		// Token: 0x040000D7 RID: 215
		private static readonly IntPtr NativeFieldInfoPtr__message;

		// Token: 0x040000D8 RID: 216
		private static readonly IntPtr NativeFieldInfoPtr__target;

		// Token: 0x040000D9 RID: 217
		private static readonly IntPtr NativeFieldInfoPtr__destroyAfter;

		// Token: 0x040000DA RID: 218
		private static readonly IntPtr NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0;

		// Token: 0x040000DB RID: 219
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
