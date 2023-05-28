// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactRequestViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ContactRequestViewProxy
  {
    public static void Serialize(Stream stream, ContactRequestView instance)
    {
      int num = 0;
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

    public static ContactRequestView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactRequestView contactRequestView = new ContactRequestView();
      contactRequestView.InitiatorCmid = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        contactRequestView.InitiatorMessage = StringProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        contactRequestView.InitiatorName = StringProxy.Deserialize(bytes);
      contactRequestView.ReceiverCmid = Int32Proxy.Deserialize(bytes);
      contactRequestView.RequestId = Int32Proxy.Deserialize(bytes);
      contactRequestView.SentDate = DateTimeProxy.Deserialize(bytes);
      contactRequestView.Status = EnumProxy<ContactRequestStatus>.Deserialize(bytes);
      return contactRequestView;
    }
  }
}
