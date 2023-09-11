// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactGroupViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
