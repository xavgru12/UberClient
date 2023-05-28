// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MessageThreadViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MessageThreadViewProxy
  {
    public static void Serialize(Stream stream, MessageThreadView instance)
    {
      int num = 0;
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

    public static MessageThreadView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MessageThreadView messageThreadView = new MessageThreadView();
      messageThreadView.HasNewMessages = BooleanProxy.Deserialize(bytes);
      if ((num & 1) != 0)
        messageThreadView.LastMessagePreview = StringProxy.Deserialize(bytes);
      messageThreadView.LastUpdate = DateTimeProxy.Deserialize(bytes);
      messageThreadView.MessageCount = Int32Proxy.Deserialize(bytes);
      messageThreadView.ThreadId = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        messageThreadView.ThreadName = StringProxy.Deserialize(bytes);
      return messageThreadView;
    }
  }
}
