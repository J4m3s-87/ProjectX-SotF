using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x02000004 RID: 4
[Token(Token = "0x2000004")]
public class IntervalTextureLogger : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600000D")]
	[Address(RVA = "0x3BE1C50", Offset = "0x3BE0A50", VA = "0x183BE1C50")]
	public static void Start(float interval)
	{
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600000E")]
	[Address(RVA = "0x3BE1E10", Offset = "0x3BE0C10", VA = "0x183BE1E10")]
	public static void Stop()
	{
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002054 File Offset: 0x00000254
	[Token(Token = "0x600000F")]
	[Address(RVA = "0x3BE1F60", Offset = "0x3BE0D60", VA = "0x183BE1F60")]
	private int GetBitsPerPixel(TextureFormat format)
	{
		return 0;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000206C File Offset: 0x0000026C
	[Token(Token = "0x6000010")]
	[Address(RVA = "0x3BE2000", Offset = "0x3BE0E00", VA = "0x183BE2000")]
	private int CalculateTextureSizeBytes(Texture tTexture)
	{
		return 0;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000011")]
	[Address(RVA = "0x3BE24E0", Offset = "0x3BE12E0", VA = "0x183BE24E0")]
	private void LateUpdate()
	{
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002082 File Offset: 0x00000282
	[Token(Token = "0x6000012")]
	[Address(RVA = "0x3BE2B60", Offset = "0x3BE1960", VA = "0x183BE2B60")]
	private static string ToReadable(int size)
	{
		return null;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000013")]
	[Address(RVA = "0xA711C0", Offset = "0xA6FFC0", VA = "0x180A711C0")]
	public IntervalTextureLogger()
	{
	}

	// Token: 0x04000002 RID: 2
	[Token(Token = "0x4000002")]
	[FieldOffset(Offset = "0x20")]
	public float Interval;

	// Token: 0x04000003 RID: 3
	[Token(Token = "0x4000003")]
	[FieldOffset(Offset = "0x24")]
	private float _lastRun;

	// Token: 0x04000004 RID: 4
	[Token(Token = "0x4000004")]
	[FieldOffset(Offset = "0x0")]
	private static IntervalTextureLogger _instance;
}
