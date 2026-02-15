using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002A RID: 42
	[Token(Token = "0x200002A")]
	public class LimitVelocityBurst : MonoBehaviour
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000094")]
		[Address(RVA = "0x3BED720", Offset = "0x3BEC520", VA = "0x183BED720")]
		private void Awake()
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000095")]
		[Address(RVA = "0x3BED8A0", Offset = "0x3BEC6A0", VA = "0x183BED8A0")]
		private void FixedUpdate()
		{
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000096")]
		[Address(RVA = "0x3BEDD50", Offset = "0x3BECB50", VA = "0x183BEDD50")]
		public LimitVelocityBurst()
		{
		}

		// Token: 0x04000062 RID: 98
		[Token(Token = "0x4000062")]
		[FieldOffset(Offset = "0x20")]
		public float _maxVelocityDelta;

		// Token: 0x04000063 RID: 99
		[Token(Token = "0x4000063")]
		[FieldOffset(Offset = "0x24")]
		public float _maxAngularVelocity;

		// Token: 0x04000064 RID: 100
		[Token(Token = "0x4000064")]
		[FieldOffset(Offset = "0x28")]
		private float _prevVelocityMag;

		// Token: 0x04000065 RID: 101
		[Token(Token = "0x4000065")]
		[FieldOffset(Offset = "0x2C")]
		private Vector3 _prevVelocity;

		// Token: 0x04000066 RID: 102
		[Token(Token = "0x4000066")]
		[FieldOffset(Offset = "0x38")]
		private Rigidbody _rb;
	}
}
