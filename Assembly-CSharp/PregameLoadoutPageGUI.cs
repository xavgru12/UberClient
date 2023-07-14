// Decompiled with JetBrains decompiler
// Type: PregameLoadoutPageGUI
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
using UnityEngine;

public class PregameLoadoutPageGUI : PageGUI
{
  private WeaponDetailGUI _weaponDetailGui;
  private WeaponRecommendListGUI _weaponRecomGui;
  private static int _itemSlotButtonHash = "Button".GetHashCode();
  private bool _doDragZoom;
  private float _zoomMultiplier = 1f;
  private bool _isDropZoomAnimating;
  private bool _showDragZoomAnimation;
  private bool _dropZoomAnimating;
  private float _alphaValue = 1f;
  private IUnityItem _activeDragItem;
  private bool _activeDragItemEquipped;
  private LoadoutSlotType _activeDragItemLoadoutSlot;
  private int _draggedControlID;
  private Rect _draggedControlRect;
  private LoadoutSlotType _lastSelectedSlot = LoadoutSlotType.Inventory;
  private int _weaponTakenFromSlot = -1;
  private Vector2 _dragScalePivot;
  private Vector2 _dropTargetPositon = Vector2.zero;

  private void Awake()
  {
    this._weaponDetailGui = new WeaponDetailGUI();
    this._weaponRecomGui = new WeaponRecommendListGUI(BuyingLocationType.PreGame);
    this._weaponRecomGui.OnSelectionChange = new Action<IUnityItem, RecommendType>(this._weaponDetailGui.SetWeaponItem);
  }

  private void OnEnable()
  {
    this.OnUpdateRecommendationEvent(new UpdateRecommendationEvent());
    this._weaponRecomGui.Enabled = true;
    CmuneEventHandler.AddListener<UpdateRecommendationEvent>(new Action<UpdateRecommendationEvent>(this.OnUpdateRecommendationEvent));
  }

  private void OnDisable()
  {
    this._weaponRecomGui.Enabled = false;
    CmuneEventHandler.RemoveListener<UpdateRecommendationEvent>(new Action<UpdateRecommendationEvent>(this.OnUpdateRecommendationEvent));
  }

  private void OnUpdateRecommendationEvent(UpdateRecommendationEvent ev)
  {
    List<KeyValuePair<RecommendType, IUnityItem>> recomendations = new List<KeyValuePair<RecommendType, IUnityItem>>(3);
    recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.StaffPick, Singleton<MapManager>.Instance.GetRecommendedItem(GameState.CurrentSpace.SceneName)));
    RecommendationUtils.WeaponRecommendation recommendedWeapon = RecommendationUtils.GetRecommendedWeapon(PlayerDataManager.PlayerLevelSecure, GameState.CurrentSpace.CombatRangeTiers);
    recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.RecommendedWeapon, (IUnityItem) (recommendedWeapon.ItemWeapon ?? RecommendationUtils.FallBackWeapon)));
    recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.RecommendedArmor, (IUnityItem) ShopUtils.GetRecommendedArmor(PlayerDataManager.PlayerLevelSecure, Singleton<LoadoutManager>.Instance.GetItemOnSlot<HoloGearItem>(LoadoutSlotType.GearHolo), Singleton<LoadoutManager>.Instance.GetItemOnSlot<GearItem>(LoadoutSlotType.GearUpperBody), Singleton<LoadoutManager>.Instance.GetItemOnSlot<GearItem>(LoadoutSlotType.GearLowerBody))));
    this._weaponRecomGui.UpdateRecommendedList((IEnumerable<KeyValuePair<RecommendType, IUnityItem>>) recomendations);
  }

  public override void DrawGUI(Rect rect)
  {
    GUI.skin = BlueStonez.Skin;
    GUI.BeginGroup(rect);
    this.DrawPanel(rect);
    GUI.EndGroup();
    this.DoDragControls();
  }

  public void EquipBoughtWeapon(IUnityItem baseItem)
  {
    if (baseItem == null)
      return;
    switch (this._lastSelectedSlot)
    {
      case LoadoutSlotType.WeaponMelee:
      case LoadoutSlotType.WeaponPrimary:
      case LoadoutSlotType.WeaponSecondary:
      case LoadoutSlotType.WeaponTertiary:
        Singleton<LoadoutManager>.Instance.SetSlot(this._lastSelectedSlot, baseItem);
        Singleton<LoadoutManager>.Instance.EquipWeapon(this._lastSelectedSlot, baseItem as WeaponItem);
        break;
      default:
        UnityEngine.Debug.LogError((object) ("Item not equipped because slot type not correct: " + (object) this._lastSelectedSlot));
        break;
    }
  }

  private void DrawPanel(Rect panelRect)
  {
    GUI.BeginGroup(new Rect(1f, 0.0f, panelRect.width - 2f, panelRect.height));
    Rect position1 = new Rect(0.0f, 0.0f, panelRect.width - 2f, 242f);
    GUI.BeginGroup(position1);
    this._weaponDetailGui.Draw(new Rect(0.0f, 0.0f, 200f, position1.height));
    this._weaponRecomGui.Draw(new Rect(199f, 0.0f, (float) ((double) position1.width - 200.0 + 1.0), position1.height));
    GUI.EndGroup();
    Rect position2 = new Rect(0.0f, 241f, panelRect.width - 2f, 167f);
    GUI.BeginGroup(position2);
    this.DrawQuickItemLoadout(new Rect(0.0f, 5f, position2.width * 0.2f, position2.height));
    this.DrawWeaponLoadout(new Rect(position2.width * 0.2f, 0.0f, (float) ((double) position2.width * 0.40000000596046448 + 1.0), position2.height));
    this.DrawGearLoadout(new Rect(position2.width * 0.6f, 0.0f, position2.width * 0.4f, position2.height));
    GUI.EndGroup();
    GUI.EndGroup();
  }

  private void DrawQuickItemLoadout(Rect rect)
  {
    GUI.BeginGroup(rect);
    this.DrawLoadoutItem(LocalizedStrings.QuickItem, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.QuickUseItem1), new Rect((float) ((double) rect.width / 2.0 - 24.0), (float) ((double) rect.height / 2.0 - 80.0), 48f, 48f), LoadoutSlotType.QuickUseItem1, string.Empty, true);
    this.DrawLoadoutItem(LocalizedStrings.QuickItem, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.QuickUseItem2), new Rect((float) ((double) rect.width / 2.0 - 24.0), (float) ((double) rect.height / 2.0 - 24.0), 48f, 48f), LoadoutSlotType.QuickUseItem2, string.Empty, true);
    this.DrawLoadoutItem(LocalizedStrings.QuickItem, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.QuickUseItem3), new Rect((float) ((double) rect.width / 2.0 - 24.0), (float) ((double) rect.height / 2.0 + 32.0), 48f, 48f), LoadoutSlotType.QuickUseItem3, string.Empty, true);
    GUI.EndGroup();
  }

  private void DrawWeaponLoadout(Rect rect)
  {
    GUI.BeginGroup(rect);
    this.DrawGroupControl(new Rect(0.0f, 10f, rect.width, rect.height - 10f), "WEAPONS", BlueStonez.label_group_interparkbold_18pt);
    this.DrawWeaponLoadoutRangeIcon(new Rect((float) ((double) rect.width / 2.0 - 65.0), (float) ((double) rect.height / 2.0 - 85.0), 128f, 128f));
    float num = (float) (((double) rect.width - 192.0 - 24.0) / 3.0);
    this.DrawLoadoutItem(LocalizedStrings.Melee, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponMelee), new Rect(12f, rect.height - 56f, 48f, 48f), LoadoutSlotType.WeaponMelee, AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString(GameInputKey.WeaponMelee), true);
    this.DrawLoadoutItem(LocalizedStrings.PrimaryWeapon, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponPrimary), new Rect(60f + num, rect.height - 56f, 48f, 48f), LoadoutSlotType.WeaponPrimary, AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString(GameInputKey.Weapon1), true);
    this.DrawLoadoutItem(LocalizedStrings.SecondaryWeapon, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponSecondary), new Rect((float) (12.0 + (48.0 + (double) num) * 2.0), rect.height - 56f, 48f, 48f), LoadoutSlotType.WeaponSecondary, AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString(GameInputKey.Weapon2), true);
    this.DrawLoadoutItem(LocalizedStrings.TertiaryWeapon, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponTertiary), new Rect((float) (12.0 + (48.0 + (double) num) * 3.0), rect.height - 56f, 48f, 48f), LoadoutSlotType.WeaponTertiary, AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString(GameInputKey.Weapon3), true);
    GUI.EndGroup();
  }

  private void DrawWeaponLoadoutRangeIcon(Rect rect)
  {
    DrawCombatRangeIconUtil.DrawWeaponRangeIcon2(rect, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponMelee), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponPrimary), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponSecondary), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponTertiary));
    DrawCombatRangeIconUtil.DrawRecommendedCombatRange(rect, GameState.CurrentSpace.CombatRangeTiers, Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponMelee), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponPrimary), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponSecondary), Singleton<LoadoutManager>.Instance.GetItemOnSlot<WeaponItem>(LoadoutSlotType.WeaponTertiary));
  }

  private void DrawGearLoadout(Rect rect)
  {
    GUI.BeginGroup(rect);
    this.DrawGroupControl(new Rect(0.0f, 10f, rect.width, rect.height - 10f), "ARMOR", BlueStonez.label_group_interparkbold_18pt);
    float num = (float) (((double) rect.width - 144.0 - 44.0) / 2.0);
    this.DrawLoadoutItem(LocalizedStrings.UpperBody, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.GearUpperBody), new Rect(22f, rect.height - 56f, 48f, 48f), LoadoutSlotType.GearUpperBody, "UB", false);
    this.DrawLoadoutItem(LocalizedStrings.LowerBody, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.GearLowerBody), new Rect(70f + num, rect.height - 56f, 48f, 48f), LoadoutSlotType.GearLowerBody, "LB", false);
    this.DrawLoadoutItem(LocalizedStrings.UpperBody, Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.GearHolo), new Rect((float) (22.0 + (48.0 + (double) num) * 2.0), rect.height - 56f, 48f, 48f), LoadoutSlotType.GearHolo, "HO", false);
    int armorPoints = 0;
    int absorbtionRatio = 0;
    Singleton<LoadoutManager>.Instance.GetArmorValues(out armorPoints, out absorbtionRatio);
    DrawArmorPowerIconUtil.DrawArmorPower(new Rect((float) ((double) rect.width / 2.0 - 45.0), (float) ((double) rect.height / 2.0 - 70.0), 90f, 90f), armorPoints, absorbtionRatio);
    GUI.EndGroup();
  }

  private void DrawLoadoutItem(
    string slotName,
    InventoryItem item,
    Rect rect,
    LoadoutSlotType loadoutSlotType,
    string slotTag,
    bool supportDrag)
  {
    if (item != null && item.Item != null)
    {
      GUI.Label(new Rect(rect.x, rect.y, 48f, 48f), new GUIContent((Texture) item.Item.Icon), BlueStonez.item_slot_large);
      if (new Rect(rect.x, rect.y, 48f, 48f).Contains(Event.current.mousePosition) && !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen)
        AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(item.Item, new Rect(rect.x, rect.y, 48f, 48f), PopupViewSide.Left);
    }
    else
      GUI.Label(new Rect(rect.x, rect.y, 48f, 48f), new GUIContent(string.Empty, LocalizedStrings.Empty), BlueStonez.item_slot_large);
    if (!string.IsNullOrEmpty(slotTag))
      this.DrawSlotTag(rect, slotTag);
    if (!supportDrag)
      return;
    this.WeaponEquipArea(rect, GUIContent.none, item.Item, loadoutSlotType, BlueStonez.loadoutdropslot);
  }

  private void DrawSlotTag(Rect rect, string slotTag)
  {
    GUI.color = Color.black;
    GUI.Label(new Rect(rect.x + 3f, (float) ((double) rect.y + (double) rect.height - 19.0), rect.width, 18f), slotTag, BlueStonez.label_interparkbold_18pt_left);
    GUI.color = Color.white;
    GUI.Label(new Rect(rect.x + 2f, (float) ((double) rect.y + (double) rect.height - 18.0), rect.width, 18f), slotTag, BlueStonez.label_interparkbold_18pt_left);
  }

  private bool WeaponEquipArea(
    Rect position,
    GUIContent guiContent,
    IUnityItem baseItem,
    LoadoutSlotType loadoutSlotType,
    GUIStyle guiStyle)
  {
    bool flag = false;
    int controlId = GUIUtility.GetControlID(PregameLoadoutPageGUI._itemSlotButtonHash, FocusType.Native);
    switch (Event.current.GetTypeForControl(controlId))
    {
      case UnityEngine.EventType.MouseDown:
        if (position.Contains(Event.current.mousePosition) && !this._isDropZoomAnimating)
        {
          GUIUtility.hotControl = controlId;
          Event.current.Use();
          break;
        }
        break;
      case UnityEngine.EventType.MouseUp:
        if (GUIUtility.hotControl == controlId && !this._isDropZoomAnimating)
        {
          GUIUtility.hotControl = 0;
          Event.current.Use();
          flag = position.Contains(Event.current.mousePosition);
          break;
        }
        if (position.Contains(Event.current.mousePosition) && this._activeDragItem != null)
        {
          if (!this._activeDragItemEquipped)
          {
            if (this._activeDragItemLoadoutSlot == LoadoutSlotType.Inventory)
            {
              switch (loadoutSlotType)
              {
                case LoadoutSlotType.WeaponMelee:
                  if (this._activeDragItem.ItemClass == UberstrikeItemClass.WeaponMelee)
                  {
                    this._lastSelectedSlot = LoadoutSlotType.WeaponMelee;
                    Singleton<LoadoutManager>.Instance.SetSlot(loadoutSlotType, this._activeDragItem);
                    break;
                  }
                  break;
                case LoadoutSlotType.WeaponPrimary:
                  if (this._activeDragItem.ItemType == UberstrikeItemType.Weapon && this._activeDragItem.ItemClass != UberstrikeItemClass.WeaponMelee)
                  {
                    this._lastSelectedSlot = LoadoutSlotType.WeaponPrimary;
                    Singleton<LoadoutManager>.Instance.SetSlot(loadoutSlotType, this._activeDragItem);
                    break;
                  }
                  break;
                case LoadoutSlotType.WeaponSecondary:
                  if (this._activeDragItem.ItemType == UberstrikeItemType.Weapon && this._activeDragItem.ItemClass != UberstrikeItemClass.WeaponMelee)
                  {
                    this._lastSelectedSlot = LoadoutSlotType.WeaponSecondary;
                    Singleton<LoadoutManager>.Instance.SetSlot(loadoutSlotType, this._activeDragItem);
                    break;
                  }
                  break;
                case LoadoutSlotType.WeaponTertiary:
                  if (this._activeDragItem.ItemType == UberstrikeItemType.Weapon && this._activeDragItem.ItemClass != UberstrikeItemClass.WeaponMelee)
                  {
                    this._lastSelectedSlot = LoadoutSlotType.WeaponTertiary;
                    Singleton<LoadoutManager>.Instance.SetSlot(loadoutSlotType, this._activeDragItem);
                    break;
                  }
                  break;
                case LoadoutSlotType.QuickUseItem1:
                case LoadoutSlotType.QuickUseItem2:
                case LoadoutSlotType.QuickUseItem3:
                  if (this._activeDragItem.ItemType == UberstrikeItemType.QuickUse)
                  {
                    Singleton<LoadoutManager>.Instance.SetSlot(loadoutSlotType, this._activeDragItem);
                    break;
                  }
                  break;
              }
            }
            else
              break;
          }
          else
          {
            switch (loadoutSlotType)
            {
              case LoadoutSlotType.WeaponMelee:
                Singleton<LoadoutManager>.Instance.SwitchWeaponsInLoadout(0, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.WeaponPrimary:
                Singleton<LoadoutManager>.Instance.SwitchWeaponsInLoadout(1, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.WeaponSecondary:
                Singleton<LoadoutManager>.Instance.SwitchWeaponsInLoadout(2, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.WeaponTertiary:
                Singleton<LoadoutManager>.Instance.SwitchWeaponsInLoadout(3, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.QuickUseItem1:
                Singleton<LoadoutManager>.Instance.SwitchQuickItemInLoadout(1, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.QuickUseItem2:
                Singleton<LoadoutManager>.Instance.SwitchQuickItemInLoadout(2, this._weaponTakenFromSlot);
                break;
              case LoadoutSlotType.QuickUseItem3:
                Singleton<LoadoutManager>.Instance.SwitchQuickItemInLoadout(3, this._weaponTakenFromSlot);
                break;
            }
            this._weaponTakenFromSlot = -1;
            break;
          }
        }
        else
          break;
        break;
      case UnityEngine.EventType.MouseDrag:
        if (GUIUtility.hotControl == controlId && !this._isDropZoomAnimating)
        {
          this._draggedControlID = GUIUtility.hotControl;
          Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
          this._draggedControlRect = new Rect(screenPoint.x, screenPoint.y, position.width, position.height);
          this._activeDragItem = baseItem;
          this._activeDragItemEquipped = true;
          this._activeDragItemLoadoutSlot = loadoutSlotType;
          if (baseItem != null)
          {
            switch (loadoutSlotType)
            {
              case LoadoutSlotType.WeaponMelee:
                this._weaponTakenFromSlot = 0;
                break;
              case LoadoutSlotType.WeaponPrimary:
                this._weaponTakenFromSlot = 1;
                break;
              case LoadoutSlotType.WeaponSecondary:
                this._weaponTakenFromSlot = 2;
                break;
              case LoadoutSlotType.WeaponTertiary:
                this._weaponTakenFromSlot = 3;
                break;
              case LoadoutSlotType.QuickUseItem1:
                this._weaponTakenFromSlot = 1;
                break;
              case LoadoutSlotType.QuickUseItem2:
                this._weaponTakenFromSlot = 2;
                break;
              case LoadoutSlotType.QuickUseItem3:
                this._weaponTakenFromSlot = 3;
                break;
              default:
                this._weaponTakenFromSlot = -1;
                break;
            }
          }
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        }
        break;
      case UnityEngine.EventType.Repaint:
        guiStyle.Draw(position, guiContent, controlId);
        break;
    }
    return flag;
  }

  private void DrawGroupControl(Rect rect, string title, GUIStyle textStyle)
  {
    GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, textStyle.CalcSize(new GUIContent(title)).x + 10f, 16f), title, textStyle);
  }

  private void DoDragControls()
  {
    if (Event.current.type == UnityEngine.EventType.MouseUp)
    {
      this._draggedControlID = 0;
      this._doDragZoom = false;
      this._activeDragItem = (IUnityItem) null;
      this._activeDragItemEquipped = false;
      this._activeDragItemLoadoutSlot = LoadoutSlotType.Shop;
    }
    if (this._draggedControlID > 0 && this._activeDragItem != null)
    {
      if (!this._doDragZoom)
      {
        this._doDragZoom = true;
        this.StartCoroutine(this.StartDragZoom(0.0f, 1f, 1.25f, 0.1f, 0.8f));
      }
      else
      {
        if (!this._showDragZoomAnimation)
        {
          Vector2 guiPoint = GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, (float) Screen.height - Input.mousePosition.y));
          this._dragScalePivot = new Vector2(guiPoint.x, guiPoint.y);
        }
        GUIUtility.ScaleAroundPivot(new Vector2(this._zoomMultiplier, this._zoomMultiplier), this._dragScalePivot);
        GUI.backgroundColor = new Color(1f, 1f, 1f, this._alphaValue);
        GUI.Label(new Rect(this._dragScalePivot.x - 24f, this._dragScalePivot.y - 24f, 48f, 48f), (Texture) this._activeDragItem.Icon, BlueStonez.item_slot_large);
      }
    }
    else
    {
      if (!this._dropZoomAnimating)
        return;
      GUI.color = new Color(1f, 1f, 1f, this._alphaValue);
      GUI.Label(new Rect(this._draggedControlRect.xMin, this._draggedControlRect.yMin, 48f, 48f), (Texture) this._activeDragItem.Icon, BlueStonez.item_slot_large);
      GUIUtility.ScaleAroundPivot(new Vector2(this._zoomMultiplier, this._zoomMultiplier), new Vector2(this._dropTargetPositon.x + 32f, this._dropTargetPositon.y + 32f));
      GUI.Label(new Rect(this._dropTargetPositon.x, this._dropTargetPositon.y, 48f, 48f), (Texture) this._activeDragItem.Icon, BlueStonez.item_slot_large);
    }
  }

  [DebuggerHidden]
  private IEnumerator StartDragZoom(
    float startTime,
    float startZoom,
    float endZoom,
    float startAlpha,
    float endAlpha)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new PregameLoadoutPageGUI.\u003CStartDragZoom\u003Ec__Iterator18()
    {
      startTime = startTime,
      startAlpha = startAlpha,
      endAlpha = endAlpha,
      startZoom = startZoom,
      endZoom = endZoom,
      \u003C\u0024\u003EstartTime = startTime,
      \u003C\u0024\u003EstartAlpha = startAlpha,
      \u003C\u0024\u003EendAlpha = endAlpha,
      \u003C\u0024\u003EstartZoom = startZoom,
      \u003C\u0024\u003EendZoom = endZoom,
      \u003C\u003Ef__this = this
    };
  }

  private string ModeName => GameModes.GetModeName(GameState.CurrentGameMode);
}
