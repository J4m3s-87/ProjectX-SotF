using System;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000017 RID: 23
	[Token(Token = "0x2000017")]
	public class ChangeColorOnHover : MonoBehaviour
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600004A")]
		[Address(RVA = "0x3BE99B0", Offset = "0x3BE87B0", VA = "0x183BE99B0")]
		private void OnDisable()
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600004B")]
		[Address(RVA = "0x3BE99C0", Offset = "0x3BE87C0", VA = "0x183BE99C0")]
		private void OnMouseExitCollider()
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600004C")]
		[Address(RVA = "0x3BE99E0", Offset = "0x3BE87E0", VA = "0x183BE99E0")]
		private void OnMouseOverCollider()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600004D")]
		[Address(RVA = "0x3BE9A00", Offset = "0x3BE8800", VA = "0x183BE9A00")]
		private void UpdateColor()
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x600004E")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public ChangeColorOnHover()
		{
		}

		// Token: 0x0400002B RID: 43
		[Token(Token = "0x400002B")]
		[FieldOffset(Offset = "0x20")]
		public MeshRenderer rendererToUse;

		// Token: 0x0400002C RID: 44
		[Token(Token = "0x400002C")]
		[FieldOffset(Offset = "0x28")]
		public Color defaultColor;

		// Token: 0x0400002D RID: 45
		[Token(Token = "0x400002D")]
		[FieldOffset(Offset = "0x38")]
		public Color highlightColor;

		// Token: 0x0400002E RID: 46
		[Token(Token = "0x400002E")]
		[FieldOffset(Offset = "0x48")]
		private bool highlighted;

		// Token: 0x0400002F RID: 47
		[Token(Token = "0x400002F")]
		[FieldOffset(Offset = "0x50")]
		private MaterialPropertyBlock MyMatPropertyBlock;
	}
}
