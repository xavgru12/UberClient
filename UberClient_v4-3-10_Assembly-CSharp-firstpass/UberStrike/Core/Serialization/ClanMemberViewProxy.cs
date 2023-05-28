// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanMemberViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanMemberViewProxy
  {
    public static void Serialize(Stream stream, ClanMemberView instance)
    {
      int num = 0;
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

    public static ClanMemberView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanMemberView clanMemberView = new ClanMemberView();
      clanMemberView.Cmid = Int32Proxy.Deserialize(bytes);
      clanMemberView.JoiningDate = DateTimeProxy.Deserialize(bytes);
      clanMemberView.Lastlogin = DateTimeProxy.Deserialize(bytes);
      if ((num & 1) != 0)
        clanMemberView.Name = StringProxy.Deserialize(bytes);
      clanMemberView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);
      return clanMemberView;
    }
  }
}
