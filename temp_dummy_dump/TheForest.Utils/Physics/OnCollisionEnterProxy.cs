using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x0200003E RID: 62
	[Token(Token = "0x200003E")]
	public class OnCollisionEnterProxy : MonoBehaviour
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D4")]
		[Address(RVA = "0x3BF03C0", Offset = "0x3BEF1C0", VA = "0x183BF03C0")]
		private void Awake()
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D5")]
		[Address(RVA = "0x3BF0450", Offset = "0x3BEF250", VA = "0x183BF0450")]
		private void OnCollisionEnter(Collision col)
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D6")]
		[Address(RVA = "0x3BF04E0", Offset = "0x3BEF2E0", VA = "0x183BF04E0")]
		private void OnDestroy()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D7")]
		[Address(RVA = "0xA135B0", Offset = "0xA123B0", VA = "0x180A135B0")]
		public void SetBlock(bool value)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D8")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public OnCollisionEnterProxy()
		{
		}

		// Token: 0x04000093 RID: 147
		[Token(Token = "0x4000093")]
		[FieldOffset(Offset = "0x20")]
		private IOnCollisionEnterProxy[] _clients;

		// Token: 0x04000094 RID: 148
		[Token(Token = "0x4000094")]
		[FieldOffset(Offset = "0x28")]
		private bool _block;
	}
}
