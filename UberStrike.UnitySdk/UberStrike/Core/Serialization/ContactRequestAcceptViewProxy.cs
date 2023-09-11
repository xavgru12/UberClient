﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactRequestAcceptViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ContactRequestAcceptViewProxy
  {
    public static void Serialize(Stream stream, ContactRequestAcceptView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
          if (instance.Contact != null)
            PublicProfileViewProxy.Serialize((Stream) bytes, instance.Contact);
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.RequestId);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ContactRequestAcceptView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactRequestAcceptView requestAcceptView = (ContactRequestAcceptView) null;
      if (num != 0)
      {
        requestAcceptView = new ContactRequestAcceptView();
        requestAcceptView.ActionResult = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          requestAcceptView.Contact = PublicProfileViewProxy.Deserialize(bytes);
        requestAcceptView.RequestId = Int32Proxy.Deserialize(bytes);
      }
      return requestAcceptView;
    }
  }
}
