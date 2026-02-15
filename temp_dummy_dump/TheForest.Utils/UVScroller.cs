using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x02000013 RID: 19
[Token(Token = "0x2000013")]
public class UVScroller : MonoBehaviour
{
	// Token: 0x06000042 RID: 66 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000042")]
	[Address(RVA = "0x3BE7380", Offset = "0x3BE6180", VA = "0x183BE7380")]
	private void Start()
	{
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000043")]
	[Address(RVA = "0x3BE74F0", Offset = "0x3BE62F0", VA = "0x183BE74F0")]
	private void Update()
	{
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000044")]
	[Address(RVA = "0x3BE7810", Offset = "0x3BE6610", VA = "0x183BE7810")]
	private void OnDestroy()
	{
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000045")]
	[Address(RVA = "0x3BE7940", Offset = "0x3BE6740", VA = "0x183BE7940")]
	public UVScroller()
	{
	}

	// Token: 0x04000022 RID: 34
	[Token(Token = "0x4000022")]
	[FieldOffset(Offset = "0x20")]
	public float Speed;

	// Token: 0x04000023 RID: 35
	[Token(Token = "0x4000023")]
	[FieldOffset(Offset = "0x24")]
	public Vector2 ScrollAmount;

	// Token: 0x04000024 RID: 36
	[Token(Token = "0x4000024")]
	[FieldOffset(Offset = "0x2C")]
	public bool AutoWrap;

	// Token: 0x04000025 RID: 37
	[Token(Token = "0x4000025")]
	[FieldOffset(Offset = "0x30")]
	private Material _instancedMaterial;
}
