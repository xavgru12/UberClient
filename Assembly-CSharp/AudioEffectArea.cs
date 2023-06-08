using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AudioEffectArea : MonoBehaviour
{
	[SerializeField]
	private GameObject outdoorEnvironment;

	[SerializeField]
	private GameObject indoorEnvironment;

	private void Awake()
	{
		base.collider.isTrigger = true;
		if (indoorEnvironment != null)
		{
			indoorEnvironment.SetActive(value: true);
		}
		if (outdoorEnvironment != null)
		{
			outdoorEnvironment.SetActive(value: false);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			if (outdoorEnvironment != null)
			{
				outdoorEnvironment.SetActive(value: false);
			}
			if (indoorEnvironment != null)
			{
				indoorEnvironment.SetActive(value: true);
			}
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Player")
		{
			if (outdoorEnvironment != null)
			{
				outdoorEnvironment.SetActive(value: true);
			}
			if (indoorEnvironment != null)
			{
				indoorEnvironment.SetActive(value: false);
			}
		}
	}
}
