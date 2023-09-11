
using Cmune.Util;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public struct RotationVector3
  {
    private Vector3 _vector;

    public RotationVector3(float x, float y, float z)
      : this(new Vector3(x, y, z))
    {
    }

    public RotationVector3(byte[] bytes, ref int idx)
    {
      byte num1 = bytes[idx++];
      byte num2 = bytes[idx++];
      byte num3 = bytes[idx++];
      int num4 = (int) num1 | (int) num2 << 8;
      this._vector.x = (float) (num4 % 360);
      this._vector.y = (float) (Mathf.Clamp(num4 / 360, 0, 180) - 90);
      this._vector.z = (float) (Mathf.Clamp((int) num3, 0, 180) - 90);
    }

    public RotationVector3(Vector3 v)
    {
      CmuneDebug.Assert((double) v.x >= -360.0 && (double) v.x <= 360.0, "Pitch Angle out of Range");
      CmuneDebug.Assert((double) v.y >= -90.0 && (double) v.y <= 90.0, "Yaw Angle out of Range");
      CmuneDebug.Assert((double) v.z >= -90.0 && (double) v.z <= 90.0, "Roll Angle out of Range");
      this._vector = v;
    }

    public Vector3 Vector3 => this._vector;

    public static void Bytes(List<byte> bytes, Vector3 v)
    {
      byte[] collection = new byte[3];
      uint num = (uint) (((double) v.x < 0.0 ? 360.0 + (double) v.x % 360.0 : (double) v.x % 360.0) + (double) Mathf.Clamp(v.y + 90f, 0.0f, 180f) * 360.0);
      collection[0] = (byte) (num & (uint) byte.MaxValue);
      collection[1] = (byte) (num >> 8 & (uint) byte.MaxValue);
      collection[2] = (byte) Mathf.Clamp(v.z + 90f, 0.0f, 180f);
      bytes.AddRange((IEnumerable<byte>) collection);
    }

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(3);
      RotationVector3.Bytes(bytes, this._vector);
      return bytes.ToArray();
    }
  }
}
