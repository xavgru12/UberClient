// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ArrayProxy`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
