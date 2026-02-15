using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000025 RID: 37
	public class PingPongRotation : MonoBehaviour
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00007784 File Offset: 0x00005984
		// Note: this type is marked as 'beforefieldinit'.
		static PingPongRotation()
		{
			Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "PingPongRotation");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr);
			PingPongRotation.NativeFieldInfoPtr__duration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_duration");
			PingPongRotation.NativeFieldInfoPtr__fromRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_fromRotation");
			PingPongRotation.NativeFieldInfoPtr__toRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_toRotation");
			PingPongRotation.NativeFieldInfoPtr__resetOnEnable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_resetOnEnable");
			PingPongRotation.NativeFieldInfoPtr__space = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_space");
			PingPongRotation.NativeFieldInfoPtr__alpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, "_alpha");
			PingPongRotation.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, 100663452);
			PingPongRotation.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, 100663453);
			PingPongRotation.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr, 100663454);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007868 File Offset: 0x00005A68
		[CallerCount(0)]
		public unsafe void OnEnable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PingPongRotation.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000789C File Offset: 0x00005A9C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499879, XrefRangeEnd = 1499886, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Update()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PingPongRotation.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000078D0 File Offset: 0x00005AD0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499886, XrefRangeEnd = 1499889, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe PingPongRotation()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PingPongRotation>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PingPongRotation.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000292F File Offset: 0x00000B2F
		public PingPongRotation(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000790C File Offset: 0x00005B0C
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00002938 File Offset: 0x00000B38
		public unsafe float _duration
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__duration);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__duration)) = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007934 File Offset: 0x00005B34
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00002953 File Offset: 0x00000B53
		public unsafe Vector3 _fromRotation
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__fromRotation);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__fromRotation)) = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000795C File Offset: 0x00005B5C
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000296E File Offset: 0x00000B6E
		public unsafe Vector3 _toRotation
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__toRotation);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__toRotation)) = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007984 File Offset: 0x00005B84
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00002989 File Offset: 0x00000B89
		public unsafe bool _resetOnEnable
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__resetOnEnable);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__resetOnEnable)) = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000079AC File Offset: 0x00005BAC
		// (set) Token: 0x06000165 RID: 357 RVA: 0x000029A4 File Offset: 0x00000BA4
		public unsafe Space _space
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__space);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__space)) = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000079D4 File Offset: 0x00005BD4
		// (set) Token: 0x06000167 RID: 359 RVA: 0x000029BF File Offset: 0x00000BBF
		public unsafe float _alpha
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__alpha);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PingPongRotation.NativeFieldInfoPtr__alpha)) = value;
			}
		}

		// Token: 0x040000CE RID: 206
		private static readonly IntPtr NativeFieldInfoPtr__duration;

		// Token: 0x040000CF RID: 207
		private static readonly IntPtr NativeFieldInfoPtr__fromRotation;

		// Token: 0x040000D0 RID: 208
		private static readonly IntPtr NativeFieldInfoPtr__toRotation;

		// Token: 0x040000D1 RID: 209
		private static readonly IntPtr NativeFieldInfoPtr__resetOnEnable;

		// Token: 0x040000D2 RID: 210
		private static readonly IntPtr NativeFieldInfoPtr__space;

		// Token: 0x040000D3 RID: 211
		private static readonly IntPtr NativeFieldInfoPtr__alpha;

		// Token: 0x040000D4 RID: 212
		private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

		// Token: 0x040000D5 RID: 213
		private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

		// Token: 0x040000D6 RID: 214
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
