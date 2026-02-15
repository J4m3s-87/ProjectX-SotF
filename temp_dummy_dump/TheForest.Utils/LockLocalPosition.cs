using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x0200000A RID: 10
[Token(Token = "0x200000A")]
public class LockLocalPosition : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600002D")]
	[Address(RVA = "0x3BE4510", Offset = "0x3BE3310", VA = "0x183BE4510")]
	private void OnEnable()
	{
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600002E")]
	[Address(RVA = "0x3BE4630", Offset = "0x3BE3430", VA = "0x183BE4630")]
	private void LateUpdate()
	{
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x600002F")]
	[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
	public LockLocalPosition()
	{
	}

	// Token: 0x04000011 RID: 17
	[Token(Token = "0x4000011")]
	[FieldOffset(Offset = "0x20")]
	private Vector3 localPos;

	// Token: 0x04000012 RID: 18
	[Token(Token = "0x4000012")]
	[FieldOffset(Offset = "0x30")]
	private Transform _transform;
}
