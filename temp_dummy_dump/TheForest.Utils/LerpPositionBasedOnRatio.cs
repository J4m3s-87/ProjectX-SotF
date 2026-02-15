using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000027 RID: 39
	[Token(Token = "0x2000027")]
	public class LerpPositionBasedOnRatio : MonoBehaviour
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008A")]
		[Address(RVA = "0x3BEC8C0", Offset = "0x3BEB6C0", VA = "0x183BEC8C0")]
		private void Update()
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008B")]
		[Address(RVA = "0x3BEC980", Offset = "0x3BEB780", VA = "0x183BEC980")]
		private void OnEnable()
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008C")]
		[Address(RVA = "0x3BEC990", Offset = "0x3BEB790", VA = "0x183BEC990")]
		private void Refresh()
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600008D")]
		[Address(RVA = "0x3BECE30", Offset = "0x3BEBC30", VA = "0x183BECE30")]
		public LerpPositionBasedOnRatio()
		{
		}

		// Token: 0x04000056 RID: 86
		[Token(Token = "0x4000056")]
		[FieldOffset(Offset = "0x20")]
		public Transform _from;

		// Token: 0x04000057 RID: 87
		[Token(Token = "0x4000057")]
		[FieldOffset(Offset = "0x28")]
		public Transform _to;

		// Token: 0x04000058 RID: 88
		[Token(Token = "0x4000058")]
		[FieldOffset(Offset = "0x30")]
		public float _fromAspectRatio;

		// Token: 0x04000059 RID: 89
		[Token(Token = "0x4000059")]
		[FieldOffset(Offset = "0x34")]
		public float _toAspectRatio;

		// Token: 0x0400005A RID: 90
		[Token(Token = "0x400005A")]
		[FieldOffset(Offset = "0x38")]
		public bool _localPosition;

		// Token: 0x0400005B RID: 91
		[Token(Token = "0x400005B")]
		[FieldOffset(Offset = "0x3C")]
		private float _lastAspectRatio;
	}
}
