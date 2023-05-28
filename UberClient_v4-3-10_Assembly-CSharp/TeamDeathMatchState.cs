// Decompiled with JetBrains decompiler
// Type: TeamDeathMatchState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

internal class TeamDeathMatchState : IState
{
  private const HudDrawFlags _hudDrawFlag = HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg;
  private TeamDeathMatchGameMode _teamDeathMatchGameMode;
  private InGameCountdown _ingameCountdown;
  private StateMachine _stateMachine;

  public TeamDeathMatchState()
  {
    this._ingameCountdown = new InGameCountdown();
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(19, (IState) new InGamePregameLoadoutState());
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
    this._stateMachine.RegisterState(21, (IState) new InGameEndOfMatchState());
    this._stateMachine.RegisterState(23, (IState) new InGamePlayerKilledState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg, true));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
    this._stateMachine.RegisterState(22, (IState) new InGameSpectatingState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
  }

  public GameMetaData GameMetaData { get; set; }

  public void OnEnter()
  {
    this._teamDeathMatchGameMode = this.GameMetaData != null ? new TeamDeathMatchGameMode(this.GameMetaData) : throw new NullReferenceException("Load team death match with invalid GameMetaData");
    GameModeUtil.OnEnterGameMode((FpsGameMode) this._teamDeathMatchGameMode);
    TabScreenPanelGUI.Instance.SortPlayersByRank = new Action<IEnumerable<CharacterInfo>>(TabScreenPlayerSorter.SortTeamMatchPlayers);
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
    CmuneEventHandler.AddListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.AddListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.AddListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.AddListener<OnUpdateTeamScoreEvent>(new Action<OnUpdateTeamScoreEvent>(this.OnUpdateTeamScore));
    CmuneEventHandler.AddListener<OnChangeTeamSuccessEvent>(new Action<OnChangeTeamSuccessEvent>(TeamGameModeUtil.OnChangeTeamSuccess));
    CmuneEventHandler.AddListener<OnChangeTeamFailEvent>(new Action<OnChangeTeamFailEvent>(TeamGameModeUtil.OnChangeTeamFail));
    CmuneEventHandler.AddListener<OnPlayerChangeTeamEvent>(new Action<OnPlayerChangeTeamEvent>(this.OnPlayerChangeTeam));
    this._stateMachine.SetState(19);
  }

  public void OnExit()
  {
    this._stateMachine.PopAllStates();
    CmuneEventHandler.RemoveListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.RemoveListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.RemoveListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.RemoveListener<OnUpdateTeamScoreEvent>(new Action<OnUpdateTeamScoreEvent>(this.OnUpdateTeamScore));
    CmuneEventHandler.RemoveListener<OnChangeTeamSuccessEvent>(new Action<OnChangeTeamSuccessEvent>(TeamGameModeUtil.OnChangeTeamSuccess));
    CmuneEventHandler.RemoveListener<OnChangeTeamFailEvent>(new Action<OnChangeTeamFailEvent>(TeamGameModeUtil.OnChangeTeamFail));
    CmuneEventHandler.RemoveListener<OnPlayerChangeTeamEvent>(new Action<OnPlayerChangeTeamEvent>(this.OnPlayerChangeTeam));
    GameModeUtil.OnExitGameMode();
    this._teamDeathMatchGameMode = (TeamDeathMatchGameMode) null;
  }

  public void OnUpdate()
  {
    Singleton<QuickItemController>.Instance.Update();
    if (this._teamDeathMatchGameMode.IsMatchRunning)
    {
      this._ingameCountdown.Update();
      TeamGameModeUtil.DetectTeamChange(this._teamDeathMatchGameMode);
      GameModeUtil.UpdatePlayerStateMsg((FpsGameMode) this._teamDeathMatchGameMode, true);
    }
    else
      this._ingameCountdown.Stop();
    this._stateMachine.Update();
  }

  public void OnGUI()
  {
  }

  private void OnModeStart(OnModeInitializedEvent ev)
  {
    Singleton<HudUtil>.Instance.SetPlayerTeam(GameState.LocalCharacter.TeamID);
    Singleton<GameModeObjectiveHud>.Instance.DisplayGameMode(GameMode.TeamDeathMatch);
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<MatchStatusHud>.Instance.RemainingSeconds = 0;
    Singleton<InGameHelpHud>.Instance.EnableChangeTeamHelp = true;
    Singleton<FrameRateHud>.Instance.Enable = true;
  }

  private void OnMatchStart(OnMatchStartEvent ev)
  {
    this._ingameCountdown.EndTime = ev.MatchEndServerTicks;
    this._stateMachine.SetState(18);
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<MatchStatusHud>.Instance.ResetKillsLeftAudio();
    Singleton<MatchStatusHud>.Instance.RemainingKills = this._teamDeathMatchGameMode.GameData.SplatLimit;
    Singleton<PlayerLeadStatus>.Instance.ResetPlayerLead();
    Singleton<HudUtil>.Instance.SetTeamScore(0, 0);
  }

  private void OnMatchEnd(OnMatchEndEvent ev)
  {
    this._stateMachine.SetState(21);
    int redTeamSplat = this._teamDeathMatchGameMode.RedTeamSplat;
    int blueTeamSplat = this._teamDeathMatchGameMode.BlueTeamSplat;
    if (redTeamSplat > blueTeamSplat)
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.RED);
    else if (redTeamSplat < blueTeamSplat)
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.BLUE);
    else
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.NONE);
  }

  private void OnUpdateTeamScore(OnUpdateTeamScoreEvent ev)
  {
    int redScore = ev.RedScore;
    int blueScore = ev.BlueScore;
    bool isLeading = ev.IsLeading;
    Singleton<MatchStatusHud>.Instance.RemainingKills = this.GameMetaData.SplatLimit - (GameState.LocalCharacter.TeamID != TeamID.RED ? blueScore : redScore);
    bool playAudio = !this._teamDeathMatchGameMode.IsGameAboutToEnd && blueScore != this.GameMetaData.SplatLimit && redScore != this.GameMetaData.SplatLimit && this._teamDeathMatchGameMode.RedTeamPlayerCount > 0 && this._teamDeathMatchGameMode.BlueTeamPlayerCount > 0;
    if (playAudio)
      Singleton<MatchStatusHud>.Instance.PlayKillsLeftAudio(this.GameMetaData.SplatLimit - Math.Max(redScore, blueScore));
    switch (GameState.LocalCharacter.TeamID)
    {
      case TeamID.BLUE:
        Singleton<PlayerLeadStatus>.Instance.PlayLeadAudio(blueScore, redScore, isLeading, playAudio);
        break;
      case TeamID.RED:
        Singleton<PlayerLeadStatus>.Instance.PlayLeadAudio(redScore, blueScore, isLeading, playAudio);
        break;
    }
    Singleton<HudUtil>.Instance.SetTeamScore(blueScore, redScore);
  }

  private void OnPlayerChangeTeam(OnPlayerChangeTeamEvent ev) => TeamGameModeUtil.OnPlayerChangeTeam(this._teamDeathMatchGameMode, ev.PlayerID, ev.PlayerInfo, ev.TargetTeamID);
}
