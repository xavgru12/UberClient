using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

public class UberBeat : MonoBehaviour
{
	private delegate int DCheckInjection(List<string> modules);

	private delegate void DoUberbeat();

	public static string hash = "mrF+8L3fED1ECYoLRBJvcx9AJfDUanmLvYwwIc2CLsM=";

	public static string trusteduber = "\\uberstrike_data\\mono\\mono.dll|\\uberstrike_data\\plugins\\csteamworks.dll|\\steam_api.dll|\\uberstrike_data\\plugins\\uberheartbeat.dll|\\uberstrike_data\\plugins\\uberbeat.dll";

	public static string[] uberTrusted = trusteduber.Split('|');

	public static string gamepath = Directory.GetCurrentDirectory().ToLower();

	public static string dllpath = Directory.GetCurrentDirectory() + "\\UberStrike_Data\\Plugins\\uberbeat.dll";

	public static EventWaitHandle ChildThreadWait = new EventWaitHandle(initialState: true, EventResetMode.ManualReset);

	public static bool SendUberBeat = false;

	static Thread Thread;

	public static string untrusted;

	public static string Report;

	public static UberBeat Instance;

	[DllImport("UberBeat", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
	public static extern void HWID(StringBuilder text);

	[DllImport("UberBeat", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
	public static extern void UBERBEAT(StringBuilder text);

	[DllImport("UberBeat", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
	public static extern void SIGNATURE(StringBuilder returnstring);

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (SendUberBeat)
		{
			AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUberBeatReport(Report);
			Report = null;
			SendUberBeat = false;
		}
	}

	public void Run()
	{
		getHWID();
		StartCoroutine(delay());
		DoUberbeat doUberbeat = Uberbeat;
		doUberbeat.BeginInvoke(null, null);
	}

	void OnApplicationQuit()
	{
		Thread.Abort();
	}

	private IEnumerator delay()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			ChildThreadWait.Set();
		}
	}

	public static void getHWID()
	{
		Thread = new Thread((ThreadStart)delegate
		{
			if (!Authenticate())
			{
				ApplicationDataManager.LockApplication("Failed to authenticate Uberbeat.");
			}
			else
			{
				try
				{
					StringBuilder stringBuilder = new StringBuilder(10000);
					HWID(stringBuilder);
					ApplicationDataManager.HWID = stringBuilder.ToString() + "|UNITY:" + SystemInfo.deviceUniqueIdentifier;
				}
				catch
				{
					ApplicationDataManager.HWID = "UNITY:" + SystemInfo.deviceUniqueIdentifier;
				}
			}
		});
		Thread.Start();
	}

	private void Uberbeat()
	{
		while (true)
		{
			try
			{
				ChildThreadWait.Reset();
				ChildThreadWait.WaitOne();
				if (!string.IsNullOrEmpty(untrusted))
				{
					CheckUntrusted();
				}
				StringBuilder stringBuilder = new StringBuilder(50000);
				UBERBEAT(stringBuilder);
				Report = "REPORT:" + stringBuilder.ToString().Trim();
				SendUberBeat = true;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Uberbeat error : " + ex.ToString());
			}
		}
	}

	private static void CheckUntrusted()
	{
		if (CheckInjection(untrusted.Split('|')) == 0)
		{
			Report = "TRUSTED:" + untrusted;
			SendUberBeat = true;
			Thread.Sleep(500);
		}
	}

	public static int CheckInjection(string[] modules)
	{
		foreach (string text in modules)
		{
			string text2 = text.ToLower();
			if (text2.EndsWith(".exe"))
			{
				continue;
			}
			if (text2.Contains("uberstrike"))
			{
				int num = 0;
				string[] array = uberTrusted;
				foreach (string str in array)
				{
					if (text2 == gamepath + str)
					{
						break;
					}
					num++;
				}
				if (num >= uberTrusted.Length)
				{
					End(text);
					return 1;
				}
			}
			else if (!CheckWhiteList(text))
			{
				End(text2);
				return 1;
			}
		}
		return 0;
	}

	private static bool CheckWhiteList(string path)
	{
		try
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
			if (versionInfo.CompanyName.Contains("Microsoft") || versionInfo.CompanyName.Contains("Intel") || versionInfo.CompanyName.Contains("Valve") || versionInfo.CompanyName != null)
			{
				return true;
			}
		}
		catch (Exception)
		{
		}
		if (CheckSignature(path))
		{
			return true;
		}
		try
		{
			File.Move(path, path.Remove(path.Length - 1));
			File.Move(path.Remove(path.Length - 1), path);
			return false;
		}
		catch (Exception)
		{
			return true;
		}
	}

	public static void End(string message)
	{
		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendUberBeatReport("DETECTED:" + message);
		AutoMonoBehaviour<CommConnectionManager>.Instance.DisableNetworkConnection("Uberbeat has forced the game to halt\nReason: " + message);
	}

	public static bool Authenticate()
	{
		if (string.IsNullOrEmpty(hash))
		{
			return true;
		}
		try
		{
			using (SHA256 sHA = SHA256.Create())
			{
				using (FileStream inputStream = File.OpenRead(dllpath))
				{
					if (hash == Convert.ToBase64String(sHA.ComputeHash(inputStream)))
					{
						return true;
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static bool CheckSignature(string path)
	{
		path = path.Trim();
		StringBuilder stringBuilder = new StringBuilder(path);
		try
		{
			SIGNATURE(stringBuilder);
			if (stringBuilder.ToString().Equals("true"))
			{
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}
}
