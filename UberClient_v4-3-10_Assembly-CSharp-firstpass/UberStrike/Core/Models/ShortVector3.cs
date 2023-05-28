// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.ShortVector3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace UberStrike.Core.Models
{
  public struct ShortVector3
  {
    private Vector3 value;

    public float x => this.value.x;

    public float y => this.value.y;

    public float z => this.value.z;

    public ShortVector3(Vector3 value) => this.value = value;

    public static implicit operator Vector3(ShortVector3 value) => value.value;

    public static implicit operator ShortVector3(Vector3 value) => new ShortVector3(value);

    public static ShortVector3 operator *(ShortVector3 vector, float value)
    {
      vector.value.x *= value;
      vector.value.y *= value;
      vector.value.z *= value;
      return vector;
    }

    public static ShortVector3 operator +(ShortVector3 vector, ShortVector3 value)
    {
      vector.value.x += value.value.x;
      vector.value.y += value.value.y;
      vector.value.z += value.value.z;
      return vector;
    }

    public static ShortVector3 operator -(ShortVector3 vector, ShortVector3 value)
    {
      vector.value.x -= value.value.x;
      vector.value.y -= value.value.y;
      vector.value.z -= value.value.z;
      return vector;
    }
  }
}
