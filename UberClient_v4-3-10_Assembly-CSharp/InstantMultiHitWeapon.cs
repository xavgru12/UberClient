// Decompiled with JetBrains decompiler
// Type: InstantMultiHitWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InstantMultiHitWeapon : BaseWeaponLogic
{
  private int ShotgunGauge;
  private BaseWeaponDecorator _decorator;

  public InstantMultiHitWeapon(
    WeaponItem item,
    BaseWeaponDecorator decorator,
    int shotGauge,
    IWeaponController controller)
    : base(item, controller)
  {
    this.ShotgunGauge = shotGauge;
    this._decorator = decorator;
  }

  public override BaseWeaponDecorator Decorator => this._decorator;

  public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    Dictionary<BaseGameProp, ShotPoint> dictionary = new Dictionary<BaseGameProp, ShotPoint>(this.ShotgunGauge);
    HitPoint point = (HitPoint) null;
    RaycastHit[] hits1 = new RaycastHit[this.ShotgunGauge];
    int projectileId = this.Controller.NextProjectileId();
    int distance = 1000;
    for (int index = 0; index < this.ShotgunGauge; ++index)
    {
      Vector3 direction = WeaponDataManager.ApplyDispersion(ray.direction, this.Config, false);
      RaycastHit hitInfo;
      if (Physics.Raycast(ray.origin, direction, out hitInfo, (float) distance, !this.Controller.IsLocal ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask))
      {
        if (point == null)
          point = new HitPoint(hitInfo.point, TagUtil.GetTag(hitInfo.collider));
        BaseGameProp component = hitInfo.collider.GetComponent<BaseGameProp>();
        if ((bool) (Object) component)
        {
          ShotPoint shotPoint;
          if (dictionary.TryGetValue(component, out shotPoint))
            shotPoint.AddPoint(hitInfo.point);
          else
            dictionary.Add(component, new ShotPoint(hitInfo.point, projectileId));
        }
        hits1[index] = hitInfo;
      }
      else
      {
        hits1[index].point = ray.origin + ray.direction * 1000f;
        hits1[index].normal = hitInfo.normal;
      }
    }
    this.Decorator.PlayImpactSoundAt(point);
    hits = new CmunePairList<BaseGameProp, ShotPoint>(dictionary.Count);
    foreach (KeyValuePair<BaseGameProp, ShotPoint> keyValuePair in dictionary)
      hits.Add(keyValuePair.Key, keyValuePair.Value);
    if ((bool) (Object) this.Decorator)
      this.Decorator.ShowShootEffect(hits1);
    this.OnHits(hits);
  }
}
