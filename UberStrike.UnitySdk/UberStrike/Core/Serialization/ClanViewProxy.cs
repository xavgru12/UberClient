// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanView clanView = (ClanView) null;
      if (num != 0)
      {
        clanView = new ClanView();
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
      }
      return clanView;
    }
  }
}
