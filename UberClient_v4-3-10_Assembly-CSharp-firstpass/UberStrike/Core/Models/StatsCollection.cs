// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.StatsCollection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class StatsCollection
  {
    public int Headshots { get; set; }

    public int Nutshots { get; set; }

    public int ConsecutiveSnipes { get; set; }

    public int Xp { get; set; }

    public int Deaths { get; set; }

    public int DamageReceived { get; set; }

    public int ArmorPickedUp { get; set; }

    public int HealthPickedUp { get; set; }

    public int MeleeKills { get; set; }

    public int MeleeShotsFired { get; set; }

    public int MeleeShotsHit { get; set; }

    public int MeleeDamageDone { get; set; }

    public int MachineGunKills { get; set; }

    public int MachineGunShotsFired { get; set; }

    public int MachineGunShotsHit { get; set; }

    public int MachineGunDamageDone { get; set; }

    public int ShotgunSplats { get; set; }

    public int ShotgunShotsFired { get; set; }

    public int ShotgunShotsHit { get; set; }

    public int ShotgunDamageDone { get; set; }

    public int SniperKills { get; set; }

    public int SniperShotsFired { get; set; }

    public int SniperShotsHit { get; set; }

    public int SniperDamageDone { get; set; }

    public int SplattergunKills { get; set; }

    public int SplattergunShotsFired { get; set; }

    public int SplattergunShotsHit { get; set; }

    public int SplattergunDamageDone { get; set; }

    public int CannonKills { get; set; }

    public int CannonShotsFired { get; set; }

    public int CannonShotsHit { get; set; }

    public int CannonDamageDone { get; set; }

    public int LauncherKills { get; set; }

    public int LauncherShotsFired { get; set; }

    public int LauncherShotsHit { get; set; }

    public int LauncherDamageDone { get; set; }

    public int Suicides { get; set; }

    public int Points { get; set; }

    public int GetKills() => this.MeleeKills + this.MachineGunKills + this.ShotgunSplats + this.SniperKills + this.SplattergunKills + this.CannonKills + this.LauncherKills - this.Suicides;

    public int GetShots() => this.MeleeShotsFired + this.MachineGunShotsFired + this.ShotgunShotsFired + this.SniperShotsFired + this.SplattergunShotsFired + this.CannonShotsFired + this.LauncherShotsFired;

    public int GetHits() => this.MeleeShotsHit + this.MachineGunShotsHit + this.ShotgunShotsHit + this.SniperShotsHit + this.SplattergunShotsHit + this.CannonShotsHit + this.LauncherShotsHit;

    public int GetDamageDealt() => this.MeleeDamageDone + this.MachineGunDamageDone + this.ShotgunDamageDone + this.SniperDamageDone + this.SplattergunDamageDone + this.CannonDamageDone + this.LauncherDamageDone;
  }
}
