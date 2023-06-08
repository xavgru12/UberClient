using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UnityEngine;

public class InventoryItemGUI : BaseItemGUI
{
	public InventoryItem InventoryItem
	{
		get;
		private set;
	}

	public InventoryItemGUI(InventoryItem item, BuyingLocationType location)
		: base(item.Item, location, BuyingRecommendationType.None)
	{
		InventoryItem = item;
	}

	public override void Draw(Rect rect, bool selected)
	{
		DrawHighlightedBackground(rect);
		GUI.BeginGroup(rect);
		DrawIcon(new Rect(4f, 4f, 48f, 48f));
		DrawName(new Rect(63f, 10f, 220f, 20f));
		DrawDaysRemaining(new Rect(63f, 30f, 220f, 20f));
		if (base.Item.View.ID == 1294)
		{
			DrawUseButton(new Rect(rect.width - 50f, 7f, 46f, 46f));
		}
		else if (base.Item.Equippable && (InventoryItem.IsPermanent || InventoryItem.DaysRemaining > 0))
		{
			DrawEquipButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Equip);
		}
		else if (base.Item.View.IsForSale)
		{
			if (!InventoryItem.IsPermanent)
			{
				DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Renew, ShopArea.Inventory);
			}
			else if (InventoryItem.AmountRemaining >= 0)
			{
				DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Buy, ShopArea.Inventory);
			}
		}
		DrawGrayLine(rect);
		if (selected)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			if (base.Item.View.ItemType == UberstrikeItemType.Weapon)
			{
				GUI.Label(new Rect(12f, 60f, 32f, 32f), UberstrikeIconsHelper.GetIconForItemClass(base.Item.View.ItemClass), GUIStyle.none);
			}
			else if (base.Item.View.ItemType == UberstrikeItemType.Gear)
			{
				GUI.Label(new Rect(12f, 60f, 32f, 32f), UberstrikeIconsHelper.GetIconForItemClass(base.Item.View.ItemClass), GUIStyle.none);
			}
			GUI.color = Color.white;
			DrawDescription(new Rect(55f, 60f, 255f, 52f));
			if (base.DetailGUI != null)
			{
				base.DetailGUI.Draw();
			}
		}
		GUI.EndGroup();
	}

	public void DrawHighlightedBackground(Rect rect)
	{
		if (InventoryItem.IsHighlighted)
		{
			GUI.color = ColorConverter.RgbaToColor(255f, 255f, 255f, 20f * GUITools.FastSinusPulse);
			GUI.DrawTexture(rect, UberstrikeIconsHelper.White);
			GUI.color = Color.white;
		}
	}

	public void DrawDaysRemaining(Rect rect)
	{
		bool flag = true;
		Color color = Color.white;
		string empty = string.Empty;
		if (InventoryItem.AmountRemaining >= 0)
		{
			empty = ((InventoryItem.AmountRemaining != 1) ? (InventoryItem.AmountRemaining.ToString() + " uses remaining") : (InventoryItem.AmountRemaining.ToString() + " use remaining"));
			flag = false;
		}
		else if (InventoryItem.IsPermanent)
		{
			empty = LocalizedStrings.Permanent;
		}
		else if (InventoryItem.DaysRemaining > 1 && InventoryItem.DaysRemaining < 5)
		{
			color = ColorScheme.UberStrikeYellow;
			empty = string.Format("{0} {1}{2}", InventoryItem.DaysRemaining.ToString(), LocalizedStrings.Day, (InventoryItem.DaysRemaining != 1) ? "s" : string.Empty);
		}
		else if (InventoryItem.DaysRemaining == 1)
		{
			color = ColorScheme.UberStrikeYellow;
			empty = LocalizedStrings.LastDay;
		}
		else if (InventoryItem.DaysRemaining <= 0)
		{
			color = ColorScheme.UberStrikeRed;
			empty = LocalizedStrings.Expired;
		}
		else
		{
			empty = string.Format("{0} {1}{2}", InventoryItem.DaysRemaining.ToString(), LocalizedStrings.Day, (InventoryItem.DaysRemaining != 1) ? "s" : string.Empty);
		}
		if (flag)
		{
			GUI.DrawTexture(new Rect(rect.x, rect.y, 16f, 16f), ShopIcons.ItemexpirationIcon);
		}
		GUI.color = color;
		GUI.Label(new Rect(rect.x + (float)(flag ? 20 : 0), rect.y + 3f, rect.width - 20f, 16f), empty, BlueStonez.label_interparkmed_11pt_left);
		GUI.color = Color.white;
	}
}
