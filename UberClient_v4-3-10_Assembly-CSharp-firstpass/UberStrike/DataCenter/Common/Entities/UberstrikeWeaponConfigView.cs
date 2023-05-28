// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeWeaponConfigView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeWeaponConfigView
  {
    public int DamageKnockback { get; set; }

    public int DamagePerProjectile { get; set; }

    public int AccuracySpread { get; set; }

    public int RecoilKickback { get; set; }

    public int StartAmmo { get; set; }

    public int MaxAmmo { get; set; }

    public int MissileTimeToDetonate { get; set; }

    public int MissileForceImpulse { get; set; }

    public int MissileBounciness { get; set; }

    public int SplashRadius { get; set; }

    public int ProjectilesPerShot { get; set; }

    public int ProjectileSpeed { get; set; }

    public int RateOfFire { get; set; }

    public int RecoilMovement { get; set; }

    public int LevelRequired { get; set; }

    public UberstrikeWeaponConfigView()
    {
    }

    public UberstrikeWeaponConfigView(
      int damageKnockback,
      int damagePerProjectile,
      int accuracySpread,
      int recoilKickback,
      int startAmmo,
      int maxAmmo,
      int missileTimeToDetonate,
      int missileForceImpulse,
      int missileBounciness,
      int rateOfire,
      int splashRadius,
      int projectilesPerShot,
      int projectileSpeed,
      int recoilMovement)
    {
      this.DamageKnockback = damageKnockback;
      this.DamagePerProjectile = damagePerProjectile;
      this.AccuracySpread = accuracySpread;
      this.RecoilKickback = recoilKickback;
      this.StartAmmo = startAmmo;
      this.MaxAmmo = maxAmmo;
      this.MissileTimeToDetonate = missileTimeToDetonate;
      this.MissileForceImpulse = missileForceImpulse;
      this.MissileBounciness = missileBounciness;
      this.SplashRadius = splashRadius;
      this.ProjectilesPerShot = projectilesPerShot;
      this.ProjectileSpeed = projectileSpeed;
      this.RateOfFire = rateOfire;
      this.RecoilMovement = recoilMovement;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeWeaponConfigView: [DamageKnockback: ");
      stringBuilder.Append(this.DamageKnockback);
      stringBuilder.Append("][DamagePerProjectile: ");
      stringBuilder.Append(this.DamagePerProjectile);
      stringBuilder.Append("][AccuracySpread: ");
      stringBuilder.Append(this.AccuracySpread);
      stringBuilder.Append("][RecoilKickback: ");
      stringBuilder.Append(this.RecoilKickback);
      stringBuilder.Append("][StartAmmo: ");
      stringBuilder.Append(this.StartAmmo);
      stringBuilder.Append("][MaxAmmo: ");
      stringBuilder.Append(this.MaxAmmo);
      stringBuilder.Append("][MissileTimeToDetonate: ");
      stringBuilder.Append(this.MissileTimeToDetonate);
      stringBuilder.Append("][MissileForceImpulse: ");
      stringBuilder.Append(this.MissileForceImpulse);
      stringBuilder.Append("][MissileBounciness: ");
      stringBuilder.Append(this.MissileBounciness);
      stringBuilder.Append("][RateOfFire: ");
      stringBuilder.Append(this.RateOfFire);
      stringBuilder.Append("][SplashRadius: ");
      stringBuilder.Append(this.SplashRadius);
      stringBuilder.Append("][ProjectilesPerShot: ");
      stringBuilder.Append(this.ProjectilesPerShot);
      stringBuilder.Append("][ProjectileSpeed: ");
      stringBuilder.Append(this.ProjectileSpeed);
      stringBuilder.Append("][RecoilMovement: ");
      stringBuilder.Append(this.RecoilMovement);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
