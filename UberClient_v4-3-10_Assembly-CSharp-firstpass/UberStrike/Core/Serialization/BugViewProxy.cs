// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BugViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BugViewProxy
  {
    public static void Serialize(Stream stream, BugView instance)
    {
      int num = 0;
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

    public static BugView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BugView bugView = new BugView();
      if ((num & 1) != 0)
        bugView.Content = StringProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        bugView.Subject = StringProxy.Deserialize(bytes);
      return bugView;
    }
  }
}
