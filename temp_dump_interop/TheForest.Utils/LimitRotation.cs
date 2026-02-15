using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class LimitRotation : MonoBehaviour
{
	// Token: 0x0600003A RID: 58 RVA: 0x00004064 File Offset: 0x00002264
	// Note: this type is marked as 'beforefieldinit'.
	static LimitRotation()
	{
		Il2CppClassPointerStore<LimitRotation>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "LimitRotation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr);
		LimitRotation.NativeFieldInfoPtr_m_MaxRotationAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, "m_MaxRotationAngle");
		LimitRotation.NativeFieldInfoPtr_m_RigidBody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, "m_RigidBody");
		LimitRotation.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, 100663329);
		LimitRotation.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, 100663330);
		LimitRotation.NativeMethodInfoPtr_ClampRotation_Private_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, 100663331);
		LimitRotation.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr, 100663332);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000410C File Offset: 0x0000230C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498579, XrefRangeEnd = 1498591, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitRotation.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00004140 File Offset: 0x00002340
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498591, XrefRangeEnd = 1498616, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitRotation.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00004174 File Offset: 0x00002374
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498616, XrefRangeEnd = 1498618, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ClampRotation(Vector3 rotationAngle)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref rotationAngle;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitRotation.NativeMethodInfoPtr_ClampRotation_Private_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000041C0 File Offset: 0x000023C0
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498618, XrefRangeEnd = 1498621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LimitRotation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LimitRotation>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitRotation.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002166 File Offset: 0x00000366
	public LimitRotation(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000040 RID: 64 RVA: 0x000041FC File Offset: 0x000023FC
	// (set) Token: 0x06000041 RID: 65 RVA: 0x0000216F File Offset: 0x0000036F
	public unsafe float m_MaxRotationAngle
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitRotation.NativeFieldInfoPtr_m_MaxRotationAngle);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitRotation.NativeFieldInfoPtr_m_MaxRotationAngle)) = value;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000042 RID: 66 RVA: 0x00004224 File Offset: 0x00002424
	// (set) Token: 0x06000043 RID: 67 RVA: 0x0000218A File Offset: 0x0000038A
	public unsafe Rigidbody m_RigidBody
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitRotation.NativeFieldInfoPtr_m_RigidBody);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitRotation.NativeFieldInfoPtr_m_RigidBody), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x04000028 RID: 40
	private static readonly IntPtr NativeFieldInfoPtr_m_MaxRotationAngle;

	// Token: 0x04000029 RID: 41
	private static readonly IntPtr NativeFieldInfoPtr_m_RigidBody;

	// Token: 0x0400002A RID: 42
	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	// Token: 0x0400002B RID: 43
	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	// Token: 0x0400002C RID: 44
	private static readonly IntPtr NativeMethodInfoPtr_ClampRotation_Private_Vector3_Vector3_0;

	// Token: 0x0400002D RID: 45
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
