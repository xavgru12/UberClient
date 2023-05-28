// Decompiled with JetBrains decompiler
// Type: ShopPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ShopPageGUI : PageGUI
{
  private const int SlotHeight = 70;
  private const int LoadoutWidth = 190;
  private const int ShopWidth = 590;
  private ItemBundlesShopGUI _masBundleGui = new ItemBundlesShopGUI();
  private CreditBundlesShopGui _creditsGui = new CreditBundlesShopGui();
  private LotteryShopGUI _lotteryGui = new LotteryShopGUI();
  private ShopSorting.ItemComparer<BaseItemGUI> _inventoryComparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.DurationComparer();
  private ShopSorting.ItemComparer<BaseItemGUI> _shopComparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.PriceComparer();
  private bool _firstLogin;
  private SelectionGroup<ShopArea> _shopAreaSelection = new SelectionGroup<ShopArea>();
  private SelectionGroup<LoadoutArea> _loadoutAreaSelection = new SelectionGroup<LoadoutArea>();
  private SelectionGroup<UberstrikeItemType> _typeSelection = new SelectionGroup<UberstrikeItemType>();
  private SelectionGroup<UberstrikeItemClass> _weaponClassSelection = new SelectionGroup<UberstrikeItemClass>();
  private SelectionGroup<UberstrikeItemClass> _gearClassSelection = new SelectionGroup<UberstrikeItemClass>();
  private Rect _rectLabs;
  private Rect _shopArea;
  private Rect _loadoutArea;
  private Vector2 _loadoutWeaponScroll;
  private Vector2 _loadoutGearScroll;
  private Vector2 _loadoutQuickUseFuncScroll;
  private Vector2 _labScroll;
  private float _highlightedSlotAlpha = 0.2f;
  private LoadoutSlotType _highlightedSlot = LoadoutSlotType.None;
  private List<Rect> _activeLoadoutUsedSpace = new List<Rect>();
  private Dictionary<LoadoutSlotType, bool> _renewItem = new Dictionary<LoadoutSlotType, bool>();
  private bool _showRenewLoadoutButton;
  private int _skippedDefaultGearCount;
  private float shopPositionX;
  private List<BaseItemGUI> _shopItemGUIList = new List<BaseItemGUI>();
  private List<BaseItemGUI> _inventoryItemGUIList = new List<BaseItemGUI>();
  private IShopItemFilter _itemFilter;
  private SearchBarGUI _searchBar;
  [SerializeField]
  private BuyingLocationType _location;

  private void Awake()
  {
    this._itemFilter = (IShopItemFilter) new SpecialItemFilter();
    this._firstLogin = true;
    this.IsOnGUIEnabled = true;
    this._searchBar = new SearchBarGUI("SearchInShop");
  }

  private void Start()
  {
    this._loadoutAreaSelection.Add(LoadoutArea.Weapons, new GUIContent((Texture) ShopIcons.LoadoutTabWeapons, LocalizedStrings.Weapons));
    this._loadoutAreaSelection.Add(LoadoutArea.Gear, new GUIContent((Texture) ShopIcons.LoadoutTabGear, LocalizedStrings.Gear));
    this._loadoutAreaSelection.Add(LoadoutArea.QuickItems, new GUIContent((Texture) ShopIcons.LoadoutTabItems, LocalizedStrings.Items));
    this._loadoutAreaSelection.OnSelectionChange += new Action<LoadoutArea>(this.SelectLoadoutArea);
    this._shopAreaSelection.Add(ShopArea.Inventory, new GUIContent(string.Empty, (Texture) ShopIcons.LabsInventory, LocalizedStrings.Inventory));
    this._shopAreaSelection.Add(ShopArea.Shop, new GUIContent(string.Empty, (Texture) ShopIcons.LabsShop, LocalizedStrings.Shop));
    this._shopAreaSelection.Add(ShopArea.MysteryBox, new GUIContent(string.Empty, (Texture) ShopIcons.IconLottery, LocalizedStrings.MysteryBox));
    if (Application.isEditor || ApplicationDataManager.Channel == ChannelType.WebFacebook || ApplicationDataManager.Channel == ChannelType.MacAppStore || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone || ApplicationDataManager.Channel == ChannelType.Android)
    {
      this._shopAreaSelection.Add(ShopArea.Packs, new GUIContent(string.Empty, (Texture) ShopIcons.BundleIcon32x32, LocalizedStrings.Packs));
      this._shopAreaSelection.Add(ShopArea.Credits, new GUIContent(string.Empty, (Texture) ShopIcons.CreditsIcon32x32, LocalizedStrings.Credits));
    }
    this._shopAreaSelection.OnSelectionChange += (Action<ShopArea>) (area => this.UpdateItemFilter());
    this._typeSelection.Add(UberstrikeItemType.Special, new GUIContent((Texture) ShopIcons.NewItems, LocalizedStrings.NewAndSaleItems));
    this._typeSelection.Add(UberstrikeItemType.Weapon, new GUIContent((Texture) ShopIcons.WeaponItems, LocalizedStrings.Weapons));
    this._typeSelection.Add(UberstrikeItemType.Gear, new GUIContent((Texture) ShopIcons.GearItems, LocalizedStrings.Gear));
    this._typeSelection.Add(UberstrikeItemType.QuickUse, new GUIContent((Texture) ShopIcons.QuickItems, LocalizedStrings.QuickItems));
    this._typeSelection.Add(UberstrikeItemType.Functional, new GUIContent((Texture) ShopIcons.FunctionalItems, LocalizedStrings.FunctionalItems));
    this._typeSelection.OnSelectionChange += (Action<UberstrikeItemType>) (itemType => this.UpdateItemFilter());
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponMelee, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsMelee, LocalizedStrings.MeleeWeapons));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponHandgun, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsHandgun, LocalizedStrings.Handguns));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponMachinegun, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsMachinegun, LocalizedStrings.Machineguns));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponShotgun, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsShotgun, LocalizedStrings.Shotguns));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponSniperRifle, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsSniperRifle, LocalizedStrings.SniperRifles));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponCannon, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsCannon, LocalizedStrings.Cannons));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponSplattergun, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsSplattergun, LocalizedStrings.Splatterguns));
    this._weaponClassSelection.Add(UberstrikeItemClass.WeaponLauncher, new GUIContent((Texture) ShopIcons.StatsMostWeaponSplatsLauncher, LocalizedStrings.Launchers));
    this._weaponClassSelection.OnSelectionChange += (Action<UberstrikeItemClass>) (itemClass => this.UpdateItemFilter());
    this._gearClassSelection.Add(UberstrikeItemClass.GearBoots, new GUIContent((Texture) ShopIcons.Boots, LocalizedStrings.Boots));
    this._gearClassSelection.Add(UberstrikeItemClass.GearHead, new GUIContent((Texture) ShopIcons.Head, LocalizedStrings.Head));
    this._gearClassSelection.Add(UberstrikeItemClass.GearFace, new GUIContent((Texture) ShopIcons.Face, LocalizedStrings.Face));
    this._gearClassSelection.Add(UberstrikeItemClass.GearUpperBody, new GUIContent((Texture) ShopIcons.Upperbody, LocalizedStrings.UpperBody));
    this._gearClassSelection.Add(UberstrikeItemClass.GearLowerBody, new GUIContent((Texture) ShopIcons.Lowerbody, LocalizedStrings.LowerBody));
    this._gearClassSelection.Add(UberstrikeItemClass.GearGloves, new GUIContent((Texture) ShopIcons.Gloves, LocalizedStrings.Gloves));
    this._gearClassSelection.Add(UberstrikeItemClass.GearHolo, new GUIContent((Texture) ShopIcons.Holos, LocalizedStrings.Holo));
    this._gearClassSelection.OnSelectionChange += (Action<UberstrikeItemClass>) (itemClass => this.UpdateItemFilter());
    if (this._showRenewLoadoutButton)
    {
      foreach (LoadoutSlotType weaponSlot in LoadoutManager.WeaponSlots)
      {
        InventoryItem inventoryItem;
        if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(weaponSlot, out inventoryItem))
          this._renewItem[weaponSlot] = !Singleton<InventoryManager>.Instance.IsItemValidForDays(inventoryItem, 5);
      }
      foreach (LoadoutSlotType gearSlot in LoadoutManager.GearSlots)
      {
        InventoryItem inventoryItem;
        if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(gearSlot, out inventoryItem))
          this._renewItem[gearSlot] = !Singleton<InventoryManager>.Instance.IsItemValidForDays(inventoryItem, 5);
      }
    }
    this.UpdateShopItems();
    if (this._shopAreaSelection.Index == 0)
      this._shopAreaSelection.Select(ShopArea.Shop);
    this._typeSelection.Select(UberstrikeItemType.Special);
    this._gearClassSelection.SetIndex(-1);
    this._weaponClassSelection.SetIndex(-1);
  }

  private void OnEnable()
  {
    CmuneEventHandler.AddListener<SelectShopAreaEvent>(new Action<SelectShopAreaEvent>(this.OnSelectShopAreaEvent));
    CmuneEventHandler.AddListener<SelectLoadoutAreaEvent>(new Action<SelectLoadoutAreaEvent>(this.OnSelectLoadoutAreaEvent));
    CmuneEventHandler.AddListener<ShopHighlightSlotEvent>(new Action<ShopHighlightSlotEvent>(this.OnHighlightSlotEvent));
    CmuneEventHandler.AddListener<ShopRefreshCurrentItemListEvent>(new Action<ShopRefreshCurrentItemListEvent>(this.OnRefreshCurrentItemListEvent));
    Singleton<DragAndDrop>.Instance.OnDragBegin += new Action<IDragSlot>(this.OnBeginDrag);
    Singleton<TemporaryLoadoutManager>.Instance.ResetGearLoadout();
    AutoMonoBehaviour<AvatarAnimationManager>.Instance.ResetAnimationState(PageType.Shop);
    this.StartCoroutine(this.StartNotifyLoadoutArea());
    if ((UnityEngine.Object) MouseOrbit.Instance != (UnityEngine.Object) null)
      MouseOrbit.Instance.MaxX = Screen.width - 590;
    if (this.IsOnGUIEnabled)
      this.StartCoroutine(this.ScrollShopFromRight(0.25f));
    this._searchBar.ClearFilter();
  }

  private void OnDisable()
  {
    CmuneEventHandler.RemoveListener<SelectShopAreaEvent>(new Action<SelectShopAreaEvent>(this.OnSelectShopAreaEvent));
    CmuneEventHandler.RemoveListener<SelectLoadoutAreaEvent>(new Action<SelectLoadoutAreaEvent>(this.OnSelectLoadoutAreaEvent));
    CmuneEventHandler.RemoveListener<ShopRefreshCurrentItemListEvent>(new Action<ShopRefreshCurrentItemListEvent>(this.OnRefreshCurrentItemListEvent));
    CmuneEventHandler.RemoveListener<ShopHighlightSlotEvent>(new Action<ShopHighlightSlotEvent>(this.OnHighlightSlotEvent));
    Singleton<DragAndDrop>.Instance.OnDragBegin -= new Action<IDragSlot>(this.OnBeginDrag);
    if (!((UnityEngine.Object) MouseOrbit.Instance != (UnityEngine.Object) null))
      return;
    MouseOrbit.Instance.MaxX = Screen.width;
  }

  private void OnHighlightSlotEvent(ShopHighlightSlotEvent ev) => this.HighlightingSlot(ev.SlotType);

  private void OnSelectShopAreaEvent(SelectShopAreaEvent ev)
  {
    this._shopAreaSelection.Select(ev.ShopArea);
    if (ev.ItemType != (UberstrikeItemType) 0)
      this._typeSelection.Select(ev.ItemType);
    if (ev.ItemClass == (UberstrikeItemClass) 0)
      return;
    switch (ev.ItemType)
    {
      case UberstrikeItemType.Weapon:
        this._weaponClassSelection.Select(ev.ItemClass);
        break;
      case UberstrikeItemType.Gear:
        this._gearClassSelection.Select(ev.ItemClass);
        break;
    }
  }

  private void OnSelectLoadoutAreaEvent(SelectLoadoutAreaEvent ev) => this._loadoutAreaSelection.Select(ev.Area);

  [DebuggerHidden]
  private IEnumerator ScrollShopFromRight(float time) => (IEnumerator) new ShopPageGUI.\u003CScrollShopFromRight\u003Ec__Iterator19()
  {
    time = time,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartNotifyLoadoutArea() => (IEnumerator) new ShopPageGUI.\u003CStartNotifyLoadoutArea\u003Ec__Iterator1A()
  {
    \u003C\u003Ef__this = this
  };

  private void OnGUI()
  {
    if (!this.IsOnGUIEnabled)
      return;
    this.DrawGUI(new Rect((float) Screen.width - this.shopPositionX, (float) GlobalUIRibbon.Instance.Height(), 590f, (float) (Screen.height - GlobalUIRibbon.Instance.Height() + 1)));
    Singleton<ArmorHud>.Instance.Update();
  }

  public override void DrawGUI(Rect rect)
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    if (this._firstLogin)
      this._firstLogin = false;
    this._rectLabs = rect;
    this._rectLabs.width += 10f;
    GUITools.PushGUIState();
    GUI.enabled = !PopupSystem.IsAnyPopupOpen && !PanelManager.IsAnyPanelOpen;
    GUI.BeginGroup(this._rectLabs);
    this.DrawLoadout(new Rect(0.0f, 0.0f, 190f, this._rectLabs.height));
    this.DrawShop(new Rect(190f, 0.0f, (float) ((double) this._rectLabs.width - 190.0 - 10.0), this._rectLabs.height));
    GUI.EndGroup();
    Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(new Rect(0.0f, 55f, (float) (Screen.width - 580), (float) (Screen.height - 55)), new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropAvatar));
    Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(new Rect((float) ((double) Screen.width - (double) this._rectLabs.width + 200.0), 55f, this._rectLabs.width - 200f, (float) (Screen.height - 55)), new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropShop));
    GUITools.PopGUIState();
    if (!PopupSystem.IsAnyPopupOpen && !PanelManager.IsAnyPanelOpen)
      GuiManager.DrawTooltip();
    if ((double) this._highlightedSlotAlpha <= 0.0)
      return;
    this._highlightedSlotAlpha = Mathf.Max(this._highlightedSlotAlpha - Time.deltaTime * 0.5f, 0.0f);
  }

  public void EquipItemFromArea(IUnityItem item, LoadoutSlotType slot, ShopArea area)
  {
    if (item != null && !Singleton<LoadoutManager>.Instance.IsItemEquipped(item.ItemId))
    {
      if (Singleton<InventoryManager>.Instance.IsItemInInventory(item.ItemId))
      {
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(item.ItemId, slot);
      }
      else
      {
        BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
        if (!(bool) (UnityEngine.Object) buyPanelGui)
          return;
        buyPanelGui.SetItem(item, this._location, BuyingRecommendationType.None, true);
      }
    }
    else
      UnityEngine.Debug.LogError((object) "Item is null or already equipped!");
  }

  public void SelectLoadoutWeapon(LoadoutSlotType slot)
  {
    if (Singleton<InventoryManager>.Instance.CurrentWeaponSlot == slot)
      return;
    Singleton<InventoryManager>.Instance.CurrentWeaponSlot = slot;
    GameState.LocalDecorator.SetActiveWeaponSlot(slot);
    GameState.LocalDecorator.ShowWeapon(slot);
    InventoryItem inventoryItem;
    if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(slot, out inventoryItem))
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, inventoryItem.Item.ItemClass);
    else
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, (UberstrikeItemClass) 0);
  }

  private void UnequipItem(IUnityItem item)
  {
    LoadoutSlotType slot;
    if (item == null || !Singleton<LoadoutManager>.Instance.TryGetSlotForItem(item.ItemId, out slot))
      return;
    this.UnequipItem(slot);
  }

  private void UnequipItem(LoadoutSlotType slotType)
  {
    UberstrikeItemClass type = (UberstrikeItemClass) 0;
    switch (slotType)
    {
      case LoadoutSlotType.GearHead:
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(Singleton<ItemManager>.Instance.DefaultHeadItemId, LoadoutSlotType.GearHead);
        type = UberstrikeItemClass.GearHead;
        break;
      case LoadoutSlotType.GearFace:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.GearFace);
        Singleton<TemporaryLoadoutManager>.Instance.SetGearLoadout(LoadoutSlotType.GearFace, (IUnityItem) null);
        SfxManager.Play2dAudioClip(GameAudio.EquipGear);
        if ((bool) (UnityEngine.Object) GameState.LocalDecorator)
          GameState.LocalDecorator.HideWeapons();
        type = UberstrikeItemClass.GearFace;
        break;
      case LoadoutSlotType.GearGloves:
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(Singleton<ItemManager>.Instance.DefaultGlovesItemId, LoadoutSlotType.GearGloves);
        type = UberstrikeItemClass.GearGloves;
        break;
      case LoadoutSlotType.GearUpperBody:
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(Singleton<ItemManager>.Instance.DefaultUpperBodyItemId, LoadoutSlotType.GearUpperBody);
        type = UberstrikeItemClass.GearUpperBody;
        break;
      case LoadoutSlotType.GearLowerBody:
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(Singleton<ItemManager>.Instance.DefaultLowerBodyItemId, LoadoutSlotType.GearLowerBody);
        type = UberstrikeItemClass.GearLowerBody;
        break;
      case LoadoutSlotType.GearBoots:
        Singleton<InventoryManager>.Instance.EquipItemOnSlot(Singleton<ItemManager>.Instance.DefaultBootsItemId, LoadoutSlotType.GearBoots);
        type = UberstrikeItemClass.GearBoots;
        break;
      case LoadoutSlotType.GearHolo:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.GearHolo);
        Singleton<TemporaryLoadoutManager>.Instance.SetGearLoadout(LoadoutSlotType.GearHolo, (IUnityItem) null);
        SfxManager.Play2dAudioClip(GameAudio.EquipGear);
        if ((bool) (UnityEngine.Object) GameState.LocalDecorator)
          GameState.LocalDecorator.HideWeapons();
        type = UberstrikeItemClass.GearHolo;
        break;
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
      case LoadoutSlotType.QuickUseItem1:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.QuickUseItem1);
        break;
      case LoadoutSlotType.QuickUseItem2:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.QuickUseItem2);
        break;
      case LoadoutSlotType.QuickUseItem3:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.QuickUseItem3);
        break;
      case LoadoutSlotType.FunctionalItem1:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.FunctionalItem1);
        break;
      case LoadoutSlotType.FunctionalItem2:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.FunctionalItem2);
        break;
      case LoadoutSlotType.FunctionalItem3:
        Singleton<LoadoutManager>.Instance.ResetSlot(LoadoutSlotType.FunctionalItem3);
        break;
    }
    if (type != (UberstrikeItemClass) 0)
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, type, slotType == LoadoutSlotType.GearHolo);
    else
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, (UberstrikeItemClass) 0);
    this.HighlightingSlot(slotType);
  }

  private void SetActiveLoadoutActiveSpaces(int slots, float width)
  {
    this._activeLoadoutUsedSpace.Clear();
    for (int index = 0; index < slots; ++index)
      this._activeLoadoutUsedSpace.Add(new Rect(0.0f, (float) (index * 70), width - 5f, 70f));
  }

  private void SetActiveLoadoutActiveSpaces(params Rect[] rects)
  {
    this._activeLoadoutUsedSpace.Clear();
    for (int index = 0; index < rects.Length; ++index)
      this._activeLoadoutUsedSpace.Add(rects[index]);
  }

  private bool IsMouseCursorInLoadout(Vector2 pos)
  {
    for (int index = 0; index < this._activeLoadoutUsedSpace.Count; ++index)
    {
      if (this._activeLoadoutUsedSpace[index].Contains(pos))
        return true;
    }
    return false;
  }

  private void ExchangedWeaponLoadoutData(int firstSlot, int secondSlot)
  {
    switch (firstSlot)
    {
      case 1:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponPrimary))
        {
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponPrimary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponPrimary));
          break;
        }
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponPrimary, (BaseWeaponDecorator) null);
        break;
      case 2:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponSecondary))
        {
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponSecondary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponSecondary));
          break;
        }
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponSecondary, (BaseWeaponDecorator) null);
        break;
      case 3:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponTertiary))
        {
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponTertiary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponTertiary));
          break;
        }
        GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponTertiary, (BaseWeaponDecorator) null);
        break;
    }
    switch (secondSlot)
    {
      case 1:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponPrimary))
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponPrimary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponPrimary));
        else
          GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponPrimary, (BaseWeaponDecorator) null);
        GameState.LocalDecorator.SetActiveWeaponSlot(LoadoutSlotType.WeaponPrimary);
        break;
      case 2:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponSecondary))
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponSecondary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponSecondary));
        else
          GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponSecondary, (BaseWeaponDecorator) null);
        GameState.LocalDecorator.SetActiveWeaponSlot(LoadoutSlotType.WeaponSecondary);
        break;
      case 3:
        if (Singleton<LoadoutManager>.Instance.HasLoadoutItem(LoadoutSlotType.WeaponTertiary))
          Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponTertiary, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponTertiary));
        else
          GameState.LocalDecorator.AssignWeapon(LoadoutSlotType.WeaponTertiary, (BaseWeaponDecorator) null);
        GameState.LocalDecorator.SetActiveWeaponSlot(LoadoutSlotType.WeaponTertiary);
        break;
    }
  }

  public void HighlightingSlot(LoadoutSlotType slot)
  {
    this._highlightedSlot = slot;
    this._highlightedSlotAlpha = 0.5f;
    switch (slot)
    {
      case LoadoutSlotType.WeaponMelee:
      case LoadoutSlotType.WeaponPrimary:
      case LoadoutSlotType.WeaponSecondary:
      case LoadoutSlotType.WeaponTertiary:
        this.SelectLoadoutArea(LoadoutArea.Weapons);
        break;
      case LoadoutSlotType.QuickUseItem1:
      case LoadoutSlotType.QuickUseItem2:
      case LoadoutSlotType.QuickUseItem3:
      case LoadoutSlotType.FunctionalItem1:
      case LoadoutSlotType.FunctionalItem2:
      case LoadoutSlotType.FunctionalItem3:
        this.SelectLoadoutArea(LoadoutArea.QuickItems);
        break;
      default:
        this.SelectLoadoutArea(LoadoutArea.Gear);
        break;
    }
  }

  public void SelectLoadoutArea(LoadoutArea area)
  {
    switch (area)
    {
      case LoadoutArea.Weapons:
        this.SetActiveLoadoutActiveSpaces(4, 185f);
        break;
      case LoadoutArea.Gear:
      case LoadoutArea.QuickItems:
        this.SetActiveLoadoutActiveSpaces(6, 185f);
        break;
    }
    CmuneEventHandler.Route((object) new LoadoutAreaChangedEvent()
    {
      Area = area
    });
  }

  private void UpdateShopItems()
  {
    this._shopItemGUIList.Clear();
    this._inventoryItemGUIList.Clear();
    foreach (InventoryItem allItem in Singleton<InventoryManager>.Instance.GetAllItems(false))
      this._inventoryItemGUIList.Add((BaseItemGUI) new InventoryItemGUI(allItem, this._location));
    this._inventoryItemGUIList.Sort((IComparer<BaseItemGUI>) this._inventoryComparer);
    foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.GetShopItems())
    {
      if (shopItem.ItemView.IsConsumable)
        this._shopItemGUIList.Add((BaseItemGUI) new ShopConsumableItemGUI(shopItem, this._location));
      else
        this._shopItemGUIList.Add((BaseItemGUI) new ShopRentItemGUI(shopItem, this._location));
    }
    this._shopItemGUIList.Sort((IComparer<BaseItemGUI>) this._shopComparer);
  }

  private void OnRefreshCurrentItemListEvent(ShopRefreshCurrentItemListEvent ev) => this.UpdateShopItems();

  private void DrawLoadout(Rect rect)
  {
    this._loadoutArea = rect;
    this._loadoutArea.x += this._rectLabs.x;
    this._loadoutArea.y += this._rectLabs.y;
    GUI.BeginGroup(rect, string.Empty, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, rect.width - 2f, 76f), LocalizedStrings.LoadoutCaps, BlueStonez.tab_strip_large);
    int index = UnityGUI.Toolbar(new Rect(4f, 32f, 120f, 44f), this._loadoutAreaSelection.Index, this._loadoutAreaSelection.GuiContent, this._loadoutAreaSelection.Length, BlueStonez.tab_largeicon);
    if (index != this._loadoutAreaSelection.Index)
    {
      this._loadoutAreaSelection.SetIndex(index);
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    Rect position = new Rect(0.0f, 76f, rect.width, rect.height - 76f);
    switch (this._loadoutAreaSelection.Current)
    {
      case LoadoutArea.Weapons:
        this.DrawWeaponLoadout(position);
        break;
      case LoadoutArea.Gear:
        this.DrawGearLoadout(position);
        break;
      case LoadoutArea.QuickItems:
        this.DrawQuickItemLoadout(position);
        break;
    }
    GUI.EndGroup();
  }

  private void DrawShop(Rect labsRect)
  {
    this._shopArea = labsRect;
    this._shopArea.x += this._rectLabs.x;
    this._shopArea.y += this._rectLabs.y;
    bool flag1 = false;
    if (!Application.isWebPlayer || Application.isEditor)
      flag1 = true;
    GUI.BeginGroup(labsRect, BlueStonez.window);
    this.DrawShopTabs(labsRect);
    if (this._shopAreaSelection.Current == ShopArea.Inventory || this._shopAreaSelection.Current == ShopArea.Shop)
      this._searchBar.Draw(new Rect(!flag1 ? labsRect.width - 128f : labsRect.width - 200f, 8f, 123f, 20f));
    switch (this._shopAreaSelection.Current)
    {
      case ShopArea.Inventory:
        this.DrawItemGUIList<BaseItemGUI>(this._inventoryItemGUIList, labsRect);
        this.DrawSortBar(new Rect(0.0f, 74f, labsRect.width, 22f), false, true);
        break;
      case ShopArea.Shop:
        this.DrawItemGUIList<BaseItemGUI>(this._shopItemGUIList, labsRect);
        this.DrawShopSubTabs(labsRect, true);
        break;
      case ShopArea.MysteryBox:
        this._lotteryGui.Draw(new Rect(0.0f, 74f, labsRect.width, labsRect.height - 74f));
        break;
      case ShopArea.Packs:
        this._masBundleGui.Draw(new Rect(0.0f, 74f, labsRect.width, labsRect.height - 74f));
        break;
      case ShopArea.Credits:
        this._creditsGui.Draw(new Rect(0.0f, 74f, labsRect.width, labsRect.height - 74f));
        break;
    }
    if (flag1)
    {
      bool flag2 = PlayerDataManager.IsPlayerLoggedIn && GUITools.SaveClickIn(7f);
      GUI.enabled = flag2 || PlayerDataManager.IsPlayerLoggedIn && GUITools.SaveClickIn(7f);
      if (GUITools.Button(new Rect(labsRect.width - 66f, 7f, 64f, 20f), new GUIContent(LocalizedStrings.Refresh), BlueStonez.buttondark_medium))
      {
        GUITools.Clicked();
        ApplicationDataManager.RefreshWallet();
      }
      GUI.enabled = flag2;
    }
    GUI.EndGroup();
  }

  private void DrawShopTabs(Rect rect)
  {
    rect = new Rect(0.0f, 0.0f, rect.width, rect.height);
    GUI.Box(rect, string.Empty, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, rect.width, 76f), LocalizedStrings.ShopCaps, BlueStonez.tab_strip_large);
    int index = UnityGUI.Toolbar(new Rect(1f, 32f, (float) (this._shopAreaSelection.Length * 44 - 2), 44f), this._shopAreaSelection.Index, this._shopAreaSelection.GuiContent, BlueStonez.tab_largeicon);
    if (index == this._shopAreaSelection.Index)
      return;
    this._shopAreaSelection.SetIndex(index);
    this._searchBar.ClearFilter();
    SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
  }

  private void DrawShopSubTabs(Rect position, bool showLevel)
  {
    int index = UnityGUI.Toolbar(new Rect(1f, 74f, position.width - 2f, 44f), this._typeSelection.Index, this._typeSelection.GuiContent, this._typeSelection.Length, BlueStonez.tab_large);
    if (index != this._typeSelection.Index)
    {
      this._typeSelection.SetIndex(index);
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    if (this._typeSelection.Current == UberstrikeItemType.Weapon)
    {
      this.DrawWeaponClassFilter(new Rect(0.0f, 114f, position.width, 30f));
      this.DrawSortBar(new Rect(0.0f, 149f, position.width + 1f, 22f), showLevel, false);
    }
    else if (this._typeSelection.Current == UberstrikeItemType.Gear)
    {
      this.DrawGearClassFilter(new Rect(0.0f, 114f, position.width, 30f));
      this.DrawSortBar(new Rect(0.0f, 149f, position.width + 1f, 22f), showLevel, false);
    }
    else
      this.DrawSortBar(new Rect(0.0f, 118f, position.width, 22f), showLevel, false);
  }

  private void DrawWeaponClassFilter(Rect rect)
  {
    GUI.changed = false;
    int index = UnityGUI.Toolbar(new Rect(rect.x, rect.y + 5f, rect.width, rect.height), this._weaponClassSelection.Index, this._weaponClassSelection.GuiContent, this._weaponClassSelection.Length, BlueStonez.tab_large);
    if (!GUI.changed)
      return;
    if (index == this._weaponClassSelection.Index)
      this._weaponClassSelection.SetIndex(-1);
    else
      this._weaponClassSelection.SetIndex(index);
    SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
  }

  private void DrawGearClassFilter(Rect rect)
  {
    GUI.changed = false;
    int index = UnityGUI.Toolbar(new Rect(rect.x, rect.y + 5f, rect.width, rect.height), this._gearClassSelection.Index, this._gearClassSelection.GuiContent, this._gearClassSelection.Length, BlueStonez.tab_large);
    if (!GUI.changed)
      return;
    if (index == this._gearClassSelection.Index)
      this._gearClassSelection.SetIndex(-1);
    else
      this._gearClassSelection.SetIndex(index);
    SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
  }

  private void DrawSortBar(Rect sortRect, bool showLevel, bool showExpDay)
  {
    ShopSorting.ItemComparer<BaseItemGUI> itemComparer = this._shopAreaSelection.Current != ShopArea.Shop ? this._inventoryComparer : this._shopComparer;
    GUI.BeginGroup(sortRect);
    if (!showLevel && showExpDay)
    {
      if (GUI.Button(new Rect(0.0f, 0.0f, sortRect.width - 134f, sortRect.height), string.Empty, BlueStonez.box_grey50))
        this.SortShopPages(ShopSortedColumns.Name);
      if (itemComparer.Column == ShopSortedColumns.Name)
      {
        if (itemComparer.Ascending)
          GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 134f, sortRect.height), new GUIContent(LocalizedStrings.Name, (Texture) ShopIcons.ArrowSmallUpWhite), BlueStonez.label_interparkmed_11pt);
        else
          GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 134f, sortRect.height), new GUIContent(LocalizedStrings.Name, (Texture) ShopIcons.ArrowSmallDownWhite), BlueStonez.label_interparkmed_11pt);
      }
      else
        GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 134f, sortRect.height), new GUIContent(LocalizedStrings.Name), BlueStonez.label_interparkmed_11pt);
      if (GUI.Button(new Rect(sortRect.width - 181f, 0.0f, 113f, sortRect.height), string.Empty, BlueStonez.box_grey50))
        this.SortShopPages(ShopSortedColumns.Duration);
      if (itemComparer.Column == ShopSortedColumns.Duration)
      {
        if (itemComparer.Ascending)
          GUI.Label(new Rect(sortRect.width - 181f, 0.0f, 113f, sortRect.height), new GUIContent(LocalizedStrings.Duration, (Texture) ShopIcons.ArrowSmallUpWhite), BlueStonez.label_interparkmed_11pt);
        else
          GUI.Label(new Rect(sortRect.width - 181f, 0.0f, 113f, sortRect.height), new GUIContent(LocalizedStrings.Duration, (Texture) ShopIcons.ArrowSmallDownWhite), BlueStonez.label_interparkmed_11pt);
      }
      else
        GUI.Label(new Rect(sortRect.width - 136f, 0.0f, 64f, sortRect.height), new GUIContent(LocalizedStrings.Duration), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(sortRect.width - 73f, 0.0f, 72f, sortRect.height), string.Empty, BlueStonez.box_grey50);
    }
    else if (showLevel && !showExpDay)
    {
      if (GUI.Button(new Rect(0.0f, 0.0f, sortRect.width - 179f, sortRect.height), string.Empty, BlueStonez.box_grey50))
        this.SortShopPages(ShopSortedColumns.Name);
      if (itemComparer.Column == ShopSortedColumns.Name)
      {
        if (itemComparer.Ascending)
          GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 179f, sortRect.height), new GUIContent(LocalizedStrings.Name, (Texture) ShopIcons.ArrowSmallUpWhite), BlueStonez.label_interparkmed_11pt);
        else
          GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 179f, sortRect.height), new GUIContent(LocalizedStrings.Name, (Texture) ShopIcons.ArrowSmallDownWhite), BlueStonez.label_interparkmed_11pt);
      }
      else
        GUI.Label(new Rect(0.0f, 0.0f, sortRect.width - 179f, sortRect.height), new GUIContent(LocalizedStrings.Name), BlueStonez.label_interparkmed_11pt);
      if (GUI.Button(new Rect(sortRect.width - 181f, 0.0f, 48f, sortRect.height), string.Empty, BlueStonez.box_grey50))
        this.SortShopPages(ShopSortedColumns.Level);
      if (itemComparer.Column == ShopSortedColumns.Level)
      {
        if (itemComparer.Ascending)
          GUI.Label(new Rect(sortRect.width - 181f, 0.0f, 48f, sortRect.height), new GUIContent(LocalizedStrings.Level, (Texture) ShopIcons.ArrowSmallUpWhite), BlueStonez.label_interparkmed_11pt);
        else
          GUI.Label(new Rect(sortRect.width - 181f, 0.0f, 48f, sortRect.height), new GUIContent(LocalizedStrings.Level, (Texture) ShopIcons.ArrowSmallDownWhite), BlueStonez.label_interparkmed_11pt);
      }
      else
        GUI.Label(new Rect(sortRect.width - 181f, 0.0f, 48f, sortRect.height), new GUIContent(LocalizedStrings.Level), BlueStonez.label_interparkmed_11pt);
      if (GUI.Button(new Rect(sortRect.width - 134f, 0.0f, 65f, sortRect.height), string.Empty, BlueStonez.box_grey50))
        this.SortShopPages(ShopSortedColumns.PriceShop);
      if (itemComparer.Column == ShopSortedColumns.PriceShop)
      {
        if (itemComparer.Ascending)
          GUI.Label(new Rect(sortRect.width - 134f, 0.0f, 65f, sortRect.height), new GUIContent(LocalizedStrings.Price, (Texture) ShopIcons.ArrowSmallUpWhite), BlueStonez.label_interparkmed_11pt);
        else
          GUI.Label(new Rect(sortRect.width - 134f, 0.0f, 65f, sortRect.height), new GUIContent(LocalizedStrings.Price, (Texture) ShopIcons.ArrowSmallDownWhite), BlueStonez.label_interparkmed_11pt);
      }
      else
        GUI.Label(new Rect(sortRect.width - 134f, 0.0f, 65f, sortRect.height), new GUIContent(LocalizedStrings.Price), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(sortRect.width - 71f, 0.0f, 70f, sortRect.height), string.Empty, BlueStonez.box_grey50);
    }
    GUI.EndGroup();
  }

  private void DrawItemGUIList<T>(List<T> list, Rect position) where T : BaseItemGUI
  {
    int num1 = this._typeSelection.Current == UberstrikeItemType.Weapon || this._typeSelection.Current == UberstrikeItemType.Gear ? 58 : 29;
    int num2 = -1;
    int num3 = 0;
    int height = 60;
    int top = this._shopAreaSelection.Current != ShopArea.Inventory ? 116 + num1 : 109;
    Rect position1 = new Rect(0.0f, (float) top, position.width, position.height - (float) (top + 1));
    Rect contentRect = new Rect(0.0f, 0.0f, position.width - 20f, (float) ((list.Count - this._skippedDefaultGearCount) * height + 106));
    bool flag = position1.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen;
    this._labScroll = GUITools.BeginScrollView(position1, this._labScroll, contentRect);
    int num4 = (double) contentRect.height <= (double) position1.height ? 5 : 20;
    this._skippedDefaultGearCount = 0;
    int num5 = -height;
    for (int index = 0; index < list.Count; ++index)
    {
      if (!this._searchBar.CheckIfPassFilter(list[index].Item.Name))
        ++this._skippedDefaultGearCount;
      else if (this._itemFilter != null && !this._itemFilter.CanPass(list[index].Item))
      {
        ++this._skippedDefaultGearCount;
      }
      else
      {
        num5 += height;
        if ((double) (num5 + height) >= (double) this._labScroll.y && (double) num5 <= (double) this._labScroll.y + (double) position1.height)
        {
          Rect rect1 = new Rect(0.0f, (float) (num5 + (num2 != -1 ? num3 - 20 : 0)), position.width - (float) num4, (float) height);
          Rect rect2 = new Rect(rect1.x, rect1.y, rect1.width - 100f, rect1.height);
          list[index].Draw(rect1, rect1.Contains(Event.current.mousePosition));
          if (flag && rect2.Contains(Event.current.mousePosition) && !Singleton<DragAndDrop>.Instance.IsDragging)
            AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(list[index].Item, rect1, PopupViewSide.Left);
          Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(rect1, new ShopPageGUI.ShopDragSlot()
          {
            Item = list[index].Item,
            Slot = LoadoutSlotType.Shop
          }, new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropShop), isItemList: true);
        }
      }
    }
    GUITools.EndScrollView();
  }

  private void UpdateItemFilter()
  {
    switch (this._shopAreaSelection.Current)
    {
      case ShopArea.Inventory:
        this._itemFilter = (IShopItemFilter) new InventoryItemFilter();
        break;
      case ShopArea.Shop:
        switch (this._typeSelection.Current)
        {
          case UberstrikeItemType.Weapon:
            if (this._weaponClassSelection.Current == (UberstrikeItemClass) 0)
            {
              this._itemFilter = (IShopItemFilter) new ItemByTypeFilter(this._typeSelection.Current);
              return;
            }
            this._itemFilter = (IShopItemFilter) new ItemByClassFilter(this._typeSelection.Current, this._weaponClassSelection.Current);
            return;
          case UberstrikeItemType.WeaponMod:
            return;
          case UberstrikeItemType.Gear:
            if (this._gearClassSelection.Current == (UberstrikeItemClass) 0)
            {
              this._itemFilter = (IShopItemFilter) new ItemByTypeFilter(this._typeSelection.Current);
              return;
            }
            this._itemFilter = (IShopItemFilter) new ItemByClassFilter(this._typeSelection.Current, this._gearClassSelection.Current);
            return;
          case UberstrikeItemType.QuickUse:
          case UberstrikeItemType.Functional:
            this._itemFilter = (IShopItemFilter) new ItemByTypeFilter(this._typeSelection.Current);
            return;
          case UberstrikeItemType.Special:
            this._itemFilter = (IShopItemFilter) new SpecialItemFilter();
            return;
          default:
            return;
        }
    }
  }

  private void DrawWeaponLoadout(Rect position)
  {
    this._loadoutWeaponScroll = GUITools.BeginScrollView(position, this._loadoutWeaponScroll, new Rect(0.0f, 0.0f, position.width - 20f, 285f));
    string[] strArray = new string[4]
    {
      LocalizedStrings.Melee,
      LocalizedStrings.PrimaryWeapon,
      LocalizedStrings.SecondaryWeapon,
      LocalizedStrings.TertiaryWeapon
    };
    LoadoutSlotType[] loadoutSlotTypeArray = new LoadoutSlotType[4]
    {
      LoadoutSlotType.WeaponMelee,
      LoadoutSlotType.WeaponPrimary,
      LoadoutSlotType.WeaponSecondary,
      LoadoutSlotType.WeaponTertiary
    };
    Rect position1 = new Rect();
    for (int index = 0; index < 4; ++index)
    {
      Rect rect = new Rect(0.0f, (float) (70 * index), position.width - 5f, 70f);
      this.DrawLoadoutWeaponItem(strArray[index], Singleton<LoadoutManager>.Instance.GetItemOnSlot(loadoutSlotTypeArray[index]), rect, loadoutSlotTypeArray[index]);
      if (loadoutSlotTypeArray[index] == Singleton<InventoryManager>.Instance.CurrentWeaponSlot)
      {
        position1.x = rect.x + 5f;
        position1.y = rect.y;
        position1.width = rect.width - 16f;
        position1.height = rect.height - 10f;
      }
    }
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Box(position1, GUIContent.none, BlueStonez.group_grey81);
    GUI.color = Color.white;
    if (this._showRenewLoadoutButton)
    {
      Rect[] rectArray = new Rect[4]
      {
        new Rect(0.0f, 0.0f, 5f, 70f),
        new Rect(0.0f, 70f, 5f, 70f),
        new Rect(0.0f, 140f, 5f, 70f),
        new Rect(0.0f, 210f, 5f, 70f)
      };
      for (int index = 0; index < LoadoutManager.WeaponSlots.Length; ++index)
      {
        LoadoutSlotType weaponSlot = LoadoutManager.WeaponSlots[index];
        this._renewItem[weaponSlot] = GUI.Toggle(rectArray[index], this._renewItem[weaponSlot], !this._renewItem[weaponSlot] ? "<" : ">", BlueStonez.panelquad_toggle);
      }
    }
    GUI.color = Color.white;
    GUITools.EndScrollView();
  }

  private void DrawGearLoadout(Rect position)
  {
    Rect[] rectArray1 = new Rect[6]
    {
      new Rect(0.0f, 70f, position.width - 5f, 70f),
      new Rect(0.0f, 140f, position.width - 5f, 70f),
      new Rect(0.0f, 210f, position.width - 5f, 70f),
      new Rect(0.0f, 280f, position.width - 5f, 70f),
      new Rect(0.0f, 350f, position.width - 5f, 70f),
      new Rect(0.0f, 420f, position.width - 5f, 70f)
    };
    Rect[] rectArray2 = new Rect[6]
    {
      new Rect(0.0f, 0.0f, 5f, 60f),
      new Rect(0.0f, 60f, 5f, 70f),
      new Rect(0.0f, 130f, 5f, 70f),
      new Rect(0.0f, 200f, 5f, 70f),
      new Rect(0.0f, 270f, 5f, 70f),
      new Rect(0.0f, 340f, 5f, 70f)
    };
    this._loadoutGearScroll = GUITools.BeginScrollView(position, this._loadoutGearScroll, new Rect(0.0f, 0.0f, position.width - 20f, 490f));
    this.DrawLoadoutGearItem(LocalizedStrings.Holo, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.GearHolo), LoadoutSlotType.GearHolo, new Rect(0.0f, 0.0f, position.width - 5f, 70f), UberstrikeItemClass.GearHolo);
    for (int index = 0; index < LoadoutManager.GearSlots.Length; ++index)
    {
      string gearSlotName = LoadoutManager.GearSlotNames[index];
      LoadoutSlotType gearSlot = LoadoutManager.GearSlots[index];
      this.DrawLoadoutGearItem(gearSlotName, Singleton<LoadoutManager>.Instance.GetItemOnSlot(gearSlot), gearSlot, rectArray1[index], LoadoutManager.GearSlotClasses[index]);
    }
    if (this._showRenewLoadoutButton)
    {
      for (int index = 0; index < LoadoutManager.GearSlots.Length; ++index)
      {
        Rect position1 = rectArray2[index];
        LoadoutSlotType gearSlot = LoadoutManager.GearSlots[index];
        this._renewItem[gearSlot] = GUI.Toggle(position1, this._renewItem[gearSlot], !this._renewItem[gearSlot] ? "<" : ">", BlueStonez.panelquad_toggle);
      }
    }
    GUITools.EndScrollView();
  }

  private void DrawQuickItemLoadout(Rect position)
  {
    this._loadoutQuickUseFuncScroll = GUITools.BeginScrollView(position, this._loadoutQuickUseFuncScroll, new Rect(0.0f, 0.0f, position.width - 20f, 285f));
    Rect position1 = new Rect();
    for (int index = 0; index < 3; ++index)
    {
      this.DrawLoadoutQuickUseItem(LocalizedStrings.QuickItem + " " + (index + 1).ToString(), Singleton<LoadoutManager>.Instance.GetItemOnSlot((LoadoutSlotType) (12 + index)), (LoadoutSlotType) (12 + index), new Rect(0.0f, (float) (70 * index), position.width - 5f, 70f), AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString((GameInputKey) (16 + index)));
      if (Singleton<InventoryManager>.Instance.CurrentQuickItemSot == (LoadoutSlotType) (12 + index))
      {
        position1.x = 5f;
        position1.y = (float) (70 * index);
        position1.width = position.width - 16f;
        position1.height = 60f;
      }
    }
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Box(position1, GUIContent.none, BlueStonez.group_grey81);
    GUI.color = Color.white;
    GUI.color = Color.white;
    GUITools.EndScrollView();
  }

  private void DrawLoadoutWeaponItem(
    string slotName,
    InventoryItem item,
    Rect rect,
    LoadoutSlotType slot)
  {
    Rect rect1 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
    GUI.BeginGroup(rect1);
    if (item.Item != null)
    {
      GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), (Texture) item.Item.Icon, BlueStonez.item_slot_large);
      GUI.Label(new Rect(0.0f, 5f, rect1.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
      GUI.Label(new Rect(0.0f, 30f, rect1.width - 65f, 12f), item.Item.Name, BlueStonez.label_interparkmed_10pt_right);
      GUI.Label(new Rect(0.0f, rect1.height - 1f, rect1.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
    }
    else
    {
      GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
      GUI.Label(new Rect(0.0f, 5f, rect1.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
      GUI.Label(new Rect(0.0f, rect1.height - 1f, rect1.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
    }
    GUI.EndGroup();
    if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen)
    {
      if (Event.current.type == UnityEngine.EventType.MouseDown)
      {
        if (Singleton<InventoryManager>.Instance.CurrentWeaponSlot != slot)
        {
          Singleton<InventoryManager>.Instance.CurrentWeaponSlot = slot;
          GameState.LocalDecorator.SetActiveWeaponSlot(slot);
          GameState.LocalDecorator.ShowWeapon(slot);
          InventoryItem inventoryItem;
          if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(slot, out inventoryItem))
            AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, inventoryItem.Item.ItemClass);
          else
            AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, (UberstrikeItemClass) 0);
        }
      }
      else
        AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(item.Item, rect1, PopupViewSide.Left, item.DaysRemaining);
    }
    Color? color = new Color?();
    if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemClass == UberstrikeItemClass.WeaponMelee && slot == LoadoutSlotType.WeaponMelee)
      color = new Color?(new Color(1f, 1f, 1f, 0.1f));
    else if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemClass != UberstrikeItemClass.WeaponMelee && slot != LoadoutSlotType.WeaponMelee)
      color = new Color?(new Color(1f, 1f, 1f, 0.1f));
    else if (slot == this._highlightedSlot)
      color = new Color?(new Color(1f, 1f, 1f, this._highlightedSlotAlpha));
    Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(new Rect(rect1.x, rect1.y - 5f, rect1.width - 6f, rect1.height), new ShopPageGUI.ShopDragSlot()
    {
      Item = item.Item,
      Slot = slot
    }, new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropLoadout), color);
  }

  private void DrawLoadoutGearItem(
    string slotName,
    InventoryItem item,
    LoadoutSlotType loadoutSlotType,
    Rect rect,
    UberstrikeItemClass itemClass)
  {
    Rect rect1 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
    GUI.BeginGroup(rect1);
    if (item.Item != null && !Singleton<ItemManager>.Instance.IsDefaultGearItem(item.Item.ItemView.PrefabName))
    {
      GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), (Texture) item.Item.Icon, BlueStonez.item_slot_large);
      GUI.Label(new Rect(0.0f, 5f, rect1.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
      GUI.Label(new Rect(0.0f, 30f, rect1.width - 65f, 12f), item.Item.Name, BlueStonez.label_interparkmed_10pt_right);
    }
    else
    {
      GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
      GUI.Label(new Rect(0.0f, 5f, rect1.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
    }
    GUI.Label(new Rect(0.0f, rect1.height - 5f, rect1.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
    GUI.EndGroup();
    if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen)
    {
      if (Event.current.type == UnityEngine.EventType.MouseDown)
      {
        if (item.Item != null)
          AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, item.Item.ItemClass);
      }
      else
        AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(item.Item, rect1, PopupViewSide.Left, item.DaysRemaining);
    }
    Color? color = new Color?();
    if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemClass == itemClass)
      color = new Color?(new Color(1f, 1f, 1f, 0.2f));
    else if (loadoutSlotType == this._highlightedSlot)
      color = new Color?(new Color(1f, 1f, 1f, this._highlightedSlotAlpha));
    Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(new Rect(rect1.x, rect1.y - 15f, rect1.width, rect1.height + 11f), new ShopPageGUI.ShopDragSlot()
    {
      Item = item.Item,
      Slot = loadoutSlotType
    }, new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropLoadout), color);
  }

  private void DrawLoadoutQuickUseItem(
    string slotName,
    InventoryItem itemQuickUse,
    LoadoutSlotType loadoutSlotType,
    Rect rect,
    string slotTag)
  {
    Rect rect1 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
    GUI.BeginGroup(rect1);
    if (itemQuickUse != null && itemQuickUse.Item != null)
    {
      if (itemQuickUse.Item.ItemView is QuickItemConfiguration)
      {
        GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), (Texture) itemQuickUse.Item.Icon, BlueStonez.item_slot_large);
        GUI.Label(new Rect(3f, 5f, rect1.width - 65f, 26f), itemQuickUse.Item.Name, BlueStonez.label_interparkbold_13pt_left);
        if (itemQuickUse.AmountRemaining > 0)
        {
          GUI.color = Color.white.SetAlpha(0.5f);
          GUI.Label(new Rect(3f, 34f, rect1.width - 65f, 12f), string.Format("Uses: {0}", (object) itemQuickUse.AmountRemaining), BlueStonez.label_interparkbold_11pt_left);
          GUI.color = Color.white;
        }
        GUI.Label(new Rect(0.0f, rect1.height - 1f, rect1.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
      }
    }
    else
    {
      GUI.Label(new Rect(rect1.width - 60f, 0.0f, 48f, 48f), GUIContent.none, BlueStonez.item_slot_large);
      GUI.Label(new Rect(0.0f, 5f, rect1.width - 65f, 18f), slotName, BlueStonez.label_interparkmed_18pt_right);
      GUI.Label(new Rect(0.0f, rect1.height - 1f, rect1.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
    }
    GUI.EndGroup();
    if (rect.Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen)
    {
      if (Event.current.type == UnityEngine.EventType.MouseDown)
        Singleton<InventoryManager>.Instance.CurrentQuickItemSot = loadoutSlotType;
      else
        AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(itemQuickUse.Item, rect1, PopupViewSide.Left);
    }
    Color? color = new Color?();
    if (Singleton<DragAndDrop>.Instance.IsDragging && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemType == UberstrikeItemType.QuickUse)
      color = new Color?(new Color(1f, 1f, 1f, 0.1f));
    else if (loadoutSlotType == this._highlightedSlot)
      color = new Color?(new Color(1f, 1f, 1f, this._highlightedSlotAlpha));
    Singleton<DragAndDrop>.Instance.DrawSlot<ShopPageGUI.ShopDragSlot>(new Rect(rect1.x, rect1.y - 5f, rect1.width - 6f, rect1.height), new ShopPageGUI.ShopDragSlot()
    {
      Item = itemQuickUse.Item,
      Slot = loadoutSlotType
    }, new Action<int, ShopPageGUI.ShopDragSlot>(this.OnDropLoadout), color);
  }

  private void OnDropAvatar(int slotId, ShopPageGUI.ShopDragSlot item)
  {
    if (item.Slot == LoadoutSlotType.Shop)
      this.EquipItemFromArea(item.Item, LoadoutSlotType.None, ShopArea.Shop);
    else
      this.UnequipItem(item.Item);
  }

  private void OnDropShop(int slotId, ShopPageGUI.ShopDragSlot item)
  {
    if (item.Slot == LoadoutSlotType.Shop)
      return;
    this.UnequipItem(item.Item);
  }

  private void OnDropLoadout(int slotId, ShopPageGUI.ShopDragSlot item)
  {
    Singleton<InventoryManager>.Instance.CurrentWeaponSlot = (LoadoutSlotType) slotId;
    if (item.Slot == LoadoutSlotType.Shop)
    {
      this.EquipItemFromArea(item.Item, (LoadoutSlotType) slotId, ShopArea.Shop);
    }
    else
    {
      switch (slotId)
      {
        case 8:
        case 9:
        case 10:
          if (item.Slot < LoadoutSlotType.WeaponPrimary || item.Slot > LoadoutSlotType.WeaponTertiary)
            break;
          this.SwapWeapons(item.Slot, (LoadoutSlotType) slotId);
          break;
        case 12:
        case 13:
        case 14:
          this.SwapQuickItems(item.Slot, (LoadoutSlotType) slotId);
          break;
      }
    }
  }

  private void OnBeginDrag(IDragSlot item)
  {
    if (item == null)
      return;
    switch (item.Item.ItemType)
    {
      case UberstrikeItemType.Weapon:
      case UberstrikeItemType.WeaponMod:
        this._loadoutAreaSelection.SetIndex(0);
        this.SelectLoadoutArea(LoadoutArea.Weapons);
        break;
      case UberstrikeItemType.Gear:
        this._loadoutAreaSelection.SetIndex(1);
        this.SelectLoadoutArea(LoadoutArea.Gear);
        break;
      case UberstrikeItemType.QuickUse:
      case UberstrikeItemType.Functional:
        this._loadoutAreaSelection.SetIndex(2);
        this.SelectLoadoutArea(LoadoutArea.QuickItems);
        break;
    }
  }

  private void SwapQuickItems(LoadoutSlotType slot, LoadoutSlotType newslot)
  {
    if (!Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slot, newslot))
      return;
    Singleton<InventoryManager>.Instance.CurrentQuickItemSot = newslot;
    this.HighlightingSlot(newslot);
  }

  private void SwapWeapons(LoadoutSlotType slot, LoadoutSlotType newslot)
  {
    if (!Singleton<LoadoutManager>.Instance.SwapLoadoutItems(slot, newslot))
      return;
    Singleton<InventoryManager>.Instance.CurrentWeaponSlot = newslot;
    this.HighlightingSlot(newslot);
  }

  private void SortShopPages(ShopSortedColumns sortedColumn)
  {
    ShopSorting.ItemComparer<BaseItemGUI> comparer = (ShopSorting.ItemComparer<BaseItemGUI>) null;
    switch (sortedColumn)
    {
      case ShopSortedColumns.PriceShop:
        comparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.PriceComparer();
        break;
      case ShopSortedColumns.Level:
        comparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.LevelComparer();
        break;
      case ShopSortedColumns.Duration:
        comparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.DurationComparer();
        break;
      case ShopSortedColumns.Name:
        comparer = (ShopSorting.ItemComparer<BaseItemGUI>) new ShopSorting.NameComparer();
        break;
    }
    this.SortShopBy(comparer);
  }

  private void SortShopBy(ShopSorting.ItemComparer<BaseItemGUI> comparer)
  {
    switch (this._shopAreaSelection.Current)
    {
      case ShopArea.Inventory:
        if (this._inventoryComparer.GetType() == comparer.GetType())
        {
          if (comparer.Column == this._inventoryComparer.Column)
          {
            this._inventoryComparer.SwitchOrder();
            break;
          }
          break;
        }
        this._inventoryComparer = comparer;
        break;
      case ShopArea.Shop:
        if (this._shopComparer.GetType() == comparer.GetType())
        {
          if (comparer.Column == this._shopComparer.Column)
          {
            this._shopComparer.SwitchOrder();
            break;
          }
          break;
        }
        this._shopComparer = comparer;
        break;
    }
    this.ApplyCurrentSorting();
  }

  private void ApplyCurrentSorting()
  {
    switch (this._shopAreaSelection.Current)
    {
      case ShopArea.Inventory:
        this._inventoryItemGUIList.Sort((IComparer<BaseItemGUI>) this._inventoryComparer);
        break;
      case ShopArea.Shop:
        this._shopItemGUIList.Sort((IComparer<BaseItemGUI>) this._shopComparer);
        break;
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ShopDragSlot : IDragSlot
  {
    public int Id => (int) this.Slot;

    public IUnityItem Item { get; set; }

    public LoadoutSlotType Slot { get; set; }
  }
}
