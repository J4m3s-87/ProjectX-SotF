using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

// Token: 0x02000003 RID: 3
public static class BoltSetReflectedShim : global::Il2CppSystem.Object
{
	// Token: 0x06000004 RID: 4 RVA: 0x00003354 File Offset: 0x00001554
	// Note: this type is marked as 'beforefieldinit'.
	static BoltSetReflectedShim()
	{
		Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "BoltSetReflectedShim");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr);
		BoltSetReflectedShim.NativeFieldInfoPtr_FLOAT_MIN_DELTA = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, "FLOAT_MIN_DELTA");
		BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663298);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663299);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663300);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663301);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663302);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663303);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663304);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663305);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663306);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663307);
		BoltSetReflectedShim.NativeMethodInfoPtr_SetLayerWeightReflected_Public_Static_Void_Animator_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoltSetReflectedShim>.NativeClassPtr, 100663308);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00003474 File Offset: 0x00001674
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498243, XrefRangeEnd = 1498245, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetFloatReflected(this Animator animator, string name, float value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000034CC File Offset: 0x000016CC
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498245, XrefRangeEnd = 1498247, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetFloatReflected(this Animator animator, string name, float value, float dampTime, float deltaTime)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)5) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref dampTime;
		ptr[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref deltaTime;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_Single_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00003540 File Offset: 0x00001740
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498247, XrefRangeEnd = 1498252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetFloatReflected(this Animator animator, int name, float value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref name;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00003594 File Offset: 0x00001794
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498252, XrefRangeEnd = 1498257, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetFloatReflected(this Animator animator, int name, float value, float dampTime, float deltaTime)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)5) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref name;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref dampTime;
		ptr[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref deltaTime;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_Single_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00003604 File Offset: 0x00001804
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498257, XrefRangeEnd = 1498259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetIntegerReflected(this Animator animator, string name, int value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_String_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000365C File Offset: 0x0000185C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498259, XrefRangeEnd = 1498264, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetIntegerReflected(this Animator animator, int nameHash, int value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref nameHash;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_Int32_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000036B0 File Offset: 0x000018B0
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498264, XrefRangeEnd = 1498266, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetBoolReflected(this Animator animator, string name, bool value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_String_Boolean_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00003708 File Offset: 0x00001908
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498266, XrefRangeEnd = 1498268, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetBoolReflected(this Animator animator, int nameHash, bool value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref nameHash;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_Int32_Boolean_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000375C File Offset: 0x0000195C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498268, XrefRangeEnd = 1498273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetTriggerReflected(this Animator animator, string name)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_String_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000037A4 File Offset: 0x000019A4
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498273, XrefRangeEnd = 1498278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetTriggerReflected(this Animator animator, int nameHash)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref nameHash;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000037E8 File Offset: 0x000019E8
	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 1498283, RefRangeEnd = 1498287, XrefRangeStart = 1498278, XrefRangeEnd = 1498283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetLayerWeightReflected(this Animator animator, int index, float value)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(animator);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref index;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref value;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(BoltSetReflectedShim.NativeMethodInfoPtr_SetLayerWeightReflected_Public_Static_Void_Animator_Int32_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002092 File Offset: 0x00000292
	public BoltSetReflectedShim(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000011 RID: 17 RVA: 0x0000383C File Offset: 0x00001A3C
	// (set) Token: 0x06000012 RID: 18 RVA: 0x0000209B File Offset: 0x0000029B
	public unsafe static float FLOAT_MIN_DELTA
	{
		get
		{
			float num;
			IL2CPP.il2cpp_field_static_get_value(BoltSetReflectedShim.NativeFieldInfoPtr_FLOAT_MIN_DELTA, (void*)(&num));
			return num;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(BoltSetReflectedShim.NativeFieldInfoPtr_FLOAT_MIN_DELTA, (void*)(&value));
		}
	}

	// Token: 0x04000002 RID: 2
	private static readonly IntPtr NativeFieldInfoPtr_FLOAT_MIN_DELTA;

	// Token: 0x04000003 RID: 3
	private static readonly IntPtr NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_0;

	// Token: 0x04000004 RID: 4
	private static readonly IntPtr NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_String_Single_Single_Single_0;

	// Token: 0x04000005 RID: 5
	private static readonly IntPtr NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_0;

	// Token: 0x04000006 RID: 6
	private static readonly IntPtr NativeMethodInfoPtr_SetFloatReflected_Public_Static_Void_Animator_Int32_Single_Single_Single_0;

	// Token: 0x04000007 RID: 7
	private static readonly IntPtr NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_String_Int32_0;

	// Token: 0x04000008 RID: 8
	private static readonly IntPtr NativeMethodInfoPtr_SetIntegerReflected_Public_Static_Void_Animator_Int32_Int32_0;

	// Token: 0x04000009 RID: 9
	private static readonly IntPtr NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_String_Boolean_0;

	// Token: 0x0400000A RID: 10
	private static readonly IntPtr NativeMethodInfoPtr_SetBoolReflected_Public_Static_Void_Animator_Int32_Boolean_0;

	// Token: 0x0400000B RID: 11
	private static readonly IntPtr NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_String_0;

	// Token: 0x0400000C RID: 12
	private static readonly IntPtr NativeMethodInfoPtr_SetTriggerReflected_Public_Static_Void_Animator_Int32_0;

	// Token: 0x0400000D RID: 13
	private static readonly IntPtr NativeMethodInfoPtr_SetLayerWeightReflected_Public_Static_Void_Animator_Int32_Single_0;
}
