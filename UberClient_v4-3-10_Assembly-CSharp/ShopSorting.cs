// Decompiled with JetBrains decompiler
// Type: ShopSorting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public static class ShopSorting
{
  private static int CompareDuration(InventoryItem a, InventoryItem b, bool ascending)
  {
    if (a.IsPermanent && b.IsPermanent)
      return ShopSorting.CompareName(a.Item, b.Item, ascending);
    if (a.IsPermanent)
      return ascending ? 1 : -1;
    if (b.IsPermanent)
      return ascending ? -1 : 1;
    if (a.DaysRemaining > b.DaysRemaining)
      return ascending ? 1 : -1;
    if (a.DaysRemaining >= b.DaysRemaining)
      return ShopSorting.CompareName(a.Item, b.Item, ascending);
    return ascending ? -1 : 1;
  }

  private static int ComparePrice(IUnityItem a, IUnityItem b, bool ascending)
  {
    ItemPrice lowestPrice1 = ShopUtils.GetLowestPrice(a);
    ItemPrice lowestPrice2 = ShopUtils.GetLowestPrice(b);
    if (lowestPrice1.Currency == lowestPrice2.Currency)
    {
      if (lowestPrice1.Price > lowestPrice2.Price)
        return ascending ? 1 : -1;
      if (lowestPrice1.Price >= lowestPrice2.Price)
        return ShopSorting.CompareName(a, b, ascending);
      return ascending ? -1 : 1;
    }
    return lowestPrice1.Currency == UberStrikeCurrencyType.Credits ? -1 : 1;
  }

  private static int CompareLevel(IUnityItem a, IUnityItem b, bool ascending)
  {
    if (a.ItemView.LevelLock < b.ItemView.LevelLock)
      return ascending ? -1 : 1;
    if (a.ItemView.LevelLock <= b.ItemView.LevelLock)
      return ShopSorting.CompareName(a, b, ascending);
    return ascending ? 1 : -1;
  }

  private static int CompareClass(IUnityItem a, IUnityItem b, bool ascending)
  {
    int num1 = !ascending ? -1 : 1;
    int num2 = a.ItemType != UberstrikeItemType.Weapon ? 100 : 10;
    int num3 = b.ItemType != UberstrikeItemType.Weapon ? 100 : 10;
    switch (a.ItemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        num2 += 7;
        break;
      case UberstrikeItemClass.WeaponHandgun:
        num2 += 6;
        break;
      case UberstrikeItemClass.WeaponMachinegun:
        num2 = num2;
        break;
      case UberstrikeItemClass.WeaponShotgun:
        num2 += 4;
        break;
      case UberstrikeItemClass.WeaponSniperRifle:
        num2 += 2;
        break;
      case UberstrikeItemClass.WeaponCannon:
        ++num2;
        break;
      case UberstrikeItemClass.WeaponSplattergun:
        num2 += 5;
        break;
      case UberstrikeItemClass.WeaponLauncher:
        num2 += 3;
        break;
      case UberstrikeItemClass.GearBoots:
        num2 += 4;
        break;
      case UberstrikeItemClass.GearHead:
        num2 = num2;
        break;
      case UberstrikeItemClass.GearFace:
        ++num2;
        break;
      case UberstrikeItemClass.GearUpperBody:
        num2 += 2;
        break;
      case UberstrikeItemClass.GearLowerBody:
        num2 += 3;
        break;
      case UberstrikeItemClass.GearGloves:
        num2 += 5;
        break;
      case UberstrikeItemClass.GearHolo:
        num2 += 6;
        break;
    }
    switch (b.ItemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        num3 += 7;
        break;
      case UberstrikeItemClass.WeaponHandgun:
        num3 += 6;
        break;
      case UberstrikeItemClass.WeaponMachinegun:
        num3 = num3;
        break;
      case UberstrikeItemClass.WeaponShotgun:
        num3 += 4;
        break;
      case UberstrikeItemClass.WeaponSniperRifle:
        num3 += 2;
        break;
      case UberstrikeItemClass.WeaponCannon:
        ++num3;
        break;
      case UberstrikeItemClass.WeaponSplattergun:
        num3 += 5;
        break;
      case UberstrikeItemClass.WeaponLauncher:
        num3 += 3;
        break;
      case UberstrikeItemClass.GearBoots:
        num3 += 4;
        break;
      case UberstrikeItemClass.GearHead:
        num3 = num3;
        break;
      case UberstrikeItemClass.GearFace:
        ++num3;
        break;
      case UberstrikeItemClass.GearUpperBody:
        num3 += 2;
        break;
      case UberstrikeItemClass.GearLowerBody:
        num3 += 3;
        break;
      case UberstrikeItemClass.GearGloves:
        num3 += 5;
        break;
      case UberstrikeItemClass.GearHolo:
        num3 += 6;
        break;
    }
    if (num2 == num3)
      return 0;
    return num2 > num3 ? num1 : num1 * -1;
  }

  private static int CompareName(IUnityItem a, IUnityItem b, bool ascending) => ascending ? string.Compare(a.ItemView.Name, b.ItemView.Name) : string.Compare(b.ItemView.Name, a.ItemView.Name);

  private static int CompareTag(IUnityItem a, IUnityItem b, bool ascending)
  {
    int num = !ascending ? -1 : 1;
    if (a.ItemView.ShopHighlightType == b.ItemView.ShopHighlightType)
      return 0;
    return a.ItemView.ShopHighlightType > b.ItemView.ShopHighlightType ? num : num * -1;
  }

  private static int CompareArmorPoints(IUnityItem a, IUnityItem b, bool ascending)
  {
    int num = !ascending ? -1 : 1;
    int armorPoints1 = a.ItemType != UberstrikeItemType.Gear ? 0 : ((GearItem) a).Configuration.ArmorPoints;
    int armorPoints2 = b.ItemType != UberstrikeItemType.Gear ? 0 : ((GearItem) b).Configuration.ArmorPoints;
    if (armorPoints1 == armorPoints2)
      return 0;
    return armorPoints1 > armorPoints2 ? num : num * -1;
  }

  public abstract class ItemComparer<T> : IComparer<T>
  {
    public bool Ascending { get; protected set; }

    public ShopSortedColumns Column { get; set; }

    public void SwitchOrder() => this.Ascending = !this.Ascending;

    public abstract int Compare(T a, T b);
  }

  public class PriceComparer : ShopSorting.ItemComparer<BaseItemGUI>
  {
    public PriceComparer()
    {
      this.Column = ShopSortedColumns.PriceShop;
      this.Ascending = false;
    }

    public override int Compare(BaseItemGUI a, BaseItemGUI b)
    {
      int num = ShopSorting.CompareTag(a.Item, b.Item, false);
      if (num == 0)
        num = ShopSorting.ComparePrice(a.Item, b.Item, this.Ascending);
      return num;
    }
  }

  public class NameComparer : ShopSorting.ItemComparer<BaseItemGUI>
  {
    public NameComparer()
    {
      this.Column = ShopSortedColumns.Name;
      this.Ascending = true;
    }

    public override int Compare(BaseItemGUI a, BaseItemGUI b) => ShopSorting.CompareName(a.Item, b.Item, this.Ascending);
  }

  public class LevelComparer : ShopSorting.ItemComparer<BaseItemGUI>
  {
    public LevelComparer()
    {
      this.Column = ShopSortedColumns.Level;
      this.Ascending = true;
    }

    public override int Compare(BaseItemGUI a, BaseItemGUI b) => ShopSorting.CompareLevel(a.Item, b.Item, this.Ascending);
  }

  public class DurationComparer : ShopSorting.ItemComparer<BaseItemGUI>
  {
    public DurationComparer()
    {
      this.Column = ShopSortedColumns.Duration;
      this.Ascending = false;
    }

    public override int Compare(BaseItemGUI a, BaseItemGUI b) => this.Compare(a as InventoryItemGUI, b as InventoryItemGUI);

    public int Compare(InventoryItemGUI a, InventoryItemGUI b) => ShopSorting.CompareDuration(a.InventoryItem, b.InventoryItem, this.Ascending);
  }
}
