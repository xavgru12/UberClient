using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UberStrike.Core.Models;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class UberKill : MonoBehaviour
{
	public static UberKill Instance;
	public static bool IsLowGravity;
	private static string normalPath;
	private static string destPath;
	private static string blueboxPath;

	public static bool KnockBack
	{
		get;
		set;
	}

	public void Start()
	{
		Instance = this;
		StartCoroutine(StartGetHomeSound());
		InitializeCuberStrikePath();
	}

	static void InitializeCuberStrikePath()
	{
		normalPath = Path.Combine(Directory.GetCurrentDirectory(), ApplicationDataManager.IsMac ? "Uberstrike.app/Contents/Data/sharedassets7.normal" : "UberStrike_Data\\sharedassets7.normal");
		destPath = Path.Combine(Directory.GetCurrentDirectory(), ApplicationDataManager.IsMac ? "Uberstrike.app/Contents/Data/sharedassets7.assets" : "UberStrike_Data\\sharedassets7.assets");
		blueboxPath = Path.Combine(Directory.GetCurrentDirectory(), ApplicationDataManager.IsMac ? "Uberstrike.app/Contents/Data/sharedassets7.bluebox" : "UberStrike_Data\\sharedassets7.bluebox");
	}

	public IEnumerator StartGetHomeSound()
	{
		string url = ApplicationDataManager.IsMac ? ("file://" + Directory.GetCurrentDirectory() + "/Uberstrike.app/Contents/Data/SeloTron.ogg") : ("file:///" + Directory.GetCurrentDirectory().Replace("\\", "/") + "/UberStrike_Data/SeloTron.ogg");
		using (WWW request = new WWW(url))
		{
			while (!request.isDone)
			{
				yield return null;
			}
			if (request.isDone)
			{
				AudioClip audioClip2 = GameAudio.HomeSceneBackground = request.GetAudioClip(threeD: false);
				AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.HomeSceneBackground);
			}
		}
	}

	public static void BoxLoader(GameBoxType box)
	{
		if (File.Exists(destPath)) File.Delete(destPath);
		switch (box)
		{
			case GameBoxType.NORMAL:
				File.Copy(normalPath, destPath, true);
				break;
			case GameBoxType.BLUE:
				File.Copy(blueboxPath, destPath, true);
				break;
		}
	}

	public void Patch()
	{
		if (GameState.Current.RoomData.MapID == 13)
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				if (gameObject.name.Equals("Blue TDM 1") || gameObject.name.Equals("Blue TE 3") || gameObject.name.Equals("FFA 8"))
				{
					gameObject.transform.position = new Vector3(-55f, 9.1f, 47.3f);
				}
			}
		}
		else if (GameState.Current.RoomData.MapID == 14 && GameState.Current.GameMode == GameModeType.EliminationMode)
		{
			IList<SpawnPoint> list = new List<SpawnPoint>();
			foreach (SpawnPoint spawnPoint in Singleton<SpawnPointManager>.Instance.GetSpawnPointList(GameModeType.TeamDeathMatch, TeamID.RED))
			{
				spawnPoint.GameMode = GameMode.TeamElimination;
				list.Add(spawnPoint);
			}
			foreach (SpawnPoint spawnPoint2 in Singleton<SpawnPointManager>.Instance.GetSpawnPointList(GameModeType.TeamDeathMatch, TeamID.NONE))
			{
				spawnPoint2.GameMode = GameMode.TeamElimination;
				list.Add(spawnPoint2);
			}
			foreach (SpawnPoint spawnPoint3 in Singleton<SpawnPointManager>.Instance.GetSpawnPointList(GameModeType.TeamDeathMatch, TeamID.BLUE))
			{
				spawnPoint3.GameMode = GameMode.TeamElimination;
				list.Add(spawnPoint3);
			}
			Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(list.ToArray(), clear: false);
		}
	}

	public void FixPink(GameObject Prefab, BaseUberStrikeItemView View)
	{
		StartCoroutine(Pink(Prefab, View));
	}

	public static void KillTransform(ref Transform t1)
	{
		if (t1 != null)
		{
			t1.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(t1.gameObject);
		}
	}

	private static IEnumerator FindDeepChildEnum(Transform Parent, string aName, Action<Transform> result)
	{
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(Parent);
		Transform transform;
		while (true)
		{
			if (queue.Count > 0)
			{
				yield return null;
				transform = queue.Dequeue();
				if (transform.name == aName)
				{
					break;
				}
				foreach (Transform item in transform)
				{
					queue.Enqueue(item);
				}
				continue;
			}
			yield break;
		}
		result(transform);
	}

	public static IEnumerator Pink(GameObject Prefab, BaseUberStrikeItemView View)
	{
		if (View.ID == 151 || View.ID == 152)
		{
			Transform t2 = null;
			yield return UnityRuntime.StartRoutine(FindDeepChildEnum(Prefab.gameObject.transform, "Effects", delegate(Transform value)
			{
				t2 = value;
			}));
			KillTransform(ref t2);
		}
		if (View.ID == 77 || View.ID == 78 || View.ID == 79 || View.ID == 80)
		{
			Transform t = null;
			yield return UnityRuntime.StartRoutine(FindDeepChildEnum(Prefab.gameObject.transform, "PFXAWPFlare", delegate(Transform value)
			{
				t = value;
			}));
			KillTransform(ref t);
		}
		if (View.ID != 148)
		{
			yield break;
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Transform));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameobject = (GameObject)array[i];
			if (gameobject.name.Contains("DLMissile"))
			{
				foreach (GameObject item in gameobject.transform)
				{
					if (item.name.Equals("PFXConfetti"))
					{
						try
						{
							item.gameObject.SetActive(value: false);
							UnityEngine.Object.Destroy(gameobject);
						}
						catch
						{
						}
					}
					if (item.name.Equals("PFXFireWorks"))
					{
						try
						{
							item.gameObject.SetActive(value: false);
							UnityEngine.Object.Destroy(gameobject);
						}
						catch
						{
						}
					}
					if (item.name.Equals("PFXSmoke"))
					{
						try
						{
							item.gameObject.SetActive(value: false);
							UnityEngine.Object.Destroy(gameobject);
						}
						catch
						{
						}
					}
					yield return null;
				}
			}
			yield return null;
		}
	}

	private void OnApplicationQuit()
	{
		if (!ApplicationDataManager.IsMac)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = "cmd.exe";
			processStartInfo.Arguments = "/c taskkill /IM uberstrike.exe /F";
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
		}
	}
}
