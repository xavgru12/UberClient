// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.IGamePeerEventsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum IGamePeerEventsType
  {
    HeartbeatChallenge = 1,
    RoomEntered = 2,
    RoomEnterFailed = 3,
    RequestPasswordForRoom = 4,
    RoomLeft = 5,
    FullGameList = 6,
    GameListUpdate = 7,
    GameListUpdateEnd = 8,
    GetGameInformation = 9,
    ServerLoadData = 10, // 0x0000000A
    DisconnectAndDisablePhoton = 11, // 0x0000000B
  }
}
