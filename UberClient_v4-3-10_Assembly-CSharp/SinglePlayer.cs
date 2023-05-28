// Decompiled with JetBrains decompiler
// Type: SinglePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
  private AvatarDecorator _decorator;
  private GameMode _gameMode = GameMode.DeathMatch;
  private TeamID _team;
  private int _spawnPoint;
  [SerializeField]
  private Transform _firstPersonWeapons;
  [SerializeField]
  private Transform _thirdPersonWeapons;

  public Transform FirstPersonWeapons => this._firstPersonWeapons;

  public Transform ThirdPersonWeapons => this._thirdPersonWeapons;

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new SinglePlayer.\u003CStart\u003Ec__Iterator2()
  {
    \u003C\u003Ef__this = this
  };

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 150f, 30f), "Game Modes:");
    if (GUI.Toggle(new Rect(160f, 10f, 60f, 30f), this._gameMode == GameMode.DeathMatch, "DM") && GUI.changed)
      this._gameMode = GameMode.DeathMatch;
    if (GUI.Toggle(new Rect(220f, 10f, 60f, 30f), this._gameMode == GameMode.TeamDeathMatch, "TDM") && GUI.changed)
      this._gameMode = GameMode.TeamDeathMatch;
    if (GUI.Toggle(new Rect(280f, 10f, 60f, 30f), this._gameMode == GameMode.TeamElimination, "TE") && GUI.changed)
      this._gameMode = GameMode.TeamElimination;
    GUI.Label(new Rect(10f, 40f, 150f, 30f), "Teams:");
    if (GUI.Toggle(new Rect(160f, 40f, 60f, 30f), this._team == TeamID.NONE, "NONE") && GUI.changed)
      this._team = TeamID.NONE;
    if (GUI.Toggle(new Rect(220f, 40f, 60f, 30f), this._team == TeamID.RED, "RED") && GUI.changed)
      this._team = TeamID.RED;
    if (GUI.Toggle(new Rect(280f, 40f, 60f, 30f), this._team == TeamID.BLUE, "BLUE") && GUI.changed)
      this._team = TeamID.BLUE;
    GUI.Label(new Rect(10f, 70f, 150f, 30f), "Points:");
    for (int index = 0; index < Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this._gameMode, this._team); ++index)
    {
      if (GUI.Toggle(new Rect((float) (160 + 30 * index), 70f, 30f, 20f), this._spawnPoint == index, string.Empty + (object) (index + 1)) && GUI.changed)
      {
        this._spawnPoint = index;
        this.Respawn();
      }
    }
    if (Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this._gameMode, this._team) == 0)
      GUI.Label(new Rect(160f, 70f, 200f, 20f), "No points found!");
    if (Screen.lockCursor || !GUI.Button(new Rect((float) (Screen.width / 2 - 100), (float) (Screen.height / 2), 200f, 30f), "CONTINUE"))
      return;
    GameState.LocalPlayer.UnPausePlayer();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.N))
    {
      this._spawnPoint = (this._spawnPoint + 1) % Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(this._gameMode, this._team);
      this.Respawn();
    }
    if (!(bool) (Object) this._decorator)
      return;
    this._decorator.transform.position = GameState.LocalCharacter.Position;
    this._decorator.transform.rotation = GameState.LocalCharacter.HorizontalRotation;
  }

  private void Respawn()
  {
    Vector3 position;
    Quaternion rotation;
    Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(this._spawnPoint, this._gameMode, this._team, out position, out rotation);
    GameState.LocalPlayer.SpawnPlayerAt(position, rotation);
  }
}
