// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanViewProxy
  {
    public static void Serialize(Stream stream, ClanView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Address != null)
          StringProxy.Serialize((Stream) bytes, instance.Address);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
        EnumProxy<GroupColor>.Serialize((Stream) bytes, instance.ColorStyle);
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 2;
        EnumProxy<GroupFontStyle>.Serialize((Stream) bytes, instance.FontStyle);
        DateTimeProxy.Serialize((Stream) bytes, instance.FoundingDate);
        Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
        DateTimeProxy.Serialize((Stream) bytes, instance.LastUpdated);
        if (instance.Members != null)
          ListProxy<ClanMemberView>.Serialize((Stream) bytes, (ICollection<ClanMemberView>) instance.Members, new ListProxy<ClanMemberView>.Serializer<ClanMemberView>(ClanMemberViewProxy.Serialize));
        else
          num |= 4;
        Int32Proxy.Serialize((Stream) bytes, instance.MembersCount);
        Int32Proxy.Serialize((Stream) bytes, instance.MembersLimit);
        if (instance.Motto != null)
          StringProxy.Serialize((Stream) bytes, instance.Motto);
        else
          num |= 8;
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 16;
        Int32Proxy.Serialize((Stream) bytes, instance.OwnerCmid);
        if (instance.OwnerName != null)
          StringProxy.Serialize((Stream) bytes, instance.OwnerName);
        else
          num |= 32;
        if (instance.Picture != null)
          StringProxy.Serialize((Stream) bytes, instance.Picture);
        else
          num |= 64;
        if (instance.Tag != null)
          StringProxy.Serialize((Stream) bytes, instance.Tag);
        else
          num |= 128;
        EnumProxy<GroupType>.Serialize((Stream) bytes, instance.Type);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ClanView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanView clanView = new ClanView();
      if ((num & 1) != 0)
        clanView.Address = StringProxy.Deserialize(bytes);
      clanView.ApplicationId = Int32Proxy.Deserialize(bytes);
      clanView.ColorStyle = EnumProxy<GroupColor>.Deserialize(bytes);
      if ((num & 2) != 0)
        clanView.Description = StringProxy.Deserialize(bytes);
      clanView.FontStyle = EnumProxy<GroupFontStyle>.Deserialize(bytes);
      clanView.FoundingDate = DateTimeProxy.Deserialize(bytes);
      clanView.GroupId = Int32Proxy.Deserialize(bytes);
      clanView.LastUpdated = DateTimeProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        clanView.Members = ListProxy<ClanMemberView>.Deserialize(bytes, new ListProxy<ClanMemberView>.Deserializer<ClanMemberView>(ClanMemberViewProxy.Deserialize));
      clanView.MembersCount = Int32Proxy.Deserialize(bytes);
      clanView.MembersLimit = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        clanView.Motto = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        clanView.Name = StringProxy.Deserialize(bytes);
      clanView.OwnerCmid = Int32Proxy.Deserialize(bytes);
      if ((num & 32) != 0)
        clanView.OwnerName = StringProxy.Deserialize(bytes);
      if ((num & 64) != 0)
        clanView.Picture = StringProxy.Deserialize(bytes);
      if ((num & 128) != 0)
        clanView.Tag = StringProxy.Deserialize(bytes);
      clanView.Type = EnumProxy<GroupType>.Deserialize(bytes);
      return clanView;
    }
  }
}
