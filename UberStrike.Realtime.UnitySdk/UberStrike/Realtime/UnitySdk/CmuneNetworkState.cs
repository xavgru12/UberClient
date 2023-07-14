// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneNetworkState
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
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
      CmuneNetworkState._currentGames[data.RoomID] = data;
      return true;
    }

    internal static bool RemoveRoom(CmuneRoomID id) => CmuneNetworkState._currentGames.Remove(id);

    public static IEnumerable<RoomMetaData> AllRooms => (IEnumerable<RoomMetaData>) CmuneNetworkState._currentGames.Values;

    public static int RoomCount => CmuneNetworkState._currentGames.Count;

    internal static void ClearRooms() => CmuneNetworkState._currentGames.Clear();
  }
}
