using UnityEngine;

public class ApplicationOptions
{
	public bool IsRagdollShootable;

	public bool IsUsingCustom;

	public bool FOVMode;

	public int VideoQualityLevel = 2;

	public int VideoTextureQuality = 4;

	public int VideoVSyncCount;

	public int VideoAntiAliasing;

	public int VideoWaterMode = 1;

	public int ScreenResolution;

	public bool IsFullscreen;

	public bool VideoBloomAndFlares;

	public bool VideoVignetting;

	public bool VideoMotionBlur;

	public bool VideoShowFps;

	public bool VideoPostProcessing = true;

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

	public float AudioMusicVolume = 0.5f;

	public float AudioMasterVolume = 0.7f;

	public void Initialize()
	{
		string @string = PlayerPrefs.GetString("Version", "Invalid");
		bool flag = false;
		if (ApplicationDataManager.Version != @string)
		{
			flag = true;
			CmunePrefs.Reset();
			QualitySettings.SetQualityLevel(2, applyExpensiveChanges: true);
			PlayerPrefs.SetString("Version", ApplicationDataManager.Version);
		}
		Application.targetFrameRate = -1;
		QualitySettings.maxQueuedFrames = -1;
		IsUsingCustom = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoIsUsingCustom, IsUsingCustom);
		VideoWaterMode = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoWaterMode, VideoWaterMode);
		if ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) && VideoWaterMode == 2)
		{
			VideoWaterMode = 1;
		}
		VideoTextureQuality = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoTextureQuality, VideoTextureQuality);
		VideoVSyncCount = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoVSyncCount, VideoVSyncCount);
		VideoAntiAliasing = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoAntiAliasing, VideoAntiAliasing);
		VideoQualityLevel = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoCurrentQualityLevel, VideoQualityLevel);
		VideoBloomAndFlares = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoBloomAndFlares, VideoBloomAndFlares);
		VideoVignetting = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoColorCorrection, VideoVignetting);
		VideoMotionBlur = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoMotionBlur, VideoMotionBlur);
		VideoShowFps = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoShowFps, VideoShowFps);
		VideoPostProcessing = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoPostProcessing, VideoPostProcessing);
		IsFullscreen = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoIsFullscreen, defaultValue: true);
		ScreenResolution = CmunePrefs.ReadKey(CmunePrefs.Key.Options_VideoScreenRes, ScreenResolutionManager.CurrentResolutionIndex);
		InputXMouseSensitivity = Mathf.Clamp(CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputXMouseSensitivity, 3f), 0.1f, 10f);
		InputYMouseSensitivity = Mathf.Clamp(CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputYMouseSensitivity, 3f), 0.1f, 10f);
		InputMouseRotationMaxX = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputMouseRotationMaxX, 360f);
		InputMouseRotationMaxY = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputMouseRotationMaxY, 90f);
		InputMouseRotationMinX = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputMouseRotationMinX, -360f);
		InputMouseRotationMinY = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputMouseRotationMinY, -90f);
		InputInvertMouse = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputInvertMouse, defaultValue: false);
		bool flag2 = CmunePrefs.ReadKey(CmunePrefs.Key.Options_InputEnableGamepad, defaultValue: false);
		AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled = (Input.GetJoystickNames().Length != 0 && flag2);
		GameplayAutoPickupEnabled = CmunePrefs.ReadKey(CmunePrefs.Key.Options_GameplayAutoPickupEnabled, defaultValue: true);
		GameplayAutoEquipEnabled = CmunePrefs.ReadKey(CmunePrefs.Key.Options_GameplayAutoEquipEnabled, defaultValue: false);
		AudioEnabled = CmunePrefs.ReadKey(CmunePrefs.Key.Options_AudioEnabled, defaultValue: true);
		AudioEffectsVolume = CmunePrefs.ReadKey(CmunePrefs.Key.Options_AudioEffectsVolume, 0.7f);
		AudioMusicVolume = CmunePrefs.ReadKey(CmunePrefs.Key.Options_AudioMusicVolume, 0.5f);
		AudioMasterVolume = CmunePrefs.ReadKey(CmunePrefs.Key.Options_AudioMasterVolume, 0.5f);
		FOVMode = CmunePrefs.ReadKey(CmunePrefs.Key.Options_FOVMode, FOVMode);
		if (flag)
		{
			SaveApplicationOptions();
		}
	}

	public void SaveApplicationOptions()
	{
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoIsUsingCustom, IsUsingCustom);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoTextureQuality, VideoTextureQuality);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoVSyncCount, VideoVSyncCount);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoAntiAliasing, VideoAntiAliasing);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoWaterMode, VideoWaterMode);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoCurrentQualityLevel, VideoQualityLevel);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoBloomAndFlares, VideoBloomAndFlares);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoColorCorrection, VideoVignetting);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoMotionBlur, VideoMotionBlur);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoScreenRes, ScreenResolution);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoIsFullscreen, IsFullscreen);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoShowFps, VideoShowFps);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_VideoPostProcessing, VideoPostProcessing);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputXMouseSensitivity, InputXMouseSensitivity);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputYMouseSensitivity, InputYMouseSensitivity);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputMouseRotationMaxX, InputMouseRotationMaxX);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputMouseRotationMaxY, InputMouseRotationMaxY);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputMouseRotationMinX, InputMouseRotationMinX);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputMouseRotationMinY, InputMouseRotationMinY);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputInvertMouse, InputInvertMouse);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_InputEnableGamepad, AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_GameplayAutoPickupEnabled, GameplayAutoPickupEnabled);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_GameplayAutoEquipEnabled, GameplayAutoEquipEnabled);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_AudioEnabled, AudioEnabled);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_AudioEffectsVolume, AudioEffectsVolume);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_AudioMusicVolume, AudioMusicVolume);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_AudioMasterVolume, AudioMasterVolume);
		CmunePrefs.WriteKey(CmunePrefs.Key.Options_FOVMode, FOVMode);
	}
}
