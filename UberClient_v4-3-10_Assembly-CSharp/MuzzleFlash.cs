// Decompiled with JetBrains decompiler
// Type: MuzzleFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : BaseWeaponEffect
{
  private float _muzzleFlashEnd;
  private Animation _animation;
  private UnityEngine.AnimationState _clip;
  private float _flashDuration = 0.1f;
  private List<Renderer> _renderers = new List<Renderer>();

  private void Awake()
  {
    this._animation = this.GetComponentInChildren<Animation>();
    if ((bool) (Object) this._animation)
    {
      this._clip = this._animation[this._animation.clip.name];
      this._clip.wrapMode = WrapMode.Once;
      this._flashDuration = this._clip.length;
      this._animation.playAutomatically = false;
    }
    this._muzzleFlashEnd = 0.0f;
    this._renderers.AddRange((IEnumerable<Renderer>) this.GetComponentsInChildren<Renderer>());
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = false;
  }

  public override void Hide()
  {
    this._muzzleFlashEnd = 0.0f;
    if ((bool) (TrackedReference) this._clip)
      this._clip.normalizedTime = 1f;
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = false;
  }

  public override void OnShoot()
  {
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = true;
    this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (float) Random.Range(0, 360));
    if ((bool) (Object) this._animation)
    {
      this._clip.speed = this._clip.length / this._flashDuration;
      this._clip.time = 0.0f;
      this._animation.Play();
    }
    this._muzzleFlashEnd = Time.time + this._flashDuration;
  }

  public override void OnPostShoot()
  {
  }

  private void Update()
  {
    if ((double) this._muzzleFlashEnd >= (double) Time.time)
      return;
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = false;
  }
}
