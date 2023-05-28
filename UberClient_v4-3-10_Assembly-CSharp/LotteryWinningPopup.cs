// Decompiled with JetBrains decompiler
// Type: LotteryWinningPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public abstract class LotteryWinningPopup : IPopupDialog
{
  private int Width;
  private int Height;
  private float _deltaY;
  private DynamicTexture _bkImage;
  private LotteryShopItem _shopItem;

  public LotteryWinningPopup(DynamicTexture image, LotteryShopItem shopItem)
  {
    this.Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
    this.Width = 388;
    this.Title = LocalizedStrings.Congratulations.ToUpper();
    this._bkImage = image;
    this._shopItem = shopItem;
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.MysteryBoxMusic);
  }

  public string Text { get; set; }

  public string Title { get; set; }

  public GuiDepth Depth => GuiDepth.Event;

  public void OnGUI()
  {
    Rect position = this.GetPosition();
    GUI.Box(position, GUIContent.none, BlueStonez.window);
    GUI.BeginGroup(position);
    if (GUI.Button(new Rect(position.width - 20f, 0.0f, 20f, 20f), "X", BlueStonez.friends_hidden_button))
      PopupSystem.HideMessage((IPopupDialog) this);
    GUI.color = ColorScheme.HudTeamBlue;
    float width1 = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(this.Title)).x * 2.5f;
    GUI.DrawTexture(new Rect((float) (((double) position.width - (double) width1) * 0.5), -29f, width1, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 15f, position.width, 30f), this.Title, BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
    GUI.Label(new Rect(30f, 40f, position.width - 60f, 40f), this.Text, BlueStonez.label_interparkbold_16pt);
    int width2 = 288;
    int left = (this.Width - width2 - 6) / 2;
    int height = 323;
    GUI.BeginGroup(new Rect((float) left, 75f, (float) width2, (float) height), BlueStonez.item_slot_large);
    this._bkImage.Draw(new Rect((float) ((width2 - 282) / 2), (float) ((height - 317) / 2), 282f, 317f));
    this.DrawItemGrid(new Rect(0.0f, 0.0f, (float) width2, (float) height), true);
    GUI.EndGroup();
    if (GUI.Button(new Rect((float) left, position.height - 55f, 120f, 32f), LocalizedStrings.PlayAgainCaps, BlueStonez.button_green))
    {
      PopupSystem.HideMessage((IPopupDialog) this);
      if (this._shopItem != null)
      {
        if (this._shopItem.Category == BundleCategoryType.Login || this._shopItem.Category == BundleCategoryType.Signup)
          Singleton<LotteryManager>.Instance.ShowNextItem(this._shopItem);
        else
          this._shopItem.Use();
      }
    }
    if (GUI.Button(new Rect(position.width - 126f - (float) left, position.height - 55f, 120f, 32f), LocalizedStrings.DoneCaps, BlueStonez.button))
    {
      PopupSystem.HideMessage((IPopupDialog) this);
      CmuneEventHandler.Route((object) new SelectShopAreaEvent()
      {
        ShopArea = ShopArea.Inventory
      });
    }
    GUI.EndGroup();
  }

  public void OnHide()
  {
    if (GameState.HasCurrentGame)
      AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
    else
      AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.SeletronRadioShort);
  }

  public void SetYOffset(float offset) => this._deltaY = offset;

  protected abstract void DrawItemGrid(Rect rect, bool showItems);

  private Rect GetPosition() => new Rect((float) (Screen.width - this.Width) * 0.5f, (float) GlobalUIRibbon.Instance.Height() + (float) (Screen.height - GlobalUIRibbon.Instance.Height() - this.Height) * 0.5f - this._deltaY, (float) this.Width, (float) this.Height);
}
