using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001F RID: 31
	public class LimitUpwardsVelocity : MonoBehaviour
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006F1C File Offset: 0x0000511C
		// Note: this type is marked as 'beforefieldinit'.
		static LimitUpwardsVelocity()
		{
			Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "LimitUpwardsVelocity");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr);
			LimitUpwardsVelocity.NativeFieldInfoPtr__maxVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, "_maxVelocity");
			LimitUpwardsVelocity.NativeFieldInfoPtr__maxAngularVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, "_maxAngularVelocity");
			LimitUpwardsVelocity.NativeFieldInfoPtr__rb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, "_rb");
			LimitUpwardsVelocity.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, 100663438);
			LimitUpwardsVelocity.NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, 100663439);
			LimitUpwardsVelocity.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr, 100663440);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006FC4 File Offset: 0x000051C4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499727, XrefRangeEnd = 1499744, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitUpwardsVelocity.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006FF8 File Offset: 0x000051F8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499744, XrefRangeEnd = 1499759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void FixedUpdate()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitUpwardsVelocity.NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000702C File Offset: 0x0000522C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499759, XrefRangeEnd = 1499762, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LimitUpwardsVelocity()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LimitUpwardsVelocity>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitUpwardsVelocity.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000274D File Offset: 0x0000094D
		public LimitUpwardsVelocity(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007068 File Offset: 0x00005268
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00002756 File Offset: 0x00000956
		public unsafe float _maxVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__maxVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__maxVelocity)) = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007090 File Offset: 0x00005290
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00002771 File Offset: 0x00000971
		public unsafe float _maxAngularVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__maxAngularVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__maxAngularVelocity)) = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000070B8 File Offset: 0x000052B8
		// (set) Token: 0x0600012D RID: 301 RVA: 0x0000278C File Offset: 0x0000098C
		public unsafe Rigidbody _rb
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__rb);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitUpwardsVelocity.NativeFieldInfoPtr__rb), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000B3 RID: 179
		private static readonly IntPtr NativeFieldInfoPtr__maxVelocity;

		// Token: 0x040000B4 RID: 180
		private static readonly IntPtr NativeFieldInfoPtr__maxAngularVelocity;

		// Token: 0x040000B5 RID: 181
		private static readonly IntPtr NativeFieldInfoPtr__rb;

		// Token: 0x040000B6 RID: 182
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x040000B7 RID: 183
		private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

		// Token: 0x040000B8 RID: 184
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
