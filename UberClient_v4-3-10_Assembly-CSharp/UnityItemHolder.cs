// Decompiled with JetBrains decompiler
// Type: UnityItemHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityItemHolder : AutoMonoBehaviour<UnityItemHolder>
{
  [SerializeField]
  private List<UnityEngine.Object> _items = new List<UnityEngine.Object>();
  public readonly Dictionary<string, IUnityItem> Prefabs = new Dictionary<string, IUnityItem>();

  public void Add(IUnityItem item, string name)
  {
    try
    {
      this.Prefabs.Add(name, item);
      this._items.Add((UnityEngine.Object) item.Prefab);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("UnityItemHolder.Add failed for item '" + name + "' with message:" + ex.Message));
    }
  }

  public IUnityItem CopyQuickItem(QuickItem item)
  {
    item = UnityEngine.Object.Instantiate((UnityEngine.Object) item) as QuickItem;
    item.transform.parent = this.transform;
    item.gameObject.SetActive(false);
    return (IUnityItem) item;
  }
}
