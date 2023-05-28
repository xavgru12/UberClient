// Decompiled with JetBrains decompiler
// Type: WeaponInputHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class WeaponInputHandler
{
  protected bool _isLocal;
  protected IWeaponLogic _weaponLogic;
  protected bool _isTriggerPulled;
  protected ZoomInfo _zoomInfo;

  protected WeaponInputHandler(IWeaponLogic logic, bool isLocal)
  {
    this._isLocal = isLocal;
    this._weaponLogic = logic;
    this._isTriggerPulled = false;
  }

  protected static void ZoomIn(ZoomInfo zoomInfo, BaseWeaponDecorator weapon, float zoom)
  {
    if (!(bool) (Object) weapon)
      return;
    if (!LevelCamera.Instance.IsZoomedIn)
      SfxManager.Play3dAudioClip(GameAudio.SniperScopeIn, weapon.transform.position);
    else if ((double) zoom < 0.0 && (double) zoomInfo.CurrentMultiplier != (double) zoomInfo.MinMultiplier)
      SfxManager.Play3dAudioClip(GameAudio.SniperZoomIn, weapon.transform.position);
    else if ((double) zoom > 0.0 && (double) zoomInfo.CurrentMultiplier != (double) zoomInfo.MaxMultiplier)
      SfxManager.Play3dAudioClip(GameAudio.SniperZoomOut, weapon.transform.position);
    zoomInfo.CurrentMultiplier = Mathf.Clamp(zoomInfo.CurrentMultiplier + zoom, zoomInfo.MinMultiplier, zoomInfo.MaxMultiplier);
    LevelCamera.Instance.DoZoomIn(60f / zoomInfo.CurrentMultiplier, 20f);
    UserInput.ZoomSpeed = 0.5f;
  }

  protected static void ZoomOut(ZoomInfo zoomInfo, BaseWeaponDecorator weapon)
  {
    if ((Object) LevelCamera.Instance != (Object) null)
      LevelCamera.Instance.DoZoomOut(60f, 10f);
    UserInput.ZoomSpeed = 1f;
    if (zoomInfo != null)
      zoomInfo.CurrentMultiplier = zoomInfo.DefaultMultiplier;
    if (!(bool) (Object) weapon)
      return;
    SfxManager.Play3dAudioClip(GameAudio.SniperScopeOut, weapon.transform.position);
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
