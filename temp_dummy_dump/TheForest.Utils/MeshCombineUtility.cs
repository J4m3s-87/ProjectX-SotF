using System;
using Il2CppDummyDll;
using UnityEngine;

// Token: 0x0200000C RID: 12
[Token(Token = "0x200000C")]
public class MeshCombineUtility
{
	// Token: 0x06000032 RID: 50 RVA: 0x00002082 File Offset: 0x00000282
	[Token(Token = "0x6000032")]
	[Address(RVA = "0x3BE5240", Offset = "0x3BE4040", VA = "0x183BE5240")]
	public static Mesh Combine(MeshCombineUtility.MeshInstance[] combines, bool generateStrips)
	{
		return null;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000033")]
	[Address(RVA = "0x3BE6570", Offset = "0x3BE5370", VA = "0x183BE6570")]
	private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000034")]
	[Address(RVA = "0x3BE6710", Offset = "0x3BE5510", VA = "0x183BE6710")]
	private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000035")]
	[Address(RVA = "0x3BE6910", Offset = "0x3BE5710", VA = "0x183BE6910")]
	private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000036")]
	[Address(RVA = "0x3BE6990", Offset = "0x3BE5790", VA = "0x183BE6990")]
	private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000037")]
	[Address(RVA = "0x3BE6A00", Offset = "0x3BE5800", VA = "0x183BE6A00")]
	private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002050 File Offset: 0x00000250
	[Token(Token = "0x6000038")]
	[Address(RVA = "0x597160", Offset = "0x595F60", VA = "0x180597160")]
	public MeshCombineUtility()
	{
	}

	// Token: 0x0200000D RID: 13
	[Token(Token = "0x200000D")]
	public struct MeshInstance
	{
		// Token: 0x04000013 RID: 19
		[Token(Token = "0x4000013")]
		[FieldOffset(Offset = "0x0")]
		public Mesh mesh;

		// Token: 0x04000014 RID: 20
		[Token(Token = "0x4000014")]
		[FieldOffset(Offset = "0x8")]
		public int subMeshIndex;

		// Token: 0x04000015 RID: 21
		[Token(Token = "0x4000015")]
		[FieldOffset(Offset = "0xC")]
		public Matrix4x4 transform;
	}
}
