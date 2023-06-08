using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class GlobalSceneLoader : MonoBehaviour
{
	private const float FadeTime = 1f;

	[SerializeField]
	private bool UseTestPhotonServers;

	[SerializeField]
	private string TestCommServer;

	[SerializeField]
	private string TestGameServer;

	[SerializeField]
	private GUISkin popupSkin;

	private Texture2D _blackTexture;

	private Color _color;

	public static string ErrorMessage
	{
		get;
		private set;
	}

	public static bool IsError => !string.IsNullOrEmpty(ErrorMessage);

	public static bool IsInitialised
	{
		get;
		set;
	}

	public static float GlobalSceneProgress
	{
		get;
		private set;
	}

	public static bool IsGlobalSceneLoaded
	{
		get;
		private set;
	}

	public static float ItemAssetBundleProgress
	{
		get;
		private set;
	}

	public static bool IsItemAssetBundleLoaded
	{
		get;
		private set;
	}

	public static bool IsItemAssetBundleDownloading
	{
		get;
		private set;
	}

	private void Awake()
	{
		PopupSkin.Initialize(popupSkin);
		_blackTexture = new Texture2D(1, 1, TextureFormat.RGB24, mipmap: false);
		_color = Color.black;
	}

	private void OnGUI()
	{
		GUI.depth = 8;
		GUI.color = _color;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _blackTexture);
		GUI.color = Color.white;
	}

	private IEnumerator Start()
	{
		Application.runInBackground = true;
		Application.LoadLevel("Menu");
		Configuration.WebserviceBaseUrl = ApplicationDataManager.WebServiceBaseUrl;
		GlobalSceneProgress = 1f;
		IsGlobalSceneLoaded = true;
		ItemAssetBundleProgress = 1f;
		IsItemAssetBundleLoaded = true;
		InitializeGlobalScene();
		yield return new WaitForSeconds(1f);
		for (float f = 0f; f < 1f; f += Time.deltaTime)
		{
			yield return new WaitForEndOfFrame();
			_color.a = 1f - f / 1f;
		}
		if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			ApplicationDataManager.LockApplication("MacOS is not supported anymore." + Environment.NewLine + "Kindly switch to Windows or Bootcamp to be able to play.");
			yield break;
		}
		Debug.Log("Start LoginByChannel");
		Singleton<AuthenticationManager>.Instance.LoginByChannel();
		yield return new WaitForSeconds(1f);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void InitializeGlobalScene()
	{
		ApplicationDataManager.CurrentLocale = LocaleType.en_US;
		ApplicationDataManager.ApplicationOptions.Initialize();
		StartCoroutine(GUITools.StartScreenSizeListener(1f));
		if (ApplicationDataManager.ApplicationOptions.IsUsingCustom)
		{
			QualitySettings.masterTextureLimit = ApplicationDataManager.ApplicationOptions.VideoTextureQuality;
			QualitySettings.vSyncCount = ApplicationDataManager.ApplicationOptions.VideoVSyncCount;
			QualitySettings.antiAliasing = ApplicationDataManager.ApplicationOptions.VideoAntiAliasing;
		}
		else
		{
			QualitySettings.SetQualityLevel(ApplicationDataManager.ApplicationOptions.VideoQualityLevel);
		}
		AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
		AutoMonoBehaviour<SfxManager>.Instance.UpdateMasterVolume();
		AutoMonoBehaviour<SfxManager>.Instance.UpdateMusicVolume();
		AutoMonoBehaviour<SfxManager>.Instance.UpdateEffectsVolume();
		AutoMonoBehaviour<InputManager>.Instance.ReadAllKeyMappings();
	}

	public static IEnumerator BeginAuthenticateApplication(Action<bool> OnSuccess)
	{
		Debug.Log("BeginAuthenticateApplication " + Configuration.WebserviceBaseUrl);
		yield return ApplicationWebServiceClient.AuthenticateApplication(ApplicationDataManager.Version, ApplicationDataManager.Channel, string.Empty, delegate(AuthenticateApplicationView callback)
		{
			OnAuthenticateApplication(callback,OnSuccess);
		}, delegate(Exception ex)
		{
			OnAuthenticateException(ex,OnSuccess);
		});
	}

	private static void OnAuthenticateApplication(AuthenticateApplicationView ev,Action<bool> OnSuccess)
	{
		try
		{
			IsInitialised = true;
			if (ev != null && ev.IsEnabled)
			{
				Configuration.EncryptionInitVector = ev.EncryptionInitVector;
				Configuration.EncryptionPassPhrase = ev.EncryptionPassPhrase;
				ApplicationDataManager.IsOnline = true;
				Singleton<GameServerManager>.Instance.CommServer = new PhotonServer(ev.CommServer);
				Singleton<GameServerManager>.Instance.AddPhotonGameServers(ev.GameServers.FindAll((PhotonView i) => i.UsageType == PhotonUsageType.All));
				OnSuccess(true);
			}
			else
			{
				Debug.Log("OnAuthenticateApplication failed with " + ApplicationDataManager.Version + "/ " + ApplicationDataManager.Channel.ToString() + ": " + ErrorMessage);
				PopupSystem.ShowMessage("Out of date", $"Game version {ApplicationDataManager.Version} is out of date, Contact support @https://discord.gg/hhxZCBamRT");
				OnSuccess(false);
			}
		}
		catch (Exception ex)
		{
			OnAuthenticateException(ex,OnSuccess);
		}
	}

	private static void OnAuthenticateException(Exception ex,Action<bool> OnSuccess)
	{
		Debug.LogError($"OnAuthenticationException: {ApplicationDataManager.Version} {ApplicationDataManager.Channel}");
		Debug.LogError(ex);
		PopupSystem.ShowMessage("Error", "There was a problem connecting to UberStrike.\nKindly check your internet connectivity.\nIf the issue persists ,contact support @https://discord.gg/hhxZCBamRT", PopupSystem.AlertType.OK, Application.Quit);
		OnSuccess(false);
	}
}
