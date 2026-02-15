using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000033 RID: 51
	public class IOnCollisionExitProxy : Il2CppObjectBase
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x00002E32 File Offset: 0x00001032
		// Note: this type is marked as 'beforefieldinit'.
		static IOnCollisionExitProxy()
		{
			Il2CppClassPointerStore<IOnCollisionExitProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "IOnCollisionExitProxy");
			IOnCollisionExitProxy.NativeMethodInfoPtr_OnCollisionExitProxied_Public_Abstract_Virtual_New_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IOnCollisionExitProxy>.NativeClassPtr, 100663513);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000094DC File Offset: 0x000076DC
		[CallerCount(0)]
		public unsafe virtual void OnCollisionExitProxied(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr(this), IOnCollisionExitProxy.NativeMethodInfoPtr_OnCollisionExitProxied_Public_Abstract_Virtual_New_Void_Collision_0), IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00002E61 File Offset: 0x00001061
		public IOnCollisionExitProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x0400012A RID: 298
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionExitProxied_Public_Abstract_Virtual_New_Void_Collision_0;
	}
}
