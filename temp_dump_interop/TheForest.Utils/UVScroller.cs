using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class UVScroller : MonoBehaviour
{
	// Token: 0x0600007E RID: 126 RVA: 0x00004E58 File Offset: 0x00003058
	// Note: this type is marked as 'beforefieldinit'.
	static UVScroller()
	{
		Il2CppClassPointerStore<UVScroller>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "UVScroller");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UVScroller>.NativeClassPtr);
		UVScroller.NativeFieldInfoPtr_Speed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, "Speed");
		UVScroller.NativeFieldInfoPtr_ScrollAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, "ScrollAmount");
		UVScroller.NativeFieldInfoPtr_AutoWrap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, "AutoWrap");
		UVScroller.NativeFieldInfoPtr__instancedMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, "_instancedMaterial");
		UVScroller.NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, 100663362);
		UVScroller.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, 100663363);
		UVScroller.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, 100663364);
		UVScroller.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UVScroller>.NativeClassPtr, 100663365);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004F28 File Offset: 0x00003128
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499004, XrefRangeEnd = 1499018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(UVScroller.NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004F5C File Offset: 0x0000315C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499018, XrefRangeEnd = 1499043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(UVScroller.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00004F90 File Offset: 0x00003190
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499043, XrefRangeEnd = 1499055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(UVScroller.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00004FC4 File Offset: 0x000031C4
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499055, XrefRangeEnd = 1499058, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UVScroller()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UVScroller>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(UVScroller.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00002320 File Offset: 0x00000520
	public UVScroller(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000084 RID: 132 RVA: 0x00005000 File Offset: 0x00003200
	// (set) Token: 0x06000085 RID: 133 RVA: 0x00002329 File Offset: 0x00000529
	public unsafe float Speed
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_Speed);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_Speed)) = value;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000086 RID: 134 RVA: 0x00005028 File Offset: 0x00003228
	// (set) Token: 0x06000087 RID: 135 RVA: 0x00002344 File Offset: 0x00000544
	public unsafe Vector2 ScrollAmount
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_ScrollAmount);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_ScrollAmount)) = value;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000088 RID: 136 RVA: 0x00005050 File Offset: 0x00003250
	// (set) Token: 0x06000089 RID: 137 RVA: 0x0000235F File Offset: 0x0000055F
	public unsafe bool AutoWrap
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_AutoWrap);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr_AutoWrap)) = value;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00005078 File Offset: 0x00003278
	// (set) Token: 0x0600008B RID: 139 RVA: 0x0000237A File Offset: 0x0000057A
	public unsafe Material _instancedMaterial
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr__instancedMaterial);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(UVScroller.NativeFieldInfoPtr__instancedMaterial), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x0400004F RID: 79
	private static readonly IntPtr NativeFieldInfoPtr_Speed;

	// Token: 0x04000050 RID: 80
	private static readonly IntPtr NativeFieldInfoPtr_ScrollAmount;

	// Token: 0x04000051 RID: 81
	private static readonly IntPtr NativeFieldInfoPtr_AutoWrap;

	// Token: 0x04000052 RID: 82
	private static readonly IntPtr NativeFieldInfoPtr__instancedMaterial;

	// Token: 0x04000053 RID: 83
	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	// Token: 0x04000054 RID: 84
	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	// Token: 0x04000055 RID: 85
	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	// Token: 0x04000056 RID: 86
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
