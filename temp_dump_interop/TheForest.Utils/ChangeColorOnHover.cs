using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000012 RID: 18
	public class ChangeColorOnHover : MonoBehaviour
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00005268 File Offset: 0x00003468
		// Note: this type is marked as 'beforefieldinit'.
		static ChangeColorOnHover()
		{
			Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "ChangeColorOnHover");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr);
			ChangeColorOnHover.NativeFieldInfoPtr_rendererToUse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, "rendererToUse");
			ChangeColorOnHover.NativeFieldInfoPtr_defaultColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, "defaultColor");
			ChangeColorOnHover.NativeFieldInfoPtr_highlightColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, "highlightColor");
			ChangeColorOnHover.NativeFieldInfoPtr_highlighted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, "highlighted");
			ChangeColorOnHover.NativeFieldInfoPtr_MyMatPropertyBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, "MyMatPropertyBlock");
			ChangeColorOnHover.NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, 100663370);
			ChangeColorOnHover.NativeMethodInfoPtr_OnMouseExitCollider_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, 100663371);
			ChangeColorOnHover.NativeMethodInfoPtr_OnMouseOverCollider_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, 100663372);
			ChangeColorOnHover.NativeMethodInfoPtr_UpdateColor_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, 100663373);
			ChangeColorOnHover.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr, 100663374);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005360 File Offset: 0x00003560
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499210, XrefRangeEnd = 1499211, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDisable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ChangeColorOnHover.NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005394 File Offset: 0x00003594
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499211, XrefRangeEnd = 1499212, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnMouseExitCollider()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ChangeColorOnHover.NativeMethodInfoPtr_OnMouseExitCollider_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000053C8 File Offset: 0x000035C8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499212, XrefRangeEnd = 1499213, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnMouseOverCollider()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ChangeColorOnHover.NativeMethodInfoPtr_OnMouseOverCollider_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000053FC File Offset: 0x000035FC
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 1499233, RefRangeEnd = 1499236, XrefRangeStart = 1499213, XrefRangeEnd = 1499233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void UpdateColor()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ChangeColorOnHover.NativeMethodInfoPtr_UpdateColor_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005430 File Offset: 0x00003630
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ChangeColorOnHover()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ChangeColorOnHover>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(ChangeColorOnHover.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000023AB File Offset: 0x000005AB
		public ChangeColorOnHover(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000546C File Offset: 0x0000366C
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000023B4 File Offset: 0x000005B4
		public unsafe MeshRenderer rendererToUse
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_rendererToUse);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_rendererToUse), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000549C File Offset: 0x0000369C
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000023D3 File Offset: 0x000005D3
		public unsafe Color defaultColor
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_defaultColor);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_defaultColor)) = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000054C4 File Offset: 0x000036C4
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000023EE File Offset: 0x000005EE
		public unsafe Color highlightColor
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_highlightColor);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_highlightColor)) = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000054EC File Offset: 0x000036EC
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00002409 File Offset: 0x00000609
		public unsafe bool highlighted
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_highlighted);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_highlighted)) = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00005514 File Offset: 0x00003714
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00002424 File Offset: 0x00000624
		public unsafe MaterialPropertyBlock MyMatPropertyBlock
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_MyMatPropertyBlock);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<MaterialPropertyBlock>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(ChangeColorOnHover.NativeFieldInfoPtr_MyMatPropertyBlock), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x0400005B RID: 91
		private static readonly IntPtr NativeFieldInfoPtr_rendererToUse;

		// Token: 0x0400005C RID: 92
		private static readonly IntPtr NativeFieldInfoPtr_defaultColor;

		// Token: 0x0400005D RID: 93
		private static readonly IntPtr NativeFieldInfoPtr_highlightColor;

		// Token: 0x0400005E RID: 94
		private static readonly IntPtr NativeFieldInfoPtr_highlighted;

		// Token: 0x0400005F RID: 95
		private static readonly IntPtr NativeFieldInfoPtr_MyMatPropertyBlock;

		// Token: 0x04000060 RID: 96
		private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

		// Token: 0x04000061 RID: 97
		private static readonly IntPtr NativeMethodInfoPtr_OnMouseExitCollider_Private_Void_0;

		// Token: 0x04000062 RID: 98
		private static readonly IntPtr NativeMethodInfoPtr_OnMouseOverCollider_Private_Void_0;

		// Token: 0x04000063 RID: 99
		private static readonly IntPtr NativeMethodInfoPtr_UpdateColor_Private_Void_0;

		// Token: 0x04000064 RID: 100
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
