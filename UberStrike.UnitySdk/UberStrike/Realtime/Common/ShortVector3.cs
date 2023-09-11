
using Cmune.Realtime.Common.IO;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public struct ShortVector3
  {
    private Vector3 _vector;

    public ShortVector3(byte[] bytes, ref int idx) => this._vector = new Vector3((float) DefaultByteConverter.ToShort(bytes, ref idx) / 10f, (float) DefaultByteConverter.ToShort(bytes, ref idx) / 100f, (float) DefaultByteConverter.ToShort(bytes, ref idx) / 10f);

    public ShortVector3(Vector3 v) => this._vector = v;

    public Vector3 Vector3 => this._vector;

    public static Vector3 OptimizedVector3(Vector3 v) => new Vector3((float) (short) Mathf.Clamp(v.x * 10f, (float) short.MinValue, (float) short.MaxValue) / 10f, (float) (short) Mathf.Clamp(v.y * 100f, (float) short.MinValue, (float) short.MaxValue) / 100f, (float) (short) Mathf.Clamp(v.z * 10f, (float) short.MinValue, (float) short.MaxValue) / 10f);

    public Vector3 GetOptimizedVector3() => new Vector3((float) (short) Mathf.Clamp(this._vector.x * 10f, (float) short.MinValue, (float) short.MaxValue) / 10f, (float) (short) Mathf.Clamp(this._vector.y * 100f, (float) short.MinValue, (float) short.MaxValue) / 100f, (float) (short) Mathf.Clamp(this._vector.z * 10f, (float) short.MinValue, (float) short.MaxValue) / 10f);

    public static void Bytes(List<byte> bytes, Vector3 v)
    {
      short f1 = (short) Mathf.Clamp(v.x * 10f, (float) short.MinValue, (float) short.MaxValue);
      short f2 = (short) Mathf.Clamp(v.y * 100f, (float) short.MinValue, (float) short.MaxValue);
      short f3 = (short) Mathf.Clamp(v.z * 10f, (float) short.MinValue, (float) short.MaxValue);
      DefaultByteConverter.FromShort(f1, ref bytes);
      DefaultByteConverter.FromShort(f2, ref bytes);
      DefaultByteConverter.FromShort(f3, ref bytes);
    }

    public static Vector3 FromBytes(byte[] bytes, ref int idx) => new Vector3((float) DefaultByteConverter.ToShort(bytes, ref idx) / 10f, (float) DefaultByteConverter.ToShort(bytes, ref idx) / 100f, (float) DefaultByteConverter.ToShort(bytes, ref idx) / 10f);

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(6);
      ShortVector3.Bytes(bytes, this._vector);
      return bytes.ToArray();
    }
  }
}
