// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerWeaponStatisticsView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerWeaponStatisticsView
  {
    public int MeleeTotalSplats { get; set; }

    public int HandgunTotalSplats { get; set; }

    public int MachineGunTotalSplats { get; set; }

    public int ShotgunTotalSplats { get; set; }

    public int SniperTotalSplats { get; set; }

    public int SplattergunTotalSplats { get; set; }

    public int CannonTotalSplats { get; set; }

    public int LauncherTotalSplats { get; set; }

    public int MeleeTotalShotsFired { get; set; }

    public int MeleeTotalShotsHit { get; set; }

    public int MeleeTotalDamageDone { get; set; }

    public int HandgunTotalShotsFired { get; set; }

    public int HandgunTotalShotsHit { get; set; }

    public int HandgunTotalDamageDone { get; set; }

    public int MachineGunTotalShotsFired { get; set; }

    public int MachineGunTotalShotsHit { get; set; }

    public int MachineGunTotalDamageDone { get; set; }

    public int ShotgunTotalShotsFired { get; set; }

    public int ShotgunTotalShotsHit { get; set; }

    public int ShotgunTotalDamageDone { get; set; }

    public int SniperTotalShotsFired { get; set; }

    public int SniperTotalShotsHit { get; set; }

    public int SniperTotalDamageDone { get; set; }

    public int SplattergunTotalShotsFired { get; set; }

    public int SplattergunTotalShotsHit { get; set; }

    public int SplattergunTotalDamageDone { get; set; }

    public int CannonTotalShotsFired { get; set; }

    public int CannonTotalShotsHit { get; set; }

    public int CannonTotalDamageDone { get; set; }

    public int LauncherTotalShotsFired { get; set; }

    public int LauncherTotalShotsHit { get; set; }

    public int LauncherTotalDamageDone { get; set; }

    public PlayerWeaponStatisticsView()
    {
    }

    public PlayerWeaponStatisticsView(
      int meleeTotalSplats,
      int handgunTotalSplats,
      int machineGunTotalSplats,
      int shotgunTotalSplats,
      int sniperTotalSplats,
      int splattergunTotalSplats,
      int cannonTotalSplats,
      int launcherTotalSplats,
      int meleeTotalShotsFired,
      int meleeTotalShotsHit,
      int meleeTotalDamageDone,
      int handgunTotalShotsFired,
      int handgunTotalShotsHit,
      int handgunTotalDamageDone,
      int machineGunTotalShotsFired,
      int machineGunTotalShotsHit,
      int machineGunTotalDamageDone,
      int shotgunTotalShotsFired,
      int shotgunTotalShotsHit,
      int shotgunTotalDamageDone,
      int sniperTotalShotsFired,
      int sniperTotalShotsHit,
      int sniperTotalDamageDone,
      int splattergunTotalShotsFired,
      int splattergunTotalShotsHit,
      int splattergunTotalDamageDone,
      int cannonTotalShotsFired,
      int cannonTotalShotsHit,
      int cannonTotalDamageDone,
      int launcherTotalShotsFired,
      int launcherTotalShotsHit,
      int launcherTotalDamageDone)
    {
      this.CannonTotalDamageDone = cannonTotalDamageDone;
      this.CannonTotalShotsFired = cannonTotalShotsFired;
      this.CannonTotalShotsHit = cannonTotalShotsHit;
      this.CannonTotalSplats = cannonTotalSplats;
      this.HandgunTotalDamageDone = handgunTotalDamageDone;
      this.HandgunTotalShotsFired = handgunTotalShotsFired;
      this.HandgunTotalShotsHit = handgunTotalShotsHit;
      this.HandgunTotalSplats = handgunTotalSplats;
      this.LauncherTotalDamageDone = launcherTotalDamageDone;
      this.LauncherTotalShotsFired = launcherTotalShotsFired;
      this.LauncherTotalShotsHit = launcherTotalShotsHit;
      this.LauncherTotalSplats = launcherTotalSplats;
      this.MachineGunTotalDamageDone = machineGunTotalDamageDone;
      this.MachineGunTotalShotsFired = machineGunTotalShotsFired;
      this.MachineGunTotalShotsHit = machineGunTotalShotsHit;
      this.MachineGunTotalSplats = machineGunTotalSplats;
      this.MeleeTotalDamageDone = meleeTotalDamageDone;
      this.MeleeTotalShotsFired = meleeTotalShotsFired;
      this.MeleeTotalShotsHit = meleeTotalShotsHit;
      this.MeleeTotalSplats = meleeTotalSplats;
      this.ShotgunTotalDamageDone = shotgunTotalDamageDone;
      this.ShotgunTotalShotsFired = shotgunTotalShotsFired;
      this.ShotgunTotalShotsHit = shotgunTotalShotsHit;
      this.ShotgunTotalSplats = shotgunTotalSplats;
      this.SniperTotalDamageDone = sniperTotalDamageDone;
      this.SniperTotalShotsFired = sniperTotalShotsFired;
      this.SniperTotalShotsHit = sniperTotalShotsHit;
      this.SniperTotalSplats = sniperTotalSplats;
      this.SplattergunTotalDamageDone = splattergunTotalDamageDone;
      this.SplattergunTotalShotsFired = splattergunTotalShotsFired;
      this.SplattergunTotalShotsHit = splattergunTotalShotsHit;
      this.SplattergunTotalSplats = splattergunTotalSplats;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[PlayerWeaponStatisticsView: ");
      stringBuilder.Append("[CannonTotalDamageDone: ");
      stringBuilder.Append(this.CannonTotalDamageDone);
      stringBuilder.Append("][CannonTotalShotsFired: ");
      stringBuilder.Append(this.CannonTotalShotsFired);
      stringBuilder.Append("][CannonTotalShotsHit: ");
      stringBuilder.Append(this.CannonTotalShotsHit);
      stringBuilder.Append("][CannonTotalSplats: ");
      stringBuilder.Append(this.CannonTotalSplats);
      stringBuilder.Append("][HandgunTotalDamageDone: ");
      stringBuilder.Append(this.HandgunTotalDamageDone);
      stringBuilder.Append("][HandgunTotalShotsFired: ");
      stringBuilder.Append(this.HandgunTotalShotsFired);
      stringBuilder.Append("][HandgunTotalShotsHit: ");
      stringBuilder.Append(this.HandgunTotalShotsHit);
      stringBuilder.Append("][HandgunTotalSplats: ");
      stringBuilder.Append(this.HandgunTotalSplats);
      stringBuilder.Append("][LauncherTotalDamageDone: ");
      stringBuilder.Append(this.LauncherTotalDamageDone);
      stringBuilder.Append("][LauncherTotalShotsFired: ");
      stringBuilder.Append(this.LauncherTotalShotsFired);
      stringBuilder.Append("][LauncherTotalShotsHit: ");
      stringBuilder.Append(this.LauncherTotalShotsHit);
      stringBuilder.Append("][LauncherTotalSplats: ");
      stringBuilder.Append(this.LauncherTotalSplats);
      stringBuilder.Append("][MachineGunTotalDamageDone: ");
      stringBuilder.Append(this.MachineGunTotalDamageDone);
      stringBuilder.Append("][MachineGunTotalShotsFired: ");
      stringBuilder.Append(this.MachineGunTotalShotsFired);
      stringBuilder.Append("][MachineGunTotalShotsHit: ");
      stringBuilder.Append(this.MachineGunTotalShotsHit);
      stringBuilder.Append("][MachineGunTotalSplats: ");
      stringBuilder.Append(this.MachineGunTotalSplats);
      stringBuilder.Append("][MeleeTotalDamageDone: ");
      stringBuilder.Append(this.MeleeTotalDamageDone);
      stringBuilder.Append("][MeleeTotalShotsFired: ");
      stringBuilder.Append(this.MeleeTotalShotsFired);
      stringBuilder.Append("][MeleeTotalShotsHit: ");
      stringBuilder.Append(this.MeleeTotalShotsHit);
      stringBuilder.Append("][MeleeTotalSplats: ");
      stringBuilder.Append(this.MeleeTotalSplats);
      stringBuilder.Append("][ShotgunTotalDamageDone: ");
      stringBuilder.Append(this.ShotgunTotalDamageDone);
      stringBuilder.Append("][ShotgunTotalShotsFired: ");
      stringBuilder.Append(this.ShotgunTotalShotsFired);
      stringBuilder.Append("][ShotgunTotalShotsHit: ");
      stringBuilder.Append(this.ShotgunTotalShotsHit);
      stringBuilder.Append("][ShotgunTotalSplats: ");
      stringBuilder.Append(this.ShotgunTotalSplats);
      stringBuilder.Append("][SniperTotalDamageDone: ");
      stringBuilder.Append(this.SniperTotalDamageDone);
      stringBuilder.Append("][SniperTotalShotsFired: ");
      stringBuilder.Append(this.SniperTotalShotsFired);
      stringBuilder.Append("][SniperTotalShotsHit: ");
      stringBuilder.Append(this.SniperTotalShotsHit);
      stringBuilder.Append("][SniperTotalSplats: ");
      stringBuilder.Append(this.SniperTotalSplats);
      stringBuilder.Append("][SplattergunTotalDamageDone: ");
      stringBuilder.Append(this.SplattergunTotalDamageDone);
      stringBuilder.Append("][SplattergunTotalShotsFired: ");
      stringBuilder.Append(this.SplattergunTotalShotsFired);
      stringBuilder.Append("][SplattergunTotalShotsHit: ");
      stringBuilder.Append(this.SplattergunTotalShotsHit);
      stringBuilder.Append("][SplattergunTotalSplats: ");
      stringBuilder.Append(this.SplattergunTotalSplats);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
