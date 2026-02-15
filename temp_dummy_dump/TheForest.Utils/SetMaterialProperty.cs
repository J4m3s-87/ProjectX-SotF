using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000035 RID: 53
	[Token(Token = "0x2000035")]
	public class SetMaterialProperty : MonoBehaviour
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000AE")]
		[Address(RVA = "0x3BEF420", Offset = "0x3BEE220", VA = "0x183BEF420")]
		private void Awake()
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000AF")]
		[Address(RVA = "0x3BEF500", Offset = "0x3BEE300", VA = "0x183BEF500")]
		public void SetColor(Color color)
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B0")]
		[Address(RVA = "0x3BEF600", Offset = "0x3BEE400", VA = "0x183BEF600")]
		public void SetFloat(float value)
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B1")]
		[Address(RVA = "0x3BEF700", Offset = "0x3BEE500", VA = "0x183BEF700")]
		public void SetMatrix(Matrix4x4 value)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B2")]
		[Address(RVA = "0x3BEF830", Offset = "0x3BEE630", VA = "0x183BEF830")]
		public void SetTexture(Texture value)
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B3")]
		[Address(RVA = "0x3BEF930", Offset = "0x3BEE730", VA = "0x183BEF930")]
		public void SetVector3(Vector3 value)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000B4")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public SetMaterialProperty()
		{
		}

		// Token: 0x0400007E RID: 126
		[Token(Token = "0x400007E")]
		[FieldOffset(Offset = "0x20")]
		public string _property;

		// Token: 0x0400007F RID: 127
		[Token(Token = "0x400007F")]
		[FieldOffset(Offset = "0x28")]
		public Renderer _renderer;

		// Token: 0x04000080 RID: 128
		[Token(Token = "0x4000080")]
		[FieldOffset(Offset = "0x30")]
		private MaterialPropertyBlock _block;
	}
}
