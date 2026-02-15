using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002F RID: 47
	public class TriggerTagSensor : MonoBehaviour
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x00008B80 File Offset: 0x00006D80
		// Note: this type is marked as 'beforefieldinit'.
		static TriggerTagSensor()
		{
			Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "TriggerTagSensor");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr);
			TriggerTagSensor.NativeFieldInfoPtr__TargetTag_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, "<TargetTag>k__BackingField");
			TriggerTagSensor.NativeFieldInfoPtr__Target_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, "<Target>k__BackingField");
			TriggerTagSensor.NativeMethodInfoPtr_get_TargetTag_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663493);
			TriggerTagSensor.NativeMethodInfoPtr_set_TargetTag_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663494);
			TriggerTagSensor.NativeMethodInfoPtr_get_Target_Public_get_ITarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663495);
			TriggerTagSensor.NativeMethodInfoPtr_set_Target_Public_set_Void_ITarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663496);
			TriggerTagSensor.NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663497);
			TriggerTagSensor.NativeMethodInfoPtr_OnTriggerExit_Private_Void_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663498);
			TriggerTagSensor.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, 100663499);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00008C64 File Offset: 0x00006E64
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008C9C File Offset: 0x00006E9C
		public unsafe string TargetTag
		{
			[CallerCount(15)]
			[CachedScanResults(RefRangeStart = 11039, RefRangeEnd = 11054, XrefRangeStart = 11039, XrefRangeEnd = 11054, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_get_TargetTag_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			[CallerCount(61)]
			[CachedScanResults(RefRangeStart = 11054, RefRangeEnd = 11115, XrefRangeStart = 11054, XrefRangeEnd = 11115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_set_TargetTag_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00008CE0 File Offset: 0x00006EE0
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00008D20 File Offset: 0x00006F20
		public unsafe TriggerTagSensor.ITarget Target
		{
			[CallerCount(7)]
			[CachedScanResults(RefRangeStart = 11115, RefRangeEnd = 11122, XrefRangeStart = 11115, XrefRangeEnd = 11122, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_get_Target_Public_get_ITarget_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<TriggerTagSensor.ITarget>(intPtr3) : null;
			}
			[CallerCount(117)]
			[CachedScanResults(RefRangeStart = 11122, RefRangeEnd = 11239, XrefRangeStart = 11122, XrefRangeEnd = 11239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.Il2CppObjectBaseToPtr(value);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_set_Target_Public_set_Void_ITarget_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008D64 File Offset: 0x00006F64
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500113, XrefRangeEnd = 1500119, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnTriggerEnter(Collider other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(other);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008DA8 File Offset: 0x00006FA8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500119, XrefRangeEnd = 1500125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnTriggerExit(Collider other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(other);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr_OnTriggerExit_Private_Void_Collider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008DEC File Offset: 0x00006FEC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe TriggerTagSensor()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(TriggerTagSensor.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00002C73 File Offset: 0x00000E73
		public TriggerTagSensor(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00008E28 File Offset: 0x00007028
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00002C7C File Offset: 0x00000E7C
		public unsafe string _TargetTag_k__BackingField
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(TriggerTagSensor.NativeFieldInfoPtr__TargetTag_k__BackingField);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(TriggerTagSensor.NativeFieldInfoPtr__TargetTag_k__BackingField), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00008E50 File Offset: 0x00007050
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00002C9B File Offset: 0x00000E9B
		public unsafe TriggerTagSensor.ITarget _Target_k__BackingField
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(TriggerTagSensor.NativeFieldInfoPtr__Target_k__BackingField);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<TriggerTagSensor.ITarget>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(TriggerTagSensor.NativeFieldInfoPtr__Target_k__BackingField), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x0400010C RID: 268
		private static readonly IntPtr NativeFieldInfoPtr__TargetTag_k__BackingField;

		// Token: 0x0400010D RID: 269
		private static readonly IntPtr NativeFieldInfoPtr__Target_k__BackingField;

		// Token: 0x0400010E RID: 270
		private static readonly IntPtr NativeMethodInfoPtr_get_TargetTag_Public_get_String_0;

		// Token: 0x0400010F RID: 271
		private static readonly IntPtr NativeMethodInfoPtr_set_TargetTag_Public_set_Void_String_0;

		// Token: 0x04000110 RID: 272
		private static readonly IntPtr NativeMethodInfoPtr_get_Target_Public_get_ITarget_0;

		// Token: 0x04000111 RID: 273
		private static readonly IntPtr NativeMethodInfoPtr_set_Target_Public_set_Void_ITarget_0;

		// Token: 0x04000112 RID: 274
		private static readonly IntPtr NativeMethodInfoPtr_OnTriggerEnter_Private_Void_Collider_0;

		// Token: 0x04000113 RID: 275
		private static readonly IntPtr NativeMethodInfoPtr_OnTriggerExit_Private_Void_Collider_0;

		// Token: 0x04000114 RID: 276
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x02000047 RID: 71
		public class ITarget : Il2CppObjectBase
		{
			// Token: 0x0600026F RID: 623 RVA: 0x00003268 File Offset: 0x00001468
			// Note: this type is marked as 'beforefieldinit'.
			static ITarget()
			{
				Il2CppClassPointerStore<TriggerTagSensor.ITarget>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<TriggerTagSensor>.NativeClassPtr, "ITarget");
				TriggerTagSensor.ITarget.NativeMethodInfoPtr_OnTargetTagTrigerEnter_Public_Abstract_Virtual_New_Void_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor.ITarget>.NativeClassPtr, 100663500);
				TriggerTagSensor.ITarget.NativeMethodInfoPtr_OnTargetTagTrigerExit_Public_Abstract_Virtual_New_Void_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerTagSensor.ITarget>.NativeClassPtr, 100663501);
			}

			// Token: 0x06000270 RID: 624 RVA: 0x0000A8B4 File Offset: 0x00008AB4
			[CallerCount(0)]
			public unsafe virtual void OnTargetTagTrigerEnter(Collider other)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.Il2CppObjectBaseToPtr(other);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr(this), TriggerTagSensor.ITarget.NativeMethodInfoPtr_OnTargetTagTrigerEnter_Public_Abstract_Virtual_New_Void_Collider_0), IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				}
			}

			// Token: 0x06000271 RID: 625 RVA: 0x0000A904 File Offset: 0x00008B04
			[CallerCount(0)]
			public unsafe virtual void OnTargetTagTrigerExit(Collider other)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.Il2CppObjectBaseToPtr(other);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr(this), TriggerTagSensor.ITarget.NativeMethodInfoPtr_OnTargetTagTrigerExit_Public_Abstract_Virtual_New_Void_Collider_0), IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				}
			}

			// Token: 0x06000272 RID: 626 RVA: 0x000032A6 File Offset: 0x000014A6
			public ITarget(IntPtr pointer)
				: base(pointer)
			{
			}

			// Token: 0x04000180 RID: 384
			private static readonly IntPtr NativeMethodInfoPtr_OnTargetTagTrigerEnter_Public_Abstract_Virtual_New_Void_Collider_0;

			// Token: 0x04000181 RID: 385
			private static readonly IntPtr NativeMethodInfoPtr_OnTargetTagTrigerExit_Public_Abstract_Virtual_New_Void_Collider_0;
		}
	}
}
