using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace TheForest.Utils.Physics
{
	// Token: 0x02000031 RID: 49
	public class IOnCollisionEnterProxy : Il2CppObjectBase
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x00002DB7 File Offset: 0x00000FB7
		// Note: this type is marked as 'beforefieldinit'.
		static IOnCollisionEnterProxy()
		{
			Il2CppClassPointerStore<IOnCollisionEnterProxy>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils.Physics", "IOnCollisionEnterProxy");
			IOnCollisionEnterProxy.NativeMethodInfoPtr_OnCollisionEnterProxied_Public_Abstract_Virtual_New_Void_Collision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IOnCollisionEnterProxy>.NativeClassPtr, 100663507);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009250 File Offset: 0x00007450
		[CallerCount(0)]
		public unsafe virtual void OnCollisionEnterProxied(Collision col)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(col);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr(this), IOnCollisionEnterProxy.NativeMethodInfoPtr_OnCollisionEnterProxied_Public_Abstract_Virtual_New_Void_Collision_0), IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00002DE6 File Offset: 0x00000FE6
		public IOnCollisionEnterProxy(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x04000122 RID: 290
		private static readonly IntPtr NativeMethodInfoPtr_OnCollisionEnterProxied_Public_Abstract_Virtual_New_Void_Collision_0;
	}
}
