// Decompiled with JetBrains decompiler
// Type: WeaponConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UnityEngine;

[Serializable]
public class WeaponConfiguration
{
  public int WeaponId;
  public UberstrikeItemClass WeaponClass;
  public int Damage;
  public float SplashRadius;
  public int Force;
  public float ReloadTime;
  public int RecoilKickback;
  public float RecoilMovement;
  public Vector2 AccuracySpread;
  public float RateOfFire;
  public int Range;
  public int ProjectileSpeed;
  public Vector2 ZoomLimits;
  public DamageEffectType DamageEffectFlag;
  public float DamageEffectValue;
}
