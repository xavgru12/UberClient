// Decompiled with JetBrains decompiler
// Type: TutorialState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class TutorialState : IState
{
  private TutorialGameMode _tutorialGameMode;
  private StateMachine _stateMachine;

  public TutorialState()
  {
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
  }

  public void OnEnter()
  {
    CmuneEventHandler.AddListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.AddListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    this._tutorialGameMode = new TutorialGameMode(GameConnectionManager.Rmi);
    GameState.CurrentGame = (FpsGameMode) this._tutorialGameMode;
    TabScreenPanelGUI.Instance.SetGameName("Tutorial");
    TabScreenPanelGUI.Instance.SetServerName(string.Empty);
    LevelCamera.Instance.SetLevelCamera(GameState.CurrentSpace.Camera, GameState.CurrentSpace.DefaultViewPoint.position, GameState.CurrentSpace.DefaultViewPoint.rotation);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    GameState.LocalPlayer.SetEnabled(true);
    Singleton<FrameRateHud>.Instance.Enable = true;
    Singleton<HudUtil>.Instance.SetPlayerTeam(TeamID.NONE);
    Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
  }

  public void OnExit()
  {
    CmuneEventHandler.RemoveListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.RemoveListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    GameModeUtil.OnExitGameMode();
    this._tutorialGameMode = (TutorialGameMode) null;
  }

  public void OnUpdate()
  {
    if (!this._tutorialGameMode.IsMatchRunning)
      return;
    GameModeUtil.UpdatePlayerStateMsg((FpsGameMode) this._tutorialGameMode, false);
  }

  public void OnGUI() => this._tutorialGameMode.DrawGui();

  private void OnMatchStart(OnMatchStartEvent ev) => Singleton<HudUtil>.Instance.ClearAllFeedbackHud();

  private void OnPlayerRespawn(OnPlayerRespawnEvent ev)
  {
    GamePageManager.Instance.UnloadCurrentPage();
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.None);
    if (this._tutorialGameMode.Sequence.State == TutorialCinematicSequence.TutorialState.AirlockMouseLookSubtitle)
      GameState.LocalPlayer.IsWalkingEnabled = false;
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.XpPoints;
  }

  private void OnPlayerPause(OnPlayerPauseEvent ev) => this._stateMachine.PushState(26);
}
