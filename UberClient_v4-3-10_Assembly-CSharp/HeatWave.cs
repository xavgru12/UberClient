// Decompiled with JetBrains decompiler
// Type: HeatWave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class HeatWave : MonoBehaviour
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

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this._transform || !(bool) (UnityEngine.Object) this._renderer)
      return;
    this._elapsedTime += Time.deltaTime;
    this._normalizedTime = this._elapsedTime / this._duration;
    this._s = Mathf.Lerp(this._startSize, this._maxSize, this._normalizedTime);
    if ((bool) (UnityEngine.Object) this._renderer.material)
      this._renderer.material.SetFloat("_BumpAmt", (1f - this._normalizedTime) * this._distortion);
    this._transform.localScale = new Vector3(this._s, this._s, this._s);
    if ((bool) (UnityEngine.Object) Camera.main)
      this._transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - this._transform.position);
    if ((double) this._elapsedTime <= (double) this._duration || !(bool) (UnityEngine.Object) this.gameObject)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
