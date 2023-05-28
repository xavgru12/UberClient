// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.GameActorInfoDelta
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Core.Models
{
  public class GameActorInfoDelta
  {
    public readonly Dictionary<GameActorInfoDelta.Keys, object> Changes = new Dictionary<GameActorInfoDelta.Keys, object>();

    public int DeltaMask { get; set; }

    public byte Id { get; set; }

    public void Apply(GameActorInfo instance)
    {
      foreach (KeyValuePair<GameActorInfoDelta.Keys, object> change in this.Changes)
      {
        switch (change.Key)
        {
          case GameActorInfoDelta.Keys.AccessLevel:
            instance.AccessLevel = (MemberAccessLevel) change.Value;
            continue;
          case GameActorInfoDelta.Keys.ArmorPointCapacity:
            instance.ArmorPointCapacity = (byte) change.Value;
            continue;
          case GameActorInfoDelta.Keys.ArmorPoints:
            instance.ArmorPoints = (byte) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Channel:
            instance.Channel = (ChannelType) change.Value;
            continue;
          case GameActorInfoDelta.Keys.ClanTag:
            instance.ClanTag = (string) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Cmid:
            instance.Cmid = (int) change.Value;
            continue;
          case GameActorInfoDelta.Keys.CurrentFiringMode:
            instance.CurrentFiringMode = (FireMode) change.Value;
            continue;
          case GameActorInfoDelta.Keys.CurrentWeaponSlot:
            instance.CurrentWeaponSlot = (byte) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Deaths:
            instance.Deaths = (short) change.Value;
            continue;
          case GameActorInfoDelta.Keys.FunctionalItems:
            instance.FunctionalItems = (List<int>) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Gear:
            instance.Gear = (List<int>) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Health:
            instance.Health = (short) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Kills:
            instance.Kills = (short) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Level:
            instance.Level = (int) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Ping:
            instance.Ping = (ushort) change.Value;
            continue;
          case GameActorInfoDelta.Keys.PlayerId:
            instance.PlayerId = (byte) change.Value;
            continue;
          case GameActorInfoDelta.Keys.PlayerName:
            instance.PlayerName = (string) change.Value;
            continue;
          case GameActorInfoDelta.Keys.PlayerState:
            instance.PlayerState = (PlayerStates) change.Value;
            continue;
          case GameActorInfoDelta.Keys.QuickItems:
            instance.QuickItems = (List<int>) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Rank:
            instance.Rank = (byte) change.Value;
            continue;
          case GameActorInfoDelta.Keys.SkinColor:
            instance.SkinColor = (Color) change.Value;
            continue;
          case GameActorInfoDelta.Keys.StepSound:
            instance.StepSound = (SurfaceType) change.Value;
            continue;
          case GameActorInfoDelta.Keys.TeamID:
            instance.TeamID = (TeamID) change.Value;
            continue;
          case GameActorInfoDelta.Keys.Weapons:
            instance.Weapons = (List<int>) change.Value;
            continue;
          default:
            continue;
        }
      }
    }

    public enum Keys
    {
      AccessLevel,
      ArmorPointCapacity,
      ArmorPoints,
      Channel,
      ClanTag,
      Cmid,
      CurrentFiringMode,
      CurrentWeaponSlot,
      Deaths,
      FunctionalItems,
      Gear,
      Health,
      Kills,
      Level,
      Ping,
      PlayerId,
      PlayerName,
      PlayerState,
      QuickItems,
      Rank,
      SkinColor,
      StepSound,
      TeamID,
      Weapons,
    }
  }
}
