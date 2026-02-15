using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000020 RID: 32
	public class LimitVelocity : MonoBehaviour
	{
		// Token: 0x0600012E RID: 302 RVA: 0x000070E8 File Offset: 0x000052E8
		// Note: this type is marked as 'beforefieldinit'.
		static LimitVelocity()
		{
			Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "LimitVelocity");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr);
			LimitVelocity.NativeFieldInfoPtr__maxVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, "_maxVelocity");
			LimitVelocity.NativeFieldInfoPtr__maxAngularVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, "_maxAngularVelocity");
			LimitVelocity.NativeFieldInfoPtr__rb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, "_rb");
			LimitVelocity.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, 100663441);
			LimitVelocity.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, 100663442);
			LimitVelocity.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr, 100663443);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007190 File Offset: 0x00005390
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499762, XrefRangeEnd = 1499779, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocity.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000071C4 File Offset: 0x000053C4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499779, XrefRangeEnd = 1499798, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Update()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocity.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000071F8 File Offset: 0x000053F8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LimitVelocity()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LimitVelocity>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocity.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000027AB File Offset: 0x000009AB
		public LimitVelocity(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00007234 File Offset: 0x00005434
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000027B4 File Offset: 0x000009B4
		public unsafe float _maxVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__maxVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__maxVelocity)) = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000725C File Offset: 0x0000545C
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000027CF File Offset: 0x000009CF
		public unsafe float _maxAngularVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__maxAngularVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__maxAngularVelocity)) = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007284 File Offset: 0x00005484
		// (set) Token: 0x06000138 RID: 312 RVA: 0x000027EA File Offset: 0x000009EA
		public unsafe Rigidbody _rb
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__rb);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocity.NativeFieldInfoPtr__rb), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000B9 RID: 185
		private static readonly IntPtr NativeFieldInfoPtr__maxVelocity;

		// Token: 0x040000BA RID: 186
		private static readonly IntPtr NativeFieldInfoPtr__maxAngularVelocity;

		// Token: 0x040000BB RID: 187
		private static readonly IntPtr NativeFieldInfoPtr__rb;

		// Token: 0x040000BC RID: 188
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x040000BD RID: 189
		private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

		// Token: 0x040000BE RID: 190
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
