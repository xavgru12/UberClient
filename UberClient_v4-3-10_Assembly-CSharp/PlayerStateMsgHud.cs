// Decompiled with JetBrains decompiler
// Type: PlayerStateMsgHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerStateMsgHud : Singleton<PlayerStateMsgHud>
{
  private MeshGUIText _temporaryMsgText;
  private MeshGUIText _permanentMsgText;
  private GUIStyle _buttonGuiStyle;

  private PlayerStateMsgHud()
  {
    this._temporaryMsgText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._permanentMsgText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this.ResetHud();
    this.TemporaryMsgEnabled = true;
    this.PermanentMsgEnabled = true;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<CameraWidthChangeEvent>(new Action<CameraWidthChangeEvent>(this.OnCameraRectChange));
  }

  public float PermanentMsgHeight => (float) Screen.height * 0.03f;

  public float TemporaryMsgHeight => (float) Screen.height * 0.08f;

  public Vector2 PermanentMsgPosition => new Vector2((float) (Screen.width / 2), (float) ((double) Screen.height * 0.47999998927116394 + (double) Screen.height * 0.57999998331069946 * (1.0 - (double) Singleton<CameraRectController>.Instance.Width)));

  public Vector2 TemporaryMsgPosition => new Vector2((float) (Screen.width / 2), (float) ((double) Screen.height * 0.5 + (double) Screen.height * 0.60000002384185791 * (1.0 - (double) Singleton<CameraRectController>.Instance.Width)));

  public bool TemporaryMsgEnabled
  {
    get => this._temporaryMsgText.IsVisible;
    set
    {
      if (value)
        this._temporaryMsgText.Show();
      else
        this._temporaryMsgText.Hide();
    }
  }

  public bool PermanentMsgEnabled
  {
    get => this._permanentMsgText.IsVisible;
    set
    {
      if (value)
        this._permanentMsgText.Show();
      else
        this._permanentMsgText.Hide();
    }
  }

  public PlayerStateMsgHud.OnButtonClickedDelegate OnButtonClicked { get; set; }

  public bool ButtonEnabled { get; set; }

  public string ButtonCaption { get; set; }

  public void Draw()
  {
    if (!this.ButtonEnabled || !((UnityEngine.Object) GameState.CurrentSpace != (UnityEngine.Object) null) || !GUITools.Button(new Rect((float) ((double) Screen.width * (double) Singleton<CameraRectController>.Instance.Width * 0.5 - 100.0), (float) ((double) Screen.height * 0.5 + (double) Screen.height * 0.60000002384185791 * (1.0 - (double) Singleton<CameraRectController>.Instance.Width)), 200f, 50f), new GUIContent(this.ButtonCaption), this._buttonGuiStyle) || this.OnButtonClicked == null)
      return;
    this.OnButtonClicked();
  }

  public void DisplayNone()
  {
    this._temporaryMsgText.Text = string.Empty;
    this._permanentMsgText.Text = string.Empty;
  }

  public void DisplayRespawnTimeMsg(int remainingSeconds)
  {
    this._temporaryMsgText.Text = LocalizedStrings.Respawn + ": " + (object) remainingSeconds;
    this._temporaryMsgText.Color = Color.white;
    this._temporaryMsgText.Position = this.TemporaryMsgPosition;
    this.ButtonEnabled = false;
  }

  public void DisplayClickToRespawnMsg()
  {
    this._temporaryMsgText.Text = !ApplicationDataManager.IsMobile ? LocalizedStrings.ClickToRespawn : LocalizedStrings.TapToRespawn;
    this._temporaryMsgText.Color = Color.white;
    this._temporaryMsgText.Position = this.TemporaryMsgPosition;
    this.ButtonEnabled = false;
  }

  public void DisplayDisconnectionTimeoutMsg(int remainingSeconds)
  {
    this._temporaryMsgText.Show();
    this._temporaryMsgText.Text = LocalizedStrings.DisconnectionIn + " " + (object) remainingSeconds;
    this._temporaryMsgText.Color = Color.red;
    this._temporaryMsgText.Position = this.TemporaryMsgPosition;
  }

  public void DisplayWaitingForOtherPlayerMsg()
  {
    this._permanentMsgText.Text = LocalizedStrings.WaitingForOtherPlayers;
    this._permanentMsgText.Color = Color.white;
    this._permanentMsgText.Position = this.PermanentMsgPosition;
  }

  public void DisplaySpectatorFollowingMsg(UberStrike.Realtime.UnitySdk.CharacterInfo info)
  {
    string str = info != null ? info.PlayerName : LocalizedStrings.Nobody;
    this._permanentMsgText.Text = string.Format("{0}\n{1}", (object) LocalizedStrings.Following, (object) str);
    this._permanentMsgText.Color = Color.white;
    this._permanentMsgText.Position = this.PermanentMsgPosition;
  }

  public void DisplaySpectatorModeMsg()
  {
    this._permanentMsgText.Text = LocalizedStrings.SpectatorMode;
    this._permanentMsgText.Color = Color.white;
    this._permanentMsgText.Position = this.PermanentMsgPosition;
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._temporaryMsgText);
    this._temporaryMsgText.Color = Color.white;
    this._temporaryMsgText.ShadowColorAnim.Alpha = 0.0f;
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._permanentMsgText);
    this._permanentMsgText.Color = Color.white;
    this._permanentMsgText.ShadowColorAnim.Alpha = 0.0f;
    this._buttonGuiStyle = StormFront.ButtonBlue;
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    switch (ev.TeamId)
    {
      case TeamID.NONE:
      case TeamID.BLUE:
        this._buttonGuiStyle = StormFront.ButtonBlue;
        break;
      case TeamID.RED:
        this._buttonGuiStyle = StormFront.ButtonRed;
        break;
    }
  }

  private void OnCameraRectChange(CameraWidthChangeEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    float num1 = this.TemporaryMsgHeight / this._temporaryMsgText.TextBounds.y;
    this._temporaryMsgText.Scale = new Vector2(num1, num1);
    this._temporaryMsgText.Position = this.TemporaryMsgPosition;
    float num2 = this.PermanentMsgHeight / this._temporaryMsgText.TextBounds.y;
    this._permanentMsgText.Scale = new Vector2(num2, num2);
    this._permanentMsgText.Position = this.PermanentMsgPosition;
  }

  public delegate void OnButtonClickedDelegate();
}
