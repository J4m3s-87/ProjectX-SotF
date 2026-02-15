using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000016 RID: 22
	[Token(Token = "0x2000016")]
	public static class CameraEx
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002130 File Offset: 0x00000330
		[Token(Token = "0x6000048")]
		[Address(RVA = "0x3BE7AF0", Offset = "0x3BE68F0", VA = "0x183BE7AF0")]
		public static Rect GetScreenRectOf(this Camera cam, Collider collider)
		{
			return default(Rect);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002148 File Offset: 0x00000348
		[Token(Token = "0x6000049")]
		[Address(RVA = "0x3BE88B0", Offset = "0x3BE76B0", VA = "0x183BE88B0")]
		public static Rect GetScreenRectOf(this Camera cam, Renderer renderer)
		{
			return default(Rect);
		}
	}
}
