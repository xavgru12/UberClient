// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.IGameRoomOperationsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum IGameRoomOperationsType
  {
    JoinGame = 1,
    JoinAsSpectator = 2,
    PowerUpRespawnTimes = 3,
    PowerUpPicked = 4,
    IncreaseHealthAndArmor = 5,
    OpenDoor = 6,
    SpawnPositions = 7,
    RespawnRequest = 8,
    DirectHitDamage = 9,
    ExplosionDamage = 10, // 0x0000000A
    DirectDamage = 11, // 0x0000000B
    DirectDeath = 12, // 0x0000000C
    Jump = 13, // 0x0000000D
    UpdatePositionAndRotation = 14, // 0x0000000E
    KickPlayer = 15, // 0x0000000F
    IsFiring = 16, // 0x00000010
    IsReadyForNextMatch = 17, // 0x00000011
    IsPaused = 18, // 0x00000012
    IsInSniperMode = 19, // 0x00000013
    SingleBulletFire = 20, // 0x00000014
    SwitchWeapon = 21, // 0x00000015
    SwitchTeam = 22, // 0x00000016
    ChangeGear = 23, // 0x00000017
    EmitProjectile = 24, // 0x00000018
    EmitQuickItem = 25, // 0x00000019
    RemoveProjectile = 26, // 0x0000001A
    HitFeedback = 27, // 0x0000001B
    ActivateQuickItem = 28, // 0x0000001C
    ChatMessage = 29, // 0x0000001D
  }
}
