using UnityEngine;

public class MuzzleLight : BaseWeaponEffect
{
	private Animation _shootAnimation;

	private void Awake()
	{
		_shootAnimation = GetComponent<Animation>();
		if ((bool)base.light)
		{
			base.light.intensity = 0f;
		}
	}

	private void OnEnable()
	{
		HideAllEffects();
	}

	public override void OnShoot()
	{
		if ((bool)_shootAnimation)
		{
			_shootAnimation.Play(PlayMode.StopSameLayer);
		}
	}

	public override void OnPostShoot()
	{
	}

	public override void Hide()
	{
		if ((bool)_shootAnimation)
		{
			_shootAnimation.Stop();
		}
	}

	public override void OnHits(RaycastHit[] hits)
	{
	}
}
