// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DictionaryProxy`2
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class DictionaryProxy<S, T>
  {
    public static void Serialize(
      Stream bytes,
      Dictionary<S, T> instance,
      DictionaryProxy<S, T>.Serializer<S> keySerialization,
      DictionaryProxy<S, T>.Serializer<T> valueSerialization)
    {
      Int32Proxy.Serialize(bytes, instance.Count);
      foreach (KeyValuePair<S, T> keyValuePair in instance)
      {
        keySerialization(bytes, keyValuePair.Key);
        valueSerialization(bytes, keyValuePair.Value);
      }
    }

    public static Dictionary<S, T> Deserialize(
      Stream bytes,
      DictionaryProxy<S, T>.Deserializer<S> keySerialization,
      DictionaryProxy<S, T>.Deserializer<T> valueSerialization)
    {
      int capacity = Int32Proxy.Deserialize(bytes);
      Dictionary<S, T> dictionary = new Dictionary<S, T>(capacity);
      for (int index = 0; index < capacity; ++index)
        dictionary.Add(keySerialization(bytes), valueSerialization(bytes));
      return dictionary;
    }

    public delegate void Serializer<U>(Stream stream, U instance);

    public delegate U Deserializer<U>(Stream stream);
  }
}
