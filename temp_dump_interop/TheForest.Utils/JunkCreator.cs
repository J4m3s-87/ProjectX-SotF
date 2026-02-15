using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class JunkCreator : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00003B68 File Offset: 0x00001D68
	// Note: this type is marked as 'beforefieldinit'.
	static JunkCreator()
	{
		Il2CppClassPointerStore<JunkCreator>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "JunkCreator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr);
		JunkCreator.NativeFieldInfoPtr_instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, "instance");
		JunkCreator.NativeFieldInfoPtr_arraySize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, "arraySize");
		JunkCreator.NativeFieldInfoPtr_junkArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, "junkArray");
		JunkCreator.NativeMethodInfoPtr_get_Instance_Public_Static_get_JunkCreator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663316);
		JunkCreator.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663317);
		JunkCreator.NativeMethodInfoPtr_Init_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663318);
		JunkCreator.NativeMethodInfoPtr_AddJunk_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663319);
		JunkCreator.NativeMethodInfoPtr_SetJunk_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663320);
		JunkCreator.NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663321);
		JunkCreator.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr, 100663322);
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000023 RID: 35 RVA: 0x00003C60 File Offset: 0x00001E60
	public unsafe static JunkCreator Instance
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1498467, RefRangeEnd = 1498469, XrefRangeStart = 1498440, XrefRangeEnd = 1498467, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_get_Instance_Public_Static_get_JunkCreator_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			IntPtr intPtr3 = intPtr;
			return (intPtr3 != 0) ? Il2CppObjectPool.Get<JunkCreator>(intPtr3) : null;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003C94 File Offset: 0x00001E94
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498469, XrefRangeEnd = 1498486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003CC8 File Offset: 0x00001EC8
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498486, XrefRangeEnd = 1498489, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Init()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_Init_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003CFC File Offset: 0x00001EFC
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498489, XrefRangeEnd = 1498490, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddJunk(int amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref amount;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_AddJunk_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003D3C File Offset: 0x00001F3C
	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1498529, RefRangeEnd = 1498531, XrefRangeStart = 1498490, XrefRangeEnd = 1498529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetJunk(int amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref amount;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_SetJunk_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003D7C File Offset: 0x00001F7C
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1498554, RefRangeEnd = 1498555, XrefRangeStart = 1498531, XrefRangeEnd = 1498554, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003DB0 File Offset: 0x00001FB0
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe JunkCreator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<JunkCreator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(JunkCreator.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000020FA File Offset: 0x000002FA
	public JunkCreator(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600002B RID: 43 RVA: 0x00003DEC File Offset: 0x00001FEC
	// (set) Token: 0x0600002C RID: 44 RVA: 0x00002103 File Offset: 0x00000303
	public unsafe static JunkCreator instance
	{
		get
		{
			IntPtr intPtr;
			IL2CPP.il2cpp_field_static_get_value(JunkCreator.NativeFieldInfoPtr_instance, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<JunkCreator>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(JunkCreator.NativeFieldInfoPtr_instance, IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00003E14 File Offset: 0x00002014
	// (set) Token: 0x0600002E RID: 46 RVA: 0x00002115 File Offset: 0x00000315
	public unsafe int arraySize
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(JunkCreator.NativeFieldInfoPtr_arraySize);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(JunkCreator.NativeFieldInfoPtr_arraySize)) = value;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600002F RID: 47 RVA: 0x00003E3C File Offset: 0x0000203C
	// (set) Token: 0x06000030 RID: 48 RVA: 0x00002130 File Offset: 0x00000330
	public unsafe Il2CppReferenceArray<GameObject> junkArray
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(JunkCreator.NativeFieldInfoPtr_junkArray);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<GameObject>>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(JunkCreator.NativeFieldInfoPtr_junkArray), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x04000018 RID: 24
	private static readonly IntPtr NativeFieldInfoPtr_instance;

	// Token: 0x04000019 RID: 25
	private static readonly IntPtr NativeFieldInfoPtr_arraySize;

	// Token: 0x0400001A RID: 26
	private static readonly IntPtr NativeFieldInfoPtr_junkArray;

	// Token: 0x0400001B RID: 27
	private static readonly IntPtr NativeMethodInfoPtr_get_Instance_Public_Static_get_JunkCreator_0;

	// Token: 0x0400001C RID: 28
	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	// Token: 0x0400001D RID: 29
	private static readonly IntPtr NativeMethodInfoPtr_Init_Public_Void_0;

	// Token: 0x0400001E RID: 30
	private static readonly IntPtr NativeMethodInfoPtr_AddJunk_Public_Void_Int32_0;

	// Token: 0x0400001F RID: 31
	private static readonly IntPtr NativeMethodInfoPtr_SetJunk_Public_Void_Int32_0;

	// Token: 0x04000020 RID: 32
	private static readonly IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	// Token: 0x04000021 RID: 33
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
