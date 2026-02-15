using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000029 RID: 41
	[Token(Token = "0x2000029")]
	public class LimitVelocity : MonoBehaviour
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000091")]
		[Address(RVA = "0x3BED2A0", Offset = "0x3BEC0A0", VA = "0x183BED2A0")]
		private void Awake()
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000092")]
		[Address(RVA = "0x3BED420", Offset = "0x3BEC220", VA = "0x183BED420")]
		private void Update()
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000093")]
		[Address(RVA = "0x3BED250", Offset = "0x3BEC050", VA = "0x183BED250")]
		public LimitVelocity()
		{
		}

		// Token: 0x0400005F RID: 95
		[Token(Token = "0x400005F")]
		[FieldOffset(Offset = "0x20")]
		public float _maxVelocity;

		// Token: 0x04000060 RID: 96
		[Token(Token = "0x4000060")]
		[FieldOffset(Offset = "0x24")]
		public float _maxAngularVelocity;

		// Token: 0x04000061 RID: 97
		[Token(Token = "0x4000061")]
		[FieldOffset(Offset = "0x28")]
		private Rigidbody _rb;
	}
}
