// Decompiled with JetBrains decompiler
// Type: ItemListPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ItemListPopupDialog : BasePopupDialog
{
  private List<IUnityItem> _items;

  private ItemListPopupDialog()
  {
    this._cancelCaption = LocalizedStrings.OkCaps;
    this._alertType = PopupSystem.AlertType.Cancel;
  }

  public ItemListPopupDialog(string title, string text, List<IUnityItem> items, ShopArea area)
    : this()
  {
    this.Title = title;
    this.Text = text;
    this._size.y = 320f;
    this._items = new List<IUnityItem>((IEnumerable<IUnityItem>) items);
    foreach (IUnityItem unityItem in this._items)
    {
      if (unityItem != null)
        Singleton<InventoryManager>.Instance.HighlightItem(unityItem.ItemId, true);
    }
    if (items.Count <= 1)
      return;
    this._alertType = PopupSystem.AlertType.OK;
    this._actionType = PopupSystem.ActionType.Positive;
    this._okCaption = LocalizedStrings.OkCaps;
  }

  public ItemListPopupDialog(IUnityItem item)
    : this()
  {
    this.Title = LocalizedStrings.NewItem;
    if (item != null)
    {
      this._items = new List<IUnityItem>() { item };
      foreach (IUnityItem unityItem in this._items)
      {
        if (unityItem != null)
          Singleton<InventoryManager>.Instance.HighlightItem(unityItem.ItemId, true);
      }
      if (item.ItemType != UberstrikeItemType.Gear && item.ItemType != UberstrikeItemType.Weapon && item.ItemType != UberstrikeItemType.QuickUse)
        return;
      this._alertType = PopupSystem.AlertType.OKCancel;
      this._actionType = PopupSystem.ActionType.Positive;
      this._okCaption = LocalizedStrings.Equip;
      this._cancelCaption = LocalizedStrings.NotNow;
      this._callbackOk = (Action) (() =>
      {
        IUnityItem unityItem = this._items[0];
        if (unityItem == null)
          return;
        Singleton<InventoryManager>.Instance.EquipItem(unityItem.ItemId);
        CmuneEventHandler.Route((object) new UpdateRecommendationEvent());
      });
      this._callbackCancel = (Action) (() => CmuneEventHandler.Route((object) new UpdateRecommendationEvent()));
    }
    else
      this._items = new List<IUnityItem>();
  }

  protected override void DrawPopupWindow()
  {
    if (this._items.Count == 0)
      GUI.Label(new Rect(17f, 115f, this._size.x - 34f, 20f), "There are no items", BlueStonez.label_interparkbold_13pt);
    else if (this._items.Count == 1)
    {
      if (this._items[0] == null)
        return;
      GUI.Label(new Rect((float) ((double) this._size.x * 0.5 - 32.0), 55f, 64f, 64f), (Texture) this._items[0].Icon);
      GUI.Label(new Rect(17f, 115f, this._size.x - 34f, 20f), this._items[0].Name, BlueStonez.label_interparkbold_13pt);
      if (this._items[0].ItemView == null)
        return;
      string text = this._items[0].ItemView.Description;
      if (string.IsNullOrEmpty(text))
        text = "No description available.";
      GUI.Label(new Rect(17f, 140f, this._size.x - 34f, 40f), text, BlueStonez.label_interparkmed_11pt);
    }
    else if (this._items.Count <= 4)
      this.DrawItemsInColumns(2);
    else if (this._items.Count <= 6)
      this.DrawItemsInColumns(3);
    else if (this._items.Count <= 8)
      this.DrawItemsInColumns(4);
    else
      GUI.Label(new Rect(17f, 150f, this._size.x - 34f, 20f), this.Text, BlueStonez.label_interparkbold_13pt);
  }

  private void DrawItemsInColumns(int columns)
  {
    int num1 = 0;
    int num2 = 0;
    float num3 = (float) ((double) this._size.x * 0.5 - (double) (64 * columns) / 2.0 - (double) (15 * (columns - 1)) / 2.0);
    foreach (IUnityItem unityItem in this._items)
    {
      if (unityItem != null)
      {
        GUI.Label(new Rect(num3 + (float) (num1 % columns * 79), (float) (55 + num2 * 70), 64f, 64f), (Texture) unityItem.Icon, BlueStonez.label_interparkbold_11pt);
        GUI.Label(new Rect((float) ((double) num3 + (double) (num1 % columns * 79) - 7.0), (float) (110 + num2 * 70), 79f, 20f), unityItem.Name, BlueStonez.label_interparkmed_11pt);
      }
      ++num1;
      num2 = num1 / columns;
    }
    GUI.Label(new Rect(17f, 220f, this._size.x - 34f, 40f), this.Text, BlueStonez.label_interparkmed_11pt);
  }
}
