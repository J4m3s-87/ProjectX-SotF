using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001E RID: 30
	public class LerpPositionBasedOnRatio : MonoBehaviour
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00006C4C File Offset: 0x00004E4C
		// Note: this type is marked as 'beforefieldinit'.
		static LerpPositionBasedOnRatio()
		{
			Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "LerpPositionBasedOnRatio");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr);
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__from = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_from");
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__to = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_to");
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__fromAspectRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_fromAspectRatio");
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__toAspectRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_toAspectRatio");
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__localPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_localPosition");
			LerpPositionBasedOnRatio.NativeFieldInfoPtr__lastAspectRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, "_lastAspectRatio");
			LerpPositionBasedOnRatio.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, 100663434);
			LerpPositionBasedOnRatio.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, 100663435);
			LerpPositionBasedOnRatio.NativeMethodInfoPtr_Refresh_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, 100663436);
			LerpPositionBasedOnRatio.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr, 100663437);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006D44 File Offset: 0x00004F44
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499693, XrefRangeEnd = 1499699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Update()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LerpPositionBasedOnRatio.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006D78 File Offset: 0x00004F78
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499699, XrefRangeEnd = 1499700, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnEnable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LerpPositionBasedOnRatio.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006DAC File Offset: 0x00004FAC
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1499722, RefRangeEnd = 1499724, XrefRangeStart = 1499700, XrefRangeEnd = 1499722, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Refresh()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LerpPositionBasedOnRatio.NativeMethodInfoPtr_Refresh_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006DE0 File Offset: 0x00004FE0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499724, XrefRangeEnd = 1499727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LerpPositionBasedOnRatio()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LerpPositionBasedOnRatio>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LerpPositionBasedOnRatio.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000269A File Offset: 0x0000089A
		public LerpPositionBasedOnRatio(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006E1C File Offset: 0x0000501C
		// (set) Token: 0x06000118 RID: 280 RVA: 0x000026A3 File Offset: 0x000008A3
		public unsafe Transform _from
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__from);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Transform>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__from), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006E4C File Offset: 0x0000504C
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000026C2 File Offset: 0x000008C2
		public unsafe Transform _to
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__to);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Transform>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__to), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006E7C File Offset: 0x0000507C
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000026E1 File Offset: 0x000008E1
		public unsafe float _fromAspectRatio
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__fromAspectRatio);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__fromAspectRatio)) = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00006EA4 File Offset: 0x000050A4
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000026FC File Offset: 0x000008FC
		public unsafe float _toAspectRatio
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__toAspectRatio);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__toAspectRatio)) = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006ECC File Offset: 0x000050CC
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00002717 File Offset: 0x00000917
		public unsafe bool _localPosition
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__localPosition);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__localPosition)) = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006EF4 File Offset: 0x000050F4
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00002732 File Offset: 0x00000932
		public unsafe float _lastAspectRatio
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__lastAspectRatio);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LerpPositionBasedOnRatio.NativeFieldInfoPtr__lastAspectRatio)) = value;
			}
		}

		// Token: 0x040000A9 RID: 169
		private static readonly IntPtr NativeFieldInfoPtr__from;

		// Token: 0x040000AA RID: 170
		private static readonly IntPtr NativeFieldInfoPtr__to;

		// Token: 0x040000AB RID: 171
		private static readonly IntPtr NativeFieldInfoPtr__fromAspectRatio;

		// Token: 0x040000AC RID: 172
		private static readonly IntPtr NativeFieldInfoPtr__toAspectRatio;

		// Token: 0x040000AD RID: 173
		private static readonly IntPtr NativeFieldInfoPtr__localPosition;

		// Token: 0x040000AE RID: 174
		private static readonly IntPtr NativeFieldInfoPtr__lastAspectRatio;

		// Token: 0x040000AF RID: 175
		private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

		// Token: 0x040000B0 RID: 176
		private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

		// Token: 0x040000B1 RID: 177
		private static readonly IntPtr NativeMethodInfoPtr_Refresh_Private_Void_0;

		// Token: 0x040000B2 RID: 178
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
