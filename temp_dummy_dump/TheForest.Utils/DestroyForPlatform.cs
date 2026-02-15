using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001D RID: 29
	[Token(Token = "0x200001D")]
	public class DestroyForPlatform : MonoBehaviour
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600005F")]
		[Address(RVA = "0x3BEAFB0", Offset = "0x3BE9DB0", VA = "0x183BEAFB0")]
		private void Reset()
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000060")]
		[Address(RVA = "0x3BEB060", Offset = "0x3BE9E60", VA = "0x183BEB060")]
		private void Awake()
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000061")]
		[Address(RVA = "0x3BEB070", Offset = "0x3BE9E70", VA = "0x183BEB070")]
		private void Start()
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000062")]
		[Address(RVA = "0x3BEB080", Offset = "0x3BE9E80", VA = "0x183BEB080")]
		private void OnEnable()
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000063")]
		[Address(RVA = "0x3BEB090", Offset = "0x3BE9E90", VA = "0x183BEB090")]
		private void OnDisable()
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000064")]
		[Address(RVA = "0x3BEB0A0", Offset = "0x3BE9EA0", VA = "0x183BEB0A0")]
		private void OnDestroy()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000065")]
		[Address(RVA = "0x3BEB0B0", Offset = "0x3BE9EB0", VA = "0x183BEB0B0")]
		private void CheckPlatform()
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000066")]
		[Address(RVA = "0x3BEB270", Offset = "0x3BEA070", VA = "0x183BEB270")]
		public DestroyForPlatform()
		{
		}

		// Token: 0x04000039 RID: 57
		[Token(Token = "0x4000039")]
		[FieldOffset(Offset = "0x20")]
		public RuntimePlatform _platform;

		// Token: 0x0400003A RID: 58
		[Token(Token = "0x400003A")]
		[FieldOffset(Offset = "0x24")]
		public DestroyForPlatform.Events _event;

		// Token: 0x0400003B RID: 59
		[Token(Token = "0x400003B")]
		[FieldOffset(Offset = "0x28")]
		public GameObject _target;

		// Token: 0x0400003C RID: 60
		[Token(Token = "0x400003C")]
		[FieldOffset(Offset = "0x30")]
		public bool _inverse;

		// Token: 0x0200001E RID: 30
		[Token(Token = "0x200001E")]
		public enum Events
		{
			// Token: 0x0400003E RID: 62
			[Token(Token = "0x400003E")]
			Awake,
			// Token: 0x0400003F RID: 63
			[Token(Token = "0x400003F")]
			Start,
			// Token: 0x04000040 RID: 64
			[Token(Token = "0x4000040")]
			OnEnable,
			// Token: 0x04000041 RID: 65
			[Token(Token = "0x4000041")]
			OnDisable,
			// Token: 0x04000042 RID: 66
			[Token(Token = "0x4000042")]
			OnDestroy
		}
	}
}
