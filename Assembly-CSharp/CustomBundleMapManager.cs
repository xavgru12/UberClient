using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class CustomBundleMapManager : MonoBehaviour
{
	public static CustomBundleMapManager Instance;

	private static List<CustomUberstikeMap> Maps;

	private static AssetBundle bundle;

	public List<MapView> DebugMaps;

	private bool isDebug;

	public static bool isLoading;

	public void Awake()
	{
		Instance = this;
		Maps = new List<CustomUberstikeMap>();
		DebugMaps = new List<MapView>();
		Caching.CleanCache();
	}

	public void Start()
	{
		List<GameModeType> list = new List<GameModeType>();
		list.Add(GameModeType.DeathMatch);
		List<GameModeType> list2 = new List<GameModeType>();
		list2.Add(GameModeType.DeathMatch);
		list2.Add(GameModeType.EliminationMode);
		list2.Add(GameModeType.TeamDeathMatch);
		InitializeCustomUberstikeMap("SpacePortAlpha", 19, list);
		InitializeCustomUberstikeMap("SpaceCity", 17, list2);
		InitializeCustomUberstikeMap("UberZone", 18, list2);
		InitializeCustomUberstikeMap("SkyGarden", 3, list2);
	}

	private void AddMap(CustomUberstikeMap uberstrikeMap)
	{
		if (!Maps.Contains(uberstrikeMap))
		{
			Maps.Add(uberstrikeMap);
		}
	}

	public static bool IsSupportedGameMode(GameModeType mode, int mapID)
	{
		CustomUberstikeMap map = GetMap(mapID);
		if (map == null)
		{
			return true;
		}
		if (map.SupportedModes.Contains(mode))
		{
			return true;
		}
		return false;
	}

	private void AddLocalExploreWebServiceMap(string displayName, string Scene, int mapid)
	{
		MapView item = new MapView
		{
			Description = "Bundle Dev Test Map",
			DisplayName = displayName,
			SceneName = Scene,
			MapId = mapid,
			BoxType = GameBoxType.NORMAL
		};
		DebugMaps.Add(item);
	}

	private void InitializeCustomUberstikeMap(string fileName, int ID, List<GameModeType> modes)
	{
		CustomUberstikeMap uberstrikeMap = new CustomUberstikeMap(fileName, ID, modes);
		AddMap(uberstrikeMap);
	}

	public static void LoadBundle(int mapid, Action onSuccess = null)
	{
		if (!isLoading)
		{
			try
			{
				bundle.Unload(unloadAllLoadedObjects: true);
			}
			catch
			{
			}
			UnityRuntime.StartRoutine(LoadBundleMap(mapid, onSuccess));
		}
	}

	public static IEnumerator LoadBundleMap(int id, Action onSuccess)
	{
		isLoading = true;
		CustomUberstikeMap map = GetMap(id);
		if (map == null)
		{
			PopupSystem.ShowMessage("Bundle Error", "Map you trying to load doesn't exist. MapID: " + id.ToString());
			yield break;
		}
		string path = GetBundlePath(map.FileName);
		using (WWW loader = WWW.LoadFromCacheOrDownload(path, 1))
		{
			yield return loader;
			if (!string.IsNullOrEmpty(loader.error))
			{
				PopupSystem.ClearAll();
				PopupSystem.ShowMessage("Error", "Failed to locate bundle files");
				Debug.LogError("Failed to locate Asset " + path + ". Error" + loader.error);
				yield break;
			}
			bundle = loader.assetBundle;
			bundle.LoadAll();
		}
		onSuccess?.Invoke();
		isLoading = false;
	}

	public bool IsBundleMap(int mapid)
	{
		foreach (CustomUberstikeMap map in Maps)
		{
			if (map.MapID.Equals(mapid))
			{
				return true;
			}
		}
		return false;
	}

	private static CustomUberstikeMap GetMap(int id)
	{
		foreach (CustomUberstikeMap map in Maps)
		{
			if (map.MapID.Equals(id))
			{
				return map;
			}
		}
		return null;
	}

	private void DevLog(string message)
	{
		if (isDebug)
		{
			PopupSystem.ShowMessage("Dev Info", message, PopupSystem.AlertType.OK);
		}
	}

	private static string GetBundlePath(string file)
	{
		if (GameState.Current.RoomData.BoxType == GameBoxType.BLUE)
		{
			file += "B";
		}
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		string result = string.Empty;
		if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			result = Path.Combine("file://" + directoryName + "/Contents/Data", file + ".unity3d");
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			result = Path.Combine("file:///" + directoryName + "//UberStrike_Data//", file + ".unity3d");
		}
		return result;
	}
}
