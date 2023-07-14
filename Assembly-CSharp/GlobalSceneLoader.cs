// Decompiled with JetBrains decompiler
// Type: GlobalSceneLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class GlobalSceneLoader : MonoBehaviour
{
  [SerializeField]
  private GUISkin popupSkin;

  public static string ErrorMessage { get; private set; }

  public static bool IsError => !string.IsNullOrEmpty(GlobalSceneLoader.ErrorMessage);

  public static bool IsInitialised { get; set; }

  public static float GlobalSceneProgress { get; private set; }

  public static bool IsGlobalSceneLoaded { get; private set; }

  public static float ItemAssetBundleProgress { get; private set; }

  public static bool IsItemAssetBundleLoaded { get; private set; }

  private void Awake()
  {
    PopupSkin.Initialize(this.popupSkin);
    ScreenResolutionManager.SetTwoMinusMaxResolution();
  }

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new GlobalSceneLoader.\u003CStart\u003Ec__Iterator6()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator LoadConfig()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GlobalSceneLoader.\u003CLoadConfig\u003Ec__Iterator7 configCIterator7 = new GlobalSceneLoader.\u003CLoadConfig\u003Ec__Iterator7();
    return (IEnumerator) configCIterator7;
  }

  private void InitializeGlobalScene()
  {
    ApplicationDataManager.CurrentLocale = LocaleType.en_US;
    ApplicationDataManager.ApplicationOptions.Initialize();
    this.StartCoroutine(GUITools.StartScreenSizeListener(1f));
    if (ApplicationDataManager.ApplicationOptions.IsUsingCustom)
    {
      QualitySettings.masterTextureLimit = ApplicationDataManager.ApplicationOptions.VideoTextureQuality;
      QualitySettings.vSyncCount = ApplicationDataManager.ApplicationOptions.VideoVSyncCount;
      QualitySettings.antiAliasing = ApplicationDataManager.ApplicationOptions.VideoAntiAliasing;
    }
    else
      QualitySettings.SetQualityLevel(ApplicationDataManager.ApplicationOptions.VideoQualityLevel);
    AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
    AutoMonoBehaviour<SfxManager>.Instance.UpdateMasterVolume();
    AutoMonoBehaviour<SfxManager>.Instance.UpdateMusicVolume();
    AutoMonoBehaviour<SfxManager>.Instance.UpdateEffectsVolume();
    AutoMonoBehaviour<InputManager>.Instance.ReadAllKeyMappings();
    PerformanceTest.RunPerformanceCheck();
  }

  [DebuggerHidden]
  private IEnumerator BeginAuthenticateApplication() => (IEnumerator) new GlobalSceneLoader.\u003CBeginAuthenticateApplication\u003Ec__Iterator8()
  {
    \u003C\u003Ef__this = this
  };

  private void OnAuthenticateApplication(AuthenticateApplicationView ev)
  {
    try
    {
      GlobalSceneLoader.IsInitialised = true;
      if (ev != null && ev.IsEnabled)
      {
        Configuration.EncryptionInitVector = ev.EncryptionInitVector;
        Configuration.EncryptionPassPhrase = ev.EncryptionPassPhrase;
        ApplicationDataManager.IsOnline = true;
        foreach (PhotonView gameServer in ev.GameServers)
        {
          if (gameServer.UsageType != PhotonUsageType.Mobile)
            Singleton<GameServerManager>.Instance.AddGameServer(gameServer);
        }
        CmuneNetworkManager.CurrentCommServer = new GameServerView(ev.CommServer);
        if (!ev.WarnPlayer)
          return;
        this.HandleVersionWarning();
      }
      else
      {
        UnityEngine.Debug.Log((object) ("OnAuthenticateApplication failed with 4.3.10/" + (object) ApplicationDataManager.Channel + ": " + GlobalSceneLoader.ErrorMessage));
        GlobalSceneLoader.ErrorMessage = "Please update.";
        this.HandleVersionError();
      }
    }
    catch (Exception ex)
    {
      GlobalSceneLoader.ErrorMessage = ex.Message + " " + ex.StackTrace;
      UnityEngine.Debug.LogError((object) ("OnAuthenticateApplication crashed with 4.3.10/" + (object) ApplicationDataManager.Channel + ": " + GlobalSceneLoader.ErrorMessage));
      this.HandleApplicationAuthenticationError("There was a problem loading UberStrike. Please check your internet connection and try again.");
    }
  }

  private void OnAuthenticateApplicationException(Exception exception)
  {
    GlobalSceneLoader.ErrorMessage = exception.Message;
    UnityEngine.Debug.LogError((object) ("An exception occurred while authenticating the application with 4.3.10/" + (object) ApplicationDataManager.Channel + ": " + exception.Message));
    this.HandleApplicationAuthenticationError("There was a problem loading UberStrike. Please check your internet connection and try again.");
  }

  private void RetryAuthentiateApplication()
  {
    GlobalSceneLoader.ErrorMessage = string.Empty;
    this.StartCoroutine(this.BeginAuthenticateApplication());
  }

  private void HandleApplicationAuthenticationError(string message)
  {
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WebPortal:
      case ChannelType.WebFacebook:
      case ChannelType.Kongregate:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.None);
        break;
      case ChannelType.WindowsStandalone:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(this.RetryAuthentiateApplication));
        break;
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
      case ChannelType.IPhone:
      case ChannelType.IPad:
      case ChannelType.Android:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(this.RetryAuthentiateApplication));
        break;
      default:
        PopupSystem.ShowError(LocalizedStrings.Error, message + "This client type is not supported.", PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
    }
  }

  private void HandleConfigurationMissingError(string message)
  {
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WebPortal:
      case ChannelType.WebFacebook:
      case ChannelType.Kongregate:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.None);
        break;
      case ChannelType.WindowsStandalone:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
      case ChannelType.IPhone:
      case ChannelType.IPad:
      case ChannelType.Android:
        PopupSystem.ShowError(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
      default:
        PopupSystem.ShowError(LocalizedStrings.Error, message + "This client type is not supported.", PopupSystem.AlertType.OK, new Action(Application.Quit));
        break;
    }
  }

  private void HandleVersionWarning()
  {
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WebPortal:
      case ChannelType.WebFacebook:
      case ChannelType.Kongregate:
        PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. You should refresh your browser.", PopupSystem.AlertType.OK, new Action(Singleton<AuthenticationManager>.Instance.LoginByChannel));
        break;
      case ChannelType.WindowsStandalone:
      case ChannelType.Android:
        PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. Click OK to update from our website.", PopupSystem.AlertType.OKCancel, new Action(this.OpenUberStrikeUpdatePage), new Action(Singleton<AuthenticationManager>.Instance.LoginByChannel));
        break;
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
        PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. Click OK to update from the Mac App Store.", PopupSystem.AlertType.OKCancel, new Action(this.OpenMacAppStoreUpdatesPage), new Action(Singleton<AuthenticationManager>.Instance.LoginByChannel));
        break;
      case ChannelType.IPhone:
      case ChannelType.IPad:
        PopupSystem.ShowError("Warning", "Your UberStrike client is out of date. Click OK to update from the App Store.", PopupSystem.AlertType.OKCancel, new Action(this.OpenIosAppStoreUpdatesPage), new Action(Singleton<AuthenticationManager>.Instance.LoginByChannel));
        break;
      default:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is not supported. Please update from our website.\n(Invalid Channel: " + (object) ApplicationDataManager.Channel + ")", PopupSystem.AlertType.OK, new Action(this.OpenUberStrikeUpdatePage));
        break;
    }
  }

  private void HandleVersionError()
  {
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WebPortal:
      case ChannelType.WebFacebook:
      case ChannelType.Kongregate:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please refresh your browser.", PopupSystem.AlertType.None);
        break;
      case ChannelType.WindowsStandalone:
      case ChannelType.Android:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please update from our website.", PopupSystem.AlertType.OK, new Action(this.OpenUberStrikeUpdatePage));
        break;
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please update from the Mac App Store.", PopupSystem.AlertType.OK, new Action(this.OpenMacAppStoreUpdatesPage));
        break;
      case ChannelType.IPhone:
      case ChannelType.IPad:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is out of date. Please update from the App Store.", PopupSystem.AlertType.OK, new Action(this.OpenIosAppStoreUpdatesPage));
        break;
      default:
        PopupSystem.ShowError(LocalizedStrings.Error, "Your UberStrike client is not supported. Please update from our website.\n(Invalid Channel: " + (object) ApplicationDataManager.Channel + ")", PopupSystem.AlertType.OK, new Action(this.OpenUberStrikeUpdatePage));
        break;
    }
  }

  private void OpenMacAppStoreUpdatesPage()
  {
    ApplicationDataManager.OpenUrl(string.Empty, "macappstore://showUpdatesPage");
    Application.Quit();
  }

  private void OpenIosAppStoreUpdatesPage() => ApplicationDataManager.OpenUrl(string.Empty, "itms-apps://itunes.com/apps/uberstrike");

  private void OpenUberStrikeUpdatePage()
  {
    ApplicationDataManager.OpenUrl(string.Empty, "http://uberstrike.com");
    Application.Quit();
  }

  [DebuggerHidden]
  private static IEnumerator PrefetchSocketPolicyAsync(string address) => (IEnumerator) new GlobalSceneLoader.\u003CPrefetchSocketPolicyAsync\u003Ec__Iterator9()
  {
    address = address,
    \u003C\u0024\u003Eaddress = address
  };
}
