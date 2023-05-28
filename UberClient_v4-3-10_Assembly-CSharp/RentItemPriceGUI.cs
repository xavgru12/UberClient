// Decompiled with JetBrains decompiler
// Type: RentItemPriceGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class RentItemPriceGUI : ItemPriceGUI
{
  private List<ItemPrice> _prices;

  public RentItemPriceGUI(IUnityItem item, Action<ItemPrice> onPriceSelected)
    : base(item.ItemView.LevelLock, onPriceSelected)
  {
    this._prices = new List<ItemPrice>((IEnumerable<ItemPrice>) item.ItemView.Prices);
    if (this._prices.Count <= 0)
      return;
    this._onPriceSelected(this._prices[this._prices.Count - 1]);
  }

  public override void Draw(Rect rect)
  {
    GUI.BeginGroup(rect);
    int num = 30;
    if (this._prices.Exists((Predicate<ItemPrice>) (p => p.Duration != BuyingDurationType.Permanent)))
    {
      GUI.Label(new Rect(0.0f, 0.0f, rect.width, 16f), "Limited Use", BlueStonez.label_interparkbold_16pt_left);
      foreach (ItemPrice price in this._prices)
      {
        if (price.Duration != BuyingDurationType.Permanent)
        {
          GUIContent content = new GUIContent(ShopUtils.PrintDuration(price.Duration));
          if (price.Currency == UberStrikeCurrencyType.Points && this._levelLocked)
          {
            GUI.enabled = false;
            content.tooltip = this._tooltip;
          }
          if (GUI.Toggle(new Rect(0.0f, (float) num, rect.width, 20f), this._selectedPrice == price, content, BlueStonez.toggle) && price != this._selectedPrice)
            this._onPriceSelected(price);
          num = this.DrawPrice(price, rect.width * 0.5f, num);
          GUI.enabled = true;
        }
      }
      num += 20;
    }
    if (this._prices.Exists((Predicate<ItemPrice>) (p => p.Duration == BuyingDurationType.Permanent)))
    {
      GUI.Label(new Rect(0.0f, (float) num, rect.width, 16f), "Unlimited Use", BlueStonez.label_interparkbold_16pt_left);
      num += 30;
      foreach (ItemPrice price in this._prices)
      {
        if (price.Duration == BuyingDurationType.Permanent)
        {
          string empty = string.Empty;
          if (GUI.Toggle(new Rect(0.0f, (float) num, rect.width, 20f), this._selectedPrice == price, new GUIContent(LocalizedStrings.Permanent, empty), BlueStonez.toggle) && price != this._selectedPrice)
            this._onPriceSelected(price);
          num = this.DrawPrice(price, rect.width * 0.5f, num);
        }
      }
    }
    this.Height = num;
    GUI.EndGroup();
  }

  private string GetRentDuration(BuyingDurationType duration)
  {
    string rentDuration = string.Empty;
    switch (duration)
    {
      case BuyingDurationType.OneDay:
        rentDuration = LocalizedStrings.OneDay;
        break;
      case BuyingDurationType.SevenDays:
        rentDuration = LocalizedStrings.SevenDays;
        break;
    }
    return rentDuration;
  }
}
