// Decompiled with JetBrains decompiler
// Type: WeaponRecommendListGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class WeaponRecommendListGUI
{
  private bool _enabled;
  private GUIStyle _selectionStyle;
  private GUIStyle _normalStyle;
  private List<KeyValuePair<RecommendType, BaseItemGUI>> _recommendedItemList;
  private IUnityItem _selectedItem;
  private BuyingLocationType _location;

  public WeaponRecommendListGUI(BuyingLocationType location)
  {
    this._recommendedItemList = new List<KeyValuePair<RecommendType, BaseItemGUI>>();
    this._location = location;
  }

  public bool Enabled
  {
    get => this._enabled;
    set
    {
      if (value == this._enabled)
        return;
      this._enabled = value;
      if (value)
      {
        if (this._selectedItem == null && this._recommendedItemList.Count > 0)
          this.SetSelection(this._recommendedItemList[0].Value.Item);
        if (this._selectionStyle == null)
        {
          this._selectionStyle = new GUIStyle(StormFront.GrayPanelBox);
          this._selectionStyle.overflow.left = 6;
        }
        if (this._normalStyle == null)
        {
          this._normalStyle = new GUIStyle(StormFront.GrayPanelBlankBox);
          this._normalStyle.overflow.left = 5;
        }
        CmuneEventHandler.AddListener<SelectShopItemEvent>(new Action<SelectShopItemEvent>(this.OnSelectItem));
      }
      else
      {
        CmuneEventHandler.RemoveListener<SelectShopItemEvent>(new Action<SelectShopItemEvent>(this.OnSelectItem));
        this.ClearSelection();
      }
    }
  }

  public Action<IUnityItem, RecommendType> OnSelectionChange { get; set; }

  public IUnityItem SelectedItem => this._selectedItem;

  public void ClearSelection() => this.SetSelection((IUnityItem) null);

  public void Draw(Rect rect)
  {
    if (!this.Enabled)
      return;
    this.DrawRecommendList(rect);
  }

  public void UpdateRecommendedList(
    IEnumerable<KeyValuePair<RecommendType, IUnityItem>> recomendations)
  {
    this._recommendedItemList.Clear();
    foreach (KeyValuePair<RecommendType, IUnityItem> recomendation in recomendations)
    {
      try
      {
        this._recommendedItemList.Add(new KeyValuePair<RecommendType, BaseItemGUI>(recomendation.Key, (BaseItemGUI) new InGameItemGUI(recomendation.Value, ShopUtils.GetRecommendationString(recomendation.Key), this._location, recomendation.Key != RecommendType.StaffPick ? BuyingRecommendationType.Behavior : BuyingRecommendationType.Manual)));
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Couldn't add item to recommendation list, it was null.\n\n" + ex.Message));
      }
    }
  }

  private void OnSelectItem(SelectShopItemEvent ev)
  {
    if (this._selectedItem == ev.Item)
      return;
    this.SetSelection(ev.Item);
  }

  private void SetSelection(IUnityItem item)
  {
    this._selectedItem = item;
    foreach (KeyValuePair<RecommendType, BaseItemGUI> recommendedItem in this._recommendedItemList)
    {
      if (recommendedItem.Value.Item == this._selectedItem && this.OnSelectionChange != null)
      {
        this.OnSelectionChange(this._selectedItem, recommendedItem.Key);
        break;
      }
    }
  }

  private void DrawRecommendList(Rect rect)
  {
    if (this._recommendedItemList.Count <= 0)
    {
      GUI.Label(rect, "Nothing to recommend", BlueStonez.label_interparkbold_11pt);
    }
    else
    {
      GUI.BeginGroup(rect);
      float height = rect.height / (float) this._recommendedItemList.Count;
      Rect rect1 = new Rect(5f, 0.0f, rect.width - 10f, height);
      for (int index = 0; index < this._recommendedItemList.Count; ++index)
      {
        if (this._selectedItem == this._recommendedItemList[index].Value.Item)
          GUI.Label(new Rect(rect1.x, rect1.y, rect.width - 5f, height), GUIContent.none, this._selectionStyle);
        else
          GUI.Label(new Rect(rect1.x, rect1.y, rect.width - 5f, height), GUIContent.none, this._normalStyle);
        this._recommendedItemList[index].Value.Draw(rect1, false);
        rect1.y += height - 1f;
      }
      GUI.EndGroup();
    }
  }
}
