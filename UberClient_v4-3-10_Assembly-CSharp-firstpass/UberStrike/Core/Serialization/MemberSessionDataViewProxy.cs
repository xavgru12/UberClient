// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberSessionDataViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberSessionDataViewProxy
  {
    public static void Serialize(Stream stream, MemberSessionDataView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<MemberAccessLevel>.Serialize((Stream) bytes, instance.AccessLevel);
        if (instance.AuthToken != null)
          StringProxy.Serialize((Stream) bytes, instance.AuthToken);
        else
          num |= 1;
        EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
        if (instance.ClanTag != null)
          StringProxy.Serialize((Stream) bytes, instance.ClanTag);
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        BooleanProxy.Serialize((Stream) bytes, instance.IsBanned);
        Int32Proxy.Serialize((Stream) bytes, instance.Level);
        DateTimeProxy.Serialize((Stream) bytes, instance.LoginDate);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 4;
        Int32Proxy.Serialize((Stream) bytes, instance.XP);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MemberSessionDataView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberSessionDataView memberSessionDataView = new MemberSessionDataView();
      memberSessionDataView.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
      if ((num & 1) != 0)
        memberSessionDataView.AuthToken = StringProxy.Deserialize(bytes);
      memberSessionDataView.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
      if ((num & 2) != 0)
        memberSessionDataView.ClanTag = StringProxy.Deserialize(bytes);
      memberSessionDataView.Cmid = Int32Proxy.Deserialize(bytes);
      memberSessionDataView.IsBanned = BooleanProxy.Deserialize(bytes);
      memberSessionDataView.Level = Int32Proxy.Deserialize(bytes);
      memberSessionDataView.LoginDate = DateTimeProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        memberSessionDataView.Name = StringProxy.Deserialize(bytes);
      memberSessionDataView.XP = Int32Proxy.Deserialize(bytes);
      return memberSessionDataView;
    }
  }
}
