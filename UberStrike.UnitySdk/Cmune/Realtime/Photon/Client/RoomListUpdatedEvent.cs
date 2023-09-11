
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
