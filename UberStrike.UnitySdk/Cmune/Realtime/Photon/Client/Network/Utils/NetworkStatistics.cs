// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.Network.Utils.NetworkStatistics
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client.Network.Utils
{
  public static class NetworkStatistics
  {
    public static bool IsEnabled = true;
    public static readonly Dictionary<string, NetworkStatistics.Statistics> Incoming = new Dictionary<string, NetworkStatistics.Statistics>();
    public static readonly Dictionary<string, NetworkStatistics.Statistics> Outgoing = new Dictionary<string, NetworkStatistics.Statistics>();

    public static long TotalBytesIn { get; private set; }

    public static long TotalBytesOut { get; private set; }

    public static void RecordOutgoingCall(string method, int bytes)
    {
      NetworkStatistics.Statistics statistics = NetworkStatistics.GetStatistics(NetworkStatistics.Outgoing, method);
      ++statistics.Counter;
      NetworkStatistics.TotalBytesOut += (long) bytes;
      statistics.Bytes += bytes;
    }

    public static void RecordIncomingCall(string method, int bytes)
    {
      NetworkStatistics.Statistics statistics = NetworkStatistics.GetStatistics(NetworkStatistics.Incoming, method);
      ++statistics.Counter;
      NetworkStatistics.TotalBytesIn += (long) bytes;
      statistics.Bytes += bytes;
    }

    private static NetworkStatistics.Statistics GetStatistics(
      Dictionary<string, NetworkStatistics.Statistics> dict,
      string method)
    {
      NetworkStatistics.Statistics statistics;
      if (!dict.TryGetValue(method, out statistics))
      {
        statistics = new NetworkStatistics.Statistics();
        dict[method] = statistics;
      }
      return statistics;
    }

    internal static void RecordOutgoingCall(OperationRequest request)
    {
      object obj1;
      object obj2;
      object obj3;
      if (!request.Parameters.TryGetValue((byte) 101, out obj1) || !request.Parameters.TryGetValue((byte) 100, out obj2) || !request.Parameters.TryGetValue((byte) 103, out obj3))
        return;
      NetworkStatistics.RecordOutgoingCall(obj1.ToString() + " " + obj2, ((byte[]) obj3).Length);
    }

    public class Statistics
    {
      public int Counter { get; set; }

      public int Bytes { get; set; }

      public override string ToString() => string.Format("\tcount:{0} | bytes:{1}", (object) this.Counter, (object) this.Bytes);
    }
  }
}
