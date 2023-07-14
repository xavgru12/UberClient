// Decompiled with JetBrains decompiler
// Type: LocalShotFeedbackAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class LocalShotFeedbackAnim : AbstractAnim
{
  private Animatable2DGroup _textGroup;
  private MeshGUIText _text;
  private float _displayTime;
  private float _fadeOutAnimTime;
  private bool _isFading;
  private AudioClip _sound;

  public LocalShotFeedbackAnim(
    Animatable2DGroup textGroup,
    MeshGUIText meshText,
    float displayTime,
    float fadeOutAnimTime,
    AudioClip sound)
  {
    this._textGroup = textGroup;
    this._text = meshText;
    this._displayTime = displayTime;
    this._fadeOutAnimTime = fadeOutAnimTime;
    this._sound = sound;
    this.Duration = this._displayTime + this._fadeOutAnimTime;
  }

  protected override void OnStart()
  {
    this._textGroup.Group.Add((IAnimatable2D) this._text);
    this._text.FadeAlphaTo(1f);
    SfxManager.Play2dAudioClip(this._sound);
  }

  protected override void OnStop()
  {
    this._text.FadeAlphaTo(0.0f);
    this._text.StopFading();
    this._isFading = false;
    this._textGroup.RemoveAndFree((IAnimatable2D) this._text);
  }

  protected override void OnUpdate()
  {
    if (this.IsAnimating && (double) Time.time > (double) this.StartTime + (double) this._displayTime && !this._isFading)
      this.DoFadeoutAnim();
    this._text.ShadowColorAnim.Alpha = 0.0f;
  }

  private void DoFadeoutAnim()
  {
    if (!this.IsAnimating)
      return;
    this._isFading = true;
    this._text.FadeAlphaTo(0.0f, this._fadeOutAnimTime, EaseType.Out);
  }
}
