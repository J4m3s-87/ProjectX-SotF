using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002B RID: 43
	[Token(Token = "0x200002B")]
	public class MaterialPropertyColors : MonoBehaviour
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000097")]
		[Address(RVA = "0x3BEDDA0", Offset = "0x3BECBA0", VA = "0x183BEDDA0")]
		public void SetColor(int colorNum)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000098")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public MaterialPropertyColors()
		{
		}

		// Token: 0x04000067 RID: 103
		[Token(Token = "0x4000067")]
		[FieldOffset(Offset = "0x20")]
		public SetMaterialProperty _target;

		// Token: 0x04000068 RID: 104
		[Token(Token = "0x4000068")]
		[FieldOffset(Offset = "0x28")]
		public Color[] _colors;
	}
}
