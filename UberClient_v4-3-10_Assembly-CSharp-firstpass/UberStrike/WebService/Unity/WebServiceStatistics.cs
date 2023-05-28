// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.WebServiceStatistics
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
      statistics.LastCall = DateTime.UtcNow;
    }

    public static void RecordWebServiceEnd(string method, int bytes, bool success)
    {
      WebServiceStatistics.Statistics statistics = WebServiceStatistics.GetStatistics(method);
      statistics.IncomingBytes += bytes;
      WebServiceStatistics.TotalBytesIn += (long) bytes;
      if (!success)
        ++statistics.FailCounter;
      statistics.Time = (float) DateTime.UtcNow.Subtract(statistics.LastCall).TotalSeconds;
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
      public int Counter { get; set; }

      public int IncomingBytes { get; set; }

      public int OutgoingBytes { get; set; }

      public int FailCounter { get; set; }

      public float Time { get; set; }

      internal DateTime LastCall { get; set; }

      public Statistics() => this.LastCall = DateTime.UtcNow;

      public override string ToString() => string.Format("\tcount:{0}({1}) | time:{2:N2} | data:{3:F0}/{4:F0}", (object) this.Counter, (object) this.FailCounter, (object) this.Time, (object) (float) ((double) this.IncomingBytes / 1024.0), (object) (float) ((double) this.OutgoingBytes / 1024.0));
    }
  }
}
