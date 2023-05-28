// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemWeaponView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
    [SerializeField]
    private int _combatRange;
    [SerializeField]
    private int _tier;
    [SerializeField]
    private int _secondaryActionReticle;
    [SerializeField]
    private int _weaponSecondaryAction;
    private int _criticalStrikeBonus;
    [SerializeField]
    private bool _hasAutoFire;
    [SerializeField]
    private int _defaultZoomMultiplier;
    [SerializeField]
    private int _minZoomMultiplier;
    [SerializeField]
    private int _maxZoomMultiplier;

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

    public int CombatRange
    {
      get => this._combatRange;
      set => this._combatRange = value;
    }

    public int Tier
    {
      get => this._tier;
      set => this._tier = value;
    }

    public int SecondaryActionReticle
    {
      get => this._secondaryActionReticle;
      set => this._secondaryActionReticle = value;
    }

    public int WeaponSecondaryAction
    {
      get => this._weaponSecondaryAction;
      set => this._weaponSecondaryAction = value;
    }

    public int CriticalStrikeBonus
    {
      get => this._criticalStrikeBonus;
      set => this._criticalStrikeBonus = value;
    }

    public float DamagePerSecond => this.RateOfFire != 0 ? (float) (this.DamagePerProjectile * this.ProjectilesPerShot / this.RateOfFire) : 0.0f;

    public bool HasAutomaticFire
    {
      get => this._hasAutoFire;
      set => this._hasAutoFire = value;
    }

    public int DefaultZoomMultiplier
    {
      get => this._defaultZoomMultiplier;
      set => this._defaultZoomMultiplier = value;
    }

    public int MinZoomMultiplier
    {
      get => this._minZoomMultiplier;
      set => this._minZoomMultiplier = value;
    }

    public int MaxZoomMultiplier
    {
      get => this._maxZoomMultiplier;
      set => this._maxZoomMultiplier = value;
    }
  }
}
