using UnityEngine;

[RequireComponent(typeof(ParticleRenderer))]
public class MuzzleSmoke : BaseWeaponEffect
{
	private ParticleEmitter _particleEmitter;

	private void Awake()
	{
		_particleEmitter = GetComponentInChildren<ParticleEmitter>();
	}

	public override void OnShoot()
	{
		if ((bool)_particleEmitter)
		{
			base.gameObject.SetActive(value: true);
			_particleEmitter.Emit();
		}
	}

	private void OnEnable()
	{
		HideAllEffects();
	}

	public override void OnPostShoot()
	{
	}

	public override void OnHits(RaycastHit[] hits)
	{
	}

	public override void Hide()
	{
		if ((bool)_particleEmitter)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
