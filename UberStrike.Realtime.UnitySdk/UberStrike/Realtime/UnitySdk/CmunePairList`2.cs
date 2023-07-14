// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmunePairList`2
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class CmunePairList<T1, T2> : List<KeyValuePair<T1, T2>>
  {
    public CmunePairList()
    {
    }

    public CmunePairList(int capacity)
      : base(capacity)
    {
    }

    public CmunePairList(IEnumerable<KeyValuePair<T1, T2>> collection)
      : base(collection)
    {
    }

    public CmunePairList(IEnumerable<T1> collection1, IEnumerable<T2> collection2)
    {
      IEnumerator<T1> enumerator1 = collection1.GetEnumerator();
      IEnumerator<T2> enumerator2 = collection2.GetEnumerator();
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
        this.Add(new KeyValuePair<T1, T2>(enumerator1.Current, enumerator2.Current));
    }

    public ICollection<KeyValuePair<T1, T2>> GetPairsWithKey(T1 key) => (ICollection<KeyValuePair<T1, T2>>) this.FindAll((Predicate<KeyValuePair<T1, T2>>) (p => p.Key.Equals((object) key)));

    public ICollection<KeyValuePair<T1, T2>> GetPairsWithValue(T2 value) => (ICollection<KeyValuePair<T1, T2>>) this.FindAll((Predicate<KeyValuePair<T1, T2>>) (p => p.Value.Equals((object) value)));

    public ICollection<T1> Keys
    {
      get
      {
        List<T1> l = new List<T1>(this.Count);
        this.ForEach((Action<KeyValuePair<T1, T2>>) (p => l.Add(p.Key)));
        return (ICollection<T1>) l;
      }
    }

    public ICollection<T2> Values
    {
      get
      {
        List<T2> l = new List<T2>(this.Count);
        this.ForEach((Action<KeyValuePair<T1, T2>>) (p => l.Add(p.Value)));
        return (ICollection<T2>) l;
      }
    }

    public void Add(T1 first, T2 second) => this.Add(new KeyValuePair<T1, T2>(first, second));

    public void Clamp(int max)
    {
      if (this.Count <= max)
        return;
      this.RemoveRange(max, this.Count - max);
    }
  }
}
