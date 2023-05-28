// Decompiled with JetBrains decompiler
// Type: TeamEliminationGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[NetworkClass(106)]
public class TeamEliminationGameMode : TeamDeathMatchGameMode
{
  private Dictionary<int, UberStrike.Realtime.UnitySdk.CharacterInfo> _pendingAvatarLoadingJobs;

  public TeamEliminationGameMode(GameMetaData gameData)
    : base(gameData)
  {
    this._pendingAvatarLoadingJobs = new Dictionary<int, UberStrike.Realtime.UnitySdk.CharacterInfo>();
  }

  protected override void OnModeInitialized() => this.IsMatchRunning = true;

  public override void RespawnPlayer()
  {
    Singleton<PlayerSpectatorControl>.Instance.IsEnabled = false;
    this._stateInterpolator.Run();
    base.RespawnPlayer();
    if (this.Players.Count <= 1)
      return;
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.AfterRound);
  }

  protected override void OnSetNextSpawnPoint(int index, int coolDownTime)
  {
    this.LoadAllPendingAvatars();
    base.OnSetNextSpawnPoint(index, coolDownTime);
  }

  [NetworkMethod(58)]
  protected void OnTeamEliminationRoundEnd(int teamId)
  {
    this.IsMatchRunning = false;
    if (this.BlueTeamPlayerCount <= 0 || this.RedTeamPlayerCount <= 0)
      return;
    CmuneEventHandler.Route((object) new OnTeamEliminationRoundEndEvent()
    {
      WinTeamID = (TeamID) teamId
    });
  }

  [NetworkMethod(93)]
  protected void OnSetWaitingForPlayers()
  {
    this._stateInterpolator.Run();
    if (!GameState.LocalPlayer.IsGamePaused)
      AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
    if (this._isLocalAvatarLoaded)
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
    else
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Spectating);
    CmuneEventHandler.Route((object) new OnSetWaitingForPlayersEvent());
  }

  [NetworkMethod(92)]
  protected virtual void OnSyncRoundTime(int roundEndServerTicks)
  {
    this._roundStartTime = roundEndServerTicks - this.GameData.RoundTime * 1000;
    CmuneEventHandler.Route((object) new OnTeamEliminationSyncRoundTimeEvent()
    {
      RoundEndServerTicks = roundEndServerTicks
    });
  }

  protected override void OnMatchStart(int roundCount, int roundEndServerTicks)
  {
    this.IsMatchRunning = true;
    this._roundStartTime = roundEndServerTicks - this.GameData.RoundTime * 1000;
    this._stateInterpolator.Run();
    if (!GameState.LocalPlayer.IsGamePaused)
      AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
    if (this._isLocalAvatarLoaded)
    {
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
    }
    else
    {
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Spectating);
      GameState.LocalDecorator.HudInformation.Hide();
    }
    if (GameState.LocalCharacter.IsSpectator)
      this.LoadAllPendingAvatars();
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
      characterConfig.IsAnimationEnabled = true;
    CmuneEventHandler.Route((object) new OnTeamEliminationRoundStartEvent()
    {
      RoundCount = roundCount,
      RoundEndServerTicks = roundEndServerTicks
    });
  }

  [NetworkMethod(91)]
  protected void OnUpdateRoundStats(int round, int blueScore, int redScore)
  {
    this._blueTeamSplats = blueScore;
    this._redTeamSplats = redScore;
    CmuneEventHandler.Route((object) new OnUpdateRoundStatsEvent()
    {
      BlueWinRoundCount = blueScore,
      RedWinRoundCount = redScore,
      Round = round
    });
  }

  [NetworkMethod(51)]
  protected void OnPlayerSpectator(int actorId)
  {
    if (actorId == this.MyActorId)
    {
      Singleton<PlayerSpectatorControl>.Instance.IsEnabled = true;
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Spectating);
    }
    else
    {
      if (!Singleton<PlayerSpectatorControl>.Instance.IsEnabled || Singleton<PlayerSpectatorControl>.Instance.CurrentActorId != actorId)
        return;
      Singleton<PlayerSpectatorControl>.Instance.EnterFreeMoveMode();
    }
  }

  [NetworkMethod(52)]
  protected void OnLoadPendingAvatarOfPlayers(List<int> actorIDs)
  {
    foreach (int actorId in actorIDs)
    {
      if (this._pendingAvatarLoadingJobs.ContainsKey(actorId))
      {
        this.InstantiateCharacter(this._pendingAvatarLoadingJobs[actorId]);
        this._pendingAvatarLoadingJobs.Remove(actorId);
      }
    }
  }

  private void LoadAllPendingAvatars()
  {
    if (this._pendingAvatarLoadingJobs.Count <= 0)
      return;
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo info in this._pendingAvatarLoadingJobs.Values)
      this.InstantiateCharacter(info);
    this._pendingAvatarLoadingJobs.Clear();
  }

  [NetworkMethod(90)]
  protected void OnGraceTimeCountDown(int round, int duration)
  {
    CmuneEventHandler.Route((object) new OnGraceTimeCountdownEvent());
    this.EnableAllAvatarHudInfo(false);
  }

  protected override void OnNormalJoin(UberStrike.Realtime.UnitySdk.CharacterInfo player)
  {
    this._pendingAvatarLoadingJobs[player.ActorId] = player;
    if (player.ActorId != this.MyActorId)
      this._stateInterpolator.AddCharacterInfo(player);
    else
      this.SendMethodToServer((byte) 62, (object) this.MyActorId, (object) PickupItem.GetRespawnDurations());
  }

  protected override void OnPlayerLeft(int actorId)
  {
    if (this._pendingAvatarLoadingJobs.ContainsKey(actorId))
      this._pendingAvatarLoadingJobs.Remove(actorId);
    if (Singleton<PlayerSpectatorControl>.Instance.CurrentActorId == actorId && Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
      Singleton<PlayerSpectatorControl>.Instance.FollowNextPlayer();
    base.OnPlayerLeft(actorId);
  }

  private void EnableAllAvatarHudInfo(bool enabled)
  {
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
    {
      if ((bool) (Object) characterConfig.Decorator && (bool) (Object) characterConfig.Decorator.HudInformation)
        characterConfig.Decorator.HudInformation.ForceShowInformation = enabled;
    }
  }

  public override bool CanShowTabscreen => this.Players.Count > 0;

  public override bool IsWaitingForPlayers
  {
    get
    {
      if (!this.IsGameStarted)
        return false;
      return this.BlueTeamPlayerCount == 0 || this.RedTeamPlayerCount == 0;
    }
  }
}
