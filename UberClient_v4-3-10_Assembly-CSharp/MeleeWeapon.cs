// Decompiled with JetBrains decompiler
// Type: MeleeWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MeleeWeapon : BaseWeaponLogic
{
  private MeleeWeaponDecorator _decorator;

  public MeleeWeapon(WeaponItem item, MeleeWeaponDecorator decorator, IWeaponController controller)
    : base(item, controller)
  {
    this._decorator = decorator;
  }

  public override BaseWeaponDecorator Decorator => (BaseWeaponDecorator) this._decorator;

  public override float HitDelay => 0.2f;

  public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    Vector3 origin = ray.origin;
    origin.y -= 0.1f;
    ray.origin = origin;
    hits = (CmunePairList<BaseGameProp, ShotPoint>) null;
    float radius = 1f;
    int layerMask = !this.Controller.IsLocal ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask;
    float distance = 1f;
    RaycastHit[] raycastHitArray = Physics.SphereCastAll(ray, radius, distance, layerMask);
    int projectileId = this.Controller.NextProjectileId();
    if (raycastHitArray != null && raycastHitArray.Length > 0)
    {
      hits = new CmunePairList<BaseGameProp, ShotPoint>();
      float num = float.PositiveInfinity;
      RaycastHit hit = raycastHitArray[0];
      for (int index = 0; index < raycastHitArray.Length; ++index)
      {
        RaycastHit raycastHit = raycastHitArray[index];
        Vector3 rhs = raycastHit.point - ray.origin;
        if ((double) Vector3.Dot(ray.direction, rhs) > 0.0 && (double) raycastHit.distance < (double) num)
        {
          num = raycastHit.distance;
          hit = raycastHit;
        }
      }
      if ((bool) (Object) hit.collider)
      {
        BaseGameProp component = hit.collider.GetComponent<BaseGameProp>();
        if ((Object) component != (Object) null)
          hits.Add(component, new ShotPoint(hit.point, projectileId));
        if ((bool) (Object) this._decorator)
          this._decorator.StartCoroutine(this.StartShowingEffect(hit, ray.origin, this.HitDelay));
      }
    }
    else if ((bool) (Object) this._decorator)
      this._decorator.ShowShootEffect(new RaycastHit[0]);
    this.EmitWaterImpactParticles(ray, radius);
    this.OnHits(hits);
  }

  [DebuggerHidden]
  private IEnumerator StartShowingEffect(RaycastHit hit, Vector3 origin, float delay) => (IEnumerator) new MeleeWeapon.\u003CStartShowingEffect\u003Ec__Iterator8D()
  {
    hit = hit,
    delay = delay,
    \u003C\u0024\u003Ehit = hit,
    \u003C\u0024\u003Edelay = delay,
    \u003C\u003Ef__this = this
  };

  private void EmitWaterImpactParticles(Ray ray, float radius)
  {
    Vector3 origin = ray.origin;
    Vector3 vector3 = origin + ray.direction * radius;
    if (!GameState.HasCurrentSpace || !GameState.CurrentSpace.HasWaterPlane || ((double) origin.y <= (double) GameState.CurrentSpace.WaterPlaneHeight || (double) vector3.y >= (double) GameState.CurrentSpace.WaterPlaneHeight) && ((double) origin.y >= (double) GameState.CurrentSpace.WaterPlaneHeight || (double) vector3.y <= (double) GameState.CurrentSpace.WaterPlaneHeight))
      return;
    Vector3 hitPoint = vector3 with
    {
      y = GameState.CurrentSpace.WaterPlaneHeight
    };
    if (!Mathf.Approximately(ray.direction.y, 0.0f))
    {
      hitPoint.x = (GameState.CurrentSpace.WaterPlaneHeight - vector3.y) / ray.direction.y * ray.direction.x + vector3.x;
      hitPoint.z = (GameState.CurrentSpace.WaterPlaneHeight - vector3.y) / ray.direction.y * ray.direction.z + vector3.z;
    }
    MoveTrailrendererObject trailRenderer = this.Decorator.TrailRenderer;
    ParticleEffectController.ShowHitEffect(ParticleConfigurationType.MeleeDefault, SurfaceEffectType.WaterEffect, Vector3.up, hitPoint, Vector3.up, origin, 1f, ref trailRenderer, this.Decorator.transform);
  }
}
