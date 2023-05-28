// Decompiled with JetBrains decompiler
// Type: PopupStack`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class PopupStack<T>
{
  private List<T> items = new List<T>();

  public int Count => this.items.Count;

  public T Peek() => this.items.Count > 0 ? this.items[this.items.Count - 1] : default (T);

  public void Push(T item) => this.items.Add(item);

  public T Pop()
  {
    if (this.items.Count <= 0)
      return default (T);
    T obj = this.items[this.items.Count - 1];
    this.items.Remove(obj);
    return obj;
  }

  public void Remove(int itemAtPosition) => this.items.RemoveAt(itemAtPosition);

  public void Remove(T item) => this.items.Remove(item);

  public void Clear() => this.items.Clear();
}
