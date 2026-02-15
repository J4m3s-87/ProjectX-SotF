using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class MeshCombiner : MonoBehaviour
{
	// Token: 0x06000055 RID: 85 RVA: 0x00004530 File Offset: 0x00002730
	// Note: this type is marked as 'beforefieldinit'.
	static MeshCombiner()
	{
		Il2CppClassPointerStore<MeshCombiner>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "MeshCombiner");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshCombiner>.NativeClassPtr);
		MeshCombiner.NativeMethodInfoPtr_CombineMeshes_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombiner>.NativeClassPtr, 100663344);
		MeshCombiner.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombiner>.NativeClassPtr, 100663345);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004588 File Offset: 0x00002788
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498647, XrefRangeEnd = 1498732, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CombineMeshes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombiner.NativeMethodInfoPtr_CombineMeshes_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000045BC File Offset: 0x000027BC
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshCombiner()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshCombiner>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombiner.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0000222F File Offset: 0x0000042F
	public MeshCombiner(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x04000037 RID: 55
	private static readonly IntPtr NativeMethodInfoPtr_CombineMeshes_Public_Void_0;

	// Token: 0x04000038 RID: 56
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
}
