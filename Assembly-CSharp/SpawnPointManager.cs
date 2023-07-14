// Decompiled with JetBrains decompiler
// Type: SpawnPointManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class SpawnPointManager : Singleton<SpawnPointManager>
{
  private static readonly Vector3 DefaultSpawnPoint = new Vector3(0.0f, 6f, 0.0f);
  private IDictionary<GameMode, IDictionary<TeamID, IList<SpawnPoint>>> _spawnPointsDictionary;

  private SpawnPointManager()
  {
    this._spawnPointsDictionary = (IDictionary<GameMode, IDictionary<TeamID, IList<SpawnPoint>>>) new Dictionary<GameMode, IDictionary<TeamID, IList<SpawnPoint>>>();
    foreach (int key in Enum.GetValues(typeof (GameMode)))
      this._spawnPointsDictionary[(GameMode) key] = (IDictionary<TeamID, IList<SpawnPoint>>) new Dictionary<TeamID, IList<SpawnPoint>>()
      {
        {
          TeamID.BLUE,
          (IList<SpawnPoint>) new List<SpawnPoint>()
        },
        {
          TeamID.RED,
          (IList<SpawnPoint>) new List<SpawnPoint>()
        },
        {
          TeamID.NONE,
          (IList<SpawnPoint>) new List<SpawnPoint>()
        }
      };
  }

  private void Clear()
  {
    foreach (int num in Enum.GetValues(typeof (GameMode)))
    {
      GameMode key = (GameMode) num;
      this._spawnPointsDictionary[key][TeamID.NONE].Clear();
      this._spawnPointsDictionary[key][TeamID.BLUE].Clear();
      this._spawnPointsDictionary[key][TeamID.RED].Clear();
    }
  }

  private bool TryGetSpawnPointAt(
    int index,
    GameMode gameMode,
    TeamID teamID,
    out SpawnPoint point)
  {
    point = index >= this.GetSpawnPointList(gameMode, teamID).Count ? (SpawnPoint) null : this.GetSpawnPointList(gameMode, teamID)[index];
    return (UnityEngine.Object) point != (UnityEngine.Object) null;
  }

  private bool TryGetRandomSpawnPoint(GameMode gameMode, TeamID teamID, out SpawnPoint point)
  {
    IList<SpawnPoint> spawnPointList = this.GetSpawnPointList(gameMode, teamID);
    point = spawnPointList.Count <= 0 ? (SpawnPoint) null : spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)];
    return (UnityEngine.Object) point != (UnityEngine.Object) null;
  }

  private IList<SpawnPoint> GetSpawnPointList(GameMode gameMode, TeamID team) => gameMode == GameMode.Training ? this._spawnPointsDictionary[GameMode.DeathMatch][TeamID.NONE] : this._spawnPointsDictionary[gameMode][team];

  public void ConfigureSpawnPoints(SpawnPoint[] points)
  {
    this.Clear();
    foreach (SpawnPoint point in points)
      this._spawnPointsDictionary[point.GameMode][point.TeamId].Add(point);
  }

  public int GetSpawnPointCount(GameMode gameMode, TeamID team) => this.GetSpawnPointList(gameMode, team).Count;

  public void GetSpawnPointAt(
    int index,
    GameMode gameMode,
    TeamID team,
    out Vector3 position,
    out Quaternion rotation)
  {
    SpawnPoint point;
    if (this.TryGetSpawnPointAt(index, gameMode, team, out point))
    {
      position = point.transform.position;
      rotation = point.transform.rotation;
    }
    else
    {
      Debug.LogError((object) ("No spawnpoints found at " + (object) index + " int list of length " + (object) this.GetSpawnPointCount(gameMode, team)));
      position = SpawnPointManager.DefaultSpawnPoint;
      rotation = Quaternion.identity;
    }
  }

  public void GetRandomSpawnPoint(out Vector3 position, out Quaternion rotation)
  {
    IList<SpawnPoint> spawnPointList = this._spawnPointsDictionary[GameMode.DeathMatch][TeamID.NONE];
    SpawnPoint spawnPoint = spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)];
    position = spawnPoint.transform.position;
    rotation = spawnPoint.transform.rotation;
  }

  public List<SpawnPoint> GetAllSpawnPoints()
  {
    List<SpawnPoint> allSpawnPoints = new List<SpawnPoint>();
    foreach (IDictionary<TeamID, IList<SpawnPoint>> dictionary in (IEnumerable<IDictionary<TeamID, IList<SpawnPoint>>>) this._spawnPointsDictionary.Values)
    {
      foreach (IList<SpawnPoint> collection in (IEnumerable<IList<SpawnPoint>>) dictionary.Values)
        allSpawnPoints.AddRange((IEnumerable<SpawnPoint>) collection);
    }
    return allSpawnPoints;
  }
}
