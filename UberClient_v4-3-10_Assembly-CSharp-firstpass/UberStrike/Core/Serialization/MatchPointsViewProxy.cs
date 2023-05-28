// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MatchPointsViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class MatchPointsViewProxy
  {
    public static void Serialize(Stream stream, MatchPointsView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.LoserPointsBase);
        Int32Proxy.Serialize((Stream) bytes, instance.LoserPointsPerMinute);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxTimeInGame);
        Int32Proxy.Serialize((Stream) bytes, instance.WinnerPointsBase);
        Int32Proxy.Serialize((Stream) bytes, instance.WinnerPointsPerMinute);
        bytes.WriteTo(stream);
      }
    }

    public static MatchPointsView Deserialize(Stream bytes) => new MatchPointsView()
    {
      LoserPointsBase = Int32Proxy.Deserialize(bytes),
      LoserPointsPerMinute = Int32Proxy.Deserialize(bytes),
      MaxTimeInGame = Int32Proxy.Deserialize(bytes),
      WinnerPointsBase = Int32Proxy.Deserialize(bytes),
      WinnerPointsPerMinute = Int32Proxy.Deserialize(bytes)
    };
  }
}
