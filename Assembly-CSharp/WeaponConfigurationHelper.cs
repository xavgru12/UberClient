using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UberStrike.Core.Models.Views;
using UnityEngine;

public static class WeaponConfigurationHelper
{
	private static Dictionary<int, SecureMemory<int>> rateOfFireCache;

	private static Dictionary<int, SecureMemory<int>> spreadCache;

	private static Dictionary<int, SecureMemory<int>> speedCache;

	private static Dictionary<int, SecureMemory<int>> splashCache;

	public static float MaxAmmo
	{
		get;
		private set;
	}

	public static float MaxDamage
	{
		get;
		private set;
	}

	public static float MaxAccuracySpread
	{
		get;
		private set;
	}

	public static float MaxProjectileSpeed
	{
		get;
		private set;
	}

	public static float MaxRateOfFire
	{
		get;
		private set;
	}

	public static float MaxRecoilKickback
	{
		get;
		private set;
	}

	public static float MaxSplashRadius
	{
		get;
		private set;
	}

	static WeaponConfigurationHelper()
	{
		rateOfFireCache = new Dictionary<int, SecureMemory<int>>();
		spreadCache = new Dictionary<int, SecureMemory<int>>();
		speedCache = new Dictionary<int, SecureMemory<int>>();
		splashCache = new Dictionary<int, SecureMemory<int>>();
		MaxSplashRadius = 1f;
		MaxRecoilKickback = 1f;
		MaxRateOfFire = 1f;
		MaxProjectileSpeed = 1f;
		MaxAccuracySpread = 1f;
		MaxDamage = 1f;
		MaxAmmo = 1f;
	}

	public static void UpdateWeaponStatistics(UberStrikeItemShopClientView shopView)
	{
		if (shopView != null && shopView.WeaponItems.Count > 0)
		{
			MaxAmmo = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.MaxAmmo).First().MaxAmmo;
			MaxSplashRadius = (float)shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.SplashRadius).First().SplashRadius / 100f;
			MaxRecoilKickback = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.RecoilKickback).First().RecoilKickback;
			MaxRateOfFire = (float)shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.RateOfFire).First().RateOfFire / 1000f;
			MaxProjectileSpeed = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.ProjectileSpeed).First().ProjectileSpeed;
			MaxAccuracySpread = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.AccuracySpread).First().AccuracySpread / 10;
			MaxDamage = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.DamagePerProjectile).First().DamagePerProjectile;
			foreach (UberStrikeItemWeaponView weaponItem in shopView.WeaponItems)
			{
				rateOfFireCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.RateOfFire);
				spreadCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.AccuracySpread);
				speedCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.ProjectileSpeed);
				splashCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.SplashRadius);
			}
		}
	}

	public static IEnumerator UpdateWeaponsStatistics(UberStrikeItemShopClientView shopView)
	{
		int count = 0;
		if (shopView != null && shopView.WeaponItems.Count > 0)
		{
			MaxAmmo = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.MaxAmmo).First().MaxAmmo;
			MaxSplashRadius = (float)shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.SplashRadius).First().SplashRadius / 100f;
			MaxRecoilKickback = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.RecoilKickback).First().RecoilKickback;
			MaxRateOfFire = (float)shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.RateOfFire).First().RateOfFire / 1000f;
			MaxProjectileSpeed = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.ProjectileSpeed).First().ProjectileSpeed;
			MaxAccuracySpread = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.AccuracySpread).First().AccuracySpread / 10;
			MaxDamage = shopView.WeaponItems.OrderByDescending((UberStrikeItemWeaponView item) => item.DamagePerProjectile).First().DamagePerProjectile;
			foreach (UberStrikeItemWeaponView weaponItem in shopView.WeaponItems)
			{
				count++;
				if (count == 10)
				{
					count = 0;
					yield return null;
				}
				rateOfFireCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.RateOfFire);
				spreadCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.AccuracySpread);
				speedCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.ProjectileSpeed);
				splashCache[weaponItem.ID] = new SecureMemory<int>(weaponItem.SplashRadius);
			}
		}
	}

	public static float GetAmmoNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.MaxAmmo / MaxAmmo;
		}
		return 0f;
	}

	public static float GetDamageNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)(view.DamagePerProjectile * view.ProjectilesPerShot) / MaxDamage;
		}
		return 0f;
	}

	public static float GetAccuracySpreadNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.AccuracySpread / 10f / MaxAccuracySpread;
		}
		return 0f;
	}

	public static float GetProjectileSpeedNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.ProjectileSpeed / MaxProjectileSpeed;
		}
		return 0f;
	}

	public static float GetRateOfFireNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.RateOfFire / 1000f / MaxRateOfFire;
		}
		return 0f;
	}

	public static float GetRecoilKickbackNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.RecoilKickback / MaxRecoilKickback;
		}
		return 0f;
	}

	public static float GetSplashRadiusNormalized(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.SplashRadius / 100f / MaxSplashRadius;
		}
		return 0f;
	}

	public static float GetAmmo(UberStrikeItemWeaponView view)
	{
		return view?.MaxAmmo ?? 0;
	}

	public static float GetDamage(UberStrikeItemWeaponView view)
	{
		return (view != null) ? (view.DamagePerProjectile * view.ProjectilesPerShot) : 0;
	}

	public static float GetRecoilKickback(UberStrikeItemWeaponView view)
	{
		return view?.RecoilKickback ?? 0;
	}

	public static float GetRecoilMovement(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.RecoilMovement / 100f;
		}
		return 0f;
	}

	public static float GetCriticalStrikeBonus(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)view.CriticalStrikeBonus / 100f;
		}
		return 0f;
	}

	public static float GetAccuracySpread(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)GetSecureSpread(view.ID) / 10f;
		}
		return 0f;
	}

	public static float GetRateOfFire(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)GetSecureRateOfFire(view.ID) / 1000f;
		}
		return 1f;
	}

	public static float GetProjectileSpeed(UberStrikeItemWeaponView view)
	{
		return (view == null) ? 1 : GetSecureProjectileSpeed(view.ID);
	}

	public static float GetSplashRadius(UberStrikeItemWeaponView view)
	{
		if (view != null)
		{
			return (float)GetSecureSplashRadius(view.ID) / 100f;
		}
		return 0f;
	}

	public static int GetSecureRateOfFire(int itemId)
	{
		if (rateOfFireCache.TryGetValue(itemId, out SecureMemory<int> value))
		{
			return value.ReadData(secure: true);
		}
		return 1;
	}

	public static int GetSecureSpread(int itemId)
	{
		if (spreadCache.TryGetValue(itemId, out SecureMemory<int> value))
		{
			return value.ReadData(secure: true);
		}
		return Mathf.RoundToInt(MaxAccuracySpread * 10f);
	}

	public static int GetSecureProjectileSpeed(int itemId)
	{
		if (speedCache.TryGetValue(itemId, out SecureMemory<int> value))
		{
			return value.ReadData(secure: true);
		}
		return 1;
	}

	public static int GetSecureSplashRadius(int itemId)
	{
		if (splashCache.TryGetValue(itemId, out SecureMemory<int> value))
		{
			return value.ReadData(secure: true);
		}
		return 0;
	}
}
