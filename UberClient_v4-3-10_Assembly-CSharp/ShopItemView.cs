// Decompiled with JetBrains decompiler
// Type: ShopItemView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class ShopItemView
{
  public ShopItemView(int itemId)
  {
    IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemId);
    this.Icon = itemInShop == null ? (Texture) UberstrikeIconsHelper.White : (Texture) itemInShop.Icon;
    this.Points = 0;
    this.Credits = 0;
    this.UnityItem = itemInShop;
    this.ItemId = itemId;
    this.Duration = BuyingDurationType.None;
  }

  public ShopItemView(BundleItemView itemView)
  {
    IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemView.ItemId);
    this.Icon = itemInShop == null ? (Texture) UberstrikeIconsHelper.White : (Texture) itemInShop.Icon;
    this.Points = 0;
    this.Credits = 0;
    this.UnityItem = itemInShop;
    this.ItemId = itemView.ItemId;
    this.Duration = itemView.Duration;
  }

  public ShopItemView(UberStrikeCurrencyType currency, int price)
  {
    switch (currency)
    {
      case UberStrikeCurrencyType.Credits:
        this.Icon = (Texture) ShopIcons.CreditsIcon48x48;
        this.UnityItem = (IUnityItem) new CreditsUnityItem(price);
        this.Points = 0;
        this.Credits = price;
        break;
      case UberStrikeCurrencyType.Points:
        this.Icon = (Texture) ShopIcons.Points48x48;
        this.UnityItem = (IUnityItem) new PointsUnityItem(price);
        this.Credits = 0;
        this.Points = price;
        break;
      default:
        this.Icon = (Texture) UberstrikeIconsHelper.White;
        this.UnityItem = (IUnityItem) null;
        this.Points = 0;
        this.Credits = 0;
        break;
    }
    this.ItemId = 0;
    this.Duration = BuyingDurationType.None;
  }

  public Texture Icon { get; private set; }

  public BuyingDurationType Duration { get; private set; }

  public IUnityItem UnityItem { get; private set; }

  public int ItemId { get; private set; }

  public int Credits { get; private set; }

  public int Points { get; private set; }
}
