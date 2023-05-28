// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GameActorInfoProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class GameActorInfoProxy
  {
    public static void Serialize(Stream stream, GameActorInfo instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<MemberAccessLevel>.Serialize((Stream) bytes, instance.AccessLevel);
        ByteProxy.Serialize((Stream) bytes, instance.ArmorPointCapacity);
        ByteProxy.Serialize((Stream) bytes, instance.ArmorPoints);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
        if (instance.ClanTag != null)
          StringProxy.Serialize((Stream) bytes, instance.ClanTag);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        EnumProxy<FireMode>.Serialize((Stream) bytes, instance.CurrentFiringMode);
        ByteProxy.Serialize((Stream) bytes, instance.CurrentWeaponSlot);
        Int16Proxy.Serialize((Stream) bytes, instance.Deaths);
        if (instance.FunctionalItems != null)
          ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.FunctionalItems, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 2;
        if (instance.Gear != null)
          ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.Gear, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 4;
        Int16Proxy.Serialize((Stream) bytes, instance.Health);
        Int16Proxy.Serialize((Stream) bytes, instance.Kills);
        Int32Proxy.Serialize((Stream) bytes, instance.Level);
        UInt16Proxy.Serialize((Stream) bytes, instance.Ping);
        ByteProxy.Serialize((Stream) bytes, instance.PlayerId);
        if (instance.PlayerName != null)
          StringProxy.Serialize((Stream) bytes, instance.PlayerName);
        else
          num |= 8;
        EnumProxy<PlayerStates>.Serialize((Stream) bytes, instance.PlayerState);
        if (instance.QuickItems != null)
          ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.QuickItems, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 16;
        ByteProxy.Serialize((Stream) bytes, instance.Rank);
        ColorProxy.Serialize((Stream) bytes, instance.SkinColor);
        EnumProxy<SurfaceType>.Serialize((Stream) bytes, instance.StepSound);
        EnumProxy<TeamID>.Serialize((Stream) bytes, instance.TeamID);
        if (instance.Weapons != null)
          ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.Weapons, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 32;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static GameActorInfo Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GameActorInfo gameActorInfo = new GameActorInfo();
      gameActorInfo.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
      gameActorInfo.ArmorPointCapacity = ByteProxy.Deserialize(bytes);
      gameActorInfo.ArmorPoints = ByteProxy.Deserialize(bytes);
      gameActorInfo.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
      if ((num & 1) != 0)
        gameActorInfo.ClanTag = StringProxy.Deserialize(bytes);
      gameActorInfo.Cmid = Int32Proxy.Deserialize(bytes);
      gameActorInfo.CurrentFiringMode = EnumProxy<FireMode>.Deserialize(bytes);
      gameActorInfo.CurrentWeaponSlot = ByteProxy.Deserialize(bytes);
      gameActorInfo.Deaths = Int16Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        gameActorInfo.FunctionalItems = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      if ((num & 4) != 0)
        gameActorInfo.Gear = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      gameActorInfo.Health = Int16Proxy.Deserialize(bytes);
      gameActorInfo.Kills = Int16Proxy.Deserialize(bytes);
      gameActorInfo.Level = Int32Proxy.Deserialize(bytes);
      gameActorInfo.Ping = UInt16Proxy.Deserialize(bytes);
      gameActorInfo.PlayerId = ByteProxy.Deserialize(bytes);
      if ((num & 8) != 0)
        gameActorInfo.PlayerName = StringProxy.Deserialize(bytes);
      gameActorInfo.PlayerState = EnumProxy<PlayerStates>.Deserialize(bytes);
      if ((num & 16) != 0)
        gameActorInfo.QuickItems = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      gameActorInfo.Rank = ByteProxy.Deserialize(bytes);
      gameActorInfo.SkinColor = ColorProxy.Deserialize(bytes);
      gameActorInfo.StepSound = EnumProxy<SurfaceType>.Deserialize(bytes);
      gameActorInfo.TeamID = EnumProxy<TeamID>.Deserialize(bytes);
      if ((num & 32) != 0)
        gameActorInfo.Weapons = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      return gameActorInfo;
    }
  }
}
