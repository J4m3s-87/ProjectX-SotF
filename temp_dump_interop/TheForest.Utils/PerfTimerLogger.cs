using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Diagnostics;

// Token: 0x0200000C RID: 12
public class PerfTimerLogger : Object
{
	// Token: 0x06000062 RID: 98 RVA: 0x00004968 File Offset: 0x00002B68
	// Note: this type is marked as 'beforefieldinit'.
	static PerfTimerLogger()
	{
		Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "PerfTimerLogger");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr);
		PerfTimerLogger.NativeFieldInfoPtr__instances = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, "_instances");
		PerfTimerLogger.NativeFieldInfoPtr__message = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, "_message");
		PerfTimerLogger.NativeFieldInfoPtr__timer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, "_timer");
		PerfTimerLogger.NativeFieldInfoPtr__logResultType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, "_logResultType");
		PerfTimerLogger.NativeFieldInfoPtr__logAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, "_logAction");
		PerfTimerLogger.NativeMethodInfoPtr_Get_Public_Static_PerfTimerLogger_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663353);
		PerfTimerLogger.NativeMethodInfoPtr__ctor_Public_Void_String_LogResultType_Action_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663354);
		PerfTimerLogger.NativeMethodInfoPtr_Pause_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663355);
		PerfTimerLogger.NativeMethodInfoPtr_Unpause_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663356);
		PerfTimerLogger.NativeMethodInfoPtr_Stop_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663357);
		PerfTimerLogger.NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr, 100663358);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00004A74 File Offset: 0x00002C74
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498918, XrefRangeEnd = 1498927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static PerfTimerLogger Get(string message)
	{
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr_Get_Public_Static_PerfTimerLogger_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			IntPtr intPtr3 = intPtr;
			return (intPtr3 != 0) ? Il2CppObjectPool.Get<PerfTimerLogger>(intPtr3) : null;
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00004AB8 File Offset: 0x00002CB8
	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1498953, RefRangeEnd = 1498955, XrefRangeStart = 1498927, XrefRangeEnd = 1498953, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PerfTimerLogger(string message, PerfTimerLogger.LogResultType resultType = PerfTimerLogger.LogResultType.Milliseconds, Action<string> logAction = null)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PerfTimerLogger>.NativeClassPtr))
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref resultType;
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(logAction);
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr__ctor_Public_Void_String_LogResultType_Action_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00004B24 File Offset: 0x00002D24
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498955, XrefRangeEnd = 1498957, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Pause()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr_Pause_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00004B58 File Offset: 0x00002D58
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498957, XrefRangeEnd = 1498961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Unpause()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr_Unpause_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00004B8C File Offset: 0x00002D8C
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498961, XrefRangeEnd = 1498962, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Stop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr_Stop_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004BC0 File Offset: 0x00002DC0
	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1498994, RefRangeEnd = 1498997, XrefRangeStart = 1498962, XrefRangeEnd = 1498994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Dispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(PerfTimerLogger.NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00002241 File Offset: 0x00000441
	public PerfTimerLogger(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600006A RID: 106 RVA: 0x00004BF4 File Offset: 0x00002DF4
	// (set) Token: 0x0600006B RID: 107 RVA: 0x0000224A File Offset: 0x0000044A
	public unsafe static Dictionary<string, PerfTimerLogger> _instances
	{
		get
		{
			IntPtr intPtr;
			IL2CPP.il2cpp_field_static_get_value(PerfTimerLogger.NativeFieldInfoPtr__instances, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Dictionary<string, PerfTimerLogger>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(PerfTimerLogger.NativeFieldInfoPtr__instances, IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600006C RID: 108 RVA: 0x00004C1C File Offset: 0x00002E1C
	// (set) Token: 0x0600006D RID: 109 RVA: 0x0000225C File Offset: 0x0000045C
	public unsafe string _message
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__message);
			return IL2CPP.Il2CppStringToManaged(*intPtr);
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__message), IL2CPP.ManagedStringToIl2Cpp(value));
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600006E RID: 110 RVA: 0x00004C44 File Offset: 0x00002E44
	// (set) Token: 0x0600006F RID: 111 RVA: 0x0000227B File Offset: 0x0000047B
	public unsafe Stopwatch _timer
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__timer);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Stopwatch>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__timer), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000070 RID: 112 RVA: 0x00004C74 File Offset: 0x00002E74
	// (set) Token: 0x06000071 RID: 113 RVA: 0x0000229A File Offset: 0x0000049A
	public unsafe PerfTimerLogger.LogResultType _logResultType
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__logResultType);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__logResultType)) = value;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000072 RID: 114 RVA: 0x00004C9C File Offset: 0x00002E9C
	// (set) Token: 0x06000073 RID: 115 RVA: 0x000022B5 File Offset: 0x000004B5
	public unsafe Action<string> _logAction
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__logAction);
			IntPtr intPtr2 = *intPtr;
			return (intPtr2 != 0) ? Il2CppObjectPool.Get<Action<string>>(intPtr2) : null;
		}
		set
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(PerfTimerLogger.NativeFieldInfoPtr__logAction), IL2CPP.Il2CppObjectBaseToPtr(value));
		}
	}

	// Token: 0x04000040 RID: 64
	private static readonly IntPtr NativeFieldInfoPtr__instances;

	// Token: 0x04000041 RID: 65
	private static readonly IntPtr NativeFieldInfoPtr__message;

	// Token: 0x04000042 RID: 66
	private static readonly IntPtr NativeFieldInfoPtr__timer;

	// Token: 0x04000043 RID: 67
	private static readonly IntPtr NativeFieldInfoPtr__logResultType;

	// Token: 0x04000044 RID: 68
	private static readonly IntPtr NativeFieldInfoPtr__logAction;

	// Token: 0x04000045 RID: 69
	private static readonly IntPtr NativeMethodInfoPtr_Get_Public_Static_PerfTimerLogger_String_0;

	// Token: 0x04000046 RID: 70
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_LogResultType_Action_1_String_0;

	// Token: 0x04000047 RID: 71
	private static readonly IntPtr NativeMethodInfoPtr_Pause_Public_Void_0;

	// Token: 0x04000048 RID: 72
	private static readonly IntPtr NativeMethodInfoPtr_Unpause_Public_Void_0;

	// Token: 0x04000049 RID: 73
	private static readonly IntPtr NativeMethodInfoPtr_Stop_Public_Void_0;

	// Token: 0x0400004A RID: 74
	private static readonly IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

	// Token: 0x0200003B RID: 59
	public enum LogResultType
	{
		// Token: 0x0400014F RID: 335
		Milliseconds,
		// Token: 0x04000150 RID: 336
		Ticks
	}
}
