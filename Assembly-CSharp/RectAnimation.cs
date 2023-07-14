// Decompiled with JetBrains decompiler
// Type: RectAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RectAnimation
{
  private Rect _rect;
  private RectAnimation.OnValueChange _onVec2Change;
  private bool _isAnimating;
  private Rect _animSrc;
  private float _animTime;
  private float _animStartTime;
  private EaseType _animEaseType;

  public RectAnimation(Rect initialRect, RectAnimation.OnValueChange onVec2Change = null)
  {
    this._animSrc = initialRect;
    this._rect = initialRect;
    this._isAnimating = false;
    if (onVec2Change == null)
      return;
    this._onVec2Change = onVec2Change;
  }

  public Rect FinalRect { get; private set; }

  public Rect Rect
  {
    get => this._rect;
    set
    {
      Rect rect = this._rect;
      this._rect = value;
      if (this._onVec2Change == null)
        return;
      this._onVec2Change(rect, this._rect);
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
      float t = Mathf.Clamp01(num * (1f / this._animTime));
      this.Rect = new Rect(Mathf.Lerp(this._animSrc.x, this.FinalRect.x, Mathfx.Ease(t, this._animEaseType)), Mathf.Lerp(this._animSrc.y, this.FinalRect.y, Mathfx.Ease(t, this._animEaseType)), Mathf.Lerp(this._animSrc.width, this.FinalRect.width, Mathfx.Ease(t, this._animEaseType)), Mathf.Lerp(this._animSrc.height, this.FinalRect.height, Mathfx.Ease(t, this._animEaseType)));
    }
    else
    {
      this.Rect = this.FinalRect;
      this._isAnimating = false;
    }
  }

  public void AnimTo(Rect destPosition, float time = 0.5f, EaseType easeType = EaseType.In, float startDelay = 0)
  {
    if ((double) time <= 0.0)
    {
      this.Rect = destPosition;
    }
    else
    {
      this._isAnimating = true;
      this._animSrc = this.Rect;
      this.FinalRect = destPosition;
      this._animTime = time;
      this._animEaseType = easeType;
      this._animStartTime = Time.time + startDelay;
    }
  }

  public void StopAnim() => this._isAnimating = false;

  public delegate void OnValueChange(Rect oldValue, Rect newValue);
}
