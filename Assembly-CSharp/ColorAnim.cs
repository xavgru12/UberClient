// Decompiled with JetBrains decompiler
// Type: ColorAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ColorAnim
{
  private Color _color;
  private ColorAnim.OnValueChange _onColorChange;
  private bool _isAnimating;
  private Color _animSrc;
  private Color _animDest;
  private float _animTime;
  private float _animStartTime;
  private EaseType _animEaseType;

  public ColorAnim(ColorAnim.OnValueChange onColorChange = null)
  {
    this._isAnimating = false;
    if (onColorChange == null)
      return;
    this._onColorChange = onColorChange;
  }

  public Color Color
  {
    get => this._color;
    set
    {
      Color color = this._color;
      this._color = value;
      if (this._onColorChange == null)
        return;
      this._onColorChange(color, this._color);
    }
  }

  public bool IsAnimating => this._isAnimating;

  public float Alpha
  {
    get => this._color.a;
    set
    {
      Color color = this._color;
      this._color.a = value;
      if (this._onColorChange == null)
        return;
      this._onColorChange(color, this._color);
    }
  }

  public void Update()
  {
    if (!this._isAnimating)
      return;
    float num = Time.time - this._animStartTime;
    if ((double) num <= (double) this._animTime)
    {
      this.Color = Color.Lerp(this._animSrc, this._animDest, Mathfx.Ease(Mathf.Clamp01(num * (1f / this._animTime)), this._animEaseType));
      this.Alpha = this.Color.a;
    }
    else
    {
      this.Color = this._animDest;
      this.Alpha = this.Color.a;
      this._isAnimating = false;
    }
  }

  public void FadeAlphaTo(float destAlpha, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    if ((double) time <= 0.0)
    {
      this.Alpha = destAlpha;
    }
    else
    {
      this._isAnimating = true;
      this._animSrc = this.Color;
      this._animDest = this.Color;
      this._animDest.a = destAlpha;
      this._animTime = time;
      this._animEaseType = easeType;
      this._animStartTime = Time.time;
    }
  }

  public void FadeAlpha(float deltaAlpha, float time = 0.0f, EaseType easeType = EaseType.None) => this.FadeAlphaTo(this.Color.a + deltaAlpha, time, easeType);

  public void FadeColorTo(Color destColor, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    if ((double) time <= 0.0)
    {
      this.Color = destColor;
    }
    else
    {
      this._isAnimating = true;
      this._animSrc = this.Color;
      this._animDest = destColor;
      this._animTime = time;
      this._animEaseType = easeType;
      this._animStartTime = Time.time;
    }
  }

  public void FadeColor(Color deltaColor, float time = 0.0f, EaseType easeType = EaseType.None) => this.FadeColorTo(this.Color + deltaColor, time, easeType);

  public void StopFading() => this._isAnimating = false;

  public delegate void OnValueChange(Color oldValue, Color newValue);
}
