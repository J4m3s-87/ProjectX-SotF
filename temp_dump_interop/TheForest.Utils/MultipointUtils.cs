using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000024 RID: 36
	public static class MultipointUtils : global::Il2CppSystem.Object
	{
		// Token: 0x06000154 RID: 340 RVA: 0x000028ED File Offset: 0x00000AED
		// Note: this type is marked as 'beforefieldinit'.
		static MultipointUtils()
		{
			Il2CppClassPointerStore<MultipointUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "MultipointUtils");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MultipointUtils>.NativeClassPtr);
			MultipointUtils.NativeMethodInfoPtr_CenterOf_Public_Static_Vector3_List_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MultipointUtils>.NativeClassPtr, 100663451);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007740 File Offset: 0x00005940
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499876, XrefRangeEnd = 1499879, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Vector3 CenterOf(List<Vector3> multipoint)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(multipoint);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MultipointUtils.NativeMethodInfoPtr_CenterOf_Public_Static_Vector3_List_1_Vector3_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00002926 File Offset: 0x00000B26
		public MultipointUtils(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x040000CD RID: 205
		private static readonly IntPtr NativeMethodInfoPtr_CenterOf_Public_Static_Vector3_List_1_Vector3_0;
	}
}
