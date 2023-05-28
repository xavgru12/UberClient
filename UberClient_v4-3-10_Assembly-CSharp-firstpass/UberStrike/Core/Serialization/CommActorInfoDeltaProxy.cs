// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CommActorInfoDeltaProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class CommActorInfoDeltaProxy
  {
    public static void Serialize(Stream stream, CommActorInfoDelta instance)
    {
      if (instance != null)
      {
        Int32Proxy.Serialize(stream, instance.DeltaMask);
        ByteProxy.Serialize(stream, instance.Id);
        if ((instance.DeltaMask & 1) != 0)
          EnumProxy<MemberAccessLevel>.Serialize(stream, (MemberAccessLevel) instance.Changes[CommActorInfoDelta.Keys.AccessLevel]);
        if ((instance.DeltaMask & 2) != 0)
          EnumProxy<ChannelType>.Serialize(stream, (ChannelType) instance.Changes[CommActorInfoDelta.Keys.Channel]);
        if ((instance.DeltaMask & 4) != 0)
          StringProxy.Serialize(stream, (string) instance.Changes[CommActorInfoDelta.Keys.ClanTag]);
        if ((instance.DeltaMask & 8) != 0)
          Int32Proxy.Serialize(stream, (int) instance.Changes[CommActorInfoDelta.Keys.Cmid]);
        if ((instance.DeltaMask & 16) != 0)
          GameRoomProxy.Serialize(stream, (GameRoom) instance.Changes[CommActorInfoDelta.Keys.CurrentRoom]);
        if ((instance.DeltaMask & 32) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[CommActorInfoDelta.Keys.ModerationFlag]);
        if ((instance.DeltaMask & 64) != 0)
          StringProxy.Serialize(stream, (string) instance.Changes[CommActorInfoDelta.Keys.ModInformation]);
        if ((instance.DeltaMask & 128) == 0)
          return;
        StringProxy.Serialize(stream, (string) instance.Changes[CommActorInfoDelta.Keys.PlayerName]);
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static CommActorInfoDelta Deserialize(Stream bytes)
    {
      int num1 = Int32Proxy.Deserialize(bytes);
      byte num2 = ByteProxy.Deserialize(bytes);
      CommActorInfoDelta commActorInfoDelta = new CommActorInfoDelta();
      commActorInfoDelta.Id = num2;
      if (num1 != 0)
      {
        if ((num1 & 1) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.AccessLevel] = (object) EnumProxy<MemberAccessLevel>.Deserialize(bytes);
        if ((num1 & 2) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.Channel] = (object) EnumProxy<ChannelType>.Deserialize(bytes);
        if ((num1 & 4) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.ClanTag] = (object) StringProxy.Deserialize(bytes);
        if ((num1 & 8) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.Cmid] = (object) Int32Proxy.Deserialize(bytes);
        if ((num1 & 16) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.CurrentRoom] = (object) GameRoomProxy.Deserialize(bytes);
        if ((num1 & 32) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.ModerationFlag] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 64) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.ModInformation] = (object) StringProxy.Deserialize(bytes);
        if ((num1 & 128) != 0)
          commActorInfoDelta.Changes[CommActorInfoDelta.Keys.PlayerName] = (object) StringProxy.Deserialize(bytes);
      }
      return commActorInfoDelta;
    }
  }
}
