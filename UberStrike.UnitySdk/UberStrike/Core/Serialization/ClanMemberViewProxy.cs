// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanMemberViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanMemberViewProxy
  {
    public static void Serialize(Stream stream, ClanMemberView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          DateTimeProxy.Serialize((Stream) bytes, instance.JoiningDate);
          DateTimeProxy.Serialize((Stream) bytes, instance.Lastlogin);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 1;
          EnumProxy<GroupPosition>.Serialize((Stream) bytes, instance.Position);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanMemberView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanMemberView clanMemberView = (ClanMemberView) null;
      if (num != 0)
      {
        clanMemberView = new ClanMemberView();
        clanMemberView.Cmid = Int32Proxy.Deserialize(bytes);
        clanMemberView.JoiningDate = DateTimeProxy.Deserialize(bytes);
        clanMemberView.Lastlogin = DateTimeProxy.Deserialize(bytes);
        if ((num & 1) != 0)
          clanMemberView.Name = StringProxy.Deserialize(bytes);
        clanMemberView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);
      }
      return clanMemberView;
    }
  }
}
