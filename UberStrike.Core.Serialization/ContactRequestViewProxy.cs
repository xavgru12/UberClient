// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactRequestViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ContactRequestViewProxy
  {
    public static void Serialize(Stream stream, ContactRequestView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.InitiatorCmid);
          if (instance.InitiatorMessage != null)
            StringProxy.Serialize((Stream) bytes, instance.InitiatorMessage);
          else
            num |= 1;
          if (instance.InitiatorName != null)
            StringProxy.Serialize((Stream) bytes, instance.InitiatorName);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.ReceiverCmid);
          Int32Proxy.Serialize((Stream) bytes, instance.RequestId);
          DateTimeProxy.Serialize((Stream) bytes, instance.SentDate);
          EnumProxy<ContactRequestStatus>.Serialize((Stream) bytes, instance.Status);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ContactRequestView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactRequestView contactRequestView = (ContactRequestView) null;
      if (num != 0)
      {
        contactRequestView = new ContactRequestView();
        contactRequestView.InitiatorCmid = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          contactRequestView.InitiatorMessage = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          contactRequestView.InitiatorName = StringProxy.Deserialize(bytes);
        contactRequestView.ReceiverCmid = Int32Proxy.Deserialize(bytes);
        contactRequestView.RequestId = Int32Proxy.Deserialize(bytes);
        contactRequestView.SentDate = DateTimeProxy.Deserialize(bytes);
        contactRequestView.Status = EnumProxy<ContactRequestStatus>.Deserialize(bytes);
      }
      return contactRequestView;
    }
  }
}
