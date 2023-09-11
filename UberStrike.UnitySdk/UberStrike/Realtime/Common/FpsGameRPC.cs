// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.FpsGameRPC
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(51, 128)]
  public class FpsGameRPC : GameRPC
  {
    public const byte PlayerSpectator = 51;
    public const byte LoadPendingAvatarOfPlayers = 52;
    public const byte TeamScoreUpdate = 53;
    public const byte PlayerTeamChange = 54;
    public const byte PrecisionSync = 55;
    public const byte SplatConfirmation = 56;
    public const byte UpdateTotalPlayerStats = 57;
    public const byte TeamEliminationRoundEnd = 58;
    public const byte LoadPendingAvatars = 59;
    public const byte ForceSplatted = 60;
    public const byte PowerUpPicked = 61;
    public const byte SetPowerUpCount = 62;
    public const byte RequestSpawnPositionsFromRoomOwner = 63;
    public const byte SendSpawnPositionsToServer = 64;
    public const byte RequestSpawnPosition = 65;
    public const byte SendSpawnPositionToPlayer = 66;
    public const byte KickOffNearbyPlayers = 67;
    public const byte PlayerHit = 68;
    public const byte SetDamage = 69;
    public const byte SetSpawnPoints = 70;
    public const byte SetNextSpawnPointForPlayer = 71;
    public const byte RequestRespawnForPlayer = 72;
    public const byte Statistics = 73;
    public const byte SetPlayerReadyForNextRound = 74;
    public const byte SetEndOfRoundCountdown = 75;
    public const byte MatchStart = 76;
    public const byte MatchEnd = 77;
    public const byte PlayerEvent = 78;
    public const byte UpdateSplatCount = 79;
    public const byte SplatGameEvent = 80;
    public const byte TeamBalanceUpdate = 81;
    public const byte RequestTeam = 82;
    public const byte PositionUpdate = 83;
    public const byte DoorOpen = 84;
    public const byte SetPlayerSpawnPosition = 85;
    public const byte EmitProjectile = 86;
    public const byte ExplodeProjectile = 87;
    public const byte UpdateXpAndPoints = 88;
    public const byte SingleBulletFire = 89;
    public const byte GraceTimeCountDown = 90;
    public const byte UpdateRoundStats = 91;
    public const byte SyncRoundTime = 92;
    public const byte SetWaitingForPlayers = 93;
    public const byte SpawnAIEnemies = 94;
    public const byte OwnAIEnemy = 95;
    public const byte AIEnemyUpdate = 96;
    public const byte SetPowerupState = 97;
    public const byte IncreaseHealthAndArmor = 98;
    public const byte QuickItemEvent = 99;
    public const byte EmitQuickItem = 100;
    public const byte KickFromGame = 101;
    public const byte SetShotCounts = 102;
  }
}
