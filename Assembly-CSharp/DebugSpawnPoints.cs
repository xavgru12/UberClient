using System;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class DebugSpawnPoints : IDebugPage
{
	private Vector2 scroll1;

	private Vector2 scroll2;

	private Vector2 scroll3;

	private GameModeType gameMode;

	public string Title => "Spawn";

	public void Draw()
	{
		GUILayout.BeginHorizontal();
		foreach (int value in Enum.GetValues(typeof(GameModeType)))
		{
			GameModeType gameModeType = (GameModeType)value;
			if (GUILayout.Button(gameModeType.ToString()))
			{
				gameMode = (GameModeType)value;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		scroll1 = GUILayout.BeginScrollView(scroll1);
		GUILayout.Label(TeamID.NONE.ToString());
		Vector3 position;
		Quaternion rotation;
		for (int i = 0; i < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.NONE); i++)
		{
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(i, gameMode, TeamID.NONE, out position, out rotation);
			string str = i.ToString();
			Vector3 vector = position;
			GUILayout.Label(str + ": " + vector.ToString());
		}
		GUILayout.EndScrollView();
		scroll2 = GUILayout.BeginScrollView(scroll2);
		GUILayout.Label(TeamID.BLUE.ToString());
		for (int j = 0; j < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.BLUE); j++)
		{
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(j, gameMode, TeamID.BLUE, out position, out rotation);
			string str2 = j.ToString();
			Vector3 vector = position;
			GUILayout.Label(str2 + ": " + vector.ToString());
		}
		GUILayout.EndScrollView();
		scroll3 = GUILayout.BeginScrollView(scroll3);
		GUILayout.Label(TeamID.RED.ToString());
		for (int k = 0; k < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(gameMode, TeamID.RED); k++)
		{
			Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(k, gameMode, TeamID.RED, out position, out rotation);
			string str3 = k.ToString();
			Vector3 vector = position;
			GUILayout.Label(str3 + ": " + vector.ToString());
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
	}
}
