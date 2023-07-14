// Decompiled with JetBrains decompiler
// Type: WeaponHeadAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Animation))]
public class WeaponHeadAnimation : BaseWeaponEffect
{
  private Animation _animation;
  private UnityEngine.AnimationState _animState;
  private float _speed;

  private void Awake()
  {
    this._animation = this.GetComponent<Animation>();
    if (!(bool) (Object) this._animation || !(bool) (Object) this._animation.clip)
      return;
    this._animation.playAutomatically = false;
    this._animState = this._animation[this._animation.clip.name];
  }

  private void Update()
  {
    if ((double) this._speed > 0.0)
    {
      if ((bool) (TrackedReference) this._animState)
        this._animState.speed = this._speed;
      this._speed = Mathf.Lerp(this._speed, -0.1f, Time.deltaTime);
    }
    else
    {
      if (!this._animation.isPlaying)
        return;
      this._animation.Stop();
    }
  }

  public override void OnShoot()
  {
    this._speed = 1f;
    if ((bool) (Object) this._animation)
    {
      if (this._animation.isPlaying)
        return;
      this._animation.Play();
    }
    else
      Debug.LogError((object) "No animation for weapon head!");
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
    if (!(bool) (Object) this._animation || !this._animation.isPlaying)
      return;
    this._animation.Stop();
  }

  public void SetSpeed(float speed) => this._speed = speed;
}
