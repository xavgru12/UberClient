// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerLevelCapViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerLevelCapViewProxy
  {
    public static void Serialize(Stream stream, PlayerLevelCapView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Level);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayerLevelCapId);
        Int32Proxy.Serialize((Stream) bytes, instance.XPRequired);
        bytes.WriteTo(stream);
      }
    }

    public static PlayerLevelCapView Deserialize(Stream bytes) => new PlayerLevelCapView()
    {
      Level = Int32Proxy.Deserialize(bytes),
      PlayerLevelCapId = Int32Proxy.Deserialize(bytes),
      XPRequired = Int32Proxy.Deserialize(bytes)
    };
  }
}
