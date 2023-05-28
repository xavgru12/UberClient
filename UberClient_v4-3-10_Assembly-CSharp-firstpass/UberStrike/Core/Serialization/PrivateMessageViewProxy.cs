// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PrivateMessageViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PrivateMessageViewProxy
  {
    public static void Serialize(Stream stream, PrivateMessageView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.ContentText != null)
          StringProxy.Serialize((Stream) bytes, instance.ContentText);
        else
          num |= 1;
        DateTimeProxy.Serialize((Stream) bytes, instance.DateSent);
        Int32Proxy.Serialize((Stream) bytes, instance.FromCmid);
        if (instance.FromName != null)
          StringProxy.Serialize((Stream) bytes, instance.FromName);
        else
          num |= 2;
        BooleanProxy.Serialize((Stream) bytes, instance.HasAttachment);
        BooleanProxy.Serialize((Stream) bytes, instance.IsDeletedByReceiver);
        BooleanProxy.Serialize((Stream) bytes, instance.IsDeletedBySender);
        BooleanProxy.Serialize((Stream) bytes, instance.IsRead);
        Int32Proxy.Serialize((Stream) bytes, instance.PrivateMessageId);
        Int32Proxy.Serialize((Stream) bytes, instance.ToCmid);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PrivateMessageView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PrivateMessageView privateMessageView = new PrivateMessageView();
      if ((num & 1) != 0)
        privateMessageView.ContentText = StringProxy.Deserialize(bytes);
      privateMessageView.DateSent = DateTimeProxy.Deserialize(bytes);
      privateMessageView.FromCmid = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        privateMessageView.FromName = StringProxy.Deserialize(bytes);
      privateMessageView.HasAttachment = BooleanProxy.Deserialize(bytes);
      privateMessageView.IsDeletedByReceiver = BooleanProxy.Deserialize(bytes);
      privateMessageView.IsDeletedBySender = BooleanProxy.Deserialize(bytes);
      privateMessageView.IsRead = BooleanProxy.Deserialize(bytes);
      privateMessageView.PrivateMessageId = Int32Proxy.Deserialize(bytes);
      privateMessageView.ToCmid = Int32Proxy.Deserialize(bytes);
      return privateMessageView;
    }
  }
}
