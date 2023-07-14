// Decompiled with JetBrains decompiler
// Type: FullAutoFireHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FullAutoFireHandler : IWeaponFireHandler
{
  private bool _isShooting;
  private bool _isTriggerPulled;
  private BaseWeaponDecorator _weapon;

  public FullAutoFireHandler(BaseWeaponDecorator weapon) => this._weapon = weapon;

  public void OnTriggerPulled(bool pulled) => this._isTriggerPulled = pulled;

  public void Update()
  {
    if (this._isTriggerPulled && Singleton<WeaponController>.Instance.Shoot())
      this._isShooting = true;
    if (!this._isShooting || this._isTriggerPulled)
      return;
    this._isShooting = false;
    if (!(bool) (Object) this._weapon)
      return;
    this._weapon.PostShoot();
  }

  public void Stop() => this._isTriggerPulled = false;
}
