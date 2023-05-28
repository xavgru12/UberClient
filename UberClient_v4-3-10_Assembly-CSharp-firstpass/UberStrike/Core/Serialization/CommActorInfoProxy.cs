// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CommActorInfoProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class CommActorInfoProxy
  {
    public static void Serialize(Stream stream, CommActorInfo instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<MemberAccessLevel>.Serialize((Stream) bytes, instance.AccessLevel);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
        if (instance.ClanTag != null)
          StringProxy.Serialize((Stream) bytes, instance.ClanTag);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        if (instance.CurrentRoom != null)
          GameRoomProxy.Serialize((Stream) bytes, instance.CurrentRoom);
        else
          num |= 2;
        ByteProxy.Serialize((Stream) bytes, instance.ModerationFlag);
        if (instance.ModInformation != null)
          StringProxy.Serialize((Stream) bytes, instance.ModInformation);
        else
          num |= 4;
        if (instance.PlayerName != null)
          StringProxy.Serialize((Stream) bytes, instance.PlayerName);
        else
          num |= 8;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static CommActorInfo Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CommActorInfo commActorInfo = new CommActorInfo();
      commActorInfo.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
      commActorInfo.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
      if ((num & 1) != 0)
        commActorInfo.ClanTag = StringProxy.Deserialize(bytes);
      commActorInfo.Cmid = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        commActorInfo.CurrentRoom = GameRoomProxy.Deserialize(bytes);
      commActorInfo.ModerationFlag = ByteProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        commActorInfo.ModInformation = StringProxy.Deserialize(bytes);
      if ((num & 8) != 0)
        commActorInfo.PlayerName = StringProxy.Deserialize(bytes);
      return commActorInfo;
    }
  }
}
