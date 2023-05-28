// Decompiled with JetBrains decompiler
// Type: MuzzleParticleSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MuzzleParticleSystem : BaseWeaponEffect
{
  private ParticleSystem _particleSystem;

  private void Awake() => this._particleSystem = this.GetComponent<ParticleSystem>();

  public override void OnShoot()
  {
    if (!(bool) (Object) this._particleSystem)
      return;
    this._particleSystem.Play();
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
  }
}
