using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppDummyDll;

namespace TheForest.Utils
{
	// Token: 0x02000038 RID: 56
	[Token(Token = "0x2000038")]
	public static class StringExtensions
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00002280 File Offset: 0x00000480
		[Token(Token = "0x60000B9")]
		[Address(RVA = "0x3BEFD20", Offset = "0x3BEEB20", VA = "0x183BEFD20")]
		public static bool IsNull(this string stringValue)
		{
			return default(bool);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00002298 File Offset: 0x00000498
		[Token(Token = "0x60000BA")]
		[Address(RVA = "0x3BEFD30", Offset = "0x3BEEB30", VA = "0x183BEFD30")]
		public static bool IsEmpty(this string stringValue)
		{
			return default(bool);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000022B0 File Offset: 0x000004B0
		[Token(Token = "0x60000BB")]
		[Address(RVA = "0x1F23DA0", Offset = "0x1F22BA0", VA = "0x181F23DA0")]
		public static bool NullOrEmpty(this string stringValue)
		{
			return default(bool);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000BC")]
		[Address(RVA = "0x3BEFDB0", Offset = "0x3BEEBB0", VA = "0x183BEFDB0")]
		public static string FirstOrDefault(this global::System.Collections.Generic.IEnumerable<string> strings, [global::System.Runtime.InteropServices.Optional] string defaultResult)
		{
			return null;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000BD")]
		[Address(RVA = "0x3BEFDE0", Offset = "0x3BEEBE0", VA = "0x183BEFDE0")]
		public static string FirstNotNull(this global::System.Collections.Generic.IEnumerable<string> strings, [global::System.Runtime.InteropServices.Optional] string defaultResult)
		{
			return null;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000BE")]
		[Address(RVA = "0x3BEFF50", Offset = "0x3BEED50", VA = "0x183BEFF50")]
		public static string FirstNotNullOrEmpty(this global::System.Collections.Generic.IEnumerable<string> strings, [global::System.Runtime.InteropServices.Optional] string defaultResult)
		{
			return null;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000BF")]
		[Address(RVA = "0x3BF00C0", Offset = "0x3BEEEC0", VA = "0x183BF00C0")]
		public static string IfNull(this string stringValue, string defaultResult)
		{
			return null;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x60000C0")]
		[Address(RVA = "0x3BF00D0", Offset = "0x3BEEED0", VA = "0x183BF00D0")]
		public static string IfNullOrEmpty(this string stringValue, string defaultResult)
		{
			return null;
		}

		// Token: 0x04000085 RID: 133
		[Token(Token = "0x4000085")]
		private const string EmptyString = "";
	}
}
