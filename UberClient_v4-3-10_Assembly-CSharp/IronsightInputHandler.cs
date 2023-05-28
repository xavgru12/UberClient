// Decompiled with JetBrains decompiler
// Type: IronsightInputHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IronsightInputHandler : WeaponInputHandler
{
  protected bool _isIronsight;
  protected float _ironSightDelay;
  private IWeaponFireHandler _fireHandler;

  public IronsightInputHandler(IWeaponLogic logic, bool isLocal, ZoomInfo zoomInfo, bool autoFire)
    : base(logic, isLocal)
  {
    this._zoomInfo = zoomInfo;
    if (autoFire)
      this._fireHandler = (IWeaponFireHandler) new FullAutoFireHandler(logic.Decorator);
    else
      this._fireHandler = (IWeaponFireHandler) new SemiAutoFireHandler(logic.Decorator);
  }

  public override void OnSecondaryFire(bool pressed) => this._isIronsight = pressed;

  public override void Update()
  {
    this._fireHandler.Update();
    this.UpdateIronsight();
    if (this._isIronsight)
    {
      if (!LevelCamera.Instance.IsZoomedIn)
        WeaponInputHandler.ZoomIn(this._zoomInfo, this._weaponLogic.Decorator, 0.0f);
    }
    else if (LevelCamera.Instance.IsZoomedIn)
      WeaponInputHandler.ZoomOut(this._zoomInfo, this._weaponLogic.Decorator);
    if (this._isIronsight || (double) this._ironSightDelay <= 0.0)
      return;
    this._ironSightDelay -= Time.deltaTime;
  }

  public override void Stop()
  {
    this._fireHandler.Stop();
    if (!this._isIronsight)
      return;
    this._isIronsight = false;
    if (this._isLocal)
      LevelCamera.Instance.ResetZoom();
    if (!WeaponFeedbackManager.Instance.IsIronSighted)
      return;
    WeaponFeedbackManager.Instance.ResetIronSight();
  }

  public override bool CanChangeWeapon() => !this._isIronsight && (double) this._ironSightDelay <= 0.0;

  private void UpdateIronsight()
  {
    if (this._isIronsight)
    {
      if (WeaponFeedbackManager.Instance.IsIronSighted)
        return;
      WeaponFeedbackManager.Instance.BeginIronSight();
    }
    else
    {
      if (!WeaponFeedbackManager.Instance.IsIronSighted)
        return;
      WeaponFeedbackManager.Instance.EndIronSight();
    }
  }

  public override void OnPrimaryFire(bool pressed) => this._fireHandler.OnTriggerPulled(pressed);

  public override void OnPrevWeapon()
  {
  }

  public override void OnNextWeapon()
  {
  }
}
