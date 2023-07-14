// Decompiled with JetBrains decompiler
// Type: MinigunInputHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MinigunInputHandler : WeaponInputHandler
{
  protected bool _isGunWarm;
  protected bool _isWarmupPlayed;
  protected float _warmTime;
  private MinigunWeaponDecorator _decorator;

  public MinigunInputHandler(IWeaponLogic logic, bool isLocal, MinigunWeaponDecorator decorator)
    : base(logic, isLocal)
  {
    this._decorator = decorator;
  }

  public override void Update()
  {
    if (!(bool) (Object) this._decorator)
      return;
    if ((double) this._warmTime < (double) this._decorator.MaxWarmUpTime)
    {
      if (this._isGunWarm || this._isTriggerPulled)
      {
        if (!this._isWarmupPlayed)
        {
          this._isWarmupPlayed = true;
          this._decorator.PlayWindUpSound(this._warmTime);
        }
        this._warmTime += Time.deltaTime;
        if ((double) this._warmTime >= (double) this._decorator.MaxWarmUpTime)
          this._decorator.PlayDuringSound();
        this._decorator.SpinWeaponHead();
      }
    }
    else if (this._isTriggerPulled)
      Singleton<WeaponController>.Instance.Shoot();
    else if (this._isGunWarm)
      this._decorator.SpinWeaponHead();
    if (this._isGunWarm || this._isTriggerPulled)
      return;
    if ((double) this._warmTime > 0.0)
    {
      this._warmTime -= Time.deltaTime;
      if ((double) this._warmTime < 0.0)
        this._warmTime = 0.0f;
      if (this._isWarmupPlayed)
        this._decorator.PlayWindDownSound((float) (1.0 - (double) this._warmTime / (double) this._decorator.MaxWarmUpTime) * this._decorator.MaxWarmDownTime);
    }
    this._isWarmupPlayed = false;
  }

  public override void OnSecondaryFire(bool pressed) => this._isGunWarm = pressed;

  public override bool CanChangeWeapon() => !this._isGunWarm;

  public override void Stop()
  {
    this._warmTime = 0.0f;
    this._isGunWarm = false;
    this._isWarmupPlayed = false;
    this._isTriggerPulled = false;
    if (!(bool) (Object) this._decorator)
      return;
    this._decorator.StopSound();
  }

  public override void OnPrimaryFire(bool pressed) => this._isTriggerPulled = pressed;

  public override void OnPrevWeapon()
  {
  }

  public override void OnNextWeapon()
  {
  }
}
