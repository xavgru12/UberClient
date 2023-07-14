// Decompiled with JetBrains decompiler
// Type: SelectionGroup`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGroup<T> where T : IComparable
{
  private List<SelectionGroup<T>.Pair> _data = new List<SelectionGroup<T>.Pair>();

  public SelectionGroup() => this.GuiContent = new GUIContent[0];

  public event Action<T> OnSelectionChange;

  public int Index { get; private set; }

  public T Current { get; private set; }

  public int Length => this._data.Count;

  public GUIContent[] GuiContent { get; private set; }

  public T[] Items { get; private set; }

  public void SetIndex(int index)
  {
    this.Index = index;
    this.Current = index < 0 || index >= this._data.Count ? default (T) : this._data[index].Item;
    if (this.OnSelectionChange == null)
      return;
    this.OnSelectionChange(this.Current);
  }

  public void Select(T item)
  {
    this.Index = this._data.FindIndex((Predicate<SelectionGroup<T>.Pair>) (i => i.Item.CompareTo((object) item) == 0));
    this.Current = item;
    if (this.OnSelectionChange == null)
      return;
    this.OnSelectionChange(this.Current);
  }

  public void Add(T item, GUIContent content)
  {
    this._data.Add(new SelectionGroup<T>.Pair()
    {
      Item = item,
      Content = content
    });
    this.GuiContent = this._data.ConvertAll<GUIContent>((Converter<SelectionGroup<T>.Pair, GUIContent>) (p => p.Content)).ToArray();
    this.Items = this._data.ConvertAll<T>((Converter<SelectionGroup<T>.Pair, T>) (p => p.Item)).ToArray();
  }

  private class Pair
  {
    public T Item;
    public GUIContent Content;
  }
}
