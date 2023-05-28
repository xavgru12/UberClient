// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanRequestAcceptViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanRequestAcceptViewProxy
  {
    public static void Serialize(Stream stream, ClanRequestAcceptView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
        Int32Proxy.Serialize((Stream) bytes, instance.ClanRequestId);
        if (instance.ClanView != null)
          ClanViewProxy.Serialize((Stream) bytes, instance.ClanView);
        else
          num |= 1;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ClanRequestAcceptView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanRequestAcceptView requestAcceptView = new ClanRequestAcceptView();
      requestAcceptView.ActionResult = Int32Proxy.Deserialize(bytes);
      requestAcceptView.ClanRequestId = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        requestAcceptView.ClanView = ClanViewProxy.Deserialize(bytes);
      return requestAcceptView;
    }
  }
}
