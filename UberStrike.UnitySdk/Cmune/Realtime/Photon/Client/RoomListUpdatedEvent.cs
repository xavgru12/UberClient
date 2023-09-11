// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.RoomListUpdatedEvent
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client
{
  public class RoomListUpdatedEvent
  {
    public List<RoomMetaData> Rooms;

    public bool IsInitialList { get; private set; }

    internal RoomListUpdatedEvent(IEnumerable<RoomMetaData> rooms, bool isInitialList = false)
    {
      this.Rooms = new List<RoomMetaData>(rooms);
      this.IsInitialList = isInitialList;
    }
  }
}
