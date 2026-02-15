using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000023 RID: 35
	public static class MultipointEx : global::Il2CppSystem.Object
	{
		// Token: 0x06000150 RID: 336 RVA: 0x00007654 File Offset: 0x00005854
		// Note: this type is marked as 'beforefieldinit'.
		static MultipointEx()
		{
			Il2CppClassPointerStore<MultipointEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "MultipointEx");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MultipointEx>.NativeClassPtr);
			MultipointEx.NativeMethodInfoPtr_ClosestPointTo_Public_Static_Vector3_List_1_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MultipointEx>.NativeClassPtr, 100663449);
			MultipointEx.NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_List_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MultipointEx>.NativeClassPtr, 100663450);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000076AC File Offset: 0x000058AC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499856, XrefRangeEnd = 1499865, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Vector3 ClosestPointTo(this List<Vector3> multipoint, Vector3 point)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(multipoint);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref point;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MultipointEx.NativeMethodInfoPtr_ClosestPointTo_Public_Static_Vector3_List_1_Vector3_Vector3_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000076FC File Offset: 0x000058FC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499865, XrefRangeEnd = 1499876, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Vector3 GetCenterPosition(this List<Vector3> multipoint)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(multipoint);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MultipointEx.NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_List_1_Vector3_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000028E4 File Offset: 0x00000AE4
		public MultipointEx(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x040000CB RID: 203
		private static readonly IntPtr NativeMethodInfoPtr_ClosestPointTo_Public_Static_Vector3_List_1_Vector3_Vector3_0;

		// Token: 0x040000CC RID: 204
		private static readonly IntPtr NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_List_1_Vector3_0;
	}
}
