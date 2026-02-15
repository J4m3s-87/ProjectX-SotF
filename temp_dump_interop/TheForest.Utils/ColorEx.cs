using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000014 RID: 20
	public static class ColorEx : global::Il2CppSystem.Object
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x0000560C File Offset: 0x0000380C
		// Note: this type is marked as 'beforefieldinit'.
		static ColorEx()
		{
			Il2CppClassPointerStore<ColorEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "ColorEx");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ColorEx>.NativeClassPtr);
			ColorEx.NativeMethodInfoPtr_SqrEuclideanDistance_Public_Static_Single_Color_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ColorEx>.NativeClassPtr, 100663377);
			ColorEx.NativeMethodInfoPtr_ClosestColorIndex_Public_Static_Int32_Color_Il2CppStructArray_1_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ColorEx>.NativeClassPtr, 100663378);
			ColorEx.NativeMethodInfoPtr_ClosestColor_Public_Static_Color_Color_Il2CppStructArray_1_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ColorEx>.NativeClassPtr, 100663379);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005678 File Offset: 0x00003878
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499245, XrefRangeEnd = 1499248, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static float SqrEuclideanDistance(Color from, Color to)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref from;
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref to;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ColorEx.NativeMethodInfoPtr_SqrEuclideanDistance_Public_Static_Single_Color_Color_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000056C4 File Offset: 0x000038C4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499248, XrefRangeEnd = 1499253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static int ClosestColorIndex(Color c, Il2CppStructArray<Color> cs)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref c;
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(cs);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ColorEx.NativeMethodInfoPtr_ClosestColorIndex_Public_Static_Int32_Color_Il2CppStructArray_1_Color_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005714 File Offset: 0x00003914
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499253, XrefRangeEnd = 1499258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Color ClosestColor(Color c, Il2CppStructArray<Color> cs)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref c;
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(cs);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ColorEx.NativeMethodInfoPtr_ClosestColor_Public_Static_Color_Color_Il2CppStructArray_1_Color_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000244C File Offset: 0x0000064C
		public ColorEx(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000067 RID: 103
		private static readonly IntPtr NativeMethodInfoPtr_SqrEuclideanDistance_Public_Static_Single_Color_Color_0;

		// Token: 0x04000068 RID: 104
		private static readonly IntPtr NativeMethodInfoPtr_ClosestColorIndex_Public_Static_Int32_Color_Il2CppStructArray_1_Color_0;

		// Token: 0x04000069 RID: 105
		private static readonly IntPtr NativeMethodInfoPtr_ClosestColor_Public_Static_Color_Color_Il2CppStructArray_1_Color_0;
	}
}
