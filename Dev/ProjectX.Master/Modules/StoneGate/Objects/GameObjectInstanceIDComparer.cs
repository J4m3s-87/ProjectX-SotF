using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000012 RID: 18
	public class GameObjectInstanceIDComparer : IEqualityComparer<GameObject>
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000055F4 File Offset: 0x000037F4
		public bool Equals(GameObject x, GameObject y)
		{
			bool flag = x == y;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = x == null || y == null;
				flag2 = !flag3 && x.GetInstanceID() == y.GetInstanceID();
			}
			return flag2;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005634 File Offset: 0x00003834
		public int GetHashCode(GameObject obj)
		{
			bool flag = obj == null;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				num = obj.GetInstanceID().GetHashCode();
			}
			return num;
		}
	}
}


