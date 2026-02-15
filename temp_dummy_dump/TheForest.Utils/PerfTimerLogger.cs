using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Il2CppDummyDll;

// Token: 0x0200000E RID: 14
[Token(Token = "0x200000E")]
public class PerfTimerLogger : global::System.IDisposable
{
	// Token: 0x06000039 RID: 57 RVA: 0x00002082 File Offset: 0x00000282
	[Token(Token = "0x6000039")]
	[Address(RVA = "0x3BE6C20", Offset = "0x3BE5A20", VA = "0x183BE6C20")]
	public static PerfTimerLogger Get(string message)
	{
		return null;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600003A")]
	[Address(RVA = "0x3BE6CD0", Offset = "0x3BE5AD0", VA = "0x183BE6CD0")]
	public PerfTimerLogger(string message, PerfTimerLogger.LogResultType resultType = PerfTimerLogger.LogResultType.Milliseconds, [global::System.Runtime.InteropServices.Optional] global::System.Action<string> logAction)
	{
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600003B")]
	[Address(RVA = "0x3BE7030", Offset = "0x3BE5E30", VA = "0x183BE7030")]
	public void Pause()
	{
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600003C")]
	[Address(RVA = "0x3BE7050", Offset = "0x3BE5E50", VA = "0x183BE7050")]
	public void Unpause()
	{
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600003D")]
	[Address(RVA = "0x3BE70B0", Offset = "0x3BE5EB0", VA = "0x183BE70B0")]
	public void Stop()
	{
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600003E")]
	[Address(RVA = "0x3BE70C0", Offset = "0x3BE5EC0", VA = "0x183BE70C0", Slot = "4")]
	public void Dispose()
	{
	}

	// Token: 0x04000016 RID: 22
	[Token(Token = "0x4000016")]
	[global::Il2CppDummyDll.FieldOffset(Offset = "0x0")]
	private static global::System.Collections.Generic.Dictionary<string, PerfTimerLogger> _instances;

	// Token: 0x04000017 RID: 23
	[Token(Token = "0x4000017")]
	[global::Il2CppDummyDll.FieldOffset(Offset = "0x10")]
	private string _message;

	// Token: 0x04000018 RID: 24
	[Token(Token = "0x4000018")]
	[global::Il2CppDummyDll.FieldOffset(Offset = "0x18")]
	private Stopwatch _timer;

	// Token: 0x04000019 RID: 25
	[Token(Token = "0x4000019")]
	[global::Il2CppDummyDll.FieldOffset(Offset = "0x20")]
	private PerfTimerLogger.LogResultType _logResultType;

	// Token: 0x0400001A RID: 26
	[Token(Token = "0x400001A")]
	[global::Il2CppDummyDll.FieldOffset(Offset = "0x28")]
	private global::System.Action<string> _logAction;

	// Token: 0x0200000F RID: 15
	[Token(Token = "0x200000F")]
	public enum LogResultType
	{
		// Token: 0x0400001C RID: 28
		[Token(Token = "0x400001C")]
		Milliseconds,
		// Token: 0x0400001D RID: 29
		[Token(Token = "0x400001D")]
		Ticks
	}
}
