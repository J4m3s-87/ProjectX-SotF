using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000022 RID: 34
	public class MaterialPropertyColors : MonoBehaviour
	{
		// Token: 0x06000148 RID: 328 RVA: 0x000074F8 File Offset: 0x000056F8
		// Note: this type is marked as 'beforefieldinit'.
		static MaterialPropertyColors()
		{
			Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "MaterialPropertyColors");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr);
			MaterialPropertyColors.NativeFieldInfoPtr__target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr, "_target");
			MaterialPropertyColors.NativeFieldInfoPtr__colors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr, "_colors");
			MaterialPropertyColors.NativeMethodInfoPtr_SetColor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr, 100663447);
			MaterialPropertyColors.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr, 100663448);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007578 File Offset: 0x00005778
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499845, XrefRangeEnd = 1499856, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetColor(int colorNum)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref colorNum;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MaterialPropertyColors.NativeMethodInfoPtr_SetColor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000075B8 File Offset: 0x000057B8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialPropertyColors()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialPropertyColors>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MaterialPropertyColors.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000289D File Offset: 0x00000A9D
		public MaterialPropertyColors(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000075F4 File Offset: 0x000057F4
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000028A6 File Offset: 0x00000AA6
		public unsafe SetMaterialProperty _target
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MaterialPropertyColors.NativeFieldInfoPtr__target);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<SetMaterialProperty>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(MaterialPropertyColors.NativeFieldInfoPtr__target), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007624 File Offset: 0x00005824
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000028C5 File Offset: 0x00000AC5
		public unsafe Il2CppStructArray<Color> _colors
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MaterialPropertyColors.NativeFieldInfoPtr__colors);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Il2CppStructArray<Color>>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(MaterialPropertyColors.NativeFieldInfoPtr__colors), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000C7 RID: 199
		private static readonly IntPtr NativeFieldInfoPtr__target;

		// Token: 0x040000C8 RID: 200
		private static readonly IntPtr NativeFieldInfoPtr__colors;

		// Token: 0x040000C9 RID: 201
		private static readonly IntPtr NativeMethodInfoPtr_SetColor_Public_Void_Int32_0;

		// Token: 0x040000CA RID: 202
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
