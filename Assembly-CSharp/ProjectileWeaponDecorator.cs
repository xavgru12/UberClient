// Decompiled with JetBrains decompiler
// Type: ProjectileWeaponDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ProjectileWeaponDecorator : BaseWeaponDecorator
{
  [SerializeField]
  private Projectile _missle;
  [SerializeField]
  private AudioClip _missleExplosionSound;
  private float _missileTimeOut;

  public Projectile Missle => this._missle;

  public float MissileTimeOut => this._missileTimeOut;

  public AudioClip ExplosionSound => this._missleExplosionSound;

  public void ShowExplosionEffect(
    Vector3 position,
    Vector3 normal,
    ParticleConfigurationType explosionEffect)
  {
    this.ShowShootEffect(new RaycastHit[0]);
    Singleton<ExplosionManager>.Instance.ShowExplosionEffect(position, normal, this.tag, explosionEffect);
  }

  public void SetMissileTimeOut(float timeOut) => this._missileTimeOut = timeOut;
}
