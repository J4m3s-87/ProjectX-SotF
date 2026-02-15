using System;
using System.Collections.Generic;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000031 RID: 49
	[Token(Token = "0x2000031")]
	public class ResourceLoader : MonoBehaviour
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A4")]
		[Address(RVA = "0x3BEEB10", Offset = "0x3BED910", VA = "0x183BEEB10")]
		private void OnEnable()
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A5")]
		[Address(RVA = "0x3BEECA0", Offset = "0x3BEDAA0", VA = "0x183BEECA0")]
		private void OnDisable()
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A6")]
		[Address(RVA = "0x3BEEDF0", Offset = "0x3BEDBF0", VA = "0x183BEEDF0")]
		public void AssetLoad()
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A7")]
		[Address(RVA = "0x3BEF030", Offset = "0x3BEDE30", VA = "0x183BEF030")]
		public void AssetUnload(bool resourceUnload)
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A8")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public ResourceLoader()
		{
		}

		// Token: 0x04000074 RID: 116
		[Token(Token = "0x4000074")]
		[FieldOffset(Offset = "0x20")]
		public ResourceLoader.AssetTypes _type;

		// Token: 0x04000075 RID: 117
		[Token(Token = "0x4000075")]
		[FieldOffset(Offset = "0x28")]
		public string _assetPath;

		// Token: 0x04000076 RID: 118
		[Token(Token = "0x4000076")]
		[FieldOffset(Offset = "0x30")]
		public global::UnityEngine.Object _target;

		// Token: 0x04000077 RID: 119
		[Token(Token = "0x4000077")]
		[FieldOffset(Offset = "0x38")]
		private global::UnityEngine.Object _asset;

		// Token: 0x04000078 RID: 120
		[Token(Token = "0x4000078")]
		[FieldOffset(Offset = "0x0")]
		private static global::System.Collections.Generic.Dictionary<string, int> InUseAssetsCounters;

		// Token: 0x02000032 RID: 50
		[Token(Token = "0x2000032")]
		public enum AssetTypes
		{
			// Token: 0x0400007A RID: 122
			[Token(Token = "0x400007A")]
			Mesh,
			// Token: 0x0400007B RID: 123
			[Token(Token = "0x400007B")]
			Texture
		}
	}
}
