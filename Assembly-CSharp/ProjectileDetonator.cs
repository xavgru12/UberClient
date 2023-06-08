using System;
using UberStrike.Core.Types;
using UnityEngine;

public class ProjectileDetonator
{
	private const float _upwardsForceMultiplier = 5f;

	public float Radius
	{
		get;
		private set;
	}

	public float Damage
	{
		get;
		private set;
	}

	public int Force
	{
		get;
		private set;
	}

	public Vector3 Direction
	{
		get;
		set;
	}

	public int WeaponID
	{
		get;
		private set;
	}

	public UberstrikeItemClass WeaponClass
	{
		get;
		private set;
	}

	public int ProjectileID
	{
		get;
		private set;
	}

	public DamageEffectType DamageEffectFlag
	{
		get;
		private set;
	}

	public float DamageEffectValue
	{
		get;
		private set;
	}

	public byte Slot
	{
		get;
		private set;
	}

	public ProjectileDetonator(float radius, float damage, int force, Vector3 direction, byte slot, int projectileId, int weaponId, UberstrikeItemClass weaponClass, DamageEffectType damageEffectFlag, float damageEffectValue)
	{
		Radius = radius;
		Damage = damage;
		Force = force;
		Direction = direction;
		Slot = slot;
		ProjectileID = projectileId;
		WeaponID = weaponId;
		WeaponClass = weaponClass;
		DamageEffectFlag = damageEffectFlag;
		DamageEffectValue = damageEffectValue;
	}

	public void Explode(Vector3 position)
	{
		Explode(position, ProjectileID, Damage, Direction, Radius, Force, Slot, WeaponID, WeaponClass, DamageEffectFlag, DamageEffectValue);
	}

	public static void Explode(Vector3 position, int projectileId, float damage, Vector3 dir, float radius, int force, byte slot, int weaponId, UberstrikeItemClass weaponClass, DamageEffectType damageEffectFlag = DamageEffectType.None, float damageEffectValue = 0f)
	{
		Collider[] array = Physics.OverlapSphere(position, radius, UberstrikeLayerMasks.ExplosionMask);
		Collider[] array2 = array;
		Collider[] array3 = array2;
		foreach (Collider collider in array3)
		{
			BaseGameProp component = collider.transform.GetComponent<BaseGameProp>();
			if (component != null && component.RecieveProjectileDamage)
			{
				DoExplosionDamage(component, position, collider.bounds.center, projectileId, damage, dir, radius, force, slot, weaponId, weaponClass, damageEffectFlag, damageEffectValue);
			}
		}
		if (Vector3.Distance(position, GameState.Current.Player.transform.position) < radius)
		{
			DoExplosionDamage(GameState.Current.Player.Character, position, GameState.Current.Player.transform.position, projectileId, damage, dir, radius, force, slot, weaponId, weaponClass, damageEffectFlag, damageEffectValue);
		}
	}

	private static void DoExplosionDamage(IShootable shootable, Vector3 explosionPoint, Vector3 hitPoint, int projectileId, float damage, Vector3 dir, float radius, int force, byte slot, int weaponId, UberstrikeItemClass weaponClass, DamageEffectType damageEffectFlag, float damageEffectValue)
	{
		if (!Physics.Linecast(explosionPoint, hitPoint, out RaycastHit hitInfo, UberstrikeLayerMasks.ProtectionMask) || hitInfo.transform == shootable.Transform || hitInfo.transform.GetComponent<BaseGameProp>() != null)
		{
			float num = (!(radius > 1f)) ? 0f : Mathf.Floor(Mathf.Clamp((hitPoint - explosionPoint).magnitude, 0f, radius));
			Vector3 vector = (hitPoint - explosionPoint).normalized;
			if (num < 0.01f)
			{
				vector = dir.normalized;
			}
			else if (Vector3.Angle(vector, Vector3.up) < 30f)
			{
				vector = Vector3.up;
				num = 0f;
			}
			short num2 = Convert.ToInt16(damage * (radius - num) / radius);
			Vector3 force2 = vector;
			if (shootable.IsLocal)
			{
				force2 *= (float)force;
				num2 = (short)(num2 / 2);
			}
			shootable.ApplyDamage(new DamageInfo(num2)
			{
				Force = force2,
				UpwardsForceMultiplier = 5f,
				Hitpoint = hitPoint,
				ProjectileID = projectileId,
				SlotId = slot,
				WeaponID = weaponId,
				WeaponClass = weaponClass,
				DamageEffectFlag = damageEffectFlag,
				DamageEffectValue = damageEffectValue,
				Distance = (byte)Mathf.Clamp(Mathf.CeilToInt(num), 0, 255)
			});
		}
	}
}
