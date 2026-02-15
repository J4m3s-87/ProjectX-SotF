using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace TheForest.Utils
{
	// Token: 0x02000030 RID: 48
	public class VideoStreaming : MonoBehaviour
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00008E80 File Offset: 0x00007080
		// Note: this type is marked as 'beforefieldinit'.
		static VideoStreaming()
		{
			Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "VideoStreaming");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr);
			VideoStreaming.NativeFieldInfoPtr__targetMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_targetMaterial");
			VideoStreaming.NativeFieldInfoPtr__defaultTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_defaultTexture");
			VideoStreaming.NativeFieldInfoPtr__onBeginRead = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_onBeginRead");
			VideoStreaming.NativeFieldInfoPtr__onPlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_onPlay");
			VideoStreaming.NativeFieldInfoPtr__onStop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_onStop");
			VideoStreaming.NativeFieldInfoPtr__onClear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_onClear");
			VideoStreaming.NativeFieldInfoPtr__lastPlayed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_lastPlayed");
			VideoStreaming.NativeFieldInfoPtr__streamingFromResources = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, "_streamingFromResources");
			VideoStreaming.NativeMethodInfoPtr_Play_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, 100663502);
			VideoStreaming.NativeMethodInfoPtr_Stop_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, 100663503);
			VideoStreaming.NativeMethodInfoPtr_get_IsPlaying_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, 100663504);
			VideoStreaming.NativeMethodInfoPtr_get_IsReading_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, 100663505);
			VideoStreaming.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr, 100663506);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008FB4 File Offset: 0x000071B4
		[CallerCount(250)]
		[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Play(string path)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(path);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(VideoStreaming.NativeMethodInfoPtr_Play_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008FF8 File Offset: 0x000071F8
		[CallerCount(250)]
		[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Stop()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(VideoStreaming.NativeMethodInfoPtr_Stop_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000902C File Offset: 0x0000722C
		public unsafe bool IsPlaying
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(VideoStreaming.NativeMethodInfoPtr_get_IsPlaying_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00009068 File Offset: 0x00007268
		public unsafe bool IsReading
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(VideoStreaming.NativeMethodInfoPtr_get_IsReading_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000090A4 File Offset: 0x000072A4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe VideoStreaming()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VideoStreaming>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(VideoStreaming.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00002CBA File Offset: 0x00000EBA
		public VideoStreaming(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000090E0 File Offset: 0x000072E0
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00002CC3 File Offset: 0x00000EC3
		public unsafe Material _targetMaterial
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__targetMaterial);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__targetMaterial), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00009110 File Offset: 0x00007310
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00002CE2 File Offset: 0x00000EE2
		public unsafe Texture _defaultTexture
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__defaultTexture);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Texture>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__defaultTexture), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00009140 File Offset: 0x00007340
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00002D01 File Offset: 0x00000F01
		public unsafe UnityEvent _onBeginRead
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onBeginRead);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onBeginRead), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00009170 File Offset: 0x00007370
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00002D20 File Offset: 0x00000F20
		public unsafe UnityEvent _onPlay
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onPlay);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onPlay), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000091A0 File Offset: 0x000073A0
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00002D3F File Offset: 0x00000F3F
		public unsafe UnityEvent _onStop
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onStop);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onStop), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000091D0 File Offset: 0x000073D0
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00002D5E File Offset: 0x00000F5E
		public unsafe UnityEvent _onClear
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onClear);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__onClear), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00009200 File Offset: 0x00007400
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00002D7D File Offset: 0x00000F7D
		public unsafe string _lastPlayed
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__lastPlayed);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__lastPlayed), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009228 File Offset: 0x00007428
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00002D9C File Offset: 0x00000F9C
		public unsafe bool _streamingFromResources
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__streamingFromResources);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(VideoStreaming.NativeFieldInfoPtr__streamingFromResources)) = value;
			}
		}

		// Token: 0x04000115 RID: 277
		private static readonly IntPtr NativeFieldInfoPtr__targetMaterial;

		// Token: 0x04000116 RID: 278
		private static readonly IntPtr NativeFieldInfoPtr__defaultTexture;

		// Token: 0x04000117 RID: 279
		private static readonly IntPtr NativeFieldInfoPtr__onBeginRead;

		// Token: 0x04000118 RID: 280
		private static readonly IntPtr NativeFieldInfoPtr__onPlay;

		// Token: 0x04000119 RID: 281
		private static readonly IntPtr NativeFieldInfoPtr__onStop;

		// Token: 0x0400011A RID: 282
		private static readonly IntPtr NativeFieldInfoPtr__onClear;

		// Token: 0x0400011B RID: 283
		private static readonly IntPtr NativeFieldInfoPtr__lastPlayed;

		// Token: 0x0400011C RID: 284
		private static readonly IntPtr NativeFieldInfoPtr__streamingFromResources;

		// Token: 0x0400011D RID: 285
		private static readonly IntPtr NativeMethodInfoPtr_Play_Public_Void_String_0;

		// Token: 0x0400011E RID: 286
		private static readonly IntPtr NativeMethodInfoPtr_Stop_Public_Void_0;

		// Token: 0x0400011F RID: 287
		private static readonly IntPtr NativeMethodInfoPtr_get_IsPlaying_Public_get_Boolean_0;

		// Token: 0x04000120 RID: 288
		private static readonly IntPtr NativeMethodInfoPtr_get_IsReading_Public_get_Boolean_0;

		// Token: 0x04000121 RID: 289
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
