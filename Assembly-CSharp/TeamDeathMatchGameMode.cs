// Decompiled with JetBrains decompiler
// Type: TeamDeathMatchGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[NetworkClass(100)]
public class TeamDeathMatchGameMode : FpsGameMode
{
  protected int _redTeamPlayerCount;
  protected int _blueTeamPlayerCount;
  protected int _redTeamSplats;
  protected int _blueTeamSplats;
  protected bool _canChangeTeamInThisLife;

  public TeamDeathMatchGameMode(GameMetaData gameData)
    : base(GameConnectionManager.Rmi, gameData)
  {
  }

  [NetworkMethod(79)]
  protected virtual void OnUpdateSplatCount(int blueScore, int redScore, bool isLeading)
  {
    if (this._blueTeamSplats == blueScore && this._redTeamSplats == redScore)
      return;
    CmuneEventHandler.Route((object) new OnUpdateTeamScoreEvent()
    {
      BlueScore = blueScore,
      RedScore = redScore,
      IsLeading = isLeading
    });
    this._blueTeamSplats = blueScore;
    this._redTeamSplats = redScore;
  }

  [NetworkMethod(81)]
  protected void OnTeamBalanceUpdate(int blueCount, int redCount)
  {
    this._blueTeamPlayerCount = blueCount;
    this._redTeamPlayerCount = redCount;
  }

  public override void RespawnPlayer()
  {
    try
    {
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
      this.IsWaitingForSpawn = false;
      this._canChangeTeamInThisLife = true;
      Vector3 position;
      Quaternion rotation;
      Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(this._nextSpawnPoint, (GameMode) this.GameData.GameMode, GameState.LocalCharacter.TeamID, out position, out rotation);
      this.SpawnPlayerAt(position, rotation);
    }
    catch
    {
      Debug.LogError((object) string.Format("RespawnPlayer with GameState.LocalCharacter {0}", (object) CmunePrint.Properties((object) GameState.LocalCharacter)));
      throw;
    }
  }

  protected override void OnEndOfMatch()
  {
    this.IsWaitingForSpawn = false;
    this.IsMatchRunning = false;
    this._stateInterpolator.Pause();
    GameState.LocalPlayer.Pause(true);
    GameState.IsReadyForNextGame = false;
    this.HideRemotePlayerHudFeedback();
    CmuneEventHandler.Route((object) new TeamGameEndEvent()
    {
      RedTeamSplats = this._redTeamSplats,
      BlueTeamSplats = this._blueTeamSplats
    });
  }

  protected override void UpdatePlayerCounters()
  {
    this._redTeamPlayerCount = 0;
    this._blueTeamPlayerCount = 0;
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo in this.Players.Values)
    {
      if (characterInfo.TeamID == TeamID.RED)
        ++this._redTeamPlayerCount;
      else if (characterInfo.TeamID == TeamID.BLUE)
        ++this._blueTeamPlayerCount;
    }
  }

  [NetworkMethod(54)]
  protected void OnPlayerTeamChange(int playerID, byte teamId)
  {
    if (!GameState.HasCurrentGame)
      return;
    UberStrike.Realtime.UnitySdk.CharacterInfo playerWithId = GameState.CurrentGame.GetPlayerWithID(playerID);
    if (playerWithId == null)
      return;
    playerWithId.TeamID = (TeamID) teamId;
    this.UpdatePlayerCounters();
    CmuneEventHandler.Route((object) new OnPlayerChangeTeamEvent()
    {
      PlayerID = playerID,
      TargetTeamID = playerWithId.TeamID
    });
  }

  protected override void OnSplatGameEvent(
    int shooter,
    int target,
    byte weaponClass,
    byte bodyPart)
  {
    base.OnSplatGameEvent(shooter, target, weaponClass, bodyPart);
    if (target != this.MyActorId)
      return;
    this._canChangeTeamInThisLife = false;
  }

  public virtual void ChangeTeam()
  {
    if (Singleton<PlayerSpectatorControl>.Instance.IsEnabled && this.HasMyTeamMorePlayers)
    {
      CmuneEventHandler.Route((object) new OnChangeTeamSuccessEvent()
      {
        CurrentTeamID = GameState.LocalCharacter.TeamID
      });
      this.SendPlayerTeamChange();
    }
    else if (!this.HasMyTeamMorePlayers)
      CmuneEventHandler.Route((object) new OnChangeTeamFailEvent()
      {
        Reason = OnChangeTeamFailEvent.FailReason.CannotChangeToATeamWithEqual
      });
    else if (!this._canChangeTeamInThisLife)
    {
      CmuneEventHandler.Route((object) new OnChangeTeamFailEvent()
      {
        Reason = OnChangeTeamFailEvent.FailReason.OnlyOneTeamChangePerLife
      });
    }
    else
    {
      CmuneEventHandler.Route((object) new OnChangeTeamSuccessEvent()
      {
        CurrentTeamID = GameState.LocalCharacter.TeamID
      });
      this.SendPlayerTeamChange();
      if (!this._isLocalAvatarLoaded || !GameState.LocalCharacter.IsAlive)
        return;
      this._canChangeTeamInThisLife = false;
    }
  }

  protected bool HasMyTeamMorePlayers
  {
    get
    {
      if (GameState.LocalCharacter.TeamID == TeamID.RED && this._redTeamPlayerCount > this._blueTeamPlayerCount)
        return true;
      return GameState.LocalCharacter.TeamID == TeamID.BLUE && this._blueTeamPlayerCount > this._redTeamPlayerCount;
    }
  }

  public int BlueTeamPlayerCount => this._blueTeamPlayerCount;

  public int RedTeamPlayerCount => this._redTeamPlayerCount;

  public int RedTeamSplat => this._redTeamSplats;

  public int BlueTeamSplat => this._blueTeamSplats;

  public bool CanJoinBlueTeam => this._redTeamPlayerCount >= this._blueTeamPlayerCount;

  public bool CanJoinRedTeam => this._redTeamPlayerCount <= this._blueTeamPlayerCount;
}
