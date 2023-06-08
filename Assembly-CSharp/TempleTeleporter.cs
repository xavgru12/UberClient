using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TempleTeleporter : SecretDoor
{
	[SerializeField]
	private float _activationTime = 15f;

	[SerializeField]
	private Renderer[] _visuals;

	[SerializeField]
	private Transform _spawnpoint;

	[SerializeField]
	private ParticleEmitter _particles;

	private int _doorID;

	private float _timeOut;

	private AudioSource[] _audios;

	public int DoorID => _doorID;

	private void Awake()
	{
		_audios = GetComponents<AudioSource>();
		_particles.emit = false;
		Renderer[] visuals = _visuals;
		Renderer[] array = visuals;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = false;
		}
		_doorID = base.transform.position.GetHashCode();
	}

	private void OnEnable()
	{
		EventHandler.Global.AddListener<GameEvents.DoorOpened>(OnDoorOpenedEvent);
	}

	private void OnDisable()
	{
		EventHandler.Global.RemoveListener<GameEvents.DoorOpened>(OnDoorOpenedEvent);
	}

	private void Update()
	{
		if (_timeOut < Time.time)
		{
			AudioSource[] audios = _audios;
			AudioSource[] array = audios;
			foreach (AudioSource audioSource in array)
			{
				audioSource.Stop();
			}
			_particles.emit = false;
			Renderer[] visuals = _visuals;
			Renderer[] array2 = visuals;
			foreach (Renderer renderer in array2)
			{
				renderer.enabled = false;
			}
			base.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player" && _timeOut > Time.time)
		{
			_timeOut = 0f;
			GameState.Current.Player.SpawnPlayerAt(_spawnpoint.position, _spawnpoint.rotation);
		}
	}

	private void OnDoorOpenedEvent(GameEvents.DoorOpened ev)
	{
		if (DoorID == ev.DoorID)
		{
			OpenDoor();
		}
	}

	public override void Open()
	{
		if (GameState.Current.HasJoinedGame)
		{
			GameState.Current.Actions.OpenDoor(DoorID);
		}
		OpenDoor();
	}

	private void OpenDoor()
	{
		base.enabled = true;
		_particles.emit = true;
		Renderer[] visuals = _visuals;
		Renderer[] array = visuals;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = true;
		}
		_timeOut = Time.time + _activationTime;
		AudioSource[] audios = _audios;
		AudioSource[] array2 = audios;
		foreach (AudioSource audioSource in array2)
		{
			audioSource.Play();
		}
	}
}
