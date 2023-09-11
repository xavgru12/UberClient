// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.Events.LobbyGameListUpdatedEvent
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
