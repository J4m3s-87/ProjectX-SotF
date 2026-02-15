using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class AnimationCurveExtension : global::Il2CppSystem.Object
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	// Note: this type is marked as 'beforefieldinit'.
	static AnimationCurveExtension()
	{
		Il2CppClassPointerStore<AnimationCurveExtension>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "AnimationCurveExtension");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationCurveExtension>.NativeClassPtr);
		AnimationCurveExtension.NativeMethodInfoPtr_LogFrameInCurve_Public_Static_Void_AnimationCurve_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationCurveExtension>.NativeClassPtr, 100663297);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00003300 File Offset: 0x00001500
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498224, XrefRangeEnd = 1498243, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogFrameInCurve(this AnimationCurve curve, float val, float ignoreThreshold = 0.2f)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(curve);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref val;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref ignoreThreshold;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(AnimationCurveExtension.NativeMethodInfoPtr_LogFrameInCurve_Public_Static_Void_AnimationCurve_Single_Single_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002089 File Offset: 0x00000289
	public AnimationCurveExtension(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x04000001 RID: 1
	private static readonly IntPtr NativeMethodInfoPtr_LogFrameInCurve_Public_Static_Void_AnimationCurve_Single_Single_0;
}
