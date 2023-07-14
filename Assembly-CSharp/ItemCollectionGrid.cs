// Decompiled with JetBrains decompiler
// Type: ItemCollectionGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ItemCollectionGrid
{
  private const int GearWidth = 200;
  private string _filter = string.Empty;
  private Vector2 _scroll;
  private ItemCollection _items;
  private Dictionary<string, ItemCollectionGrid.GridItem> _objects;

  public ItemCollectionGrid(ItemCollection items)
  {
    this._items = items;
    this._objects = new Dictionary<string, ItemCollectionGrid.GridItem>(items.GetCount());
    foreach (GearItem gear in (IEnumerable<GearItem>) items.Gears)
    {
      ItemCollectionGrid.GridItem gridItem = new ItemCollectionGrid.GridItem()
      {
        Object = Object.Instantiate((Object) gear.gameObject) as GameObject
      };
      gridItem.Renderer = (Renderer) gridItem.Object.GetComponentInChildren<SkinnedMeshRenderer>();
      float num = (float) ((double) ((float) (200.0 * (double) Camera.main.orthographicSize * 2.0) / (float) Screen.width) / (double) Mathf.Max(gridItem.Renderer.bounds.size.x, gridItem.Renderer.bounds.size.y, gridItem.Renderer.bounds.size.z));
      gridItem.Object.transform.localScale = Vector3.one * num;
      gridItem.YOffset = gridItem.Renderer.bounds.center.y;
      this._objects[gear.name] = gridItem;
    }
    foreach (HoloGearItem holo in (IEnumerable<HoloGearItem>) items.Holos)
    {
      ItemCollectionGrid.GridItem gridItem = new ItemCollectionGrid.GridItem()
      {
        Object = Object.Instantiate((Object) holo.Configuration.Avatar.gameObject) as GameObject
      };
      gridItem.Renderer = (Renderer) gridItem.Object.GetComponentInChildren<SkinnedMeshRenderer>();
      float num = (float) ((double) ((float) (200.0 * (double) Camera.main.orthographicSize * 2.0) / (float) Screen.width) / (double) Mathf.Max(gridItem.Renderer.bounds.size.x, gridItem.Renderer.bounds.size.y, gridItem.Renderer.bounds.size.z));
      gridItem.Object.transform.localScale = Vector3.one * num;
      gridItem.YOffset = gridItem.Renderer.bounds.center.y;
      this._objects[holo.name] = gridItem;
    }
    foreach (WeaponItem weapon in (IEnumerable<WeaponItem>) items.Weapons)
    {
      ItemCollectionGrid.GridItem gridItem = new ItemCollectionGrid.GridItem();
      Bounds bounds = new Bounds();
      gridItem.Object = Object.Instantiate((Object) weapon.gameObject) as GameObject;
      foreach (Renderer componentsInChild in gridItem.Object.GetComponentsInChildren<Renderer>())
        bounds.Encapsulate(componentsInChild.bounds);
      float num = (float) ((double) ((float) (200.0 * (double) Camera.main.orthographicSize * 2.0) / (float) Screen.width) / (double) Mathf.Clamp(Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z), 0.1f, 1f));
      gridItem.Object.transform.localRotation = Quaternion.LookRotation(Vector3.left);
      gridItem.Object.transform.localScale = Vector3.one * num;
      this._objects[weapon.name] = gridItem;
    }
  }

  public void SetFilter(string filter)
  {
    if (filter.Equals(this._filter))
      return;
    this._filter = filter;
  }

  public void Draw(Rect rect)
  {
    this._scroll = GUI.BeginScrollView(rect, this._scroll, new Rect(0.0f, 0.0f, rect.width, (float) (200 * this._objects.Count)));
    int num = 0;
    foreach (GearItem gear in (IEnumerable<GearItem>) this._items.Gears)
    {
      if (string.IsNullOrEmpty(this._filter) || gear.name.ToLower().Contains(this._filter.ToLower()))
      {
        GUI.Label(new Rect(0.0f, (float) (num * 200), 48f, 48f), (Texture) gear.Icon);
        GUI.Label(new Rect(48f, (float) (num * 200), 152f, 20f), gear.name);
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint((Vector2) (new Vector3(0.0f, (float) (num * 200)) + Vector3.one * 200f * 0.5f));
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, (float) Screen.height - screenPoint.y, 0.0f));
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(gear.name, out gridItem))
        {
          gridItem.Object.SetActiveRecursively(true);
          gridItem.Object.transform.position = worldPoint + new Vector3(0.0f, -gridItem.YOffset, -1f);
        }
        ++num;
      }
      else
      {
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(gear.name, out gridItem))
          gridItem.Object.SetActiveRecursively(false);
      }
    }
    foreach (HoloGearItem holo in (IEnumerable<HoloGearItem>) this._items.Holos)
    {
      if (string.IsNullOrEmpty(this._filter) || holo.name.ToLower().Contains(this._filter.ToLower()))
      {
        GUI.Label(new Rect(0.0f, (float) (num * 200), 48f, 48f), (Texture) holo.Icon);
        GUI.Label(new Rect(48f, (float) (num * 200), 152f, 20f), holo.name);
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint((Vector2) (new Vector3(0.0f, (float) (num * 200)) + Vector3.one * 200f * 0.5f));
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, (float) Screen.height - screenPoint.y, 0.0f));
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(holo.name, out gridItem))
        {
          gridItem.Object.SetActiveRecursively(true);
          gridItem.Object.transform.position = worldPoint + new Vector3(0.0f, -gridItem.YOffset, -1f);
        }
        ++num;
      }
      else
      {
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(holo.name, out gridItem))
          gridItem.Object.SetActiveRecursively(false);
      }
    }
    foreach (WeaponItem weapon in (IEnumerable<WeaponItem>) this._items.Weapons)
    {
      if (string.IsNullOrEmpty(this._filter) || weapon.name.ToLower().Contains(this._filter.ToLower()))
      {
        GUI.Label(new Rect(0.0f, (float) (num * 200), 48f, 48f), (Texture) weapon.Icon);
        GUI.Label(new Rect(48f, (float) (num * 200), 152f, 20f), weapon.name);
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint((Vector2) (new Vector3(0.0f, (float) (num * 200)) + Vector3.one * 200f * 0.5f));
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, (float) Screen.height - screenPoint.y, 0.0f));
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(weapon.name, out gridItem))
        {
          gridItem.Object.SetActiveRecursively(true);
          gridItem.Object.transform.position = worldPoint + new Vector3(0.0f, -gridItem.YOffset, -1f);
        }
        ++num;
      }
      else
      {
        ItemCollectionGrid.GridItem gridItem;
        if (this._objects.TryGetValue(weapon.name, out gridItem))
          gridItem.Object.SetActiveRecursively(false);
      }
    }
    GUI.EndGroup();
  }

  public void Dispose()
  {
    List<ItemCollectionGrid.GridItem> gridItemList = new List<ItemCollectionGrid.GridItem>((IEnumerable<ItemCollectionGrid.GridItem>) this._objects.Values);
    this._objects.Clear();
    foreach (ItemCollectionGrid.GridItem gridItem in gridItemList)
      Object.Destroy((Object) gridItem.Object);
  }

  private class GridItem
  {
    public GameObject Object { get; set; }

    public Renderer Renderer { get; set; }

    public float YOffset { get; set; }
  }
}
