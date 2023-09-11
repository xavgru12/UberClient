
using Cmune.Realtime.Common;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client.Events
{
  public class LobbyGameListUpdatedEvent
  {
    public ICollection<RoomMetaData> Rooms;
    public int PlayerCount;

    public LobbyGameListUpdatedEvent(ICollection<RoomMetaData> rooms, int playerCount)
    {
      this.Rooms = rooms;
      this.PlayerCount = playerCount;
    }
  }
}
