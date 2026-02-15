using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class SkewTransform : global::Il2CppSystem.Object
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x00008530 File Offset: 0x00006730
		// Note: this type is marked as 'beforefieldinit'.
		static SkewTransform()
		{
			Il2CppClassPointerStore<SkewTransform>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "SkewTransform");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr);
			SkewTransform.NativeFieldInfoPtr__skewerScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, "_skewerScale");
			SkewTransform.NativeFieldInfoPtr__skeweeScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, "_skeweeScaleY");
			SkewTransform.NativeFieldInfoPtr__skeweeScaleZ = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, "_skeweeScaleZ");
			SkewTransform.NativeFieldInfoPtr__skeweeRotX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, "_skeweeRotX");
			SkewTransform.NativeMethodInfoPtr_SetSkew_Public_Void_Transform_Transform_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, 100663477);
			SkewTransform.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr, 100663478);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000085D8 File Offset: 0x000067D8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500061, XrefRangeEnd = 1500072, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetSkew(Transform skewer, Transform skewee, float angle)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(skewer);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(skewee);
			ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref angle;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(SkewTransform.NativeMethodInfoPtr_SetSkew_Public_Void_Transform_Transform_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000863C File Offset: 0x0000683C
		[CallerCount(0)]
		public unsafe SkewTransform()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkewTransform>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(SkewTransform.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public SkewTransform(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00008678 File Offset: 0x00006878
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00002BC9 File Offset: 0x00000DC9
		public unsafe Vector3 _skewerScale
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skewerScale);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skewerScale)) = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000086A0 File Offset: 0x000068A0
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public unsafe AnimationCurve _skeweeScaleY
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeScaleY);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeScaleY), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000086D0 File Offset: 0x000068D0
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00002C03 File Offset: 0x00000E03
		public unsafe AnimationCurve _skeweeScaleZ
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeScaleZ);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeScaleZ), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00008700 File Offset: 0x00006900
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00002C22 File Offset: 0x00000E22
		public unsafe AnimationCurve _skeweeRotX
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeRotX);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(SkewTransform.NativeFieldInfoPtr__skeweeRotX), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000FB RID: 251
		private static readonly IntPtr NativeFieldInfoPtr__skewerScale;

		// Token: 0x040000FC RID: 252
		private static readonly IntPtr NativeFieldInfoPtr__skeweeScaleY;

		// Token: 0x040000FD RID: 253
		private static readonly IntPtr NativeFieldInfoPtr__skeweeScaleZ;

		// Token: 0x040000FE RID: 254
		private static readonly IntPtr NativeFieldInfoPtr__skeweeRotX;

		// Token: 0x040000FF RID: 255
		private static readonly IntPtr NativeMethodInfoPtr_SetSkew_Public_Void_Transform_Transform_Single_0;

		// Token: 0x04000100 RID: 256
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
