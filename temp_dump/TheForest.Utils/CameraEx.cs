using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000011 RID: 17
	public static class CameraEx : global::Il2CppSystem.Object
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00005168 File Offset: 0x00003368
		// Note: this type is marked as 'beforefieldinit'.
		static CameraEx()
		{
			Il2CppClassPointerStore<CameraEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "CameraEx");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CameraEx>.NativeClassPtr);
			CameraEx.NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraEx>.NativeClassPtr, 100663368);
			CameraEx.NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Renderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraEx>.NativeClassPtr, 100663369);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000051C0 File Offset: 0x000033C0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499069, XrefRangeEnd = 1499123, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Rect GetScreenRectOf(this Camera cam, Collider collider)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(cam);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(collider);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(CameraEx.NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Collider_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005214 File Offset: 0x00003414
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499123, XrefRangeEnd = 1499210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Rect GetScreenRectOf(this Camera cam, Renderer renderer)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(cam);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(renderer);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(CameraEx.NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Renderer_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000023A2 File Offset: 0x000005A2
		public CameraEx(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000059 RID: 89
		private static readonly IntPtr NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Collider_0;

		// Token: 0x0400005A RID: 90
		private static readonly IntPtr NativeMethodInfoPtr_GetScreenRectOf_Public_Static_Rect_Camera_Renderer_0;
	}
}
