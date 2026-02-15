using System;
using Il2CppDummyDll;

// Token: 0x02000006 RID: 6
[Token(Token = "0x2000006")]
public class LCGRand
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002088 File Offset: 0x00000288
	[Token(Token = "0x600001B")]
	[Address(RVA = "0x3BE3920", Offset = "0x3BE2720", VA = "0x183BE3920")]
	private static uint Next()
	{
		return 0U;
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600001C RID: 28 RVA: 0x000020A0 File Offset: 0x000002A0
	[Token(Token = "0x17000002")]
	public static float Value01
	{
		[Token(Token = "0x600001C")]
		[Address(RVA = "0x3BE39B0", Offset = "0x3BE27B0", VA = "0x183BE39B0")]
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000020B8 File Offset: 0x000002B8
	[Token(Token = "0x600001D")]
	[Address(RVA = "0x3BE3A80", Offset = "0x3BE2880", VA = "0x183BE3A80")]
	public static float Range(float min, float max)
	{
		return 0f;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000020D0 File Offset: 0x000002D0
	[Token(Token = "0x600001E")]
	[Address(RVA = "0x3BE3AF0", Offset = "0x3BE28F0", VA = "0x183BE3AF0")]
	public static int Range(int min, int max)
	{
		return 0;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600001F")]
	[Address(RVA = "0x597160", Offset = "0x595F60", VA = "0x180597160")]
	public LCGRand()
	{
	}

	// Token: 0x04000008 RID: 8
	[Token(Token = "0x4000008")]
	[FieldOffset(Offset = "0x0")]
	public static uint Seed;
}
