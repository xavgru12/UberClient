// Decompiled with JetBrains decompiler
// Type: SemiAutoFireHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public class SemiAutoFireHandler : IWeaponFireHandler
{
  private bool _isTriggerPulled;
  private BaseWeaponDecorator _weapon;

  public SemiAutoFireHandler(BaseWeaponDecorator weapon)
  {
    this._weapon = weapon;
    this._isTriggerPulled = false;
  }

  public void OnTriggerPulled(bool pulled)
  {
    if (pulled && !this._isTriggerPulled && Singleton<WeaponController>.Instance.Shoot())
      this._weapon.PostShoot();
    this._isTriggerPulled = pulled;
  }

  public void Update()
  {
  }

  public void Stop()
  {
  }
}
