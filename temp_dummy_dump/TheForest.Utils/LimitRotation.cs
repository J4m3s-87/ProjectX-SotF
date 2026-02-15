using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x02000007 RID: 7
[Token(Token = "0x2000007")]
public class LimitRotation : MonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000021")]
	[Address(RVA = "0x3BE3BF0", Offset = "0x3BE29F0", VA = "0x183BE3BF0")]
	private void Awake()
	{
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000022")]
	[Address(RVA = "0x3BE3D20", Offset = "0x3BE2B20", VA = "0x183BE3D20")]
	private void Update()
	{
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000020E8 File Offset: 0x000002E8
	[Token(Token = "0x6000023")]
	[Address(RVA = "0x3BE40B0", Offset = "0x3BE2EB0", VA = "0x183BE40B0")]
	private Vector3 ClampRotation(Vector3 rotationAngle)
	{
		return default(Vector3);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000024")]
	[Address(RVA = "0x3BE4110", Offset = "0x3BE2F10", VA = "0x183BE4110")]
	public LimitRotation()
	{
	}

	// Token: 0x04000009 RID: 9
	[Token(Token = "0x4000009")]
	[FieldOffset(Offset = "0x20")]
	[SerializeField]
	private float m_MaxRotationAngle;

	// Token: 0x0400000A RID: 10
	[Token(Token = "0x400000A")]
	[FieldOffset(Offset = "0x28")]
	[SerializeField]
	private Rigidbody m_RigidBody;
}
