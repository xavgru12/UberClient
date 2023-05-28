// Decompiled with JetBrains decompiler
// Type: DebugSpawnPoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugSpawnPoints : IDebugPage
{
  private Vector2 scroll1;
  private Vector2 scroll2;
  private Vector2 scroll3;
  private GameMode gameMode;

  public string Title => "Spawn";

  public void Draw()
  {
    GUILayout.BeginHorizontal();
    foreach (int num in Enum.GetValues(typeof (GameMode)))
    {
      GameMode gameMode = (GameMode) num;
      if (GUILayout.Button(gameMode.ToString()))
        this.gameMode = gameMode;
    }
    GUILayout.EndHorizontal();
    if (GameState.HasCurrentGame)
      GUILayout.Label("CurrentSpawnPoint " + (object) GameState.CurrentGame.GameMode + " - " + (object) GameState.CurrentGame.CurrentSpawnPoint);
    else
      GUILayout.Label("CurrentSpawnPoint - no game mode running");
    GUILayout.BeginHorizontal();
    this.scroll1 = GUILayout.BeginScrollView(this.scroll1);
    GUILayout.Label(((Enum) TeamID.NONE).ToString());
    Vector3 position;
    Quaternion rotation;
    for (int index = 0; index < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this.gameMode, TeamID.NONE); ++index)
    {
      Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(index, this.gameMode, TeamID.NONE, out position, out rotation);
      GUILayout.Label(index.ToString() + ": " + (object) position);
    }
    GUILayout.EndScrollView();
    this.scroll2 = GUILayout.BeginScrollView(this.scroll2);
    GUILayout.Label(((Enum) TeamID.BLUE).ToString());
    for (int index = 0; index < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this.gameMode, TeamID.BLUE); ++index)
    {
      Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(index, this.gameMode, TeamID.BLUE, out position, out rotation);
      GUILayout.Label(index.ToString() + ": " + (object) position);
    }
    GUILayout.EndScrollView();
    this.scroll3 = GUILayout.BeginScrollView(this.scroll3);
    GUILayout.Label(((Enum) TeamID.RED).ToString());
    for (int index = 0; index < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this.gameMode, TeamID.RED); ++index)
    {
      Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(index, this.gameMode, TeamID.RED, out position, out rotation);
      GUILayout.Label(index.ToString() + ": " + (object) position);
    }
    GUILayout.EndScrollView();
    GUILayout.EndHorizontal();
  }
}
