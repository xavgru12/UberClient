// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MessageThreadViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MessageThreadViewProxy
  {
    public static void Serialize(Stream stream, MessageThreadView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          BooleanProxy.Serialize((Stream) bytes, instance.HasNewMessages);
          if (instance.LastMessagePreview != null)
            StringProxy.Serialize((Stream) bytes, instance.LastMessagePreview);
          else
            num |= 1;
          DateTimeProxy.Serialize((Stream) bytes, instance.LastUpdate);
          Int32Proxy.Serialize((Stream) bytes, instance.MessageCount);
          Int32Proxy.Serialize((Stream) bytes, instance.ThreadId);
          if (instance.ThreadName != null)
            StringProxy.Serialize((Stream) bytes, instance.ThreadName);
          else
            num |= 2;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MessageThreadView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MessageThreadView messageThreadView = (MessageThreadView) null;
      if (num != 0)
      {
        messageThreadView = new MessageThreadView();
        messageThreadView.HasNewMessages = BooleanProxy.Deserialize(bytes);
        if ((num & 1) != 0)
          messageThreadView.LastMessagePreview = StringProxy.Deserialize(bytes);
        messageThreadView.LastUpdate = DateTimeProxy.Deserialize(bytes);
        messageThreadView.MessageCount = Int32Proxy.Deserialize(bytes);
        messageThreadView.ThreadId = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          messageThreadView.ThreadName = StringProxy.Deserialize(bytes);
      }
      return messageThreadView;
    }
  }
}
