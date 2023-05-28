// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ConnectionAddressProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class ConnectionAddressProxy
  {
    public static void Serialize(Stream stream, ConnectionAddress instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Ipv4);
        UInt16Proxy.Serialize((Stream) bytes, instance.Port);
        bytes.WriteTo(stream);
      }
    }

    public static ConnectionAddress Deserialize(Stream bytes) => new ConnectionAddress()
    {
      Ipv4 = Int32Proxy.Deserialize(bytes),
      Port = UInt16Proxy.Deserialize(bytes)
    };
  }
}
