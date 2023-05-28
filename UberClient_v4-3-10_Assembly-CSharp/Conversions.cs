// Decompiled with JetBrains decompiler
// Type: Conversions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public static class Conversions
{
  public static MysteryBoxShopItem ToUnityItem(this MysteryBoxUnityView mysteryBox)
  {
    List<IUnityItem> unityItemList = new List<IUnityItem>();
    if (mysteryBox.ExposeItemsToPlayers)
    {
      if (mysteryBox.PointsAttributed > 0)
        unityItemList.Add((IUnityItem) new PointsUnityItem(mysteryBox.PointsAttributed));
      if (mysteryBox.CreditsAttributed > 0)
        unityItemList.Add((IUnityItem) new CreditsUnityItem(mysteryBox.CreditsAttributed));
      foreach (BundleItemView mysteryBoxItem in mysteryBox.MysteryBoxItems)
        unityItemList.Add(Singleton<ItemManager>.Instance.GetItemInShop(mysteryBoxItem.ItemId));
    }
    MysteryBoxShopItem unityItem = new MysteryBoxShopItem();
    unityItem.Name = mysteryBox.Name;
    unityItem.Id = mysteryBox.Id;
    unityItem.Price = new ItemPrice()
    {
      Price = mysteryBox.Price,
      Currency = mysteryBox.UberStrikeCurrencyType
    };
    unityItem.Icon = new DynamicTexture(mysteryBox.IconUrl);
    unityItem.Image = new DynamicTexture(mysteryBox.ImageUrl);
    unityItem.View = mysteryBox;
    unityItem.Items = unityItemList;
    unityItem.Category = mysteryBox.Category;
    return unityItem;
  }

  public static GUIContent PriceTag(this ItemPrice price, bool printCurrency = false, string tooltip = "")
  {
    switch (price.Currency)
    {
      case UberStrikeCurrencyType.Credits:
        return new GUIContent(price.Price.ToString("N0") + (!printCurrency ? string.Empty : "Credits"), (Texture) ShopIcons.IconCredits20x20, tooltip);
      case UberStrikeCurrencyType.Points:
        return new GUIContent(price.Price.ToString("N0") + (!printCurrency ? string.Empty : "Points"), (Texture) ShopIcons.IconPoints20x20, tooltip);
      default:
        return new GUIContent("N/A");
    }
  }

  public static LuckyDrawShopItem ToUnityItem(this LuckyDrawUnityView luckyDraw)
  {
    LuckyDrawShopItem luckyDrawShopItem = new LuckyDrawShopItem();
    luckyDrawShopItem.Name = luckyDraw.Name;
    luckyDrawShopItem.Id = luckyDraw.Id;
    luckyDrawShopItem.Price = new ItemPrice()
    {
      Price = luckyDraw.Price,
      Currency = luckyDraw.UberStrikeCurrencyType
    };
    luckyDrawShopItem.Icon = new DynamicTexture(luckyDraw.IconUrl);
    luckyDrawShopItem.View = luckyDraw;
    luckyDrawShopItem.Category = luckyDraw.Category;
    LuckyDrawShopItem unityItem = luckyDrawShopItem;
    List<LuckyDrawShopItem.LuckyDrawSet> luckyDrawSetList = new List<LuckyDrawShopItem.LuckyDrawSet>();
    foreach (LuckyDrawSetUnityView luckyDrawSet1 in luckyDraw.LuckyDrawSets)
    {
      LuckyDrawShopItem.LuckyDrawSet luckyDrawSet2 = new LuckyDrawShopItem.LuckyDrawSet()
      {
        Id = luckyDrawSet1.Id,
        Items = new List<IUnityItem>(),
        Image = new DynamicTexture(luckyDrawSet1.ImageUrl),
        View = luckyDrawSet1,
        Parent = unityItem
      };
      if (luckyDrawSet1.ExposeItemsToPlayers)
      {
        if (luckyDrawSet1.PointsAttributed > 0)
          luckyDrawSet2.Items.Add((IUnityItem) new PointsUnityItem(luckyDrawSet1.PointsAttributed));
        if (luckyDrawSet1.CreditsAttributed > 0)
          luckyDrawSet2.Items.Add((IUnityItem) new CreditsUnityItem(luckyDrawSet1.CreditsAttributed));
        foreach (BundleItemView luckyDrawSetItem in luckyDrawSet1.LuckyDrawSetItems)
          luckyDrawSet2.Items.Add(Singleton<ItemManager>.Instance.GetItemInShop(luckyDrawSetItem.ItemId));
      }
      luckyDrawSetList.Add(luckyDrawSet2);
    }
    unityItem.Sets = luckyDrawSetList;
    return unityItem;
  }
}
