// Decompiled with JetBrains decompiler
// Type: PromotionPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PromotionPopupDialog : BaseEventPopup
{
  private PromotionItemGUI _itemGui;
  private Texture2D _texture;

  public PromotionPopupDialog(WeeklySpecialView weeklySpecial)
    : this(weeklySpecial.Title, weeklySpecial.Text, weeklySpecial.ItemId, weeklySpecial.ImageUrl)
  {
  }

  public PromotionPopupDialog(string title, string text, int itemId, string imageUrl)
  {
    this._itemGui = new PromotionItemGUI(Singleton<ItemManager>.Instance.GetItemInShop(itemId), BuyingLocationType.HomeScreen);
    this._texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
    this.Title = title;
    this.Text = text;
    float num = 0.744186044f;
    this.Width = 700;
    this.Height = Mathf.RoundToInt((float) ((double) this.Width * (double) num * 0.5));
    MonoRoutine.Start(this.DownloadTexture(imageUrl));
    CmuneEventHandler.AddListener<BuyItemEvent>(new Action<BuyItemEvent>(this.OnBuyItemEvent));
  }

  private void OnBuyItemEvent(BuyItemEvent ev) => PopupSystem.HideMessage((IPopupDialog) this);

  public override void OnHide() => CmuneEventHandler.RemoveListener<BuyItemEvent>(new Action<BuyItemEvent>(this.OnBuyItemEvent));

  protected override void DrawGUI(Rect rect)
  {
    GUI.DrawTexture(new Rect(0.0f, 0.0f, rect.width / 2f, rect.height), (Texture) this._texture);
    GUI.Label(new Rect(rect.width / 2f, 20f, rect.width / 2f, 20f), this.Title, BlueStonez.label_interparkbold_16pt);
    GUI.Label(new Rect((float) ((double) rect.width / 2.0 + 15.0), 60f, (float) ((double) rect.width / 2.0 - 30.0), 100f), this.Text, BlueStonez.label_interparkbold_13pt_left);
    this._itemGui.Draw(new Rect((float) ((double) rect.width / 2.0 + 10.0), rect.height - 64f, (float) ((double) rect.width / 2.0 - 20.0), 54f), true);
  }

  [DebuggerHidden]
  private IEnumerator DownloadTexture(string url) => (IEnumerator) new PromotionPopupDialog.\u003CDownloadTexture\u003Ec__Iterator1F()
  {
    url = url,
    \u003C\u0024\u003Eurl = url,
    \u003C\u003Ef__this = this
  };
}
