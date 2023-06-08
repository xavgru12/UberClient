using System;
using UnityEngine;

public class SecretBehaviour : MonoBehaviour
{
	[Serializable]
	public class Door
	{
		public string _description;

		[SerializeField]
		private SecretDoor _door;

		[SerializeField]
		private SecretTrigger[] _trigger;

		public SecretTrigger[] Trigger => _trigger;

		public void CheckAllTriggers()
		{
			bool flag = true;
			SecretTrigger[] trigger = _trigger;
			SecretTrigger[] array = trigger;
			foreach (SecretTrigger secretTrigger in array)
			{
				flag &= (secretTrigger.ActivationTimeOut > Time.time);
			}
			if (flag)
			{
				_door.Open();
			}
		}
	}

	[SerializeField]
	private Door[] _doors;

	private void Awake()
	{
		Door[] doors = _doors;
		Door[] array = doors;
		foreach (Door door in array)
		{
			SecretTrigger[] trigger = door.Trigger;
			SecretTrigger[] array2 = trigger;
			foreach (SecretTrigger secretTrigger in array2)
			{
				secretTrigger.SetSecretReciever(this);
			}
		}
	}

	public void SetTriggerActivated(SecretTrigger trigger)
	{
		Door[] doors = _doors;
		Door[] array = doors;
		foreach (Door door in array)
		{
			door.CheckAllTriggers();
		}
	}
}
