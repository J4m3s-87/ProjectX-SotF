using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000021 RID: 33
	public class LimitVelocityBurst : MonoBehaviour
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000072B4 File Offset: 0x000054B4
		// Note: this type is marked as 'beforefieldinit'.
		static LimitVelocityBurst()
		{
			Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "LimitVelocityBurst");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr);
			LimitVelocityBurst.NativeFieldInfoPtr__maxVelocityDelta = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, "_maxVelocityDelta");
			LimitVelocityBurst.NativeFieldInfoPtr__maxAngularVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, "_maxAngularVelocity");
			LimitVelocityBurst.NativeFieldInfoPtr__prevVelocityMag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, "_prevVelocityMag");
			LimitVelocityBurst.NativeFieldInfoPtr__prevVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, "_prevVelocity");
			LimitVelocityBurst.NativeFieldInfoPtr__rb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, "_rb");
			LimitVelocityBurst.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, 100663444);
			LimitVelocityBurst.NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, 100663445);
			LimitVelocityBurst.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr, 100663446);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007384 File Offset: 0x00005584
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499798, XrefRangeEnd = 1499815, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocityBurst.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000073B8 File Offset: 0x000055B8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499815, XrefRangeEnd = 1499842, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void FixedUpdate()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocityBurst.NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000073EC File Offset: 0x000055EC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499842, XrefRangeEnd = 1499845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LimitVelocityBurst()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LimitVelocityBurst>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LimitVelocityBurst.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00002809 File Offset: 0x00000A09
		public LimitVelocityBurst(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00007428 File Offset: 0x00005628
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00002812 File Offset: 0x00000A12
		public unsafe float _maxVelocityDelta
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__maxVelocityDelta);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__maxVelocityDelta)) = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00007450 File Offset: 0x00005650
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000282D File Offset: 0x00000A2D
		public unsafe float _maxAngularVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__maxAngularVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__maxAngularVelocity)) = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00007478 File Offset: 0x00005678
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00002848 File Offset: 0x00000A48
		public unsafe float _prevVelocityMag
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__prevVelocityMag);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__prevVelocityMag)) = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000074A0 File Offset: 0x000056A0
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00002863 File Offset: 0x00000A63
		public unsafe Vector3 _prevVelocity
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__prevVelocity);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__prevVelocity)) = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000074C8 File Offset: 0x000056C8
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000287E File Offset: 0x00000A7E
		public unsafe Rigidbody _rb
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__rb);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LimitVelocityBurst.NativeFieldInfoPtr__rb), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000BF RID: 191
		private static readonly IntPtr NativeFieldInfoPtr__maxVelocityDelta;

		// Token: 0x040000C0 RID: 192
		private static readonly IntPtr NativeFieldInfoPtr__maxAngularVelocity;

		// Token: 0x040000C1 RID: 193
		private static readonly IntPtr NativeFieldInfoPtr__prevVelocityMag;

		// Token: 0x040000C2 RID: 194
		private static readonly IntPtr NativeFieldInfoPtr__prevVelocity;

		// Token: 0x040000C3 RID: 195
		private static readonly IntPtr NativeFieldInfoPtr__rb;

		// Token: 0x040000C4 RID: 196
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x040000C5 RID: 197
		private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

		// Token: 0x040000C6 RID: 198
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
