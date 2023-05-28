// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.GameActorInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Core.Models
{
  [Synchronizable]
  [Serializable]
  public class GameActorInfo
  {
    public int Cmid { get; set; }

    public string PlayerName { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }

    public ChannelType Channel { get; set; }

    public string ClanTag { get; set; }

    public byte Rank { get; set; }

    public byte PlayerId { get; set; }

    public PlayerStates PlayerState { get; set; }

    public short Health { get; set; }

    public TeamID TeamID { get; set; }

    public int Level { get; set; }

    public ushort Ping { get; set; }

    public byte CurrentWeaponSlot { get; set; }

    public FireMode CurrentFiringMode { get; set; }

    public byte ArmorPoints { get; set; }

    public byte ArmorPointCapacity { get; set; }

    public Color SkinColor { get; set; }

    public short Kills { get; set; }

    public short Deaths { get; set; }

    public List<int> Weapons { get; set; }

    public List<int> Gear { get; set; }

    public List<int> FunctionalItems { get; set; }

    public List<int> QuickItems { get; set; }

    public SurfaceType StepSound { get; set; }

    public bool IsFiring => this.Is(PlayerStates.Shooting);

    public bool IsReadyForGame => this.Is(PlayerStates.Ready);

    public bool IsOnline => !this.Is(PlayerStates.Offline);

    public int CurrentWeaponID => this.Weapons == null || this.Weapons.Count <= (int) this.CurrentWeaponSlot ? 0 : this.Weapons[(int) this.CurrentWeaponSlot];

    public bool IsAlive => (this.PlayerState & PlayerStates.Dead) == PlayerStates.None;

    public bool IsSpectator => (this.PlayerState & PlayerStates.Spectator) != 0;

    public GameActorInfo()
    {
      this.Weapons = new List<int>() { 0, 0, 0, 0 };
      this.Gear = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
      this.QuickItems = new List<int>() { 0, 0, 0 };
      this.FunctionalItems = new List<int>() { 0, 0, 0 };
    }

    public bool Is(PlayerStates state) => (this.PlayerState & state) != 0;

    public float GetAbsorptionRate() => 0.66f;

    public void Damage(short damage, BodyPart part, out short healthDamage, out byte armorDamage)
    {
      if (this.ArmorPoints > (byte) 0)
      {
        int num = Mathf.CeilToInt(this.GetAbsorptionRate() * (float) damage);
        armorDamage = (byte) Mathf.Clamp(num, 0, (int) this.ArmorPoints);
        healthDamage = (short) ((int) damage - (int) armorDamage);
      }
      else
      {
        armorDamage = (byte) 0;
        healthDamage = damage;
      }
    }
  }
}
