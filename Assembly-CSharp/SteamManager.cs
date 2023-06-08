using Steamworks;
using System;
using System.Text;
using UnityEngine;

internal class SteamManager : MonoBehaviour
{
	private static SteamManager m_instance;

	private bool m_bInitialized;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	public static bool Initialized => m_instance.m_bInitialized;

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
	}

	private void Start()
	{
		Debug.Log("INITIALIZING STEAMWORKS SDK");
		if (m_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		m_instance = this;
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)291210u))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex?.ToString(), this);
			Application.Quit();
			return;
		}
		if (SteamAPI.Init())
		{
			m_bInitialized = true;
			Debug.Log("SteamAPI was successfully initialized!");
			if (!SteamUser.BLoggedOn())
			{
				Debug.LogError("[Steamworks.NET] Steam user must be logged in to play this game (SteamUser()->BLoggedOn() returned false).", this);
				Application.Quit();
			}
		}
		else
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			Application.Quit();
		}
	}

	private void OnEnable()
	{
		if (m_bInitialized && m_SteamAPIWarningMessageHook == null)
		{
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}

	private void OnApplicationQuit()
	{
		if (m_bInitialized)
		{
			SteamAPI.Shutdown();
		}
	}

	private void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}
}
