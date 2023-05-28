// Decompiled with JetBrains decompiler
// Type: ItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
  public const string DefaultHeadPrefabName = "LutzDefaultGearHead";
  public const string DefaultGlovesPrefabName = "LutzDefaultGearGloves";
  public const string DefaultUpperBodyPrefabName = "LutzDefaultGearUpperBody";
  public const string DefaultLowerBodyPrefabName = "LutzDefaultGearLowerBody";
  public const string DefaultBootsPrefabName = "LutzDefaultGearBoots";
  public const string DefaulMeleePrefabName = "TheSplatbat";
  public const string DefaultHandGunPrefabName = "HandGun";
  public const string DefaultMachineGunPrefabName = "MachineGun";
  public const string DefaultSplatterGunPrefabName = "SplatterGun";
  public const string DefaultCannonPrefabName = "Cannon";
  public const string DefaultSniperRiflePrefabName = "SniperRifle";
  public const string DefaultLauncherPrefabName = "Launcher";
  public const string DefaultShotGunPrefabName = "ShotGun";
  private Dictionary<UberstrikeItemClass, string> _defaultGearPrefabNames;
  private Dictionary<UberstrikeItemClass, string> _defaultWeaponPrefabNames;
  private Dictionary<int, IUnityItem> _shopItems;

  private ItemManager()
  {
    this._shopItems = new Dictionary<int, IUnityItem>();
    this._defaultGearPrefabNames = new Dictionary<UberstrikeItemClass, string>()
    {
      {
        UberstrikeItemClass.GearHead,
        "LutzDefaultGearHead"
      },
      {
        UberstrikeItemClass.GearGloves,
        "LutzDefaultGearGloves"
      },
      {
        UberstrikeItemClass.GearUpperBody,
        "LutzDefaultGearUpperBody"
      },
      {
        UberstrikeItemClass.GearLowerBody,
        "LutzDefaultGearLowerBody"
      },
      {
        UberstrikeItemClass.GearBoots,
        "LutzDefaultGearBoots"
      }
    };
    this._defaultWeaponPrefabNames = new Dictionary<UberstrikeItemClass, string>()
    {
      {
        UberstrikeItemClass.WeaponMelee,
        "TheSplatbat"
      },
      {
        UberstrikeItemClass.WeaponHandgun,
        "HandGun"
      },
      {
        UberstrikeItemClass.WeaponMachinegun,
        "MachineGun"
      },
      {
        UberstrikeItemClass.WeaponSplattergun,
        "SplatterGun"
      },
      {
        UberstrikeItemClass.WeaponCannon,
        "Cannon"
      },
      {
        UberstrikeItemClass.WeaponSniperRifle,
        "SniperRifle"
      },
      {
        UberstrikeItemClass.WeaponLauncher,
        "Launcher"
      },
      {
        UberstrikeItemClass.WeaponShotgun,
        "ShotGun"
      }
    };
  }

  public IEnumerable<IUnityItem> ShopItems => (IEnumerable<IUnityItem>) this._shopItems.Values;

  public int ShopItemCount => this._shopItems.Count;

  public int DefaultHeadItemId => 1084;

  public int DefaultGlovesItemId => 1086;

  public int DefaultUpperBodyItemId => 1087;

  public int DefaultLowerBodyItemId => 1088;

  public int DefaultBootsItemId => 1089;

  private void UpdateShopItems(UberStrikeItemShopClientView shopView)
  {
    foreach (BaseUberStrikeItemView gearItem in shopView.GearItems)
      this.AddItemToShop(gearItem);
    foreach (BaseUberStrikeItemView weaponItem in shopView.WeaponItems)
      this.AddItemToShop(weaponItem);
    foreach (BaseUberStrikeItemView functionalItem in shopView.FunctionalItems)
      this.AddItemToShop(functionalItem);
    foreach (BaseUberStrikeItemView quickItem in shopView.QuickItems)
      this.AddItemToShop(quickItem);
    CmuneEventHandler.Route((object) new ShopRefreshCurrentItemListEvent());
  }

  internal GameObject GetPrefab(int itemId)
  {
    IUnityItem unityItem = (IUnityItem) null;
    return this._shopItems.TryGetValue(itemId, out unityItem) ? ((Component) unityItem).gameObject : (GameObject) null;
  }

  public void SetPrefab(IUnityItem item)
  {
    this._shopItems[item.ItemId] = item;
    AutoMonoBehaviour<UnityItemHolder>.Instance.Add(item, item.Prefab.name);
  }

  internal GameObject Instantiate(int itemId) => UnityEngine.Object.Instantiate((UnityEngine.Object) this.GetPrefab(itemId)) as GameObject;

  public void AddUnityItems(IUnityItem[] unityItems)
  {
    if (unityItems == null)
      return;
    foreach (IUnityItem unityItem in unityItems)
    {
      if (unityItem is QuickItem)
      {
        QuickItem quickItem = unityItem as QuickItem;
        if ((bool) (UnityEngine.Object) quickItem.Sfx)
          Singleton<QuickItemSfxController>.Instance.RegisterQuickItemEffect(quickItem.Logic, quickItem.Sfx);
      }
      if ((bool) (UnityEngine.Object) unityItem.Prefab)
      {
        string name = unityItem.Prefab.name;
        AutoMonoBehaviour<UnityItemHolder>.Instance.Add(unityItem, name);
      }
      else
        UnityEngine.Debug.Log((object) ("Failed to add unity item: " + (object) unityItem.ItemId));
    }
  }

  public void AddFunctionalItems(IUnityItem[] items)
  {
    foreach (IUnityItem unityItem in items)
      this._shopItems[unityItem.ItemId] = unityItem;
  }

  public bool AddItemToShop(BaseUberStrikeItemView itemView)
  {
    if (itemView != null)
    {
      if (itemView.ItemClass == UberstrikeItemClass.FunctionalGeneral)
      {
        IUnityItem unityItem;
        if (this._shopItems.TryGetValue(itemView.ID, out unityItem))
          ItemConfigurationUtil.CopyProperties<BaseUberStrikeItemView>(unityItem.ItemView, itemView);
      }
      else if (string.IsNullOrEmpty(itemView.PrefabName))
      {
        UnityEngine.Debug.LogWarning((object) ("Missing PrefabName for item: " + itemView.Name));
      }
      else
      {
        IUnityItem unityItem;
        if (AutoMonoBehaviour<UnityItemHolder>.Instance.Prefabs.TryGetValue(itemView.PrefabName, out unityItem))
        {
          if (unityItem is QuickItem)
            unityItem = AutoMonoBehaviour<UnityItemHolder>.Instance.CopyQuickItem(unityItem as QuickItem);
          ItemConfigurationUtil.CopyProperties<BaseUberStrikeItemView>(unityItem.ItemView, itemView);
          this._shopItems[unityItem.ItemId] = unityItem;
          return true;
        }
        UnityEngine.Debug.LogError((object) ("Missing UnityItem for: '" + itemView.Name + "' with PrefabName: '" + itemView.PrefabName + "'"));
      }
    }
    return false;
  }

  public static bool IsItemEquippable(IUnityItem item) => item != null && item.ItemType != UberstrikeItemType.Functional;

  private int GetDefaultItemId(UberstrikeItemClass itemClass)
  {
    IUnityItem unityItem;
    return this.TryGetDefaultItem(itemClass, out unityItem) ? unityItem.ItemId : 0;
  }

  public bool TryGetDefaultItem(UberstrikeItemClass itemClass, out IUnityItem item)
  {
    string prefabName;
    if (this._defaultGearPrefabNames.TryGetValue(itemClass, out prefabName) || this._defaultWeaponPrefabNames.TryGetValue(itemClass, out prefabName))
    {
      item = this._shopItems.Values.FirstOrDefault<IUnityItem>((Func<IUnityItem, bool>) (i => i.ItemView.PrefabName == prefabName));
      return item != null;
    }
    item = (IUnityItem) null;
    return false;
  }

  public bool IsDefaultGearItem(string prefabName) => this._defaultGearPrefabNames.ContainsValue(prefabName);

  public GearItem GetDefaultGearItem(UberstrikeItemClass itemClass)
  {
    IUnityItem defaultGearItem;
    this.TryGetDefaultItem(itemClass, out defaultGearItem);
    return defaultGearItem as GearItem;
  }

  public List<IUnityItem> GetShopItems(UberstrikeItemType itemType, BuyingMarketType marketType)
  {
    List<IUnityItem> shopItems = this.GetShopItems();
    shopItems.RemoveAll((Predicate<IUnityItem>) (item => item.ItemType != itemType));
    return shopItems;
  }

  public List<IUnityItem> GetShopItems(UberstrikeItemType itemType, UberstrikeItemClass itemClass)
  {
    List<IUnityItem> shopItems = this.GetShopItems();
    shopItems.RemoveAll((Predicate<IUnityItem>) (item => item.ItemType != itemType && item.ItemClass != itemClass));
    return shopItems;
  }

  public List<IUnityItem> GetShopItems()
  {
    List<IUnityItem> shopItems = new List<IUnityItem>((IEnumerable<IUnityItem>) this._shopItems.Values);
    shopItems.RemoveAll((Predicate<IUnityItem>) (item => !item.ItemView.IsForSale));
    return shopItems;
  }

  public List<IUnityItem> GetFeaturedShopItems()
  {
    List<IUnityItem> shopItems = this.GetShopItems();
    shopItems.RemoveAll((Predicate<IUnityItem>) (i => i.ItemView.ShopHighlightType == ItemShopHighlightType.None));
    return shopItems;
  }

  public IUnityItem GetItemInShop(int itemId) => this.GetShopItemOfType<IUnityItem>(itemId);

  public IUnityItem GetGearItemInShop(int itemId, UberstrikeItemClass itemClass) => this.GetShopItemOfType<IUnityItem>(itemId) ?? (IUnityItem) this.GetDefaultGearItem(itemClass);

  public WeaponItem GetWeaponItemInShop(int itemId) => this.GetShopItemOfType<WeaponItem>(itemId);

  public QuickItem GetQuickItemInShop(int itemId) => this.GetShopItemOfType<QuickItem>(itemId);

  public FunctionalItem GetFunctionalItemInShop(int itemId) => this.GetShopItemOfType<FunctionalItem>(itemId);

  private T GetShopItemOfType<T>(int itemID) where T : class, IUnityItem
  {
    IUnityItem shopItemOfType;
    this._shopItems.TryGetValue(itemID, out shopItemOfType);
    return shopItemOfType as T;
  }

  public bool ValidateItemMall() => this._shopItems.Count > 0;

  [DebuggerHidden]
  public IEnumerator StartGetShop() => (IEnumerator) new ItemManager.\u003CStartGetShop\u003Ec__Iterator6E()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartGetInventory(bool showProgress) => (IEnumerator) new ItemManager.\u003CStartGetInventory\u003Ec__Iterator6F()
  {
    showProgress = showProgress,
    \u003C\u0024\u003EshowProgress = showProgress,
    \u003C\u003Ef__this = this
  };
}
