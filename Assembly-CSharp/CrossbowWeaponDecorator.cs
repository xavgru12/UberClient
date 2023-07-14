// Decompiled with JetBrains decompiler
// Type: CrossbowWeaponDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CrossbowWeaponDecorator : BaseWeaponDecorator
{
  [SerializeField]
  private ArrowProjectile _arrowProjectile;

  protected override void ShowImpactEffects(
    RaycastHit hit,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance,
    bool playSound)
  {
    this.CreateArrow(hit, direction);
    base.ShowImpactEffects(hit, direction, muzzlePosition, distance, playSound);
  }

  private void CreateArrow(RaycastHit hit, Vector3 direction)
  {
    if (!(bool) (Object) this._arrowProjectile || !((Object) hit.collider != (Object) null))
      return;
    Quaternion quaternion = new Quaternion();
    Quaternion rotation = Quaternion.FromToRotation(Vector3.back, direction * -1f);
    ArrowProjectile arrowProjectile = Object.Instantiate((Object) this._arrowProjectile, hit.point, rotation) as ArrowProjectile;
    if (hit.collider.gameObject.layer == 18)
    {
      if ((bool) (Object) GameState.LocalDecorator)
      {
        arrowProjectile.gameObject.transform.parent = GameState.LocalDecorator.GetBone(BoneIndex.Hips);
        foreach (Renderer componentsInChild in arrowProjectile.GetComponentsInChildren<Renderer>(true))
          componentsInChild.enabled = false;
      }
    }
    else if (hit.collider.gameObject.layer == 20)
      arrowProjectile.SetParent(hit.collider.transform);
    arrowProjectile.Destroy(15);
  }
}
