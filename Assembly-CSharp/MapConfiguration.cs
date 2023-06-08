using System.Collections.Generic;
using UnityEngine;

public class MapConfiguration : MonoBehaviour
{
	[SerializeField]
	private bool _isEnabled = true;

	[SerializeField]
	private Transform _defaultSpawnPoint;

	[SerializeField]
	private FootStepSoundType _defaultFootStep = FootStepSoundType.Sand;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _defaultViewPoint;

	[SerializeField]
	protected GameObject _staticContentParent;

	[SerializeField]
	private GameObject _spawnPoints;

	[SerializeField]
	private Transform _waterPlane;

	private Dictionary<AudioSource, float> audioSources;

	public bool IsEnabled => _isEnabled;

	public Transform DefaultSpawnPoint
	{
		get
		{
			try
			{
				if ((bool)_defaultSpawnPoint)
				{
					return _defaultSpawnPoint;
				}
				Debug.LogError("No DefaultSpawnPoint assigned for " + base.gameObject.name);
				return _spawnPoints.transform.GetChild(0).GetChild(0);
			}
			catch
			{
				Debug.LogError("No DefaultSpawnPoint assigned for " + base.gameObject.name);
				return base.transform;
			}
		}
	}

	public string SceneName
	{
		get;
		private set;
	}

	public Camera Camera => _camera;

	public FootStepSoundType DefaultFootStep => _defaultFootStep;

	public Transform DefaultViewPoint => _defaultViewPoint;

	public GameObject SpawnPoints => _spawnPoints;

	public bool HasWaterPlane => _waterPlane != null;

	public float WaterPlaneHeight
	{
		get
		{
			if ((bool)_waterPlane)
			{
				Vector3 position = _waterPlane.position;
				return position.y;
			}
			return float.MinValue;
		}
	}

	private void Awake()
	{
		if (_defaultViewPoint == null)
		{
			_defaultViewPoint = base.transform;
		}
		Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(SpawnPoints.GetComponentsInChildren<SpawnPoint>(includeInactive: true));
		GameState.Current.Map = this;
		SceneName = Singleton<SceneLoader>.Instance.CurrentScene;
	}

	public void UpdateVolumes(float volume = 1f)
	{
		foreach (KeyValuePair<AudioSource, float> audioSource in audioSources)
		{
			audioSource.Key.volume = audioSource.Value * volume;
		}
	}

	private void Start()
	{
		audioSources = new Dictionary<AudioSource, float>();
		AudioSource[] componentsInChildren = GetComponentsInChildren<AudioSource>();
		AudioSource[] array = componentsInChildren;
		foreach (AudioSource audioSource in array)
		{
			audioSources.Add(audioSource, audioSource.volume);
		}
		UpdateVolumes(ApplicationDataManager.ApplicationOptions.AudioMusicVolume);
	}
}
