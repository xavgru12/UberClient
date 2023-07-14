// Decompiled with JetBrains decompiler
// Type: Vector2Anim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Vector2Anim
{
  private Vector2 _vec2;
  private Vector2Anim.OnValueChange _onVec2Change;
  private bool _isAnimating;
  private Vector2 _animSrc;
  private Vector2 _animDest;
  private float _animTime;
  private float _animStartTime;
  private EaseType _animEaseType;

  public Vector2Anim(Vector2Anim.OnValueChange onVec2Change = null)
  {
    this._isAnimating = false;
    if (onVec2Change == null)
      return;
    this._onVec2Change = onVec2Change;
  }

  public Vector2 Vec2
  {
    get => this._vec2;
    set
    {
      Vector2 vec2 = this._vec2;
      this._vec2 = value;
      if (this._onVec2Change == null)
        return;
      this._onVec2Change(vec2, this._vec2);
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
      this.Vec2 = Vector2.Lerp(this._animSrc, this._animDest, Mathfx.Ease(Mathf.Clamp01(num * (1f / this._animTime)), this._animEaseType));
    }
    else
    {
      this.Vec2 = this._animDest;
      this._isAnimating = false;
    }
  }

  public void AnimTo(Vector2 destPosition, float time = 0.0f, EaseType easeType = EaseType.None, float startDelay = 0)
  {
    if ((double) time <= 0.0)
    {
      this.Vec2 = destPosition;
    }
    else
    {
      this._isAnimating = true;
      this._animSrc = this.Vec2;
      this._animDest = destPosition;
      this._animTime = time;
      this._animEaseType = easeType;
      this._animStartTime = Time.time + startDelay;
    }
  }

  public void AnimBy(Vector2 deltaPosition, float time = 0.0f, EaseType easeType = EaseType.None) => this.AnimTo(this.Vec2 + deltaPosition, time, easeType, 0.0f);

  public void StopAnim() => this._isAnimating = false;

  public delegate void OnValueChange(Vector2 oldValue, Vector2 newValue);
}
