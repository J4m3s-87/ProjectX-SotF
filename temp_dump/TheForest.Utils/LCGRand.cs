using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

// Token: 0x02000006 RID: 6
public class LCGRand : Object
{
	// Token: 0x06000031 RID: 49 RVA: 0x00003E6C File Offset: 0x0000206C
	// Note: this type is marked as 'beforefieldinit'.
	static LCGRand()
	{
		Il2CppClassPointerStore<LCGRand>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "LCGRand");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LCGRand>.NativeClassPtr);
		LCGRand.NativeFieldInfoPtr_Seed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, "Seed");
		LCGRand.NativeMethodInfoPtr_Next_Private_Static_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, 100663323);
		LCGRand.NativeMethodInfoPtr_get_Value01_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, 100663324);
		LCGRand.NativeMethodInfoPtr_Range_Public_Static_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, 100663325);
		LCGRand.NativeMethodInfoPtr_Range_Public_Static_Int32_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, 100663326);
		LCGRand.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LCGRand>.NativeClassPtr, 100663327);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003F14 File Offset: 0x00002114
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498555, XrefRangeEnd = 1498560, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static uint Next()
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LCGRand.NativeMethodInfoPtr_Next_Private_Static_UInt32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *IL2CPP.il2cpp_object_unbox(intPtr);
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000033 RID: 51 RVA: 0x00003F44 File Offset: 0x00002144
	public unsafe static float Value01
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 1498568, RefRangeEnd = 1498571, XrefRangeStart = 1498560, XrefRangeEnd = 1498568, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LCGRand.NativeMethodInfoPtr_get_Value01_Public_Static_get_Single_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003F74 File Offset: 0x00002174
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498571, XrefRangeEnd = 1498575, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float Range(float min, float max)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref min;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref max;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LCGRand.NativeMethodInfoPtr_Range_Public_Static_Single_Single_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *IL2CPP.il2cpp_object_unbox(intPtr);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003FC0 File Offset: 0x000021C0
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498575, XrefRangeEnd = 1498579, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int Range(int min, int max)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref min;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref max;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LCGRand.NativeMethodInfoPtr_Range_Public_Static_Int32_Int32_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *IL2CPP.il2cpp_object_unbox(intPtr);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0000400C File Offset: 0x0000220C
	[CallerCount(250)]
	[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LCGRand()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LCGRand>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LCGRand.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000214F File Offset: 0x0000034F
	public LCGRand(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000038 RID: 56 RVA: 0x00004048 File Offset: 0x00002248
	// (set) Token: 0x06000039 RID: 57 RVA: 0x00002158 File Offset: 0x00000358
	public unsafe static uint Seed
	{
		get
		{
			uint num;
			IL2CPP.il2cpp_field_static_get_value(LCGRand.NativeFieldInfoPtr_Seed, (void*)(&num));
			return num;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(LCGRand.NativeFieldInfoPtr_Seed, (void*)(&value));
		}
	}

	// Token: 0x04000022 RID: 34
	private static readonly IntPtr NativeFieldInfoPtr_Seed;

	// Token: 0x04000023 RID: 35
	private static readonly IntPtr NativeMethodInfoPtr_Next_Private_Static_UInt32_0;

	// Token: 0x04000024 RID: 36
	private static readonly IntPtr NativeMethodInfoPtr_get_Value01_Public_Static_get_Single_0;

	// Token: 0x04000025 RID: 37
	private static readonly IntPtr NativeMethodInfoPtr_Range_Public_Static_Single_Single_Single_0;

	// Token: 0x04000026 RID: 38
	private static readonly IntPtr NativeMethodInfoPtr_Range_Public_Static_Int32_Int32_Int32_0;

	// Token: 0x04000027 RID: 39
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
