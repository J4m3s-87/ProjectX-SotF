using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000018 RID: 24
	public class DestroyGOListOnDestroy : MonoBehaviour
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00005F1C File Offset: 0x0000411C
		// Note: this type is marked as 'beforefieldinit'.
		static DestroyGOListOnDestroy()
		{
			Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DestroyGOListOnDestroy");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr);
			DestroyGOListOnDestroy.NativeFieldInfoPtr__golist = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr, "_golist");
			DestroyGOListOnDestroy.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr, 100663399);
			DestroyGOListOnDestroy.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr, 100663400);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005F88 File Offset: 0x00004188
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499424, XrefRangeEnd = 1499437, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DestroyGOListOnDestroy.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005FBC File Offset: 0x000041BC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DestroyGOListOnDestroy()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DestroyGOListOnDestroy>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DestroyGOListOnDestroy.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000254F File Offset: 0x0000074F
		public DestroyGOListOnDestroy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005FF8 File Offset: 0x000041F8
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00002558 File Offset: 0x00000758
		public unsafe Il2CppReferenceArray<GameObject> _golist
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DestroyGOListOnDestroy.NativeFieldInfoPtr__golist);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<GameObject>>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DestroyGOListOnDestroy.NativeFieldInfoPtr__golist), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x04000083 RID: 131
		private static readonly IntPtr NativeFieldInfoPtr__golist;

		// Token: 0x04000084 RID: 132
		private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

		// Token: 0x04000085 RID: 133
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
