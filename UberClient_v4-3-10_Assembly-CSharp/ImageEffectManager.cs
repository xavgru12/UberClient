// Decompiled with JetBrains decompiler
// Type: ImageEffectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ImageEffectManager
{
  private const float _motionBlurMaxValue = 0.5f;
  private ImageEffectManager.ImageEffectType _currentEffect;
  private Dictionary<ImageEffectManager.ImageEffectType, MonoBehaviour> _effects = new Dictionary<ImageEffectManager.ImageEffectType, MonoBehaviour>();
  private Dictionary<ImageEffectManager.ImageEffectType, ImageEffectManager.ImageEffectParameters> _effectsParameters = new Dictionary<ImageEffectManager.ImageEffectType, ImageEffectManager.ImageEffectParameters>();

  public void ApplyMotionBlur(float time)
  {
    if (!this._effects.ContainsKey(ImageEffectManager.ImageEffectType.MotionBlur))
      return;
    this.EnableEffect(ImageEffectManager.ImageEffectType.MotionBlur, time);
  }

  public void ApplyMotionBlur(float time, float intensity)
  {
    if (!this._effects.ContainsKey(ImageEffectManager.ImageEffectType.MotionBlur))
      return;
    this.EnableEffect(ImageEffectManager.ImageEffectType.MotionBlur, time, intensity);
  }

  public void ApplyWhiteout(float time)
  {
    if (!this._effects.ContainsKey(ImageEffectManager.ImageEffectType.BloomAndLensFlares))
      return;
    this.EnableEffect(ImageEffectManager.ImageEffectType.BloomAndLensFlares, time);
  }

  public void AddEffect(
    ImageEffectManager.ImageEffectType imageEffectType,
    MonoBehaviour monoBehaviour)
  {
    this._effects[imageEffectType] = monoBehaviour;
    this._effectsParameters[imageEffectType] = new ImageEffectManager.ImageEffectParameters();
  }

  public void Clear() => this._effects.Clear();

  public void Update()
  {
    if (ApplicationDataManager.ApplicationOptions.VideoMotionBlur)
    {
      ImageEffectManager.ImageEffectParameters effectParameters;
      if (this._effectsParameters.TryGetValue(ImageEffectManager.ImageEffectType.MotionBlur, out effectParameters) && effectParameters != null && effectParameters.EffectEnable)
      {
        if ((double) effectParameters.ActiveTime > 0.0)
        {
          effectParameters.ChangeActiveTime(-Time.deltaTime);
          if ((double) effectParameters.ActiveTime < 0.0)
            effectParameters.SetTimedEnable(false);
        }
        if (effectParameters.PermanentEnable)
          ((MotionBlur) this._effects[ImageEffectManager.ImageEffectType.MotionBlur]).blurAmount = 0.5f;
        else if (effectParameters.TimedEnable)
        {
          float baseIntencity = this._effectsParameters[ImageEffectManager.ImageEffectType.MotionBlur].BaseIntencity;
          ((MotionBlur) this._effects[ImageEffectManager.ImageEffectType.MotionBlur]).blurAmount = this._effectsParameters[ImageEffectManager.ImageEffectType.MotionBlur].ActiveTime / this._effectsParameters[ImageEffectManager.ImageEffectType.MotionBlur].TotalTime * ((double) baseIntencity <= 0.0 ? 0.5f : baseIntencity);
        }
      }
    }
    else if (this._effects.ContainsKey(ImageEffectManager.ImageEffectType.MotionBlur))
      this._effects[ImageEffectManager.ImageEffectType.MotionBlur].enabled = false;
    if (this._effects.ContainsKey(ImageEffectManager.ImageEffectType.ColorCorrectionCurves))
      this._effects[ImageEffectManager.ImageEffectType.ColorCorrectionCurves].enabled = ApplicationDataManager.ApplicationOptions.VideoVignetting;
    if (!this._effects.ContainsKey(ImageEffectManager.ImageEffectType.BloomAndLensFlares))
      return;
    this._effects[ImageEffectManager.ImageEffectType.BloomAndLensFlares].enabled = ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares;
  }

  public void EnableEffect(ImageEffectManager.ImageEffectType imageEffectType) => this.EnableEffect(imageEffectType, -1f, -1f);

  public void EnableEffect(ImageEffectManager.ImageEffectType imageEffectType, float time) => this.EnableEffect(imageEffectType, time, -1f);

  public void EnableEffect(
    ImageEffectManager.ImageEffectType imageEffectType,
    float duration,
    float intensity)
  {
    if (this._effects.ContainsKey(imageEffectType) && this._effectsParameters.ContainsKey(imageEffectType))
    {
      this._effects[imageEffectType].enabled = true;
      if (imageEffectType == ImageEffectManager.ImageEffectType.BloomAndLensFlares)
        this._effectsParameters[imageEffectType].SetBaseIntensity(((BloomAndLensFlares) this._effects[ImageEffectManager.ImageEffectType.BloomAndLensFlares]).bloomIntensity);
      if ((double) intensity > 0.0)
        this._effectsParameters[imageEffectType].SetBaseIntensity(intensity);
      if ((double) duration > 0.0)
      {
        this._effectsParameters[imageEffectType].SetTotalAndActiveTime(duration);
        this._effectsParameters[imageEffectType].SetTimedEnable(true);
      }
      else
        this._effectsParameters[imageEffectType].SetPermanentEnable(true);
      this._currentEffect = imageEffectType;
    }
    else
      Debug.LogError((object) "You're trying to enable an effect that hasn't been initialized. Check the components on MainCamera in the level.");
  }

  public void DisableEffect(ImageEffectManager.ImageEffectType imageEffectType)
  {
    if (!this._effects.ContainsKey(imageEffectType) || !this._effectsParameters.ContainsKey(imageEffectType))
      return;
    this._effects[imageEffectType].enabled = false;
    this._effectsParameters[imageEffectType].SetPermanentEnable(false);
    this._currentEffect = ImageEffectManager.ImageEffectType.None;
  }

  public void DisableEffectInstant(ImageEffectManager.ImageEffectType imageEffectType)
  {
    if (!this._effects.ContainsKey(imageEffectType) || !this._effectsParameters.ContainsKey(imageEffectType))
      return;
    this._effects[imageEffectType].enabled = false;
    this._effectsParameters[imageEffectType].SetPermanentEnable(false);
    this._effectsParameters[imageEffectType].SetTimedEnable(false);
    this._currentEffect = ImageEffectManager.ImageEffectType.None;
  }

  public void DisableAllEffects()
  {
    foreach (ImageEffectManager.ImageEffectType key in this._effectsParameters.Keys)
      this.DisableEffectInstant(key);
  }

  public ImageEffectManager.ImageEffectType CurrentEffect => this._currentEffect;

  public enum ImageEffectType
  {
    None,
    ColorCorrectionCurves,
    BloomAndLensFlares,
    MotionBlur,
  }

  private class ImageEffectParameters
  {
    private bool _permanentEnable;
    private bool _timedEnable;
    private float _baseIntencity;
    private float _totalTime;
    private float _activeTime;

    public void SetPermanentEnable(bool value) => this._permanentEnable = value;

    public void SetTimedEnable(bool value) => this._timedEnable = value;

    public void SetBaseIntensity(float value) => this._baseIntencity = value;

    public void SetTotalTime(float value) => this._totalTime = value;

    public void SetActiveTime(float value) => this._activeTime = value;

    public void SetTotalAndActiveTime(float time)
    {
      this.SetActiveTime(time);
      this.SetTotalTime(time);
    }

    public void ChangeActiveTime(float change) => this._activeTime += change;

    public bool PermanentEnable => this._permanentEnable;

    public bool TimedEnable => this._timedEnable;

    public bool EffectEnable => this._permanentEnable || this._timedEnable;

    public float BaseIntencity => this._baseIntencity;

    public float TotalTime => this._totalTime;

    public float ActiveTime => this._activeTime;
  }
}
