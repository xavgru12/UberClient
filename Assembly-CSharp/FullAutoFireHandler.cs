using UberStrike.Core.Models;
using UnityEngine;

public class FullAutoFireHandler : IWeaponFireHandler
{
	private BaseWeaponDecorator weapon;

	private float frequency;

	private float shootingStartTime;

	private int shootCounter;

	public bool IsTriggerPulled
	{
		get;
		private set;
	}

	public bool IsShooting
	{
		get;
		private set;
	}

	public bool CanShoot => shootingStartTime + frequency * (float)shootCounter <= Time.time;

	public FullAutoFireHandler(BaseWeaponDecorator weapon, float frequency)
	{
		this.weapon = weapon;
		this.frequency = frequency;
	}

	public void OnTriggerPulled(bool pulled)
	{
		IsTriggerPulled = pulled;
	}

	public void Update()
	{
		if (IsTriggerPulled && !IsShooting && CanShoot)
		{
			GameState.Current.PlayerData.Set(PlayerStates.Shooting, on: true);
			IsShooting = true;
			shootingStartTime = Time.time;
			shootCounter = 0;
		}
		if (IsShooting)
		{
			Singleton<WeaponController>.Instance.Shoot();
		}
		if (IsShooting && (!IsTriggerPulled || !Singleton<WeaponController>.Instance.CheckAmmoCount()))
		{
			GameState.Current.PlayerData.Set(PlayerStates.Shooting, on: false);
			IsShooting = false;
			if ((bool)weapon && !BaseWeaponDecorator._noAmmoSoundPlaying)
			{
				weapon.PostShoot();
			}
		}
	}

	public void Stop()
	{
		GameState.Current.PlayerData.Set(PlayerStates.Shooting, on: false);
		IsTriggerPulled = false;
		IsShooting = false;
	}

	public void RegisterShot()
	{
		shootCounter++;
	}
}
