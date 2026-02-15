using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000036 RID: 54
	public class OnCollisionStayProxy : MonoBehaviour
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00009814 File Offset: 0x00007A14
		// Note: this type is marked as 'beforefieldinit'.
		static OnCollisionStayProxy()
		{
			Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "OnCollisionStayProxy");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr);
			OnCollisionStayProxy.NativeFieldInfoPtr__clients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, "_clients");
			OnCollisionStayProxy.NativeFieldInfoPtr__block = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, "_block");
			OnCollisionStayProxy.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, 100663521);
			OnCollisionStayProxy.NativeMethodInfoPtr_OnCollisionStay_Private_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, 100663522);
			OnCollisionStayProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, 100663523);
			OnCollisionStayProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, 100663524);
			OnCollisionStayProxy.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr, 100663525);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000098D0 File Offset: 0x00007AD0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500147, XrefRangeEnd = 1500150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionStayProxy.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009904 File Offset: 0x00007B04
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500150, XrefRangeEnd = 1500154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnCollisionStay(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionStayProxy.NativeMethodInfoPtr_OnCollisionStay_Private_Void_Collision_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009948 File Offset: 0x00007B48
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500154, XrefRangeEnd = 1500155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionStayProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000997C File Offset: 0x00007B7C
		[CallerCount(0)]
		public unsafe void SetBlock(bool value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref value;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionStayProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000099BC File Offset: 0x00007BBC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OnCollisionStayProxy()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OnCollisionStayProxy>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionStayProxy.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00002EE5 File Offset: 0x000010E5
		public OnCollisionStayProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000099F8 File Offset: 0x00007BF8
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00002EEE File Offset: 0x000010EE
		public unsafe Il2CppReferenceArray<IOnCollisionStayProxy> _clients
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionStayProxy.NativeFieldInfoPtr__clients);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<IOnCollisionStayProxy>>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionStayProxy.NativeFieldInfoPtr__clients), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00009A28 File Offset: 0x00007C28
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00002F0D File Offset: 0x0000110D
		public unsafe bool _block
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionStayProxy.NativeFieldInfoPtr__block);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionStayProxy.NativeFieldInfoPtr__block)) = value;
			}
		}

		// Token: 0x04000134 RID: 308
		private static readonly IntPtr NativeFieldInfoPtr__clients;

		// Token: 0x04000135 RID: 309
		private static readonly IntPtr NativeFieldInfoPtr__block;

		// Token: 0x04000136 RID: 310
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x04000137 RID: 311
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionStay_Private_Void_Collision_0;

		// Token: 0x04000138 RID: 312
		private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

		// Token: 0x04000139 RID: 313
		private static readonly IntPtr NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0;

		// Token: 0x0400013A RID: 314
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
