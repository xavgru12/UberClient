// Decompiled with JetBrains decompiler
// Type: ApplicationDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class ApplicationDataManager
{
  public const string Version = "4.3.10";
  public const string FacebookAppId = "24509077139";
  public const int MinimalWidth = 989;
  public const int MinimalHeight = 560;
  public const string HeaderFilename = "UberStrikeHeader";
  public const string MainFilename = "UberStrikeMain";
  public const string TutorialFileName = "UberStrikeTutorial";
  public const string StandaloneFilename = "UberStrike";
  private const string _evalBrwJS = "\r\n        var br=new Array(4);\r\n        //var os=new Array(2);\r\n        //var flash=new Array(2);\r\n        br=getBrowser();\r\n        //os=getOS();\r\n        //flash=hasFlashPlugin();\r\n        //popups = popupsAllowed();\r\n        //jsver = jsVersion();\r\n        //document.write('Browser identifier: '+br[0]+'<br />');\r\n        //document.write('Browser version: '+br[1]+'<br />');\r\n        //document.write('Browser major version: '+getMajorVersion(br[1])+'<br />');\r\n        //document.write('Browser minor version: '+getMinorVersion(br[1])+'<br />');\r\n        //document.write('Browser engine: '+br[2]+'<br />');\r\n        //document.write('Browser engine version: '+br[3]+'<br />');\r\n        //document.write('Full user agent string: '+getFullUAString()+'<br />');\r\n        //document.write('Operating system identifier: '+os[0]+'<br />');\r\n        //document.write('Operating system version: '+os[1]+'<br />');\r\n        //document.write('Is Flash installed? '+ (flash[0]==2 ? 'Yes' : (flash[0] == 1 ? 'No' : 'unknown'))+'<br />');\r\n        //document.write('Flash version: '+flash[1] + '<br />');\r\n        //document.write('Are popups allowed for this site? ' + (popups ? 'Yes' : 'No') + '<br />' );\r\n        //document.write('What is the newest version of Javascript this browser supports? ' + jsver + '<br />');\r\n        GetUnity().SendMessage('ApplicationDataManager', 'SetBrowserInfo', \r\n        br[0] + '%' + \r\n        br[1] + '%' +\r\n        getMajorVersion(br[1]) + '%' +\r\n        getMinorVersion(br[1]) + '%' + \r\n        br[2] + '%' +\r\n        br[3] + '%' +\r\n        getFullUAString()\r\n        ); \r\n        ";
  private static CmuneSystemInfo localSystemInfo;
  public static readonly Dictionary<int, int> XpByLevel = new Dictionary<int, int>();
  private static float applicationDateTime = 0.0f;
  private static DateTime serverDateTime = DateTime.Now;
  private static bool _isEditor = Application.isEditor;

  static ApplicationDataManager()
  {
    ApplicationDataManager.localSystemInfo = new CmuneSystemInfo();
    if (Application.isWebPlayer)
      ApplicationDataManager.WebPlayerSrcValues = new WebPlayerSrcValues(Application.srcValue);
    ApplicationDataManager.ApplicationOptions = new ApplicationOptions();
    AutoMonoBehaviour<UnityRuntime>.Instance.OnAppFocus += new Action<bool>(ApplicationDataManager.OnApplicationFocus);
  }

  public static ClientConfiguration Config { get; set; }

  public static ChannelType Channel => Application.isWebPlayer ? ApplicationDataManager.WebPlayerSrcValues.ChannelType : ApplicationDataManager.Config.ChannelType;

  public static string BuildNumber
  {
    get
    {
      if (Application.isEditor)
        return "Editor";
      switch (ApplicationDataManager.Channel)
      {
        case ChannelType.WebPortal:
        case ChannelType.WebFacebook:
        case ChannelType.Kongregate:
          string str = Application.absoluteURL;
          int startIndex1 = str.IndexOf(".unity3d?");
          if (startIndex1 > 0)
            str = str.Remove(startIndex1);
          int startIndex2 = str.LastIndexOf('/');
          if (startIndex2 > 0)
            str = str.Substring(startIndex2);
          int num = str.LastIndexOf('.');
          if (num > 0)
            return str.Substring(num + 1);
          UnityEngine.Debug.LogError((object) ("SetBuildNumber failed because URL malformed: " + Application.absoluteURL));
          return "Error";
        default:
          return "N/A";
      }
    }
  }

  public static BuildType BuildType => ApplicationDataManager.Config.BuildType;

  public static ApplicationOptions ApplicationOptions { get; private set; }

  public static bool IsOnline { get; set; }

  public static bool IsMobile => ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone;

  public static bool IsEditor
  {
    get => ApplicationDataManager._isEditor;
    set => ApplicationDataManager._isEditor = value;
  }

  public static WebPlayerSrcValues WebPlayerSrcValues { get; set; }

  public static LocaleType CurrentLocale { get; set; }

  public static string BaseAudioURL
  {
    get
    {
      if (ApplicationDataManager.Config != null)
        return ApplicationDataManager.Config.ContentBaseUrl + "Audio/";
      throw new NullReferenceException("BaseAudioURL was called before Configuration file was successfully loaded.");
    }
  }

  public static string BaseMapsURL
  {
    get
    {
      if (ApplicationDataManager.Config != null)
        return ApplicationDataManager.Config.ContentBaseUrl + "Maps/4.3.10/";
      throw new NullReferenceException("BaseMapsURL was called before Configuration file was successfully loaded.");
    }
  }

  public static string BaseItemsURL
  {
    get
    {
      if (ApplicationDataManager.Config != null)
        return ApplicationDataManager.Config.ContentBaseUrl + "Items/4.3.10/";
      throw new NullReferenceException("BaseItemsURL was called before Configuration file was successfully loaded.");
    }
  }

  public static string BaseImageURL
  {
    get
    {
      if (ApplicationDataManager.Config != null)
        return ApplicationDataManager.Config.ContentBaseUrl + "Images/";
      throw new NullReferenceException("BaseImageURL was called before Configuration file was successfully loaded.");
    }
  }

  public static string BaseStandaloneBundlesURL
  {
    get
    {
      RuntimePlatform platform = Application.platform;
      switch (platform)
      {
        case RuntimePlatform.IPhonePlayer:
          return "file://" + Application.dataPath + "/Raw/";
        case RuntimePlatform.Android:
          return "jar:file://" + Application.dataPath + "!/assets/";
        default:
          if (platform == RuntimePlatform.OSXPlayer)
            return "file://" + Application.dataPath + "/Data/";
          return platform == RuntimePlatform.WindowsPlayer ? "file://" + Application.dataPath + "/" : string.Empty;
      }
    }
  }

  private static void OnApplicationFocus(bool isFocused)
  {
    if (isFocused)
      Application.targetFrameRate = ApplicationDataManager.ApplicationOptions == null ? 200 : ApplicationDataManager.ApplicationOptions.GeneralTargetFrameRate;
    else
      Application.targetFrameRate = 20;
  }

  public static void LockApplication(string message = "An error occured that forced UberStrike to halt.")
  {
    PopupSystem.ClearAll();
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WindowsStandalone:
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
      case ChannelType.IPhone:
      case ChannelType.IPad:
      case ChannelType.Android:
        PopupSystem.ShowMessage(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, new Action(Singleton<AuthenticationManager>.Instance.StartLogout));
        break;
      default:
        PopupSystem.ShowMessage(LocalizedStrings.Error, message, PopupSystem.AlertType.None);
        break;
    }
  }

  public static void RefreshWallet() => MonoRoutine.Start(ApplicationDataManager.StartRefreshWalletInventory());

  public static void SetBrowserInfo(string result)
  {
    bool flag = true;
    if (result != null)
    {
      string[] strArray = result.Split('%');
      if (strArray != null)
      {
        ApplicationDataManager.localSystemInfo.BrowserIdentifier = strArray.Length <= 0 || strArray[0] == null ? "Error getting data." : strArray[0];
        ApplicationDataManager.localSystemInfo.BrowserVersion = strArray.Length <= 1 || strArray[1] == null ? "Error getting data." : strArray[1];
        ApplicationDataManager.localSystemInfo.BrowserMajorVersion = strArray.Length <= 2 || strArray[2] == null ? "Error getting data." : strArray[2];
        ApplicationDataManager.localSystemInfo.BrowserMinorVersion = strArray.Length <= 3 || strArray[3] == null ? "Error getting data." : strArray[3];
        ApplicationDataManager.localSystemInfo.BrowserEngine = strArray.Length <= 4 || strArray[4] == null ? "Error getting data." : strArray[4];
        ApplicationDataManager.localSystemInfo.BrowserEngineVersion = strArray.Length <= 5 || strArray[5] == null ? "Error getting data." : strArray[5];
        ApplicationDataManager.localSystemInfo.BrowserUserAgent = strArray.Length <= 6 || strArray[6] == null ? "Error getting data." : strArray[6];
        flag = false;
      }
    }
    if (!flag)
      return;
    ApplicationDataManager.localSystemInfo.BrowserIdentifier = ApplicationDataManager.localSystemInfo.BrowserVersion = ApplicationDataManager.localSystemInfo.BrowserMajorVersion = ApplicationDataManager.localSystemInfo.BrowserMinorVersion = ApplicationDataManager.localSystemInfo.BrowserEngine = ApplicationDataManager.localSystemInfo.BrowserEngineVersion = ApplicationDataManager.localSystemInfo.BrowserUserAgent = "Error communicating with browser.";
  }

  public static void OpenUrl(string title, string url)
  {
    if (Application.isWebPlayer)
    {
      Application.ExternalCall("displayMessage", (object) title, (object) url);
    }
    else
    {
      if (Screen.fullScreen && Application.platform != RuntimePlatform.WindowsPlayer)
        ScreenResolutionManager.IsFullScreen = false;
      Application.OpenURL(url);
    }
  }

  public static void OpenBuyCredits()
  {
    switch (ApplicationDataManager.Channel)
    {
      case ChannelType.WebPortal:
      case ChannelType.Kongregate:
        ScreenResolutionManager.IsFullScreen = false;
        Application.ExternalCall("getCreditsWrapper", (object) PlayerDataManager.CmidSecure);
        break;
      case ChannelType.WebFacebook:
        ApplicationDataManager.LoadBuyCreditsPage();
        break;
      case ChannelType.WindowsStandalone:
      case ChannelType.OSXStandalone:
        ApplicationDataManager.OpenUrl(string.Empty, ApplicationDataManager.GetStandalonePaymentUrl());
        MonoRoutine.Start(ApplicationDataManager.ShowStandaloneRefreshBalancePopup(2f));
        break;
      case ChannelType.MacAppStore:
        if (Singleton<BundleManager>.Instance.CanMakeMasPayments)
        {
          ApplicationDataManager.LoadBuyCreditsPage();
          break;
        }
        PopupSystem.ShowMessage(LocalizedStrings.Error, "Sorry, In App Purchases are only available in Mac OSX Lion (v10.7) and above.", PopupSystem.AlertType.OK);
        break;
      case ChannelType.IPhone:
      case ChannelType.IPad:
        if (Singleton<BundleManager>.Instance.CanMakeMasPayments)
        {
          ApplicationDataManager.LoadBuyCreditsPage();
          break;
        }
        PopupSystem.ShowMessage(LocalizedStrings.Error, "Sorry, In App Purchases are currently unavailable.", PopupSystem.AlertType.OK);
        break;
      case ChannelType.Android:
        if (Singleton<BundleManager>.Instance.CanMakeMasPayments)
        {
          ApplicationDataManager.LoadBuyCreditsPage();
          break;
        }
        PopupSystem.ShowMessage(LocalizedStrings.Error, "Sorry, In App Purchases are currently unavailable.", PopupSystem.AlertType.OK);
        break;
      default:
        UnityEngine.Debug.LogError((object) ("OpenBuyCredits not supported on channel: " + (object) ApplicationDataManager.Channel));
        break;
    }
  }

  private static void LoadBuyCreditsPage()
  {
    if (GameState.HasCurrentGame)
    {
      if (GamePageManager.Instance.HasPage)
      {
        SceneGuiController component = GamePageManager.Instance.GetCurrentPage().GetComponent<SceneGuiController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetShopArea();
      }
    }
    else
      MenuPageManager.Instance.LoadPage(PageType.Shop);
    CmuneEventHandler.Route((object) new SelectShopAreaEvent()
    {
      ShopArea = ShopArea.Credits
    });
  }

  public static void OpenLinkFacebookUrl() => ApplicationDataManager.OpenUrl(string.Empty, ApplicationDataManager.GetLinkFacebookUrl());

  [DebuggerHidden]
  private static IEnumerator ShowStandaloneRefreshBalancePopup(float delayInSecs) => (IEnumerator) new ApplicationDataManager.\u003CShowStandaloneRefreshBalancePopup\u003Ec__Iterator5C()
  {
    delayInSecs = delayInSecs,
    \u003C\u0024\u003EdelayInSecs = delayInSecs
  };

  private static string GetLinkFacebookUrl()
  {
    string str = string.Empty;
    switch (ApplicationDataManager.BuildType)
    {
      case BuildType.Prod:
        str = "http://uberstrike.com/";
        break;
      case BuildType.Dev:
        str = "http://dev.uberstrike.com/";
        break;
      case BuildType.Staging:
        str = "http://qa.uberstrike.com/";
        break;
    }
    return str + string.Format("account/facebooklogin?channel={0}", (object) (int) ApplicationDataManager.Channel);
  }

  private static string GetStandalonePaymentUrl()
  {
    string str = string.Empty;
    switch (ApplicationDataManager.BuildType)
    {
      case BuildType.Prod:
        str = "http://uberstrike.com/";
        break;
      case BuildType.Dev:
        str = "http://dev.uberstrike.com/";
        break;
      case BuildType.Staging:
        str = "http://qa.uberstrike.com/";
        break;
    }
    return str + string.Format("account/externallogin?channel={0}&email={1}&lang={2}", (object) (int) ApplicationDataManager.Channel, (object) PlayerDataManager.EmailSecure, (object) ((Enum) ApplicationDataManager.CurrentLocale).ToString().Replace("_", "-"));
  }

  private static string GetStandaloneUpdateUrl()
  {
    string str = string.Empty;
    switch (ApplicationDataManager.BuildType)
    {
      case BuildType.Prod:
        str = "http://uberstrike.com/";
        break;
      case BuildType.Dev:
        str = "http://dev.uberstrike.com/";
        break;
      case BuildType.Staging:
        str = "http://qa.uberstrike.com/";
        break;
    }
    return str + string.Format("download?mode=update&channel={0}", (object) (int) ApplicationDataManager.Channel);
  }

  public static void ShowMenuTabsInBrowser()
  {
    if (!Application.isWebPlayer)
      return;
    Application.ExternalCall("displayHeader", (object) PlayerDataManager.CmidSecure.ToString());
  }

  public static string FrameRate
  {
    get
    {
      int num = Mathf.Max(Mathf.RoundToInt(Time.smoothDeltaTime * 1000f), 1);
      return string.Format("{0} ({1}ms)", (object) (1000 / num), (object) num);
    }
  }

  [DebuggerHidden]
  private static IEnumerator StartRefreshWalletInventory()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ApplicationDataManager.\u003CStartRefreshWalletInventory\u003Ec__Iterator5D inventoryCIterator5D = new ApplicationDataManager.\u003CStartRefreshWalletInventory\u003Ec__Iterator5D();
    return (IEnumerator) inventoryCIterator5D;
  }

  public static void SetLevelCapsView(List<PlayerLevelCapView> caps)
  {
    ApplicationDataManager.XpByLevel.Clear();
    foreach (PlayerLevelCapView cap in caps)
    {
      if (PlayerXpUtil.MaxPlayerLevel < cap.Level)
        PlayerXpUtil.MaxPlayerLevel = cap.Level;
      ApplicationDataManager.XpByLevel[cap.Level] = cap.XPRequired;
    }
  }

  public static CmuneSystemInfo LocalSystemInfo => ApplicationDataManager.localSystemInfo;

  public static DateTime ServerDateTime
  {
    get => ApplicationDataManager.serverDateTime.AddSeconds((double) Time.time - (double) ApplicationDataManager.applicationDateTime);
    set
    {
      ApplicationDataManager.serverDateTime = value;
      ApplicationDataManager.applicationDateTime = Time.realtimeSinceStartup;
    }
  }
}
