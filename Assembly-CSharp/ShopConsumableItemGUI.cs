// Decompiled with JetBrains decompiler
// Type: ShopConsumableItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class ShopConsumableItemGUI : BaseItemGUI
{
  private ItemPrice _pointsPrice;
  private ItemPrice _creditsPrice;

  public ShopConsumableItemGUI(IUnityItem item, BuyingLocationType location)
    : base(item, location, BuyingRecommendationType.None)
  {
    this._pointsPrice = ShopUtils.GetLowestPrice(item, UberStrikeCurrencyType.Points);
    this._creditsPrice = ShopUtils.GetLowestPrice(item, UberStrikeCurrencyType.Credits);
  }

  public override void Draw(Rect rect, bool selected)
  {
    GUI.BeginGroup(rect);
    this.DrawIcon(new Rect(4f, 4f, 48f, 48f));
    this.DrawArmorOverlay();
    this.DrawPromotionalTag();
    this.DrawName(new Rect(63f, 10f, 220f, 20f));
    this.DrawPrice(new Rect(63f, 30f, 220f, 20f), this._pointsPrice, this._creditsPrice);
    if (PlayerDataManager.PlayerLevel < this.Item.ItemView.LevelLock)
    {
      GUI.color = new Color(1f, 1f, 1f, 0.1f);
      GUI.DrawTexture(new Rect(rect.width - 100f, 7f, 46f, 46f), (Texture) ShopIcons.BlankItemFrame);
      GUI.color = Color.white;
    }
    if (Singleton<InventoryManager>.Instance.IsItemInInventory(this.Item.ItemId))
      this.DrawEquipButton(new Rect(rect.width - 100f, 7f, 46f, 46f), LocalizedStrings.Equip);
    this.DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Buy);
    this.DrawGrayLine(rect);
    GUI.EndGroup();
  }

  private void DrawPackPrice(Rect rect)
  {
    string text = string.Format("{0}", this._creditsPrice.Price != 0 ? (object) this._creditsPrice.Price.ToString("N0") : (object) "FREE");
    GUI.DrawTexture(new Rect(rect.x, rect.y, 16f, 16f), (Texture) ShopUtils.CurrencyIcon(this._creditsPrice.Currency));
    GUI.Label(new Rect(rect.x + 20f, rect.y + 3f, rect.width - 20f, 16f), text, BlueStonez.label_interparkmed_11pt_left);
  }
}
