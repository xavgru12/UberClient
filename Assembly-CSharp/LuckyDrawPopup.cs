// Decompiled with JetBrains decompiler
// Type: LuckyDrawPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.WebService.Unity;
using UnityEngine;

public class LuckyDrawPopup : LotteryPopupDialog
{
  private const int spacing = 20;
  private const int offset = 30;
  private LuckyDrawShopItem luckyDraw;
  private List<ShopItemGrid> _itemGrids;
  private Vector2Anim scroll = new Vector2Anim();
  private int _luckyDrawResult = -1;

  public LuckyDrawPopup(LuckyDrawShopItem luckyDraw)
  {
    this.luckyDraw = luckyDraw;
    this.Title = luckyDraw.Name;
    this.Text = luckyDraw.View.Description;
    this.Width = 60 + luckyDraw.Sets.Count * 308 - 20;
    this.Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
    this.ShowNavigationArrows = true;
    this.HelpText = LocalizedStrings.LuckyDrawHelpText;
    this._itemGrids = new List<ShopItemGrid>(luckyDraw.Sets.Count);
    foreach (LuckyDrawSetUnityView luckyDrawSet in luckyDraw.View.LuckyDrawSets)
    {
      Debug.Log((object) ("Box: " + (object) luckyDrawSet.Id + "Items: " + (object) luckyDrawSet.LuckyDrawSetItems.Count + " Creds: " + (object) luckyDrawSet.CreditsAttributed + " Points: " + (object) luckyDrawSet.PointsAttributed));
      this._itemGrids.Add(new ShopItemGrid(luckyDrawSet.LuckyDrawSetItems, luckyDrawSet.CreditsAttributed, luckyDrawSet.PointsAttributed));
    }
    this._showExitButton = luckyDraw.View.Category != BundleCategoryType.Login && luckyDraw.View.Category != BundleCategoryType.Signup;
    this.IsVisible = true;
    if (!this._showExitButton)
      return;
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.MysteryBoxMusic);
  }

  public bool ShowNavigationArrows { get; set; }

  public string HelpText { get; set; }

  protected override void DrawPlayGUI(Rect rect)
  {
    this.Width = 60 + this.luckyDraw.Sets.Count * 308 - 20;
    GUI.color = ColorScheme.HudTeamBlue;
    float width1 = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(this.Title)).x * 2.5f;
    GUI.DrawTexture(new Rect((float) (((double) rect.width - (double) width1) * 0.5), -29f, width1, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 10f, rect.width, 30f), this.Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
    GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), this.Text, BlueStonez.label_interparkbold_16pt);
    int index = 0;
    int width2 = 288;
    int left = 30;
    int height = 323;
    foreach (LuckyDrawShopItem.LuckyDrawSet set in this.luckyDraw.Sets)
    {
      GUI.BeginGroup(new Rect((float) left, 75f, (float) width2, (float) height), BlueStonez.item_slot_large);
      Rect rect1 = new Rect((float) ((width2 - 282) / 2), (float) ((height - 317) / 2), 282f, 317f);
      set.Image.Draw(rect1);
      this._itemGrids[index].Show = (rect1.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile) && !this.IsUIDisabled;
      if (set.View.ExposeItemsToPlayers)
        this._itemGrids[index++].Draw(new Rect(0.0f, 0.0f, (float) width2, (float) height));
      left += width2 + 20;
      GUI.EndGroup();
    }
    if (this.luckyDraw.Price.Price > 0)
    {
      if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 70.0), rect.height - 47f, 140f, 30f), this.luckyDraw.Price.PriceTag(tooltip: string.Empty), BlueStonez.buttongold_large_price))
        this.Play();
    }
    else if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 70.0), rect.height - 47f, 140f, 30f), LocalizedStrings.PlayCaps, BlueStonez.buttongold_large))
      this.Play();
    if (!this.ShowNavigationArrows)
      return;
    this.DrawNaviArrows(rect, (LotteryShopItem) this.luckyDraw);
  }

  public override void OnAfterGUI() => this.scroll.Update();

  public override LotteryWinningPopup ShowReward()
  {
    LuckyDrawShopItem.LuckyDrawSet luckyDrawSet = this.luckyDraw.Sets.Find((Predicate<LuckyDrawShopItem.LuckyDrawSet>) (s => s.Id == this._luckyDrawResult));
    return luckyDrawSet != null ? (LotteryWinningPopup) new LuckyDrawWinningPopup(this.Text, luckyDrawSet.Image, (LotteryShopItem) this.luckyDraw, luckyDrawSet.View) : (LotteryWinningPopup) null;
  }

  private void Play()
  {
    if (this.luckyDraw.Price.Currency == UberStrikeCurrencyType.Credits && this.luckyDraw.Price.Price > PlayerDataManager.CreditsSecure)
      PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouNeedMoreCreditsToBuyThisItem, PopupSystem.AlertType.OKCancel, new Action(ApplicationDataManager.OpenBuyCredits), LocalizedStrings.BuyCreditsCaps, (Action) null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Positive);
    else if (this.luckyDraw.Price.Currency == UberStrikeCurrencyType.Points && this.luckyDraw.Price.Price > PlayerDataManager.PointsSecure)
      PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouNeedToEarnMorePointsToBuyThisItem, PopupSystem.AlertType.OK, LocalizedStrings.OkCaps, (Action) null);
    else
      this.RollLuckyDraw();
  }

  private void RollLuckyDraw()
  {
    if (this._onLotteryRolled != null)
      this._onLotteryRolled();
    ShopWebServiceClient.RollLuckyDraw(PlayerDataManager.CmidSecure, this.luckyDraw.View.Id, ApplicationDataManager.Channel, new Action<int>(this.OnLuckyDrawReturn), (Action<Exception>) (ex =>
    {
      this.ReturnedState = LotteryPopupDialog.MyState.Failed;
      Debug.LogError((object) ("ERROR IN StartPlaying: " + ex.Message));
      PopupSystem.ShowMessage("Server Error", "There was a problem. Please check your internet connection and try again.");
    }));
  }

  private void OnLuckyDrawReturn(int result)
  {
    this.ReturnedState = LotteryPopupDialog.MyState.Success;
    this._luckyDrawResult = result;
  }
}
