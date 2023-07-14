// Decompiled with JetBrains decompiler
// Type: Boo.Lang.List`1
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Boo.Lang
{
  [Serializable]
  public class List<T> : 
    IEnumerable,
    ICollection,
    IEnumerable<T>,
    IList<T>,
    ICollection<T>,
    IEquatable<List<T>>,
    IList
  {
    private static readonly T[] EmptyArray = new T[0];
    protected T[] _items;
    protected int _count;

    public List() => this._items = List<T>.EmptyArray;

    public List(IEnumerable enumerable)
      : this()
    {
      this.Extend(enumerable);
    }

    public List(int initialCapacity)
    {
      this._items = initialCapacity >= 0 ? new T[initialCapacity] : throw new ArgumentOutOfRangeException(nameof (initialCapacity));
      this._count = 0;
    }

    public List(T[] items, bool takeOwnership)
    {
      if (items == null)
        throw new ArgumentNullException(nameof (items));
      this._items = !takeOwnership ? (T[]) items.Clone() : items;
      this._count = items.Length;
    }

    void ICollection<T>.Add(T item) => this.Push(item);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    void IList<T>.Insert(int index, T item) => this.Insert(index, item);

    void IList<T>.RemoveAt(int index) => this.InnerRemoveAt(this.CheckIndex(this.NormalizeIndex(index)));

    bool ICollection<T>.Remove(T item) => this.InnerRemove(item);

    int IList.Add(object value)
    {
      this.Add((T) value);
      return this.Count - 1;
    }

    void IList.Insert(int index, object value) => this.Insert(index, List<T>.Coerce(value));

    void IList.Remove(object value) => this.Remove(List<T>.Coerce(value));

    int IList.IndexOf(object value) => this.IndexOf(List<T>.Coerce(value));

    bool IList.Contains(object value) => this.Contains(List<T>.Coerce(value));

    object IList.this[int index]
    {
      get => (object) this[index];
      set => this[index] = List<T>.Coerce(value);
    }

    void IList.RemoveAt(int index) => this.RemoveAt(index);

    bool IList.IsFixedSize => false;

    void ICollection.CopyTo(Array array, int index) => Array.Copy((Array) this._items, 0, array, index, this._count);

    public List<T> Multiply(int count)
    {
      T[] objArray = count >= 0 ? new T[this._count * count] : throw new ArgumentOutOfRangeException(nameof (count));
      for (int index = 0; index < count; ++index)
        Array.Copy((Array) this._items, 0, (Array) objArray, index * this._count, this._count);
      return this.NewConcreteList(objArray, true);
    }

    protected virtual List<T> NewConcreteList(T[] items, bool takeOwnership) => new List<T>(items, takeOwnership);

    public IEnumerable<T> Reversed
    {
      get
      {
        List<T>.\u003C\u003Ec__Iterator5 reversed = new List<T>.\u003C\u003Ec__Iterator5()
        {
          \u003C\u003Ef__this = this
        };
        reversed.\u0024PC = -2;
        return (IEnumerable<T>) reversed;
      }
    }

    public int Count => this._count;

    [DebuggerHidden]
    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) new List<T>.\u003CGetEnumerator\u003Ec__Iterator6()
    {
      \u003C\u003Ef__this = this
    };

    public void CopyTo(T[] target, int index) => Array.Copy((Array) this._items, 0, (Array) target, index, this._count);

    public bool IsSynchronized => false;

    public object SyncRoot => (object) this._items;

    public bool IsReadOnly => false;

    public T this[int index]
    {
      get => this._items[this.CheckIndex(this.NormalizeIndex(index))];
      set => this._items[this.CheckIndex(this.NormalizeIndex(index))] = value;
    }

    public T FastAt(int normalizedIndex) => this._items[normalizedIndex];

    public List<T> Push(T item) => this.Add(item);

    public virtual List<T> Add(T item)
    {
      this.EnsureCapacity(this._count + 1);
      this._items[this._count] = item;
      ++this._count;
      return this;
    }

    public List<T> AddUnique(T item)
    {
      if (!this.Contains(item))
        this.Add(item);
      return this;
    }

    public List<T> Extend(IEnumerable enumerable)
    {
      this.AddRange(enumerable);
      return this;
    }

    public void AddRange(IEnumerable enumerable)
    {
      foreach (T obj in enumerable)
        this.Add(obj);
    }

    public List<T> ExtendUnique(IEnumerable enumerable)
    {
      foreach (T obj in enumerable)
        this.AddUnique(obj);
      return this;
    }

    public List<T> Collect(Predicate<T> condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      List<T> target = this.NewConcreteList(new T[0], true);
      this.InnerCollect(target, condition);
      return target;
    }

    public List<T> Collect(List<T> target, Predicate<T> condition)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      this.InnerCollect(target, condition);
      return target;
    }

    public T[] ToArray()
    {
      if (this._count == 0)
        return List<T>.EmptyArray;
      T[] target = new T[this._count];
      this.CopyTo(target, 0);
      return target;
    }

    public T[] ToArray(T[] array)
    {
      this.CopyTo(array, 0);
      return array;
    }

    public TOut[] ToArray<TOut>(Function<T, TOut> selector)
    {
      TOut[] array = new TOut[this._count];
      for (int index = 0; index < this._count; ++index)
        array[index] = selector(this._items[index]);
      return array;
    }

    public List<T> Sort()
    {
      Array.Sort((Array) this._items, 0, this._count, BooComparer.Default);
      return this;
    }

    public List<T> Sort(IComparer comparer)
    {
      Array.Sort((Array) this._items, 0, this._count, comparer);
      return this;
    }

    public List<T> Sort(Comparison<T> comparison) => this.Sort((IComparer<T>) new List<T>.ComparisonComparer(comparison));

    public List<T> Sort(IComparer<T> comparer)
    {
      Array.Sort<T>(this._items, 0, this._count, comparer);
      return this;
    }

    public List<T> Sort(Comparer comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      Array.Sort((Array) this._items, 0, this._count, (IComparer) comparer);
      return this;
    }

    public override string ToString() => "[" + this.Join(", ") + "]";

    public string Join(string separator) => Builtins.join((IEnumerable) this, separator);

    public override int GetHashCode()
    {
      int count = this._count;
      for (int index = 0; index < this._count; ++index)
      {
        T obj = this._items[index];
        if ((object) obj != null)
          count ^= obj.GetHashCode();
      }
      return count;
    }

    public override bool Equals(object other)
    {
      if (other == null)
        return false;
      return this == other || this.Equals(other as List<T>);
    }

    public bool Equals(List<T> other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (this._count != other.Count)
        return false;
      for (int index = 0; index < this._count; ++index)
      {
        if (!RuntimeServices.EqualityOperator((object) this._items[index], (object) other[index]))
          return false;
      }
      return true;
    }

    public void Clear()
    {
      for (int index = 0; index < this._count; ++index)
        this._items[index] = default (T);
      this._count = 0;
    }

    public List<T> GetRange(int begin) => this.InnerGetRange(this.AdjustIndex(this.NormalizeIndex(begin)), this._count);

    public List<T> GetRange(int begin, int end) => this.InnerGetRange(this.AdjustIndex(this.NormalizeIndex(begin)), this.AdjustIndex(this.NormalizeIndex(end)));

    public bool Contains(T item) => -1 != this.IndexOf(item);

    public bool Contains(Predicate<T> condition) => -1 != this.IndexOf(condition);

    public bool Find(Predicate<T> condition, out T found)
    {
      int index = this.IndexOf(condition);
      if (index != -1)
      {
        found = this._items[index];
        return true;
      }
      found = default (T);
      return false;
    }

    public List<T> FindAll(Predicate<T> condition)
    {
      List<T> all = this.NewConcreteList(new T[0], true);
      foreach (T obj in this)
      {
        if (condition(obj))
          all.Add(obj);
      }
      return all;
    }

    public int IndexOf(Predicate<T> condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      for (int index = 0; index < this._count; ++index)
      {
        if (condition(this._items[index]))
          return index;
      }
      return -1;
    }

    public int IndexOf(T item)
    {
      for (int index = 0; index < this._count; ++index)
      {
        if (RuntimeServices.EqualityOperator((object) this._items[index], (object) item))
          return index;
      }
      return -1;
    }

    public List<T> Insert(int index, T item)
    {
      int index1 = this.NormalizeIndex(index);
      this.EnsureCapacity(Math.Max(this._count, index1) + 1);
      if (index1 < this._count)
        Array.Copy((Array) this._items, index1, (Array) this._items, index1 + 1, this._count - index1);
      this._items[index1] = item;
      ++this._count;
      return this;
    }

    public T Pop() => this.Pop(-1);

    public T Pop(int index)
    {
      int index1 = this.CheckIndex(this.NormalizeIndex(index));
      T obj = this._items[index1];
      this.InnerRemoveAt(index1);
      return obj;
    }

    public List<T> PopRange(int begin)
    {
      int begin1 = this.AdjustIndex(this.NormalizeIndex(begin));
      List<T> range = this.InnerGetRange(begin1, this.AdjustIndex(this.NormalizeIndex(this._count)));
      for (int index = begin1; index < this._count; ++index)
        this._items[index] = default (T);
      this._count = begin1;
      return range;
    }

    public List<T> RemoveAll(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException(nameof (match));
      for (int index = 0; index < this._count; ++index)
      {
        if (match(this._items[index]))
          this.InnerRemoveAt(index--);
      }
      return this;
    }

    public List<T> Remove(T item)
    {
      this.InnerRemove(item);
      return this;
    }

    public List<T> RemoveAt(int index)
    {
      this.InnerRemoveAt(this.CheckIndex(this.NormalizeIndex(index)));
      return this;
    }

    private void EnsureCapacity(int minCapacity)
    {
      if (minCapacity <= this._items.Length)
        return;
      T[] destinationArray = this.NewArray(minCapacity);
      Array.Copy((Array) this._items, 0, (Array) destinationArray, 0, this._count);
      this._items = destinationArray;
    }

    private T[] NewArray(int minCapacity) => new T[Math.Max(Math.Max(1, this._items.Length) * 2, minCapacity)];

    private void InnerRemoveAt(int index)
    {
      --this._count;
      this._items[index] = default (T);
      if (index == this._count)
        return;
      Array.Copy((Array) this._items, index + 1, (Array) this._items, index, this._count - index);
    }

    private bool InnerRemove(T item)
    {
      int index = this.IndexOf(item);
      if (index == -1)
        return false;
      this.InnerRemoveAt(index);
      return true;
    }

    private void InnerCollect(List<T> target, Predicate<T> condition)
    {
      for (int index = 0; index < this._count; ++index)
      {
        T obj = this._items[index];
        if (condition(obj))
          target.Add(obj);
      }
    }

    private List<T> InnerGetRange(int begin, int end)
    {
      int length = end - begin;
      if (length <= 0)
        return this.NewConcreteList(new T[0], true);
      T[] objArray = new T[length];
      Array.Copy((Array) this._items, begin, (Array) objArray, 0, length);
      return this.NewConcreteList(objArray, true);
    }

    private int AdjustIndex(int index)
    {
      if (index > this._count)
        return this._count;
      return index < 0 ? 0 : index;
    }

    private int CheckIndex(int index) => index < this._count ? index : throw new IndexOutOfRangeException();

    private int NormalizeIndex(int index) => index < 0 ? index + this._count : index;

    private static T Coerce(object value) => value is T obj ? obj : (T) RuntimeServices.Coerce(value, typeof (T));

    public static List<T> operator *(List<T> lhs, int count) => lhs.Multiply(count);

    public static List<T> operator *(int count, List<T> rhs) => rhs.Multiply(count);

    public static List<T> operator +(List<T> lhs, IEnumerable rhs)
    {
      List<T> list = lhs.NewConcreteList(lhs.ToArray(), true);
      list.Extend(rhs);
      return list;
    }

    private sealed class ComparisonComparer : IComparer<T>
    {
      private readonly Comparison<T> _comparison;

      public ComparisonComparer(Comparison<T> comparison) => this._comparison = comparison;

      public int Compare(T x, T y) => this._comparison(x, y);
    }
  }
}
