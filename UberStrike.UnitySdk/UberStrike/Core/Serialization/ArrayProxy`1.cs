// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ArrayProxy`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ArrayProxy<T>
  {
    public static void Serialize(Stream bytes, T[] instance, Action<Stream, T> serialization)
    {
      UShortProxy.Serialize(bytes, (ushort) instance.Length);
      foreach (T obj in instance)
        serialization(bytes, obj);
    }

    public static T[] Deserialize(Stream bytes, ArrayProxy<T>.Deserializer<T> serialization)
    {
      ushort length = UShortProxy.Deserialize(bytes);
      T[] objArray = new T[(int) length];
      for (int index = 0; index < (int) length; ++index)
        objArray[index] = serialization(bytes);
      return objArray;
    }

    public delegate void Serializer<U>(Stream stream, U instance);

    public delegate U Deserializer<U>(Stream stream);
  }
}
