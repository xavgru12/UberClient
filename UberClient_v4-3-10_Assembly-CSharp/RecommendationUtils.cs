// Decompiled with JetBrains decompiler
// Type: RecommendationUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public static class RecommendationUtils
{
  public static RecommendationUtils.WeaponRecommendation GetRecommendedWeapon(
    int playerLevel,
    CombatRangeTier mapCombatRange,
    List<WeaponItem> loadout = null,
    List<WeaponItem> inventory = null)
  {
    if (loadout == null)
    {
      loadout = new List<WeaponItem>(4);
      if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponMelee))
        loadout.Add(Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponMelee).Item as WeaponItem);
      if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponPrimary))
        loadout.Add(Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponPrimary).Item as WeaponItem);
      if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponSecondary))
        loadout.Add(Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponSecondary).Item as WeaponItem);
      if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponTertiary))
        loadout.Add(Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponTertiary).Item as WeaponItem);
    }
    if (inventory == null)
    {
      inventory = new List<WeaponItem>();
      foreach (InventoryItem allItem in Singleton<InventoryManager>.Instance.GetAllItems(false))
      {
        if (allItem.Item is WeaponItem && (allItem.DaysRemaining > 0 || allItem.IsPermanent))
          inventory.Add(allItem.Item as WeaponItem);
      }
    }
    RecommendationUtils.WeaponRecommendation recommendedWeapon = RecommendationUtils.CheckMyLoadout(mapCombatRange, loadout, inventory);
    recommendedWeapon.Debug += "[RC: ";
    if (recommendedWeapon.IsComplete)
    {
      int num = 0;
      foreach (WeaponItem weakestLink in RecommendationUtils.GetWeakestItemsInLoadout(loadout, mapCombatRange))
      {
        KeyValuePair<WeaponItem, ItemPrice> affordableWeapon = RecommendationUtils.GetNextBestAffordableWeapon(weakestLink, playerLevel, inventory);
        if ((UnityEngine.Object) affordableWeapon.Key != (UnityEngine.Object) null)
        {
          recommendedWeapon.ItemWeapon = affordableWeapon.Key;
          recommendedWeapon.Price = affordableWeapon.Value;
          recommendedWeapon.Debug += string.Format("Better in Class, try:{0}] ", (object) num);
          return recommendedWeapon;
        }
        ++num;
      }
      KeyValuePair<WeaponItem, ItemPrice> nextBestWeapon = RecommendationUtils.GetNextBestWeapon(RecommendationUtils.GetWeakestItemsInLoadout(loadout, mapCombatRange)[0], inventory);
      if ((UnityEngine.Object) nextBestWeapon.Key != (UnityEngine.Object) null)
      {
        recommendedWeapon.ItemWeapon = nextBestWeapon.Key;
        recommendedWeapon.Price = nextBestWeapon.Value;
        recommendedWeapon.Debug += "Better in Class | too exp] ";
        return recommendedWeapon;
      }
      recommendedWeapon.Debug += "NULL] ";
      return recommendedWeapon;
    }
    if ((UnityEngine.Object) recommendedWeapon.ItemWeapon == (UnityEngine.Object) null)
    {
      KeyValuePair<WeaponItem, ItemPrice> additionalWeapon = RecommendationUtils.GetAdditionalWeapon(recommendedWeapon.CombatRange, playerLevel, inventory, loadout);
      recommendedWeapon.ItemWeapon = additionalWeapon.Key;
      recommendedWeapon.Price = additionalWeapon.Value;
      recommendedWeapon.Debug += "Add Weapon] ";
      return recommendedWeapon;
    }
    recommendedWeapon.Debug += "None] ";
    return recommendedWeapon;
  }

  public static string PrintDPS()
  {
    StringBuilder stringBuilder = new StringBuilder();
    Dictionary<UberstrikeItemClass, List<WeaponItem>> dictionary = new Dictionary<UberstrikeItemClass, List<WeaponItem>>();
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      if (dictionary.ContainsKey(shopItem.ItemClass))
        dictionary[shopItem.ItemClass].Add(shopItem);
      else
        dictionary.Add(shopItem.ItemClass, new List<WeaponItem>()
        {
          shopItem
        });
    }
    foreach (List<WeaponItem> weaponItemList in dictionary.Values)
      weaponItemList.Sort((Comparison<WeaponItem>) ((a, b) => b.Configuration.DPS.CompareTo(a.Configuration.DPS)));
    foreach (KeyValuePair<UberstrikeItemClass, List<WeaponItem>> keyValuePair in dictionary)
    {
      stringBuilder.AppendLine("+++" + (object) keyValuePair.Key + "+++");
      foreach (WeaponItem weaponItem in keyValuePair.Value)
        stringBuilder.AppendLine("Level: " + (object) weaponItem.Configuration.LevelLock + "\t Name: " + weaponItem.Name + " [" + (object) weaponItem.ItemClass + "] \t DPS: " + (object) weaponItem.Configuration.DPS + "\t Tier: " + (object) weaponItem.Configuration.Tier);
    }
    return stringBuilder.ToString();
  }

  public static WeaponItem FallBackWeapon => Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponMelee).Item as WeaponItem;

  public static List<CombatRangeCategory> GetCategoriesForCombatRange(CombatRangeTier mapCombatRange)
  {
    int closeRange = mapCombatRange.CloseRange;
    int num1 = mapCombatRange.MediumRange;
    int longRange = mapCombatRange.LongRange;
    List<CombatRangeCategory> categoriesForCombatRange = new List<CombatRangeCategory>(3);
    if (closeRange > 0)
    {
      categoriesForCombatRange.Add(CombatRangeCategory.Close);
      --closeRange;
    }
    if (num1 > 0)
    {
      categoriesForCombatRange.Add(CombatRangeCategory.Medium);
      --num1;
    }
    if (longRange > 0)
      categoriesForCombatRange.Add(CombatRangeCategory.Far);
    int num2 = 3 - categoriesForCombatRange.Count;
    for (int index = 0; index < num2; ++index)
    {
      if (closeRange > num1)
      {
        categoriesForCombatRange.Add(CombatRangeCategory.Close);
        --closeRange;
      }
      else
      {
        categoriesForCombatRange.Add(CombatRangeCategory.Medium);
        num1 = Mathf.Max(num1 - 1, 0);
      }
    }
    categoriesForCombatRange.Sort((Comparison<CombatRangeCategory>) ((i, j) => mapCombatRange.GetTierForRange(j).CompareTo(mapCombatRange.GetTierForRange(i))));
    return categoriesForCombatRange;
  }

  private static WeaponItem GetBestItemForRange(
    CombatRangeCategory range,
    IEnumerable<WeaponItem> items)
  {
    WeaponItem bestItemForRange = (WeaponItem) null;
    foreach (WeaponItem weaponItem in items)
    {
      if ((weaponItem.Configuration.CombatRange & range) != (CombatRangeCategory) 0 && ((UnityEngine.Object) bestItemForRange == (UnityEngine.Object) null || weaponItem.Configuration.Tier > bestItemForRange.Configuration.Tier))
        bestItemForRange = weaponItem;
    }
    return bestItemForRange;
  }

  public static void DebugRecommendation(
    RecommendationUtils.WeaponRecommendation recommendation)
  {
    if ((UnityEngine.Object) recommendation.ItemWeapon != (UnityEngine.Object) null)
      Debug.Log((object) (recommendation.Debug + recommendation.ItemWeapon.Name + " " + (object) recommendation.ItemWeapon.ItemClass + "/" + (object) recommendation.ItemWeapon.Configuration.Tier + ", " + (object) recommendation.CombatRange + ", Slot: " + (object) recommendation.LoadoutSlot + " " + (object) recommendation.Price.Price + ", Level: " + (object) recommendation.ItemWeapon.ItemView.LevelLock));
    else
      Debug.Log((object) (recommendation.Debug + " NIL " + (object) recommendation.CombatRange + " " + (object) recommendation.LoadoutSlot + ", isComplete: " + (object) recommendation.IsComplete));
  }

  public static LoadoutSlotType FindBestSlotToEquipWeapon(
    WeaponItem weapon,
    List<CombatRangeCategory> ranges)
  {
    if ((UnityEngine.Object) weapon != (UnityEngine.Object) null)
    {
      if (weapon.ItemClass == UberstrikeItemClass.WeaponMelee)
        return LoadoutSlotType.WeaponMelee;
      Dictionary<LoadoutSlotType, WeaponItem> dictionary = new Dictionary<LoadoutSlotType, WeaponItem>()
      {
        {
          LoadoutSlotType.WeaponPrimary,
          Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponPrimary).Item as WeaponItem
        },
        {
          LoadoutSlotType.WeaponSecondary,
          Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponSecondary).Item as WeaponItem
        },
        {
          LoadoutSlotType.WeaponTertiary,
          Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponTertiary).Item as WeaponItem
        }
      };
      foreach (KeyValuePair<LoadoutSlotType, WeaponItem> keyValuePair in dictionary)
      {
        if ((UnityEngine.Object) keyValuePair.Value != (UnityEngine.Object) null && keyValuePair.Value.ItemClass == weapon.ItemClass)
          return keyValuePair.Key;
      }
      foreach (KeyValuePair<LoadoutSlotType, WeaponItem> keyValuePair in dictionary)
      {
        KeyValuePair<LoadoutSlotType, WeaponItem> i = keyValuePair;
        if ((UnityEngine.Object) i.Value == (UnityEngine.Object) null || ranges.TrueForAll((Predicate<CombatRangeCategory>) (r => (r & i.Value.Configuration.CombatRange) == (CombatRangeCategory) 0)))
          return i.Key;
      }
      foreach (KeyValuePair<LoadoutSlotType, WeaponItem> keyValuePair in dictionary)
      {
        KeyValuePair<LoadoutSlotType, WeaponItem> i = keyValuePair;
        if ((UnityEngine.Object) i.Value != (UnityEngine.Object) null)
        {
          CombatRangeCategory combatRangeCategory = ranges.Find((Predicate<CombatRangeCategory>) (r => r == i.Value.Configuration.CombatRange));
          if (combatRangeCategory == (CombatRangeCategory) 0)
            return i.Key;
          ranges.Remove(combatRangeCategory);
        }
      }
    }
    return LoadoutSlotType.None;
  }

  private static RecommendationUtils.WeaponRecommendation CheckMyLoadout(
    CombatRangeTier mapCombatRange,
    List<WeaponItem> loadout,
    List<WeaponItem> inventory)
  {
    loadout.RemoveAll((Predicate<WeaponItem>) (w => (UnityEngine.Object) w == (UnityEngine.Object) null));
    RecommendationUtils.WeaponRecommendation weaponRecommendation = new RecommendationUtils.WeaponRecommendation();
    weaponRecommendation.Debug += "[LC: ";
    Dictionary<UberstrikeItemClass, WeaponItem> dictionary = new Dictionary<UberstrikeItemClass, WeaponItem>();
    foreach (WeaponItem weaponItem1 in inventory)
    {
      WeaponItem weaponItem2;
      if (dictionary.TryGetValue(weaponItem1.ItemClass, out weaponItem2))
      {
        if (weaponItem2.Configuration.Tier < weaponItem1.Configuration.Tier || weaponItem2.Configuration.Tier == weaponItem1.Configuration.Tier && (double) weaponItem2.Configuration.DPS < (double) weaponItem1.Configuration.DPS)
          dictionary[weaponItem1.ItemClass] = weaponItem1;
      }
      else
        dictionary[weaponItem1.ItemClass] = weaponItem1;
    }
    HashSet<UberstrikeItemClass> equippedItemClasses = new HashSet<UberstrikeItemClass>();
    foreach (WeaponItem weaponItem in loadout)
      equippedItemClasses.Add(weaponItem.ItemClass);
    if (!equippedItemClasses.Contains(UberstrikeItemClass.WeaponMelee))
    {
      weaponRecommendation.ItemWeapon = dictionary[UberstrikeItemClass.WeaponMelee];
      weaponRecommendation.CombatRange = CombatRangeCategory.Close;
      weaponRecommendation.LoadoutSlot = LoadoutSlotType.WeaponMelee;
      weaponRecommendation.Debug += " No Melee] ";
      return weaponRecommendation;
    }
    List<int> processedItemIds = new List<int>();
    WeaponItem weaponItem3 = loadout.Find((Predicate<WeaponItem>) (w => w.ItemClass == UberstrikeItemClass.WeaponMelee));
    if ((UnityEngine.Object) weaponItem3 != (UnityEngine.Object) null)
      processedItemIds.Add(weaponItem3.ItemId);
    List<CombatRangeCategory> categoriesForCombatRange = RecommendationUtils.GetCategoriesForCombatRange(mapCombatRange);
    foreach (CombatRangeCategory combatRangeCategory in categoriesForCombatRange)
    {
      CombatRangeCategory range = combatRangeCategory;
      WeaponItem weaponItem4 = loadout.Find((Predicate<WeaponItem>) (w => (w.Configuration.CombatRange & range) == w.Configuration.CombatRange && !processedItemIds.Contains(w.ItemId)));
      if ((UnityEngine.Object) weaponItem4 == (UnityEngine.Object) null)
        weaponItem4 = loadout.Find((Predicate<WeaponItem>) (w => (w.Configuration.CombatRange & range) != (CombatRangeCategory) 0 && !processedItemIds.Contains(w.ItemId)));
      if ((UnityEngine.Object) weaponItem4 != (UnityEngine.Object) null)
      {
        processedItemIds.Add(weaponItem4.ItemId);
      }
      else
      {
        List<WeaponItem> items = new List<WeaponItem>((IEnumerable<WeaponItem>) dictionary.Values);
        items.RemoveAll((Predicate<WeaponItem>) (w => equippedItemClasses.Contains(w.ItemClass)));
        WeaponItem bestItemForRange = RecommendationUtils.GetBestItemForRange(range, (IEnumerable<WeaponItem>) items);
        weaponRecommendation.ItemWeapon = bestItemForRange;
        weaponRecommendation.CombatRange = range;
        weaponRecommendation.LoadoutSlot = RecommendationUtils.FindBestSlotToEquipWeapon(bestItemForRange, categoriesForCombatRange);
        weaponRecommendation.Debug += " Uncovered Range] ";
        return weaponRecommendation;
      }
    }
    foreach (WeaponItem weaponItem5 in loadout)
    {
      WeaponItem weapon;
      if (dictionary.TryGetValue(weaponItem5.ItemClass, out weapon) && weapon.Configuration.Tier > weaponItem5.Configuration.Tier)
      {
        weaponRecommendation.ItemWeapon = weapon;
        weaponRecommendation.CombatRange = weapon.Configuration.CombatRange;
        weaponRecommendation.LoadoutSlot = RecommendationUtils.FindBestSlotToEquipWeapon(weapon, categoriesForCombatRange);
        weaponRecommendation.Debug += " Better Inventory] ";
        return weaponRecommendation;
      }
    }
    weaponRecommendation.LoadoutSlot = LoadoutSlotType.None;
    weaponRecommendation.IsComplete = true;
    weaponRecommendation.Debug += " OK] ";
    return weaponRecommendation;
  }

  private static List<WeaponItem> GetWeakestItemsInLoadout(
    List<WeaponItem> loadout,
    CombatRangeTier combatRange)
  {
    List<WeaponItem> weakestItemsInLoadout1 = new List<WeaponItem>((IEnumerable<WeaponItem>) loadout);
    weakestItemsInLoadout1.Sort((Comparison<WeaponItem>) ((i, j) =>
    {
      int weakestItemsInLoadout2 = i.Configuration.Tier.CompareTo(j.Configuration.Tier);
      if (weakestItemsInLoadout2 == 0)
        weakestItemsInLoadout2 = combatRange.GetTierForRange(j.Configuration.CombatRange).CompareTo(combatRange.GetTierForRange(i.Configuration.CombatRange));
      return weakestItemsInLoadout2;
    }));
    return weakestItemsInLoadout1;
  }

  private static KeyValuePair<WeaponItem, ItemPrice> GetAdditionalWeapon(
    CombatRangeCategory range,
    int playerLevel,
    List<WeaponItem> inventory,
    List<WeaponItem> loadout)
  {
    HashSet<UberstrikeItemClass> uberstrikeItemClassSet = new HashSet<UberstrikeItemClass>();
    foreach (WeaponItem weaponItem in loadout)
      uberstrikeItemClassSet.Add(weaponItem.ItemClass);
    playerLevel = Mathf.Max(2, playerLevel);
    List<KeyValuePair<WeaponItem, ItemPrice>> keyValuePairList = new List<KeyValuePair<WeaponItem, ItemPrice>>();
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      WeaponItem weapon = shopItem;
      if ((weapon.Configuration.CombatRange & range) != (CombatRangeCategory) 0 && inventory != null && !inventory.Exists((Predicate<WeaponItem>) (w => w.ItemId == weapon.ItemId)) && !uberstrikeItemClassSet.Contains(weapon.ItemClass))
      {
        ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Credits);
        ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Points);
        if (lowestPrice2 != null && lowestPrice2.Price > 0 && lowestPrice2.Price <= PlayerDataManager.Points && weapon.ItemView.LevelLock <= playerLevel)
          keyValuePairList.Add(new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice2));
        else if (lowestPrice1 != null && lowestPrice1.Price > 0 && lowestPrice1.Price <= PlayerDataManager.Credits)
          keyValuePairList.Add(new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice1));
      }
    }
    if (keyValuePairList.Count <= 0)
      return RecommendationUtils.GetNextBestWeapon(range, inventory);
    keyValuePairList.Sort((IComparer<KeyValuePair<WeaponItem, ItemPrice>>) new ShopUtils.PriceComparer<WeaponItem>());
    return keyValuePairList[0];
  }

  private static KeyValuePair<WeaponItem, ItemPrice> GetNextBestAffordableWeapon(
    WeaponItem weakestLink,
    int playerLevel,
    List<WeaponItem> inventory)
  {
    playerLevel = Mathf.Max(2, playerLevel);
    List<KeyValuePair<WeaponItem, ItemPrice>> keyValuePairList = new List<KeyValuePair<WeaponItem, ItemPrice>>();
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      WeaponItem weapon = shopItem;
      if (weapon.ItemClass == weakestLink.ItemClass && !inventory.Exists((Predicate<WeaponItem>) (w => w.ItemId == weapon.ItemId)) && (weapon.Configuration.Tier > weakestLink.Configuration.Tier || weapon.Configuration.Tier == weakestLink.Configuration.Tier && (double) weapon.Configuration.DPS > (double) weakestLink.Configuration.DPS))
      {
        ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Credits);
        ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Points);
        if (lowestPrice2 != null && lowestPrice2.Price > 0 && lowestPrice2.Price <= PlayerDataManager.Points && weapon.ItemView.LevelLock <= playerLevel)
          keyValuePairList.Add(new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice2));
        else if (lowestPrice1 != null && lowestPrice1.Price > 0 && lowestPrice1.Price <= PlayerDataManager.Credits)
          keyValuePairList.Add(new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice1));
      }
    }
    if (keyValuePairList.Count <= 0)
      return new KeyValuePair<WeaponItem, ItemPrice>();
    keyValuePairList.Sort((IComparer<KeyValuePair<WeaponItem, ItemPrice>>) new ShopUtils.PriceComparer<WeaponItem>());
    return keyValuePairList[0];
  }

  private static KeyValuePair<WeaponItem, ItemPrice> GetNextBestWeapon(
    WeaponItem weakestLink,
    List<WeaponItem> inventory)
  {
    KeyValuePair<WeaponItem, ItemPrice> nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>();
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      WeaponItem weapon = shopItem;
      if (weapon.ItemClass == weakestLink.ItemClass && !inventory.Exists((Predicate<WeaponItem>) (w => w.ItemId == weapon.ItemId)) && (weapon.Configuration.Tier > weakestLink.Configuration.Tier || weapon.Configuration.Tier == weakestLink.Configuration.Tier && (double) weapon.Configuration.DPS > (double) weakestLink.Configuration.DPS))
      {
        ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Credits);
        ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Points);
        if (lowestPrice2 != null && lowestPrice2.Price > 0 && ((UnityEngine.Object) nextBestWeapon.Key == (UnityEngine.Object) null || (double) nextBestWeapon.Key.Configuration.DPS > (double) weapon.Configuration.DPS))
          nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice2);
        else if (lowestPrice1 != null && lowestPrice1.Price > 0 && ((UnityEngine.Object) nextBestWeapon.Key == (UnityEngine.Object) null || (double) nextBestWeapon.Key.Configuration.DPS > (double) weapon.Configuration.DPS))
          nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice1);
      }
    }
    return nextBestWeapon;
  }

  private static KeyValuePair<WeaponItem, ItemPrice> GetNextBestWeapon(
    CombatRangeCategory range,
    List<WeaponItem> inventory)
  {
    KeyValuePair<WeaponItem, ItemPrice> nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>();
    foreach (WeaponItem shopItem in Singleton<ItemManager>.Instance.GetShopItems(UberstrikeItemType.Weapon, BuyingMarketType.Shop))
    {
      WeaponItem weapon = shopItem;
      if ((weapon.Configuration.CombatRange & range) != (CombatRangeCategory) 0 && inventory != null && !inventory.Exists((Predicate<WeaponItem>) (w => w.ItemId == weapon.ItemId)))
      {
        ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Credits);
        ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice((IUnityItem) weapon, UberStrikeCurrencyType.Points);
        if (lowestPrice2 != null && lowestPrice2.Price > 0 && ((UnityEngine.Object) nextBestWeapon.Key == (UnityEngine.Object) null || (double) nextBestWeapon.Key.Configuration.DPS > (double) weapon.Configuration.DPS))
          nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice2);
        else if (lowestPrice1 != null && lowestPrice1.Price > 0 && ((UnityEngine.Object) nextBestWeapon.Key == (UnityEngine.Object) null || (double) nextBestWeapon.Key.Configuration.DPS > (double) weapon.Configuration.DPS))
          nextBestWeapon = new KeyValuePair<WeaponItem, ItemPrice>(weapon, lowestPrice1);
      }
    }
    return nextBestWeapon;
  }

  public class WeaponRecommendation
  {
    public string Debug;

    public bool IsComplete { get; set; }

    public WeaponItem ItemWeapon { get; set; }

    public CombatRangeCategory CombatRange { get; set; }

    public LoadoutSlotType LoadoutSlot { get; set; }

    public ItemPrice Price { get; set; }
  }
}
