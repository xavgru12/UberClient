// Decompiled with JetBrains decompiler
// Type: WeaponConfigurationHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UnityEngine;

public static class WeaponConfigurationHelper
{
  static WeaponConfigurationHelper()
  {
    WeaponConfigurationHelper.MaxSplashRadius = 1f;
    WeaponConfigurationHelper.MaxRecoilKickback = 1f;
    WeaponConfigurationHelper.MaxRateOfFire = 1f;
    WeaponConfigurationHelper.MaxProjectileSpeed = 1f;
    WeaponConfigurationHelper.MaxAccuracySpread = 1f;
    WeaponConfigurationHelper.MaxDamage = 1f;
    WeaponConfigurationHelper.MaxAmmo = 1f;
  }

  public static void UpdateWeaponStatistics(UberStrikeItemShopClientView shopView)
  {
    WeaponConfigurationHelper.MaxSplashRadius = 1f;
    WeaponConfigurationHelper.MaxRecoilKickback = 1f;
    WeaponConfigurationHelper.MaxRateOfFire = 1f;
    WeaponConfigurationHelper.MaxProjectileSpeed = 1f;
    WeaponConfigurationHelper.MaxAccuracySpread = 1f;
    WeaponConfigurationHelper.MaxDamage = 1f;
    WeaponConfigurationHelper.MaxAmmo = 1f;
    foreach (UberStrikeItemWeaponView weaponItem in shopView.WeaponItems)
    {
      WeaponConfigurationHelper.MaxAmmo = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.MaxAmmo * 1.1f), WeaponConfigurationHelper.MaxAmmo);
      WeaponConfigurationHelper.MaxSplashRadius = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.SplashRadius * 1.1f), WeaponConfigurationHelper.MaxSplashRadius);
      WeaponConfigurationHelper.MaxRecoilKickback = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.RecoilKickback * 1.1f), WeaponConfigurationHelper.MaxRecoilKickback);
      WeaponConfigurationHelper.MaxRateOfFire = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.RateOfFire * 1.1f), WeaponConfigurationHelper.MaxRateOfFire);
      WeaponConfigurationHelper.MaxProjectileSpeed = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.ProjectileSpeed * 1.1f), WeaponConfigurationHelper.MaxProjectileSpeed);
      WeaponConfigurationHelper.MaxAccuracySpread = Mathf.Max((float) Mathf.RoundToInt((float) weaponItem.AccuracySpread * 1.1f), WeaponConfigurationHelper.MaxAccuracySpread);
      WeaponConfigurationHelper.MaxDamage = Mathf.Max((float) Mathf.RoundToInt((float) (weaponItem.DamagePerProjectile * weaponItem.ProjectilesPerShot) * 1.1f), WeaponConfigurationHelper.MaxDamage);
    }
    WeaponConfigurationHelper.MaxSplashRadius /= 100f;
    WeaponConfigurationHelper.MaxRateOfFire /= 1000f;
    WeaponConfigurationHelper.MaxAccuracySpread /= 10f;
  }

  public static float MaxAmmo { get; private set; }

  public static float MaxDamage { get; private set; }

  public static float MaxAccuracySpread { get; private set; }

  public static float MaxProjectileSpeed { get; private set; }

  public static float MaxRateOfFire { get; private set; }

  public static float MaxRecoilKickback { get; private set; }

  public static float MaxSplashRadius { get; private set; }

  public static float GetAmmoNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.MaxAmmo / WeaponConfigurationHelper.MaxAmmo : 0.0f;

  public static float GetDamageNormalized(UberStrikeItemWeaponView view) => view != null ? (float) (view.DamagePerProjectile * view.ProjectilesPerShot) / WeaponConfigurationHelper.MaxDamage : 0.0f;

  public static float GetAccuracySpreadNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.AccuracySpread / 10f / WeaponConfigurationHelper.MaxAccuracySpread : 0.0f;

  public static float GetProjectileSpeedNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.ProjectileSpeed / WeaponConfigurationHelper.MaxProjectileSpeed : 0.0f;

  public static float GetRateOfFireNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.RateOfFire / 1000f / WeaponConfigurationHelper.MaxRateOfFire : 0.0f;

  public static float GetRecoilKickbackNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.RecoilKickback / WeaponConfigurationHelper.MaxRecoilKickback : 0.0f;

  public static float GetSplashRadiusNormalized(UberStrikeItemWeaponView view) => view != null ? (float) view.SplashRadius / 100f / WeaponConfigurationHelper.MaxSplashRadius : 0.0f;

  public static float GetAmmo(UberStrikeItemWeaponView view) => view == null ? 0.0f : (float) view.MaxAmmo;

  public static float GetDamage(UberStrikeItemWeaponView view) => view == null ? 0.0f : (float) (view.DamagePerProjectile * view.ProjectilesPerShot);

  public static float GetAccuracySpread(UberStrikeItemWeaponView view) => view != null ? (float) view.AccuracySpread / 10f : 0.0f;

  public static float GetProjectileSpeed(UberStrikeItemWeaponView view) => view == null ? 0.0f : (float) view.ProjectileSpeed;

  public static float GetRateOfFire(UberStrikeItemWeaponView view) => view != null ? (float) view.RateOfFire / 1000f : 1f;

  public static float GetRecoilKickback(UberStrikeItemWeaponView view) => view == null ? 0.0f : (float) view.RecoilKickback;

  public static float GetRecoilMovement(UberStrikeItemWeaponView view) => view != null ? (float) view.RecoilMovement / 100f : 0.0f;

  public static float GetSplashRadius(UberStrikeItemWeaponView view) => view != null ? (float) view.SplashRadius / 100f : 0.0f;

  public static float GetCriticalStrikeBonus(WeaponItemConfiguration view) => view != null ? (float) view.CriticalStrikeBonus / 100f : 0.0f;
}
