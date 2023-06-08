using UberStrike.Core.Models.Views;
using UnityEngine;

public class IronsightInputHandler : WeaponInputHandler
{
	protected bool _isIronsight;

	protected float _ironSightDelay;

	public IronsightInputHandler(IWeaponLogic logic, bool isLocal, ZoomInfo zoomInfo, UberStrikeItemWeaponView view)
		: base(logic, isLocal)
	{
		_zoomInfo = zoomInfo;
		if (view.HasAutomaticFire)
		{
			base.FireHandler = new FullAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		}
		else
		{
			base.FireHandler = new SemiAutoFireHandler(logic.Decorator, WeaponConfigurationHelper.GetRateOfFire(view));
		}
	}

	public override void OnSecondaryFire(bool pressed)
	{
		_isIronsight = pressed;
	}

	public override void Update()
	{
		base.FireHandler.Update();
		UpdateIronsight();
		if (_isIronsight)
		{
			if (!LevelCamera.IsZoomedIn)
			{
				WeaponInputHandler.ZoomIn(_zoomInfo, _weaponLogic.Decorator, 0f, hideWeapon: false);
			}
		}
		else if (LevelCamera.IsZoomedIn)
		{
			WeaponInputHandler.ZoomOut(_zoomInfo, _weaponLogic.Decorator);
		}
		if (!_isIronsight && _ironSightDelay > 0f)
		{
			_ironSightDelay -= Time.deltaTime;
		}
	}

	public override void Stop()
	{
		base.FireHandler.Stop();
		if (_isIronsight)
		{
			_isIronsight = false;
			if (_isLocal)
			{
				LevelCamera.ResetZoom();
			}
			if (WeaponFeedbackManager.Instance.IsIronSighted)
			{
				WeaponFeedbackManager.Instance.ResetIronSight();
			}
		}
	}

	public override bool CanChangeWeapon()
	{
		if (!_isIronsight)
		{
			return _ironSightDelay <= 0f;
		}
		return false;
	}

	private void UpdateIronsight()
	{
		if (_isIronsight)
		{
			if (!WeaponFeedbackManager.Instance.IsIronSighted)
			{
				WeaponFeedbackManager.Instance.BeginIronSight();
			}
		}
		else if (WeaponFeedbackManager.Instance.IsIronSighted)
		{
			WeaponFeedbackManager.Instance.EndIronSight();
		}
	}

	public override void OnPrimaryFire(bool pressed)
	{
		base.FireHandler.OnTriggerPulled(pressed);
	}

	public override void OnPrevWeapon()
	{
	}

	public override void OnNextWeapon()
	{
	}
}
