// Decompiled with JetBrains decompiler
// Type: LoginPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class LoginPanelGUI : PanelGuiBase
{
  private Rect _rect;
  private string _emailAddress = string.Empty;
  private string _password = string.Empty;
  private bool _rememberPassword;
  private float _keyboardOffset;
  private float _targetKeyboardOffset;
  private float _errorAlpha;
  private float _panelAlpha;
  private BasePopupDialog _facebookLoginDialog;
  private float dialogTimer;

  public static string ErrorMessage { get; set; }

  public static bool IsBanned { get; set; }

  private void Start()
  {
    this._rememberPassword = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Player_AutoLogin);
    if (!this._rememberPassword)
      return;
    this._password = CmunePrefs.ReadKey<string>(CmunePrefs.Key.Player_Password);
    this._emailAddress = CmunePrefs.ReadKey<string>(CmunePrefs.Key.Player_Email);
  }

  public override void Hide()
  {
    base.Hide();
    this._errorAlpha = 0.0f;
    LoginPanelGUI.ErrorMessage = string.Empty;
    this._targetKeyboardOffset = 0.0f;
  }

  public override void Show()
  {
    base.Show();
    if (LoginPanelGUI.IsBanned)
      LoginPanelGUI.ErrorMessage = LocalizedStrings.YourAccountHasBeenBanned;
    if (!string.IsNullOrEmpty(LoginPanelGUI.ErrorMessage))
      this._errorAlpha = 1f;
    this._panelAlpha = 0.0f;
  }

  private void HideKeyboard()
  {
  }

  private void Update()
  {
    if (!string.IsNullOrEmpty(this._emailAddress))
      this._emailAddress = this._emailAddress.Replace("\n", string.Empty).Replace("\t", string.Empty);
    if (!string.IsNullOrEmpty(this._password))
      this._password = this._password.Replace("\n", string.Empty).Replace("\t", string.Empty);
    if ((double) this._errorAlpha <= 0.0)
      return;
    this._errorAlpha -= Time.deltaTime * 0.1f;
  }

  private void OnGUI()
  {
    this._panelAlpha = Mathf.Lerp(this._panelAlpha, 1f, Time.deltaTime / 2f);
    GUI.color = new Color(1f, 1f, 1f, this._panelAlpha);
    this._keyboardOffset = (double) Mathf.Abs(this._keyboardOffset - this._targetKeyboardOffset) <= 2.0 ? this._targetKeyboardOffset : Mathf.Lerp(this._keyboardOffset, this._targetKeyboardOffset, Time.deltaTime * 4f);
    this._rect = new Rect((float) ((Screen.width - 334) / 2), (float) ((Screen.height - 200) / 2) - this._keyboardOffset, 334f, 200f);
    this.DrawLoginPanel();
    if (!string.IsNullOrEmpty(GUI.tooltip))
    {
      Matrix4x4 matrix = GUI.matrix;
      GUI.matrix = Matrix4x4.identity;
      Vector2 vector2 = BlueStonez.tooltip.CalcSize(new GUIContent(GUI.tooltip));
      GUI.Label(new Rect(Mathf.Clamp(Event.current.mousePosition.x, 14f, (float) Screen.width - (vector2.x + 14f)), Event.current.mousePosition.y + 24f, vector2.x, vector2.y + 16f), GUI.tooltip, BlueStonez.tooltip);
      GUI.matrix = matrix;
    }
    GUI.color = Color.white;
  }

  private void DrawLoginPanel()
  {
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window);
    if (!string.IsNullOrEmpty(this._emailAddress) && !string.IsNullOrEmpty(this._password) && Event.current.type == UnityEngine.EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
    {
      this.Login(this._emailAddress, this._password);
      this.HideKeyboard();
    }
    GUI.depth = 3;
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 23f), LocalizedStrings.WelcomeToUS, BlueStonez.tab_strip);
    if (!string.IsNullOrEmpty(LoginPanelGUI.ErrorMessage))
    {
      GUI.contentColor = ColorScheme.UberStrikeYellow.SetAlpha(this._errorAlpha);
      GUI.Label(new Rect(8f, 30f, this._rect.width - 16f, 23f), LoginPanelGUI.ErrorMessage, BlueStonez.label_interparkmed_11pt);
      GUI.contentColor = Color.white;
    }
    this._emailAddress = GUI.TextField(new Rect(8f, 64f, 220f, 24f), this._emailAddress, 100, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._emailAddress))
    {
      GUI.color = Color.white.SetAlpha(0.3f);
      GUI.Label(new Rect(8f, 64f, 200f, 24f), "  " + LocalizedStrings.Email, BlueStonez.label_interparkbold_13pt_left);
      GUI.color = Color.white;
    }
    this._password = GUI.PasswordField(new Rect(8f, 92f, 220f, 24f), this._password, '*', 64, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._password))
    {
      GUI.color = Color.white.SetAlpha(0.3f);
      GUI.Label(new Rect(8f, 92f, 200f, 24f), "  " + LocalizedStrings.Password, BlueStonez.label_interparkbold_13pt_left);
      GUI.color = Color.white;
    }
    GUI.color = Color.white.SetAlpha(0.7f);
    GUI.Label(new Rect(8f, 120f, 108f, 24f), GUIContent.none, BlueStonez.buttondark_small);
    this._rememberPassword = GUI.Toggle(new Rect(12f, 124f, 100f, 24f), this._rememberPassword, LocalizedStrings.RememberMe, BlueStonez.toggle);
    if (GUI.Button(new Rect(120f, 120f, 108f, 24f), new GUIContent(LocalizedStrings.ForgotPassword, LocalizedStrings.TooltipForgotPassword), BlueStonez.buttondark_small))
    {
      this.HideKeyboard();
      switch (ApplicationDataManager.BuildType)
      {
        case BuildType.Prod:
          ApplicationDataManager.OpenUrl(string.Empty, "http://uberstrike.cmune.com/Account/RecoverPassword");
          break;
        case BuildType.Dev:
          ApplicationDataManager.OpenUrl(string.Empty, "http://dev.uberstrike.com/Account/RecoverPassword");
          break;
        case BuildType.Staging:
          ApplicationDataManager.OpenUrl(string.Empty, "http://qa.uberstrike.com/Account/RecoverPassword");
          break;
      }
    }
    GUI.color = Color.white;
    GUI.enabled = (string.IsNullOrEmpty(this._emailAddress) ? 1 : (string.IsNullOrEmpty(this._password) ? 1 : 0)) == 0;
    if (GUITools.Button(new Rect(236f, 64f, 90f, 52f), new GUIContent(LocalizedStrings.Login), BlueStonez.button_green))
    {
      this.Login(this._emailAddress, this._password);
      this.HideKeyboard();
    }
    GUI.enabled = true;
    GUI.Label(new Rect(8f, 150f, this._rect.width - 16f, 8f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    if (GUITools.Button(new Rect(8f, 160f, 152f, 30f), new GUIContent(LocalizedStrings.SignUp, LocalizedStrings.CreateNewAccount), BlueStonez.buttondark_medium))
    {
      this.Hide();
      this.HideKeyboard();
      PanelManager.Instance.OpenPanel(PanelType.Signup);
    }
    if (ApplicationDataManager.Channel != ChannelType.WindowsStandalone && GUITools.Button(new Rect(178f, 160f, 152f, 30f), new GUIContent(string.Empty, LocalizedStrings.TooltipFacebookAccount), BlueStonez.button_fbconnect))
    {
      this.HideKeyboard();
      ApplicationDataManager.OpenLinkFacebookUrl();
    }
    GUI.enabled = true;
    this.DrawMiniButtons();
    GUI.EndGroup();
  }

  [DebuggerHidden]
  public IEnumerator StartCancelDialogTimer() => (IEnumerator) new LoginPanelGUI.\u003CStartCancelDialogTimer\u003Ec__Iterator1E()
  {
    \u003C\u003Ef__this = this
  };

  private void FacebookSessionInvalidatedEvent()
  {
  }

  private void FacebookLoggedOutEvent()
  {
  }

  private void FacebookLoginFailedEvent(string obj) => LoginPanelGUI.ErrorMessage = "Facebook Login attempt failed!";

  private void FacebookLoginSucceededEvent() => this.Hide();

  private void FacebookMacLoginSucceededEvent(string accessToken) => this.Hide();

  private void DrawMiniButtons()
  {
    if (Application.isWebPlayer)
      return;
    if (GUITools.Button(new Rect(this._rect.width - 57f, 9f, 16f, 16f), !ApplicationDataManager.ApplicationOptions.AudioEnabled ? new GUIContent((Texture) GlobalUiIcons.QuadpanelButtonSoundoff, LocalizedStrings.Unmute) : new GUIContent((Texture) GlobalUiIcons.QuadpanelButtonSoundon, LocalizedStrings.Mute), BlueStonez.panelquad_button))
    {
      ApplicationDataManager.ApplicationOptions.AudioEnabled = !ApplicationDataManager.ApplicationOptions.AudioEnabled;
      AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
      ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
    }
    if (GUITools.Button(new Rect(this._rect.width - 41f, 9f, 16f, 16f), !Screen.fullScreen ? new GUIContent((Texture) GlobalUiIcons.QuadpanelButtonFullscreen, LocalizedStrings.GoFullscreen) : new GUIContent((Texture) GlobalUiIcons.QuadpanelButtonNormalize, LocalizedStrings.ExitFullscreen), BlueStonez.panelquad_button))
    {
      if (Screen.fullScreen)
        ScreenResolutionManager.SetTwoMinusMaxResolution();
      else
        ScreenResolutionManager.SetFullScreenMaxResolution();
    }
    if (!GUITools.Button(new Rect(this._rect.width - 25f, 9f, 16f, 16f), new GUIContent("x"), BlueStonez.panelquad_button))
      return;
    Application.Quit();
  }

  private void Login(string emailAddress, string password)
  {
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Player_AutoLogin, this._rememberPassword);
    if (this._rememberPassword)
    {
      CmunePrefs.WriteKey<string>(CmunePrefs.Key.Player_Password, password);
      CmunePrefs.WriteKey<string>(CmunePrefs.Key.Player_Email, emailAddress);
    }
    this._errorAlpha = 1f;
    if (string.IsNullOrEmpty(emailAddress))
      LoginPanelGUI.ErrorMessage = LocalizedStrings.EnterYourEmailAddress;
    else if (string.IsNullOrEmpty(password))
      LoginPanelGUI.ErrorMessage = LocalizedStrings.EnterYourPassword;
    else if (!ValidationUtilities.IsValidEmailAddress(emailAddress))
      LoginPanelGUI.ErrorMessage = LocalizedStrings.EmailAddressIsInvalid;
    else if (!ValidationUtilities.IsValidPassword(password))
    {
      LoginPanelGUI.ErrorMessage = LocalizedStrings.PasswordIsInvalid;
    }
    else
    {
      this.Hide();
      MonoRoutine.Start(Singleton<AuthenticationManager>.Instance.StartLoginMemberEmail(emailAddress, password));
    }
  }
}
