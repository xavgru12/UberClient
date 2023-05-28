// Decompiled with JetBrains decompiler
// Type: MuzzleSmoke
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (ParticleRenderer))]
public class MuzzleSmoke : BaseWeaponEffect
{
  private ParticleEmitter _particleEmitter;

  private void Awake() => this._particleEmitter = this.GetComponentInChildren<ParticleEmitter>();

  public override void OnShoot()
  {
    if (!(bool) (Object) this._particleEmitter)
      return;
    this.gameObject.SetActive(true);
    this._particleEmitter.Emit();
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
  }
}
