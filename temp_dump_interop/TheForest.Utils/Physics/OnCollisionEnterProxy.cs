using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000032 RID: 50
	public class OnCollisionEnterProxy : MonoBehaviour
	{
		// Token: 0x060001EB RID: 491 RVA: 0x000092A0 File Offset: 0x000074A0
		// Note: this type is marked as 'beforefieldinit'.
		static OnCollisionEnterProxy()
		{
			Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "OnCollisionEnterProxy");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr);
			OnCollisionEnterProxy.NativeFieldInfoPtr__clients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, "_clients");
			OnCollisionEnterProxy.NativeFieldInfoPtr__block = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, "_block");
			OnCollisionEnterProxy.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, 100663508);
			OnCollisionEnterProxy.NativeMethodInfoPtr_OnCollisionEnter_Private_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, 100663509);
			OnCollisionEnterProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, 100663510);
			OnCollisionEnterProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, 100663511);
			OnCollisionEnterProxy.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr, 100663512);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000935C File Offset: 0x0000755C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500125, XrefRangeEnd = 1500128, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionEnterProxy.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009390 File Offset: 0x00007590
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500128, XrefRangeEnd = 1500132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnCollisionEnter(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionEnterProxy.NativeMethodInfoPtr_OnCollisionEnter_Private_Void_Collision_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000093D4 File Offset: 0x000075D4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500132, XrefRangeEnd = 1500133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionEnterProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009408 File Offset: 0x00007608
		[CallerCount(0)]
		public unsafe void SetBlock(bool value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref value;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionEnterProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009448 File Offset: 0x00007648
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OnCollisionEnterProxy()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OnCollisionEnterProxy>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionEnterProxy.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00002DEF File Offset: 0x00000FEF
		public OnCollisionEnterProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00009484 File Offset: 0x00007684
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public unsafe Il2CppReferenceArray<IOnCollisionEnterProxy> _clients
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionEnterProxy.NativeFieldInfoPtr__clients);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<IOnCollisionEnterProxy>>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionEnterProxy.NativeFieldInfoPtr__clients), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000094B4 File Offset: 0x000076B4
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00002E17 File Offset: 0x00001017
		public unsafe bool _block
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionEnterProxy.NativeFieldInfoPtr__block);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionEnterProxy.NativeFieldInfoPtr__block)) = value;
			}
		}

		// Token: 0x04000123 RID: 291
		private static readonly IntPtr NativeFieldInfoPtr__clients;

		// Token: 0x04000124 RID: 292
		private static readonly IntPtr NativeFieldInfoPtr__block;

		// Token: 0x04000125 RID: 293
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x04000126 RID: 294
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionEnter_Private_Void_Collision_0;

		// Token: 0x04000127 RID: 295
		private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

		// Token: 0x04000128 RID: 296
		private static readonly IntPtr NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0;

		// Token: 0x04000129 RID: 297
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
