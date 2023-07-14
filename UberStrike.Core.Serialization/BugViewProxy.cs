// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BugViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BugViewProxy
  {
    public static void Serialize(Stream stream, BugView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Content != null)
            StringProxy.Serialize((Stream) bytes, instance.Content);
          else
            num |= 1;
          if (instance.Subject != null)
            StringProxy.Serialize((Stream) bytes, instance.Subject);
          else
            num |= 2;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static BugView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BugView bugView = (BugView) null;
      if (num != 0)
      {
        bugView = new BugView();
        if ((num & 1) != 0)
          bugView.Content = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          bugView.Subject = StringProxy.Deserialize(bytes);
      }
      return bugView;
    }
  }
}
