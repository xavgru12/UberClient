// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.LiveFeedViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class LiveFeedViewProxy
  {
    public static void Serialize(Stream stream, LiveFeedView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        DateTimeProxy.Serialize((Stream) bytes, instance.Date);
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.LivedFeedId);
        Int32Proxy.Serialize((Stream) bytes, instance.Priority);
        if (instance.Url != null)
          StringProxy.Serialize((Stream) bytes, instance.Url);
        else
          num |= 2;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static LiveFeedView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LiveFeedView liveFeedView = new LiveFeedView();
      liveFeedView.Date = DateTimeProxy.Deserialize(bytes);
      if ((num & 1) != 0)
        liveFeedView.Description = StringProxy.Deserialize(bytes);
      liveFeedView.LivedFeedId = Int32Proxy.Deserialize(bytes);
      liveFeedView.Priority = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        liveFeedView.Url = StringProxy.Deserialize(bytes);
      return liveFeedView;
    }
  }
}
