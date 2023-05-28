// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DailyPointsViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class DailyPointsViewProxy
  {
    public static void Serialize(Stream stream, DailyPointsView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Current);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsMax);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsTomorrow);
        bytes.WriteTo(stream);
      }
    }

    public static DailyPointsView Deserialize(Stream bytes) => new DailyPointsView()
    {
      Current = Int32Proxy.Deserialize(bytes),
      PointsMax = Int32Proxy.Deserialize(bytes),
      PointsTomorrow = Int32Proxy.Deserialize(bytes)
    };
  }
}
