using System;
using Il2CppDummyDll;
using UnityEngine;
using UnityEngine.Events;

namespace TheForest.Utils
{
	// Token: 0x0200003C RID: 60
	[Token(Token = "0x200003C")]
	public class VideoStreaming : MonoBehaviour
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000CE")]
		[Address(RVA = "0x597160", Offset = "0x595F60", VA = "0x180597160")]
		public void Play(string path)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000CF")]
		[Address(RVA = "0x597160", Offset = "0x595F60", VA = "0x180597160")]
		public void Stop()
		{
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000022F8 File Offset: 0x000004F8
		[Token(Token = "0x17000009")]
		public bool IsPlaying
		{
			[Token(Token = "0x60000D0")]
			[Address(RVA = "0x632410", Offset = "0x631210", VA = "0x180632410")]
			get
			{
				return default(bool);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00002310 File Offset: 0x00000510
		[Token(Token = "0x1700000A")]
		public bool IsReading
		{
			[Token(Token = "0x60000D1")]
			[Address(RVA = "0x3BF03B0", Offset = "0x3BEF1B0", VA = "0x183BF03B0")]
			get
			{
				return default(bool);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000D2")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public VideoStreaming()
		{
		}

		// Token: 0x0400008B RID: 139
		[Token(Token = "0x400008B")]
		[FieldOffset(Offset = "0x20")]
		public Material _targetMaterial;

		// Token: 0x0400008C RID: 140
		[Token(Token = "0x400008C")]
		[FieldOffset(Offset = "0x28")]
		public Texture _defaultTexture;

		// Token: 0x0400008D RID: 141
		[Token(Token = "0x400008D")]
		[FieldOffset(Offset = "0x30")]
		public UnityEvent _onBeginRead;

		// Token: 0x0400008E RID: 142
		[Token(Token = "0x400008E")]
		[FieldOffset(Offset = "0x38")]
		public UnityEvent _onPlay;

		// Token: 0x0400008F RID: 143
		[Token(Token = "0x400008F")]
		[FieldOffset(Offset = "0x40")]
		public UnityEvent _onStop;

		// Token: 0x04000090 RID: 144
		[Token(Token = "0x4000090")]
		[FieldOffset(Offset = "0x48")]
		public UnityEvent _onClear;

		// Token: 0x04000091 RID: 145
		[Token(Token = "0x4000091")]
		[FieldOffset(Offset = "0x50")]
		private string _lastPlayed;

		// Token: 0x04000092 RID: 146
		[Token(Token = "0x4000092")]
		[FieldOffset(Offset = "0x58")]
		private bool _streamingFromResources;
	}
}
