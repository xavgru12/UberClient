// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PhotonServerLoadProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.Core.Models;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PhotonServerLoadProxy
  {
    public static void Serialize(Stream stream, PhotonServerLoad instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        SingleProxy.Serialize((Stream) bytes, instance.MaxPlayerCount);
        Int32Proxy.Serialize((Stream) bytes, instance.PeersConnected);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersConnected);
        Int32Proxy.Serialize((Stream) bytes, instance.RoomsCreated);
        bytes.WriteTo(stream);
      }
    }

    public static PhotonServerLoad Deserialize(Stream bytes) => new PhotonServerLoad()
    {
      MaxPlayerCount = SingleProxy.Deserialize(bytes),
      PeersConnected = Int32Proxy.Deserialize(bytes),
      PlayersConnected = Int32Proxy.Deserialize(bytes),
      RoomsCreated = Int32Proxy.Deserialize(bytes)
    };
  }
}
