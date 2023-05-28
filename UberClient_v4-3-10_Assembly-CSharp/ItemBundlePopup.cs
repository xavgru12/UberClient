// Decompiled with JetBrains decompiler
// Type: ItemBundlePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class ItemBundlePopup : LotteryPopupDialog
{
  private BundleUnityView _bundleUnityView;
  private ShopItemGrid _lotteryItemGrid;

  public ItemBundlePopup(BundleUnityView bundleUnityView)
  {
    this._bundleUnityView = bundleUnityView;
    this.Title = bundleUnityView.BundleView.Name;
    this.Text = bundleUnityView.BundleView.Description;
    this.Width = 388;
    this.Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
    this._lotteryItemGrid = new ShopItemGrid(bundleUnityView.BundleView.BundleItemViews);
  }

  protected override void DrawPlayGUI(Rect rect)
  {
    GUI.color = ColorScheme.HudTeamBlue;
    float width1 = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(this.Title)).x * 2.5f;
    GUI.DrawTexture(new Rect((float) (((double) rect.width - (double) width1) * 0.5), -29f, width1, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 10f, rect.width, 30f), this.Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
    GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), this.Text, BlueStonez.label_interparkbold_13pt);
    int width2 = 288;
    int left = (this.Width - width2 - 6) / 2;
    int height = 323;
    GUI.BeginGroup(new Rect((float) left, 75f, (float) width2, (float) height), BlueStonez.item_slot_large);
    Rect rect1 = new Rect((float) ((width2 - 282) / 2), (float) ((height - 317) / 2), 282f, 317f);
    this._bundleUnityView.Image.Draw(rect1);
    this._lotteryItemGrid.Show = rect1.Contains(Event.current.mousePosition) || ApplicationDataManager.IsMobile;
    this._lotteryItemGrid.Draw(new Rect(0.0f, 0.0f, (float) width2, (float) height));
    GUI.EndGroup();
    if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 95.0), rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_left))
    {
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
      PopupSystem.HideMessage((IPopupDialog) this);
      BundleUnityView previousItem = Singleton<BundleManager>.Instance.GetPreviousItem(this._bundleUnityView);
      if (previousItem != null)
        PopupSystem.Show((IPopupDialog) new ItemBundlePopup(previousItem));
    }
    if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 + 75.0), rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_right))
    {
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
      PopupSystem.HideMessage((IPopupDialog) this);
      BundleUnityView nextItem = Singleton<BundleManager>.Instance.GetNextItem(this._bundleUnityView);
      if (nextItem != null)
        PopupSystem.Show((IPopupDialog) new ItemBundlePopup(nextItem));
    }
    GUI.enabled = !this._bundleUnityView.IsOwned && this._bundleUnityView.IsValid && GUITools.SaveClickIn(1f);
    this.BuyButton(rect, this._bundleUnityView);
    GUI.enabled = true;
  }

  private void BuyButton(Rect position, BundleUnityView bundleUnityView)
  {
    ChannelType channel = ApplicationDataManager.Channel;
    switch (channel)
    {
      case ChannelType.MacAppStore:
      case ChannelType.IPad:
      case ChannelType.Android:
        this.BuyStoreKitButton(position, bundleUnityView);
        break;
      default:
        if (channel != ChannelType.WebFacebook)
          break;
        this.BuyFBCButton(position, bundleUnityView);
        break;
    }
  }

  private void BuyFBCButton(Rect position, BundleUnityView bundleUnityView)
  {
    if (!GUI.Button(new Rect((float) ((double) position.width * 0.5 - 70.0), position.height - 47f, 140f, 30f), !this._bundleUnityView.IsOwned ? new GUIContent(bundleUnityView.Price, (Texture) UberstrikeIcons.FacebookCreditsIcon, "Buy the " + bundleUnityView.BundleView.Name + " pack.") : new GUIContent("Purchased"), BlueStonez.buttongold_large_price))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    GUITools.Clicked();
    if (ScreenResolutionManager.IsFullScreen)
      ScreenResolutionManager.IsFullScreen = false;
    Singleton<BundleManager>.Instance.BuyFacebookBundle(bundleUnityView.BundleView.Id);
  }

  private void BuyStoreKitButton(Rect position, BundleUnityView bundleUnityView)
  {
    if (!GUI.Button(new Rect((float) ((double) position.width * 0.5 - 70.0), position.height - 47f, 140f, 30f), !this._bundleUnityView.IsOwned ? new GUIContent(bundleUnityView.CurrencySymbol + bundleUnityView.Price, "Buy the " + bundleUnityView.BundleView.Name + " pack.") : new GUIContent("Purchased"), BlueStonez.buttongold_large_price))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    if (Singleton<BundleManager>.Instance.CanMakeMasPayments)
    {
      GUITools.Clicked();
      if (ScreenResolutionManager.IsFullScreen)
        ScreenResolutionManager.IsFullScreen = false;
      Singleton<BundleManager>.Instance.BuyStoreKitItem(bundleUnityView);
    }
    else
      PopupSystem.ShowMessage(LocalizedStrings.Error, "Sorry, it appears you are unable to make In App purchases at this time.", PopupSystem.AlertType.OK);
  }

  public override LotteryWinningPopup ShowReward() => (LotteryWinningPopup) null;
}
