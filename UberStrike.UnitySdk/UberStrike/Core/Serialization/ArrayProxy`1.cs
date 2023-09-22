
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
