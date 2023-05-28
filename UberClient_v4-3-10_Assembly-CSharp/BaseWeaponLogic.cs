// Decompiled with JetBrains decompiler
// Type: BaseWeaponLogic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public abstract class BaseWeaponLogic : IWeaponLogic
{
  protected BaseWeaponLogic(WeaponItem item, IWeaponController controller)
  {
    this.Controller = controller;
    this.Config = item.Configuration;
  }

  public event Action<CmunePairList<BaseGameProp, ShotPoint>> OnTargetHit;

  public IWeaponController Controller { get; private set; }

  public WeaponItemConfiguration Config { get; private set; }

  public abstract BaseWeaponDecorator Decorator { get; }

  public virtual float HitDelay => 0.0f;

  public bool IsWeaponReady { get; private set; }

  public bool IsWeaponActive { get; set; }

  public abstract void Shoot(Ray ray, out CmunePairList<BaseGameProp, ShotPoint> hits);

  protected void OnHits(CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    if (this.OnTargetHit == null)
      return;
    this.OnTargetHit(hits);
  }
}
