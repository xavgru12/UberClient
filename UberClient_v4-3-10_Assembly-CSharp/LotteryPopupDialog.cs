// Decompiled with JetBrains decompiler
// Type: LotteryPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UnityEngine;

public abstract class LotteryPopupDialog : IPopupDialog
{
  private const float BerpSpeed = 2.5f;
  protected int Width = 650;
  protected int Height = 330;
  public bool ClickAnywhereToExit = true;
  protected LotteryPopupDialog.State _state;
  protected Action _onLotteryRolled;
  protected Action _onLotteryReturned;
  protected bool _showExitButton = true;

  public string Text { get; set; }

  public string Title { get; set; }

  public LotteryPopupDialog.MyState ReturnedState { get; protected set; }

  public bool IsVisible { get; set; }

  public bool IsUIDisabled { get; set; }

  public bool IsWaiting { get; set; }

  public GuiDepth Depth => GuiDepth.Event;

  public void OnGUI()
  {
    Rect position = this.GetPosition();
    GUI.Box(position, GUIContent.none, BlueStonez.window);
    GUITools.PushGUIState();
    GUI.enabled = !this.IsUIDisabled;
    GUI.BeginGroup(position);
    if (this._showExitButton && GUI.Button(new Rect(position.width - 20f, 0.0f, 20f, 20f), "X", BlueStonez.friends_hidden_button))
      PopupSystem.HideMessage((IPopupDialog) this);
    this.DrawPlayGUI(position);
    GUI.EndGroup();
    GUITools.PopGUIState();
    if (this.IsWaiting)
      WaitingTexture.Draw(position.center);
    if (this.ClickAnywhereToExit && Event.current.type == UnityEngine.EventType.MouseDown && !position.Contains(Event.current.mousePosition))
    {
      this.ClosePopup();
      Event.current.Use();
    }
    this.OnAfterGUI();
  }

  public virtual void OnAfterGUI()
  {
  }

  public void OnHide()
  {
    if (GameState.HasCurrentGame)
      AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
    else
      AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.SeletronRadioShort);
  }

  public void SetRollCallback(Action onLotteryRolled) => this._onLotteryRolled = onLotteryRolled;

  public void SetLotteryReturnedCallback(Action onLotteryReturned) => this._onLotteryReturned = onLotteryReturned;

  public abstract LotteryWinningPopup ShowReward();

  protected abstract void DrawPlayGUI(Rect rect);

  protected void DrawNaviArrows(Rect rect, LotteryShopItem item)
  {
    if (GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 95.0), rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_left))
    {
      PopupSystem.HideMessage((IPopupDialog) this);
      Singleton<LotteryManager>.Instance.ShowPreviousItem(item);
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    if (!GUI.Button(new Rect((float) ((double) rect.width * 0.5 + 75.0), rect.height - 42f, 20f, 20f), GUIContent.none, BlueStonez.button_right))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    Singleton<LotteryManager>.Instance.ShowNextItem(item);
    SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
  }

  protected void ClosePopup() => PopupSystem.HideMessage((IPopupDialog) this);

  protected void OpenGetCredits()
  {
    ApplicationDataManager.OpenBuyCredits();
    if (ApplicationDataManager.Channel != ChannelType.MacAppStore)
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
  }

  private Rect GetPosition() => new Rect((float) (Screen.width - this.Width) * 0.5f, (float) GlobalUIRibbon.Instance.Height() + (float) (Screen.height - GlobalUIRibbon.Instance.Height() - this.Height) * 0.5f, (float) this.Width, (float) this.Height);

  protected enum State
  {
    Normal,
    Rolled,
  }

  public enum MyState
  {
    Waiting,
    Success,
    Failed,
  }
}
