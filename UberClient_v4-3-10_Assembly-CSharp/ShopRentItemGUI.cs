// Decompiled with JetBrains decompiler
// Type: ShopRentItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class ShopRentItemGUI : BaseItemGUI
{
  private float _alpha;
  private ItemPrice _pointsPrice;
  private ItemPrice _creditsPrice;

  public ShopRentItemGUI(
    IUnityItem item,
    BuyingLocationType location,
    BuyingRecommendationType recommendation = BuyingRecommendationType.None)
    : base(item, location, recommendation)
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
    {
      if (!Singleton<LoadoutManager>.Instance.IsItemEquipped(this.Item.ItemId))
        this.DrawEquipButton(new Rect(rect.width - 100f, 7f, 46f, 46f), LocalizedStrings.Equip);
    }
    else if (!GameState.HasCurrentGame && (this.Item.ItemType == UberstrikeItemType.Weapon || this.Item.ItemType == UberstrikeItemType.Gear && this.Item.ItemClass != UberstrikeItemClass.GearHolo))
    {
      this._alpha = Mathf.Lerp(this._alpha, !selected ? 0.0f : 1f, Time.deltaTime * (!selected ? 10f : 2f));
      GUI.color = new Color(1f, 1f, 1f, this._alpha);
      this.DrawTryButton(new Rect(rect.width - 100f, 7f, 46f, 46f));
      GUI.color = Color.white;
    }
    this.DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Buy);
    this.DrawGrayLine(rect);
    GUI.EndGroup();
  }
}
