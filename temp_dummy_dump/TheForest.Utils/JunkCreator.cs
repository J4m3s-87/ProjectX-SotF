using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x02000005 RID: 5
[Token(Token = "0x2000005")]
public class JunkCreator : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002082 File Offset: 0x00000282
	[Token(Token = "0x17000001")]
	public static JunkCreator Instance
	{
		[Token(Token = "0x6000014")]
		[Address(RVA = "0x3BE2CD0", Offset = "0x3BE1AD0", VA = "0x183BE2CD0")]
		get
		{
			return null;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000015")]
	[Address(RVA = "0x3BE2F20", Offset = "0x3BE1D20", VA = "0x183BE2F20")]
	private void OnDestroy()
	{
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000016")]
	[Address(RVA = "0x3BE30B0", Offset = "0x3BE1EB0", VA = "0x183BE30B0")]
	public void Init()
	{
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000017")]
	[Address(RVA = "0x3BE3140", Offset = "0x3BE1F40", VA = "0x183BE3140")]
	public void AddJunk(int amount)
	{
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000018")]
	[Address(RVA = "0x3BE3150", Offset = "0x3BE1F50", VA = "0x183BE3150")]
	public void SetJunk(int amount)
	{
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000019")]
	[Address(RVA = "0x3BE3670", Offset = "0x3BE2470", VA = "0x183BE3670")]
	public void Clear()
	{
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600001A")]
	[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
	public JunkCreator()
	{
	}

	// Token: 0x04000005 RID: 5
	[Token(Token = "0x4000005")]
	[FieldOffset(Offset = "0x0")]
	private static JunkCreator instance;

	// Token: 0x04000006 RID: 6
	[Token(Token = "0x4000006")]
	[FieldOffset(Offset = "0x20")]
	private int arraySize;

	// Token: 0x04000007 RID: 7
	[Token(Token = "0x4000007")]
	[FieldOffset(Offset = "0x28")]
	private GameObject[] junkArray;
}
