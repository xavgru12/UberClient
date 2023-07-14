// Decompiled with JetBrains decompiler
// Type: WeaponShootEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WeaponShootEvent
{
  public Vector3 Force;
  public float Noise;
  public float Angle;

  public WeaponShootEvent(Vector3 force, float noise, float angle)
  {
    this.Force = force;
    this.Noise = noise;
    this.Angle = angle;
  }
}
