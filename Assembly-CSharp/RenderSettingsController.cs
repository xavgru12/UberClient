using System;
using System.Reflection;
using UnityEngine;

public class RenderSettingsController : MonoBehaviour
{
	private const float UNDERWATER_FOG_START = 10f;

	private const float UNDERWATER_FOG_END = 100f;

	private const float FADE_SPEED = 3f;

	private const float TRANSITION_STRENGTH = 5f;

	private const float CHROMATIC_ABERRATION = 10f;

	private static volatile RenderSettingsController _instance;

	private static object _lock = new object();

	private float interpolationValue;

	private float fogStart;

	private float fogEnd;

	private Color fogColor;

	private FogMode fogMode;

	private UnderWaterEffect underWaterEffect;

	private Vignetting vignetteEffect;

	[SerializeField]
	private Color underwaterFogColor;

	[SerializeField]
	private GameObject advancedWater;

	[SerializeField]
	private GameObject simpleWater;

	[SerializeField]
	private MonoBehaviour[] simpleImageEffects;

	[SerializeField]
	private PostEffectsBase[] advancedImageEffects;

	public static RenderSettingsController Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						ConstructorInfo constructor = typeof(RenderSettingsController).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
						if (constructor == null || constructor.IsAssembly)
						{
							throw new Exception("A private or protected constructor is missing for '" + typeof(RenderSettingsController).Name + "'.");
						}
						_instance = (RenderSettingsController)constructor.Invoke(null);
					}
				}
			}
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
		fogMode = RenderSettings.fogMode;
		fogColor = RenderSettings.fogColor;
		fogStart = RenderSettings.fogStartDistance;
		fogEnd = RenderSettings.fogEndDistance;
		if (simpleWater != null)
		{
			simpleWater.SetActive(ApplicationDataManager.IsMobile);
		}
		if (advancedWater != null)
		{
			advancedWater.SetActive(!ApplicationDataManager.IsMobile);
		}
		EnableImageEffects();
		if (null == underWaterEffect)
		{
			underWaterEffect = Camera.main.gameObject.AddComponent<UnderWaterEffect>();
			if ((bool)underWaterEffect)
			{
				underWaterEffect.enabled = false;
				underWaterEffect.shader = Shader.Find("CMune/Under Water Effect");
				underWaterEffect.textureRamp = (Texture)Resources.Load("ImageEffects/Underwater_ColorRamp");
			}
		}
		if (null == vignetteEffect && !ApplicationDataManager.IsMobile)
		{
			vignetteEffect = Camera.main.gameObject.AddComponent<Vignetting>();
			if ((bool)vignetteEffect)
			{
				vignetteEffect.enabled = false;
				vignetteEffect.vignetteShader = Shader.Find("CMune/Vignetting");
				vignetteEffect.chromAberrationShader = Shader.Find("CMune/ChromaticAberration");
				vignetteEffect.separableBlurShader = Shader.Find("CMune/SeparableBlur");
				vignetteEffect.blurSpread = 4f;
				vignetteEffect.intensity = 0f;
			}
		}
		ResetInterpolation();
	}

	private void OnDisable()
	{
		ResetInterpolation();
	}

	public void EnableImageEffects()
	{
		MonoBehaviour[] array = simpleImageEffects;
		MonoBehaviour[] array2 = array;
		foreach (MonoBehaviour monoBehaviour in array2)
		{
			monoBehaviour.enabled = ApplicationDataManager.IsMobile;
		}
		PostEffectsBase[] array3 = advancedImageEffects;
		PostEffectsBase[] array4 = array3;
		foreach (PostEffectsBase postEffectsBase in array4)
		{
			postEffectsBase.enabled = (!ApplicationDataManager.IsMobile && ApplicationDataManager.ApplicationOptions.VideoPostProcessing);
		}
	}

	public void DisableImageEffects()
	{
		MonoBehaviour[] array = simpleImageEffects;
		MonoBehaviour[] array2 = array;
		foreach (MonoBehaviour monoBehaviour in array2)
		{
			monoBehaviour.enabled = false;
		}
		PostEffectsBase[] array3 = advancedImageEffects;
		PostEffectsBase[] array4 = array3;
		foreach (PostEffectsBase postEffectsBase in array4)
		{
			postEffectsBase.enabled = false;
		}
		if ((bool)underWaterEffect)
		{
			underWaterEffect.enabled = false;
		}
		if ((bool)vignetteEffect)
		{
			vignetteEffect.enabled = false;
		}
		interpolationValue = 0f;
	}

	private void Update()
	{
		if (GameState.Current.MatchState.CurrentStateId != 0)
		{
			if (GameState.Current.PlayerData.IsUnderWater)
			{
				interpolationValue += Time.deltaTime;
				RenderSettings.fogMode = FogMode.Linear;
			}
			else
			{
				interpolationValue -= Time.deltaTime;
				RenderSettings.fogMode = fogMode;
			}
			interpolationValue = Mathf.Clamp01(interpolationValue);
			UpdateSettings(interpolationValue);
		}
	}

	private void UpdateSettings(float value)
	{
		float num = Mathf.Clamp01(value * 3f);
		bool flag = (0f < value) ? true : false;
		if ((bool)underWaterEffect)
		{
			underWaterEffect.enabled = flag;
			underWaterEffect.Weight = num;
		}
		if ((bool)vignetteEffect)
		{
			bool flag2 = flag && ApplicationDataManager.ApplicationOptions.VideoPostProcessing;
			vignetteEffect.enabled = flag2;
			if (flag2)
			{
				float num2 = Mathf.Sin(value * (float)Math.PI);
				vignetteEffect.blur = 5f * num2 + value;
				vignetteEffect.chromaticAberration = (5f * num2 + value) * 10f;
			}
		}
		RenderSettings.fogColor = Color.Lerp(fogColor, underwaterFogColor, num);
		RenderSettings.fogStartDistance = Mathf.Lerp(fogStart, 10f, num);
		RenderSettings.fogEndDistance = Mathf.Lerp(fogEnd, 100f, num);
	}

	public void ResetInterpolation()
	{
		interpolationValue = 0f;
		UpdateSettings(interpolationValue);
	}
}
