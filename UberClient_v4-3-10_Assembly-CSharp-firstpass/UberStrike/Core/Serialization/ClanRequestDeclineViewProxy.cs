// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanRequestDeclineViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanRequestDeclineViewProxy
  {
    public static void Serialize(Stream stream, ClanRequestDeclineView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
        Int32Proxy.Serialize((Stream) bytes, instance.ClanRequestId);
        bytes.WriteTo(stream);
      }
    }

    public static ClanRequestDeclineView Deserialize(Stream bytes) => new ClanRequestDeclineView()
    {
      ActionResult = Int32Proxy.Deserialize(bytes),
      ClanRequestId = Int32Proxy.Deserialize(bytes)
    };
  }
}
