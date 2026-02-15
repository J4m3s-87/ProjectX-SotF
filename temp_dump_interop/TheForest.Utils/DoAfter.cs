using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace TheForest.Utils
{
	// Token: 0x0200001B RID: 27
	public class DoAfter : MonoBehaviour
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00006618 File Offset: 0x00004818
		// Note: this type is marked as 'beforefieldinit'.
		static DoAfter()
		{
			Il2CppClassPointerStore<DoAfter>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DoAfter");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DoAfter>.NativeClassPtr);
			DoAfter.NativeFieldInfoPtr__delay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DoAfter>.NativeClassPtr, "_delay");
			DoAfter.NativeFieldInfoPtr__callback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DoAfter>.NativeClassPtr, "_callback");
			DoAfter.NativeMethodInfoPtr_BeginDelay_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DoAfter>.NativeClassPtr, 100663421);
			DoAfter.NativeMethodInfoPtr_Finished_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DoAfter>.NativeClassPtr, 100663422);
			DoAfter.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DoAfter>.NativeClassPtr, 100663423);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000066AC File Offset: 0x000048AC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499492, XrefRangeEnd = 1499495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void BeginDelay()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DoAfter.NativeMethodInfoPtr_BeginDelay_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000066E0 File Offset: 0x000048E0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Finished()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DoAfter.NativeMethodInfoPtr_Finished_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006714 File Offset: 0x00004914
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DoAfter()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DoAfter>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DoAfter.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00002633 File Offset: 0x00000833
		public DoAfter(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00006750 File Offset: 0x00004950
		// (set) Token: 0x060000FF RID: 255 RVA: 0x0000263C File Offset: 0x0000083C
		public unsafe float _delay
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DoAfter.NativeFieldInfoPtr__delay);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DoAfter.NativeFieldInfoPtr__delay)) = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006778 File Offset: 0x00004978
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00002657 File Offset: 0x00000857
		public unsafe UnityEvent _callback
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DoAfter.NativeFieldInfoPtr__callback);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DoAfter.NativeFieldInfoPtr__callback), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x0400009A RID: 154
		private static readonly IntPtr NativeFieldInfoPtr__delay;

		// Token: 0x0400009B RID: 155
		private static readonly IntPtr NativeFieldInfoPtr__callback;

		// Token: 0x0400009C RID: 156
		private static readonly IntPtr NativeMethodInfoPtr_BeginDelay_Public_Void_0;

		// Token: 0x0400009D RID: 157
		private static readonly IntPtr NativeMethodInfoPtr_Finished_Private_Void_0;

		// Token: 0x0400009E RID: 158
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
