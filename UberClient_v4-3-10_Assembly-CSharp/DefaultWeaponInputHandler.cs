// Decompiled with JetBrains decompiler
// Type: DefaultWeaponInputHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public class DefaultWeaponInputHandler : WeaponInputHandler
{
  private IWeaponFireHandler _primaryFireHandler;
  private IWeaponFireHandler _secondaryFireHandler;

  public DefaultWeaponInputHandler(
    IWeaponLogic logic,
    bool isLocal,
    bool autoFire,
    IWeaponFireHandler secondaryFireHandler = null)
    : base(logic, isLocal)
  {
    this._primaryFireHandler = !autoFire ? (IWeaponFireHandler) new SemiAutoFireHandler(logic.Decorator) : (IWeaponFireHandler) new FullAutoFireHandler(logic.Decorator);
    this._secondaryFireHandler = secondaryFireHandler;
  }

  public override void OnPrimaryFire(bool pressed) => this._primaryFireHandler.OnTriggerPulled(pressed);

  public override void OnSecondaryFire(bool pressed)
  {
    if (this._secondaryFireHandler == null)
      return;
    this._secondaryFireHandler.OnTriggerPulled(pressed);
  }

  public override void OnPrevWeapon()
  {
  }

  public override void OnNextWeapon()
  {
  }

  public override void Update() => this._primaryFireHandler.Update();

  public override bool CanChangeWeapon() => true;

  public override void Stop() => this._primaryFireHandler.Stop();
}
