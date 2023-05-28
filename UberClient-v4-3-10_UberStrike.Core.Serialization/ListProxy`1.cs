// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ListProxy`1
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ListProxy<T>
  {
    public static void Serialize(
      Stream bytes,
      ICollection<T> instance,
      ListProxy<T>.Serializer<T> serialization)
    {
      UShortProxy.Serialize(bytes, (ushort) instance.Count);
      foreach (T instance1 in (IEnumerable<T>) instance)
        serialization(bytes, instance1);
    }

    public static List<T> Deserialize(Stream bytes, ListProxy<T>.Deserializer<T> serialization)
    {
      ushort capacity = UShortProxy.Deserialize(bytes);
      List<T> objList = new List<T>((int) capacity);
      for (int index = 0; index < (int) capacity; ++index)
        objList.Add(serialization(bytes));
      return objList;
    }

    public delegate void Serializer<U>(Stream stream, U instance);

    public delegate U Deserializer<U>(Stream stream);
  }
}
