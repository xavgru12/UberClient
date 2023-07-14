// Decompiled with JetBrains decompiler
// Type: GrenadeProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GrenadeProjectile : Projectile
{
  [SerializeField]
  private bool _sticky;

  protected override void Start()
  {
    base.Start();
    if (this.Detonator != null)
      this.Detonator.Direction = Vector3.zero;
    this.Rigidbody.useGravity = true;
    this.Rigidbody.AddRelativeTorque(Random.insideUnitSphere.normalized * 10f);
  }

  protected override void OnTriggerEnter(Collider c)
  {
    if (this.IsProjectileExploded)
      return;
    if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
    {
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.ID);
      GameState.CurrentGame.RemoveProjectile(this.ID, true);
    }
    this.PlayBounceSound(c.transform.position);
  }

  protected override void OnCollisionEnter(Collision c)
  {
    if (this.IsProjectileExploded)
      return;
    if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
    {
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.ID);
      GameState.CurrentGame.RemoveProjectile(this.ID, true);
    }
    else if (this._sticky)
    {
      this.Rigidbody.isKinematic = true;
      this.collider.isTrigger = true;
      if (c.contacts.Length > 0)
        this.transform.position = c.contacts[0].point + c.contacts[0].normal * this.collider.bounds.extents.sqrMagnitude;
    }
    this.PlayBounceSound(c.transform.position);
  }
}
