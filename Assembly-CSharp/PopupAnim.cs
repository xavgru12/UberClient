// Decompiled with JetBrains decompiler
// Type: PopupAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PopupAnim : AbstractAnim
{
  private Animatable2DGroup _popupGroup;
  private MeshGUIQuad _glowBlur;
  protected MeshGUIText _popupText;
  private Vector2 _spawnPosition;
  private Vector2 _destBlurScale;
  private Vector2 _destTextScale;
  private AudioClip _sound;
  private float _berpTime;
  private float _displayTime;
  private float _fadeOutAnimTime;
  private bool _isFading;

  public PopupAnim(
    Animatable2DGroup popupGroup,
    MeshGUIQuad glowBlur,
    MeshGUIText multiKillText,
    Vector2 spawnPosition,
    Vector2 destBlurScale,
    Vector2 destMultiKillScale,
    float displayTime,
    float fadeOutTime,
    AudioClip sound)
  {
    this._popupGroup = popupGroup;
    this._glowBlur = glowBlur;
    this._popupText = multiKillText;
    this._spawnPosition = spawnPosition;
    this._destBlurScale = destBlurScale;
    this._destTextScale = destMultiKillScale;
    this._berpTime = 0.1f;
    this._displayTime = displayTime;
    this._fadeOutAnimTime = fadeOutTime;
    this._sound = sound;
    this.Duration = this._berpTime + this._displayTime + this._fadeOutAnimTime;
  }

  protected override void OnStart()
  {
    this.DoBerpAnim();
    if (!((Object) this._sound != (Object) null))
      return;
    SfxManager.Play2dAudioClip(this._sound);
  }

  protected override void OnStop()
  {
    this._popupGroup.RemoveAndFree((IAnimatable2D) this._glowBlur);
    this._popupGroup.RemoveAndFree((IAnimatable2D) this._popupText);
    this._isFading = false;
  }

  protected override void OnUpdate()
  {
    if (!this.IsAnimating || (double) Time.time <= (double) this.StartTime + (double) this._berpTime + (double) this._displayTime || this._isFading)
      return;
    this.DoFadeOutAnim();
  }

  private void DoBerpAnim()
  {
    this._popupText.ScaleTo(this._destTextScale, this._berpTime, EaseType.Berp);
    this._popupText.FadeAlphaTo(1f, this._berpTime, EaseType.Berp);
    this._glowBlur.FadeAlphaTo(1f, this._berpTime, EaseType.Berp);
    this._glowBlur.ScaleToAroundPivot(this._destBlurScale, this._spawnPosition, this._berpTime, EaseType.Berp);
  }

  private void DoFadeOutAnim()
  {
    if (!this.IsAnimating)
      return;
    this._isFading = true;
    this._popupText.FadeAlphaTo(0.0f, this._fadeOutAnimTime, EaseType.Out);
    this._glowBlur.FadeAlphaTo(0.0f, this._fadeOutAnimTime, EaseType.Out);
  }
}
