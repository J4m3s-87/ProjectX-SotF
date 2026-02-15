using System;
using System.Collections;
using Il2CppDummyDll;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheForest.Utils
{
	// Token: 0x02000020 RID: 32
	[Token(Token = "0x2000020")]
	public class DisableAfterDelay : MonoBehaviour
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000069")]
		[Address(RVA = "0x3BEB400", Offset = "0x3BEA200", VA = "0x183BEB400")]
		public void Restart(float duration)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600006A")]
		[Address(RVA = "0x3BEB410", Offset = "0x3BEA210", VA = "0x183BEB410")]
		private void Start()
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600006B")]
		[Address(RVA = "0x3BEB530", Offset = "0x3BEA330", VA = "0x183BEB530")]
		private void OnEnable()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600006C")]
		[Address(RVA = "0x3BEB540", Offset = "0x3BEA340", VA = "0x183BEB540")]
		private void OnDisable()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x600006D")]
		[Address(RVA = "0x3BEB6A0", Offset = "0x3BEA4A0", VA = "0x183BEB6A0")]
		private global::System.Collections.IEnumerator Disable()
		{
			return null;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600006E")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public DisableAfterDelay()
		{
		}

		// Token: 0x04000044 RID: 68
		[Token(Token = "0x4000044")]
		[FieldOffset(Offset = "0x20")]
		[FormerlySerializedAs("delay")]
		[SerializeField]
		private float _delay;

		// Token: 0x04000045 RID: 69
		[Token(Token = "0x4000045")]
		[FieldOffset(Offset = "0x28")]
		private Coroutine _activeCoroutine;
	}
}
