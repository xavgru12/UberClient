﻿// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.NetworkState
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.Realtime.Photon.Client
{
  public enum NetworkState
  {
    STATE_NPEER_CREATED = 0,
    STATE_CONNECT = 1,
    STATE_CONNECTING = 2,
    STATE_CONNECTED = 3,
    STATE_JOINING = 4,
    STATE_ERROR_JOINING = 5,
    STATE_JOINED = 6,
    STATE_LEAVE = 7,
    STATE_LEAVING = 8,
    STATE_ERROR_LEAVING = 9,
    STATE_LEFT = 10, // 0x0000000A
    STATE_ERROR_CONNECTING = 11, // 0x0000000B
    STATE_RECEIVING = 12, // 0x0000000C
    STATE_DISCONNECTING = 13, // 0x0000000D
    STATE_DISCONNECTED = 14, // 0x0000000E
    STATE_JOIN = 15, // 0x0000000F
    STATE_DISCONNECT = 16, // 0x00000010
    STATE_UNDEFINED = 100, // 0x00000064
  }
}
