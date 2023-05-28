// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactGroupViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ContactGroupViewProxy
  {
    public static void Serialize(Stream stream, ContactGroupView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Contacts != null)
            ListProxy<PublicProfileView>.Serialize((Stream) bytes, (ICollection<PublicProfileView>) instance.Contacts, new ListProxy<PublicProfileView>.Serializer<PublicProfileView>(PublicProfileViewProxy.Serialize));
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
          if (instance.GroupName != null)
            StringProxy.Serialize((Stream) bytes, instance.GroupName);
          else
            num |= 2;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ContactGroupView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactGroupView contactGroupView = (ContactGroupView) null;
      if (num != 0)
      {
        contactGroupView = new ContactGroupView();
        if ((num & 1) != 0)
          contactGroupView.Contacts = ListProxy<PublicProfileView>.Deserialize(bytes, new ListProxy<PublicProfileView>.Deserializer<PublicProfileView>(PublicProfileViewProxy.Deserialize));
        contactGroupView.GroupId = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          contactGroupView.GroupName = StringProxy.Deserialize(bytes);
      }
      return contactGroupView;
    }
  }
}
