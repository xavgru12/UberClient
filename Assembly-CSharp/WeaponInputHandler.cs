using UnityEngine;

public abstract class WeaponInputHandler
{
	protected bool _isLocal;

	protected IWeaponLogic _weaponLogic;

	protected ZoomInfo _zoomInfo;

	public IWeaponFireHandler FireHandler
	{
		get;
		protected set;
	}

	protected WeaponInputHandler(IWeaponLogic logic, bool isLocal)
	{
		_isLocal = isLocal;
		_weaponLogic = logic;
	}

	protected static void ZoomIn(ZoomInfo zoomInfo, BaseWeaponDecorator weapon, float zoom, bool hideWeapon)
	{
		if ((bool)weapon)
		{
			if (!LevelCamera.IsZoomedIn)
			{
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperScopeIn, 0uL);
			}
			else if (zoom < 0f && zoomInfo.CurrentMultiplier != zoomInfo.MinMultiplier)
			{
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperZoomIn, 0uL);
			}
			else if (zoom > 0f && zoomInfo.CurrentMultiplier != zoomInfo.MaxMultiplier)
			{
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperZoomOut, 0uL);
			}
			zoomInfo.CurrentMultiplier = Mathf.Clamp(zoomInfo.CurrentMultiplier + zoom, zoomInfo.MinMultiplier, zoomInfo.MaxMultiplier);
			if (ApplicationDataManager.ApplicationOptions.FOVMode)
			{
				LevelCamera.DoZoomIn(100f / zoomInfo.CurrentMultiplier, 20f, hideWeapon);
			}
			else
			{
				LevelCamera.DoZoomIn(75f / zoomInfo.CurrentMultiplier, 20f, hideWeapon);
			}
			UserInputt.ZoomSpeed = 0.5f;
		}
	}

	protected static void ZoomOut(ZoomInfo zoomInfo, BaseWeaponDecorator weapon)
	{
		if (ApplicationDataManager.ApplicationOptions.FOVMode)
		{
			LevelCamera.DoZoomOut(100f, 10f);
		}
		else
		{
			LevelCamera.DoZoomOut(75f, 10f);
		}
		UserInputt.ZoomSpeed = 1f;
		if (zoomInfo != null)
		{
			zoomInfo.CurrentMultiplier = zoomInfo.DefaultMultiplier;
		}
		if ((bool)weapon)
		{
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(GameAudio.SniperScopeOut, 0uL);
		}
	}

	public abstract void OnPrimaryFire(bool pressed);

	public abstract void OnSecondaryFire(bool pressed);

	public abstract void OnPrevWeapon();

	public abstract void OnNextWeapon();

	public abstract void Update();

	public abstract bool CanChangeWeapon();

	public virtual void Stop()
	{
	}
}
