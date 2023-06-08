using System.Collections;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MeleeWeapon : BaseWeaponLogic
{
	private MeleeWeaponDecorator _decorator;

	public override BaseWeaponDecorator Decorator => _decorator;

	public override float HitDelay => 0.2f;

	public MeleeWeapon(WeaponItem item, MeleeWeaponDecorator decorator, IWeaponController controller)
		: base(item, controller)
	{
		_decorator = decorator;
	}

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
	{
		Vector3 origin = ray.origin;
		origin.y -= 0.1f;
		ray.origin = origin;
		hits = null;
		float radius = 1f;
		int layerMask = (!base.Controller.IsLocal) ? UberstrikeLayerMasks.ShootMaskRemotePlayer : UberstrikeLayerMasks.ShootMask;
		float distance = 1f;
		RaycastHit[] array = Physics.SphereCastAll(ray, radius, distance, layerMask);
		int projectileId = base.Controller.NextProjectileId();
		if (array != null && array.Length != 0)
		{
			hits = new CmunePairList<BaseGameProp, ShotPoint>();
			float num = float.PositiveInfinity;
			RaycastHit hit = array[0];
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				Vector3 rhs = raycastHit.point - ray.origin;
				if (Vector3.Dot(ray.direction, rhs) > 0f && raycastHit.distance < num)
				{
					num = raycastHit.distance;
					hit = raycastHit;
				}
			}
			if ((bool)hit.collider)
			{
				BaseGameProp component = hit.collider.GetComponent<BaseGameProp>();
				if (component != null)
				{
					hits.Add(component, new ShotPoint(hit.point, projectileId));
				}
				if ((bool)_decorator)
				{
					_decorator.StartCoroutine(StartShowingEffect(hit, ray.origin, HitDelay));
				}
			}
		}
		else if ((bool)_decorator)
		{
			_decorator.ShowShootEffect(new RaycastHit[0]);
		}
		EmitWaterImpactParticles(ray, radius);
		OnHits(hits);
	}

	private IEnumerator StartShowingEffect(RaycastHit hit, Vector3 origin, float delay)
	{
		if ((bool)_decorator)
		{
			_decorator.ShowShootEffect(new RaycastHit[1]
			{
				hit
			});
		}
		yield return new WaitForSeconds(delay);
		Decorator.PlayImpactSoundAt(new HitPoint(hit.point, TagUtil.GetTag(hit.collider)));
	}

	private void EmitWaterImpactParticles(Ray ray, float radius)
	{
		Vector3 origin = ray.origin;
		Vector3 vector = origin + ray.direction * radius;
		if (GameState.Current.Map != null && GameState.Current.Map.HasWaterPlane && ((origin.y > GameState.Current.Map.WaterPlaneHeight && vector.y < GameState.Current.Map.WaterPlaneHeight) || (origin.y < GameState.Current.Map.WaterPlaneHeight && vector.y > GameState.Current.Map.WaterPlaneHeight)))
		{
			Vector3 hitPoint = vector;
			hitPoint.y = GameState.Current.Map.WaterPlaneHeight;
			Vector3 direction = ray.direction;
			if (!Mathf.Approximately(direction.y, 0f))
			{
				float num = GameState.Current.Map.WaterPlaneHeight - vector.y;
				Vector3 direction2 = ray.direction;
				float num2 = num / direction2.y;
				Vector3 direction3 = ray.direction;
				hitPoint.x = num2 * direction3.x + vector.x;
				float num3 = GameState.Current.Map.WaterPlaneHeight - vector.y;
				Vector3 direction4 = ray.direction;
				float num4 = num3 / direction4.y;
				Vector3 direction5 = ray.direction;
				hitPoint.z = num4 * direction5.z + vector.z;
			}
			MoveTrailrendererObject trailRenderer = Decorator.TrailRenderer;
			ParticleEffectController.ShowHitEffect(ParticleConfigurationType.MeleeDefault, SurfaceEffectType.WaterEffect, Vector3.up, hitPoint, Vector3.up, origin, 1f, ref trailRenderer, Decorator.transform);
		}
	}
}
