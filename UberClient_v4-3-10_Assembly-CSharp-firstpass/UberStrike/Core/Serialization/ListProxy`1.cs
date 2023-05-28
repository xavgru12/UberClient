// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ListProxy`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
