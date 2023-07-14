// Decompiled with JetBrains decompiler
// Type: ShopItemGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGrid
{
  protected const int MAX_COLUMN = 6;
  protected List<ShopItemView> _items;
  private float offset = -300f;

  public ShopItemGrid(List<BundleItemView> items, int credits = 0, int points = 0)
  {
    this._items = new List<ShopItemView>(items.Count + 2);
    foreach (BundleItemView itemView in items)
      this._items.Add(new ShopItemView(itemView));
    if (credits > 0)
      this._items.Add(new ShopItemView(UberStrikeCurrencyType.Credits, credits));
    if (points <= 0)
      return;
    this._items.Add(new ShopItemView(UberStrikeCurrencyType.Points, points));
  }

  public ShopItemGrid(List<ShopItemView> items, int credits = 0, int points = 0)
  {
    this._items = items;
    if (credits > 0)
      this._items.Add(new ShopItemView(UberStrikeCurrencyType.Credits, credits));
    if (points <= 0)
      return;
    this._items.Add(new ShopItemView(UberStrikeCurrencyType.Points, points));
  }

  public List<bool> HighlightState { get; set; }

  public bool Show { get; set; }

  public List<ShopItemView> Items => this._items;

  public void Draw(Rect rect)
  {
    float num1 = rect.width / 6f;
    int num2 = this._items.Count / 6 + (this._items.Count % 6 <= 0 ? 0 : 1);
    this.offset = !this.Show ? Mathf.Lerp(this.offset, (float) -num2 * num1, Time.deltaTime * 5f) : Mathf.Lerp(this.offset, 0.0f, Time.deltaTime * 5f);
    GUI.BeginGroup(rect);
    for (int index1 = 0; index1 < num2; ++index1)
    {
      for (int index2 = 0; index2 < 6; ++index2)
      {
        int index3 = index1 * 6 + index2;
        Rect rect1 = new Rect((float) index2 * num1, rect.height - (float) (index1 + 1) * num1 - this.offset, num1, num1);
        if (index3 < this._items.Count)
        {
          if (this.HighlightState != null)
          {
            if (this.HighlightState[index3])
            {
              GUI.Button(rect1, this._items[index3].Icon, BlueStonez.item_slot_small);
              GUI.color = GUI.color.SetAlpha(GUITools.FastSinusPulse);
              GUI.DrawTexture(rect1, (Texture) ShopIcons.ItemSlotSelected);
              GUI.color = Color.white;
              if ((Object) AutoMonoBehaviour<ItemToolTip>.Instance != (Object) null && this.Show && (rect1.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile) && (double) this.offset < (double) num1)
                AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(this._items[index3].UnityItem, rect1, PopupViewSide.Top, duration: this._items[index3].Duration);
            }
            else
            {
              GUITools.PushGUIState();
              GUI.enabled = false;
              GUI.Label(rect1, this._items[index3].Icon, BlueStonez.item_slot_alpha);
              GUITools.PopGUIState();
            }
          }
          else
          {
            GUI.Button(rect1, this._items[index3].Icon, BlueStonez.item_slot_alpha);
            if ((Object) AutoMonoBehaviour<ItemToolTip>.Instance != (Object) null && this.Show && rect1.Contains(Event.current.mousePosition) && (double) this.offset < (double) num1)
              AutoMonoBehaviour<ItemToolTip>.Instance.SetItem(this._items[index3].UnityItem, rect1, PopupViewSide.Top, duration: this._items[index3].Duration);
          }
        }
        else
        {
          GUITools.PushGUIState();
          GUI.enabled = false;
          GUI.Label(rect1, GUIContent.none, BlueStonez.item_slot_alpha);
          GUITools.PopGUIState();
        }
      }
    }
    GUI.EndGroup();
  }
}
