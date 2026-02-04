using System;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000013 RID: 19
	public struct ModeGameObjectKey : IEquatable<ModeGameObjectKey>
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00005669 File Offset: 0x00003869
		public readonly string Mode { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00005671 File Offset: 0x00003871
		public readonly int GameObjectInstanceID { get; }

		// Token: 0x06000075 RID: 117 RVA: 0x00005679 File Offset: 0x00003879
		public ModeGameObjectKey(string mode, GameObject gameObject)
		{
			this.Mode = mode;
			this.GameObjectInstanceID = ((gameObject != null) ? gameObject.GetInstanceID() : 0);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000569C File Offset: 0x0000389C
		public bool Equals(ModeGameObjectKey other)
		{
			return string.Equals(this.Mode, other.Mode, StringComparison.OrdinalIgnoreCase) && this.GameObjectInstanceID == other.GameObjectInstanceID;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000056D8 File Offset: 0x000038D8
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is ModeGameObjectKey)
			{
				ModeGameObjectKey modeGameObjectKey = (ModeGameObjectKey)obj;
				flag = this.Equals(modeGameObjectKey);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005704 File Offset: 0x00003904
		public override int GetHashCode()
		{
			string mode = this.Mode;
			return (((mode != null) ? mode.ToUpperInvariant().GetHashCode() : 0) * 397) ^ this.GameObjectInstanceID;
		}
	}
}


