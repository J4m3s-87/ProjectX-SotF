using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001F RID: 31
	[Token(Token = "0x200001F")]
	public class DestroyGOListOnDestroy : MonoBehaviour
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000067")]
		[Address(RVA = "0x3BEB2C0", Offset = "0x3BEA0C0", VA = "0x183BEB2C0")]
		private void OnDestroy()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000068")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public DestroyGOListOnDestroy()
		{
		}

		// Token: 0x04000043 RID: 67
		[Token(Token = "0x4000043")]
		[FieldOffset(Offset = "0x20")]
		public GameObject[] _golist;
	}
}
