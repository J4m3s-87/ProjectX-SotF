using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace TheForest.Utils
{
	// Token: 0x0200002E RID: 46
	public static class StringExtensions : Object
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x00008820 File Offset: 0x00006A20
		// Note: this type is marked as 'beforefieldinit'.
		static StringExtensions()
		{
			Il2CppClassPointerStore<StringExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "StringExtensions");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr);
			StringExtensions.NativeFieldInfoPtr_EmptyString = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, "EmptyString");
			StringExtensions.NativeMethodInfoPtr_IsNull_Public_Static_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663481);
			StringExtensions.NativeMethodInfoPtr_IsEmpty_Public_Static_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663482);
			StringExtensions.NativeMethodInfoPtr_NullOrEmpty_Public_Static_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663483);
			StringExtensions.NativeMethodInfoPtr_FirstOrDefault_Public_Static_String_IEnumerable_1_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663484);
			StringExtensions.NativeMethodInfoPtr_FirstNotNull_Public_Static_String_IEnumerable_1_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663485);
			StringExtensions.NativeMethodInfoPtr_FirstNotNullOrEmpty_Public_Static_String_IEnumerable_1_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663486);
			StringExtensions.NativeMethodInfoPtr_IfNull_Public_Static_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663487);
			StringExtensions.NativeMethodInfoPtr_IfNullOrEmpty_Public_Static_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, 100663488);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008904 File Offset: 0x00006B04
		[CallerCount(0)]
		public unsafe static bool IsNull(this string stringValue)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_IsNull_Public_Static_Boolean_String_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008948 File Offset: 0x00006B48
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500074, XrefRangeEnd = 1500077, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool IsEmpty(this string stringValue)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_IsEmpty_Public_Static_Boolean_String_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000898C File Offset: 0x00006B8C
		[CallerCount(0)]
		public unsafe static bool NullOrEmpty(this string stringValue)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_NullOrEmpty_Public_Static_Boolean_String_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000089D0 File Offset: 0x00006BD0
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1500078, RefRangeEnd = 1500079, XrefRangeStart = 1500077, XrefRangeEnd = 1500078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string FirstOrDefault(this IEnumerable<string> strings, string defaultResult = null)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(strings);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_FirstOrDefault_Public_Static_String_IEnumerable_1_String_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008A20 File Offset: 0x00006C20
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500079, XrefRangeEnd = 1500096, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string FirstNotNull(this IEnumerable<string> strings, string defaultResult = null)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(strings);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_FirstNotNull_Public_Static_String_IEnumerable_1_String_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008A70 File Offset: 0x00006C70
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500096, XrefRangeEnd = 1500113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string FirstNotNullOrEmpty(this IEnumerable<string> strings, string defaultResult = null)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(strings);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_FirstNotNullOrEmpty_Public_Static_String_IEnumerable_1_String_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008AC0 File Offset: 0x00006CC0
		[CallerCount(0)]
		public unsafe static string IfNull(this string stringValue, string defaultResult)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_IfNull_Public_Static_String_String_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008B10 File Offset: 0x00006D10
		[CallerCount(0)]
		public unsafe static string IfNullOrEmpty(this string stringValue, string defaultResult)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.NativeMethodInfoPtr_IfNullOrEmpty_Public_Static_String_String_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00002C58 File Offset: 0x00000E58
		public StringExtensions(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00008B60 File Offset: 0x00006D60
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00002C61 File Offset: 0x00000E61
		public unsafe static string EmptyString
		{
			get
			{
				IntPtr intPtr;
				IL2CPP.il2cpp_field_static_get_value(StringExtensions.NativeFieldInfoPtr_EmptyString, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(StringExtensions.NativeFieldInfoPtr_EmptyString, IL2CPP.ManagedStringToIl2Cpp(value));
			}
		}

		// Token: 0x04000103 RID: 259
		private static readonly IntPtr NativeFieldInfoPtr_EmptyString;

		// Token: 0x04000104 RID: 260
		private static readonly IntPtr NativeMethodInfoPtr_IsNull_Public_Static_Boolean_String_0;

		// Token: 0x04000105 RID: 261
		private static readonly IntPtr NativeMethodInfoPtr_IsEmpty_Public_Static_Boolean_String_0;

		// Token: 0x04000106 RID: 262
		private static readonly IntPtr NativeMethodInfoPtr_NullOrEmpty_Public_Static_Boolean_String_0;

		// Token: 0x04000107 RID: 263
		private static readonly IntPtr NativeMethodInfoPtr_FirstOrDefault_Public_Static_String_IEnumerable_1_String_String_0;

		// Token: 0x04000108 RID: 264
		private static readonly IntPtr NativeMethodInfoPtr_FirstNotNull_Public_Static_String_IEnumerable_1_String_String_0;

		// Token: 0x04000109 RID: 265
		private static readonly IntPtr NativeMethodInfoPtr_FirstNotNullOrEmpty_Public_Static_String_IEnumerable_1_String_String_0;

		// Token: 0x0400010A RID: 266
		private static readonly IntPtr NativeMethodInfoPtr_IfNull_Public_Static_String_String_String_0;

		// Token: 0x0400010B RID: 267
		private static readonly IntPtr NativeMethodInfoPtr_IfNullOrEmpty_Public_Static_String_String_String_0;

		// Token: 0x02000046 RID: 70
		[ObfuscatedName("TheForest.Utils.StringExtensions+<>c")]
		[Serializable]
		public sealed class __c : Object
		{
			// Token: 0x06000264 RID: 612 RVA: 0x0000A6BC File Offset: 0x000088BC
			// Note: this type is marked as 'beforefieldinit'.
			static __c()
			{
				Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StringExtensions>.NativeClassPtr, "<>c");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr);
				StringExtensions.__c.NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, "<>9");
				StringExtensions.__c.NativeFieldInfoPtr___9__5_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, "<>9__5_0");
				StringExtensions.__c.NativeFieldInfoPtr___9__6_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, "<>9__6_0");
				StringExtensions.__c.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, 100663490);
				StringExtensions.__c.NativeMethodInfoPtr__FirstNotNull_b__5_0_Internal_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, 100663491);
				StringExtensions.__c.NativeMethodInfoPtr__FirstNotNullOrEmpty_b__6_0_Internal_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr, 100663492);
			}

			// Token: 0x06000265 RID: 613 RVA: 0x0000A760 File Offset: 0x00008960
			[CallerCount(250)]
			[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe __c()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringExtensions.__c>.NativeClassPtr))
			{
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.__c.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			// Token: 0x06000266 RID: 614 RVA: 0x0000A79C File Offset: 0x0000899C
			[CallerCount(0)]
			public unsafe bool _FirstNotNull_b__5_0(string stringValue)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.__c.NativeMethodInfoPtr__FirstNotNull_b__5_0_Internal_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
					return *IL2CPP.il2cpp_object_unbox(intPtr);
				}
			}

			// Token: 0x06000267 RID: 615 RVA: 0x0000A7EC File Offset: 0x000089EC
			[CallerCount(0)]
			public unsafe bool _FirstNotNullOrEmpty_b__6_0(string stringValue)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				checked
				{
					IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
					*ptr = IL2CPP.ManagedStringToIl2Cpp(stringValue);
					IntPtr intPtr2;
					IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringExtensions.__c.NativeMethodInfoPtr__FirstNotNullOrEmpty_b__6_0_Internal_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
					Il2CppException.RaiseExceptionIfNecessary(intPtr2);
					return *IL2CPP.il2cpp_object_unbox(intPtr);
				}
			}

			// Token: 0x06000268 RID: 616 RVA: 0x00003229 File Offset: 0x00001429
			public __c(IntPtr pointer)
				: base(pointer)
			{
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x06000269 RID: 617 RVA: 0x0000A83C File Offset: 0x00008A3C
			// (set) Token: 0x0600026A RID: 618 RVA: 0x00003232 File Offset: 0x00001432
			public unsafe static StringExtensions.__c __9
			{
				get
				{
					IntPtr intPtr;
					IL2CPP.il2cpp_field_static_get_value(StringExtensions.__c.NativeFieldInfoPtr___9, (void*)(&intPtr));
					IntPtr intPtr2 = intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<StringExtensions.__c>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(StringExtensions.__c.NativeFieldInfoPtr___9, IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x0600026B RID: 619 RVA: 0x0000A864 File Offset: 0x00008A64
			// (set) Token: 0x0600026C RID: 620 RVA: 0x00003244 File Offset: 0x00001444
			public unsafe static Func<string, bool> __9__5_0
			{
				get
				{
					IntPtr intPtr;
					IL2CPP.il2cpp_field_static_get_value(StringExtensions.__c.NativeFieldInfoPtr___9__5_0, (void*)(&intPtr));
					IntPtr intPtr2 = intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<Func<string, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(StringExtensions.__c.NativeFieldInfoPtr___9__5_0, IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x0600026D RID: 621 RVA: 0x0000A88C File Offset: 0x00008A8C
			// (set) Token: 0x0600026E RID: 622 RVA: 0x00003256 File Offset: 0x00001456
			public unsafe static Func<string, bool> __9__6_0
			{
				get
				{
					IntPtr intPtr;
					IL2CPP.il2cpp_field_static_get_value(StringExtensions.__c.NativeFieldInfoPtr___9__6_0, (void*)(&intPtr));
					IntPtr intPtr2 = intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<Func<string, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(StringExtensions.__c.NativeFieldInfoPtr___9__6_0, IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x0400017A RID: 378
			private static readonly IntPtr NativeFieldInfoPtr___9;

			// Token: 0x0400017B RID: 379
			private static readonly IntPtr NativeFieldInfoPtr___9__5_0;

			// Token: 0x0400017C RID: 380
			private static readonly IntPtr NativeFieldInfoPtr___9__6_0;

			// Token: 0x0400017D RID: 381
			private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			// Token: 0x0400017E RID: 382
			private static readonly IntPtr NativeMethodInfoPtr__FirstNotNull_b__5_0_Internal_Boolean_String_0;

			// Token: 0x0400017F RID: 383
			private static readonly IntPtr NativeMethodInfoPtr__FirstNotNullOrEmpty_b__6_0_Internal_Boolean_String_0;
		}
	}
}
