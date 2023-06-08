using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
	private Dictionary<string, UberstrikeMap> _mapsByName = new Dictionary<string, UberstrikeMap>();

	public IEnumerable<UberstrikeMap> AllMaps => _mapsByName.Values;

	public int Count => _mapsByName.Count;

	private MapManager()
	{
		Clear();
	}

	public string GetMapDescription(int mapId)
	{
		UberstrikeMap mapWithId = GetMapWithId(mapId);
		if (mapWithId != null)
		{
			return mapWithId.Description;
		}
		return LocalizedStrings.None;
	}

	public string GetMapName(string name)
	{
		if (_mapsByName.TryGetValue(name, out UberstrikeMap value))
		{
			return value.Name;
		}
		return LocalizedStrings.None;
	}

	public string GetMapName(int mapId)
	{
		UberstrikeMap mapWithId = GetMapWithId(mapId);
		if (mapWithId != null)
		{
			return mapWithId.Name;
		}
		return LocalizedStrings.None;
	}

	public string GetMapSceneName(int mapId)
	{
		UberstrikeMap mapWithId = GetMapWithId(mapId);
		if (mapWithId != null)
		{
			return mapWithId.SceneName;
		}
		return LocalizedStrings.None;
	}

	public UberstrikeMap GetMapWithId(int mapId)
	{
		foreach (UberstrikeMap value in _mapsByName.Values)
		{
			if (value.Id == mapId)
			{
				return value;
			}
		}
		return null;
	}

	public bool MapExistsWithId(int mapId)
	{
		return GetMapWithId(mapId) != null;
	}

	public bool HasMapWithId(int mapId)
	{
		return GetMapWithId(mapId) != null;
	}

	private UberstrikeMap AddMapView(MapView mapView, bool isVisible = true, bool isBuiltIn = false)
	{
		UberstrikeMap uberstrikeMap = new UberstrikeMap(mapView);
		uberstrikeMap.IsVisible = isVisible;
		uberstrikeMap.IsBuiltIn = isBuiltIn;
		UberstrikeMap uberstrikeMap2 = uberstrikeMap;
		if (_mapsByName.TryGetValue(mapView.SceneName, out UberstrikeMap value))
		{
			uberstrikeMap2.View.MapId = value.View.MapId;
			uberstrikeMap2.View.Settings = value.View.Settings;
			uberstrikeMap2.View.SupportedGameModes = value.View.SupportedGameModes;
		}
		_mapsByName[mapView.SceneName] = uberstrikeMap2;
		return uberstrikeMap2;
	}

	private void Clear()
	{
		_mapsByName.Clear();
		AddMapView(new MapView
		{
			Description = "Menu",
			DisplayName = "Menu",
			SceneName = "Menu"
		}, isVisible: false, isBuiltIn: true);
	}

	public bool InitializeMapsToLoad(List<MapView> mapViews)
	{
		Clear();
		foreach (MapView mapView in mapViews)
		{
			AddMapView(mapView);
		}
		return _mapsByName.Count > 0;
	}

	public void LoadMap(UberstrikeMap map, Action onSuccess)
	{
		PickupItem.Reset();
		Debug.LogWarning("Loading map: " + map.SceneName);
		if (CustomBundleMapManager.Instance.IsBundleMap(map.Id))
		{
			CustomBundleMapManager.LoadBundle(map.Id, delegate
			{
				Singleton<SceneLoader>.Instance.LoadLevel(map.SceneName, delegate
				{
					if (onSuccess != null)
					{
						onSuccess();
					}
					Debug.LogWarning("Finished Loading map");
				});
			});
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadLevel(map.SceneName, delegate
			{
				if (onSuccess != null)
				{
					onSuccess();
				}
				Debug.LogWarning("Finished Loading map");
			});
		}
	}

	public void LoadMapNext(string mapScene, Action onSuccess)
	{
		Singleton<SceneLoader>.Instance.LoadLevel(mapScene, delegate
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
			PopupSystem.ClearAll();
			Debug.LogWarning("Finished Loading map");
		});
	}

	public bool TryGetMapId(string mapName, out int mapId)
	{
		foreach (UberstrikeMap value in _mapsByName.Values)
		{
			if (value.SceneName == mapName)
			{
				mapId = value.Id;
				return true;
			}
		}
		mapId = 0;
		return false;
	}
}
