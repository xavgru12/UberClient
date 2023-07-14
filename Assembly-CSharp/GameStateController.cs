// Decompiled with JetBrains decompiler
// Type: GameStateController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameStateController : Singleton<GameStateController>
{
  private TrainingState _trainingState;
  private ShopTryWeaponState _shopTryWeaponState;
  private GearTestState _gearTestState;
  private WeaponTestState _weaponTestState;
  private DeathMatchState _deathMatchState;
  private TeamDeathMatchState _teamDeathMatchState;
  private TeamEliminationMatchState _teamEliminationMatchState;

  private GameStateController()
  {
    this.StateMachine = new StateMachine();
    this._trainingState = new TrainingState();
    this._shopTryWeaponState = new ShopTryWeaponState();
    this._gearTestState = new GearTestState();
    this._weaponTestState = new WeaponTestState();
    this._deathMatchState = new DeathMatchState();
    this._teamDeathMatchState = new TeamDeathMatchState();
    this._teamEliminationMatchState = new TeamEliminationMatchState();
    this.StateMachine.RegisterState(10, (IState) this._deathMatchState);
    this.StateMachine.RegisterState(11, (IState) this._teamDeathMatchState);
    this.StateMachine.RegisterState(12, (IState) this._teamEliminationMatchState);
    this.StateMachine.RegisterState(15, (IState) this._shopTryWeaponState);
    this.StateMachine.RegisterState(16, (IState) this._weaponTestState);
    this.StateMachine.RegisterState(17, (IState) this._gearTestState);
    this.StateMachine.RegisterState(14, (IState) this._trainingState);
    this.StateMachine.RegisterState(13, (IState) new TutorialState());
    CmuneEventHandler.AddListener<ShopTryEvent>(new Action<ShopTryEvent>(this.OnTryItem));
  }

  public StateMachine StateMachine { get; private set; }

  protected override void OnDispose() => CmuneEventHandler.RemoveListener<ShopTryEvent>(new Action<ShopTryEvent>(this.OnTryItem));

  public void CreateGame(
    UberstrikeMap map,
    string name = " ",
    string password = "",
    int timeMinutes = 0,
    int killLimit = 1,
    int playerLimit = 1,
    GameModeType mode = GameModeType.None,
    GameFlags.GAME_FLAGS flags = GameFlags.GAME_FLAGS.None)
  {
    GameMetaData data = new GameMetaData(0, name, Singleton<GameServerController>.Instance.SelectedServer == null ? string.Empty : Singleton<GameServerController>.Instance.SelectedServer.ConnectionString, map.Id, password, timeMinutes, playerLimit, mode.GetGameModeID());
    data.GameModifierFlags = (int) flags;
    data.SplatLimit = killLimit;
    data.Password = password;
    this.JoinGame(data);
  }

  public void JoinGame(GameMetaData data)
  {
    if (data != null)
    {
      UberstrikeMap mapWithId = Singleton<MapManager>.Instance.GetMapWithId(data.MapID);
      if (mapWithId == null)
        return;
      PickupItem.ResetInstanceCounter();
      Singleton<MapManager>.Instance.LoadMap(mapWithId, (Action) (() => this.CreateGame(data)));
    }
    else
      Debug.LogError((object) "JoinGame failed because GameMetaData is null");
  }

  public void LeaveGame()
  {
    this.UnloadGameMode();
    Singleton<SceneLoader>.Instance.LoadLevel("Menu");
  }

  private void CreateGame(GameMetaData game)
  {
    Singleton<AvatarBuilder>.Instance.UpdateLocalAvatar(Singleton<LoadoutManager>.Instance.GearLoadout);
    LobbyConnectionManager.Stop();
    if (!AutoMonoBehaviour<GameConnectionManager>.Instance.IsConnectedToServer(game.ServerConnection))
      GameConnectionManager.Stop();
    if (GameStateController.IsMultiplayerGameMode((int) game.GameMode))
      GameConnectionManager.Start(game);
    GameState.LocalPlayer.SetEnabled(true);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    this.LoadGameMode((GameMode) game.GameMode, game);
  }

  public void SpectateCurrentGame()
  {
    if (GameState.HasCurrentGame)
      ModeratorGameMode.ModerateGameMode(GameState.CurrentGame);
    else
      Debug.LogError((object) "SpectateCurrentGame: GameState doesn't has any game!");
  }

  public void LoadTryWeaponMode(int itemId = 0)
  {
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
    this._shopTryWeaponState.ItemId = itemId;
    this.StateMachine.SetState(15);
  }

  public void LoadTestWeaponMode(int itemId = 0)
  {
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
    this._shopTryWeaponState.ItemId = itemId;
    this.StateMachine.SetState(16);
  }

  public void LoadTestGearMode(GearLoadout gearLoadout)
  {
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Stop();
    this._gearTestState.Loadout = gearLoadout;
    this.StateMachine.SetState(17);
  }

  public static bool IsMultiplayerGameMode(int mode)
  {
    switch (mode)
    {
      case 100:
      case 101:
      case 106:
        return true;
      default:
        return false;
    }
  }

  public void LoadGameMode(GameMode mode, GameMetaData data = null)
  {
    switch (mode)
    {
      case GameMode.TeamDeathMatch:
        this._teamDeathMatchState.GameMetaData = data;
        this.StateMachine.SetState(11);
        break;
      case GameMode.DeathMatch:
        this._deathMatchState.GameMetaData = data;
        this.StateMachine.SetState(10);
        break;
      case GameMode.TeamElimination:
        this._teamEliminationMatchState.GameMetaData = data;
        this.StateMachine.SetState(12);
        break;
      case GameMode.Tutorial:
        this.StateMachine.SetState(13);
        break;
      case GameMode.Training:
        this._trainingState.MapId = data.MapID;
        this.StateMachine.SetState(14);
        break;
      default:
        throw new NotImplementedException("The Game mode " + (object) mode + " is not supported");
    }
  }

  public void UnloadGameMode() => this.StateMachine.PopAllStates();

  private void OnTryItem(ShopTryEvent ev)
  {
    if (ev.Item.ItemType == UberstrikeItemType.Weapon)
    {
      this.LoadTryWeaponMode(ev.Item.ItemId);
    }
    else
    {
      if (ev.Item.ItemType != UberstrikeItemType.Gear)
        return;
      Singleton<TemporaryLoadoutManager>.Instance.SetGearLoadout(InventoryManager.GetSlotTypeForGear(ev.Item as GearItem), (IUnityItem) (ev.Item as GearItem));
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.SetAnimationState(PageType.Shop, ev.Item.ItemClass);
      switch (ev.Item.ItemType)
      {
        case UberstrikeItemType.Weapon:
          CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
          {
            Area = LoadoutArea.Weapons
          });
          break;
        case UberstrikeItemType.Gear:
          CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
          {
            Area = LoadoutArea.Gear
          });
          break;
        case UberstrikeItemType.QuickUse:
          CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
          {
            Area = LoadoutArea.QuickItems
          });
          break;
      }
    }
  }
}
