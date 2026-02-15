using System;
using System.Collections.Generic;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x02000011 RID: 17
[Token(Token = "0x2000011")]
[CreateAssetMenu(fileName = "StringPairSet", menuName = "General/StringPairSet", order = 1)]
public class StringPairSet : ScriptableObject
{
	// Token: 0x06000040 RID: 64 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000040")]
	[Address(RVA = "0x3BE72C0", Offset = "0x3BE60C0", VA = "0x183BE72C0")]
	public StringPairSet()
	{
	}

	// Token: 0x0400001F RID: 31
	[Token(Token = "0x400001F")]
	[FieldOffset(Offset = "0x18")]
	public global::System.Collections.Generic.List<StringPairSet.StringPair> Items;

	// Token: 0x02000012 RID: 18
	[Token(Token = "0x2000012")]
	[global::System.Serializable]
	public class StringPair
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000041")]
		[Address(RVA = "0x597160", Offset = "0x595F60", VA = "0x180597160")]
		public StringPair()
		{
		}

		// Token: 0x04000020 RID: 32
		[Token(Token = "0x4000020")]
		[FieldOffset(Offset = "0x10")]
		public string Key;

		// Token: 0x04000021 RID: 33
		[Token(Token = "0x4000021")]
		[FieldOffset(Offset = "0x18")]
		public string Value;
	}
}
