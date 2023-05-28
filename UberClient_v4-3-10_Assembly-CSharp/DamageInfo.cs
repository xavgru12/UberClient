// Decompiled with JetBrains decompiler
// Type: DamageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DamageInfo
{
  public DamageInfo(short damage)
  {
    this.Damage = damage;
    this.Force = Vector3.zero;
  }

  public short Damage { get; set; }

  public Vector3 Force { get; set; }

  public Vector3 Hitpoint { get; set; }

  public BodyPart BodyPart { get; set; }

  public int ProjectileID { get; set; }

  public int ShotCount { get; set; }

  public int WeaponID { get; set; }

  public UberstrikeItemClass WeaponClass { get; set; }

  public float CriticalStrikeBonus { get; set; }

  public DamageEffectType DamageEffectFlag { get; set; }

  public float DamageEffectValue { get; set; }
}
