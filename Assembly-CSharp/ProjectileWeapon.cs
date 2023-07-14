// Decompiled with JetBrains decompiler
// Type: ProjectileWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ProjectileWeapon : BaseWeaponLogic
{
  private ProjectileWeaponDecorator _decorator;

  public ProjectileWeapon(
    WeaponItem item,
    ProjectileWeaponDecorator decorator,
    IWeaponController controller)
    : base(item, controller)
  {
    this._decorator = decorator;
    this.MaxConcurrentProjectiles = item.Configuration.MaxConcurrentProjectiles;
    this.MinProjectileDistance = item.Configuration.MinProjectileDistance;
    this.ExplosionType = item.Configuration.ParticleEffect;
    this.ProjetileCountPerShoot = item.Configuration.ProjectilesPerShot;
  }

  public event Action<ProjectileInfo> OnProjectileShoot;

  public override BaseWeaponDecorator Decorator => (BaseWeaponDecorator) this._decorator;

  public int MaxConcurrentProjectiles { get; private set; }

  public int MinProjectileDistance { get; private set; }

  public int ProjetileCountPerShoot { get; set; }

  public bool HasProjectileLimit => this.MaxConcurrentProjectiles > 0;

  public ParticleConfigurationType ExplosionType { get; private set; }

  public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    hits = (CmunePairList<BaseGameProp, ShotPoint>) null;
    RaycastHit hitInfo;
    if (this.MinProjectileDistance > 0 && Physics.Raycast(ray.origin, ray.direction, out hitInfo, (float) this.MinProjectileDistance, UberstrikeLayerMasks.LocalRocketMask))
    {
      int num = this.Controller.NextProjectileId();
      hits = new CmunePairList<BaseGameProp, ShotPoint>(1);
      hits.Add((BaseGameProp) null, new ShotPoint(hitInfo.point, num));
      this.ShowExplosionEffect(hitInfo.point, hitInfo.normal, ray.direction, num);
      if (this.OnProjectileShoot == null)
        return;
      this.OnProjectileShoot(new ProjectileInfo(num, new Ray(hitInfo.point, -ray.direction)));
    }
    else
    {
      if ((bool) (UnityEngine.Object) this._decorator)
        this._decorator.ShowShootEffect(new RaycastHit[0]);
      MonoRoutine.Start(this.EmitProjectile(ray));
    }
  }

  public void ShowExplosionEffect(
    Vector3 position,
    Vector3 normal,
    Vector3 direction,
    int projectileId)
  {
    if (!(bool) (UnityEngine.Object) this._decorator)
      return;
    this._decorator.ShowExplosionEffect(position, normal, this.ExplosionType);
  }

  [DebuggerHidden]
  private IEnumerator EmitProjectile(Ray ray) => (IEnumerator) new ProjectileWeapon.\u003CEmitProjectile\u003Ec__Iterator8E()
  {
    ray = ray,
    \u003C\u0024\u003Eray = ray,
    \u003C\u003Ef__this = this
  };

  public Projectile EmitProjectile(Ray ray, int projectileID, int actorID)
  {
    if ((bool) (UnityEngine.Object) this._decorator && (bool) (UnityEngine.Object) this._decorator.Missle)
    {
      Projectile projectile = UnityEngine.Object.Instantiate((UnityEngine.Object) this._decorator.Missle) as Projectile;
      if ((bool) (UnityEngine.Object) projectile)
      {
        projectile.transform.position = this._decorator.MuzzlePosition;
        projectile.transform.rotation = Quaternion.LookRotation(ray.direction);
        projectile.transform.parent = ProjectileManager.ProjectileContainer.transform;
        projectile.gameObject.tag = "Prop";
        projectile.ExplosionEffect = this.ExplosionType;
        projectile.TimeOut = this._decorator.MissileTimeOut;
        projectile.SetExplosionSound(this._decorator.ExplosionSound);
        projectile.transform.position = ray.origin + (float) this.MinProjectileDistance * ray.direction;
        if (this.Controller.IsLocal)
          projectile.gameObject.layer = 26;
        else
          projectile.gameObject.layer = 24;
        CharacterConfig character;
        if (GameState.CurrentGame != null && GameState.CurrentGame.TryGetCharacter(actorID, out character) && (bool) (UnityEngine.Object) character.Decorator && projectile.gameObject.activeSelf)
        {
          foreach (CharacterHitArea hitArea in character.Decorator.HitAreas)
          {
            if (hitArea.gameObject.activeInHierarchy)
              Physics.IgnoreCollision(projectile.gameObject.collider, hitArea.collider);
          }
        }
        projectile.MoveInDirection(ray.direction * WeaponConfigurationHelper.GetProjectileSpeed((UberStrikeItemWeaponView) this.Config));
        return projectile;
      }
    }
    else
      UnityEngine.Debug.LogError((object) "Failed to create projectile!");
    return (Projectile) null;
  }
}
