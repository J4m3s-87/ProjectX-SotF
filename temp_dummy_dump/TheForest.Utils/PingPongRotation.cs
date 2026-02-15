using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200002E RID: 46
	[Token(Token = "0x200002E")]
	public class PingPongRotation : MonoBehaviour
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600009C")]
		[Address(RVA = "0x3BEE470", Offset = "0x3BED270", VA = "0x183BEE470")]
		private void OnEnable()
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600009D")]
		[Address(RVA = "0x3BEE480", Offset = "0x3BED280", VA = "0x183BEE480")]
		private void Update()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600009E")]
		[Address(RVA = "0x3BEE640", Offset = "0x3BED440", VA = "0x183BEE640")]
		public PingPongRotation()
		{
		}

		// Token: 0x04000069 RID: 105
		[Token(Token = "0x4000069")]
		[FieldOffset(Offset = "0x20")]
		public float _duration;

		// Token: 0x0400006A RID: 106
		[Token(Token = "0x400006A")]
		[FieldOffset(Offset = "0x24")]
		public Vector3 _fromRotation;

		// Token: 0x0400006B RID: 107
		[Token(Token = "0x400006B")]
		[FieldOffset(Offset = "0x30")]
		public Vector3 _toRotation;

		// Token: 0x0400006C RID: 108
		[Token(Token = "0x400006C")]
		[FieldOffset(Offset = "0x3C")]
		public bool _resetOnEnable;

		// Token: 0x0400006D RID: 109
		[Token(Token = "0x400006D")]
		[FieldOffset(Offset = "0x40")]
		public Space _space;

		// Token: 0x0400006E RID: 110
		[Token(Token = "0x400006E")]
		[FieldOffset(Offset = "0x44")]
		private float _alpha;
	}
}
