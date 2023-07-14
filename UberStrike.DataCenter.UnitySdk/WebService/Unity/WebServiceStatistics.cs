// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.WebServiceStatistics
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.WebService.Unity
{
  public static class WebServiceStatistics
  {
    public static bool IsEnabled = true;
    public static readonly Dictionary<string, WebServiceStatistics.Statistics> Data = new Dictionary<string, WebServiceStatistics.Statistics>();

    public static long TotalBytesIn { get; private set; }

    public static long TotalBytesOut { get; private set; }

    public static void RecordWebServiceBegin(string method, int bytes)
    {
      WebServiceStatistics.Statistics statistics = WebServiceStatistics.GetStatistics(method);
      ++statistics.Counter;
      statistics.OutgoingBytes += bytes;
      WebServiceStatistics.TotalBytesOut += (long) bytes;
      statistics.LastCall = DateTime.Now;
    }

    public static void RecordWebServiceEnd(string method, int bytes, bool success)
    {
      WebServiceStatistics.Statistics statistics = WebServiceStatistics.GetStatistics(method);
      statistics.IncomingBytes += bytes;
      WebServiceStatistics.TotalBytesIn += (long) bytes;
      if (!success)
        ++statistics.FailCounter;
      statistics.Time = (float) DateTime.Now.Subtract(statistics.LastCall).TotalSeconds;
    }

    private static WebServiceStatistics.Statistics GetStatistics(string method)
    {
      WebServiceStatistics.Statistics statistics;
      if (!WebServiceStatistics.Data.TryGetValue(method, out statistics))
      {
        statistics = new WebServiceStatistics.Statistics();
        WebServiceStatistics.Data[method] = statistics;
      }
      return statistics;
    }

    public class Statistics
    {
      public Statistics() => this.LastCall = DateTime.Now;

      public int Counter { get; set; }

      public int IncomingBytes { get; set; }

      public int OutgoingBytes { get; set; }

      public int FailCounter { get; set; }

      public float Time { get; set; }

      internal DateTime LastCall { get; set; }

      public override string ToString() => string.Format("\tcount:{0}({1}) | time:{2:N2} | data:{3:F0}/{4:F0}", (object) this.Counter, (object) this.FailCounter, (object) this.Time, (object) (float) ((double) this.IncomingBytes / 1024.0), (object) (float) ((double) this.OutgoingBytes / 1024.0));
    }
  }
}
