// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.IGamePeerOperationsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum IGamePeerOperationsType
  {
    SendHeartbeatResponse = 1,
    GetServerLoad = 2,
    GetGameInformation = 3,
    GetGameListUpdates = 4,
    EnterRoom = 5,
    CreateRoom = 6,
    LeaveRoom = 7,
    CloseRoom = 8,
    InspectRoom = 9,
    ReportPlayer = 10, // 0x0000000A
    KickPlayer = 11, // 0x0000000B
    UpdateLoadout = 12, // 0x0000000C
    UpdatePing = 13, // 0x0000000D
    UpdateKeyState = 14, // 0x0000000E
    RefreshBackendData = 15, // 0x0000000F
  }
}
