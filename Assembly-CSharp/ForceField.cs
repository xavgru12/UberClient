using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ForceField : MonoBehaviour
{
	[SerializeField]
	private Vector3 _direction;

	[SerializeField]
	private int _force = 1000;

	private float gizmofactor = 0.0055f;

	private void Awake()
	{
		base.collider.isTrigger = true;
		base.gameObject.layer = 2;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			GameState.Current.Player.MoveController.ApplyForce(_direction.normalized * _force, CharacterMoveController.ForceType.Exclusive);
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.JumpPad2D, 0uL);
		}
		else if (collider.gameObject.layer == 20)
		{
			AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.JumpPad, base.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(base.transform.localPosition, 0.2f);
		Vector3 normalized = _direction.normalized;
		normalized.y *= 0.6f;
		Gizmos.DrawLine(base.transform.localPosition, base.transform.localPosition + normalized * Mathf.Log(_force) * _force * gizmofactor);
	}
}
