// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmunePairList`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class CmunePairList<T1, T2> : List<KeyValuePair<T1, T2>>
  {
    public ICollection<T1> Keys
    {
      get
      {
        List<T1> i = new List<T1>(this.Count);
        this.ForEach((Action<KeyValuePair<T1, T2>>) (p => i.Add(p.Key)));
        return (ICollection<T1>) i;
      }
    }

    public ICollection<T2> Values
    {
      get
      {
        List<T2> i = new List<T2>(this.Count);
        this.ForEach((Action<KeyValuePair<T1, T2>>) (p => i.Add(p.Value)));
        return (ICollection<T2>) i;
      }
    }

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

    public void Add(T1 first, T2 second) => this.Add(new KeyValuePair<T1, T2>(first, second));

    public void Clamp(int max)
    {
      if (this.Count <= max)
        return;
      this.RemoveRange(max, this.Count - max);
    }
  }
}
