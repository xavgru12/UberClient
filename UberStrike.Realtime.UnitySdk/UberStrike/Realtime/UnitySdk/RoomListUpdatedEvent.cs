// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.RoomListUpdatedEvent
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
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
