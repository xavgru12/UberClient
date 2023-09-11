// Decompiled with JetBrains decompiler
// Type: UberStrike.Helper.LimitedQueue`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Helper
{
  public class LimitedQueue<T> : IEnumerable<T>, IEnumerable
  {
    private List<T> _list;
    private int _capacity;

    public T LastItem { get; private set; }

    public LimitedQueue(int capacity)
    {
      this._capacity = capacity;
      this._list = new List<T>(capacity);
    }

    public T this[int index]
    {
      get => this._list[index];
      set => this._list[index] = value;
    }

    public bool Contains(T item) => this._list.Contains(item);

    public bool Remove(T item) => this._list.Remove(item);

    public bool EnqueueUnique(T item)
    {
      int num = this._list.RemoveAll((Predicate<T>) (p => p.Equals((object) item)));
      this.Enqueue(item);
      return num == 0;
    }

    public void Enqueue(T item)
    {
      this.LastItem = this._list.Count + 1 <= this._capacity ? default (T) : this.Dequeue();
      this._list.Add(item);
    }

    public T Dequeue()
    {
      T obj = default (T);
      if (this._list.Count > 0)
      {
        obj = this._list[0];
        this._list.RemoveAt(0);
      }
      return obj;
    }

    public T Peek() => this._list.Count > 0 ? this._list[0] : default (T);

    public T Tail() => this._list.Count > 0 ? this._list[this._list.Count - 1] : default (T);

    public int Count => this._list.Count;

    public void Clear() => this._list.Clear();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
