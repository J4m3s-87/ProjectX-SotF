using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000035 RID: 53
	public class IOnCollisionStayProxy : Il2CppObjectBase
	{
		// Token: 0x06000205 RID: 517 RVA: 0x00002EAD File Offset: 0x000010AD
		// Note: this type is marked as 'beforefieldinit'.
		static IOnCollisionStayProxy()
		{
			Il2CppClassPointerStore<IOnCollisionStayProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "IOnCollisionStayProxy");
			IOnCollisionStayProxy.NativeMethodInfoPtr_OnCollisionStayProxied_Public_Abstract_Virtual_New_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IOnCollisionStayProxy>.NativeClassPtr, 100663520);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000097C4 File Offset: 0x000079C4
		[CallerCount(0)]
		public unsafe virtual void OnCollisionStayProxied(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr(this), IOnCollisionStayProxy.NativeMethodInfoPtr_OnCollisionStayProxied_Public_Abstract_Virtual_New_Void_Collision_0), IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00002EDC File Offset: 0x000010DC
		public IOnCollisionStayProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000133 RID: 307
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionStayProxied_Public_Abstract_Virtual_New_Void_Collision_0;
	}
}
