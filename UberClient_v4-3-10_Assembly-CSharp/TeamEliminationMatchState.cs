// Decompiled with JetBrains decompiler
// Type: TeamEliminationMatchState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class TeamEliminationMatchState : IState
{
  private const HudDrawFlags _hudDrawFlag = HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg;
  private TeamEliminationGameMode _teamEliminationMatchGameMode;
  private InGameCountdown _ingameCountdown;
  private StateMachine _stateMachine;

  public TeamEliminationMatchState()
  {
    this._ingameCountdown = new InGameCountdown();
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(19, (IState) new InGamePregameLoadoutState());
    this._stateMachine.RegisterState(25, (IState) new InGameGraceCountdownState());
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
    this._stateMachine.RegisterState(21, (IState) new InGameEndOfMatchState());
    this._stateMachine.RegisterState(23, (IState) new InGamePlayerKilledState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg, true));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
    this._stateMachine.RegisterState(22, (IState) new InGameSpectatingState(this._stateMachine, HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
  }

  public GameMetaData GameMetaData { get; set; }

  public void OnEnter()
  {
    this._teamEliminationMatchGameMode = this.GameMetaData != null ? new TeamEliminationGameMode(this.GameMetaData) : throw new NullReferenceException("Load team elimination match with invalid GameMetaData");
    GameModeUtil.OnEnterGameMode((FpsGameMode) this._teamEliminationMatchGameMode);
    TabScreenPanelGUI.Instance.SortPlayersByRank = new Action<IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>>(TabScreenPlayerSorter.SortTeamMatchPlayers);
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
    CmuneEventHandler.AddListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.AddListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.AddListener<OnUpdateRoundStatsEvent>(new Action<OnUpdateRoundStatsEvent>(this.OnUpdateRoundStats));
    CmuneEventHandler.AddListener<OnGraceTimeCountdownEvent>(new Action<OnGraceTimeCountdownEvent>(this.OnGraceTimeCountdown));
    CmuneEventHandler.AddListener<OnTeamEliminationRoundStartEvent>(new Action<OnTeamEliminationRoundStartEvent>(this.OnRoundStart));
    CmuneEventHandler.AddListener<OnTeamEliminationRoundEndEvent>(new Action<OnTeamEliminationRoundEndEvent>(this.OnRoundEnd));
    CmuneEventHandler.AddListener<OnTeamEliminationSyncRoundTimeEvent>(new Action<OnTeamEliminationSyncRoundTimeEvent>(this.OnSyncRoundTime));
    CmuneEventHandler.AddListener<OnSetWaitingForPlayersEvent>(new Action<OnSetWaitingForPlayersEvent>(this.OnSetWatingForPlayers));
    CmuneEventHandler.AddListener<OnChangeTeamSuccessEvent>(new Action<OnChangeTeamSuccessEvent>(TeamGameModeUtil.OnChangeTeamSuccess));
    CmuneEventHandler.AddListener<OnChangeTeamFailEvent>(new Action<OnChangeTeamFailEvent>(TeamGameModeUtil.OnChangeTeamFail));
    CmuneEventHandler.AddListener<OnPlayerChangeTeamEvent>(new Action<OnPlayerChangeTeamEvent>(this.OnPlayerChangeTeam));
    this._stateMachine.SetState(19);
  }

  public void OnExit()
  {
    this._stateMachine.PopAllStates();
    CmuneEventHandler.RemoveListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.RemoveListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.RemoveListener<OnUpdateRoundStatsEvent>(new Action<OnUpdateRoundStatsEvent>(this.OnUpdateRoundStats));
    CmuneEventHandler.RemoveListener<OnGraceTimeCountdownEvent>(new Action<OnGraceTimeCountdownEvent>(this.OnGraceTimeCountdown));
    CmuneEventHandler.RemoveListener<OnTeamEliminationRoundStartEvent>(new Action<OnTeamEliminationRoundStartEvent>(this.OnRoundStart));
    CmuneEventHandler.RemoveListener<OnTeamEliminationRoundEndEvent>(new Action<OnTeamEliminationRoundEndEvent>(this.OnRoundEnd));
    CmuneEventHandler.RemoveListener<OnTeamEliminationSyncRoundTimeEvent>(new Action<OnTeamEliminationSyncRoundTimeEvent>(this.OnSyncRoundTime));
    CmuneEventHandler.RemoveListener<OnSetWaitingForPlayersEvent>(new Action<OnSetWaitingForPlayersEvent>(this.OnSetWatingForPlayers));
    CmuneEventHandler.RemoveListener<OnChangeTeamSuccessEvent>(new Action<OnChangeTeamSuccessEvent>(TeamGameModeUtil.OnChangeTeamSuccess));
    CmuneEventHandler.RemoveListener<OnChangeTeamFailEvent>(new Action<OnChangeTeamFailEvent>(TeamGameModeUtil.OnChangeTeamFail));
    CmuneEventHandler.RemoveListener<OnPlayerChangeTeamEvent>(new Action<OnPlayerChangeTeamEvent>(this.OnPlayerChangeTeam));
    GameModeUtil.OnExitGameMode();
    this._teamEliminationMatchGameMode = (TeamEliminationGameMode) null;
  }

  public void OnUpdate()
  {
    Singleton<QuickItemController>.Instance.Update();
    if (this._teamEliminationMatchGameMode.IsMatchRunning)
    {
      this._ingameCountdown.Update();
      TeamGameModeUtil.DetectTeamChange((TeamDeathMatchGameMode) this._teamEliminationMatchGameMode);
      GameModeUtil.UpdatePlayerStateMsg((FpsGameMode) this._teamEliminationMatchGameMode, true);
      this.UpdateSpectatorStateMsg();
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
    this._stateMachine.SetState(18);
    Singleton<HudUtil>.Instance.SetPlayerTeam(GameState.LocalCharacter.TeamID);
    Singleton<GameModeObjectiveHud>.Instance.DisplayGameMode(GameMode.TeamElimination);
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<MatchStatusHud>.Instance.RemainingSeconds = 0;
    Singleton<InGameHelpHud>.Instance.EnableChangeTeamHelp = true;
    Singleton<HudUtil>.Instance.SetTeamScore(0, 0);
    Singleton<PlayerLeadStatus>.Instance.ResetPlayerLead();
    Singleton<MatchStatusHud>.Instance.RemainingRoundsText = string.Empty;
    GlobalUIRibbon.Instance.Hide();
    GameState.LocalPlayer.UnPausePlayer();
  }

  private void OnSetWatingForPlayers(OnSetWaitingForPlayersEvent ev)
  {
    Singleton<PlayerStateMsgHud>.Instance.DisplayWaitingForOtherPlayerMsg();
    this._stateMachine.SetState(18);
  }

  private void OnMatchEnd(OnMatchEndEvent ev)
  {
    this._stateMachine.SetState(21);
    int redTeamSplat = this._teamEliminationMatchGameMode.RedTeamSplat;
    int blueTeamSplat = this._teamEliminationMatchGameMode.BlueTeamSplat;
    if (redTeamSplat > blueTeamSplat)
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.RED);
    else if (redTeamSplat < blueTeamSplat)
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.BLUE);
    else
      Singleton<PopupHud>.Instance.PopupWinTeam(TeamID.NONE);
  }

  private void OnUpdateRoundStats(OnUpdateRoundStatsEvent ev)
  {
    Singleton<MatchStatusHud>.Instance.RemainingRoundsText = this.GetRoundStatus(out Color _);
    Singleton<HudUtil>.Instance.SetTeamScore(ev.BlueWinRoundCount, ev.RedWinRoundCount);
  }

  private void OnGraceTimeCountdown(OnGraceTimeCountdownEvent ev) => this._stateMachine.SetState(25);

  private void OnRoundStart(OnTeamEliminationRoundStartEvent ev)
  {
    Singleton<QuickItemController>.Instance.Restriction.RenewRoundUses();
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    Singleton<ProjectileManager>.Instance.ClearAll();
    this._ingameCountdown.EndTime = ev.RoundEndServerTicks;
    if (Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
      this._stateMachine.SetState(22);
    else
      this._stateMachine.SetState(18);
  }

  private void OnRoundEnd(OnTeamEliminationRoundEndEvent ev)
  {
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.AfterRound);
    Singleton<QuickItemController>.Instance.IsEnabled = false;
    Singleton<PopupHud>.Instance.PopupWinTeam(ev.WinTeamID);
    Singleton<ProjectileManager>.Instance.ClearAll();
  }

  private void OnSyncRoundTime(OnTeamEliminationSyncRoundTimeEvent ev) => this._ingameCountdown.EndTime = ev.RoundEndServerTicks;

  private void OnPlayerChangeTeam(OnPlayerChangeTeamEvent ev) => TeamGameModeUtil.OnPlayerChangeTeam((TeamDeathMatchGameMode) this._teamEliminationMatchGameMode, ev.PlayerID, ev.PlayerInfo, ev.TargetTeamID);

  private string GetRoundStatus(out Color color)
  {
    color = Color.white;
    int redTeamSplat = this._teamEliminationMatchGameMode.RedTeamSplat;
    int blueTeamSplat = this._teamEliminationMatchGameMode.BlueTeamSplat;
    bool flag1 = redTeamSplat == this.GameMetaData.SplatLimit - 1;
    bool flag2 = blueTeamSplat == this.GameMetaData.SplatLimit - 1;
    if (flag2 && flag1)
      return LocalizedStrings.FinalRoundCaps;
    if (flag2)
    {
      color = ColorScheme.HudTeamBlue;
      return string.Format(LocalizedStrings.FinalRoundX, (object) LocalizedStrings.BlueCaps);
    }
    if (!flag1)
      return string.Format(LocalizedStrings.NRoundsLeft, (object) (this.GameMetaData.SplatLimit - Mathf.Max(blueTeamSplat, redTeamSplat)));
    color = ColorScheme.HudTeamRed;
    return string.Format(LocalizedStrings.FinalRoundX, (object) LocalizedStrings.RedCaps);
  }

  private void UpdateSpectatorStateMsg()
  {
    if (GlobalUIRibbon.Instance.IsEnabled && Screen.lockCursor)
      Screen.lockCursor = false;
    if (this._teamEliminationMatchGameMode.IsWaitingForPlayers)
      Singleton<PlayerStateMsgHud>.Instance.DisplayWaitingForOtherPlayerMsg();
    else if (Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
    {
      if (Singleton<PlayerSpectatorControl>.Instance.IsFollowingPlayer)
        Singleton<PlayerStateMsgHud>.Instance.DisplaySpectatorFollowingMsg(this._teamEliminationMatchGameMode.GetPlayerWithID(Singleton<PlayerSpectatorControl>.Instance.CurrentActorId));
      else
        Singleton<PlayerStateMsgHud>.Instance.DisplaySpectatorModeMsg();
      if (!GameState.LocalPlayer.IsGamePaused)
        return;
      Singleton<HudUtil>.Instance.ShowContinueButton();
    }
    else
      Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
  }
}
