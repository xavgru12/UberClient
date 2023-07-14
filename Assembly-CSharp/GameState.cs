// Decompiled with JetBrains decompiler
// Type: GameState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
  public const float InterpolationFactor = 10f;
  public static bool IsRagdollShootable = false;
  [SerializeField]
  private LocalPlayer _player;
  private static FpsGameMode _currentGameMode;
  private static UberStrike.Realtime.UnitySdk.CharacterInfo _localCharacter = new UberStrike.Realtime.UnitySdk.CharacterInfo();

  public static event Action DrawGizmos;

  public static GameState Instance { get; private set; }

  public static LocalPlayer LocalPlayer => GameState.Instance._player;

  public static UberStrike.Realtime.UnitySdk.CharacterInfo LocalCharacter => GameState._localCharacter;

  public static AvatarDecorator LocalDecorator { get; set; }

  public static bool IsShuttingDown { get; private set; }

  public static bool UsePlayerPing => true;

  public static bool Exists => (UnityEngine.Object) GameState.Instance != (UnityEngine.Object) null;

  private void Awake() => GameState.Instance = this;

  private void Start()
  {
    this._player = UnityEngine.Object.Instantiate((UnityEngine.Object) this._player) as LocalPlayer;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this._player.gameObject);
  }

  private void FixedUpdate()
  {
    if (GameState.CurrentGame == null)
      return;
    GameState.CurrentGame.FixedUpdate();
  }

  private void Update()
  {
    if (GameState.CurrentGame != null)
      GameState.CurrentGame.Update();
    Singleton<GameStateController>.Instance.StateMachine.Update();
  }

  private void OnGUI() => Singleton<GameStateController>.Instance.StateMachine.OnGUI();

  private void OnDrawGizmos()
  {
    if (GameState.DrawGizmos == null)
      return;
    GameState.DrawGizmos();
  }

  private void OnApplicationQuit() => GameState.IsShuttingDown = true;

  public static bool IsReadyForNextGame { get; set; }

  public static Transform WeaponCameraTransform => GameState.LocalPlayer.WeaponCamera.transform;

  public static int CurrentPlayerID => GameState.LocalCharacter != null ? GameState.LocalCharacter.ActorId : 0;

  public static bool HasCurrentPlayer => GameState.Exists && (UnityEngine.Object) GameState.Instance._player != (UnityEngine.Object) null;

  public static GameMode CurrentGameMode => GameState.HasCurrentGame ? GameState.CurrentGame.GameMode : GameMode.None;

  public static bool IsSinglePlayer => !GameStateController.IsMultiplayerGameMode((int) GameState.CurrentGameMode);

  public static FpsGameMode CurrentGame
  {
    get => GameState._currentGameMode;
    set
    {
      if (GameState._currentGameMode != null)
        GameState._currentGameMode.Dispose();
      GameState._currentGameMode = value;
    }
  }

  public static bool HasCurrentGame => GameState._currentGameMode != null;

  public static MapConfiguration CurrentSpace { get; set; }

  public static bool HasCurrentSpace => (UnityEngine.Object) GameState.CurrentSpace != (UnityEngine.Object) null;
}
