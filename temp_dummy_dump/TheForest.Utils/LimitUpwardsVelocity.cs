using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000028 RID: 40
	[Token(Token = "0x2000028")]
	public class LimitUpwardsVelocity : MonoBehaviour
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008E")]
		[Address(RVA = "0x3BECE80", Offset = "0x3BEBC80", VA = "0x183BECE80")]
		private void Awake()
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008F")]
		[Address(RVA = "0x3BED000", Offset = "0x3BEBE00", VA = "0x183BED000")]
		private void FixedUpdate()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000090")]
		[Address(RVA = "0x3BED250", Offset = "0x3BEC050", VA = "0x183BED250")]
		public LimitUpwardsVelocity()
		{
		}

		// Token: 0x0400005C RID: 92
		[Token(Token = "0x400005C")]
		[FieldOffset(Offset = "0x20")]
		public float _maxVelocity;

		// Token: 0x0400005D RID: 93
		[Token(Token = "0x400005D")]
		[FieldOffset(Offset = "0x24")]
		public float _maxAngularVelocity;

		// Token: 0x0400005E RID: 94
		[Token(Token = "0x400005E")]
		[FieldOffset(Offset = "0x28")]
		private Rigidbody _rb;
	}
}
