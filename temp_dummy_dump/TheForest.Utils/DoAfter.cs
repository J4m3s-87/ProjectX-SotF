using System;
using Il2CppDummyDll;
using UnityEngine;
using UnityEngine.Events;

namespace TheForest.Utils
{
	// Token: 0x02000024 RID: 36
	[Token(Token = "0x2000024")]
	public class DoAfter : MonoBehaviour
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007D")]
		[Address(RVA = "0x3BEBB00", Offset = "0x3BEA900", VA = "0x183BEBB00")]
		public void BeginDelay()
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007E")]
		[Address(RVA = "0xB4B8D0", Offset = "0xB4A6D0", VA = "0x180B4B8D0")]
		private void Finished()
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007F")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public DoAfter()
		{
		}

		// Token: 0x04000053 RID: 83
		[Token(Token = "0x4000053")]
		[FieldOffset(Offset = "0x20")]
		public float _delay;

		// Token: 0x04000054 RID: 84
		[Token(Token = "0x4000054")]
		[FieldOffset(Offset = "0x28")]
		public UnityEvent _callback;
	}
}
