// Decompiled with JetBrains decompiler
// Type: FloatAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FloatAnim
{
  private float _value;
  private FloatAnim.OnValueChange _onValueChange;
  private bool _isAnimating;
  private float _animSrc;
  private float _animDest;
  private float _animTime;
  private float _animStartTime;
  private EaseType _animEaseType;

  public FloatAnim(FloatAnim.OnValueChange onValueChange = null, float value = 0.0f)
  {
    this._isAnimating = false;
    this._value = value;
    if (onValueChange == null)
      return;
    this._onValueChange = onValueChange;
  }

  public float Value
  {
    get => this._value;
    set
    {
      float oldValue = this._value;
      this._value = value;
      if (this._onValueChange == null)
        return;
      this._onValueChange(oldValue, this._value);
    }
  }

  public bool IsAnimating => this._isAnimating;

  public void Update()
  {
    if (!this._isAnimating)
      return;
    float num = Time.time - this._animStartTime;
    if ((double) num <= (double) this._animTime)
    {
      this.Value = Mathf.Lerp(this._animSrc, this._animDest, Mathfx.Ease(Mathf.Clamp01(num * (1f / this._animTime)), this._animEaseType));
    }
    else
    {
      this.Value = this._animDest;
      this._isAnimating = false;
    }
  }

  public void AnimTo(float destValue, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    if ((double) time <= 0.0)
    {
      this.Value = destValue;
    }
    else
    {
      this._isAnimating = true;
      this._animSrc = this.Value;
      this._animDest = destValue;
      this._animTime = time;
      this._animEaseType = easeType;
      this._animStartTime = Time.time;
    }
  }

  public void AnimBy(float deltaValue, float time = 0.0f, EaseType easeType = EaseType.None) => this.AnimTo(this.Value + deltaValue, time, easeType);

  public void StopAnim() => this._isAnimating = false;

  public delegate void OnValueChange(float oldValue, float newValue);
}
