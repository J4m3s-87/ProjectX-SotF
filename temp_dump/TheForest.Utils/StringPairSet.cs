using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class StringPairSet : ScriptableObject
{
	// Token: 0x06000079 RID: 121 RVA: 0x00004D94 File Offset: 0x00002F94
	// Note: this type is marked as 'beforefieldinit'.
	static StringPairSet()
	{
		Il2CppClassPointerStore<StringPairSet>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "StringPairSet");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringPairSet>.NativeClassPtr);
		StringPairSet.NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringPairSet>.NativeClassPtr, "Items");
		StringPairSet.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringPairSet>.NativeClassPtr, 100663360);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004DEC File Offset: 0x00002FEC
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498997, XrefRangeEnd = 1499004, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StringPairSet()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringPairSet>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringPairSet.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000022F8 File Offset: 0x000004F8
	public StringPairSet(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600007C RID: 124 RVA: 0x00004E28 File Offset: 0x00003028
	// (set) Token: 0x0600007D RID: 125 RVA: 0x00002301 File Offset: 0x00000501
	public unsafe List<StringPairSet.StringPair> Items
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.NativeFieldInfoPtr_Items);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<List<StringPairSet.StringPair>>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.NativeFieldInfoPtr_Items), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x0400004D RID: 77
	private static readonly IntPtr NativeFieldInfoPtr_Items;

	// Token: 0x0400004E RID: 78
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	// Token: 0x0200003C RID: 60
	[Serializable]
	public class StringPair : global::Il2CppSystem.Object
	{
		// Token: 0x06000232 RID: 562 RVA: 0x00009ED8 File Offset: 0x000080D8
		// Note: this type is marked as 'beforefieldinit'.
		static StringPair()
		{
			Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StringPairSet>.NativeClassPtr, "StringPair");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr);
			StringPairSet.StringPair.NativeFieldInfoPtr_Key = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr, "Key");
			StringPairSet.StringPair.NativeFieldInfoPtr_Value = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr, "Value");
			StringPairSet.StringPair.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr, 100663361);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009F40 File Offset: 0x00008140
		[CallerCount(250)]
		[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe StringPair()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringPairSet.StringPair>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringPairSet.StringPair.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000303E File Offset: 0x0000123E
		public StringPair(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00009F7C File Offset: 0x0000817C
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00003047 File Offset: 0x00001247
		public unsafe string Key
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.StringPair.NativeFieldInfoPtr_Key);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.StringPair.NativeFieldInfoPtr_Key), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00009FA4 File Offset: 0x000081A4
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00003066 File Offset: 0x00001266
		public unsafe string Value
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.StringPair.NativeFieldInfoPtr_Value);
				return IL2CPP.Il2CppStringToManaged(*intPtr);
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(StringPairSet.StringPair.NativeFieldInfoPtr_Value), IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x04000151 RID: 337
		private static readonly IntPtr NativeFieldInfoPtr_Key;

		// Token: 0x04000152 RID: 338
		private static readonly IntPtr NativeFieldInfoPtr_Value;

		// Token: 0x04000153 RID: 339
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
