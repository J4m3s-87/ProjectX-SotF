using System;
using System.Runtime.CompilerServices;
using Sons.Gui.Input;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Tools
{
	// Token: 0x0200000A RID: 10
	internal static class LinkUi
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000036A8 File Offset: 0x000018A8
		internal static LinkUiElement CreateLinkUi(GameObject toAddLinkUiOn, float maxDistance, Texture texture, Texture2D texture2D, Vector3? worldSpaceOffset, string elementId = "screen.take")
		{
			Vector3 vector = worldSpaceOffset ?? new Vector3(0f, 0.2f, 0f);
			LinkUiElement linkUiElement = toAddLinkUiOn.AddComponent<LinkUiElement>();
			linkUiElement._applyMaterial = false;
			linkUiElement._applyText = false;
			linkUiElement._applyTexture = true;
			bool flag = texture != null;
			if (flag)
			{
				linkUiElement._texture = texture;
			}
			else
			{
				bool flag2 = texture2D != null;
				if (flag2)
				{
					linkUiElement._texture = texture2D;
				}
			}
			linkUiElement._maxDistance = maxDistance;
			linkUiElement._worldSpaceOffset = vector;
			linkUiElement._uiElementId = elementId;
			linkUiElement.enabled = false;
			linkUiElement.enabled = true;
			return linkUiElement;
		}
	}
}


