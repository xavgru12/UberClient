// Decompiled with JetBrains decompiler
// Type: AuthenticationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Core.ViewModel;
using UnityEngine;

public class AuthenticationManager : Singleton<AuthenticationManager>
{
  private ProgressPopupDialog _progress;
  public bool ForceFacebookRelogin;

  private AuthenticationManager() => this._progress = new ProgressPopupDialog(LocalizedStrings.SettingUp, LocalizedStrings.ProcessingLogin);

  public void LoginByChannel()
  {
    if (ApplicationDataManager.IsEditor || Application.absoluteURL.StartsWith("file://"))
    {
      PanelManager.Instance.OpenPanel(PanelType.Login);
    }
    else
    {
      switch (ApplicationDataManager.Channel)
      {
        case ChannelType.WebPortal:
        case ChannelType.Kongregate:
          MonoRoutine.Start(this.StartLoginMemberPortal(ApplicationDataManager.WebPlayerSrcValues));
          break;
        case ChannelType.WebFacebook:
          MonoRoutine.Start(this.StartLoginMemberFacebook(ApplicationDataManager.WebPlayerSrcValues.EsnsId, ApplicationDataManager.WebPlayerSrcValues.Hash));
          break;
        case ChannelType.WindowsStandalone:
        case ChannelType.MacAppStore:
        case ChannelType.OSXStandalone:
        case ChannelType.IPhone:
        case ChannelType.IPad:
        case ChannelType.Android:
          PopupSystem.ClearAll();
          PanelManager.Instance.OpenPanel(PanelType.Login);
          break;
        default:
          UnityEngine.Debug.LogError((object) ("No login mode defined for unsupported channel: " + (object) ApplicationDataManager.Channel));
          this.ShowLoginErrorPopup(LocalizedStrings.Error, "No login mode defined for unsupported channel: " + (object) ApplicationDataManager.Channel + "\nPlease visit support.uberstrike.com");
          break;
      }
    }
  }

  [DebuggerHidden]
  public IEnumerator StartLoginMemberPortal(WebPlayerSrcValues webArguments) => (IEnumerator) new AuthenticationManager.\u003CStartLoginMemberPortal\u003Ec__Iterator5E()
  {
    webArguments = webArguments,
    \u003C\u0024\u003EwebArguments = webArguments,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartLoginMemberEmail(string emailAddress, string password) => (IEnumerator) new AuthenticationManager.\u003CStartLoginMemberEmail\u003Ec__Iterator5F()
  {
    emailAddress = emailAddress,
    password = password,
    \u003C\u0024\u003EemailAddress = emailAddress,
    \u003C\u0024\u003Epassword = password,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartLoginMemberFacebook(string facebookID, string hash) => (IEnumerator) new AuthenticationManager.\u003CStartLoginMemberFacebook\u003Ec__Iterator60()
  {
    hash = hash,
    facebookID = facebookID,
    \u003C\u0024\u003Ehash = hash,
    \u003C\u0024\u003EfacebookID = facebookID,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartLoginMemberFacebook(string facebookAuthToken) => (IEnumerator) new AuthenticationManager.\u003CStartLoginMemberFacebook\u003Ec__Iterator61()
  {
    facebookAuthToken = facebookAuthToken,
    \u003C\u0024\u003EfacebookAuthToken = facebookAuthToken,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator CompleteAuthentication(
    MemberAuthenticationResultView memberAuthenticationResultView)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AuthenticationManager.\u003CCompleteAuthentication\u003Ec__Iterator62()
    {
      memberAuthenticationResultView = memberAuthenticationResultView,
      \u003C\u0024\u003EmemberAuthenticationResultView = memberAuthenticationResultView,
      \u003C\u003Ef__this = this
    };
  }

  public void StartLogout() => MonoRoutine.Start(this.Logout());

  [DebuggerHidden]
  private IEnumerator Logout()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AuthenticationManager.\u003CLogout\u003Ec__Iterator63 logoutCIterator63 = new AuthenticationManager.\u003CLogout\u003Ec__Iterator63();
    return (IEnumerator) logoutCIterator63;
  }

  private bool ValidateAuthenticationResult(
    MemberAuthenticationResultView memberAuthenticationResultView)
  {
    return memberAuthenticationResultView != null && memberAuthenticationResultView.MemberAuthenticationResult == MemberAuthenticationResult.Ok;
  }

  private void ShowLoginErrorPopup(string title, string message)
  {
    UnityEngine.Debug.Log((object) "Login Error!");
    PopupSystem.HideMessage((IPopupDialog) this._progress);
    PopupSystem.ShowMessage(title, message, PopupSystem.AlertType.OK, (Action) (() =>
    {
      LoginPanelGUI.ErrorMessage = string.Empty;
      this.LoginByChannel();
    }));
  }
}
