using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class IntervalTextureLogger : MonoBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00003858 File Offset: 0x00001A58
	// Note: this type is marked as 'beforefieldinit'.
	static IntervalTextureLogger()
	{
		Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "IntervalTextureLogger");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr);
		IntervalTextureLogger.NativeFieldInfoPtr_Interval = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, "Interval");
		IntervalTextureLogger.NativeFieldInfoPtr__lastRun = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, "_lastRun");
		IntervalTextureLogger.NativeFieldInfoPtr__instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, "_instance");
		IntervalTextureLogger.NativeMethodInfoPtr_Start_Public_Static_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663309);
		IntervalTextureLogger.NativeMethodInfoPtr_Stop_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663310);
		IntervalTextureLogger.NativeMethodInfoPtr_GetBitsPerPixel_Private_Int32_TextureFormat_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663311);
		IntervalTextureLogger.NativeMethodInfoPtr_CalculateTextureSizeBytes_Private_Int32_Texture_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663312);
		IntervalTextureLogger.NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663313);
		IntervalTextureLogger.NativeMethodInfoPtr_ToReadable_Private_Static_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663314);
		IntervalTextureLogger.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr, 100663315);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00003950 File Offset: 0x00001B50
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498287, XrefRangeEnd = 1498310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Start(float interval)
	{
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref interval;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_Start_Public_Static_Void_Single_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00003984 File Offset: 0x00001B84
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498310, XrefRangeEnd = 1498327, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Stop()
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_Stop_Public_Static_Void_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000039AC File Offset: 0x00001BAC
	[CallerCount(0)]
	public unsafe int GetBitsPerPixel(TextureFormat format)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref format;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_GetBitsPerPixel_Private_Int32_TextureFormat_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000039F8 File Offset: 0x00001BF8
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1498330, RefRangeEnd = 1498331, XrefRangeStart = 1498327, XrefRangeEnd = 1498330, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int CalculateTextureSizeBytes(Texture tTexture)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(tTexture);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_CalculateTextureSizeBytes_Private_Int32_Texture_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00003A48 File Offset: 0x00001C48
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498331, XrefRangeEnd = 1498414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00003A7C File Offset: 0x00001C7C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498414, XrefRangeEnd = 1498440, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ToReadable(int size)
	{
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref size;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr_ToReadable_Private_Static_String_Int32_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003AB4 File Offset: 0x00001CB4
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IntervalTextureLogger()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IntervalTextureLogger>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IntervalTextureLogger.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000020A9 File Offset: 0x000002A9
	public IntervalTextureLogger(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600001C RID: 28 RVA: 0x00003AF0 File Offset: 0x00001CF0
	// (set) Token: 0x0600001D RID: 29 RVA: 0x000020B2 File Offset: 0x000002B2
	public unsafe float Interval
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(IntervalTextureLogger.NativeFieldInfoPtr_Interval);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(IntervalTextureLogger.NativeFieldInfoPtr_Interval)) = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600001E RID: 30 RVA: 0x00003B18 File Offset: 0x00001D18
	// (set) Token: 0x0600001F RID: 31 RVA: 0x000020CD File Offset: 0x000002CD
	public unsafe float _lastRun
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(IntervalTextureLogger.NativeFieldInfoPtr__lastRun);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(IntervalTextureLogger.NativeFieldInfoPtr__lastRun)) = value;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00003B40 File Offset: 0x00001D40
	// (set) Token: 0x06000021 RID: 33 RVA: 0x000020E8 File Offset: 0x000002E8
	public unsafe static IntervalTextureLogger _instance
	{
		get
		{
			IntPtr intPtr;
			IL2CPP.il2cpp_field_static_get_value(IntervalTextureLogger.NativeFieldInfoPtr__instance, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<IntervalTextureLogger>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(IntervalTextureLogger.NativeFieldInfoPtr__instance, IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x0400000E RID: 14
	private static readonly IntPtr NativeFieldInfoPtr_Interval;

	// Token: 0x0400000F RID: 15
	private static readonly IntPtr NativeFieldInfoPtr__lastRun;

	// Token: 0x04000010 RID: 16
	private static readonly IntPtr NativeFieldInfoPtr__instance;

	// Token: 0x04000011 RID: 17
	private static readonly IntPtr NativeMethodInfoPtr_Start_Public_Static_Void_Single_0;

	// Token: 0x04000012 RID: 18
	private static readonly IntPtr NativeMethodInfoPtr_Stop_Public_Static_Void_0;

	// Token: 0x04000013 RID: 19
	private static readonly IntPtr NativeMethodInfoPtr_GetBitsPerPixel_Private_Int32_TextureFormat_0;

	// Token: 0x04000014 RID: 20
	private static readonly IntPtr NativeMethodInfoPtr_CalculateTextureSizeBytes_Private_Int32_Texture_0;

	// Token: 0x04000015 RID: 21
	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	// Token: 0x04000016 RID: 22
	private static readonly IntPtr NativeMethodInfoPtr_ToReadable_Private_Static_String_Int32_0;

	// Token: 0x04000017 RID: 23
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
