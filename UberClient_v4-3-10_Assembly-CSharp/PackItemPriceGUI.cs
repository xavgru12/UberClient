// Decompiled with JetBrains decompiler
// Type: PackItemPriceGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class PackItemPriceGUI : ItemPriceGUI
{
  private List<ItemPrice> _prices;

  public PackItemPriceGUI(IUnityItem item, Action<ItemPrice> onPriceSelected)
    : base(item.ItemView.LevelLock, onPriceSelected)
  {
    this._prices = new List<ItemPrice>((IEnumerable<ItemPrice>) item.ItemView.Prices);
    if (this._prices.Count > 1)
      this._onPriceSelected(this._prices[1]);
    else
      this._onPriceSelected(this._prices[0]);
  }

  public override void Draw(Rect rect)
  {
    GUI.BeginGroup(rect);
    int num = 30;
    GUI.Label(new Rect(0.0f, 0.0f, rect.width, 16f), "Purchase Options", BlueStonez.label_interparkbold_16pt_left);
    foreach (ItemPrice price in this._prices)
    {
      GUIContent content = new GUIContent(price.Amount.ToString() + " Uses");
      if (this._levelLocked && price.Currency == UberStrikeCurrencyType.Points)
      {
        GUI.enabled = false;
        content.tooltip = this._tooltip;
      }
      if (GUI.Toggle(new Rect(0.0f, (float) num, rect.width, 20f), this._selectedPrice == price, content, BlueStonez.toggle) && price != this._selectedPrice)
        this._onPriceSelected(price);
      num = this.DrawPrice(price, rect.width * 0.5f, num);
      GUI.enabled = true;
    }
    this.Height = num;
    GUI.EndGroup();
  }
}
