using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class SpawnPointManager : Singleton<SpawnPointManager>
{
	private IDictionary<GameModeType, IDictionary<TeamID, IList<SpawnPoint>>> _spawnPointsDictionary;

	private SpawnPointManager()
	{
		_spawnPointsDictionary = new Dictionary<GameModeType, IDictionary<TeamID, IList<SpawnPoint>>>();
		foreach (int value in Enum.GetValues(typeof(GameModeType)))
		{
			_spawnPointsDictionary[(GameModeType)value] = new Dictionary<TeamID, IList<SpawnPoint>>
			{
				{
					TeamID.BLUE,
					new List<SpawnPoint>()
				},
				{
					TeamID.RED,
					new List<SpawnPoint>()
				},
				{
					TeamID.NONE,
					new List<SpawnPoint>()
				}
			};
		}
	}

	private void Clear()
	{
		foreach (int value in Enum.GetValues(typeof(GameModeType)))
		{
			_spawnPointsDictionary[(GameModeType)value][TeamID.NONE].Clear();
			_spawnPointsDictionary[(GameModeType)value][TeamID.BLUE].Clear();
			_spawnPointsDictionary[(GameModeType)value][TeamID.RED].Clear();
		}
	}

	private bool TryGetSpawnPointAt(int index, GameModeType gameMode, TeamID teamID, out SpawnPoint point)
	{
		point = ((index >= GetSpawnPointList(gameMode, teamID).Count) ? null : GetSpawnPointList(gameMode, teamID)[index]);
		return point != null;
	}

	private bool TryGetRandomSpawnPoint(GameModeType gameMode, TeamID teamID, out SpawnPoint point)
	{
		IList<SpawnPoint> spawnPointList = GetSpawnPointList(gameMode, teamID);
		point = ((spawnPointList.Count <= 0) ? null : spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)]);
		return point != null;
	}

	public IList<SpawnPoint> GetSpawnPointList(GameModeType gameMode, TeamID team)
	{
		if (gameMode == GameModeType.None)
		{
			return _spawnPointsDictionary[GameModeType.DeathMatch][TeamID.NONE];
		}
		return _spawnPointsDictionary[gameMode][team];
	}

	public void ConfigureSpawnPoints(SpawnPoint[] points, bool clear = true)
	{
		if (clear)
		{
			Clear();
		}
		foreach (SpawnPoint spawnPoint in points)
		{
			if (_spawnPointsDictionary.ContainsKey(spawnPoint.GameModeType))
			{
				_spawnPointsDictionary[spawnPoint.GameModeType][spawnPoint.TeamId].Add(spawnPoint);
			}
		}
	}

	public int GetSpawnPointCount(GameModeType gameMode, TeamID team)
	{
		return GetSpawnPointList(gameMode, team).Count;
	}

	public void GetAllSpawnPoints(GameModeType gameMode, TeamID team, out List<Vector3> positions, out List<byte> angles)
	{
		IList<SpawnPoint> spawnPointList = GetSpawnPointList(gameMode, team);
		positions = new List<Vector3>(spawnPointList.Count);
		angles = new List<byte>(spawnPointList.Count);
		foreach (SpawnPoint item in spawnPointList)
		{
			positions.Add(item.Position);
			List<byte> list = angles;
			Vector3 eulerAngles = item.transform.rotation.eulerAngles;
			list.Add(Conversion.Angle2Byte(eulerAngles.y));
		}
	}

	public void GetSpawnPointAt(int index, GameModeType gameMode, TeamID team, out Vector3 position, out Quaternion rotation)
	{
		if (gameMode == GameModeType.None)
		{
			gameMode = GameModeType.DeathMatch;
		}
		if (TryGetSpawnPointAt(index, gameMode, team, out SpawnPoint point))
		{
			position = point.transform.position;
			rotation = point.transform.rotation;
			return;
		}
		Debug.LogException(new Exception("No spawnpoints found at " + index.ToString() + " int list of length " + GetSpawnPointCount(gameMode, team).ToString()));
		if (!(GameState.Current.Map != null) || !(GameState.Current.Map.DefaultSpawnPoint != null))
		{
			position = new Vector3(0f, 10f, 0f);
		}
		else
		{
			position = GameState.Current.Map.DefaultSpawnPoint.position;
		}
		rotation = Quaternion.identity;
	}

	public void GetRandomSpawnPoint(GameModeType gameMode, TeamID team, out Vector3 position, out Quaternion rotation)
	{
		if (gameMode == GameModeType.None)
		{
			gameMode = GameModeType.DeathMatch;
		}
		IList<SpawnPoint> list = _spawnPointsDictionary[gameMode][team];
		if (list.Count > 0)
		{
			SpawnPoint spawnPoint = list[UnityEngine.Random.Range(0, list.Count)];
			position = spawnPoint.transform.position;
			rotation = spawnPoint.transform.rotation;
		}
		else
		{
			Debug.LogWarning("GetRandomSpawnPoint failed for " + team.ToString() + "/" + gameMode.ToString());
			position = Vector3.zero;
			rotation = Quaternion.identity;
		}
	}
}
