using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000013 RID: 19
	public class CheckUniqueTag : MonoBehaviour
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00005544 File Offset: 0x00003744
		// Note: this type is marked as 'beforefieldinit'.
		static CheckUniqueTag()
		{
			Il2CppClassPointerStore<CheckUniqueTag>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "CheckUniqueTag");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CheckUniqueTag>.NativeClassPtr);
			CheckUniqueTag.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckUniqueTag>.NativeClassPtr, 100663375);
			CheckUniqueTag.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckUniqueTag>.NativeClassPtr, 100663376);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000559C File Offset: 0x0000379C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499236, XrefRangeEnd = 1499245, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(CheckUniqueTag.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000055D0 File Offset: 0x000037D0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CheckUniqueTag()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CheckUniqueTag>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(CheckUniqueTag.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002443 File Offset: 0x00000643
		public CheckUniqueTag(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000065 RID: 101
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x04000066 RID: 102
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
