using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000042 RID: 66
	[Token(Token = "0x2000042")]
	public class OnCollisionStayProxy : MonoBehaviour
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000E1")]
		[Address(RVA = "0x3BF0710", Offset = "0x3BEF510", VA = "0x183BF0710")]
		private void Awake()
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000E2")]
		[Address(RVA = "0x3BF07A0", Offset = "0x3BEF5A0", VA = "0x183BF07A0")]
		private void OnCollisionStay(Collision col)
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000E3")]
		[Address(RVA = "0x3BF0850", Offset = "0x3BEF650", VA = "0x183BF0850")]
		private void OnDestroy()
		{
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000E4")]
		[Address(RVA = "0xA135B0", Offset = "0xA123B0", VA = "0x180A135B0")]
		public void SetBlock(bool value)
		{
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000E5")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public OnCollisionStayProxy()
		{
		}

		// Token: 0x04000097 RID: 151
		[Token(Token = "0x4000097")]
		[FieldOffset(Offset = "0x20")]
		private IOnCollisionStayProxy[] _clients;

		// Token: 0x04000098 RID: 152
		[Token(Token = "0x4000098")]
		[FieldOffset(Offset = "0x28")]
		private bool _block;
	}
}
