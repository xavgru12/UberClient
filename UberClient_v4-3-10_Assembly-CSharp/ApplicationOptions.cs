// Decompiled with JetBrains decompiler
// Type: ApplicationOptions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ApplicationOptions
{
  public int GeneralTargetFrameRate = 200;
  public bool IsUsingCustom;
  public int VideoQualityLevel = 1;
  public int VideoMaxQueuedFrames = 1;
  public int VideoTextureQuality = 4;
  public int VideoVSyncCount;
  public int VideoAntiAliasing;
  public int VideoWaterMode = 1;
  public int ScreenResolution;
  public bool IsFullscreen;
  public bool VideoBloomAndFlares;
  public bool VideoVignetting;
  public bool VideoMotionBlur;
  public bool VideoBloomHitEffect = true;
  public float InputXMouseSensitivity = 3f;
  public float InputYMouseSensitivity = 3f;
  public float InputMouseRotationMaxX = 360f;
  public float InputMouseRotationMaxY = 90f;
  public float InputMouseRotationMinX = -360f;
  public float InputMouseRotationMinY = -90f;
  public bool InputInvertMouse;
  public float TouchLookSensitivity = 1f;
  public float TouchMoveSensitivity = 1f;
  public bool UseMultiTouch;
  public bool GameplayAutoPickupEnabled = true;
  public bool GameplayAutoEquipEnabled;
  public float CameraFovMax = 65f;
  public float CameraFovMin = 5f;
  public bool AudioEnabled = true;
  public float AudioEffectsVolume = 0.7f;
  public float AudioMusicVolume = 0.3f;
  public float AudioMasterVolume = 0.5f;

  public void Initialize()
  {
    string str = PlayerPrefs.GetString("Version", "Invalid");
    bool flag1 = false;
    if ("4.3.10" != str)
    {
      flag1 = true;
      CmunePrefs.Reset();
      QualitySettings.SetQualityLevel(1, true);
      PlayerPrefs.SetString("Version", "4.3.10");
    }
    this.GeneralTargetFrameRate = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_GeneralTargetFrameRate, 200);
    this.IsUsingCustom = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoIsUsingCustom, this.IsUsingCustom);
    this.VideoWaterMode = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoWaterMode, this.VideoWaterMode);
    if ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) && this.VideoWaterMode == 2)
      this.VideoWaterMode = 1;
    this.VideoMaxQueuedFrames = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoMaxQueuedFrames, this.VideoMaxQueuedFrames);
    this.VideoTextureQuality = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoTextureQuality, this.VideoTextureQuality);
    this.VideoVSyncCount = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoVSyncCount, this.VideoVSyncCount);
    this.VideoAntiAliasing = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoAntiAliasing, this.VideoAntiAliasing);
    this.VideoQualityLevel = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoCurrentQualityLevel, this.VideoQualityLevel);
    this.VideoBloomAndFlares = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoBloomAndFlares, this.VideoBloomAndFlares);
    this.VideoVignetting = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoColorCorrection, this.VideoVignetting);
    this.VideoMotionBlur = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoMotionBlur, this.VideoMotionBlur);
    this.VideoBloomHitEffect = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoBloomHitEffect, this.VideoBloomHitEffect);
    this.IsFullscreen = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_VideoIsFullscreen, true);
    this.ScreenResolution = CmunePrefs.ReadKey<int>(CmunePrefs.Key.Options_VideoScreenRes, ScreenResolutionManager.CurrentResolutionIndex);
    this.InputXMouseSensitivity = Mathf.Clamp(CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputXMouseSensitivity, 3f), 1f, 10f);
    this.InputYMouseSensitivity = Mathf.Clamp(CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputYMouseSensitivity, 3f), 1f, 10f);
    this.InputMouseRotationMaxX = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputMouseRotationMaxX, 360f);
    this.InputMouseRotationMaxY = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputMouseRotationMaxY, 90f);
    this.InputMouseRotationMinX = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputMouseRotationMinX, -360f);
    this.InputMouseRotationMinY = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_InputMouseRotationMinY, -90f);
    this.InputInvertMouse = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_InputInvertMouse, false);
    bool flag2 = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_InputEnableGamepad, false);
    AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled = Input.GetJoystickNames().Length > 0 && flag2;
    this.GameplayAutoPickupEnabled = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_GameplayAutoPickupEnabled, true);
    this.GameplayAutoEquipEnabled = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_GameplayAutoEquipEnabled, false);
    this.AudioEnabled = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Options_AudioEnabled, true);
    this.AudioEffectsVolume = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_AudioEffectsVolume, 0.7f);
    this.AudioMusicVolume = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_AudioMusicVolume, 0.3f);
    this.AudioMasterVolume = CmunePrefs.ReadKey<float>(CmunePrefs.Key.Options_AudioMasterVolume, 0.5f);
    if (!flag1)
      return;
    this.SaveApplicationOptions();
  }

  public void SaveApplicationOptions()
  {
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_GeneralTargetFrameRate, this.GeneralTargetFrameRate);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoIsUsingCustom, this.IsUsingCustom);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoMaxQueuedFrames, this.VideoMaxQueuedFrames);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoTextureQuality, this.VideoTextureQuality);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoVSyncCount, this.VideoVSyncCount);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoAntiAliasing, this.VideoAntiAliasing);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoWaterMode, this.VideoWaterMode);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoCurrentQualityLevel, this.VideoQualityLevel);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoBloomAndFlares, this.VideoBloomAndFlares);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoColorCorrection, this.VideoVignetting);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoMotionBlur, this.VideoMotionBlur);
    CmunePrefs.WriteKey<int>(CmunePrefs.Key.Options_VideoScreenRes, this.ScreenResolution);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoIsFullscreen, this.IsFullscreen);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_VideoBloomHitEffect, this.VideoBloomHitEffect);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputXMouseSensitivity, this.InputXMouseSensitivity);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputYMouseSensitivity, this.InputYMouseSensitivity);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputMouseRotationMaxX, this.InputMouseRotationMaxX);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputMouseRotationMaxY, this.InputMouseRotationMaxY);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputMouseRotationMinX, this.InputMouseRotationMinX);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_InputMouseRotationMinY, this.InputMouseRotationMinY);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_InputInvertMouse, this.InputInvertMouse);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_InputEnableGamepad, AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_GameplayAutoPickupEnabled, this.GameplayAutoPickupEnabled);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_GameplayAutoEquipEnabled, this.GameplayAutoEquipEnabled);
    CmunePrefs.WriteKey<bool>(CmunePrefs.Key.Options_AudioEnabled, this.AudioEnabled);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_AudioEffectsVolume, this.AudioEffectsVolume);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_AudioMusicVolume, this.AudioMusicVolume);
    CmunePrefs.WriteKey<float>(CmunePrefs.Key.Options_AudioMasterVolume, this.AudioMasterVolume);
  }
}
