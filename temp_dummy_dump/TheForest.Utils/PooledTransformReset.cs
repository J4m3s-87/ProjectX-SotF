using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000030 RID: 48
	[Token(Token = "0x2000030")]
	public class PooledTransformReset : MonoBehaviour
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A1")]
		[Address(RVA = "0x3BEE820", Offset = "0x3BED620", VA = "0x183BEE820")]
		private void Awake()
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A2")]
		[Address(RVA = "0x3BEE9A0", Offset = "0x3BED7A0", VA = "0x183BEE9A0")]
		private void OnSpawned()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000A3")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public PooledTransformReset()
		{
		}

		// Token: 0x04000072 RID: 114
		[Token(Token = "0x4000072")]
		[FieldOffset(Offset = "0x20")]
		private Vector3 _localPosition;

		// Token: 0x04000073 RID: 115
		[Token(Token = "0x4000073")]
		[FieldOffset(Offset = "0x2C")]
		private Quaternion _localRotation;
	}
}
