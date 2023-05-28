// Decompiled with JetBrains decompiler
// Type: GearTestState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class GearTestState : IState
{
  private GearTestMode _testGameMode;
  private StateMachine _stateMachine;
  private HudDrawFlags _hudDrawFlags = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.StateMsg;
  private AvatarDecorator _testAvatar;

  public GearTestState()
  {
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, this._hudDrawFlags));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
  }

  public GearLoadout Loadout { get; set; }

  public void OnEnter()
  {
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    CmuneEventHandler.AddListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpause));
    this._testGameMode = new GearTestMode(GameConnectionManager.Rmi);
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
    this._testAvatar = Singleton<AvatarBuilder>.Instance.CreateRemoteAvatar(this.Loadout, Color.white);
    this._testAvatar.SetPosition(new Vector3(-18f, -2.35f, -2f), Quaternion.Euler(0.0f, 110f, 0.0f));
  }

  public void OnExit()
  {
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    CmuneEventHandler.RemoveListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpause));
    GameModeUtil.OnExitGameMode();
    this._testGameMode = (GearTestMode) null;
  }

  public void OnUpdate() => Singleton<QuickItemController>.Instance.Update();

  public void OnGUI()
  {
    if (!GUI.Button(new Rect((float) ((Screen.width - 100) / 2), (float) (Screen.height / 4 * 3), 100f, 60f), "Ragdoll", StormFront.ButtonBlue) || !(bool) (UnityEngine.Object) this._testAvatar)
      return;
    if ((bool) (UnityEngine.Object) this._testAvatar.CurrentRagdoll)
      this._testAvatar.DisableRagdoll();
    else
      this._testAvatar.SpawnDeadRagdoll(new DamageInfo((short) 999));
  }

  private void OnPlayerPause(OnPlayerPauseEvent ev) => this._stateMachine.SetState(26);

  private void OnPlayerUnpause(OnPlayerUnpauseEvent ev) => this._stateMachine.SetState(18);
}
