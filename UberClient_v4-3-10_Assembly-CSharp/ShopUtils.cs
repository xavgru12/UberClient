// Decompiled with JetBrains decompiler
// Type: ShopUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

public static class ShopUtils
{
  public static ItemPrice GetLowestPrice(IUnityItem item, UberStrikeCurrencyType currency = UberStrikeCurrencyType.None)
  {
    ItemPrice lowestPrice = (ItemPrice) null;
    if (item != null && item.ItemView != null && item.ItemView.Prices != null)
    {
      foreach (ItemPrice price in (IEnumerable<ItemPrice>) item.ItemView.Prices)
      {
        if ((currency == UberStrikeCurrencyType.None || price.Currency == currency) && (lowestPrice == null || lowestPrice.Price > price.Price))
          lowestPrice = price;
      }
    }
    return lowestPrice;
  }

  public static string PrintDuration(BuyingDurationType duration)
  {
    switch (duration)
    {
      case BuyingDurationType.OneDay:
        return " 1 " + LocalizedStrings.Day;
      case BuyingDurationType.SevenDays:
        return " 1 " + LocalizedStrings.Week;
      case BuyingDurationType.ThirtyDays:
        return " 1 " + LocalizedStrings.Month;
      case BuyingDurationType.NinetyDays:
        return " " + LocalizedStrings.ThreeMonths;
      case BuyingDurationType.Permanent:
        return " " + LocalizedStrings.Permanent;
      default:
        return string.Empty;
    }
  }

  public static string PrintCurrency(UberStrikeCurrencyType currency)
  {
    switch (currency)
    {
      case UberStrikeCurrencyType.Credits:
        return LocalizedStrings.Credits;
      case UberStrikeCurrencyType.Points:
        return LocalizedStrings.Points;
      default:
        return string.Empty;
    }
  }

  public static Texture2D CurrencyIcon(UberStrikeCurrencyType currency)
  {
    switch (currency)
    {
      case UberStrikeCurrencyType.Credits:
        return ShopIcons.IconCredits20x20;
      case UberStrikeCurrencyType.Points:
        return ShopIcons.IconPoints20x20;
      default:
        return (Texture2D) null;
    }
  }

  public static WeaponItem GetRecommendedWeaponForMap(
    CombatRangeTier mapCombatRange,
    int playerLevel,
    PlayerStatisticsView stats,
    List<WeaponItem> inventory,
    List<WeaponItem> loadout)
  {
    if (inventory == null)
      inventory = new List<WeaponItem>();
    if (loadout == null)
      loadout = new List<WeaponItem>();
    playerLevel = Mathf.Max(2, playerLevel);
    List<KeyValuePair<WeaponItem, ItemPrice>> keyValuePairList1 = new List<KeyValuePair<WeaponItem, ItemPrice>>();
    List<KeyValuePair<UberstrikeItemClass, int>> keyValuePairList2 = new List<KeyValuePair<UberstrikeItemClass, int>>()
    {
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponMelee, stats.WeaponStatistics.MeleeTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponMachinegun, stats.WeaponStatistics.MachineGunTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponLauncher, stats.WeaponStatistics.LauncherTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponCannon, stats.WeaponStatistics.CannonTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponHandgun, stats.WeaponStatistics.HandgunTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponSniperRifle, stats.WeaponStatistics.SniperTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponSplattergun, stats.WeaponStatistics.SplattergunTotalDamageDone),
      new KeyValuePair<UberstrikeItemClass, int>(UberstrikeItemClass.WeaponShotgun, stats.WeaponStatistics.ShotgunTotalDamageDone)
    };
    keyValuePairList2.Sort((Comparison<KeyValuePair<UberstrikeItemClass, int>>) ((a, b) => -a.Value.CompareTo(b.Value)));
    List<UberstrikeItemClass> damageDone = keyValuePairList2.ConvertAll<UberstrikeItemClass>((Converter<KeyValuePair<UberstrikeItemClass, int>, UberstrikeItemClass>) (a => a.Key));
    KeyValuePair<WeaponItem, ItemPrice> keyValuePair = new KeyValuePair<WeaponItem, ItemPrice>((WeaponItem) null, new ItemPrice()
    {
      Price = int.MaxValue
    });
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      if (shopItem.ItemView.IsForSale && (shopItem.Configuration.CombatRange & mapCombatRange.RangeCategory) != (CombatRangeCategory) 0 && !Singleton<InventoryManager>.Instance.IsItemInInventory(shopItem.ItemId))
      {
        ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice((IUnityItem) shopItem, UberStrikeCurrencyType.Credits);
        ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice((IUnityItem) shopItem, UberStrikeCurrencyType.Points);
        if (shopItem.ItemView.LevelLock <= playerLevel && keyValuePair.Value.Price > lowestPrice2.Price)
          keyValuePair = new KeyValuePair<WeaponItem, ItemPrice>(shopItem, lowestPrice2);
        if (lowestPrice2.Price > 0 && lowestPrice2.Price <= PlayerDataManager.Points && shopItem.ItemView.LevelLock <= playerLevel)
          keyValuePairList1.Add(new KeyValuePair<WeaponItem, ItemPrice>(shopItem, lowestPrice2));
        else if (lowestPrice1.Price > 0 && lowestPrice1.Price <= PlayerDataManager.Credits)
          keyValuePairList1.Add(new KeyValuePair<WeaponItem, ItemPrice>(shopItem, lowestPrice1));
      }
    }
    if (keyValuePairList1.Count == 0)
      return (UnityEngine.Object) keyValuePair.Key != (UnityEngine.Object) null ? keyValuePair.Key : Singleton<ItemManager>.Instance.GetWeaponItemInShop(1002);
    try
    {
      keyValuePairList1.Sort((Comparison<KeyValuePair<WeaponItem, ItemPrice>>) ((a, b) => -b.Value.Price.CompareTo(a.Value.Price)));
      keyValuePairList1.Sort((Comparison<KeyValuePair<WeaponItem, ItemPrice>>) ((a, b) => -damageDone.IndexOf(b.Key.ItemClass).CompareTo(damageDone.IndexOf(a.Key.ItemClass))));
      StringBuilder builder = new StringBuilder();
      keyValuePairList1.ForEach((Action<KeyValuePair<WeaponItem, ItemPrice>>) (w => builder.AppendLine(w.Key.ItemView.LevelLock.ToString() + " " + w.Key.Name + " " + (object) w.Key.Configuration.DPS + " " + (object) w.Key.ItemClass + " " + (object) w.Key.Configuration.CombatRange + " " + (object) w.Value.Currency + " " + (object) w.Value.Price)));
      builder.AppendLine("--");
      damageDone.ForEach((Action<UberstrikeItemClass>) (w => builder.AppendLine(((Enum) w).ToString())));
    }
    catch
    {
      throw;
    }
    return keyValuePairList1[0].Key;
  }

  public static GearItem GetRecommendedArmor(
    int playerLevel,
    HoloGearItem holo,
    GearItem upper,
    GearItem lower)
  {
    int armorPoints1 = !((UnityEngine.Object) holo != (UnityEngine.Object) null) ? 0 : holo.Configuration.ArmorPoints;
    int armorPoints2 = !((UnityEngine.Object) upper != (UnityEngine.Object) null) ? 0 : upper.Configuration.ArmorPoints;
    int armorPoints3 = !((UnityEngine.Object) lower != (UnityEngine.Object) null) ? 0 : lower.Configuration.ArmorPoints;
    playerLevel = Mathf.Max(2, playerLevel);
    List<KeyValuePair<GearItem, ItemPrice>> keyValuePairList = new List<KeyValuePair<GearItem, ItemPrice>>();
    KeyValuePair<GearItem, ItemPrice> keyValuePair = new KeyValuePair<GearItem, ItemPrice>((GearItem) null, new ItemPrice()
    {
      Price = int.MaxValue
    });
    foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Gear, BuyingMarketType.Shop))
    {
      GearItem key = shopItem as GearItem;
      if ((UnityEngine.Object) key != (UnityEngine.Object) null && key.Configuration.IsForSale)
      {
        bool flag = key.ItemClass == UberstrikeItemClass.GearHolo && key.Configuration.ArmorPoints >= armorPoints1 && (UnityEngine.Object) key != (UnityEngine.Object) holo || key.ItemClass == UberstrikeItemClass.GearUpperBody && key.Configuration.ArmorPoints >= armorPoints2 && (UnityEngine.Object) key != (UnityEngine.Object) upper || key.ItemClass == UberstrikeItemClass.GearLowerBody && key.Configuration.ArmorPoints >= armorPoints3 && (UnityEngine.Object) key != (UnityEngine.Object) lower;
        if (key.Configuration.ArmorPoints > 0 && flag)
        {
          ItemPrice lowestPrice = ShopUtils.GetLowestPrice((IUnityItem) key);
          if (key.Configuration.LevelLock <= playerLevel && keyValuePair.Value.Price > lowestPrice.Price)
            keyValuePair = new KeyValuePair<GearItem, ItemPrice>(key, lowestPrice);
          if (key.Configuration.LevelLock <= playerLevel && lowestPrice.Currency == UberStrikeCurrencyType.Points && lowestPrice.Price <= PlayerDataManager.Points)
          {
            if (flag)
              keyValuePairList.Add(new KeyValuePair<GearItem, ItemPrice>(key, lowestPrice));
          }
          else if (lowestPrice.Currency == UberStrikeCurrencyType.Credits && lowestPrice.Price <= PlayerDataManager.Credits && flag)
            keyValuePairList.Add(new KeyValuePair<GearItem, ItemPrice>(key, lowestPrice));
        }
      }
    }
    if (keyValuePairList.Count == 0)
    {
      if ((UnityEngine.Object) keyValuePair.Key != (UnityEngine.Object) null)
        return keyValuePair.Key;
      return Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.GearUpperBody) ? Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.GearUpperBody).Item as GearItem : Singleton<ItemManager>.Instance.GetGearItemInShop(0, UberstrikeItemClass.GearUpperBody) as GearItem;
    }
    try
    {
      keyValuePairList.Sort((IComparer<KeyValuePair<GearItem, ItemPrice>>) new ShopUtils.PriceComparer<GearItem>());
    }
    catch
    {
      throw;
    }
    return keyValuePairList[0].Key;
  }

  public static bool IsMeleeWeapon(IUnityItem view) => view != null && view.ItemView != null && view.ItemView.ItemClass == UberstrikeItemClass.WeaponMelee;

  public static bool IsInstantHitWeapon(IUnityItem view)
  {
    if (view == null || view.ItemView == null)
      return false;
    return view.ItemView.ItemClass == UberstrikeItemClass.WeaponHandgun || view.ItemView.ItemClass == UberstrikeItemClass.WeaponMachinegun || view.ItemView.ItemClass == UberstrikeItemClass.WeaponShotgun || view.ItemView.ItemClass == UberstrikeItemClass.WeaponSniperRifle;
  }

  public static bool IsProjectileWeapon(IUnityItem view)
  {
    if (view == null || view.ItemView == null)
      return false;
    return view.ItemView.ItemClass == UberstrikeItemClass.WeaponCannon || view.ItemView.ItemClass == UberstrikeItemClass.WeaponLauncher || view.ItemView.ItemClass == UberstrikeItemClass.WeaponSplattergun;
  }

  public static string GetRecommendationString(RecommendType recommendation)
  {
    switch (recommendation)
    {
      case RecommendType.MostEfficient:
        return LocalizedStrings.MostEfficientWeaponCaps;
      case RecommendType.RecommendedWeapon:
        return LocalizedStrings.RecommendedWeaponCaps;
      case RecommendType.RecommendedArmor:
        return LocalizedStrings.RecommendedArmorCaps;
      case RecommendType.StaffPick:
        return LocalizedStrings.StaffPickCaps;
      default:
        return string.Empty;
    }
  }

  public class PriceComparer<T> : IComparer<KeyValuePair<T, ItemPrice>>
  {
    public int Compare(KeyValuePair<T, ItemPrice> x, KeyValuePair<T, ItemPrice> y)
    {
      int num = x.Value.Price + (x.Value.Currency != UberStrikeCurrencyType.Credits ? 0 : 1000000);
      return (y.Value.Price + (y.Value.Currency != UberStrikeCurrencyType.Credits ? 0 : 1000000)).CompareTo(num);
    }
  }

  private class DescendedComparer : IComparer<int>
  {
    public int Compare(int x, int y) => y - x;
  }
}
