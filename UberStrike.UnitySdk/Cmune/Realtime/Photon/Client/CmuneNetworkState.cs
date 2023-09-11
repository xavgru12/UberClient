// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.CmuneNetworkState
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client
{
  public static class CmuneNetworkState
  {
    private static readonly Dictionary<CmuneRoomID, RoomMetaData> _currentGames = new Dictionary<CmuneRoomID, RoomMetaData>();
    private static int _currentSessionID = 0;
    public static bool DebugMessaging = false;
    public static int TotalRecievedBytes = 0;
    public static int TotalSentBytes = 0;
    public static int IncomingMessagesCount = 0;
    public static int UnreliableOutgoingMessagesCount = 0;
    public static int ReliableOutgoingMessagesCount = 0;

    public static int OutgoingMessagesCount => CmuneNetworkState.ReliableOutgoingMessagesCount + CmuneNetworkState.UnreliableOutgoingMessagesCount;

    public static int GetNextSessionID() => CmuneNetworkState._currentSessionID++;

    public static bool TryGetRoom(CmuneRoomID roomID, out RoomMetaData meta) => CmuneNetworkState._currentGames.TryGetValue(roomID, out meta);

    internal static bool AddRoom(RoomMetaData data)
    {
      if (!data.RoomID.IsVersionCompatible)
        return false;
      CmuneNetworkState._currentGames[data.RoomID] = data;
      return true;
    }

    internal static bool RemoveRoom(CmuneRoomID id) => CmuneNetworkState._currentGames.Remove(id);

    public static IEnumerable<RoomMetaData> AllRooms => (IEnumerable<RoomMetaData>) CmuneNetworkState._currentGames.Values;

    public static int RoomCount => CmuneNetworkState._currentGames.Count;

    internal static void ClearRooms() => CmuneNetworkState._currentGames.Clear();
  }
}
