﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BasicClanViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BasicClanViewProxy
  {
    public static void Serialize(Stream stream, BasicClanView instance)
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
          Int32Proxy.Serialize((Stream) bytes, instance.MembersCount);
          Int32Proxy.Serialize((Stream) bytes, instance.MembersLimit);
          if (instance.Motto != null)
            StringProxy.Serialize((Stream) bytes, instance.Motto);
          else
            num |= 4;
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 8;
          Int32Proxy.Serialize((Stream) bytes, instance.OwnerCmid);
          if (instance.OwnerName != null)
            StringProxy.Serialize((Stream) bytes, instance.OwnerName);
          else
            num |= 16;
          if (instance.Picture != null)
            StringProxy.Serialize((Stream) bytes, instance.Picture);
          else
            num |= 32;
          if (instance.Tag != null)
            StringProxy.Serialize((Stream) bytes, instance.Tag);
          else
            num |= 64;
          EnumProxy<GroupType>.Serialize((Stream) bytes, instance.Type);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static BasicClanView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BasicClanView basicClanView = (BasicClanView) null;
      if (num != 0)
      {
        basicClanView = new BasicClanView();
        if ((num & 1) != 0)
          basicClanView.Address = StringProxy.Deserialize(bytes);
        basicClanView.ApplicationId = Int32Proxy.Deserialize(bytes);
        basicClanView.ColorStyle = EnumProxy<GroupColor>.Deserialize(bytes);
        if ((num & 2) != 0)
          basicClanView.Description = StringProxy.Deserialize(bytes);
        basicClanView.FontStyle = EnumProxy<GroupFontStyle>.Deserialize(bytes);
        basicClanView.FoundingDate = DateTimeProxy.Deserialize(bytes);
        basicClanView.GroupId = Int32Proxy.Deserialize(bytes);
        basicClanView.LastUpdated = DateTimeProxy.Deserialize(bytes);
        basicClanView.MembersCount = Int32Proxy.Deserialize(bytes);
        basicClanView.MembersLimit = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          basicClanView.Motto = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          basicClanView.Name = StringProxy.Deserialize(bytes);
        basicClanView.OwnerCmid = Int32Proxy.Deserialize(bytes);
        if ((num & 16) != 0)
          basicClanView.OwnerName = StringProxy.Deserialize(bytes);
        if ((num & 32) != 0)
          basicClanView.Picture = StringProxy.Deserialize(bytes);
        if ((num & 64) != 0)
          basicClanView.Tag = StringProxy.Deserialize(bytes);
        basicClanView.Type = EnumProxy<GroupType>.Deserialize(bytes);
      }
      return basicClanView;
    }
  }
}
