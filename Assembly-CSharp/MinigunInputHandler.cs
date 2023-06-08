using UberStrike.Core.Models.Views;
using UnityEngine;

public class MinigunInputHandler : WeaponInputHandler
{
	protected bool _isGunWarm;

	protected bool _isWarmupPlayed;

	protected float _warmTime;

	private bool _isTriggerPulled;

	private MinigunWeaponDecorator _weapon;

	public MinigunInputHandler(IWeaponLogic logic, bool isLocal, MinigunWeaponDecorator weapon, UberStrikeItemWeaponView view)
		: base(logic, isLocal)
	{
		_weapon = weapon;
		base.FireHandler = new FullAutoFireHandler(weapon, WeaponConfigurationHelper.GetRateOfFire(view));
	}

	public override void Update()
	{
		if (!_weapon)
		{
			return;
		}
		if (_warmTime < _weapon.MaxWarmUpTime)
		{
			if (_isGunWarm || _isTriggerPulled)
			{
				if (!_isWarmupPlayed)
				{
					_isWarmupPlayed = true;
					_weapon.PlayWindUpSound(_warmTime);
				}
				_warmTime += Time.deltaTime;
				if (_warmTime >= _weapon.MaxWarmUpTime)
				{
					_weapon.PlayDuringSound();
				}
				_weapon.SpinWeaponHead();
			}
			base.FireHandler.OnTriggerPulled(pulled: false);
		}
		else if (_isTriggerPulled)
		{
			base.FireHandler.OnTriggerPulled(pulled: true);
		}
		else if (_isGunWarm)
		{
			_weapon.SpinWeaponHead();
			base.FireHandler.OnTriggerPulled(pulled: false);
		}
		else
		{
			base.FireHandler.OnTriggerPulled(pulled: false);
		}
		if (!_isGunWarm && !_isTriggerPulled)
		{
			if (_warmTime > 0f)
			{
				_warmTime -= Time.deltaTime;
				if (_warmTime < 0f)
				{
					_warmTime = 0f;
				}
				if (_isWarmupPlayed)
				{
					_weapon.PlayWindDownSound((1f - _warmTime / _weapon.MaxWarmUpTime) * _weapon.MaxWarmDownTime);
				}
			}
			_isWarmupPlayed = false;
		}
		base.FireHandler.Update();
	}

	public override void OnSecondaryFire(bool pressed)
	{
		_isGunWarm = pressed;
	}

	public override bool CanChangeWeapon()
	{
		return !_isGunWarm;
	}

	public override void Stop()
	{
		_warmTime = 0f;
		_isGunWarm = false;
		_isWarmupPlayed = false;
		_isTriggerPulled = false;
		base.FireHandler.Stop();
		if ((bool)_weapon)
		{
			_weapon.StopSound();
		}
	}

	public override void OnPrimaryFire(bool pressed)
	{
		_isTriggerPulled = pressed;
	}

	public override void OnPrevWeapon()
	{
	}

	public override void OnNextWeapon()
	{
	}
}
