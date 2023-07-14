// Decompiled with JetBrains decompiler
// Type: WeaponSelectorHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectorHud
{
  private Dictionary<LoadoutSlotType, WeaponItem> _loadoutWeapons;
  private Dictionary<LoadoutSlotType, int> _weaponIndicesInList;
  private MeshGUIList _weaponList;
  private float _weaponListHideTime;
  private float _weaponListDisplayTime;
  private bool _isWeaponListFadingOut;

  public WeaponSelectorHud()
  {
    this._loadoutWeapons = new Dictionary<LoadoutSlotType, WeaponItem>();
    this._weaponIndicesInList = new Dictionary<LoadoutSlotType, int>();
    this._weaponListDisplayTime = 3f;
    if (HudAssets.Exists)
    {
      this._weaponList = new MeshGUIList(new Action(this.OnDrawWeaponList));
      this._weaponList.Enabled = false;
    }
    this._weaponList.Enabled = false;
  }

  public bool Enabled
  {
    get => this._weaponList.Enabled;
    set => this._weaponList.Enabled = value;
  }

  public void Draw() => this._weaponList.Draw();

  public void Update() => this._weaponList.Update();

  public void SetSlotWeapon(LoadoutSlotType slot, WeaponItem weapon)
  {
    if ((UnityEngine.Object) weapon != (UnityEngine.Object) null)
      this._loadoutWeapons[slot] = weapon;
    else
      this._loadoutWeapons.Remove(slot);
    this.OnWeaponSlotsChange();
  }

  public WeaponItem GetLoadoutWeapon(LoadoutSlotType loadoutSlotType) => this._loadoutWeapons.ContainsKey(loadoutSlotType) ? this._loadoutWeapons[loadoutSlotType] : (WeaponItem) null;

  public void SetActiveWeaponLoadout(LoadoutSlotType loadoutSlotType)
  {
    if (!this._loadoutWeapons.ContainsKey(loadoutSlotType))
      return;
    this._weaponList.AnimToIndex(this._weaponIndicesInList[loadoutSlotType], 0.1f);
    this.OnWeaponListTrigger();
  }

  private void OnWeaponSlotsChange()
  {
    this.ResetWeaponListItems();
    this.OnWeaponListTrigger();
  }

  private void OnWeaponListTrigger()
  {
    this._weaponListHideTime = Time.time + this._weaponListDisplayTime;
    this._isWeaponListFadingOut = false;
  }

  private void OnDrawWeaponList()
  {
    if (!this.CanWeaponListFadeOut())
      return;
    this.FadeOutWeaponList();
  }

  private bool CanWeaponListFadeOut() => (double) Time.time > (double) this._weaponListHideTime && !this._isWeaponListFadingOut;

  private void FadeOutWeaponList()
  {
    this._isWeaponListFadingOut = true;
    this._weaponList.FadeOut(1f, EaseType.Out);
  }

  private void ResetWeaponListItems()
  {
    int num1 = 5;
    int num2 = 0;
    this._weaponIndicesInList.Clear();
    this._weaponList.ClearAllItems();
    for (int index = 0; index < num1; ++index)
    {
      LoadoutSlotType key = (LoadoutSlotType) (7 + index);
      if (this._loadoutWeapons.ContainsKey(key))
      {
        string name = this._loadoutWeapons[key].Name;
        this._weaponIndicesInList.Add(key, num2++);
        this._weaponList.AddItem(name);
      }
    }
  }
}
