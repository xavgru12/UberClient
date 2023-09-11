﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactRequestViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
