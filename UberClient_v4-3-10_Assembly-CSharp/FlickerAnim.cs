// Decompiled with JetBrains decompiler
// Type: FlickerAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class FlickerAnim
{
  private bool _isAnimating;
  private Action<FlickerAnim> _onFlickerVisibleChange;
  private float _flickerInterval;
  private float _flickerStartTime;
  private float _flickerEndTime;
  private float _lastFlickerTime;
  private bool _isFlickerVisible;

  public FlickerAnim(Action<FlickerAnim> onFlickerVisibleChange = null)
  {
    this._isAnimating = false;
    this._onFlickerVisibleChange = onFlickerVisibleChange;
  }

  public bool IsAnimating => this._isAnimating;

  public bool IsFlickerVisible
  {
    get => this._isFlickerVisible;
    set
    {
      this._isFlickerVisible = value;
      if (this._onFlickerVisibleChange == null)
        return;
      this._onFlickerVisibleChange(this);
    }
  }

  public void Update()
  {
    if (!this._isAnimating)
      return;
    float time = Time.time;
    if ((double) time > (double) this._flickerEndTime)
    {
      this._isAnimating = false;
      this.IsFlickerVisible = true;
    }
    else
    {
      if ((double) time <= (double) this._lastFlickerTime + (double) this._flickerInterval)
        return;
      this.IsFlickerVisible = !this._isFlickerVisible;
      this._lastFlickerTime = time;
    }
  }

  public void Flicker(float time, float flickerInterval = 0.02f)
  {
    if ((double) time <= 0.0 || (double) flickerInterval >= (double) time)
      return;
    this._isAnimating = true;
    this._flickerInterval = 0.02f;
    this._flickerStartTime = Time.time;
    this._flickerEndTime = this._flickerStartTime + time;
    this._lastFlickerTime = this._flickerStartTime;
  }

  public void StopAnim() => this._isAnimating = false;
}
