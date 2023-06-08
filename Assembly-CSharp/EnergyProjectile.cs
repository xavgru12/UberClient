using System.Collections;
using UnityEngine;

public class EnergyProjectile : Projectile
{
	[SerializeField]
	private MeshRenderer _trailRenderer;

	[SerializeField]
	private MeshRenderer _headRenderer;

	[SerializeField]
	private Light _light;

	[SerializeField]
	private Color _energyColor = Color.white;

	[SerializeField]
	private float _afterGlowDuration = 2f;

	public Color EnergyColor
	{
		get
		{
			return _energyColor;
		}
		set
		{
			_energyColor = value;
			_headRenderer.material.SetColor("_TintColor", _energyColor);
			_trailRenderer.material.SetColor("_TintColor", _energyColor);
		}
	}

	public float AfterGlowDuration
	{
		get
		{
			return _afterGlowDuration;
		}
		set
		{
			_afterGlowDuration = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (_light != null)
		{
			_light.enabled = Application.isWebPlayer;
		}
	}

	protected override void OnTriggerEnter(Collider c)
	{
		if (!base.IsProjectileExploded && LayerUtil.IsLayerInMask(base.CollisionMask, c.gameObject.layer))
		{
			Explode();
		}
	}

	protected override void OnCollisionEnter(Collision c)
	{
		if (!base.IsProjectileExploded && LayerUtil.IsLayerInMask(base.CollisionMask, c.gameObject.layer))
		{
			if (c.contacts.Length != 0)
			{
				Explode(c.contacts[0].point, c.contacts[0].normal, TagUtil.GetTag(c.collider));
			}
			else
			{
				Explode();
			}
		}
	}

	protected override IEnumerator StartTimeout()
	{
		yield return new WaitForSeconds((!(base.TimeOut > 0f)) ? 30f : base.TimeOut);
		Explode();
	}
}
