
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cmune.Realtime.Common.Utils
{
  public static class Conversion
  {
    public static T[] ToArray<T>(ICollection<T> collection)
    {
      T[] array = new T[collection.Count];
      collection.CopyTo(array, 0);
      return array;
    }

    public static Array ToArray(ICollection collection)
    {
      object[] array = new object[collection.Count];
      collection.CopyTo((Array) array, 0);
      return (Array) array;
    }

    public static T ToEnum<T>(string value) => typeof (T).IsEnum && !string.IsNullOrEmpty(value) && Enum.IsDefined(typeof (T), (object) value) ? (T) Enum.Parse(typeof (T), value) : default (T);

    public static float Deg2Rad(float angle) => Mathf.Abs((float) (((double) angle % 360.0 + 360.0) % 360.0 / 360.0));

    public static byte Angle2Byte(float angle) => (byte) ((double) byte.MaxValue * (double) Conversion.Deg2Rad(angle));

    public static float Byte2Angle(byte angle) => 360f * (float) angle / (float) byte.MaxValue;

    public static ushort Angle2Short(float angle) => (ushort) ((double) ushort.MaxValue * (double) Conversion.Deg2Rad(angle));

    public static float Short2Angle(ushort angle) => 360f * (float) angle / (float) ushort.MaxValue;
  }
}
