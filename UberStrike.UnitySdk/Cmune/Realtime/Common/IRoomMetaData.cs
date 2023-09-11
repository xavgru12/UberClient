// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.IRoomMetaData
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.Realtime.Common
{
  public interface IRoomMetaData
  {
    CmuneRoomID RoomID { get; }

    string RoomName { get; }

    string Password { get; }

    bool IsPublic { get; }

    int ConnectedPlayers { get; set; }

    int MaxPlayers { get; }

    string ServerConnection { get; }
  }
}
