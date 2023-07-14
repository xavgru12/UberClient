// Decompiled with JetBrains decompiler
// Type: ItemCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ItemCollection
{
  private Dictionary<string, GearItem> _gears;
  private Dictionary<string, HoloGearItem> _holos;
  private Dictionary<string, WeaponItem> _weapons;
  private Dictionary<string, QuickItem> _quickItems;

  public ItemCollection()
  {
    this._gears = new Dictionary<string, GearItem>();
    this._holos = new Dictionary<string, HoloGearItem>();
    this._weapons = new Dictionary<string, WeaponItem>();
    this._quickItems = new Dictionary<string, QuickItem>();
  }

  public ICollection<GearItem> Gears => (ICollection<GearItem>) this._gears.Values;

  public ICollection<HoloGearItem> Holos => (ICollection<HoloGearItem>) this._holos.Values;

  public ICollection<WeaponItem> Weapons => (ICollection<WeaponItem>) this._weapons.Values;

  public ICollection<QuickItem> QuickItems => (ICollection<QuickItem>) this._quickItems.Values;

  public void AddItem(GameObject item)
  {
    GearItem component1;
    if ((bool) (Object) (component1 = item.GetComponent<GearItem>()))
    {
      this.AddGear(component1);
    }
    else
    {
      HoloGearItem component2;
      if ((bool) (Object) (component2 = item.GetComponent<HoloGearItem>()))
      {
        this.AddHolo(component2);
      }
      else
      {
        WeaponItem component3;
        if ((bool) (Object) (component3 = item.GetComponent<WeaponItem>()))
        {
          this.AddWeapon(component3);
        }
        else
        {
          QuickItem component4;
          if (!(bool) (Object) (component4 = item.GetComponent<QuickItem>()))
            return;
          this.AddQuickItem(component4);
        }
      }
    }
  }

  public int GetCount() => this._gears.Count + this._holos.Count + this._weapons.Count;

  private void AddGear(GearItem gear)
  {
    if (this._gears.ContainsKey(gear.name))
      Debug.LogError((object) ("Duplicated gear: " + gear.name));
    else
      this._gears.Add(gear.name, gear);
  }

  private void AddHolo(HoloGearItem holo)
  {
    if (this._holos.ContainsKey(holo.name))
      Debug.LogError((object) ("Duplicated holo: " + holo.name));
    else
      this._holos.Add(holo.name, holo);
  }

  private void AddWeapon(WeaponItem weapon)
  {
    if (this._weapons.ContainsKey(weapon.name))
      Debug.LogError((object) ("Duplicated weapon: " + weapon.name));
    else
      this._weapons.Add(weapon.name, weapon);
  }

  private void AddQuickItem(QuickItem quickItem)
  {
    if (this._quickItems.ContainsKey(quickItem.name))
      Debug.LogError((object) ("Duplicated QuickItem: " + quickItem.name));
    else
      this._quickItems.Add(quickItem.name, quickItem);
  }
}
