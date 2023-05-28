// Decompiled with JetBrains decompiler
// Type: InventoryItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UnityEngine;

public class InventoryItemGUI : BaseItemGUI
{
  private float _alpha;

  public InventoryItemGUI(InventoryItem item, BuyingLocationType location)
    : base(item.Item, location, BuyingRecommendationType.None)
  {
    this.InventoryItem = item;
  }

  public InventoryItem InventoryItem { get; private set; }

  public override void Draw(Rect rect, bool selected)
  {
    this.DrawHighlightedBackground(rect);
    GUI.BeginGroup(rect);
    this.DrawIcon(new Rect(4f, 4f, 48f, 48f));
    this.DrawName(new Rect(63f, 10f, 220f, 20f));
    this.DrawDaysRemaining(new Rect(63f, 30f, 220f, 20f));
    if (this.Item.ItemId == 1294)
      this.DrawUseButton(new Rect(rect.width - 100f, 7f, 46f, 46f));
    else if (ItemManager.IsItemEquippable(this.Item))
    {
      if (this.InventoryItem.IsPermanent || this.InventoryItem.DaysRemaining > 0)
        this.DrawEquipButton(new Rect(rect.width - 100f, 7f, 46f, 46f), LocalizedStrings.Equip);
      else if (!GameState.HasCurrentGame)
      {
        this._alpha = Mathf.Lerp(this._alpha, !selected ? 0.0f : 1f, Time.deltaTime * (!selected ? 10f : 2f));
        GUI.color = new Color(1f, 1f, 1f, this._alpha);
        this.DrawTryButton(new Rect(rect.width - 100f, 7f, 46f, 46f));
        GUI.color = Color.white;
      }
    }
    if (this.Item.ItemView.IsForSale)
    {
      if (!this.InventoryItem.IsPermanent)
        this.DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Renew, ShopArea.Inventory);
      else if (this.InventoryItem.AmountRemaining >= 0)
        this.DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Buy, ShopArea.Inventory);
    }
    this.DrawGrayLine(rect);
    if (selected)
    {
      GUI.color = new Color(1f, 1f, 1f, 0.5f);
      if (this.Item.ItemType == UberstrikeItemType.Weapon)
        GUI.Label(new Rect(12f, 60f, 32f, 32f), (Texture) UberstrikeIconsHelper.GetIconForItemClass(this.Item.ItemClass), GUIStyle.none);
      else if (this.Item.ItemType == UberstrikeItemType.Gear)
        GUI.Label(new Rect(12f, 60f, 32f, 32f), (Texture) UberstrikeIconsHelper.GetIconForItemClass(this.Item.ItemClass), GUIStyle.none);
      GUI.color = Color.white;
      this.DrawDescription(new Rect(55f, 60f, (float) byte.MaxValue, 52f));
      if (this.DetailGUI != null)
        this.DetailGUI.Draw();
    }
    GUI.EndGroup();
  }

  public void DrawHighlightedBackground(Rect rect)
  {
    if (!this.InventoryItem.IsHighlighted)
      return;
    GUI.color = ColorConverter.RgbaToColor((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 20f * GUITools.FastSinusPulse);
    GUI.DrawTexture(rect, (Texture) UberstrikeIconsHelper.White);
    GUI.color = Color.white;
  }

  public void DrawDaysRemaining(Rect rect)
  {
    bool flag = true;
    Color color = Color.white;
    string empty = string.Empty;
    string text;
    if (this.InventoryItem.AmountRemaining >= 0)
    {
      text = this.InventoryItem.AmountRemaining != 1 ? this.InventoryItem.AmountRemaining.ToString() + " uses remaining" : this.InventoryItem.AmountRemaining.ToString() + " use remaining";
      flag = false;
    }
    else if (this.InventoryItem.IsPermanent)
      text = LocalizedStrings.Permanent;
    else if (this.InventoryItem.DaysRemaining > 1 && this.InventoryItem.DaysRemaining < 5)
    {
      color = ColorScheme.UberStrikeYellow;
      text = string.Format("{0} {1}{2}", (object) this.InventoryItem.DaysRemaining.ToString(), (object) LocalizedStrings.Day, this.InventoryItem.DaysRemaining != 1 ? (object) "s" : (object) string.Empty);
    }
    else if (this.InventoryItem.DaysRemaining == 1)
    {
      color = ColorScheme.UberStrikeYellow;
      text = LocalizedStrings.LastDay;
    }
    else if (this.InventoryItem.DaysRemaining <= 0)
    {
      color = ColorScheme.UberStrikeRed;
      text = LocalizedStrings.Expired;
    }
    else
      text = string.Format("{0} {1}{2}", (object) this.InventoryItem.DaysRemaining.ToString(), (object) LocalizedStrings.Day, this.InventoryItem.DaysRemaining != 1 ? (object) "s" : (object) string.Empty);
    if (flag)
      GUI.DrawTexture(new Rect(rect.x, rect.y, 16f, 16f), (Texture) ShopIcons.ItemexpirationIcon);
    GUI.color = color;
    GUI.Label(new Rect(rect.x + (!flag ? 0.0f : 20f), rect.y + 3f, rect.width - 20f, 16f), text, BlueStonez.label_interparkmed_11pt_left);
    GUI.color = Color.white;
  }
}
