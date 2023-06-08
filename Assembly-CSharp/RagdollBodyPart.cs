using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollBodyPart : BaseGameProp, IShootable
{
	public override void ApplyDamage(DamageInfo damageInfo)
	{
		Vector3 force = damageInfo.Force * 0.5f;
		force += new Vector3(0f, damageInfo.UpwardsForceMultiplier, 0f);
		base.rigidbody.AddForce(force, ForceMode.Impulse);
	}
}
