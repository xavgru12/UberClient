// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanMemberViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
