﻿
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class LiveFeedViewProxy
  {
    public static void Serialize(Stream stream, LiveFeedView instance)
    {
      int num = 0;
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static LiveFeedView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LiveFeedView liveFeedView = (LiveFeedView) null;
      if (num != 0)
      {
        liveFeedView = new LiveFeedView();
        liveFeedView.Date = DateTimeProxy.Deserialize(bytes);
        if ((num & 1) != 0)
          liveFeedView.Description = StringProxy.Deserialize(bytes);
        liveFeedView.LivedFeedId = Int32Proxy.Deserialize(bytes);
        liveFeedView.Priority = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          liveFeedView.Url = StringProxy.Deserialize(bytes);
      }
      return liveFeedView;
    }
  }
}
