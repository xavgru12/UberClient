using UnityEngine;

public class ShipBob : MonoBehaviour
{
	[SerializeField]
	private float rotateAmount = 1f;

	[SerializeField]
	private float moveAmount = 0.005f;

	private Transform _transform;

	private Vector3 shipRotation;

	private void Awake()
	{
		_transform = base.transform;
		shipRotation = _transform.localRotation.eulerAngles;
	}

	private void Update()
	{
		Transform transform = _transform;
		Vector3 position = _transform.position;
		float x = position.x;
		Vector3 position2 = _transform.position;
		float y = position2.y + Mathf.Sin(Time.time) * moveAmount;
		Vector3 position3 = _transform.position;
		transform.position = new Vector3(x, y, position3.z);
		float num = Mathf.Sin(Time.time) * rotateAmount;
		_transform.localRotation = Quaternion.Euler(shipRotation + new Vector3(num, num, num));
	}
}
