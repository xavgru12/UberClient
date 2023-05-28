// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.IGameRoomEventsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum IGameRoomEventsType
  {
    PowerUpPicked = 12, // 0x0000000C
    SetPowerupState = 13, // 0x0000000D
    ResetAllPowerups = 14, // 0x0000000E
    DoorOpen = 15, // 0x0000000F
    DisconnectCountdown = 16, // 0x00000010
    MatchStartCountdown = 17, // 0x00000011
    MatchStart = 18, // 0x00000012
    MatchEnd = 19, // 0x00000013
    TeamWins = 20, // 0x00000014
    WaitingForPlayers = 21, // 0x00000015
    PrepareNextRound = 22, // 0x00000016
    AllPlayers = 23, // 0x00000017
    AllPlayerDeltas = 24, // 0x00000018
    AllPlayerPositions = 25, // 0x00000019
    PlayerDelta = 26, // 0x0000001A
    PlayerJumped = 27, // 0x0000001B
    PlayerRespawnCountdown = 28, // 0x0000001C
    PlayerRespawned = 29, // 0x0000001D
    PlayerJoinedGame = 30, // 0x0000001E
    JoinGameFailed = 31, // 0x0000001F
    PlayerLeftGame = 32, // 0x00000020
    PlayerChangedTeam = 33, // 0x00000021
    JoinedAsSpectator = 34, // 0x00000022
    PlayersReadyUpdated = 35, // 0x00000023
    DamageEvent = 36, // 0x00000024
    PlayerKilled = 37, // 0x00000025
    UpdateRoundScore = 38, // 0x00000026
    KillsRemaining = 39, // 0x00000027
    LevelUp = 40, // 0x00000028
    KickPlayer = 41, // 0x00000029
    QuickItemEvent = 42, // 0x0000002A
    SingleBulletFire = 43, // 0x0000002B
    PlayerHit = 44, // 0x0000002C
    RemoveProjectile = 45, // 0x0000002D
    EmitProjectile = 46, // 0x0000002E
    EmitQuickItem = 47, // 0x0000002F
    ActivateQuickItem = 48, // 0x00000030
    ChatMessage = 49, // 0x00000031
  }
}
