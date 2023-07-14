// Decompiled with JetBrains decompiler
// Type: InventoryManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;

public class InventoryManager : Singleton<InventoryManager>
{
  private IDictionary<int, InventoryItem> _inventoryItems;
  public LoadoutSlotType CurrentWeaponSlot = LoadoutSlotType.WeaponPrimary;
  public LoadoutSlotType CurrentQuickItemSot = LoadoutSlotType.QuickUseItem1;
  public LoadoutSlotType CurrentFunctionalSlot = LoadoutSlotType.FunctionalItem1;
  private static readonly InventoryItem EmptyItem = new InventoryItem((IUnityItem) null);

  private InventoryManager() => this._inventoryItems = (IDictionary<int, InventoryItem>) new Dictionary<int, InventoryItem>();

  public IEnumerable<InventoryItem> InventoryItems => (IEnumerable<InventoryItem>) this._inventoryItems.Values;

  [DebuggerHidden]
  public IEnumerator StartUpdateInventoryAndEquipNewItem(int itemId, bool autoEquip = false) => (IEnumerator) new InventoryManager.\u003CStartUpdateInventoryAndEquipNewItem\u003Ec__Iterator6D()
  {
    itemId = itemId,
    autoEquip = autoEquip,
    \u003C\u0024\u003EitemId = itemId,
    \u003C\u0024\u003EautoEquip = autoEquip,
    \u003C\u003Ef__this = this
  };

  public bool EquipItem(int itemId)
  {
    LoadoutSlotType slotType = LoadoutSlotType.None;
    InventoryItem inventoryItem;
    if (this.TryGetInventoryItem(itemId, out inventoryItem) && inventoryItem.IsValid && inventoryItem.Item.ItemType == UberstrikeItemType.Weapon && (UnityEngine.Object) GameState.CurrentSpace != (UnityEngine.Object) null)
      slotType = RecommendationUtils.FindBestSlotToEquipWeapon(inventoryItem.Item as WeaponItem, RecommendationUtils.GetCategoriesForCombatRange(GameState.CurrentSpace.CombatRangeTiers));
    return this.EquipItemOnSlot(itemId, slotType);
  }

  public void UnequipWeaponSlot(LoadoutSlotType slotType)
  {
    switch (slotType)
    {
      case LoadoutSlotType.WeaponMelee:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.WeaponMelee);
        GameState.LocalDecorator.SetActiveWeaponSlot(slotType);
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponMelee, (BaseWeaponDecorator) null);
        break;
      case LoadoutSlotType.WeaponPrimary:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.WeaponPrimary);
        GameState.LocalDecorator.SetActiveWeaponSlot(slotType);
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponPrimary, (BaseWeaponDecorator) null);
        break;
      case LoadoutSlotType.WeaponSecondary:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.WeaponSecondary);
        GameState.LocalDecorator.SetActiveWeaponSlot(slotType);
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponSecondary, (BaseWeaponDecorator) null);
        break;
      case LoadoutSlotType.WeaponTertiary:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.WeaponTertiary);
        GameState.LocalDecorator.SetActiveWeaponSlot(slotType);
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponTertiary, (BaseWeaponDecorator) null);
        break;
    }
  }

  public bool EquipItemOnSlot(int itemId, LoadoutSlotType slotType)
  {
    InventoryItem inventoryItem;
    if (!this.TryGetInventoryItem(itemId, out inventoryItem) || !inventoryItem.IsValid)
      return false;
    if (Singleton<LoadoutManager>.Instance.IsItemEquipped(itemId))
    {
      LoadoutSlotType slot;
      if (Singleton<LoadoutManager>.Instance.TryGetSlotForItem(itemId, out slot))
      {
        CmuneEventHandler.Route((object) new ShopHighlightSlotEvent()
        {
          SlotType = slot
        });
        Singleton<TemporaryLoadoutManager>.Instance.SetGearLoadout(slot, (IUnityItem) null);
      }
    }
    else
    {
      this.HighlightItem(itemId, false);
      switch (inventoryItem.Item.ItemType)
      {
        case UberstrikeItemType.Weapon:
          if (inventoryItem.Item.ItemClass == UberstrikeItemClass.WeaponMelee)
          {
            slotType = LoadoutSlotType.WeaponMelee;
            Singleton<LoadoutManager>.Instance.RemoveDuplicateWeaponClass(inventoryItem);
            Singleton<LoadoutManager>.Instance.SetSlot(slotType, inventoryItem);
            Singleton<LoadoutManager>.Instance.EquipWeapon(slotType, inventoryItem.Item as WeaponItem);
            SfxManager.Play2dAudioClip(GameAudio.WeaponSwitch);
            break;
          }
          if (slotType == LoadoutSlotType.None || slotType == LoadoutSlotType.WeaponMelee)
            slotType = this.GetNextFreeWeaponSlot();
          LoadoutSlotType updatedSlot = slotType;
          if (Singleton<LoadoutManager>.Instance.RemoveDuplicateWeaponClass(inventoryItem, ref updatedSlot) && slotType != updatedSlot)
            Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, updatedSlot);
          Singleton<LoadoutManager>.Instance.SetSlot(slotType, inventoryItem);
          Singleton<LoadoutManager>.Instance.EquipWeapon(slotType, inventoryItem.Item as WeaponItem);
          SfxManager.Play2dAudioClip(GameAudio.EquipWeapon);
          break;
        case UberstrikeItemType.Gear:
          slotType = PlayerDataManager.GetSlotTypeForItemClass(inventoryItem.Item.ItemClass);
          Singleton<LoadoutManager>.Instance.SetSlot(slotType, inventoryItem);
          Singleton<TemporaryLoadoutManager>.Instance.SetGearLoadout(slotType, inventoryItem.Item);
          SfxManager.Play2dAudioClip(GameAudio.EquipGear);
          if ((bool) (UnityEngine.Object) GameState.LocalDecorator)
          {
            GameState.LocalDecorator.HideWeapons();
            break;
          }
          break;
        case UberstrikeItemType.QuickUse:
          this.EquipQuickItemOnSlot(inventoryItem, slotType);
          break;
        case UberstrikeItemType.Functional:
          if (ItemManager.IsItemEquippable(inventoryItem.Item))
          {
            if (slotType == LoadoutSlotType.None)
              slotType = this.GetNextFreeFunctionalSlot();
            LoadoutSlotType lastRemovedSlot = slotType;
            if (Singleton<LoadoutManager>.Instance.RemoveDuplicateFunctionalItemClass(inventoryItem, ref lastRemovedSlot) && slotType != lastRemovedSlot)
              Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, lastRemovedSlot);
            Singleton<LoadoutManager>.Instance.SetSlot(slotType, inventoryItem);
            SfxManager.Play2dAudioClip(GameAudio.EquipItem);
            break;
          }
          break;
        default:
          SfxManager.Play2dAudioClip(GameAudio.EquipItem);
          UnityEngine.Debug.LogError((object) ("Equip item of type: " + (object) inventoryItem.Item.ItemType));
          break;
      }
      bool resetAnimations = inventoryItem.Item.ItemClass == UberstrikeItemClass.GearHolo;
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, inventoryItem.Item.ItemClass, resetAnimations);
      CmuneEventHandler.Route((object) new ShopHighlightSlotEvent()
      {
        SlotType = slotType
      });
    }
    return true;
  }

  private void EquipQuickItemOnSlot(InventoryItem item, LoadoutSlotType slotType)
  {
    if (slotType < LoadoutSlotType.QuickUseItem1 || slotType > LoadoutSlotType.QuickUseItem3)
      slotType = this.GetNextFreeQuickItemSlot();
    LoadoutSlotType lastRemovedSlot = slotType;
    if (Singleton<LoadoutManager>.Instance.RemoveDuplicateQuickItemClass(((QuickItem) item.Item).Configuration, ref lastRemovedSlot) && slotType != lastRemovedSlot)
      Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slotType, lastRemovedSlot);
    SfxManager.Play2dAudioClip(GameAudio.EquipItem);
    Singleton<LoadoutManager>.Instance.SetSlot(slotType, item);
  }

  private LoadoutSlotType GetNextFreeWeaponSlot()
  {
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponPrimary))
      return LoadoutSlotType.WeaponPrimary;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponSecondary))
      return LoadoutSlotType.WeaponSecondary;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponTertiary))
      return LoadoutSlotType.WeaponTertiary;
    return this.CurrentWeaponSlot == LoadoutSlotType.WeaponPrimary || this.CurrentWeaponSlot == LoadoutSlotType.WeaponSecondary || this.CurrentWeaponSlot == LoadoutSlotType.WeaponTertiary ? this.CurrentWeaponSlot : LoadoutSlotType.WeaponPrimary;
  }

  private LoadoutSlotType GetNextFreeFunctionalSlot()
  {
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem1))
      return LoadoutSlotType.FunctionalItem1;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem2))
      return LoadoutSlotType.FunctionalItem2;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.FunctionalItem3))
      return LoadoutSlotType.FunctionalItem3;
    switch (this.CurrentFunctionalSlot)
    {
      case LoadoutSlotType.FunctionalItem1:
        return LoadoutSlotType.FunctionalItem2;
      case LoadoutSlotType.FunctionalItem2:
        return LoadoutSlotType.FunctionalItem3;
      case LoadoutSlotType.FunctionalItem3:
        return LoadoutSlotType.FunctionalItem1;
      default:
        return this.CurrentFunctionalSlot;
    }
  }

  private LoadoutSlotType GetNextFreeQuickItemSlot()
  {
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem1))
      return LoadoutSlotType.QuickUseItem1;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem2))
      return LoadoutSlotType.QuickUseItem2;
    if (!Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.QuickUseItem3))
      return LoadoutSlotType.QuickUseItem3;
    switch (this.CurrentQuickItemSot)
    {
      case LoadoutSlotType.QuickUseItem1:
        return LoadoutSlotType.QuickUseItem2;
      case LoadoutSlotType.QuickUseItem2:
        return LoadoutSlotType.QuickUseItem3;
      case LoadoutSlotType.QuickUseItem3:
        return LoadoutSlotType.QuickUseItem1;
      default:
        return this.CurrentQuickItemSot;
    }
  }

  public static LoadoutSlotType GetSlotTypeForGear(GearItem gearItem)
  {
    if (!((UnityEngine.Object) gearItem != (UnityEngine.Object) null))
      return LoadoutSlotType.None;
    switch (gearItem.ItemClass)
    {
      case UberstrikeItemClass.GearBoots:
        return LoadoutSlotType.GearBoots;
      case UberstrikeItemClass.GearHead:
        return LoadoutSlotType.GearHead;
      case UberstrikeItemClass.GearFace:
        return LoadoutSlotType.GearFace;
      case UberstrikeItemClass.GearUpperBody:
        return LoadoutSlotType.GearUpperBody;
      case UberstrikeItemClass.GearLowerBody:
        return LoadoutSlotType.GearLowerBody;
      case UberstrikeItemClass.GearGloves:
        return LoadoutSlotType.GearGloves;
      case UberstrikeItemClass.GearHolo:
        return LoadoutSlotType.GearHolo;
      default:
        return LoadoutSlotType.None;
    }
  }

  public List<InventoryItem> GetAllItems(bool ignoreEquippedItems)
  {
    List<InventoryItem> allItems = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem in (IEnumerable<InventoryItem>) this._inventoryItems.Values)
    {
      bool flag = inventoryItem.DaysRemaining <= 0 && inventoryItem.Item.ItemView.Prices != null && inventoryItem.Item.ItemView.Prices.Count > 0;
      if (inventoryItem.DaysRemaining > 0 || inventoryItem.IsPermanent || flag)
      {
        if (ignoreEquippedItems)
        {
          if (!Singleton<LoadoutManager>.Instance.IsItemEquipped(inventoryItem.Item.ItemId))
            allItems.Add(inventoryItem);
        }
        else
          allItems.Add(inventoryItem);
      }
    }
    return allItems;
  }

  public int GetGearItem(int itemID, UberstrikeItemClass itemClass)
  {
    InventoryItem inventoryItem;
    if (this._inventoryItems.TryGetValue(itemID, out inventoryItem) && inventoryItem != null && inventoryItem.Item.ItemType == UberstrikeItemType.Gear)
      return inventoryItem.Item.ItemId;
    IUnityItem unityItem;
    return Singleton<ItemManager>.Instance.TryGetDefaultItem(itemClass, out unityItem) ? unityItem.ItemId : 0;
  }

  public InventoryItem GetItem(int itemID)
  {
    InventoryItem inventoryItem;
    return this._inventoryItems.TryGetValue(itemID, out inventoryItem) && inventoryItem != null ? inventoryItem : InventoryManager.EmptyItem;
  }

  public InventoryItem GetWeaponItem(int itemId)
  {
    InventoryItem inventoryItem;
    return this._inventoryItems.TryGetValue(itemId, out inventoryItem) && inventoryItem != null && inventoryItem.Item.ItemType == UberstrikeItemType.Weapon ? inventoryItem : InventoryManager.EmptyItem;
  }

  public bool TryGetInventoryItem(int itemID, out InventoryItem item) => this._inventoryItems.TryGetValue(itemID, out item) && item != null && item.Item != null;

  public bool HasClanLicense() => this.IsItemInInventory(1234);

  public bool IsItemValidForDays(InventoryItem item, int days)
  {
    if (item == null)
      return false;
    return item.DaysRemaining > days || item.IsPermanent;
  }

  public bool IsItemInInventory(int itemId)
  {
    InventoryItem inventoryItem;
    return this._inventoryItems.TryGetValue(itemId, out inventoryItem) && this.IsItemValidForDays(inventoryItem, 0);
  }

  public void UpdateInventoryItems(List<ItemInventoryView> inventory)
  {
    if (Singleton<ItemManager>.Instance.ShopItemCount == 0)
      return;
    HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this._inventoryItems.Keys);
    this._inventoryItems.Clear();
    foreach (ItemInventoryView itemInventoryView in inventory)
    {
      IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemInventoryView.ItemId);
      if (itemInShop != null && itemInShop.ItemId == itemInventoryView.ItemId)
      {
        IDictionary<int, InventoryItem> inventoryItems = this._inventoryItems;
        int itemId = itemInShop.ItemId;
        InventoryItem inventoryItem1 = new InventoryItem(itemInShop);
        inventoryItem1.IsPermanent = !itemInventoryView.ExpirationDate.HasValue;
        inventoryItem1.AmountRemaining = itemInventoryView.AmountRemaining;
        InventoryItem inventoryItem2 = inventoryItem1;
        DateTime? expirationDate = itemInventoryView.ExpirationDate;
        DateTime? nullable = new DateTime?(!expirationDate.HasValue ? DateTime.MinValue : expirationDate.Value);
        inventoryItem2.ExpirationDate = nullable;
        inventoryItem1.IsHighlighted = intSet.Count > 0 && !intSet.Contains(itemInShop.ItemId);
        InventoryItem inventoryItem3 = inventoryItem1;
        inventoryItems[itemId] = inventoryItem3;
      }
      else
        UnityEngine.Debug.LogWarning((object) ("Inventory Item not found: " + (object) itemInventoryView.ItemId));
    }
    CmuneEventHandler.Route((object) new ShopRefreshCurrentItemListEvent());
  }

  internal void HighlightItem(int itemId, bool isHighlighted)
  {
    InventoryItem inventoryItem;
    if (!this._inventoryItems.TryGetValue(itemId, out inventoryItem) || inventoryItem == null)
      return;
    inventoryItem.IsHighlighted = isHighlighted;
  }
}
