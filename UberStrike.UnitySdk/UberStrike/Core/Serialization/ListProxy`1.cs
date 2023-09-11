// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ListProxy`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
