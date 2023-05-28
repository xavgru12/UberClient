// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ContactGroupViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static ContactGroupView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ContactGroupView contactGroupView = new ContactGroupView();
      if ((num & 1) != 0)
        contactGroupView.Contacts = ListProxy<PublicProfileView>.Deserialize(bytes, new ListProxy<PublicProfileView>.Deserializer<PublicProfileView>(PublicProfileViewProxy.Deserialize));
      contactGroupView.GroupId = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        contactGroupView.GroupName = StringProxy.Deserialize(bytes);
      return contactGroupView;
    }
  }
}
