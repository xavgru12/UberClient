// Decompiled with JetBrains decompiler
// Type: DeathMatchGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

[NetworkClass(101)]
public class DeathMatchGameMode : FpsGameMode
{
  public DeathMatchGameMode(GameMetaData gameData)
    : base(GameConnectionManager.Rmi, gameData)
  {
    Singleton<PlayerLeadStatus>.Instance.ResetPlayerLead();
  }

  [NetworkMethod(79)]
  protected void OnUpdateSplatCount(short myKills, short otherKills, bool isLeading)
  {
    GameState.LocalCharacter.Kills = myKills;
    CmuneEventHandler.Route((object) new OnUpdateDeathMatchScoreEvent()
    {
      MyScore = (int) myKills,
      OtherPlayerScore = (int) otherKills,
      IsLeading = isLeading
    });
  }

  protected override void OnEndOfMatch()
  {
    this.IsWaitingForSpawn = false;
    this.IsMatchRunning = false;
    this._stateInterpolator.Pause();
    GameState.LocalPlayer.Pause();
    GameState.IsReadyForNextGame = false;
    this.HideRemotePlayerHudFeedback();
  }
}
