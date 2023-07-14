// Decompiled with JetBrains decompiler
// Type: ItemPriceGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Core.Models.Views;
using UnityEngine;

public abstract class ItemPriceGUI
{
  protected bool _levelLocked;
  protected string _tooltip = string.Empty;
  protected ItemPrice _selectedPrice;
  protected Action<ItemPrice> _onPriceSelected;

  public ItemPriceGUI(int levelLock, Action<ItemPrice> onPriceSelected)
  {
    if (levelLock > PlayerDataManager.PlayerLevel)
    {
      this._levelLocked = true;
      this._tooltip = string.Format("Not so fast, squirt!\n\nYou need to be Level {0} to buy this item using points.\n\nGet fragging!", (object) levelLock);
    }
    this._onPriceSelected = (Action<ItemPrice>) (price => this._selectedPrice = price);
    this._onPriceSelected += onPriceSelected;
  }

  public int Height { get; protected set; }

  public ItemPrice SelectedPriceOption => this._selectedPrice;

  public abstract void Draw(Rect rect);

  protected int DrawPrice(ItemPrice price, float width, int y)
  {
    GUIContent content = new GUIContent(price.Price <= 0 ? " FREE" : string.Format(" {0:N0}", (object) price.Price), price.Currency != UberStrikeCurrencyType.Points ? (Texture) ShopIcons.IconCredits20x20 : (Texture) ShopIcons.IconPoints20x20);
    GUI.Label(new Rect(width, (float) y, width, 20f), content, BlueStonez.label_itemdescription);
    if (price.Price > 0 && price.Discount > 0)
    {
      string text = string.Format(LocalizedStrings.DiscountPercentOff, (object) price.Discount);
      GUI.color = ColorScheme.UberStrikeYellow;
      GUI.Label(new Rect(width + 80f, (float) (y + 5), width, 20f), text, BlueStonez.label_itemdescription);
      GUI.color = Color.white;
    }
    return y += 24;
  }
}
