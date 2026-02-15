using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000019 RID: 25
	public class DisableAfterDelay : MonoBehaviour
	{
		// Token: 0x060000DB RID: 219 RVA: 0x00006028 File Offset: 0x00004228
		// Note: this type is marked as 'beforefieldinit'.
		static DisableAfterDelay()
		{
			Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DisableAfterDelay");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr);
			DisableAfterDelay.NativeFieldInfoPtr__delay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, "_delay");
			DisableAfterDelay.NativeFieldInfoPtr__activeCoroutine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, "_activeCoroutine");
			DisableAfterDelay.NativeMethodInfoPtr_Restart_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663401);
			DisableAfterDelay.NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663402);
			DisableAfterDelay.NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663403);
			DisableAfterDelay.NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663404);
			DisableAfterDelay.NativeMethodInfoPtr_Disable_Private_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663405);
			DisableAfterDelay.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, 100663406);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000060F8 File Offset: 0x000042F8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499445, XrefRangeEnd = 1499446, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Restart(float duration)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref duration;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr_Restart_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006138 File Offset: 0x00004338
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1499451, RefRangeEnd = 1499453, XrefRangeStart = 1499446, XrefRangeEnd = 1499451, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Start()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000616C File Offset: 0x0000436C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499453, XrefRangeEnd = 1499454, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnEnable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000061A0 File Offset: 0x000043A0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499454, XrefRangeEnd = 1499463, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDisable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000061D4 File Offset: 0x000043D4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499463, XrefRangeEnd = 1499466, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IEnumerator Disable()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr_Disable_Private_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			IntPtr intPtr3 = intPtr;
			return (intPtr3 != 0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr3) : null;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006214 File Offset: 0x00004414
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DisableAfterDelay()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002577 File Offset: 0x00000777
		public DisableAfterDelay(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006250 File Offset: 0x00004450
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00002580 File Offset: 0x00000780
		public unsafe float _delay
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay.NativeFieldInfoPtr__delay);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay.NativeFieldInfoPtr__delay)) = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006278 File Offset: 0x00004478
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000259B File Offset: 0x0000079B
		public unsafe Coroutine _activeCoroutine
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay.NativeFieldInfoPtr__activeCoroutine);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Coroutine>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay.NativeFieldInfoPtr__activeCoroutine), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x04000086 RID: 134
		private static readonly IntPtr NativeFieldInfoPtr__delay;

		// Token: 0x04000087 RID: 135
		private static readonly IntPtr NativeFieldInfoPtr__activeCoroutine;

		// Token: 0x04000088 RID: 136
		private static readonly IntPtr NativeMethodInfoPtr_Restart_Public_Void_Single_0;

		// Token: 0x04000089 RID: 137
		private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

		// Token: 0x0400008A RID: 138
		private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

		// Token: 0x0400008B RID: 139
		private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

		// Token: 0x0400008C RID: 140
		private static readonly IntPtr NativeMethodInfoPtr_Disable_Private_IEnumerator_0;

		// Token: 0x0400008D RID: 141
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x02000040 RID: 64
		[ObfuscatedName("TheForest.Utils.DisableAfterDelay+<Disable>d__6")]
		public sealed class _Disable_d__6 : global::Il2CppSystem.Object
		{
			// Token: 0x06000253 RID: 595 RVA: 0x0000A2EC File Offset: 0x000084EC
			// Note: this type is marked as 'beforefieldinit'.
			static _Disable_d__6()
			{
				Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DisableAfterDelay>.NativeClassPtr, "<Disable>d__6");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr);
				DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, "<>1__state");
				DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, "<>2__current");
				DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, "<>4__this");
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663407);
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663408);
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663409);
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663410);
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663411);
				DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr, 100663412);
			}

			// Token: 0x06000254 RID: 596 RVA: 0x0000A3CC File Offset: 0x000085CC
			[CallerCount(0)]
			public unsafe _Disable_d__6(int <>1__state)
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DisableAfterDelay._Disable_d__6>.NativeClassPtr))
			{
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = ref <>1__state;
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				}
			}

			// Token: 0x06000255 RID: 597 RVA: 0x0000A414 File Offset: 0x00008614
			[CallerCount(250)]
			[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe void System_IDisposable_Dispose()
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			// Token: 0x06000256 RID: 598 RVA: 0x0000A448 File Offset: 0x00008648
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499437, XrefRangeEnd = 1499440, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe bool MoveNext()
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000257 RID: 599 RVA: 0x0000A484 File Offset: 0x00008684
			public unsafe global::Il2CppSystem.Object System.Collections.Generic.IEnumerator<System.Object>.Current
			{
				[CallerCount(5)]
				[CachedScanResults(RefRangeStart = 9147, RefRangeEnd = 9152, XrefRangeStart = 9147, XrefRangeEnd = 9152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
				get
				{
					IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IntPtr* ptr = null;
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
					IntPtr intPtr3 = intPtr;
					return (intPtr3 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr3) : null;
				}
			}

			// Token: 0x06000258 RID: 600 RVA: 0x0000A4C4 File Offset: 0x000086C4
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499440, XrefRangeEnd = 1499445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe void System_Collections_IEnumerator_Reset()
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x06000259 RID: 601 RVA: 0x0000A4F8 File Offset: 0x000086F8
			public unsafe global::Il2CppSystem.Object System.Collections.IEnumerator.Current
			{
				[CallerCount(5)]
				[CachedScanResults(RefRangeStart = 9147, RefRangeEnd = 9152, XrefRangeStart = 9147, XrefRangeEnd = 9152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
				get
				{
					IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IntPtr* ptr = null;
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DisableAfterDelay._Disable_d__6.NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
					IntPtr intPtr3 = intPtr;
					return (intPtr3 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr3) : null;
				}
			}

			// Token: 0x0600025A RID: 602 RVA: 0x000031C7 File Offset: 0x000013C7
			public _Disable_d__6(IntPtr pointer)
				: base(pointer)
			{
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x0600025B RID: 603 RVA: 0x0000A538 File Offset: 0x00008738
			// (set) Token: 0x0600025C RID: 604 RVA: 0x000031D0 File Offset: 0x000013D0
			public unsafe int __1__state
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___1__state);
					return *intPtr;
				}
				set
				{
					*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___1__state)) = value;
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x0600025D RID: 605 RVA: 0x0000A560 File Offset: 0x00008760
			// (set) Token: 0x0600025E RID: 606 RVA: 0x000031EB File Offset: 0x000013EB
			public unsafe global::Il2CppSystem.Object __2__current
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___2__current);
					IntPtr intPtr2 = *intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr2) : null;
				}
				set
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___2__current), IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x0600025F RID: 607 RVA: 0x0000A590 File Offset: 0x00008790
			// (set) Token: 0x06000260 RID: 608 RVA: 0x0000320A File Offset: 0x0000140A
			public unsafe DisableAfterDelay __4__this
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___4__this);
					IntPtr intPtr2 = *intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<DisableAfterDelay>(intPtr2) : null;
				}
				set
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DisableAfterDelay._Disable_d__6.NativeFieldInfoPtr___4__this), IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x04000165 RID: 357
			private static readonly IntPtr NativeFieldInfoPtr___1__state;

			// Token: 0x04000166 RID: 358
			private static readonly IntPtr NativeFieldInfoPtr___2__current;

			// Token: 0x04000167 RID: 359
			private static readonly IntPtr NativeFieldInfoPtr___4__this;

			// Token: 0x04000168 RID: 360
			private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

			// Token: 0x04000169 RID: 361
			private static readonly IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

			// Token: 0x0400016A RID: 362
			private static readonly IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

			// Token: 0x0400016B RID: 363
			private static readonly IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

			// Token: 0x0400016C RID: 364
			private static readonly IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

			// Token: 0x0400016D RID: 365
			private static readonly IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;
		}
	}
}
