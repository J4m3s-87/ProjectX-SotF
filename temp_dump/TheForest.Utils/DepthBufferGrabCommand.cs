using System;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheForest.Utils
{
	// Token: 0x02000016 RID: 22
	public class DepthBufferGrabCommand : MonoBehaviour
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x0000583C File Offset: 0x00003A3C
		// Note: this type is marked as 'beforefieldinit'.
		static DepthBufferGrabCommand()
		{
			Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr = IL2CPP.GetIl2CppClass("TheForest.Utils.dll", "TheForest.Utils", "DepthBufferGrabCommand");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr);
			DepthBufferGrabCommand.NativeFieldInfoPtr_depthCopySdr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, "depthCopySdr");
			DepthBufferGrabCommand.NativeFieldInfoPtr_m_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, "m_data");
			DepthBufferGrabCommand.NativeFieldInfoPtr_m_camera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, "m_camera");
			DepthBufferGrabCommand.NativeFieldInfoPtr_m_depthCopyMat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, "m_depthCopyMat");
			DepthBufferGrabCommand.NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663382);
			DepthBufferGrabCommand.NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663383);
			DepthBufferGrabCommand.NativeMethodInfoPtr_AddBinding_Public_Static_Void_Camera_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663384);
			DepthBufferGrabCommand.NativeMethodInfoPtr_RemoveBinding_Public_Static_Void_Camera_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663385);
			DepthBufferGrabCommand.NativeMethodInfoPtr_HasCamera_Public_Static_Boolean_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663386);
			DepthBufferGrabCommand.NativeMethodInfoPtr_CreateCommand_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663387);
			DepthBufferGrabCommand.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, 100663388);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005948 File Offset: 0x00003B48
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499279, XrefRangeEnd = 1499310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Start()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000597C File Offset: 0x00003B7C
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499310, XrefRangeEnd = 1499311, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Update()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000059B0 File Offset: 0x00003BB0
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499311, XrefRangeEnd = 1499332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void AddBinding(Camera cam, string name)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(cam);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_AddBinding_Public_Static_Void_Camera_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000059F8 File Offset: 0x00003BF8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499332, XrefRangeEnd = 1499353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void RemoveBinding(Camera cam, string name)
		{
			IntPtr* ptr;
			checked
			{
				ptr = stackalloc IntPtr[unchecked((UIntPtr)2) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(cam);
			}
			ptr[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)) / (UIntPtr)sizeof(IntPtr)] = IL2CPP.ManagedStringToIl2Cpp(name);
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_RemoveBinding_Public_Static_Void_Camera_String_0, 0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005A40 File Offset: 0x00003C40
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499353, XrefRangeEnd = 1499362, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool HasCamera(Camera cam)
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr(cam);
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_HasCamera_Public_Static_Boolean_Camera_0, 0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005A84 File Offset: 0x00003C84
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1499386, RefRangeEnd = 1499388, XrefRangeStart = 1499362, XrefRangeEnd = 1499386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CreateCommand()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr_CreateCommand_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005AB8 File Offset: 0x00003CB8
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DepthBufferGrabCommand()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			IntPtr intPtr2;
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000245E File Offset: 0x0000065E
		public DepthBufferGrabCommand(IntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005AF4 File Offset: 0x00003CF4
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00002467 File Offset: 0x00000667
		public unsafe Shader depthCopySdr
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_depthCopySdr);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Shader>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_depthCopySdr), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005B24 File Offset: 0x00003D24
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00002486 File Offset: 0x00000686
		public unsafe static Dictionary<Camera, DepthBufferGrabCommand.CommandData> m_data
		{
			get
			{
				IntPtr intPtr;
				IL2CPP.il2cpp_field_static_get_value(DepthBufferGrabCommand.NativeFieldInfoPtr_m_data, (void*)(&intPtr));
				IntPtr intPtr2 = intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Dictionary<Camera, DepthBufferGrabCommand.CommandData>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(DepthBufferGrabCommand.NativeFieldInfoPtr_m_data, IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005B4C File Offset: 0x00003D4C
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00002498 File Offset: 0x00000698
		public unsafe Camera m_camera
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_m_camera);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Camera>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_m_camera), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005B7C File Offset: 0x00003D7C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000024B7 File Offset: 0x000006B7
		public unsafe Material m_depthCopyMat
		{
			get
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_m_depthCopyMat);
				IntPtr intPtr2 = *intPtr;
				return (intPtr2 != 0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
			}
			set
			{
				IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.NativeFieldInfoPtr_m_depthCopyMat), IL2CPP.Il2CppObjectBaseToPtr(value));
			}
		}

		// Token: 0x0400006C RID: 108
		private static readonly IntPtr NativeFieldInfoPtr_depthCopySdr;

		// Token: 0x0400006D RID: 109
		private static readonly IntPtr NativeFieldInfoPtr_m_data;

		// Token: 0x0400006E RID: 110
		private static readonly IntPtr NativeFieldInfoPtr_m_camera;

		// Token: 0x0400006F RID: 111
		private static readonly IntPtr NativeFieldInfoPtr_m_depthCopyMat;

		// Token: 0x04000070 RID: 112
		private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

		// Token: 0x04000071 RID: 113
		private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

		// Token: 0x04000072 RID: 114
		private static readonly IntPtr NativeMethodInfoPtr_AddBinding_Public_Static_Void_Camera_String_0;

		// Token: 0x04000073 RID: 115
		private static readonly IntPtr NativeMethodInfoPtr_RemoveBinding_Public_Static_Void_Camera_String_0;

		// Token: 0x04000074 RID: 116
		private static readonly IntPtr NativeMethodInfoPtr_HasCamera_Public_Static_Boolean_Camera_0;

		// Token: 0x04000075 RID: 117
		private static readonly IntPtr NativeMethodInfoPtr_CreateCommand_Private_Void_0;

		// Token: 0x04000076 RID: 118
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		// Token: 0x0200003E RID: 62
		public class CommandData : global::Il2CppSystem.Object
		{
			// Token: 0x06000246 RID: 582 RVA: 0x0000A134 File Offset: 0x00008334
			// Note: this type is marked as 'beforefieldinit'.
			static CommandData()
			{
				Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DepthBufferGrabCommand>.NativeClassPtr, "CommandData");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr);
				DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_bindings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, "bindings");
				DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_command = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, "command");
				DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, "width");
				DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, "height");
				DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_recreate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, "recreate");
				DepthBufferGrabCommand.CommandData.NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr, 100663390);
			}

			// Token: 0x06000247 RID: 583 RVA: 0x0000A1D8 File Offset: 0x000083D8
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1499273, XrefRangeEnd = 1499279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe CommandData()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DepthBufferGrabCommand.CommandData>.NativeClassPtr))
			{
				IntPtr* ptr = null;
				IntPtr intPtr2;
				IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(DepthBufferGrabCommand.CommandData.NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			// Token: 0x06000248 RID: 584 RVA: 0x0000312F File Offset: 0x0000132F
			public CommandData(IntPtr pointer)
				: base(pointer)
			{
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x06000249 RID: 585 RVA: 0x0000A214 File Offset: 0x00008414
			// (set) Token: 0x0600024A RID: 586 RVA: 0x00003138 File Offset: 0x00001338
			public unsafe HashSet<string> bindings
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_bindings);
					IntPtr intPtr2 = *intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<HashSet<string>>(intPtr2) : null;
				}
				set
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_bindings), IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x0600024B RID: 587 RVA: 0x0000A244 File Offset: 0x00008444
			// (set) Token: 0x0600024C RID: 588 RVA: 0x00003157 File Offset: 0x00001357
			public unsafe CommandBuffer command
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_command);
					IntPtr intPtr2 = *intPtr;
					return (intPtr2 != 0) ? Il2CppObjectPool.Get<CommandBuffer>(intPtr2) : null;
				}
				set
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(intPtr, intPtr + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_command), IL2CPP.Il2CppObjectBaseToPtr(value));
				}
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x0600024D RID: 589 RVA: 0x0000A274 File Offset: 0x00008474
			// (set) Token: 0x0600024E RID: 590 RVA: 0x00003176 File Offset: 0x00001376
			public unsafe int width
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_width);
					return *intPtr;
				}
				set
				{
					*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_width)) = value;
				}
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x0600024F RID: 591 RVA: 0x0000A29C File Offset: 0x0000849C
			// (set) Token: 0x06000250 RID: 592 RVA: 0x00003191 File Offset: 0x00001391
			public unsafe int height
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_height);
					return *intPtr;
				}
				set
				{
					*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_height)) = value;
				}
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x06000251 RID: 593 RVA: 0x0000A2C4 File Offset: 0x000084C4
			// (set) Token: 0x06000252 RID: 594 RVA: 0x000031AC File Offset: 0x000013AC
			public unsafe bool recreate
			{
				get
				{
					IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_recreate);
					return *intPtr;
				}
				set
				{
					*(IL2CPP.Il2CppObjectBaseToPtrNotNull(this) + (IntPtr)IL2CPP.il2cpp_field_get_offset(DepthBufferGrabCommand.CommandData.NativeFieldInfoPtr_recreate)) = value;
				}
			}

			// Token: 0x04000159 RID: 345
			private static readonly IntPtr NativeFieldInfoPtr_bindings;

			// Token: 0x0400015A RID: 346
			private static readonly IntPtr NativeFieldInfoPtr_command;

			// Token: 0x0400015B RID: 347
			private static readonly IntPtr NativeFieldInfoPtr_width;

			// Token: 0x0400015C RID: 348
			private static readonly IntPtr NativeFieldInfoPtr_height;

			// Token: 0x0400015D RID: 349
			private static readonly IntPtr NativeFieldInfoPtr_recreate;

			// Token: 0x0400015E RID: 350
			private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;
		}
	}
}
