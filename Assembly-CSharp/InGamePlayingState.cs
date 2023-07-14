// Decompiled with JetBrains decompiler
// Type: InGamePlayingState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class InGamePlayingState : IState
{
  private const HudDrawFlags cameraZoomedDrawFlagTuning = ~HudDrawFlags.Weapons;
  private StateMachine _stateMachine;
  private KillComboCounter _killComboCounter;
  private HudDrawFlags _hudDrawFlag;

  public InGamePlayingState(StateMachine stateMachine, HudDrawFlags hudDrawFlag)
  {
    this._stateMachine = stateMachine;
    this._killComboCounter = new KillComboCounter();
    this._hudDrawFlag = hudDrawFlag;
  }

  public void OnEnter()
  {
    Singleton<PlayerLeadStatus>.Instance.ResetPlayerLead();
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = this._hudDrawFlag;
    this._killComboCounter.ResetCounter();
    Singleton<QuickItemController>.Instance.IsEnabled = true;
    CmuneEventHandler.AddListener<OnPlayerKillEnemyEvent>(new Action<OnPlayerKillEnemyEvent>(this.OnPlayerKillEnemy));
    CmuneEventHandler.AddListener<OnPlayerSuicideEvent>(new Action<OnPlayerSuicideEvent>(GameModeUtil.OnPlayerSuicide));
    CmuneEventHandler.AddListener<OnPlayerKilledEvent>(new Action<OnPlayerKilledEvent>(GameModeUtil.OnPlayerKilled));
    CmuneEventHandler.AddListener<OnPlayerDamageEvent>(new Action<OnPlayerDamageEvent>(GameModeUtil.OnPlayerDamage));
    CmuneEventHandler.AddListener<OnPlayerDeadEvent>(new Action<OnPlayerDeadEvent>(this.OnPlayerDead));
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPaused));
    CmuneEventHandler.AddListener<OnPlayerSpectatingEvent>(new Action<OnPlayerSpectatingEvent>(this.OnPlayerSpectating));
    CmuneEventHandler.AddListener<OnCameraZoomInEvent>(new Action<OnCameraZoomInEvent>(this.OnCameraZoomIn));
    CmuneEventHandler.AddListener<OnCameraZoomOutEvent>(new Action<OnCameraZoomOutEvent>(this.OnCameraZoomOut));
    if (GameState.LocalPlayer.IsGamePaused)
      this._stateMachine.PushState(26);
    Singleton<HudDrawFlagGroup>.Instance.RemoveFlag(~HudDrawFlags.Weapons);
  }

  public void OnExit()
  {
    Singleton<QuickItemController>.Instance.IsEnabled = false;
    CmuneEventHandler.RemoveListener<OnPlayerKillEnemyEvent>(new Action<OnPlayerKillEnemyEvent>(this.OnPlayerKillEnemy));
    CmuneEventHandler.RemoveListener<OnPlayerSuicideEvent>(new Action<OnPlayerSuicideEvent>(GameModeUtil.OnPlayerSuicide));
    CmuneEventHandler.RemoveListener<OnPlayerKilledEvent>(new Action<OnPlayerKilledEvent>(GameModeUtil.OnPlayerKilled));
    CmuneEventHandler.RemoveListener<OnPlayerDamageEvent>(new Action<OnPlayerDamageEvent>(GameModeUtil.OnPlayerDamage));
    CmuneEventHandler.RemoveListener<OnPlayerDeadEvent>(new Action<OnPlayerDeadEvent>(this.OnPlayerDead));
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPaused));
    CmuneEventHandler.RemoveListener<OnPlayerSpectatingEvent>(new Action<OnPlayerSpectatingEvent>(this.OnPlayerSpectating));
    CmuneEventHandler.RemoveListener<OnCameraZoomInEvent>(new Action<OnCameraZoomInEvent>(this.OnCameraZoomIn));
    CmuneEventHandler.RemoveListener<OnCameraZoomOutEvent>(new Action<OnCameraZoomOutEvent>(this.OnCameraZoomOut));
  }

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }

  private void OnPlayerKillEnemy(OnPlayerKillEnemyEvent ev)
  {
    this._killComboCounter.OnKillEnemy();
    GameModeUtil.OnPlayerKillEnemy(ev);
  }

  private void OnPlayerPaused(OnPlayerPauseEvent ev) => this._stateMachine.PushState(26);

  private void OnPlayerSpectating(OnPlayerSpectatingEvent ev) => this._stateMachine.SetState(22);

  private void OnPlayerDead(OnPlayerDeadEvent ev) => this._stateMachine.PushState(23);

  private void OnCameraZoomIn(OnCameraZoomInEvent ev) => Singleton<HudDrawFlagGroup>.Instance.AddFlag(~HudDrawFlags.Weapons);

  private void OnCameraZoomOut(OnCameraZoomOutEvent ev) => Singleton<HudDrawFlagGroup>.Instance.RemoveFlag(~HudDrawFlags.Weapons);
}
