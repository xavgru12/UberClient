// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanRequestDeclineViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanRequestDeclineViewProxy
  {
    public static void Serialize(Stream stream, ClanRequestDeclineView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
          Int32Proxy.Serialize((Stream) bytes, instance.ClanRequestId);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanRequestDeclineView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanRequestDeclineView requestDeclineView = (ClanRequestDeclineView) null;
      if (num != 0)
      {
        requestDeclineView = new ClanRequestDeclineView();
        requestDeclineView.ActionResult = Int32Proxy.Deserialize(bytes);
        requestDeclineView.ClanRequestId = Int32Proxy.Deserialize(bytes);
      }
      return requestDeclineView;
    }
  }
}
