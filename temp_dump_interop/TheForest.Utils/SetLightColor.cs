using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002A RID: 42
	public class SetLightColor : MonoBehaviour
	{
		// Token: 0x06000190 RID: 400 RVA: 0x000080B4 File Offset: 0x000062B4
		// Note: this type is marked as 'beforefieldinit'.
		static SetLightColor()
		{
			Il2CppClassPointerStore<SetLightColor>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "SetLightColor");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr);
			SetLightColor.NativeFieldInfoPtr__light = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr, "_light");
			SetLightColor.NativeFieldInfoPtr__color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr, "_color");
			SetLightColor.NativeMethodInfoPtr_ApplyColor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr, 100663468);
			SetLightColor.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr, 100663469);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008134 File Offset: 0x00006334
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500004, XrefRangeEnd = 1500006, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ApplyColor()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(SetLightColor.NativeMethodInfoPtr_ApplyColor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008168 File Offset: 0x00006368
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SetLightColor()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SetLightColor>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(SetLightColor.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00002B17 File Offset: 0x00000D17
		public SetLightColor(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000081A4 File Offset: 0x000063A4
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00002B20 File Offset: 0x00000D20
		public unsafe Light _light
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SetLightColor.NativeFieldInfoPtr__light);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Light>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(SetLightColor.NativeFieldInfoPtr__light), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000081D4 File Offset: 0x000063D4
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00002B3F File Offset: 0x00000D3F
		public unsafe Color _color
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SetLightColor.NativeFieldInfoPtr__color);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SetLightColor.NativeFieldInfoPtr__color)) = value;
			}
		}

		// Token: 0x040000ED RID: 237
		private static readonly IntPtr NativeFieldInfoPtr__light;

		// Token: 0x040000EE RID: 238
		private static readonly IntPtr NativeFieldInfoPtr__color;

		// Token: 0x040000EF RID: 239
		private static readonly IntPtr NativeMethodInfoPtr_ApplyColor_Public_Void_0;

		// Token: 0x040000F0 RID: 240
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
