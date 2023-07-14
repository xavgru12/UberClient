// Decompiled with JetBrains decompiler
// Type: EnviromentSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviromentSettings
{
  public EnviromentSettings.TYPE Type;
  public Bounds EnviromentBounds;
  public float GroundAcceleration = 15f;
  public float GroundFriction = 8f;
  public float AirAcceleration = 3f;
  public float WaterAcceleration = 6f;
  public float WaterFriction = 2f;
  public float FlyAcceleration = 8f;
  public float FlyFriction = 3f;
  public float SpectatorFriction = 5f;
  public float StopSpeed = 8f;
  public float Gravity = 50f;

  public void CheckPlayerEnclosure(Vector3 position, float height, out float enclosure)
  {
    if (this.EnviromentBounds.Contains(position))
    {
      float distance;
      if (this.EnviromentBounds.IntersectRay(new Ray(position + Vector3.up * height, Vector3.down), out distance))
        enclosure = (height - distance) / height;
      else
        enclosure = 0.0f;
    }
    else
      enclosure = 0.0f;
  }

  public override string ToString() => string.Format("Type: ", (object) this.Type);

  public enum TYPE
  {
    NONE,
    WATER,
    SURFACE,
    LATTER,
  }
}
