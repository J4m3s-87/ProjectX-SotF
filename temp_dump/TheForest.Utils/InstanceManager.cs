using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace TheForest.Utils
{
	// Token: 0x0200001D RID: 29
	public static class InstanceManager : global::Il2CppSystem.Object
	{
		// Token: 0x0600010A RID: 266 RVA: 0x000069F4 File Offset: 0x00004BF4
		// Note: this type is marked as 'beforefieldinit'.
		static InstanceManager()
		{
			Il2CppClassPointerStore<InstanceManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "InstanceManager");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InstanceManager>.NativeClassPtr);
			InstanceManager.NativeFieldInfoPtr__sharedInstances = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstanceManager>.NativeClassPtr, "_sharedInstances");
			InstanceManager.NativeMethodInfoPtr_GetSharedInstance_Public_Static_T_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstanceManager>.NativeClassPtr, 100663430);
			InstanceManager.NativeMethodInfoPtr_TryGetValue_Private_Static_Boolean_T_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstanceManager>.NativeClassPtr, 100663431);
			InstanceManager.NativeMethodInfoPtr_CreateInstance_Private_Static_T_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstanceManager>.NativeClassPtr, 100663432);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006A74 File Offset: 0x00004C74
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499639, XrefRangeEnd = 1499655, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static T GetSharedInstance<T>(T source) where T : global::UnityEngine.Object
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				ref IntPtr ptr2 = ref *ptr;
				IntPtr intPtr;
				if (!typeof(T).IsValueType)
				{
					T t = source;
					intPtr = ((t is string) ? IL2CPP.ManagedStringToIl2Cpp(t as string) : IL2CPP.Il2CppObjectBaseToPtr(t as Il2CppObjectBase));
				}
				else
				{
					intPtr = ref source;
				}
				ptr2 = intPtr;
				IntPtr intPtr3;
				IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(InstanceManager.MethodInfoStoreGeneric_GetSharedInstance_Public_Static_T_T_0<T>.Pointer, 0, (void**)ptr, ref intPtr3);
				Il2CppException.RaiseExceptionIfNecessary(intPtr3);
				return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006AE8 File Offset: 0x00004CE8
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1499687, RefRangeEnd = 1499688, XrefRangeStart = 1499655, XrefRangeEnd = 1499687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool TryGetValue<T>(T source, out T result) where T : global::UnityEngine.Object
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				ref IntPtr ptr2 = ref *ptr;
				IntPtr intPtr;
				if (!typeof(T).IsValueType)
				{
					T t = source;
					intPtr = ((t is string) ? IL2CPP.ManagedStringToIl2Cpp(t as string) : IL2CPP.Il2CppObjectBaseToPtr(t as Il2CppObjectBase));
				}
				else
				{
					intPtr = ref source;
				}
				ptr2 = intPtr;
			}
			ref IntPtr ptr3 = ref ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)];
			IntPtr intPtr2;
			IntPtr intPtr3;
			if (!typeof(T).IsValueType)
			{
				intPtr2 = 0;
				intPtr3 = &intPtr2;
			}
			else
			{
				intPtr3 = ref result;
			}
			ptr3 = intPtr3;
			IntPtr intPtr5;
			IntPtr intPtr4 = IL2CPP.il2cpp_runtime_invoke(InstanceManager.MethodInfoStoreGeneric_TryGetValue_Private_Static_Boolean_T_byref_T_0<T>.Pointer, 0, (void**)ptr, ref intPtr5);
			Il2CppException.RaiseExceptionIfNecessary(intPtr5);
			if (!typeof(T).IsValueType)
			{
				IntPtr intPtr6 = intPtr2;
				result = ((intPtr6 == 0) ? null : IL2CPP.PointerToValueGeneric<T>(intPtr6, false, false));
			}
			return *IL2CPP.il2cpp_object_unbox(intPtr4);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006BB0 File Offset: 0x00004DB0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499688, XrefRangeEnd = 1499693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static T CreateInstance<T>(T source) where T : global::UnityEngine.Object
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				ref IntPtr ptr2 = ref *ptr;
				IntPtr intPtr;
				if (!typeof(T).IsValueType)
				{
					T t = source;
					intPtr = ((t is string) ? IL2CPP.ManagedStringToIl2Cpp(t as string) : IL2CPP.Il2CppObjectBaseToPtr(t as Il2CppObjectBase));
				}
				else
				{
					intPtr = ref source;
				}
				ptr2 = intPtr;
				IntPtr intPtr3;
				IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(InstanceManager.MethodInfoStoreGeneric_CreateInstance_Private_Static_T_T_0<T>.Pointer, 0, (void**)ptr, ref intPtr3);
				Il2CppException.RaiseExceptionIfNecessary(intPtr3);
				return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000267F File Offset: 0x0000087F
		public InstanceManager(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006C24 File Offset: 0x00004E24
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00002688 File Offset: 0x00000888
		public unsafe static Dictionary<global::UnityEngine.Object, global::UnityEngine.Object> _sharedInstances
		{
			get
			{
				IntPtr intPtr;
				IL2CPP.il2cpp_field_static_get_value(InstanceManager.NativeFieldInfoPtr__sharedInstances, (void*)(&intPtr));
				IntPtr intPtr2 = intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Dictionary<global::UnityEngine.Object, global::UnityEngine.Object>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(InstanceManager.NativeFieldInfoPtr__sharedInstances, IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x040000A5 RID: 165
		private static readonly IntPtr NativeFieldInfoPtr__sharedInstances;

		// Token: 0x040000A6 RID: 166
		private static readonly IntPtr NativeMethodInfoPtr_GetSharedInstance_Public_Static_T_T_0;

		// Token: 0x040000A7 RID: 167
		private static readonly IntPtr NativeMethodInfoPtr_TryGetValue_Private_Static_Boolean_T_byref_T_0;

		// Token: 0x040000A8 RID: 168
		private static readonly IntPtr NativeMethodInfoPtr_CreateInstance_Private_Static_T_T_0;

		// Token: 0x02000042 RID: 66
		private sealed class MethodInfoStoreGeneric_GetSharedInstance_Public_Static_T_T_0<T>
		{
			// Token: 0x04000174 RID: 372
			internal static IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull(new MethodInfo(IL2CPP.il2cpp_method_get_object(InstanceManager.NativeMethodInfoPtr_GetSharedInstance_Public_Static_T_T_0, Il2CppClassPointerStore<InstanceManager>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Type>(new Type[] { Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
		}

		// Token: 0x02000043 RID: 67
		private sealed class MethodInfoStoreGeneric_TryGetValue_Private_Static_Boolean_T_byref_T_0<T>
		{
			// Token: 0x04000175 RID: 373
			internal static IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull(new MethodInfo(IL2CPP.il2cpp_method_get_object(InstanceManager.NativeMethodInfoPtr_TryGetValue_Private_Static_Boolean_T_byref_T_0, Il2CppClassPointerStore<InstanceManager>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Type>(new Type[] { Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
		}

		// Token: 0x02000044 RID: 68
		private sealed class MethodInfoStoreGeneric_CreateInstance_Private_Static_T_T_0<T>
		{
			// Token: 0x04000176 RID: 374
			internal static IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull(new MethodInfo(IL2CPP.il2cpp_method_get_object(InstanceManager.NativeMethodInfoPtr_CreateInstance_Private_Static_T_T_0, Il2CppClassPointerStore<InstanceManager>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Type>(new Type[] { Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
		}
	}
}
