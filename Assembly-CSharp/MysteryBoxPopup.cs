// Decompiled with JetBrains decompiler
// Type: MysteryBoxPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.WebService.Unity;
using UnityEngine;

public class MysteryBoxPopup : LotteryPopupDialog
{
  private MysteryBoxShopItem mysteryBox;
  private Vector2Anim scroll = new Vector2Anim();
  private ShopItemGrid _lotteryItemGrid;
  private List<bool> _rewardHighlight;

  public MysteryBoxPopup(MysteryBoxShopItem mysteryBox)
  {
    this.mysteryBox = mysteryBox;
    this.Title = mysteryBox.Name;
    this.Text = mysteryBox.View.Description;
    this.Width = 388;
    this.Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
    this._lotteryItemGrid = new ShopItemGrid(mysteryBox.View.MysteryBoxItems, mysteryBox.View.CreditsAttributed, mysteryBox.View.PointsAttributed);
    this.IsVisible = true;
    if (mysteryBox.Category == BundleCategoryType.Login || mysteryBox.Category == BundleCategoryType.Signup)
      return;
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.MysteryBoxMusic);
  }

  protected override void DrawPlayGUI(Rect rect)
  {
    GUI.color = ColorScheme.HudTeamBlue;
    float width1 = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(this.Title)).x * 2.5f;
    GUI.DrawTexture(new Rect((float) (((double) rect.width - (double) width1) * 0.5), -29f, width1, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 10f, rect.width, 30f), this.Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
    GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), this.Text, BlueStonez.label_interparkbold_16pt);
    int width2 = 288;
    int left = (this.Width - width2 - 6) / 2;
    int height = 323;
    GUI.BeginGroup(new Rect((float) left, 75f, (float) width2, (float) height), BlueStonez.item_slot_large);
    Rect rect1 = new Rect((float) ((width2 - 282) / 2), (float) ((height - 317) / 2), 282f, 317f);
    this.mysteryBox.Image.Draw(rect1);
    this._lotteryItemGrid.Show = (rect1.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile) && !this.IsUIDisabled;
    if (this.mysteryBox.View.ExposeItemsToPlayers)
      this._lotteryItemGrid.Draw(new Rect(0.0f, 0.0f, (float) width2, (float) height));
    GUI.EndGroup();
    if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 70.0), rect.height - 47f, 140f, 30f), this.mysteryBox.Price.PriceTag(tooltip: string.Empty), BlueStonez.buttongold_large_price))
      this.Play();
    this.DrawNaviArrows(rect, (LotteryShopItem) this.mysteryBox);
  }

  public override void OnAfterGUI() => this.scroll.Update();

  public override LotteryWinningPopup ShowReward() => (LotteryWinningPopup) new MysteryBoxWinningPopup(this.mysteryBox.Image, this.mysteryBox, this._rewardHighlight);

  private void Play()
  {
    if (this.mysteryBox.Price.Currency == UberStrikeCurrencyType.Credits && this.mysteryBox.Price.Price > PlayerDataManager.CreditsSecure)
      PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouNeedMoreCreditsToBuyThisItem, PopupSystem.AlertType.OKCancel, new Action(((LotteryPopupDialog) this).OpenGetCredits), "GET CREDITS", (Action) null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Positive);
    else if (this.mysteryBox.Price.Currency == UberStrikeCurrencyType.Points && this.mysteryBox.Price.Price > PlayerDataManager.PointsSecure)
      PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouNeedToEarnMorePointsToBuyThisItem, PopupSystem.AlertType.OK, LocalizedStrings.OkCaps, (Action) null);
    else
      this.RollMysteryBox();
  }

  private void RollMysteryBox()
  {
    if (this._onLotteryRolled != null)
      this._onLotteryRolled();
    ShopWebServiceClient.RollMysteryBox(PlayerDataManager.CmidSecure, this.mysteryBox.View.Id, ApplicationDataManager.Channel, new Action<List<MysteryBoxWonItemUnityView>>(this.OnMysteryBoxReturned), (Action<Exception>) (ex =>
    {
      this.ReturnedState = LotteryPopupDialog.MyState.Failed;
      Debug.LogError((object) ("ERROR IN StartPlaying: " + ex.Message));
      PopupSystem.ShowMessage("Server Error", "There was a problem. Please check your internet connection and try again.");
    }));
  }

  private void OnMysteryBoxReturned(List<MysteryBoxWonItemUnityView> items)
  {
    this.ReturnedState = LotteryPopupDialog.MyState.Success;
    this._rewardHighlight = new List<bool>(this._lotteryItemGrid.Items.Count);
    for (int index = 0; index < this._lotteryItemGrid.Items.Count; ++index)
      this._rewardHighlight.Add(false);
    foreach (MysteryBoxWonItemUnityView wonItemUnityView in items)
    {
      MysteryBoxWonItemUnityView view = wonItemUnityView;
      int index = this._lotteryItemGrid.Items.FindIndex((Predicate<ShopItemView>) (t =>
      {
        if (t.ItemId > 0 && t.ItemId == view.ItemIdWon)
          return true;
        if (t.ItemId != 0)
          return false;
        return t.Credits == view.CreditWon || t.Points == view.PointWon;
      }));
      if (index >= 0)
        this._rewardHighlight[index] = true;
    }
  }
}
