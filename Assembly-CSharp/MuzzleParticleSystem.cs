using UnityEngine;

public class MuzzleParticleSystem : BaseWeaponEffect
{
	private ParticleSystem _particleSystem;

	private void Awake()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}

	public override void OnShoot()
	{
		if ((bool)_particleSystem)
		{
			_particleSystem.Play();
		}
	}

	public override void OnPostShoot()
	{
	}

	private void OnEnable()
	{
		HideAllEffects();
	}

	public override void Hide()
	{
		if ((bool)_particleSystem)
		{
			_particleSystem.Stop();
		}
	}

	public override void OnHits(RaycastHit[] hits)
	{
	}
}
