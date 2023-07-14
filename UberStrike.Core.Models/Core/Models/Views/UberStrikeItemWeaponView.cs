// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemWeaponView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class UberStrikeItemWeaponView : BaseUberStrikeItemView
  {
    [SerializeField]
    private int _damageKnockback;
    [SerializeField]
    private int _damagePerProjectile;
    [SerializeField]
    private int _accuracySpread;
    [SerializeField]
    private int _recoilKickback;
    [SerializeField]
    private int _startAmmo;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _missileTimeToDetonate;
    [SerializeField]
    private int _missileForceImpulse;
    [SerializeField]
    private int _missileBounciness;
    [SerializeField]
    private int _splashRadius;
    [SerializeField]
    private int _projectilesPerShot;
    [SerializeField]
    private int _projectileSpeed;
    [SerializeField]
    private int _rateOfFire;
    [SerializeField]
    private int _recoilMovement;

    public override UberstrikeItemType ItemType => UberstrikeItemType.Weapon;

    public int DamageKnockback
    {
      get => this._damageKnockback;
      set => this._damageKnockback = value;
    }

    public int DamagePerProjectile
    {
      get => this._damagePerProjectile;
      set => this._damagePerProjectile = value;
    }

    public int AccuracySpread
    {
      get => this._accuracySpread;
      set => this._accuracySpread = value;
    }

    public int RecoilKickback
    {
      get => this._recoilKickback;
      set => this._recoilKickback = value;
    }

    public int StartAmmo
    {
      get => this._startAmmo;
      set => this._startAmmo = value;
    }

    public int MaxAmmo
    {
      get => this._maxAmmo;
      set => this._maxAmmo = value;
    }

    public int MissileTimeToDetonate
    {
      get => this._missileTimeToDetonate;
      set => this._missileTimeToDetonate = value;
    }

    public int MissileForceImpulse
    {
      get => this._missileForceImpulse;
      set => this._missileForceImpulse = value;
    }

    public int MissileBounciness
    {
      get => this._missileBounciness;
      set => this._missileBounciness = value;
    }

    public int SplashRadius
    {
      get => this._splashRadius;
      set => this._splashRadius = value;
    }

    public int ProjectilesPerShot
    {
      get => this._projectilesPerShot;
      set => this._projectilesPerShot = value;
    }

    public int ProjectileSpeed
    {
      get => this._projectileSpeed;
      set => this._projectileSpeed = value;
    }

    public int RateOfFire
    {
      get => this._rateOfFire;
      set => this._rateOfFire = value;
    }

    public int RecoilMovement
    {
      get => this._recoilMovement;
      set => this._recoilMovement = value;
    }
  }
}
