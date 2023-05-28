// Decompiled with JetBrains decompiler
// Type: SniperRifleInputHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

public class SniperRifleInputHandler : WeaponInputHandler
{
  protected const float ZOOM = 4f;
  protected bool _scopeOpen;
  protected float _zoom;
  private IWeaponFireHandler _fireHandler;

  public SniperRifleInputHandler(
    IWeaponLogic logic,
    bool isLocal,
    ZoomInfo zoomInfo,
    bool autoFire)
    : base(logic, isLocal)
  {
    this._zoomInfo = zoomInfo;
    if (autoFire)
      this._fireHandler = (IWeaponFireHandler) new FullAutoFireHandler(logic.Decorator);
    else
      this._fireHandler = (IWeaponFireHandler) new SemiAutoFireHandler(logic.Decorator);
  }

  public override void OnSecondaryFire(bool pressed)
  {
    this._scopeOpen = pressed;
    this.Update();
  }

  public override void OnPrevWeapon() => this._zoom = -4f;

  public override void OnNextWeapon() => this._zoom = 4f;

  public override void Update()
  {
    this._fireHandler.Update();
    if (this._scopeOpen)
    {
      if (LevelCamera.Instance.IsZoomedIn && (double) this._zoom == 0.0)
        return;
      WeaponInputHandler.ZoomIn(this._zoomInfo, this._weaponLogic.Decorator, this._zoom);
      this._zoom = 0.0f;
      CmuneEventHandler.Route((object) new OnCameraZoomInEvent());
      GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
    }
    else
    {
      if (!LevelCamera.Instance.IsZoomedIn)
        return;
      GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(true);
      WeaponInputHandler.ZoomOut(this._zoomInfo, this._weaponLogic.Decorator);
    }
  }

  public override bool CanChangeWeapon() => !this._scopeOpen;

  public override void Stop()
  {
    this._fireHandler.Stop();
    if (!this._scopeOpen)
      return;
    this._scopeOpen = false;
    if (!this._isLocal)
      return;
    LevelCamera.Instance.ResetZoom();
    if (!GameState.LocalCharacter.IsAlive)
      return;
    GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(true);
  }

  public override void OnPrimaryFire(bool pressed) => this._fireHandler.OnTriggerPulled(pressed);
}
