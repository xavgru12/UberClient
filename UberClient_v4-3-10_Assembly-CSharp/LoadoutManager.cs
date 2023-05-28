// Decompiled with JetBrains decompiler
// Type: LoadoutManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

public class LoadoutManager : Singleton<LoadoutManager>
{
  private Dictionary<LoadoutSlotType, int> _loadout;
  public static readonly LoadoutSlotType[] QuickSlots = new LoadoutSlotType[3]
  {
    LoadoutSlotType.QuickUseItem1,
    LoadoutSlotType.QuickUseItem2,
    LoadoutSlotType.QuickUseItem3
  };
  public static readonly LoadoutSlotType[] WeaponSlots = new LoadoutSlotType[4]
  {
    LoadoutSlotType.WeaponMelee,
    LoadoutSlotType.WeaponPrimary,
    LoadoutSlotType.WeaponSecondary,
    LoadoutSlotType.WeaponTertiary
  };
  public static readonly LoadoutSlotType[] GearSlots = new LoadoutSlotType[6]
  {
    LoadoutSlotType.GearHead,
    LoadoutSlotType.GearFace,
    LoadoutSlotType.GearGloves,
    LoadoutSlotType.GearUpperBody,
    LoadoutSlotType.GearLowerBody,
    LoadoutSlotType.GearBoots
  };
  public static readonly UberstrikeItemClass[] GearSlotClasses = new UberstrikeItemClass[6]
  {
    UberstrikeItemClass.GearHead,
    UberstrikeItemClass.GearFace,
    UberstrikeItemClass.GearGloves,
    UberstrikeItemClass.GearUpperBody,
    UberstrikeItemClass.GearLowerBody,
    UberstrikeItemClass.GearBoots
  };
  public static readonly string[] GearSlotNames = new string[6]
  {
    LocalizedStrings.Head,
    LocalizedStrings.Face,
    LocalizedStrings.Gloves,
    LocalizedStrings.UpperBody,
    LocalizedStrings.LowerBody,
    LocalizedStrings.Boots
  };
  private static readonly InventoryItem EmptyItem = new InventoryItem((IUnityItem) null);

  private LoadoutManager()
  {
    this._loadout = new Dictionary<LoadoutSlotType, int>()
    {
      {
        LoadoutSlotType.GearHead,
        Singleton<ItemManager>.Instance.DefaultHeadItemId
      },
      {
        LoadoutSlotType.GearUpperBody,
        Singleton<ItemManager>.Instance.DefaultUpperBodyItemId
      },
      {
        LoadoutSlotType.GearLowerBody,
        Singleton<ItemManager>.Instance.DefaultLowerBodyItemId
      },
      {
        LoadoutSlotType.GearGloves,
        Singleton<ItemManager>.Instance.DefaultGlovesItemId
      },
      {
        LoadoutSlotType.GearBoots,
        Singleton<ItemManager>.Instance.DefaultBootsItemId
      }
    };
    foreach (int num in Enum.GetValues(typeof (LoadoutSlotType)))
    {
      LoadoutSlotType key = (LoadoutSlotType) num;
      if (!this._loadout.ContainsKey(key))
        this._loadout[key] = 0;
    }
  }

  public GearLoadout GearLoadout => new GearLoadout(this._loadout[LoadoutSlotType.GearHolo], this._loadout[LoadoutSlotType.GearHead], this._loadout[LoadoutSlotType.GearFace], this._loadout[LoadoutSlotType.GearGloves], this._loadout[LoadoutSlotType.GearUpperBody], this._loadout[LoadoutSlotType.GearLowerBody], this._loadout[LoadoutSlotType.GearBoots]);

  public WeaponItem GetEquippedWeapon(UberstrikeItemClass type)
  {
    InventoryItem inventoryItem;
    if (this.TryGetItemInSlot(LoadoutSlotType.WeaponMelee, out inventoryItem) && inventoryItem.Item.ItemClass == type)
      return inventoryItem.Item as WeaponItem;
    if (this.TryGetItemInSlot(LoadoutSlotType.WeaponPrimary, out inventoryItem) && inventoryItem.Item.ItemClass == type)
      return inventoryItem.Item as WeaponItem;
    if (this.TryGetItemInSlot(LoadoutSlotType.WeaponSecondary, out inventoryItem) && inventoryItem.Item.ItemClass == type)
      return inventoryItem.Item as WeaponItem;
    return this.TryGetItemInSlot(LoadoutSlotType.WeaponTertiary, out inventoryItem) && inventoryItem.Item.ItemClass == type ? inventoryItem.Item as WeaponItem : (WeaponItem) null;
  }

  public void EquipWeapon(LoadoutSlotType weaponSlot, WeaponItem itemWeapon)
  {
    BaseWeaponDecorator decorator = (BaseWeaponDecorator) null;
    if ((UnityEngine.Object) itemWeapon != (UnityEngine.Object) null)
    {
      decorator = (UnityEngine.Object.Instantiate((UnityEngine.Object) Singleton<ItemManager>.Instance.GetPrefab(itemWeapon.ItemId)) as GameObject).GetComponent<BaseWeaponDecorator>();
      decorator.EnableShootAnimation = false;
    }
    switch (weaponSlot)
    {
      case LoadoutSlotType.WeaponMelee:
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponMelee, decorator);
        break;
      case LoadoutSlotType.WeaponPrimary:
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponPrimary, decorator);
        break;
      case LoadoutSlotType.WeaponSecondary:
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponSecondary, decorator);
        break;
      case LoadoutSlotType.WeaponTertiary:
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponTertiary, decorator);
        break;
      case LoadoutSlotType.WeaponPickup:
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponPickup, decorator);
        break;
    }
    if (!((UnityEngine.Object) decorator != (UnityEngine.Object) null))
      return;
    GameState.LocalDecorator.ShowWeapon(weaponSlot);
  }

  public int[] SetLoadoutWeapons(int[] weaponIds)
  {
    int[] numArray = new int[4];
    for (int index = 0; index < LoadoutManager.WeaponSlots.Length; ++index)
    {
      LoadoutSlotType weaponSlot = LoadoutManager.WeaponSlots[index];
      numArray[index] = this._loadout[weaponSlot];
      this._loadout[weaponSlot] = weaponIds[index];
    }
    return numArray;
  }

  public void RefreshLoadoutFromServerCache(LoadoutView view)
  {
    try
    {
      this._loadout[LoadoutSlotType.GearHead] = Singleton<InventoryManager>.Instance.GetGearItem(view.Head, UberstrikeItemClass.GearHead);
      this._loadout[LoadoutSlotType.GearFace] = Singleton<InventoryManager>.Instance.GetGearItem(view.Face, UberstrikeItemClass.GearFace);
      this._loadout[LoadoutSlotType.GearGloves] = Singleton<InventoryManager>.Instance.GetGearItem(view.Gloves, UberstrikeItemClass.GearGloves);
      this._loadout[LoadoutSlotType.GearUpperBody] = Singleton<InventoryManager>.Instance.GetGearItem(view.UpperBody, UberstrikeItemClass.GearUpperBody);
      this._loadout[LoadoutSlotType.GearLowerBody] = Singleton<InventoryManager>.Instance.GetGearItem(view.LowerBody, UberstrikeItemClass.GearLowerBody);
      this._loadout[LoadoutSlotType.GearBoots] = Singleton<InventoryManager>.Instance.GetGearItem(view.Boots, UberstrikeItemClass.GearBoots);
      this._loadout[LoadoutSlotType.GearHolo] = Singleton<InventoryManager>.Instance.GetGearItem(view.Webbing, UberstrikeItemClass.GearHolo);
      this._loadout[LoadoutSlotType.QuickUseItem1] = view.QuickItem1;
      this._loadout[LoadoutSlotType.QuickUseItem2] = view.QuickItem2;
      this._loadout[LoadoutSlotType.QuickUseItem3] = view.QuickItem3;
      this._loadout[LoadoutSlotType.FunctionalItem1] = view.FunctionalItem1;
      this._loadout[LoadoutSlotType.FunctionalItem2] = view.FunctionalItem2;
      this._loadout[LoadoutSlotType.FunctionalItem3] = view.FunctionalItem3;
      this._loadout[LoadoutSlotType.WeaponMelee] = view.MeleeWeapon;
      this._loadout[LoadoutSlotType.WeaponPrimary] = view.Weapon1;
      this._loadout[LoadoutSlotType.WeaponSecondary] = view.Weapon2;
      this._loadout[LoadoutSlotType.WeaponTertiary] = view.Weapon3;
      this.UpdateArmor();
    }
    catch
    {
      throw;
    }
  }

  public bool RemoveDuplicateWeaponClass(InventoryItem baseItem)
  {
    LoadoutSlotType updatedSlot = LoadoutSlotType.None;
    return this.RemoveDuplicateWeaponClass(baseItem, ref updatedSlot);
  }

  public bool RemoveDuplicateWeaponClass(InventoryItem baseItem, ref LoadoutSlotType updatedSlot)
  {
    bool flag = false;
    if (baseItem != null && baseItem.Item.ItemType == UberstrikeItemType.Weapon)
    {
      foreach (LoadoutSlotType weaponSlot in LoadoutManager.WeaponSlots)
      {
        InventoryItem inventoryItem;
        if (this.TryGetItemInSlot(weaponSlot, out inventoryItem) && inventoryItem.Item.ItemClass == baseItem.Item.ItemClass && inventoryItem.Item.ItemId != baseItem.Item.ItemId)
        {
          GameState.LocalDecorator.AssignWeapon(weaponSlot, (BaseWeaponDecorator) null);
          this.ResetSlot(weaponSlot);
          updatedSlot = weaponSlot;
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public bool RemoveDuplicateQuickItemClass(
    QuickItemConfiguration item,
    ref LoadoutSlotType lastRemovedSlot)
  {
    bool flag = false;
    if (item != null && item.ItemType == UberstrikeItemType.QuickUse)
    {
      InventoryItem inventoryItem;
      if (this.TryGetItemInSlot(LoadoutSlotType.QuickUseItem1, out inventoryItem) && inventoryItem.Item is QuickItem && ((QuickItem) inventoryItem.Item).Configuration.BehaviourType == item.BehaviourType)
      {
        this.ResetSlot(LoadoutSlotType.QuickUseItem1);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.QuickUseItem1;
      }
      if (this.TryGetItemInSlot(LoadoutSlotType.QuickUseItem2, out inventoryItem) && inventoryItem.Item is QuickItem && ((QuickItem) inventoryItem.Item).Configuration.BehaviourType == item.BehaviourType)
      {
        this.ResetSlot(LoadoutSlotType.QuickUseItem2);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.QuickUseItem2;
      }
      if (this.TryGetItemInSlot(LoadoutSlotType.QuickUseItem3, out inventoryItem) && inventoryItem.Item is QuickItem && ((QuickItem) inventoryItem.Item).Configuration.BehaviourType == item.BehaviourType)
      {
        this.ResetSlot(LoadoutSlotType.QuickUseItem3);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.QuickUseItem3;
      }
    }
    return flag;
  }

  public bool RemoveDuplicateFunctionalItemClass(
    InventoryItem inventoryItem,
    ref LoadoutSlotType lastRemovedSlot)
  {
    bool flag = false;
    if (inventoryItem != null && inventoryItem.Item.ItemType == UberstrikeItemType.Functional)
    {
      if (this.HasLoadoutItem(LoadoutSlotType.FunctionalItem1) && this.GetItemOnSlot<FunctionalItem>(LoadoutSlotType.FunctionalItem1).ItemClass == inventoryItem.Item.ItemClass)
      {
        this.ResetSlot(LoadoutSlotType.FunctionalItem1);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.FunctionalItem1;
      }
      if (this.HasLoadoutItem(LoadoutSlotType.FunctionalItem2) && this.GetItemOnSlot<FunctionalItem>(LoadoutSlotType.FunctionalItem2).ItemClass == inventoryItem.Item.ItemClass)
      {
        this.ResetSlot(LoadoutSlotType.FunctionalItem2);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.FunctionalItem2;
      }
      if (this.HasLoadoutItem(LoadoutSlotType.FunctionalItem3) && this.GetItemOnSlot<FunctionalItem>(LoadoutSlotType.FunctionalItem3).ItemClass == inventoryItem.Item.ItemClass)
      {
        this.ResetSlot(LoadoutSlotType.FunctionalItem3);
        flag = true;
        lastRemovedSlot = LoadoutSlotType.FunctionalItem3;
      }
    }
    return flag;
  }

  public bool SwitchWeaponsInLoadout(int firstSlot, int secondSlot)
  {
    bool flag = true;
    if (firstSlot == secondSlot)
      return true;
    switch (firstSlot)
    {
      case 1:
        InventoryItem itemOnSlot1 = this.GetItemOnSlot(LoadoutSlotType.WeaponPrimary);
        switch (secondSlot)
        {
          case 2:
            this.SetSlot(LoadoutSlotType.WeaponPrimary, this.GetItemOnSlot(LoadoutSlotType.WeaponSecondary));
            this.SetSlot(LoadoutSlotType.WeaponSecondary, itemOnSlot1);
            break;
          case 3:
            this.SetSlot(LoadoutSlotType.WeaponPrimary, this.GetItemOnSlot(LoadoutSlotType.WeaponTertiary));
            this.SetSlot(LoadoutSlotType.WeaponTertiary, itemOnSlot1);
            break;
          default:
            flag = false;
            break;
        }
        break;
      case 2:
        InventoryItem itemOnSlot2 = this.GetItemOnSlot(LoadoutSlotType.WeaponSecondary);
        switch (secondSlot)
        {
          case 1:
            this.SetSlot(LoadoutSlotType.WeaponSecondary, this.GetItemOnSlot(LoadoutSlotType.WeaponPrimary));
            this.SetSlot(LoadoutSlotType.WeaponPrimary, itemOnSlot2);
            break;
          case 3:
            this.SetSlot(LoadoutSlotType.WeaponSecondary, this.GetItemOnSlot(LoadoutSlotType.WeaponTertiary));
            this.SetSlot(LoadoutSlotType.WeaponTertiary, itemOnSlot2);
            break;
          default:
            flag = false;
            break;
        }
        break;
      case 3:
        InventoryItem itemOnSlot3 = this.GetItemOnSlot(LoadoutSlotType.WeaponTertiary);
        switch (secondSlot)
        {
          case 1:
            this.SetSlot(LoadoutSlotType.WeaponTertiary, this.GetItemOnSlot(LoadoutSlotType.WeaponPrimary));
            this.SetSlot(LoadoutSlotType.WeaponPrimary, itemOnSlot3);
            break;
          case 2:
            this.SetSlot(LoadoutSlotType.WeaponTertiary, this.GetItemOnSlot(LoadoutSlotType.WeaponSecondary));
            this.SetSlot(LoadoutSlotType.WeaponSecondary, itemOnSlot3);
            break;
          default:
            flag = false;
            break;
        }
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public bool SwitchQuickItemInLoadout(int firstSlot, int secondSlot)
  {
    bool flag = true;
    if (firstSlot == secondSlot)
      return true;
    switch (firstSlot)
    {
      case 1:
        InventoryItem itemOnSlot1 = this.GetItemOnSlot(LoadoutSlotType.QuickUseItem1);
        switch (secondSlot)
        {
          case 2:
            this.SetSlot(LoadoutSlotType.QuickUseItem1, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem2));
            this.SetSlot(LoadoutSlotType.QuickUseItem2, itemOnSlot1);
            break;
          case 3:
            this.SetSlot(LoadoutSlotType.QuickUseItem1, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem3));
            this.SetSlot(LoadoutSlotType.QuickUseItem3, itemOnSlot1);
            break;
          default:
            flag = false;
            break;
        }
        break;
      case 2:
        InventoryItem itemOnSlot2 = this.GetItemOnSlot(LoadoutSlotType.QuickUseItem2);
        switch (secondSlot)
        {
          case 1:
            this.SetSlot(LoadoutSlotType.QuickUseItem2, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem1));
            this.SetSlot(LoadoutSlotType.QuickUseItem1, itemOnSlot2);
            break;
          case 3:
            this.SetSlot(LoadoutSlotType.QuickUseItem2, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem3));
            this.SetSlot(LoadoutSlotType.QuickUseItem3, itemOnSlot2);
            break;
          default:
            flag = false;
            break;
        }
        break;
      case 3:
        InventoryItem itemOnSlot3 = this.GetItemOnSlot(LoadoutSlotType.QuickUseItem3);
        switch (secondSlot)
        {
          case 1:
            this.SetSlot(LoadoutSlotType.QuickUseItem3, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem1));
            this.SetSlot(LoadoutSlotType.QuickUseItem1, itemOnSlot3);
            break;
          case 2:
            this.SetSlot(LoadoutSlotType.QuickUseItem3, this.GetItemOnSlot(LoadoutSlotType.QuickUseItem2));
            this.SetSlot(LoadoutSlotType.QuickUseItem2, itemOnSlot3);
            break;
          default:
            flag = false;
            break;
        }
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public bool IsWeaponSlotType(LoadoutSlotType slot) => slot == LoadoutSlotType.WeaponMelee || slot == LoadoutSlotType.WeaponPrimary || slot == LoadoutSlotType.WeaponSecondary || slot == LoadoutSlotType.WeaponTertiary;

  public bool IsQuickItemSlotType(LoadoutSlotType slot) => slot == LoadoutSlotType.QuickUseItem1 || slot == LoadoutSlotType.QuickUseItem2 || slot == LoadoutSlotType.QuickUseItem3;

  public bool IsFunctionalItemSlotType(LoadoutSlotType slot) => slot == LoadoutSlotType.FunctionalItem1 || slot == LoadoutSlotType.FunctionalItem2 || slot == LoadoutSlotType.FunctionalItem3;

  public bool SwapLoadoutItems(LoadoutSlotType slotA, LoadoutSlotType slotB)
  {
    bool flag = false;
    if (slotA != slotB)
    {
      if (this.IsWeaponSlotType(slotA) && this.IsWeaponSlotType(slotB))
      {
        InventoryItem inventoryItem1 = (InventoryItem) null;
        InventoryItem inventoryItem2 = (InventoryItem) null;
        this.TryGetItemInSlot(slotA, out inventoryItem1);
        this.TryGetItemInSlot(slotB, out inventoryItem2);
        if (inventoryItem1 != null || inventoryItem2 != null)
        {
          this.SetLoadoutItem(slotA, inventoryItem2);
          this.SetLoadoutItem(slotB, inventoryItem1);
          if (inventoryItem2 != null)
            this.EquipWeapon(slotA, inventoryItem2.Item as WeaponItem);
          if (inventoryItem1 != null)
            this.EquipWeapon(slotB, inventoryItem1.Item as WeaponItem);
          flag = true;
        }
      }
      else if (this.IsQuickItemSlotType(slotA) && this.IsQuickItemSlotType(slotB) || this.IsFunctionalItemSlotType(slotA) && this.IsFunctionalItemSlotType(slotB))
      {
        InventoryItem inventoryItem3 = (InventoryItem) null;
        InventoryItem inventoryItem4 = (InventoryItem) null;
        this.TryGetItemInSlot(slotA, out inventoryItem3);
        this.TryGetItemInSlot(slotB, out inventoryItem4);
        if (inventoryItem3 != null || inventoryItem4 != null)
        {
          this.SetLoadoutItem(slotA, inventoryItem4);
          this.SetLoadoutItem(slotB, inventoryItem3);
          flag = true;
        }
      }
    }
    return flag;
  }

  public void ResetSlot(LoadoutSlotType loadoutSlotType) => this.SetLoadoutItem(loadoutSlotType, (InventoryItem) null);

  public void SetSlot(LoadoutSlotType loadoutSlotType, InventoryItem item) => this.SetSlot(loadoutSlotType, item == null ? (IUnityItem) null : item.Item);

  public void SetSlot(LoadoutSlotType loadoutSlotType, IUnityItem item)
  {
    if (item == null)
    {
      this.SetLoadoutItem(loadoutSlotType, (InventoryItem) null);
    }
    else
    {
      InventoryItem baseItem;
      if (Singleton<InventoryManager>.Instance.TryGetInventoryItem(item.ItemId, out baseItem) && baseItem.IsValid)
      {
        if (item.ItemType == UberstrikeItemType.Weapon)
          this.RemoveDuplicateWeaponClass(baseItem);
        this.SetLoadoutItem(loadoutSlotType, baseItem);
      }
      else
      {
        if (item.ItemView == null)
          return;
        BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
        if (!(bool) (UnityEngine.Object) buyPanelGui)
          return;
        buyPanelGui.SetItem(item, BuyingLocationType.Shop, BuyingRecommendationType.None);
      }
    }
  }

  public void GetArmorValues(out int armorPoints, out int absorbtionRatio)
  {
    armorPoints = absorbtionRatio = 0;
    InventoryItem inventoryItem;
    if (this.TryGetItemInSlot(LoadoutSlotType.GearLowerBody, out inventoryItem) && inventoryItem.Item is GearItem)
    {
      GearItem gearItem = inventoryItem.Item as GearItem;
      armorPoints += gearItem.Configuration.ArmorPoints;
      absorbtionRatio += gearItem.Configuration.ArmorAbsorptionPercent;
    }
    if (this.TryGetItemInSlot(LoadoutSlotType.GearUpperBody, out inventoryItem) && inventoryItem.Item is GearItem)
    {
      GearItem gearItem = inventoryItem.Item as GearItem;
      armorPoints += gearItem.Configuration.ArmorPoints;
      absorbtionRatio += gearItem.Configuration.ArmorAbsorptionPercent;
    }
    if (!this.TryGetItemInSlot(LoadoutSlotType.GearHolo, out inventoryItem) || !(inventoryItem.Item is HoloGearItem))
      return;
    HoloGearItem holoGearItem = inventoryItem.Item as HoloGearItem;
    armorPoints += holoGearItem.Configuration.ArmorPoints;
    absorbtionRatio += holoGearItem.Configuration.ArmorAbsorptionPercent;
  }

  public bool HasLoadoutItem(LoadoutSlotType loadoutSlotType)
  {
    int num;
    return this._loadout.TryGetValue(loadoutSlotType, out num) && num > 0;
  }

  public int GetItemIdOnSlot(LoadoutSlotType loadoutSlotType) => this._loadout[loadoutSlotType];

  public InventoryItem GetItemOnSlot(LoadoutSlotType loadoutSlotType)
  {
    InventoryItem inventoryItem;
    return Singleton<InventoryManager>.Instance.TryGetInventoryItem(this.GetItemIdOnSlot(loadoutSlotType), out inventoryItem) ? inventoryItem : LoadoutManager.EmptyItem;
  }

  public T GetItemOnSlot<T>(LoadoutSlotType loadoutSlotType) where T : class, IUnityItem
  {
    InventoryItem inventoryItem;
    return Singleton<InventoryManager>.Instance.TryGetInventoryItem(this.GetItemIdOnSlot(loadoutSlotType), out inventoryItem) ? (T) inventoryItem.Item : (T) null;
  }

  public Dictionary<LoadoutSlotType, int> GetCurrentLoadoutIds() => new Dictionary<LoadoutSlotType, int>(6)
  {
    {
      LoadoutSlotType.GearHead,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHead)
    },
    {
      LoadoutSlotType.GearFace,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearFace)
    },
    {
      LoadoutSlotType.GearGloves,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearGloves)
    },
    {
      LoadoutSlotType.GearUpperBody,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearUpperBody)
    },
    {
      LoadoutSlotType.GearLowerBody,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearLowerBody)
    },
    {
      LoadoutSlotType.GearBoots,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearBoots)
    },
    {
      LoadoutSlotType.GearHolo,
      Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo)
    }
  };

  public void SetLoadoutItem(LoadoutSlotType loadoutSlotType, InventoryItem item)
  {
    this._loadout[loadoutSlotType] = item == null || item.Item == null ? 0 : item.Item.ItemId;
    MonoRoutine.Start(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
    this.UpdateArmor();
  }

  public IEnumerable<int> EquippedItems => (IEnumerable<int>) this._loadout.Values;

  public bool IsItemEquipped(int itemId) => this._loadout.Any<KeyValuePair<LoadoutSlotType, int>>((Func<KeyValuePair<LoadoutSlotType, int>, bool>) (i => i.Value == itemId));

  public bool HasItemInSlot(LoadoutSlotType slot) => this.TryGetItemInSlot(slot, out InventoryItem _);

  public bool TryGetItemInSlot(LoadoutSlotType slot, out InventoryItem item)
  {
    item = (InventoryItem) null;
    int itemID;
    return this._loadout.TryGetValue(slot, out itemID) && Singleton<InventoryManager>.Instance.TryGetInventoryItem(itemID, out item);
  }

  public bool TryGetSlotForItem(int itemId, out LoadoutSlotType slot)
  {
    slot = LoadoutSlotType.None;
    foreach (KeyValuePair<LoadoutSlotType, int> keyValuePair in this._loadout)
    {
      if (keyValuePair.Value == itemId)
      {
        slot = keyValuePair.Key;
        return true;
      }
    }
    return false;
  }

  public bool ValidateLoadout() => this._loadout.Count > 0;

  public void UpdateArmor()
  {
    int armorPoints;
    int absorbtionRatio;
    this.GetArmorValues(out armorPoints, out absorbtionRatio);
    Singleton<ArmorHud>.Instance.ArmorCarried = armorPoints;
    Singleton<ArmorHud>.Instance.DefenseBonus = absorbtionRatio;
  }
}
