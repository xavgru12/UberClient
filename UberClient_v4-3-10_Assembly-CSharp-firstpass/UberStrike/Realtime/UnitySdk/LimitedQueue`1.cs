// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.LimitedQueue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class LimitedQueue<T> : IEnumerable, IEnumerable<T>
  {
    private List<T> _list;
    private int _capacity;

    public T LastItem { get; private set; }

    public T this[int index]
    {
      get => this._list[index];
      set => this._list[index] = value;
    }

    public int Count => this._list.Count;

    public LimitedQueue(int capacity)
    {
      this._capacity = capacity;
      this._list = new List<T>(capacity);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

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

    public void Clear() => this._list.Clear();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();
  }
}
