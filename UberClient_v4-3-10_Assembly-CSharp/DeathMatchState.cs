// Decompiled with JetBrains decompiler
// Type: DeathMatchState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class DeathMatchState : IState
{
  private const HudDrawFlags _hudDrawFlag = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg;
  private DeathMatchGameMode _deathMatchGameMode;
  private InGameCountdown _ingameCountdown;
  private StateMachine _stateMachine;

  public DeathMatchState()
  {
    this._ingameCountdown = new InGameCountdown();
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(19, (IState) new InGamePregameLoadoutState());
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
    this._stateMachine.RegisterState(21, (IState) new InGameEndOfMatchState());
    this._stateMachine.RegisterState(23, (IState) new InGamePlayerKilledState(this._stateMachine, HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg, true));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
    this._stateMachine.RegisterState(22, (IState) new InGameSpectatingState(this._stateMachine, HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg));
  }

  public GameMetaData GameMetaData { get; set; }

  public void OnEnter()
  {
    this._deathMatchGameMode = this.GameMetaData != null ? new DeathMatchGameMode(this.GameMetaData) : throw new NullReferenceException("Load death match with invalid GameMetaData");
    GameModeUtil.OnEnterGameMode((FpsGameMode) this._deathMatchGameMode);
    TabScreenPanelGUI.Instance.SortPlayersByRank = new Action<IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>>(TabScreenPlayerSorter.SortDeathMatchPlayers);
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = true;
    Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
    CmuneEventHandler.AddListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.AddListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.AddListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.AddListener<OnUpdateDeathMatchScoreEvent>(new Action<OnUpdateDeathMatchScoreEvent>(this.OnUpdateScore));
    this._stateMachine.SetState(19);
  }

  public void OnExit()
  {
    this._stateMachine.PopAllStates();
    CmuneEventHandler.RemoveListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    CmuneEventHandler.RemoveListener<OnMatchStartEvent>(new Action<OnMatchStartEvent>(this.OnMatchStart));
    CmuneEventHandler.RemoveListener<OnMatchEndEvent>(new Action<OnMatchEndEvent>(this.OnMatchEnd));
    CmuneEventHandler.RemoveListener<OnUpdateDeathMatchScoreEvent>(new Action<OnUpdateDeathMatchScoreEvent>(this.OnUpdateScore));
    GameModeUtil.OnExitGameMode();
    this._deathMatchGameMode = (DeathMatchGameMode) null;
  }

  public void OnUpdate()
  {
    Singleton<QuickItemController>.Instance.Update();
    if (this._deathMatchGameMode.IsMatchRunning)
    {
      this._ingameCountdown.Update();
      GameModeUtil.UpdatePlayerStateMsg((FpsGameMode) this._deathMatchGameMode, true);
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
    Singleton<HudUtil>.Instance.SetPlayerTeam(TeamID.NONE);
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<MatchStatusHud>.Instance.RemainingSeconds = 0;
    Singleton<GameModeObjectiveHud>.Instance.DisplayGameMode(GameMode.DeathMatch);
    Singleton<InGameHelpHud>.Instance.EnableChangeTeamHelp = false;
    Singleton<FrameRateHud>.Instance.Enable = true;
  }

  private void OnMatchStart(OnMatchStartEvent ev)
  {
    this._ingameCountdown.EndTime = ev.MatchEndServerTicks;
    this._stateMachine.SetState(18);
    Singleton<MatchStatusHud>.Instance.RemainingKills = GameState.CurrentGame.GameData.SplatLimit;
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<MatchStatusHud>.Instance.ResetKillsLeftAudio();
  }

  private void OnMatchEnd(OnMatchEndEvent ev)
  {
    Singleton<PlayerLeadStatus>.Instance.OnDeathMatchOver();
    this._stateMachine.SetState(21);
  }

  private void OnUpdateScore(OnUpdateDeathMatchScoreEvent ev)
  {
    int myScore = ev.MyScore;
    int otherPlayerScore = ev.OtherPlayerScore;
    bool isLeading = ev.IsLeading;
    int num = this.GameMetaData.SplatLimit - Mathf.Max(myScore, otherPlayerScore);
    bool playAudio = !this._deathMatchGameMode.IsGameAboutToEnd && myScore != this.GameMetaData.SplatLimit && otherPlayerScore != this.GameMetaData.SplatLimit && this._deathMatchGameMode.PlayerCount > 1;
    if (num != Singleton<MatchStatusHud>.Instance.RemainingKills)
    {
      Singleton<MatchStatusHud>.Instance.RemainingKills = num;
      if (playAudio)
        Singleton<MatchStatusHud>.Instance.PlayKillsLeftAudio(this.GameMetaData.SplatLimit - Mathf.Max(otherPlayerScore, myScore));
    }
    Singleton<PlayerLeadStatus>.Instance.PlayLeadAudio(myScore, otherPlayerScore, isLeading, playAudio);
  }
}
