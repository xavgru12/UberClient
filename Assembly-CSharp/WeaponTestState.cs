// Decompiled with JetBrains decompiler
// Type: WeaponTestState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class WeaponTestState : IState
{
  private WeaponTestMode _testGameMode;
  private StateMachine _stateMachine;
  private HudDrawFlags _hudDrawFlags = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.StateMsg;

  public WeaponTestState()
  {
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, this._hudDrawFlags));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
  }

  public int ItemId { get; set; }

  public void OnEnter()
  {
    CmuneEventHandler.AddListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    CmuneEventHandler.AddListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpause));
    this._testGameMode = new WeaponTestMode(GameConnectionManager.Rmi);
    GameState.CurrentGame = (FpsGameMode) this._testGameMode;
    Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(GameState.CurrentSpace.SpawnPoints.GetComponentsInChildren<SpawnPoint>(true));
    LevelCamera.Instance.SetLevelCamera(GameState.CurrentSpace.Camera, GameState.CurrentSpace.DefaultViewPoint.position, GameState.CurrentSpace.DefaultViewPoint.rotation);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    GameState.LocalPlayer.SetEnabled(true);
    Singleton<QuickItemController>.Instance.IsEnabled = true;
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = false;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = false;
    ProjectileManager.CreateContainer();
    this._testGameMode.InitializeMode();
    MenuPageManager.Instance.UnloadCurrentPage();
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<HudUtil>.Instance.SetPlayerTeam(TeamID.NONE);
    Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
    Singleton<FrameRateHud>.Instance.Enable = true;
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.StateMsg;
  }

  public void OnExit()
  {
    CmuneEventHandler.RemoveListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    CmuneEventHandler.RemoveListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpause));
    GameModeUtil.OnExitGameMode();
    this._testGameMode = (WeaponTestMode) null;
  }

  public void OnUpdate() => Singleton<QuickItemController>.Instance.Update();

  public void OnGUI()
  {
  }

  private void OnPlayerRespawn(OnPlayerRespawnEvent ev) => Singleton<WeaponController>.Instance.SetPickupWeapon(this.ItemId, false, true);

  private void OnPlayerPause(OnPlayerPauseEvent ev) => this._stateMachine.SetState(26);

  private void OnPlayerUnpause(OnPlayerUnpauseEvent ev) => this._stateMachine.SetState(18);
}
