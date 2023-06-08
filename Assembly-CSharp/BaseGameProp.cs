using UnityEngine;

public class BaseGameProp : MonoBehaviour, IShootable
{
	[SerializeField]
	protected bool _recieveProjectileDamage = true;

	public bool RecieveProjectileDamage => _recieveProjectileDamage;

	public virtual bool IsVulnerable => true;

	public virtual bool IsLocal => false;

	public Transform Transform => base.transform;

	public virtual void ApplyDamage(DamageInfo damageInfo)
	{
	}

	public virtual void ApplyForce(Vector3 position, Vector3 force)
	{
	}
}
