// Decompiled with JetBrains decompiler
// Type: EnumerationExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public static class EnumerationExtensions
{
  public static T[] ValueArray<S, T>(this Dictionary<S, T> dict)
  {
    T[] array = new T[dict.Count];
    dict.Values.CopyTo(array, 0);
    return array;
  }

  public static S[] KeyArray<S, T>(this Dictionary<S, T> dict)
  {
    S[] array = new S[dict.Count];
    dict.Keys.CopyTo(array, 0);
    return array;
  }

  public static KeyValuePair<S, T> First<S, T>(this Dictionary<S, T> dict)
  {
    Dictionary<S, T>.Enumerator enumerator = dict.GetEnumerator();
    enumerator.MoveNext();
    return enumerator.Current;
  }
}
