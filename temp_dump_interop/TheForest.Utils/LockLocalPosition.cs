using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class LockLocalPosition : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x000043A0 File Offset: 0x000025A0
	// Note: this type is marked as 'beforefieldinit'.
	static LockLocalPosition()
	{
		Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "LockLocalPosition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr);
		LockLocalPosition.NativeFieldInfoPtr_localPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr, "localPos");
		LockLocalPosition.NativeFieldInfoPtr__transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr, "_transform");
		LockLocalPosition.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr, 100663341);
		LockLocalPosition.NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr, 100663342);
		LockLocalPosition.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr, 100663343);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004434 File Offset: 0x00002634
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498639, XrefRangeEnd = 1498643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LockLocalPosition.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004468 File Offset: 0x00002668
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498643, XrefRangeEnd = 1498647, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LockLocalPosition.NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000449C File Offset: 0x0000269C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LockLocalPosition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LockLocalPosition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LockLocalPosition.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000021EC File Offset: 0x000003EC
	public LockLocalPosition(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000051 RID: 81 RVA: 0x000044D8 File Offset: 0x000026D8
	// (set) Token: 0x06000052 RID: 82 RVA: 0x000021F5 File Offset: 0x000003F5
	public unsafe Vector3 localPos
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LockLocalPosition.NativeFieldInfoPtr_localPos);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LockLocalPosition.NativeFieldInfoPtr_localPos)) = value;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00004500 File Offset: 0x00002700
	// (set) Token: 0x06000054 RID: 84 RVA: 0x00002210 File Offset: 0x00000410
	public unsafe Transform _transform
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LockLocalPosition.NativeFieldInfoPtr__transform);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Transform>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LockLocalPosition.NativeFieldInfoPtr__transform), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x04000032 RID: 50
	private static readonly IntPtr NativeFieldInfoPtr_localPos;

	// Token: 0x04000033 RID: 51
	private static readonly IntPtr NativeFieldInfoPtr__transform;

	// Token: 0x04000034 RID: 52
	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	// Token: 0x04000035 RID: 53
	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	// Token: 0x04000036 RID: 54
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
