using UnityEngine;

public class PickupBounce : MonoBehaviour
{
	private float origPosY;

	private float startOffset;

	private void Awake()
	{
		Vector3 position = base.transform.position;
		origPosY = position.y;
		startOffset = Random.value * 3f;
	}

	private void FixedUpdate()
	{
		base.transform.Rotate(new Vector3(0f, 2f, 0f));
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		float x = position.x;
		float y = origPosY + Mathf.Sin((startOffset + Time.realtimeSinceStartup) * 4f) * 0.08f;
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, y, position2.z);
	}
}
