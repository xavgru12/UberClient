// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberWalletViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberWalletViewProxy
  {
    public static void Serialize(Stream stream, MemberWalletView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize((Stream) bytes, instance.Credits);
        DateTimeProxy.Serialize((Stream) bytes, instance.CreditsExpiration);
        Int32Proxy.Serialize((Stream) bytes, instance.Points);
        DateTimeProxy.Serialize((Stream) bytes, instance.PointsExpiration);
        bytes.WriteTo(stream);
      }
    }

    public static MemberWalletView Deserialize(Stream bytes) => new MemberWalletView()
    {
      Cmid = Int32Proxy.Deserialize(bytes),
      Credits = Int32Proxy.Deserialize(bytes),
      CreditsExpiration = DateTimeProxy.Deserialize(bytes),
      Points = Int32Proxy.Deserialize(bytes),
      PointsExpiration = DateTimeProxy.Deserialize(bytes)
    };
  }
}
