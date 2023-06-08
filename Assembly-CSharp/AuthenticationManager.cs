using Cmune.DataCenter.Common.Entities;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class AuthenticationManager : Singleton<AuthenticationManager>
{
	public static ProgressPopupDialog _progress;

	private Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

	public bool IsAuthComplete
	{
		get;
		private set;
	}

	private AuthenticationManager()
	{
		_progress = new ProgressPopupDialog(LocalizedStrings.SettingUp, LocalizedStrings.ProcessingLogin);
		GameObject gameObject = new GameObject("Plugin Holder");
		gameObject.AddComponent<UberKill>();
		gameObject.AddComponent<UberBeat>();
		gameObject.AddComponent<AvatarConfigAsync>();
		gameObject.AddComponent<CustomBundleMapManager>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
	}

	public void SetAuthComplete(bool enabled)
	{
		IsAuthComplete = enabled;
	}

	public void LoginByChannel()
	{
		UnityRuntime.StartRoutine(StartLoginMemberSteam(directSteamLogin: true));
	}

	public IEnumerator StartLoginMemberEmail(string emailAddress, string password)
	{
		if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
		{
			ApplicationDataManager.LockApplication("Your login credentials are not correct. Please try to login again.");
			yield break;
		}
		_progress.Text = "Authenticating Account";
		_progress.Progress = 0.1f;
		PopupSystem.Show(_progress);
		MemberAuthenticationResultView authenticationView = null;
		yield return AuthenticationWebServiceClient.LoginMemberEmail(emailAddress, password, ApplicationDataManager.Channel, SystemInfo.deviceUniqueIdentifier, delegate(MemberAuthenticationResultView ev)
		{
			authenticationView = ev;
		}, delegate
		{
		});
		if (authenticationView == null)
		{
			ApplicationDataManager.LockApplication("The login could not be processed. Please check your internet connection and try again.");
		}
		else
		{
			yield return UnityRuntime.StartRoutine(CompleteAuthentication(authenticationView));
		}
	}

	public IEnumerator StartLoginMemberSteam(bool directSteamLogin)
	{
		if (directSteamLogin)
		{
			PopupSystem.Show(_progress);
			yield return UnityRuntime.StartRoutine(WindowsUpdater.Updater());

			_progress.Text = "Checking Client";
			_progress.Progress = 0f;
			bool Success = false;
			yield return UnityRuntime.StartRoutine(GlobalSceneLoader.BeginAuthenticateApplication(delegate(bool OnSuccess){Success = OnSuccess;}));
			if (!Success)
			{
				PopupSystem.HideMessage(_progress);
				yield break;
			}
			_progress.Text = "Collecting Hardware ID";
			_progress.Progress = 0f;
			UberBeat.Instance.Run();
			while (string.IsNullOrEmpty(ApplicationDataManager.HWID))
			{
				yield return new WaitForSeconds(0.05f);
			}
			_progress.Text = "Authenticating with Steam";
			_progress.Progress = 0.05f;
			m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
			byte[] pTicket = new byte[1024];
			SteamUser.GetAuthSessionTicket(pTicket, 1024, out uint pcbTicket);
			int num = (int)pcbTicket;
			string authToken = num.ToString();
			string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			MemberAuthenticationResultView authenticationView = null;
			_progress.Text = "Authenticating with UberStrike";
			_progress.Progress = 0.1f;
			yield return AuthenticationWebServiceClient.LoginSteam(ApplicationDataManager.Version, PlayerDataManager.SteamId, authToken, deviceUniqueIdentifier, ApplicationDataManager.HWID, ApplicationDataManager.IsMac, delegate(MemberAuthenticationResultView result)
			{
				authenticationView = result;
				PlayerPrefs.SetString("CurrentSteamUser", PlayerDataManager.SteamId);
				PlayerPrefs.Save();
			}, delegate(Exception error)
			{
				Debug.LogError("Account authentication error: " + error?.ToString());
				ApplicationDataManager.LockApplication("There was an error logging you in. Please try again later or contact us at https://discord.gg/hhxZCBamRT");
			});
			yield return UnityRuntime.StartRoutine(CompleteAuthentication(authenticationView));
		}
		else
		{
			PopupSystem.ClearAll();
			yield return PanelManager.Instance.OpenPanel(PanelType.Login);
		}
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			163.ToString(),
			" - GetAuthSessionTicketResponse] - ",
			null,
			null,
			null
		};
		HAuthTicket hAuthTicket = pCallback.m_hAuthTicket;
		obj[3] = hAuthTicket.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		Debug.Log(string.Concat(obj));
	}

	private IEnumerator CompleteAuthentication(MemberAuthenticationResultView authView, bool isRegistrationLogin = false)
	{
		if (authView == null)
		{
			Debug.LogError("Account authentication error: MemberAuthenticationResultView was null, isRegistrationLogin: " + isRegistrationLogin.ToString());
			ApplicationDataManager.LockApplication("There was an error logging you in. Please try again later or contact us at https://discord.gg/hhxZCBamRT");
			yield break;
		}
		if (authView.MemberAuthenticationResult == MemberAuthenticationResult.IsBanned || authView.MemberAuthenticationResult == MemberAuthenticationResult.IsIpBanned)
		{
			if (authView.BanDuration == -1)
			{
				ApplicationDataManager.LockApplication("Your account has been banned permanently!");
			}
			else
			{
				ApplicationDataManager.LockApplication("Your account has been temporarily banned!" + Environment.NewLine + "Duration left: " + (authView.BanDuration / 60).ToString() + " hours " + (authView.BanDuration % 60).ToString() + " minutes.");
			}
			yield break;
		}
		if (authView.MemberAuthenticationResult == MemberAuthenticationResult.NewUpdate)
		{
			ApplicationDataManager.LockApplication(string.Format(LocalizedStrings.UberStrikeIsOutOfDateVisitWebsite, ApplicationDataManager.Version, authView.ServerGameVersion));
			yield break;
		}
		if (authView.MemberAuthenticationResult == MemberAuthenticationResult.InvalidEsns)
		{
			Debug.Log("Result: " + authView.MemberAuthenticationResult.ToString());
			ApplicationDataManager.LockApplication("Sorry this account is linked already.");
			yield break;
		}
		if (authView.MemberAuthenticationResult != 0)
		{
			Debug.Log("Result: " + authView.MemberAuthenticationResult.ToString());
			ApplicationDataManager.LockApplication("Your login credentials are not correct. Please try to login again.");
			yield break;
		}
		Singleton<PlayerDataManager>.Instance.SetLocalPlayerMemberView(authView.MemberView);
		PlayerDataManager.AuthToken = authView.AuthToken;
		if (!PlayerDataManager.IsTestBuild)
		{
			PlayerDataManager.MagicHash = UberDaemon.Instance.GetMagicHash(authView.AuthToken);
			Debug.Log("Magic Hash:" + PlayerDataManager.MagicHash);
		}
		ApplicationDataManager.ServerDateTime = authView.ServerTime;
		EventHandler.Global.Fire(new GlobalEvents.Login(authView.MemberView.PublicProfile.AccessLevel));

		/*_progress.Text = "Syncing daily achievement";
		_progress.Progress = 0.15f;
		DailyBonusView bonusview = null;
		yield return UserWebServiceClient.DailyLogin(authView.AuthToken, delegate (DailyBonusView bonus) { bonusview = bonus; },
			delegate (Exception ex) { Debug.LogWarning("Webservices returned null for streak"); });*/

		_progress.Text = LocalizedStrings.LoadingFriendsList;
		_progress.Progress = 0.2f;
		yield return UnityRuntime.StartRoutine(Singleton<CommsManager>.Instance.GetContactsByGroups());
		_progress.Text = LocalizedStrings.LoadingCharacterData;
		_progress.Progress = 0.3f;
		yield return ApplicationWebServiceClient.GetConfigurationData(ApplicationDataManager.Version, delegate(ApplicationConfigurationView appConfigView)
		{
			XpPointsUtil.Config = appConfigView;
		}, delegate
		{
			ApplicationDataManager.LockApplication(LocalizedStrings.ErrorLoadingData);
		});
		Singleton<PlayerDataManager>.Instance.SetPlayerStatisticsView(authView.PlayerStatisticsView);
		_progress.Text = LocalizedStrings.LoadingMapData;
		_progress.Progress = 0.5f;
		bool mapsLoadedSuccessfully = false;
		yield return ApplicationWebServiceClient.GetMaps(ApplicationDataManager.Version, DefinitionType.StandardDefinition, delegate(List<MapView> callback)
		{
			mapsLoadedSuccessfully = Singleton<MapManager>.Instance.InitializeMapsToLoad(callback);
		}, delegate
		{
			ApplicationDataManager.LockApplication(LocalizedStrings.ErrorLoadingMaps);
		});
		if (!mapsLoadedSuccessfully)
		{
			ApplicationDataManager.LockApplication(LocalizedStrings.ErrorLoadingMapsSupport);
			PopupSystem.HideMessage(_progress);
			yield break;
		}
		_progress.Progress = 0.6f;
		_progress.Text = LocalizedStrings.LoadingWeaponAndGear;
		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetShop());
		if (!Singleton<ItemManager>.Instance.ValidateItemMall())
		{
			PopupSystem.HideMessage(_progress);
			yield break;
		}
		_progress.Progress = 0.7f;
		_progress.Text = LocalizedStrings.LoadingPlayerInventory;
		yield return UnityRuntime.StartRoutine(Singleton<ItemManager>.Instance.StartGetInventory(showProgress: false));
		_progress.Progress = 0.8f;
		_progress.Text = LocalizedStrings.GettingPlayerLoadout;
		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetLoadout());
		if (!Singleton<LoadoutManager>.Instance.ValidateLoadout())
		{
			ApplicationDataManager.LockApplication(LocalizedStrings.ErrorGettingPlayerLoadoutSupport);
			yield break;
		}
		_progress.Progress = 0.85f;
		_progress.Text = LocalizedStrings.LoadingPlayerStatistics;
		yield return UnityRuntime.StartRoutine(Singleton<PlayerDataManager>.Instance.StartGetMember());
		if (!Singleton<PlayerDataManager>.Instance.ValidateMemberData())
		{
			ApplicationDataManager.LockApplication(LocalizedStrings.ErrorPlayerStatisticsSupport);
			yield break;
		}
		_progress.Progress = 0.9f;
		_progress.Text = LocalizedStrings.LoadingClanData;
		yield return ClanWebServiceClient.GetMyClanId(PlayerDataManager.AuthToken, delegate(int id)
		{
			PlayerDataManager.ClanID = id;
		}, delegate
		{
		});
		if (PlayerDataManager.ClanID > 0)
		{
			yield return ClanWebServiceClient.GetOwnClan(PlayerDataManager.AuthToken, PlayerDataManager.ClanID, delegate(ClanView ev)
			{
				Singleton<ClanDataManager>.Instance.SetClanData(ev);
			}, delegate
			{
			});
		}
		GameState.Current.Avatar.SetDecorator(AvatarBuilder.CreateLocalAvatar());
		GameState.Current.Avatar.UpdateAllWeapons();
		yield return new WaitForEndOfFrame();
		Singleton<InboxManager>.Instance.Initialize();
		yield return new WaitForEndOfFrame();
		Singleton<BundleManager>.Instance.Initialize();
		yield return new WaitForEndOfFrame();
		PopupSystem.HideMessage(_progress);
		if (!authView.IsAccountComplete)
		{
			PanelManager.Instance.OpenPanel(PanelType.CompleteAccount);
		}
		else
		{
			MenuPageManager.Instance.LoadPage(PageType.Home);
			IsAuthComplete = true;
		}
		GlobalUIRibbon.Instance.Show();

		/*if(bonusview!=null)
		{
			DailyPointsPopupDialog popup = new DailyPointsPopupDialog(bonusview);
			PopupSystem.Show(popup);
		}*/
	}

	public void StartLogout()
	{
		UnityRuntime.StartRoutine(Logout());
	}

	private IEnumerator Logout()
	{
		if (GameState.Current.HasJoinedGame)
		{
			Singleton<GameStateController>.Instance.LeaveGame();
			yield return new WaitForSeconds(3f);
		}
		MenuPageManager.Instance.LoadPage(PageType.Home);
		MenuPageManager.Instance.UnloadCurrentPage();
		GlobalUIRibbon.Instance.Hide();
		if (GameState.Current.Avatar.Decorator != null)
		{
			AvatarBuilder.Destroy(GameState.Current.Avatar.Decorator.gameObject);
		}
		GameState.Current.Avatar.SetDecorator(null);
		Singleton<PlayerDataManager>.Instance.Dispose();
		Singleton<InventoryManager>.Instance.Dispose();
		Singleton<LoadoutManager>.Instance.Dispose();
		Singleton<ClanDataManager>.Instance.Dispose();
		Singleton<ChatManager>.Instance.Dispose();
		Singleton<InboxManager>.Instance.Dispose();
		Singleton<TransactionHistory>.Instance.Dispose();
		Singleton<BundleManager>.Instance.Dispose();
		Singleton<GameStateController>.Instance.ResetClient();
		AutoMonoBehaviour<CommConnectionManager>.Instance.Reconnect();
		InboxThread.Current = null;
		EventHandler.Global.Fire(new GlobalEvents.Logout());
		GameData.Instance.MainMenu.Value = MainMenuState.Logout;
		Application.Quit();
	}

	private void ShowLoginErrorPopup(string title, string message)
	{
		Debug.Log("Login Error!");
		PopupSystem.HideMessage(_progress);
		PopupSystem.ShowMessage(title, message, PopupSystem.AlertType.OK, delegate
		{
			LoginPanelGUI.ErrorMessage = string.Empty;
			LoginByChannel();
		});
	}
}
