using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class LoadFromResourcesAsync : MonoBehaviour
{
	// Token: 0x06000044 RID: 68 RVA: 0x00004254 File Offset: 0x00002454
	// Note: this type is marked as 'beforefieldinit'.
	static LoadFromResourcesAsync()
	{
		Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "LoadFromResourcesAsync");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr);
		LoadFromResourcesAsync.NativeFieldInfoPtr_path = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr, "path");
		LoadFromResourcesAsync.NativeFieldInfoPtr_delay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr, "delay");
		LoadFromResourcesAsync.NativeMethodInfoPtr_Start_Private_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr, 100663333);
		LoadFromResourcesAsync.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr, 100663334);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000042D4 File Offset: 0x000024D4
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498636, XrefRangeEnd = 1498639, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IEnumerator Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync.NativeMethodInfoPtr_Start_Private_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		IntPtr intPtr3 = intPtr;
		return (intPtr3 != 0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr3) : null;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00004314 File Offset: 0x00002514
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LoadFromResourcesAsync()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000021A9 File Offset: 0x000003A9
	public LoadFromResourcesAsync(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00004350 File Offset: 0x00002550
	// (set) Token: 0x06000049 RID: 73 RVA: 0x000021B2 File Offset: 0x000003B2
	public unsafe string path
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync.NativeFieldInfoPtr_path);
			return IL2CPP.Il2CppStringToManaged(*intPtr);
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync.NativeFieldInfoPtr_path), IL2CPP.ManagedStringToIl2Cpp(value));
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600004A RID: 74 RVA: 0x00004378 File Offset: 0x00002578
	// (set) Token: 0x0600004B RID: 75 RVA: 0x000021D1 File Offset: 0x000003D1
	public unsafe float delay
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync.NativeFieldInfoPtr_delay);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync.NativeFieldInfoPtr_delay)) = value;
		}
	}

	// Token: 0x0400002E RID: 46
	private static readonly IntPtr NativeFieldInfoPtr_path;

	// Token: 0x0400002F RID: 47
	private static readonly IntPtr NativeFieldInfoPtr_delay;

	// Token: 0x04000030 RID: 48
	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_IEnumerator_0;

	// Token: 0x04000031 RID: 49
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	// Token: 0x02000039 RID: 57
	[ObfuscatedName("LoadFromResourcesAsync+<Start>d__2")]
	public sealed class _Start_d__2 : global::Il2CppSystem.Object
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00009AD8 File Offset: 0x00007CD8
		// Note: this type is marked as 'beforefieldinit'.
		static _Start_d__2()
		{
			Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LoadFromResourcesAsync>.NativeClassPtr, "<Start>d__2");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr);
			LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, "<>1__state");
			LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, "<>2__current");
			LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, "<>4__this");
			LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr__resourceRequest_5__2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, "<resourceRequest>5__2");
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663335);
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663336);
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663337);
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663338);
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663339);
			LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr, 100663340);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009BCC File Offset: 0x00007DCC
		[CallerCount(0)]
		public unsafe _Start_d__2(int <>1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LoadFromResourcesAsync._Start_d__2>.NativeClassPtr))
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref <>1__state;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009C14 File Offset: 0x00007E14
		[CallerCount(250)]
		[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009C48 File Offset: 0x00007E48
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498621, XrefRangeEnd = 1498631, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *IL2CPP.il2cpp_object_unbox(intPtr);
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00009C84 File Offset: 0x00007E84
		public unsafe global::Il2CppSystem.Object System.Collections.Generic.IEnumerator<System.Object>.Current
		{
			[CallerCount(5)]
			[CachedScanResults(RefRangeStart = 9147, RefRangeEnd = 9152, XrefRangeStart = 9147, XrefRangeEnd = 9152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr3) : null;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009CC4 File Offset: 0x00007EC4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498631, XrefRangeEnd = 1498636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00009CF8 File Offset: 0x00007EF8
		public unsafe global::Il2CppSystem.Object System.Collections.IEnumerator.Current
		{
			[CallerCount(5)]
			[CachedScanResults(RefRangeStart = 9147, RefRangeEnd = 9152, XrefRangeStart = 9147, XrefRangeEnd = 9152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(LoadFromResourcesAsync._Start_d__2.NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr3) : null;
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00002F4D File Offset: 0x0000114D
		public _Start_d__2(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00009D38 File Offset: 0x00007F38
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00002F56 File Offset: 0x00001156
		public unsafe int __1__state
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___1__state);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___1__state)) = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009D60 File Offset: 0x00007F60
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00002F71 File Offset: 0x00001171
		public unsafe global::Il2CppSystem.Object __2__current
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___2__current);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<global::Il2CppSystem.Object>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___2__current), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00009D90 File Offset: 0x00007F90
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00002F90 File Offset: 0x00001190
		public unsafe LoadFromResourcesAsync __4__this
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___4__this);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<LoadFromResourcesAsync>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr___4__this), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00009DC0 File Offset: 0x00007FC0
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00002FAF File Offset: 0x000011AF
		public unsafe ResourceRequest _resourceRequest_5__2
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr__resourceRequest_5__2);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<ResourceRequest>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(LoadFromResourcesAsync._Start_d__2.NativeFieldInfoPtr__resourceRequest_5__2), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x04000141 RID: 321
		private static readonly IntPtr NativeFieldInfoPtr___1__state;

		// Token: 0x04000142 RID: 322
		private static readonly IntPtr NativeFieldInfoPtr___2__current;

		// Token: 0x04000143 RID: 323
		private static readonly IntPtr NativeFieldInfoPtr___4__this;

		// Token: 0x04000144 RID: 324
		private static readonly IntPtr NativeFieldInfoPtr__resourceRequest_5__2;

		// Token: 0x04000145 RID: 325
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		// Token: 0x04000146 RID: 326
		private static readonly IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		// Token: 0x04000147 RID: 327
		private static readonly IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		// Token: 0x04000148 RID: 328
		private static readonly IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

		// Token: 0x04000149 RID: 329
		private static readonly IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		// Token: 0x0400014A RID: 330
		private static readonly IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;
	}
}
