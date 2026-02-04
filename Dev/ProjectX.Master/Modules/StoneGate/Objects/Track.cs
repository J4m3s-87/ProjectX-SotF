using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000014 RID: 20
	internal class Track
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000573C File Offset: 0x0000393C
		public static GameObject FindGo(BoltEntity entity)
		{
			bool flag = Track.spawendStoneGates.ContainsKey(entity);
			GameObject gameObject;
			if (flag)
			{
				gameObject = Track.spawendStoneGates[entity];
			}
			else
			{
				gameObject = null;
			}
			return gameObject;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005770 File Offset: 0x00003970
		public static BoltEntity FindEntity(GameObject go)
		{
			foreach (KeyValuePair<BoltEntity, GameObject> keyValuePair in Track.spawendStoneGates)
			{
				bool flag = keyValuePair.Value == go;
				if (flag)
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x04000034 RID: 52
		public static Dictionary<BoltEntity, GameObject> spawendStoneGates = new Dictionary<BoltEntity, GameObject>();
	}
}


