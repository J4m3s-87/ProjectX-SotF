using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000034 RID: 52
	public class OnCollisionExitProxy : MonoBehaviour
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x0000952C File Offset: 0x0000772C
		// Note: this type is marked as 'beforefieldinit'.
		static OnCollisionExitProxy()
		{
			Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "OnCollisionExitProxy");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr);
			OnCollisionExitProxy.NativeFieldInfoPtr__clients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, "_clients");
			OnCollisionExitProxy.NativeFieldInfoPtr__block = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, "_block");
			OnCollisionExitProxy.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663514);
			OnCollisionExitProxy.NativeMethodInfoPtr_OnCollisionExit_Private_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663515);
			OnCollisionExitProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663516);
			OnCollisionExitProxy.NativeMethodInfoPtr_ConvertToArray_Public_Il2CppArrayBase_1_T_IList_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663517);
			OnCollisionExitProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663518);
			OnCollisionExitProxy.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr, 100663519);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000095FC File Offset: 0x000077FC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500133, XrefRangeEnd = 1500136, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00009630 File Offset: 0x00007830
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500136, XrefRangeEnd = 1500140, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnCollisionExit(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.NativeMethodInfoPtr_OnCollisionExit_Private_Void_Collision_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009674 File Offset: 0x00007874
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000096A8 File Offset: 0x000078A8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500140, XrefRangeEnd = 1500147, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Il2CppArrayBase<T> ConvertToArray<T>(IList list)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(list);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.MethodInfoStoreGeneric_ConvertToArray_Public_Il2CppArrayBase_1_T_IList_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000096F0 File Offset: 0x000078F0
		[CallerCount(0)]
		public unsafe void SetBlock(bool value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref value;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009730 File Offset: 0x00007930
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OnCollisionExitProxy()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(OnCollisionExitProxy.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00002E6A File Offset: 0x0000106A
		public OnCollisionExitProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000976C File Offset: 0x0000796C
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00002E73 File Offset: 0x00001073
		public unsafe Il2CppReferenceArray<IOnCollisionExitProxy> _clients
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionExitProxy.NativeFieldInfoPtr__clients);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<IOnCollisionExitProxy>>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionExitProxy.NativeFieldInfoPtr__clients), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000979C File Offset: 0x0000799C
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00002E92 File Offset: 0x00001092
		public unsafe bool _block
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionExitProxy.NativeFieldInfoPtr__block);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(OnCollisionExitProxy.NativeFieldInfoPtr__block)) = value;
			}
		}

		// Token: 0x0400012B RID: 299
		private static readonly IntPtr NativeFieldInfoPtr__clients;

		// Token: 0x0400012C RID: 300
		private static readonly IntPtr NativeFieldInfoPtr__block;

		// Token: 0x0400012D RID: 301
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x0400012E RID: 302
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionExit_Private_Void_Collision_0;

		// Token: 0x0400012F RID: 303
		private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

		// Token: 0x04000130 RID: 304
		private static readonly IntPtr NativeMethodInfoPtr_ConvertToArray_Public_Il2CppArrayBase_1_T_IList_0;

		// Token: 0x04000131 RID: 305
		private static readonly IntPtr NativeMethodInfoPtr_SetBlock_Public_Void_Boolean_0;

		// Token: 0x04000132 RID: 306
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x02000048 RID: 72
		private sealed class MethodInfoStoreGeneric_ConvertToArray_Public_Il2CppArrayBase_1_T_IList_0<T>
		{
			// Token: 0x04000182 RID: 386
			internal static IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull(new MethodInfo(IL2CPP.il2cpp_method_get_object(OnCollisionExitProxy.NativeMethodInfoPtr_ConvertToArray_Public_Il2CppArrayBase_1_T_IList_0, Il2CppClassPointerStore<OnCollisionExitProxy>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Type>(new Type[] { Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
		}
	}
}
