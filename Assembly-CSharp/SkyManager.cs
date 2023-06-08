using UnityEngine;

public class SkyManager : MonoBehaviour
{
	[SerializeField]
	private float _dayNightCycle;

	[SerializeField]
	private float _sunsetOffset;

	[SerializeField]
	private float _sunsetVisibility;

	[SerializeField]
	private Color _daySkyColor;

	[SerializeField]
	private Color _horizonColor;

	[SerializeField]
	private Color _sunsetColor;

	private Vector2 _dayCloudMoveVector = new Vector2(0f, 0f);

	private Vector2 _dayCloudHorizonMoveVector = new Vector2(0f, 0f);

	private float _cloudXAxisRot = 0.005f;

	private float _cloudYAxisRot = 0.005f;

	private float _cloudXAxisRotIndex = 0.001f;

	private float _cloudYAxisRotIndex = 0.001f;

	private Material _skyMaterial;

	public float DayNightCycle
	{
		get
		{
			return _dayNightCycle;
		}
		set
		{
			_dayNightCycle = value;
		}
	}

	public float CloudXAxisRot
	{
		get
		{
			return _cloudXAxisRot;
		}
		set
		{
			_cloudXAxisRot = value;
		}
	}

	public float CloudYAxisRot
	{
		get
		{
			return _cloudYAxisRot;
		}
		set
		{
			_cloudYAxisRot = value;
		}
	}

	private void OnEnable()
	{
		_skyMaterial = new Material(base.renderer.material);
	}

	private void OnDisable()
	{
		base.renderer.material = _skyMaterial;
	}

	private void Update()
	{
		_dayCloudMoveVector.x += Time.deltaTime * _cloudXAxisRot;
		_dayCloudHorizonMoveVector.y += Time.deltaTime * _cloudYAxisRot;
		if (_dayCloudMoveVector.x > 1f)
		{
			_dayCloudMoveVector.x = 0f;
			if (_cloudXAxisRot > 0.008f)
			{
				_cloudXAxisRotIndex = -0.001f;
			}
			if (_cloudXAxisRot < 0.002f)
			{
				_cloudXAxisRotIndex = 0.001f;
			}
			_cloudXAxisRot += _cloudXAxisRotIndex;
		}
		if (_dayCloudHorizonMoveVector.y > 1f)
		{
			_dayCloudHorizonMoveVector.y = 0f;
			if (_cloudYAxisRot > 0.008f)
			{
				_cloudYAxisRotIndex = -0.001f;
			}
			if (_cloudYAxisRot < 0.002f)
			{
				_cloudYAxisRotIndex = 0.001f;
			}
			_cloudYAxisRot += _cloudYAxisRotIndex;
		}
		base.renderer.material.SetTextureOffset("_DayCloudTex", _dayCloudMoveVector);
		base.renderer.material.SetTextureOffset("_NightCloudTex", _dayCloudHorizonMoveVector);
		_dayNightCycle = Mathf.Clamp01(_dayNightCycle);
		base.renderer.material.SetFloat("_DayNightCycle", Mathf.Clamp01(_dayNightCycle));
		_sunsetOffset = Mathf.Clamp01(_sunsetOffset);
		base.renderer.material.SetFloat("_SunsetOffset", Mathf.Clamp01(_sunsetOffset));
		_sunsetVisibility = Mathf.Clamp01(_sunsetVisibility);
		base.renderer.material.SetFloat("_SunsetVisibility", _sunsetVisibility);
		base.renderer.material.SetColor("_HorizonColor", _horizonColor);
		base.renderer.material.SetColor("_DaySkyColor", _daySkyColor);
		base.renderer.material.SetColor("_SunSetColor", _sunsetColor);
	}
}
