using System;
using System.Runtime.InteropServices;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000025 RID: 37
	[Token(Token = "0x2000025")]
	public static class GameObjectExtensions
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000080")]
		[Address(RVA = "0x3BEBB50", Offset = "0x3BEA950", VA = "0x183BEBB50")]
		public static string SafeName(this GameObject myObject, [global::System.Runtime.InteropServices.Optional] string defaultResult)
		{
			return null;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000081")]
		[Address(RVA = "0x3BEBC30", Offset = "0x3BEAA30", VA = "0x183BEBC30")]
		public static void SetActiveSelfSafe(this Component target, bool activeValue)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002208 File Offset: 0x00000408
		[Token(Token = "0x6000082")]
		[Address(RVA = "0x3BEBE90", Offset = "0x3BEAC90", VA = "0x183BEBE90")]
		public static bool IsNull(this global::UnityEngine.Object target)
		{
			return default(bool);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000083")]
		[Address(RVA = "0x3BEBF50", Offset = "0x3BEAD50", VA = "0x183BEBF50")]
		public static GameObject GetRootGameObject(this GameObject target)
		{
			return null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000084")]
		[Address(RVA = "0x3BEC230", Offset = "0x3BEB030", VA = "0x183BEC230")]
		public static GameObject GetRootGameObject(this Component target)
		{
			return null;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000085")]
		[Address(RVA = "0x3BEC510", Offset = "0x3BEB310", VA = "0x183BEC510")]
		public static GameObject GetRootGameObject(this Transform target)
		{
			return null;
		}
	}
}
