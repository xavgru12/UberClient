// Decompiled with JetBrains decompiler
// Type: LuckyDrawWinningPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class LuckyDrawWinningPopup : LotteryWinningPopup
{
  private ShopItemGrid _grid;

  public LuckyDrawWinningPopup(
    string text,
    DynamicTexture image,
    LotteryShopItem item,
    LuckyDrawSetUnityView luckyDrawSet)
    : base(image, item)
  {
    this._grid = new ShopItemGrid(luckyDrawSet.LuckyDrawSetItems, luckyDrawSet.CreditsAttributed, luckyDrawSet.PointsAttributed);
    this.Text = LocalizedStrings.LuckyDrawWinningsInInventory;
  }

  protected override void DrawItemGrid(Rect rect, bool showItems)
  {
    this._grid.Show = showItems;
    this._grid.Draw(rect);
  }
}
