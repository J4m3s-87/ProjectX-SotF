using System;
using System.Runtime.CompilerServices;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200003A RID: 58
	[Token(Token = "0x200003A")]
	public class TriggerTagSensor : MonoBehaviour
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00002082 File Offset: 0x00000282
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x17000007")]
		public string TargetTag
		{
			[Token(Token = "0x60000C5")]
			[Address(RVA = "0x5AAE20", Offset = "0x5A9C20", VA = "0x1805AAE20")]
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			get
			{
				return null;
			}
			[Token(Token = "0x60000C6")]
			[Address(RVA = "0x5AAE30", Offset = "0x5A9C30", VA = "0x1805AAE30")]
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			set
			{
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00002082 File Offset: 0x00000282
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x17000008")]
		public TriggerTagSensor.ITarget Target
		{
			[Token(Token = "0x60000C7")]
			[Address(RVA = "0x5AAE90", Offset = "0x5A9C90", VA = "0x1805AAE90")]
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			get
			{
				return null;
			}
			[Token(Token = "0x60000C8")]
			[Address(RVA = "0x5AA800", Offset = "0x5A9600", VA = "0x1805AA800")]
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			set
			{
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000C9")]
		[Address(RVA = "0x3BF0190", Offset = "0x3BEEF90", VA = "0x183BF0190")]
		private void OnTriggerEnter(Collider other)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000CA")]
		[Address(RVA = "0x3BF02A0", Offset = "0x3BEF0A0", VA = "0x183BF02A0")]
		private void OnTriggerExit(Collider other)
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002050 File Offset: 0x00000250
		[Token(Token = "0x60000CB")]
		[Address(RVA = "0x597410", Offset = "0x596210", VA = "0x180597410")]
		public TriggerTagSensor()
		{
		}

		// Token: 0x0200003B RID: 59
		[Token(Token = "0x200003B")]
		public interface ITarget
		{
			// Token: 0x060000CC RID: 204
			[Token(Token = "0x60000CC")]
			void OnTargetTagTrigerEnter(Collider other);

			// Token: 0x060000CD RID: 205
			[Token(Token = "0x60000CD")]
			void OnTargetTagTrigerExit(Collider other);
		}
	}
}
