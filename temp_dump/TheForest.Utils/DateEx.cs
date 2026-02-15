using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace TheForest.Utils
{
	// Token: 0x02000015 RID: 21
	public static class DateEx : Object
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00005764 File Offset: 0x00003964
		// Note: this type is marked as 'beforefieldinit'.
		static DateEx()
		{
			Il2CppClassPointerStore<DateEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DateEx");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DateEx>.NativeClassPtr);
			DateEx.NativeMethodInfoPtr_UnixTimeStampToDateTime_Public_Static_DateTime_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DateEx>.NativeClassPtr, 100663380);
			DateEx.NativeMethodInfoPtr_ToUnixTimestamp_Public_Static_Int64_DateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DateEx>.NativeClassPtr, 100663381);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000057BC File Offset: 0x000039BC
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499258, XrefRangeEnd = 1499261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref unixTimeStamp;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DateEx.NativeMethodInfoPtr_UnixTimeStampToDateTime_Public_Static_DateTime_Int64_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000057FC File Offset: 0x000039FC
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 1499270, RefRangeEnd = 1499273, XrefRangeStart = 1499261, XrefRangeEnd = 1499270, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static long ToUnixTimestamp(this DateTime dateTime)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = ref dateTime;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DateEx.NativeMethodInfoPtr_ToUnixTimestamp_Public_Static_Int64_DateTime_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002455 File Offset: 0x00000655
		public DateEx(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x0400006A RID: 106
		private static readonly IntPtr NativeMethodInfoPtr_UnixTimeStampToDateTime_Public_Static_DateTime_Int64_0;

		// Token: 0x0400006B RID: 107
		private static readonly IntPtr NativeMethodInfoPtr_ToUnixTimestamp_Public_Static_Int64_DateTime_0;
	}
}
