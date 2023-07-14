// Decompiled with JetBrains decompiler
// Type: MysteryBoxWinningPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxWinningPopup : LotteryWinningPopup
{
  private ShopItemGrid _grid;

  public MysteryBoxWinningPopup(
    DynamicTexture image,
    MysteryBoxShopItem item,
    List<bool> highlight)
    : base(image, (LotteryShopItem) item)
  {
    this._grid = new ShopItemGrid(item.View.MysteryBoxItems, item.View.CreditsAttributed, item.View.PointsAttributed);
    this._grid.HighlightState = highlight;
    this.Text = "You find your winnings in the inventory!";
  }

  protected override void DrawItemGrid(Rect rect, bool showItems)
  {
    this._grid.Show = showItems;
    this._grid.Draw(rect);
  }
}
