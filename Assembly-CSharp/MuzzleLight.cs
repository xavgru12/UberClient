// Decompiled with JetBrains decompiler
// Type: MuzzleLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MuzzleLight : BaseWeaponEffect
{
  private Animation _shootAnimation;

  private void Awake()
  {
    this._shootAnimation = this.GetComponent<Animation>();
    if (!(bool) (Object) this.light)
      return;
    this.light.intensity = 0.0f;
  }

  public override void OnShoot()
  {
    if (!(bool) (Object) this._shootAnimation)
      return;
    this._shootAnimation.Play(PlayMode.StopSameLayer);
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
    if (!(bool) (Object) this._shootAnimation)
      return;
    this._shootAnimation.Stop();
  }
}
