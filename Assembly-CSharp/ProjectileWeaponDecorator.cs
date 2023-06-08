using UnityEngine;

public class ProjectileWeaponDecorator : BaseWeaponDecorator
{
	[SerializeField]
	private Projectile _missle;

	[SerializeField]
	private AudioClip _missleExplosionSound;

	private float _missileTimeOut;

	public Projectile Missle => _missle;

	public float MissileTimeOut => _missileTimeOut;

	public AudioClip ExplosionSound => _missleExplosionSound;

	public void ShowExplosionEffect(Vector3 position, Vector3 normal, ParticleConfigurationType explosionEffect)
	{
		ShowShootEffect(new RaycastHit[0]);
		Singleton<ExplosionManager>.Instance.ShowExplosionEffect(position, normal, base.tag, explosionEffect);
	}

	public void SetMissileTimeOut(float timeOut)
	{
		_missileTimeOut = timeOut;
	}
}
