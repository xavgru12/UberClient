// Decompiled with JetBrains decompiler
// Type: TemporaryDisplayAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TemporaryDisplayAnim : AbstractAnim
{
  private IAnimatable2D _animatable;
  private float _displayTime;
  private float _fadeOutAnimTime;
  private bool _isFading;

  public TemporaryDisplayAnim(IAnimatable2D animatable2D, float displayTime, float fadeOutAnimTime)
  {
    this._animatable = animatable2D;
    this._displayTime = displayTime;
    this._fadeOutAnimTime = fadeOutAnimTime;
    this.Duration = this._displayTime + this._fadeOutAnimTime;
  }

  protected override void OnStart() => this._animatable.FadeAlphaTo(1f, 0.0f, EaseType.None);

  protected override void OnStop()
  {
    this._animatable.FadeAlphaTo(0.0f, 0.0f, EaseType.None);
    this._animatable.StopFading();
    this._isFading = false;
  }

  protected override void OnUpdate()
  {
    if (!this.IsAnimating || (double) Time.time <= (double) this.StartTime + (double) this._displayTime || this._isFading)
      return;
    this.DoFadeoutAnim();
  }

  private void DoFadeoutAnim(params object[] args)
  {
    if (!this.IsAnimating)
      return;
    this._isFading = true;
    this._animatable.FadeAlphaTo(0.0f, this._fadeOutAnimTime, EaseType.Out);
  }
}
