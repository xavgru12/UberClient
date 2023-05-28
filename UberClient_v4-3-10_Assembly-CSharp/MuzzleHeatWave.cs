// Decompiled with JetBrains decompiler
// Type: MuzzleHeatWave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class MuzzleHeatWave : BaseWeaponEffect
{
  [SerializeField]
  private float _startSize;
  [SerializeField]
  private float _maxSize = 0.05f;
  [SerializeField]
  private float _duration = 0.25f;
  [SerializeField]
  private float _distortion = 64f;
  private Transform _transform;
  private Renderer _renderer;
  private float _elapsedTime;
  private float _normalizedTime;
  private float _s;

  private void Awake()
  {
    this._transform = this.transform;
    this._renderer = this.renderer;
    if ((UnityEngine.Object) this._renderer == (UnityEngine.Object) null)
      throw new Exception("No Renderer attached to HeatWave script on GameObject " + this.gameObject.name);
  }

  private void Start()
  {
    this._renderer.enabled = false;
    this.enabled = false;
  }

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this._transform || !(bool) (UnityEngine.Object) this._renderer)
      return;
    this._elapsedTime += Time.deltaTime;
    this._normalizedTime = this._elapsedTime / this._duration;
    this._s = Mathf.Lerp(this._startSize, this._maxSize, this._normalizedTime);
    this._renderer.material.SetFloat("_BumpAmt", (1f - this._normalizedTime) * this._distortion);
    this._transform.localScale = new Vector3(this._s, this._s, this._s);
    this._transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - this._transform.position);
    if ((double) this._elapsedTime <= (double) this._duration)
      return;
    this._transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    this._renderer.enabled = false;
    this.enabled = false;
  }

  public override void OnShoot()
  {
    if (!SystemInfo.supportsImageEffects)
      return;
    this._elapsedTime = 0.0f;
    this._transform.rotation = Quaternion.FromToRotation(Vector3.up, Camera.main.transform.position - this._transform.position);
    this._renderer.enabled = true;
    this.enabled = true;
  }

  public override void OnPostShoot()
  {
  }

  public override void Hide()
  {
    if (!(bool) (UnityEngine.Object) this._renderer)
      this._renderer = this.renderer;
    if (!(bool) (UnityEngine.Object) this._renderer)
      return;
    this._renderer.enabled = false;
  }
}
