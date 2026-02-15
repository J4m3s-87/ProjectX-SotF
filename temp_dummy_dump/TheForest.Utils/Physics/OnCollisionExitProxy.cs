using System;
using System.Collections;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000040 RID: 64
	[Token(Token = "0x2000040")]
	public class OnCollisionExitProxy : MonoBehaviour
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000DA")]
		[Address(RVA = "0x3BF05F0", Offset = "0x3BEF3F0", VA = "0x183BF05F0")]
		private void Awake()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000DB")]
		[Address(RVA = "0x3BF0680", Offset = "0x3BEF480", VA = "0x183BF0680")]
		private void OnCollisionExit(Collision col)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000DC")]
		[Address(RVA = "0x3BF04E0", Offset = "0x3BEF2E0", VA = "0x183BF04E0")]
		private void OnDestroy()
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000DD")]
		public T[] ConvertToArray<T>(global::System.Collections.IList list)
		{
			return null;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000DE")]
		[Address(RVA = "0xA135B0", Offset = "0xA123B0", VA = "0x180A135B0")]
		public void SetBlock(bool value)
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000DF")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public OnCollisionExitProxy()
		{
		}

		// Token: 0x04000095 RID: 149
		[Token(Token = "0x4000095")]
		[FieldOffset(Offset = "0x20")]
		private IOnCollisionExitProxy[] _clients;

		// Token: 0x04000096 RID: 150
		[Token(Token = "0x4000096")]
		[FieldOffset(Offset = "0x28")]
		private bool _block;
	}
}
