using System;
using System.Collections;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ProjectileWeapon : BaseWeaponLogic
{
	private UberStrikeItemWeaponView _view;

	private ProjectileWeaponDecorator _decorator;

	public override BaseWeaponDecorator Decorator => _decorator;

	public int MaxConcurrentProjectiles
	{
		get;
		private set;
	}

	public int MinProjectileDistance
	{
		get;
		private set;
	}

	public override int AmmoCountPerShot => _view.ProjectilesPerShot;

	public bool HasProjectileLimit => MaxConcurrentProjectiles > 0;

	public ParticleConfigurationType ExplosionType
	{
		get;
		private set;
	}

	public event Action<ProjectileInfo> OnProjectileShoot;

	public ProjectileWeapon(WeaponItem item, ProjectileWeaponDecorator decorator, IWeaponController controller, UberStrikeItemWeaponView view)
		: base(item, controller)
	{
		_view = view;
		_decorator = decorator;
		MaxConcurrentProjectiles = item.Configuration.MaxConcurrentProjectiles;
		MinProjectileDistance = item.Configuration.MinProjectileDistance;
		ExplosionType = item.Configuration.ParticleEffect;
	}

	public override void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits)
	{
		hits = null;
		if (MinProjectileDistance > 0 && Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, MinProjectileDistance, UberstrikeLayerMasks.LocalRocketMask))
		{
			int num = base.Controller.NextProjectileId();
			hits = new CmunePairList<BaseGameProp, ShotPoint>(1);
			hits.Add(null, new ShotPoint(hitInfo.point, num));
			ShowExplosionEffect(hitInfo.point, hitInfo.normal, ray.direction, num);
			if (this.OnProjectileShoot != null)
			{
				this.OnProjectileShoot(new ProjectileInfo(num, new Ray(hitInfo.point, -ray.direction)));
			}
		}
		else
		{
			if ((bool)_decorator)
			{
				_decorator.ShowShootEffect(new RaycastHit[0]);
			}
			UnityRuntime.StartRoutine(EmitProjectile(ray));
		}
	}

	public void ShowExplosionEffect(Vector3 position, Vector3 normal, Vector3 direction, int projectileId)
	{
		if ((bool)_decorator)
		{
			_decorator.ShowExplosionEffect(position, normal, ExplosionType);
		}
	}

	private IEnumerator EmitProjectile(Ray ray)
	{
		if (AmmoCountPerShot > 1)
		{
			float angle = 360 / AmmoCountPerShot;
			for (int i = 0; i < AmmoCountPerShot; i++)
			{
				if ((bool)_decorator)
				{
					int num = base.Controller.NextProjectileId();
					ray.origin = _decorator.MuzzlePosition + Quaternion.AngleAxis(angle * (float)i, _decorator.transform.forward) * _decorator.transform.up * 0.2f;
					Projectile projectile = EmitProjectile(ray, num, base.Controller.Cmid);
					if ((bool)projectile && this.OnProjectileShoot != null)
					{
						this.OnProjectileShoot(new ProjectileInfo(num, ray)
						{
							Projectile = projectile
						});
					}
					yield return new WaitForSeconds(0.2f);
				}
			}
		}
		else
		{
			int num2 = base.Controller.NextProjectileId();
			Projectile projectile2 = EmitProjectile(ray, num2, base.Controller.Cmid);
			if ((bool)projectile2 && this.OnProjectileShoot != null)
			{
				this.OnProjectileShoot(new ProjectileInfo(num2, ray)
				{
					Projectile = projectile2
				});
			}
		}
	}

	public Projectile EmitProjectile(Ray ray, int projectileID, int cmid)
	{
		if ((bool)_decorator && (bool)_decorator.Missle)
		{
			Vector3 muzzlePosition = _decorator.MuzzlePosition;
			Quaternion rotation = Quaternion.LookRotation(ray.direction);
			Projectile projectile = UnityEngine.Object.Instantiate(_decorator.Missle, muzzlePosition, rotation) as Projectile;
			if ((bool)projectile)
			{
				if (projectile is GrenadeProjectile)
				{
					GrenadeProjectile grenadeProjectile = projectile as GrenadeProjectile;
					grenadeProjectile.Sticky = base.Config.Sticky;
				}
				projectile.transform.parent = ProjectileManager.Container.transform;
				projectile.gameObject.tag = "Prop";
				projectile.ExplosionEffect = ExplosionType;
				projectile.TimeOut = _decorator.MissileTimeOut;
				projectile.SetExplosionSound(_decorator.ExplosionSound);
				projectile.transform.position = ray.origin + MinProjectileDistance * ray.direction;
				if (base.Controller.IsLocal)
				{
					projectile.gameObject.layer = 26;
				}
				else
				{
					projectile.gameObject.layer = 24;
				}
				if (GameState.Current != null && GameState.Current.TryGetPlayerAvatar(cmid, out CharacterConfig character) && (bool)character.Avatar.Decorator && projectile.gameObject.activeSelf)
				{
					CharacterHitArea[] hitAreas = character.Avatar.Decorator.HitAreas;
					CharacterHitArea[] array = hitAreas;
					foreach (CharacterHitArea characterHitArea in array)
					{
						if (characterHitArea.gameObject.activeInHierarchy)
						{
							Physics.IgnoreCollision(projectile.gameObject.collider, characterHitArea.collider);
						}
					}
				}
				projectile.MoveInDirection(ray.direction * WeaponConfigurationHelper.GetProjectileSpeed(_view));
				return projectile;
			}
		}
		return null;
	}
}
