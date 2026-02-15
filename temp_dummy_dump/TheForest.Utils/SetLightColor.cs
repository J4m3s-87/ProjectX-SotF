using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000034 RID: 52
	[Token(Token = "0x2000034")]
	public class SetLightColor : MonoBehaviour
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000AC")]
		[Address(RVA = "0x3BEF3B0", Offset = "0x3BEE1B0", VA = "0x183BEF3B0")]
		public void ApplyColor()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000AD")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public SetLightColor()
		{
		}

		// Token: 0x0400007C RID: 124
		[Token(Token = "0x400007C")]
		[FieldOffset(Offset = "0x20")]
		public Light _light;

		// Token: 0x0400007D RID: 125
		[Token(Token = "0x400007D")]
		[FieldOffset(Offset = "0x28")]
		public Color _color;
	}
}
