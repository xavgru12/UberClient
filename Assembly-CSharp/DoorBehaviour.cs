using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorBehaviour : MonoBehaviour
{
	private enum DoorState
	{
		Closed,
		Opening,
		Open,
		Closing
	}

	[Serializable]
	public class DoorElement
	{
		[HideInInspector]
		public Vector3 ClosedPosition;

		[HideInInspector]
		public Quaternion ClosedRotation;

		public DoorTrigger Element;

		public Vector3 OpenPosition;
	}

	[SerializeField]
	private DoorElement[] _elements;

	[SerializeField]
	private float _maxTime = 1f;

	private DoorState _state;

	private float _currentTime;

	private float _timeToClose;

	private int _doorID;

	public int DoorID => _doorID;

	private void Awake()
	{
		DoorElement[] elements = _elements;
		DoorElement[] array = elements;
		foreach (DoorElement doorElement in array)
		{
			doorElement.Element.SetDoorLogic(this);
			doorElement.ClosedPosition = doorElement.Element.transform.localPosition;
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

	private void OnDoorOpenedEvent(GameEvents.DoorOpened ev)
	{
		if (DoorID == ev.DoorID)
		{
			OpenDoor();
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			Open();
		}
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.tag == "Player")
		{
			_timeToClose = Time.time + 2f;
		}
	}

	private void OpenDoor()
	{
		switch (_state)
		{
		case DoorState.Opening:
			break;
		case DoorState.Closed:
			_state = DoorState.Opening;
			_currentTime = 0f;
			if ((bool)base.audio)
			{
				base.audio.Play();
			}
			break;
		case DoorState.Closing:
			_state = DoorState.Opening;
			_currentTime = _maxTime - _currentTime;
			break;
		case DoorState.Open:
			_timeToClose = Time.time + 2f;
			break;
		}
	}

	public void Open()
	{
		GameState.Current.Actions.OpenDoor(DoorID);
		OpenDoor();
	}

	public void Close()
	{
		_state = DoorState.Closing;
		_currentTime = 0f;
		if ((bool)base.audio)
		{
			base.audio.Play();
		}
	}

	private void Update()
	{
		float num = _maxTime;
		if (_maxTime == 0f)
		{
			num = 1f;
		}
		if (_state == DoorState.Opening)
		{
			_currentTime += Time.deltaTime;
			DoorElement[] elements = _elements;
			DoorElement[] array = elements;
			foreach (DoorElement doorElement in array)
			{
				doorElement.Element.transform.localPosition = Vector3.Lerp(doorElement.ClosedPosition, doorElement.OpenPosition, _currentTime / num);
			}
			if (_currentTime >= num)
			{
				_state = DoorState.Open;
				_timeToClose = Time.time + 2f;
				if ((bool)base.audio)
				{
					base.audio.Stop();
				}
			}
		}
		else if (_state == DoorState.Open)
		{
			if (_maxTime != 0f && _timeToClose < Time.time)
			{
				_state = DoorState.Closing;
				_currentTime = 0f;
				if ((bool)base.audio)
				{
					base.audio.Play();
				}
			}
		}
		else
		{
			if (_state != DoorState.Closing)
			{
				return;
			}
			_currentTime += Time.deltaTime;
			DoorElement[] elements2 = _elements;
			DoorElement[] array2 = elements2;
			foreach (DoorElement doorElement2 in array2)
			{
				doorElement2.Element.transform.localPosition = Vector3.Lerp(doorElement2.OpenPosition, doorElement2.ClosedPosition, _currentTime / num);
			}
			if (_currentTime >= num)
			{
				_state = DoorState.Closed;
				if ((bool)base.audio)
				{
					base.audio.Stop();
				}
			}
		}
	}
}
