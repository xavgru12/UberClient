// Decompiled with JetBrains decompiler
// Type: BulletTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BulletTrail : BaseWeaponEffect
{
  private Animation _animation;
  private UnityEngine.AnimationState _clip;
  private float _trailDuration = 0.1f;
  private Renderer[] _renderers = new Renderer[0];

  private void Awake()
  {
    this._animation = this.GetComponentInChildren<Animation>();
    if ((bool) (Object) this._animation)
    {
      this._clip = this._animation[this._animation.clip.name];
      this._clip.wrapMode = WrapMode.Once;
      this._trailDuration = this._clip.length;
      this._animation.playAutomatically = false;
    }
    this._renderers = this.GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = false;
  }

  public override void OnShoot()
  {
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = true;
    if ((bool) (Object) this._animation)
    {
      this._clip.speed = this._trailDuration / this._clip.length;
      this._animation.Play();
    }
    this.StartCoroutine(this.StartTrailEffect(this._trailDuration));
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
  }

  [DebuggerHidden]
  private IEnumerator StartTrailEffect(float time) => (IEnumerator) new BulletTrail.\u003CStartTrailEffect\u003Ec__Iterator89()
  {
    time = time,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };
}
