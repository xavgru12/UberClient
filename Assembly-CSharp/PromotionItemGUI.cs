// Decompiled with JetBrains decompiler
// Type: PromotionItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PromotionItemGUI : BaseItemGUI
{
  private ItemPrice _pointsPrice;
  private ItemPrice _creditsPrice;

  public PromotionItemGUI(IUnityItem item, BuyingLocationType location)
    : base(item, location, BuyingRecommendationType.Manual)
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
    if (this.Item.ItemView != null && PlayerDataManager.PlayerLevel < this.Item.ItemView.LevelLock)
    {
      GUI.color = new Color(1f, 1f, 1f, 0.1f);
      GUI.DrawTexture(new Rect(rect.width - 100f, 7f, 46f, 46f), (Texture) ShopIcons.BlankItemFrame);
      GUI.color = Color.white;
    }
    if (Singleton<InventoryManager>.Instance.IsItemInInventory(this.Item.ItemId))
      this.DrawEquipButton(new Rect(rect.width - 100f, 7f, 46f, 46f), LocalizedStrings.Equip);
    else if (!GameState.HasCurrentGame && (this.Item.ItemType == UberstrikeItemType.Weapon || this.Item.ItemType == UberstrikeItemType.Gear && this.Item.ItemClass != UberstrikeItemClass.GearHolo) && GUI.Button(new Rect(rect.width - 100f, 7f, 46f, 46f), new GUIContent(LocalizedStrings.Try), BlueStonez.buttondark_medium))
    {
      MenuPageManager.Instance.LoadPage(PageType.Shop);
      CmuneEventHandler.Route((object) new SelectShopAreaEvent()
      {
        ShopArea = ShopArea.Shop,
        ItemClass = this.Item.ItemClass,
        ItemType = this.Item.ItemType
      });
      CmuneEventHandler.Route((object) new ShopTryEvent()
      {
        Item = this.Item
      });
    }
    this.DrawBuyButton(new Rect(rect.width - 50f, 7f, 46f, 46f), LocalizedStrings.Buy);
    AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(this.Item, new Rect(4f, 4f, 48f, 48f), PopupViewSide.Left, duration: ItemPackGuiUtil.GetDuration(this.Item));
    GUI.EndGroup();
  }
}
