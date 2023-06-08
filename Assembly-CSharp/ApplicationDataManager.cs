using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.IO;
using UberStrike.Core.Types;
using UnityEngine;

public static class ApplicationDataManager
{
	public const string HeaderFilename = "UberStrikeHeader";

	public const string MainFilename = "UberStrikeMain";

	public const string StandaloneFilename = "UberStrike";

	public static string Version;

	public static bool IsMac;

	public const int MinimalWidth = 989;

	public const int MinimalHeight = 560;

	public static string WebServiceBaseUrl;

	public static string ImagePath;

	public static bool IsDebug;

	private static float applicationDateTime;

	private static DateTime serverDateTime;

	public static string HWID;

	public static bool WebPlayerHasResult;

	public static ChannelType Channel => ChannelType.Steam;

	public static ApplicationOptions ApplicationOptions
	{
		get;
		private set;
	}

	public static bool IsOnline
	{
		get;
		set;
	}

	public static bool IsMobile
	{
		get
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				return Application.platform == RuntimePlatform.Android;
			}
			return true;
		}
	}

	public static bool IsDesktop
	{
		get
		{
			if (Application.platform != RuntimePlatform.OSXPlayer)
			{
				return Application.platform == RuntimePlatform.WindowsPlayer;
			}
			return true;
		}
	}

	public static LocaleType CurrentLocale
	{
		get;
		set;
	}

	public static string FrameRate
	{
		get
		{
			int num = Mathf.Max(Mathf.RoundToInt(Time.smoothDeltaTime * 1000f), 1);
			return $"{1000 / num} ({num}ms)";
		}
	}

	public static DateTime ServerDateTime
	{
		get
		{
			return serverDateTime.AddSeconds(Time.time - applicationDateTime);
		}
		set
		{
			serverDateTime = value;
			applicationDateTime = Time.realtimeSinceStartup;
		}
	}

	static ApplicationDataManager()
	{
#if DEBUG
		IsDebug = true;
#endif
        if (!IsDebug)
        {
            WebServiceBaseUrl = "127.0.0.1:5000/2.0/";
            ImagePath = "127.0.0.1:5000/images/";
            Version = "4.8.6";
        }
        else
        {
            WebServiceBaseUrl = "127.0.0.1:5000/2.0/";
            ImagePath = "127.0.0.1:5000/images/";
            Version = "4.8.6";
        }
        Version = "4.8.6";
        IsMac = (Application.platform == RuntimePlatform.OSXPlayer);
		applicationDateTime = 0f;
		serverDateTime = DateTime.Now;
		WebPlayerHasResult = false;
		ApplicationOptions = new ApplicationOptions();
	}

	public static void LockApplication(string message = "An error occured that forced UberStrike to halt.")
	{
		PopupSystem.ClearAll();
		PopupSystem.ShowMessage(LocalizedStrings.Error, message, PopupSystem.AlertType.OK, Application.Quit);
	}

	public static void RefreshWallet()
	{
		UnityRuntime.StartRoutine(StartRefreshWalletInventory());
	}

	public static void OpenUrl(string title, string url)
	{
		if (Application.isWebPlayer)
		{
			Application.ExternalCall("displayMessage", title, url);
			return;
		}
		if (Screen.fullScreen && Application.platform != RuntimePlatform.WindowsPlayer)
		{
			ScreenResolutionManager.IsFullScreen = false;
		}
		Application.OpenURL(url);
	}

	public static void OpenBuyCredits()
	{
		if (Channel == ChannelType.Steam)
		{
			LoadBuyCreditsPage();
			return;
		}
		LoadBuyCreditsPage();
		Debug.LogWarning("Buying credits might not be supported on channel: " + Channel.ToString());
	}

	private static void LoadBuyCreditsPage()
	{
		if (!GameState.Current.HasJoinedGame)
		{
			GameData.Instance.MainMenu.Value = MainMenuState.None;
			MenuPageManager.Instance.LoadPage(PageType.Shop);
		}
		EventHandler.Global.Fire(new ShopEvents.SelectShopArea
		{
			ShopArea = ShopArea.Credits
		});
	}

	private static IEnumerator StartRefreshWalletInventory()
	{
		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetMemberWallet());
		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(showProgress: true));
	}
}
