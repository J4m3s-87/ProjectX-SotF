using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000022 RID: 34
	[Token(Token = "0x2000022")]
	public class DisableForPlatform : MonoBehaviour
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000075")]
		[Address(RVA = "0x3BEAFB0", Offset = "0x3BE9DB0", VA = "0x183BEAFB0")]
		private void Reset()
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000076")]
		[Address(RVA = "0x3BEB910", Offset = "0x3BEA710", VA = "0x183BEB910")]
		private void Awake()
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000077")]
		[Address(RVA = "0x3BEB920", Offset = "0x3BEA720", VA = "0x183BEB920")]
		private void Start()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000078")]
		[Address(RVA = "0x3BEB930", Offset = "0x3BEA730", VA = "0x183BEB930")]
		private void OnEnable()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000079")]
		[Address(RVA = "0x3BEB940", Offset = "0x3BEA740", VA = "0x183BEB940")]
		private void OnDisable()
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007A")]
		[Address(RVA = "0x3BEB950", Offset = "0x3BEA750", VA = "0x183BEB950")]
		private void OnDestroy()
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007B")]
		[Address(RVA = "0x3BEB960", Offset = "0x3BEA760", VA = "0x183BEB960")]
		private void CheckPlatform()
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600007C")]
		[Address(RVA = "0x3BEB270", Offset = "0x3BEA070", VA = "0x183BEB270")]
		public DisableForPlatform()
		{
		}

		// Token: 0x04000049 RID: 73
		[Token(Token = "0x4000049")]
		[FieldOffset(Offset = "0x20")]
		public RuntimePlatform _platform;

		// Token: 0x0400004A RID: 74
		[Token(Token = "0x400004A")]
		[FieldOffset(Offset = "0x24")]
		public DisableForPlatform.Events _event;

		// Token: 0x0400004B RID: 75
		[Token(Token = "0x400004B")]
		[FieldOffset(Offset = "0x28")]
		public GameObject _target;

		// Token: 0x0400004C RID: 76
		[Token(Token = "0x400004C")]
		[FieldOffset(Offset = "0x30")]
		public bool _inverse;

		// Token: 0x02000023 RID: 35
		[Token(Token = "0x2000023")]
		public enum Events
		{
			// Token: 0x0400004E RID: 78
			[Token(Token = "0x400004E")]
			Awake,
			// Token: 0x0400004F RID: 79
			[Token(Token = "0x400004F")]
			Start,
			// Token: 0x04000050 RID: 80
			[Token(Token = "0x4000050")]
			OnEnable,
			// Token: 0x04000051 RID: 81
			[Token(Token = "0x4000051")]
			OnDisable,
			// Token: 0x04000052 RID: 82
			[Token(Token = "0x4000052")]
			OnDestroy
		}
	}
}
