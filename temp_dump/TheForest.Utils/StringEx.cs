using System;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace TheForest.Utils
{
	// Token: 0x0200002D RID: 45
	public class StringEx : Object
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00008730 File Offset: 0x00006930
		// Note: this type is marked as 'beforefieldinit'.
		static StringEx()
		{
			Il2CppClassPointerStore<StringEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "StringEx");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringEx>.NativeClassPtr);
			StringEx.NativeMethodInfoPtr_TryFormat_Public_Static_String_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringEx>.NativeClassPtr, 100663479);
			StringEx.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringEx>.NativeClassPtr, 100663480);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008788 File Offset: 0x00006988
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1500072, XrefRangeEnd = 1500074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string TryFormat(string format, [Optional] Il2CppReferenceArray<Object> args)
		{
			if (args == null)
			{
				args = new Il2CppReferenceArray<Object>(0L);
			}
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(format);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(args);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringEx.NativeMethodInfoPtr_TryFormat_Public_Static_String_String_Il2CppReferenceArray_1_Object_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000087E4 File Offset: 0x000069E4
		[CallerCount(250)]
		[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe StringEx()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringEx>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(StringEx.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00002C41 File Offset: 0x00000E41
		public static string TryFormat(string format, params Object[] args)
		{
			return StringEx.TryFormat(format, new Il2CppReferenceArray<Object>(args));
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00002C4F File Offset: 0x00000E4F
		public StringEx(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000101 RID: 257
		private static readonly IntPtr NativeMethodInfoPtr_TryFormat_Public_Static_String_String_Il2CppReferenceArray_1_Object_0;

		// Token: 0x04000102 RID: 258
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
	}
}
