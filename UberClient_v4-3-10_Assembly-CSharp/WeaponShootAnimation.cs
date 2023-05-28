// Decompiled with JetBrains decompiler
// Type: WeaponShootAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WeaponShootAnimation : BaseWeaponEffect
{
  [SerializeField]
  private Animation _shootAnimation;

  private void Awake()
  {
    if (!(bool) (Object) this._shootAnimation)
      return;
    this._shootAnimation.playAutomatically = false;
  }

  public override void OnShoot()
  {
    if (!(bool) (Object) this._shootAnimation)
      return;
    this._shootAnimation.Rewind();
    this._shootAnimation.Play();
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
    if (!(bool) (Object) this._shootAnimation || !(bool) (Object) this._shootAnimation.clip)
      return;
    this.gameObject.SampleAnimation(this._shootAnimation.clip, 0.0f);
    this._shootAnimation.Stop();
  }
}
