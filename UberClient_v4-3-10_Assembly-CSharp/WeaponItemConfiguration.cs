// Decompiled with JetBrains decompiler
// Type: WeaponItemConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Models.Views;
using UnityEngine;

[Serializable]
public class WeaponItemConfiguration : UberStrikeItemWeaponView
{
  [CustomProperty("CriticalStrikeBonus")]
  private int _criticalStrikeBonus;
  [CustomProperty("SwitchDelay")]
  private int _switchDelay = 500;
  [SerializeField]
  private DamageEffectType _damageEffectFlag;
  [SerializeField]
  private float _damageEffectValue;
  [SerializeField]
  private bool _hasAutomaticFire;
  [SerializeField]
  private ZoomInfo _zoomInformation;
  [SerializeField]
  private Vector3 _ironSightPosition;
  [SerializeField]
  private WeaponSecondaryAction _secondaryAction;
  [SerializeField]
  private bool _showReticleForPrimaryAction;
  [SerializeField]
  private ReticuleForSecondaryAction _secondaryActionReticule;
  [SerializeField]
  private CombatRangeCategory _combatRange;
  [SerializeField]
  private int _tier;
  [SerializeField]
  private int _minProjectileDistance = 2;
  [SerializeField]
  private int _maxConcurrentProjectiles;
  [SerializeField]
  private Vector3 _position;
  [SerializeField]
  private Vector3 _rotation;
  [SerializeField]
  private ParticleConfigurationType _impactEffect;

  public int CriticalStrikeBonus
  {
    get => this._criticalStrikeBonus;
    set => this._criticalStrikeBonus = value;
  }

  public int SwitchDelayMilliSeconds
  {
    get => this._switchDelay;
    set => this._switchDelay = value;
  }

  public int MaxConcurrentProjectiles
  {
    get => this._maxConcurrentProjectiles;
    set => this._maxConcurrentProjectiles = value;
  }

  public int MinProjectileDistance
  {
    get => this._minProjectileDistance;
    set => this._minProjectileDistance = value;
  }

  public Vector3 Position
  {
    get => this._position;
    set => this._position = value;
  }

  public Vector3 Rotation
  {
    get => this._rotation;
    set => this._rotation = value;
  }

  public bool ShowReticleForPrimaryAction
  {
    get => this._showReticleForPrimaryAction;
    set => this._showReticleForPrimaryAction = value;
  }

  public ReticuleForSecondaryAction SecondaryActionReticle
  {
    get => this._secondaryActionReticule;
    set => this._secondaryActionReticule = value;
  }

  public WeaponSecondaryAction SecondaryAction
  {
    get => this._secondaryAction;
    set => this._secondaryAction = value;
  }

  public Vector3 IronSightPosition
  {
    get => this._ironSightPosition;
    set => this._ironSightPosition = value;
  }

  public ZoomInfo ZoomInformation
  {
    get => this._zoomInformation;
    set => this._zoomInformation = value;
  }

  public bool HasAutomaticFire
  {
    get => this._hasAutomaticFire;
    set => this._hasAutomaticFire = value;
  }

  public DamageEffectType DamageEffectFlag
  {
    get => this._damageEffectFlag;
    set => this._damageEffectFlag = value;
  }

  public float DamageEffectValue
  {
    get => this._damageEffectValue;
    set => this._damageEffectValue = value;
  }

  public ParticleConfigurationType ParticleEffect
  {
    get => this._impactEffect;
    set => this._impactEffect = value;
  }

  public CombatRangeCategory CombatRange
  {
    get => this._combatRange;
    set => this._combatRange = value;
  }

  public int Tier
  {
    get => this._tier;
    set => this._tier = value;
  }

  public float DPS => this.RateOfFire == 0 ? 0.0f : (float) (this.DamagePerProjectile * this.ProjectilesPerShot / this.RateOfFire);
}
