using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class QualitySettingCurve : PropertyAttribute
{
	// Token: 0x06000074 RID: 116 RVA: 0x00004CCC File Offset: 0x00002ECC
	// Note: this type is marked as 'beforefieldinit'.
	static QualitySettingCurve()
	{
		Il2CppClassPointerStore<QualitySettingCurve>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "", "QualitySettingCurve");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QualitySettingCurve>.NativeClassPtr);
		QualitySettingCurve.NativeFieldInfoPtr__qualityLevels = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QualitySettingCurve>.NativeClassPtr, "_qualityLevels");
		QualitySettingCurve.NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualitySettingCurve>.NativeClassPtr, 100663359);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004D24 File Offset: 0x00002F24
	[CallerCount(0)]
	public unsafe QualitySettingCurve(int qualityLevels)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QualitySettingCurve>.NativeClassPtr))
	{
		checked
		{
			IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
			*ptr = ref qualityLevels;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(QualitySettingCurve.NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000022D4 File Offset: 0x000004D4
	public QualitySettingCurve(IntPtr pointer)
		: base(pointer)
	{
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000077 RID: 119 RVA: 0x00004D6C File Offset: 0x00002F6C
	// (set) Token: 0x06000078 RID: 120 RVA: 0x000022DD File Offset: 0x000004DD
	public unsafe int _qualityLevels
	{
		get
		{
			IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(QualitySettingCurve.NativeFieldInfoPtr__qualityLevels);
			return *intPtr;
		}
		set
		{
			*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(QualitySettingCurve.NativeFieldInfoPtr__qualityLevels)) = value;
		}
	}

	// Token: 0x0400004B RID: 75
	private static readonly IntPtr NativeFieldInfoPtr__qualityLevels;

	// Token: 0x0400004C RID: 76
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;
}
