﻿
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ContactRequestDeclineViewProxy
  {
    public static void Serialize(Stream stream, ContactRequestDeclineView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
          Int32Proxy.Serialize((Stream) bytes, instance.RequestId);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ContactRequestDeclineView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactRequestDeclineView requestDeclineView = (ContactRequestDeclineView) null;
      if (num != 0)
      {
        requestDeclineView = new ContactRequestDeclineView();
        requestDeclineView.ActionResult = Int32Proxy.Deserialize(bytes);
        requestDeclineView.RequestId = Int32Proxy.Deserialize(bytes);
      }
      return requestDeclineView;
    }
  }
}
