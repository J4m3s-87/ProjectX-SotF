using System;
using System.Collections.Generic;
using Il2CppDummyDll;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x02000026 RID: 38
	[Token(Token = "0x2000026")]
	public static class InstanceManager
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000086")]
		public static T GetSharedInstance<T>(T source) where T : global::UnityEngine.Object
		{
			return null;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002220 File Offset: 0x00000420
		[Token(Token = "0x6000087")]
		private static bool TryGetValue<T>(T source, out T result) where T : global::UnityEngine.Object
		{
			return default(bool);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002082 File Offset: 0x00000282
		[Token(Token = "0x6000088")]
		private static T CreateInstance<T>(T source) where T : global::UnityEngine.Object
		{
			return null;
		}

		// Token: 0x04000055 RID: 85
		[Token(Token = "0x4000055")]
		[FieldOffset(Offset = "0x0")]
		public static global::System.Collections.Generic.Dictionary<global::UnityEngine.Object, global::UnityEngine.Object> _sharedInstances;
	}
}
