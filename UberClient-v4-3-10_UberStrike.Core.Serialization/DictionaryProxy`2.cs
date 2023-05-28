// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DictionaryProxy`2
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
