using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000036 RID: 54
	[Token(Token = "0x2000036")]
	[global::System.Serializable]
	public class SkewTransform
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B5")]
		[Address(RVA = "0x3BEFA70", Offset = "0x3BEE870", VA = "0x183BEFA70")]
		public void SetSkew(Transform skewer, Transform skewee, float angle)
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B6")]
		[Address(RVA = "0x3BEFCC0", Offset = "0x3BEEAC0", VA = "0x183BEFCC0")]
		public SkewTransform()
		{
		}

		// Token: 0x04000081 RID: 129
		[Token(Token = "0x4000081")]
		[FieldOffset(Offset = "0x10")]
		public Vector3 _skewerScale;

		// Token: 0x04000082 RID: 130
		[Token(Token = "0x4000082")]
		[FieldOffset(Offset = "0x20")]
		public AnimationCurve _skeweeScaleY;

		// Token: 0x04000083 RID: 131
		[Token(Token = "0x4000083")]
		[FieldOffset(Offset = "0x28")]
		public AnimationCurve _skeweeScaleZ;

		// Token: 0x04000084 RID: 132
		[Token(Token = "0x4000084")]
		[FieldOffset(Offset = "0x30")]
		public AnimationCurve _skeweeRotX;
	}
}
