// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MapSettingsProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class MapSettingsProxy
  {
    public static void Serialize(Stream stream, MapSettings instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.KillsCurrent);
        Int32Proxy.Serialize((Stream) bytes, instance.KillsMax);
        Int32Proxy.Serialize((Stream) bytes, instance.KillsMin);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersCurrent);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersMax);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersMin);
        Int32Proxy.Serialize((Stream) bytes, instance.TimeCurrent);
        Int32Proxy.Serialize((Stream) bytes, instance.TimeMax);
        Int32Proxy.Serialize((Stream) bytes, instance.TimeMin);
        bytes.WriteTo(stream);
      }
    }

    public static MapSettings Deserialize(Stream bytes) => new MapSettings()
    {
      KillsCurrent = Int32Proxy.Deserialize(bytes),
      KillsMax = Int32Proxy.Deserialize(bytes),
      KillsMin = Int32Proxy.Deserialize(bytes),
      PlayersCurrent = Int32Proxy.Deserialize(bytes),
      PlayersMax = Int32Proxy.Deserialize(bytes),
      PlayersMin = Int32Proxy.Deserialize(bytes),
      TimeCurrent = Int32Proxy.Deserialize(bytes),
      TimeMax = Int32Proxy.Deserialize(bytes),
      TimeMin = Int32Proxy.Deserialize(bytes)
    };
  }
}
