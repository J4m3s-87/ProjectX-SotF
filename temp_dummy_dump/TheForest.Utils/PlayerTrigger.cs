using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002F RID: 47
	[Token(Token = "0x200002F")]
	public class PlayerTrigger : MonoBehaviour
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600009F")]
		[Address(RVA = "0x3BEE690", Offset = "0x3BED490", VA = "0x183BEE690")]
		private void OnTriggerEnter(Collider other)
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A0")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public PlayerTrigger()
		{
		}

		// Token: 0x0400006F RID: 111
		[Token(Token = "0x400006F")]
		[FieldOffset(Offset = "0x20")]
		public string _message;

		// Token: 0x04000070 RID: 112
		[Token(Token = "0x4000070")]
		[FieldOffset(Offset = "0x28")]
		public GameObject _target;

		// Token: 0x04000071 RID: 113
		[Token(Token = "0x4000071")]
		[FieldOffset(Offset = "0x30")]
		public bool _destroyAfter;
	}
}
