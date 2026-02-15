using System;
using System.Collections.Generic;
using Il2CppDummyDll;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheForest.Utils
{
	// Token: 0x0200001B RID: 27
	[Token(Token = "0x200001B")]
	[RequireComponent(typeof(Camera))]
	public class DepthBufferGrabCommand : MonoBehaviour
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000056")]
		[Address(RVA = "0x3BEA1C0", Offset = "0x3BE8FC0", VA = "0x183BEA1C0")]
		private void Start()
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000057")]
		[Address(RVA = "0x3BEA4A0", Offset = "0x3BE92A0", VA = "0x183BEA4A0")]
		private void Update()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000058")]
		[Address(RVA = "0x3BEA4B0", Offset = "0x3BE92B0", VA = "0x183BEA4B0")]
		public static void AddBinding(Camera cam, string name)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x6000059")]
		[Address(RVA = "0x3BEA630", Offset = "0x3BE9430", VA = "0x183BEA630")]
		public static void RemoveBinding(Camera cam, string name)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000021D8 File Offset: 0x000003D8
		[Token(Token = "0x600005A")]
		[Address(RVA = "0x3BEA7A0", Offset = "0x3BE95A0", VA = "0x183BEA7A0")]
		public static bool HasCamera(Camera cam)
		{
			return default(bool);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600005B")]
		[Address(RVA = "0x3BEA860", Offset = "0x3BE9660", VA = "0x183BEA860")]
		private void CreateCommand()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600005C")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public DepthBufferGrabCommand()
		{
		}

		// Token: 0x04000030 RID: 48
		[Token(Token = "0x4000030")]
		[FieldOffset(Offset = "0x20")]
		public Shader depthCopySdr;

		// Token: 0x04000031 RID: 49
		[Token(Token = "0x4000031")]
		[FieldOffset(Offset = "0x0")]
		private static global::System.Collections.Generic.Dictionary<Camera, DepthBufferGrabCommand.CommandData> m_data;

		// Token: 0x04000032 RID: 50
		[Token(Token = "0x4000032")]
		[FieldOffset(Offset = "0x28")]
		private Camera m_camera;

		// Token: 0x04000033 RID: 51
		[Token(Token = "0x4000033")]
		[FieldOffset(Offset = "0x30")]
		private Material m_depthCopyMat;

		// Token: 0x0200001C RID: 28
		[Token(Token = "0x200001C")]
		public class CommandData
		{
			// Token: 0x0600005E RID: 94 RVA: 0x00002050 File Offset: 0x00000250
			[Token(Token = "0x600005E")]
			[Address(RVA = "0x3BEAF00", Offset = "0x3BE9D00", VA = "0x183BEAF00")]
			public CommandData()
			{
			}

			// Token: 0x04000034 RID: 52
			[Token(Token = "0x4000034")]
			[FieldOffset(Offset = "0x10")]
			public HashSet<string> bindings;

			// Token: 0x04000035 RID: 53
			[Token(Token = "0x4000035")]
			[FieldOffset(Offset = "0x18")]
			public CommandBuffer command;

			// Token: 0x04000036 RID: 54
			[Token(Token = "0x4000036")]
			[FieldOffset(Offset = "0x20")]
			public int width;

			// Token: 0x04000037 RID: 55
			[Token(Token = "0x4000037")]
			[FieldOffset(Offset = "0x24")]
			public int height;

			// Token: 0x04000038 RID: 56
			[Token(Token = "0x4000038")]
			[FieldOffset(Offset = "0x28")]
			public bool recreate;
		}
	}
}
