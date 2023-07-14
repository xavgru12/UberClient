// Decompiled with JetBrains decompiler
// Type: InstantHitWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InstantHitWeapon : BaseWeaponLogic
{
  private BaseWeaponDecorator _decorator;
  private bool _supportIronSight;

  public InstantHitWeapon(
    WeaponItem item,
    BaseWeaponDecorator decorator,
    IWeaponController controller)
    : base(item, controller)
  {
    this._decorator = decorator;
    this._supportIronSight = item.Configuration.SecondaryAction == WeaponSecondaryAction.IronSight;
  }

  public override BaseWeaponDecorator Decorator => this._decorator;

  public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    hits = (CmunePairList<BaseGameProp, ShotPoint>) null;
    Vector3 direction = WeaponDataManager.ApplyDispersion(ray.direction, this.Config, this._supportIronSight);
    int projectileId = this.Controller.NextProjectileId();
    RaycastHit hitInfo;
    if (Physics.Raycast(ray.origin, direction, out hitInfo, 1000f, !this.Controller.IsLocal ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask))
    {
      HitPoint point = new HitPoint(hitInfo.point, TagUtil.GetTag(hitInfo.collider));
      BaseGameProp component = hitInfo.collider.GetComponent<BaseGameProp>();
      if ((bool) (Object) component)
      {
        hits = new CmunePairList<BaseGameProp, ShotPoint>(1);
        hits.Add(component, new ShotPoint(hitInfo.point, projectileId));
      }
      this.Decorator.PlayImpactSoundAt(point);
    }
    else
      hitInfo.point = ray.origin + ray.direction * 1000f;
    if ((bool) (Object) this.Decorator)
      this.Decorator.ShowShootEffect(new RaycastHit[1]
      {
        hitInfo
      });
    this.OnHits(hits);
  }
}
