using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001C RID: 28
	public static class GameObjectExtensions : global::Il2CppSystem.Object
	{
		// Token: 0x06000102 RID: 258 RVA: 0x000067A8 File Offset: 0x000049A8
		// Note: this type is marked as 'beforefieldinit'.
		static GameObjectExtensions()
		{
			Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "GameObjectExtensions");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr);
			GameObjectExtensions.NativeMethodInfoPtr_SafeName_Public_Static_String_GameObject_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663424);
			GameObjectExtensions.NativeMethodInfoPtr_SetActiveSelfSafe_Public_Static_Void_Component_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663425);
			GameObjectExtensions.NativeMethodInfoPtr_IsNull_Public_Static_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663426);
			GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663427);
			GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Component_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663428);
			GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExtensions>.NativeClassPtr, 100663429);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006850 File Offset: 0x00004A50
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499495, XrefRangeEnd = 1499505, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string SafeName(this GameObject myObject, string defaultResult = null)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(myObject);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(defaultResult);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_SafeName_Public_Static_String_GameObject_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000068A0 File Offset: 0x00004AA0
		[CallerCount(14)]
		[CachedScanResults(RefRangeStart = 1499530, RefRangeEnd = 1499544, XrefRangeStart = 1499505, XrefRangeEnd = 1499530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetActiveSelfSafe(this Component target, bool activeValue)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(target);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref activeValue;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_SetActiveSelfSafe_Public_Static_Void_Component_Boolean_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000068E4 File Offset: 0x00004AE4
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499544, XrefRangeEnd = 1499553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool IsNull(this global::UnityEngine.Object target)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(target);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_IsNull_Public_Static_Boolean_Object_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006928 File Offset: 0x00004B28
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 1499580, RefRangeEnd = 1499584, XrefRangeStart = 1499553, XrefRangeEnd = 1499580, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static GameObject GetRootGameObject(this GameObject target)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(target);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_GameObject_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<GameObject>(intPtr3) : null;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000696C File Offset: 0x00004B6C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499584, XrefRangeEnd = 1499611, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static GameObject GetRootGameObject(this Component target)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(target);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Component_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<GameObject>(intPtr3) : null;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000069B0 File Offset: 0x00004BB0
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1499638, RefRangeEnd = 1499639, XrefRangeStart = 1499611, XrefRangeEnd = 1499638, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static GameObject GetRootGameObject(this Transform target)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(target);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(GameObjectExtensions.NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Transform_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				IntPtr intPtr3 = intPtr;
				return (intPtr3 != 0) ? Il2CppObjectPool.Get<GameObject>(intPtr3) : null;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00002676 File Offset: 0x00000876
		public GameObjectExtensions(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x0400009F RID: 159
		private static readonly IntPtr NativeMethodInfoPtr_SafeName_Public_Static_String_GameObject_String_0;

		// Token: 0x040000A0 RID: 160
		private static readonly IntPtr NativeMethodInfoPtr_SetActiveSelfSafe_Public_Static_Void_Component_Boolean_0;

		// Token: 0x040000A1 RID: 161
		private static readonly IntPtr NativeMethodInfoPtr_IsNull_Public_Static_Boolean_Object_0;

		// Token: 0x040000A2 RID: 162
		private static readonly IntPtr NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_GameObject_0;

		// Token: 0x040000A3 RID: 163
		private static readonly IntPtr NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Component_0;

		// Token: 0x040000A4 RID: 164
		private static readonly IntPtr NativeMethodInfoPtr_GetRootGameObject_Public_Static_GameObject_Transform_0;
	}
}
