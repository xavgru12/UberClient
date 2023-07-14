// Decompiled with JetBrains decompiler
// Type: MapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
  private const float MaxBundleLoadProgress = 0.9f;
  private const float MaxSceneLoadProgress = 0.1f;
  private Dictionary<string, UberstrikeMap> _mapsByName = new Dictionary<string, UberstrikeMap>();
  private Dictionary<int, int> _itemRecommendationsPerMap = new Dictionary<int, int>();

  private MapManager() => this.Clear();

  public IEnumerable<UberstrikeMap> AllMaps => (IEnumerable<UberstrikeMap>) this._mapsByName.Values;

  public int Count => this._mapsByName.Count;

  public bool IsLoading { get; private set; }

  public float Progress { get; private set; }

  public bool IsSimulateWebplayer { get; private set; }

  public string SimulatedWebPlayerPath { get; private set; }

  public void SimulateWebplayer(string path)
  {
    this.IsSimulateWebplayer = true;
    this.SimulatedWebPlayerPath = path;
  }

  public IUnityItem GetRecommendedItem(string mapName)
  {
    int mapId;
    int itemId;
    if (this.TryGetMapId(mapName, out mapId) && this._itemRecommendationsPerMap.TryGetValue(mapId, out itemId))
      return Singleton<ItemManager>.Instance.GetItemInShop(itemId);
    IUnityItem unityItem;
    return Singleton<ItemManager>.Instance.TryGetDefaultItem(UberstrikeItemClass.WeaponMachinegun, out unityItem) ? unityItem : (IUnityItem) null;
  }

  public string GetMapDescription(int mapId)
  {
    UberstrikeMap mapWithId = this.GetMapWithId(mapId);
    return mapWithId != null ? mapWithId.Description : LocalizedStrings.None;
  }

  public string GetMapName(string name)
  {
    UberstrikeMap uberstrikeMap;
    return this._mapsByName.TryGetValue(name, out uberstrikeMap) ? uberstrikeMap.Name : LocalizedStrings.None;
  }

  public string GetMapName(int mapId)
  {
    UberstrikeMap mapWithId = this.GetMapWithId(mapId);
    return mapWithId != null ? mapWithId.Name : LocalizedStrings.None;
  }

  public UberstrikeMap GetMapWithId(int mapId)
  {
    foreach (UberstrikeMap mapWithId in this._mapsByName.Values)
    {
      if (mapWithId.Id == mapId)
        return mapWithId;
    }
    return (UberstrikeMap) null;
  }

  public bool IsBlueBox(int mapId)
  {
    UberstrikeMap mapWithId = this.GetMapWithId(mapId);
    return mapWithId != null && mapWithId.IsBluebox;
  }

  public bool HasMapWithId(int mapId) => this.GetMapWithId(mapId) != null;

  public UberstrikeMap AddMapView(MapView mapView, bool isVisible = true, bool isBuiltIn = false)
  {
    UberstrikeMap uberstrikeMap = new UberstrikeMap(mapView)
    {
      IsVisible = isVisible,
      IsBuiltIn = isBuiltIn
    };
    this._mapsByName[mapView.SceneName] = uberstrikeMap;
    return uberstrikeMap;
  }

  private void Clear()
  {
    this._mapsByName.Clear();
    this.AddMapView(new MapView()
    {
      Description = "Menu",
      DisplayName = "Menu",
      SceneName = "Menu",
      FileName = "Main.unity3d"
    }, false, true);
    this.AddMapView(new MapView()
    {
      Description = "Tutorial",
      DisplayName = "Tutorial",
      SceneName = "Tutorial",
      FileName = "Tutorial.unity3d"
    }, false, true);
  }

  public bool InitializeMapsToLoad(List<MapView> mapViews)
  {
    this.Clear();
    foreach (MapView mapView in mapViews)
      this.AddMapView(mapView);
    return this._mapsByName.Count > 0;
  }

  public void CancelLoadMap(UberstrikeMap map)
  {
    if (string.IsNullOrEmpty(map.View.FileName) || !(Singleton<SceneLoader>.Instance.CurrentScene != map.SceneName))
      return;
    AssetBundleLoader.CancelLoading(map.View.FileName);
  }

  public Coroutine LoadMap(UberstrikeMap map, Action OnMapLoaded) => MonoRoutine.Start(this.StartLoadingMap(map, OnMapLoaded));

  [DebuggerHidden]
  private IEnumerator StartLoadingMap(UberstrikeMap map, Action OnMapLoaded) => (IEnumerator) new MapManager.\u003CStartLoadingMap\u003Ec__Iterator71()
  {
    map = map,
    OnMapLoaded = OnMapLoaded,
    \u003C\u0024\u003Emap = map,
    \u003C\u0024\u003EOnMapLoaded = OnMapLoaded,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator LoadMapAssetbundle(string filename, string sceneName, Action<string> onError = null) => (IEnumerator) new MapManager.\u003CLoadMapAssetbundle\u003Ec__Iterator72()
  {
    filename = filename,
    onError = onError,
    sceneName = sceneName,
    \u003C\u0024\u003Efilename = filename,
    \u003C\u0024\u003EonError = onError,
    \u003C\u0024\u003EsceneName = sceneName,
    \u003C\u003Ef__this = this
  };

  internal bool TryGetMapId(string mapName, out int mapId)
  {
    foreach (UberstrikeMap uberstrikeMap in this._mapsByName.Values)
    {
      if (uberstrikeMap.SceneName == mapName)
      {
        mapId = uberstrikeMap.Id;
        return true;
      }
    }
    mapId = 0;
    return false;
  }

  internal void SetRecomendations(Dictionary<int, int> recommendationsPerMap) => this._itemRecommendationsPerMap = recommendationsPerMap;
}
