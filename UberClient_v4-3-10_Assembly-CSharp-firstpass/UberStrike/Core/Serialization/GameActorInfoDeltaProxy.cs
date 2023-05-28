// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GameActorInfoDeltaProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UnityEngine;

namespace UberStrike.Core.Serialization
{
  public static class GameActorInfoDeltaProxy
  {
    public static void Serialize(Stream stream, GameActorInfoDelta instance)
    {
      if (instance != null)
      {
        Int32Proxy.Serialize(stream, instance.DeltaMask);
        ByteProxy.Serialize(stream, instance.Id);
        if ((instance.DeltaMask & 1) != 0)
          EnumProxy<MemberAccessLevel>.Serialize(stream, (MemberAccessLevel) instance.Changes[GameActorInfoDelta.Keys.AccessLevel]);
        if ((instance.DeltaMask & 2) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[GameActorInfoDelta.Keys.ArmorPointCapacity]);
        if ((instance.DeltaMask & 4) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[GameActorInfoDelta.Keys.ArmorPoints]);
        if ((instance.DeltaMask & 8) != 0)
          EnumProxy<ChannelType>.Serialize(stream, (ChannelType) instance.Changes[GameActorInfoDelta.Keys.Channel]);
        if ((instance.DeltaMask & 16) != 0)
          StringProxy.Serialize(stream, (string) instance.Changes[GameActorInfoDelta.Keys.ClanTag]);
        if ((instance.DeltaMask & 32) != 0)
          Int32Proxy.Serialize(stream, (int) instance.Changes[GameActorInfoDelta.Keys.Cmid]);
        if ((instance.DeltaMask & 64) != 0)
          EnumProxy<FireMode>.Serialize(stream, (FireMode) instance.Changes[GameActorInfoDelta.Keys.CurrentFiringMode]);
        if ((instance.DeltaMask & 128) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[GameActorInfoDelta.Keys.CurrentWeaponSlot]);
        if ((instance.DeltaMask & 256) != 0)
          Int16Proxy.Serialize(stream, (short) instance.Changes[GameActorInfoDelta.Keys.Deaths]);
        if ((instance.DeltaMask & 512) != 0)
          ListProxy<int>.Serialize(stream, (ICollection<int>) instance.Changes[GameActorInfoDelta.Keys.FunctionalItems], new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        if ((instance.DeltaMask & 1024) != 0)
          ListProxy<int>.Serialize(stream, (ICollection<int>) instance.Changes[GameActorInfoDelta.Keys.Gear], new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        if ((instance.DeltaMask & 2048) != 0)
          Int16Proxy.Serialize(stream, (short) instance.Changes[GameActorInfoDelta.Keys.Health]);
        if ((instance.DeltaMask & 4096) != 0)
          Int16Proxy.Serialize(stream, (short) instance.Changes[GameActorInfoDelta.Keys.Kills]);
        if ((instance.DeltaMask & 8192) != 0)
          Int32Proxy.Serialize(stream, (int) instance.Changes[GameActorInfoDelta.Keys.Level]);
        if ((instance.DeltaMask & 16384) != 0)
          UInt16Proxy.Serialize(stream, (ushort) instance.Changes[GameActorInfoDelta.Keys.Ping]);
        if ((instance.DeltaMask & 32768) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[GameActorInfoDelta.Keys.PlayerId]);
        if ((instance.DeltaMask & 65536) != 0)
          StringProxy.Serialize(stream, (string) instance.Changes[GameActorInfoDelta.Keys.PlayerName]);
        if ((instance.DeltaMask & 131072) != 0)
          EnumProxy<PlayerStates>.Serialize(stream, (PlayerStates) instance.Changes[GameActorInfoDelta.Keys.PlayerState]);
        if ((instance.DeltaMask & 262144) != 0)
          ListProxy<int>.Serialize(stream, (ICollection<int>) instance.Changes[GameActorInfoDelta.Keys.QuickItems], new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        if ((instance.DeltaMask & 524288) != 0)
          ByteProxy.Serialize(stream, (byte) instance.Changes[GameActorInfoDelta.Keys.Rank]);
        if ((instance.DeltaMask & 1048576) != 0)
          ColorProxy.Serialize(stream, (Color) instance.Changes[GameActorInfoDelta.Keys.SkinColor]);
        if ((instance.DeltaMask & 2097152) != 0)
          EnumProxy<SurfaceType>.Serialize(stream, (SurfaceType) instance.Changes[GameActorInfoDelta.Keys.StepSound]);
        if ((instance.DeltaMask & 4194304) != 0)
          EnumProxy<TeamID>.Serialize(stream, (TeamID) instance.Changes[GameActorInfoDelta.Keys.TeamID]);
        if ((instance.DeltaMask & 8388608) == 0)
          return;
        ListProxy<int>.Serialize(stream, (ICollection<int>) instance.Changes[GameActorInfoDelta.Keys.Weapons], new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static GameActorInfoDelta Deserialize(Stream bytes)
    {
      int num1 = Int32Proxy.Deserialize(bytes);
      byte num2 = ByteProxy.Deserialize(bytes);
      GameActorInfoDelta gameActorInfoDelta = new GameActorInfoDelta();
      gameActorInfoDelta.Id = num2;
      if (num1 != 0)
      {
        if ((num1 & 1) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.AccessLevel] = (object) EnumProxy<MemberAccessLevel>.Deserialize(bytes);
        if ((num1 & 2) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.ArmorPointCapacity] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 4) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.ArmorPoints] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 8) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Channel] = (object) EnumProxy<ChannelType>.Deserialize(bytes);
        if ((num1 & 16) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.ClanTag] = (object) StringProxy.Deserialize(bytes);
        if ((num1 & 32) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Cmid] = (object) Int32Proxy.Deserialize(bytes);
        if ((num1 & 64) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.CurrentFiringMode] = (object) EnumProxy<FireMode>.Deserialize(bytes);
        if ((num1 & 128) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.CurrentWeaponSlot] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 256) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Deaths] = (object) Int16Proxy.Deserialize(bytes);
        if ((num1 & 512) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.FunctionalItems] = (object) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num1 & 1024) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Gear] = (object) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num1 & 2048) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Health] = (object) Int16Proxy.Deserialize(bytes);
        if ((num1 & 4096) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Kills] = (object) Int16Proxy.Deserialize(bytes);
        if ((num1 & 8192) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Level] = (object) Int32Proxy.Deserialize(bytes);
        if ((num1 & 16384) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Ping] = (object) UInt16Proxy.Deserialize(bytes);
        if ((num1 & 32768) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.PlayerId] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 65536) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.PlayerName] = (object) StringProxy.Deserialize(bytes);
        if ((num1 & 131072) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.PlayerState] = (object) EnumProxy<PlayerStates>.Deserialize(bytes);
        if ((num1 & 262144) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.QuickItems] = (object) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num1 & 524288) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Rank] = (object) ByteProxy.Deserialize(bytes);
        if ((num1 & 1048576) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.SkinColor] = (object) ColorProxy.Deserialize(bytes);
        if ((num1 & 2097152) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.StepSound] = (object) EnumProxy<SurfaceType>.Deserialize(bytes);
        if ((num1 & 4194304) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.TeamID] = (object) EnumProxy<TeamID>.Deserialize(bytes);
        if ((num1 & 8388608) != 0)
          gameActorInfoDelta.Changes[GameActorInfoDelta.Keys.Weapons] = (object) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      }
      return gameActorInfoDelta;
    }
  }
}
