using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001A RID: 26
	public class DisableForPlatform : MonoBehaviour
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x000062A8 File Offset: 0x000044A8
		// Note: this type is marked as 'beforefieldinit'.
		static DisableForPlatform()
		{
			Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DisableForPlatform");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr);
			DisableForPlatform.NativeFieldInfoPtr__platform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, "_platform");
			DisableForPlatform.NativeFieldInfoPtr__event = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, "_event");
			DisableForPlatform.NativeFieldInfoPtr__target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, "_target");
			DisableForPlatform.NativeFieldInfoPtr__inverse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, "_inverse");
			DisableForPlatform.NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663413);
			DisableForPlatform.NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663414);
			DisableForPlatform.NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663415);
			DisableForPlatform.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663416);
			DisableForPlatform.NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663417);
			DisableForPlatform.NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663418);
			DisableForPlatform.NativeMethodInfoPtr_CheckPlatform_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663419);
			DisableForPlatform.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr, 100663420);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000063C8 File Offset: 0x000045C8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000063FC File Offset: 0x000045FC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499466, XrefRangeEnd = 1499467, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Awake()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006430 File Offset: 0x00004630
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499467, XrefRangeEnd = 1499468, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Start()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006464 File Offset: 0x00004664
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499468, XrefRangeEnd = 1499469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnEnable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006498 File Offset: 0x00004698
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499469, XrefRangeEnd = 1499470, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDisable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000064CC File Offset: 0x000046CC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499470, XrefRangeEnd = 1499471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006500 File Offset: 0x00004700
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 1499487, RefRangeEnd = 1499492, XrefRangeStart = 1499471, XrefRangeEnd = 1499487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CheckPlatform()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr_CheckPlatform_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006534 File Offset: 0x00004734
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DisableForPlatform()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DisableForPlatform>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableForPlatform.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000025BA File Offset: 0x000007BA
		public DisableForPlatform(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00006570 File Offset: 0x00004770
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x000025C3 File Offset: 0x000007C3
		public unsafe RuntimePlatform _platform
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__platform);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__platform)) = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00006598 File Offset: 0x00004798
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x000025DE File Offset: 0x000007DE
		public unsafe DisableForPlatform.Events _event
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__event);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__event)) = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000065C0 File Offset: 0x000047C0
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x000025F9 File Offset: 0x000007F9
		public unsafe GameObject _target
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__target);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<GameObject>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__target), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000065F0 File Offset: 0x000047F0
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00002618 File Offset: 0x00000818
		public unsafe bool _inverse
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__inverse);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableForPlatform.NativeFieldInfoPtr__inverse)) = value;
			}
		}

		// Token: 0x0400008E RID: 142
		private static readonly IntPtr NativeFieldInfoPtr__platform;

		// Token: 0x0400008F RID: 143
		private static readonly IntPtr NativeFieldInfoPtr__event;

		// Token: 0x04000090 RID: 144
		private static readonly IntPtr NativeFieldInfoPtr__target;

		// Token: 0x04000091 RID: 145
		private static readonly IntPtr NativeFieldInfoPtr__inverse;

		// Token: 0x04000092 RID: 146
		private static readonly IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

		// Token: 0x04000093 RID: 147
		private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

		// Token: 0x04000094 RID: 148
		private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

		// Token: 0x04000095 RID: 149
		private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

		// Token: 0x04000096 RID: 150
		private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

		// Token: 0x04000097 RID: 151
		private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

		// Token: 0x04000098 RID: 152
		private static readonly IntPtr NativeMethodInfoPtr_CheckPlatform_Private_Void_0;

		// Token: 0x04000099 RID: 153
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x02000041 RID: 65
		public enum Events
		{
			// Token: 0x0400016F RID: 367
			Awake,
			// Token: 0x04000170 RID: 368
			Start,
			// Token: 0x04000171 RID: 369
			OnEnable,
			// Token: 0x04000172 RID: 370
			OnDisable,
			// Token: 0x04000173 RID: 371
			OnDestroy
		}
	}
}
