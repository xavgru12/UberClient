// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PrivateMessageViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PrivateMessageViewProxy
  {
    public static void Serialize(Stream stream, PrivateMessageView instance)
    {
      int num = 0;
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PrivateMessageView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PrivateMessageView privateMessageView = (PrivateMessageView) null;
      if (num != 0)
      {
        privateMessageView = new PrivateMessageView();
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
      }
      return privateMessageView;
    }
  }
}
