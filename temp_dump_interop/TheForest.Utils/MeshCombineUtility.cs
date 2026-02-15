using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class MeshCombineUtility : global::Il2CppSystem.Object
{
	// Token: 0x06000059 RID: 89 RVA: 0x000045F8 File Offset: 0x000027F8
	// Note: this type is marked as 'beforefieldinit'.
	static MeshCombineUtility()
	{
		Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "MeshCombineUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr);
		MeshCombineUtility.NativeMethodInfoPtr_Combine_Public_Static_Mesh_Il2CppReferenceArray_1_MeshInstance_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663346);
		MeshCombineUtility.NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663347);
		MeshCombineUtility.NativeMethodInfoPtr_CopyNormal_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663348);
		MeshCombineUtility.NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663349);
		MeshCombineUtility.NativeMethodInfoPtr_CopyColors_Private_Static_Void_Int32_Il2CppStructArray_1_Color_Il2CppStructArray_1_Color_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663350);
		MeshCombineUtility.NativeMethodInfoPtr_CopyTangents_Private_Static_Void_Int32_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector4_byref_Int32_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663351);
		MeshCombineUtility.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, 100663352);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000046B4 File Offset: 0x000028B4
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498732, XrefRangeEnd = 1498902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh Combine(Il2CppReferenceArray<MeshCombineUtility.MeshInstance> combines, bool generateStrips)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr(combines);
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref generateStrips;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_Combine_Public_Static_Mesh_Il2CppReferenceArray_1_MeshInstance_Boolean_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		IntPtr intPtr3 = intPtr;
		return (intPtr3 != 0) ? Il2CppObjectPool.Get<Mesh>(intPtr3) : null;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00004708 File Offset: 0x00002908
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1498903, RefRangeEnd = 1498904, XrefRangeStart = 1498902, XrefRangeEnd = 1498903, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Copy(int vertexcount, Il2CppStructArray<Vector3> src, Il2CppStructArray<Vector3> dst, ref int offset, Matrix4x4 transform)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)5) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref vertexcount;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(src);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(dst);
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = &offset;
		ptr[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref transform;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0000477C File Offset: 0x0000297C
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1498909, RefRangeEnd = 1498910, XrefRangeStart = 1498904, XrefRangeEnd = 1498909, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CopyNormal(int vertexcount, Il2CppStructArray<Vector3> src, Il2CppStructArray<Vector3> dst, ref int offset, Matrix4x4 transform)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)5) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref vertexcount;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(src);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(dst);
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = &offset;
		ptr[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref transform;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_CopyNormal_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000047F0 File Offset: 0x000029F0
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498910, XrefRangeEnd = 1498911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Copy(int vertexcount, Il2CppStructArray<Vector2> src, Il2CppStructArray<Vector2> dst, ref int offset)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref vertexcount;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(src);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(dst);
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = &offset;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_byref_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004854 File Offset: 0x00002A54
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1498911, XrefRangeEnd = 1498912, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CopyColors(int vertexcount, Il2CppStructArray<Color> src, Il2CppStructArray<Color> dst, ref int offset)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref vertexcount;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(src);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(dst);
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = &offset;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_CopyColors_Private_Static_Void_Int32_Il2CppStructArray_1_Color_Il2CppStructArray_1_Color_byref_Int32_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000048B8 File Offset: 0x00002AB8
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1498917, RefRangeEnd = 1498918, XrefRangeStart = 1498912, XrefRangeEnd = 1498917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CopyTangents(int vertexcount, Il2CppStructArray<Vector4> src, Il2CppStructArray<Vector4> dst, ref int offset, Matrix4x4 transform)
	{
		IntPtr* ptr;
		checked
		{
			ptr = stackalloc IntPtr[unchecked((UIntPtr)5) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref vertexcount;
		}
		ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(src);
		ptr[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.Il2CppObjectBaseToPtr(dst);
		ptr[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = &offset;
		ptr[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = ref transform;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr_CopyTangents_Private_Static_Void_Int32_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector4_byref_Int32_Matrix4x4_0, 0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0000492C File Offset: 0x00002B2C
	[CallerCount(250)]
	[CachedScanResults(RefRangeStart = 11, RefRangeEnd = 261, XrefRangeStart = 11, XrefRangeEnd = 261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshCombineUtility()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		IntPtr intPtr2;
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MeshCombineUtility.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00002238 File Offset: 0x00000438
	public MeshCombineUtility(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x04000039 RID: 57
	private static readonly IntPtr NativeMethodInfoPtr_Combine_Public_Static_Mesh_Il2CppReferenceArray_1_MeshInstance_Boolean_0;

	// Token: 0x0400003A RID: 58
	private static readonly IntPtr NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0;

	// Token: 0x0400003B RID: 59
	private static readonly IntPtr NativeMethodInfoPtr_CopyNormal_Private_Static_Void_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector3_byref_Int32_Matrix4x4_0;

	// Token: 0x0400003C RID: 60
	private static readonly IntPtr NativeMethodInfoPtr_Copy_Private_Static_Void_Int32_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_byref_Int32_0;

	// Token: 0x0400003D RID: 61
	private static readonly IntPtr NativeMethodInfoPtr_CopyColors_Private_Static_Void_Int32_Il2CppStructArray_1_Color_Il2CppStructArray_1_Color_byref_Int32_0;

	// Token: 0x0400003E RID: 62
	private static readonly IntPtr NativeMethodInfoPtr_CopyTangents_Private_Static_Void_Int32_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector4_byref_Int32_Matrix4x4_0;

	// Token: 0x0400003F RID: 63
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	// Token: 0x0200003A RID: 58
	public sealed class MeshInstance : ValueType
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00009DF0 File Offset: 0x00007FF0
		// Note: this type is marked as 'beforefieldinit'.
		static MeshInstance()
		{
			Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshCombineUtility>.NativeClassPtr, "MeshInstance");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr);
			MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr, "mesh");
			MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_subMeshIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr, "subMeshIndex");
			MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr, "transform");
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00002FCE File Offset: 0x000011CE
		public MeshInstance(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00002FD7 File Offset: 0x000011D7
		public MeshInstance()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshCombineUtility.MeshInstance>.NativeClassPtr))
		{
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00009E58 File Offset: 0x00008058
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00002FE9 File Offset: 0x000011E9
		public unsafe Mesh mesh
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_mesh);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Mesh>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_mesh), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00009E88 File Offset: 0x00008088
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00003008 File Offset: 0x00001208
		public unsafe int subMeshIndex
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_subMeshIndex);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_subMeshIndex)) = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00009EB0 File Offset: 0x000080B0
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00003023 File Offset: 0x00001223
		public unsafe Matrix4x4 transform
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_transform);
				return *intPtr;
			}
			set
			{
				*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(MeshCombineUtility.MeshInstance.NativeFieldInfoPtr_transform)) = value;
			}
		}

		// Token: 0x0400014B RID: 331
		private static readonly IntPtr NativeFieldInfoPtr_mesh;

		// Token: 0x0400014C RID: 332
		private static readonly IntPtr NativeFieldInfoPtr_subMeshIndex;

		// Token: 0x0400014D RID: 333
		private static readonly IntPtr NativeFieldInfoPtr_transform;
	}
}
