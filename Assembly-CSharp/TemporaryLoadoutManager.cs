// Decompiled with JetBrains decompiler
// Type: TemporaryLoadoutManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TemporaryLoadoutManager : Singleton<TemporaryLoadoutManager>
{
  private Dictionary<LoadoutSlotType, int> _temporaryGearLoadout;

  private TemporaryLoadoutManager() => this._temporaryGearLoadout = new Dictionary<LoadoutSlotType, int>();

  public void SetGearLoadout(LoadoutSlotType slot, IUnityItem item)
  {
    if (item != null)
      this._temporaryGearLoadout[slot] = item.ItemId;
    else
      this._temporaryGearLoadout.Remove(slot);
    this.UpdateGearLoadout(slot == LoadoutSlotType.GearHolo);
  }

  public bool IsGearLoadoutModified { get; private set; }

  public bool IsGearLoadoutModifiedOnSlot(LoadoutSlotType slot)
  {
    int num;
    return this._temporaryGearLoadout.TryGetValue(slot, out num) && num != Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(slot);
  }

  public void ResetGearLoadout(LoadoutSlotType slot) => this._temporaryGearLoadout.Remove(slot);

  public void ResetGearLoadout()
  {
    bool flag = false;
    bool recreateAvatar = false;
    foreach (LoadoutSlotType key in this._temporaryGearLoadout.Keys)
    {
      if (this.IsGearLoadoutModifiedOnSlot(key))
      {
        if (key == LoadoutSlotType.GearHolo)
          recreateAvatar = true;
        flag = true;
      }
    }
    this._temporaryGearLoadout.Clear();
    if (!flag)
      return;
    this.UpdateGearLoadout(recreateAvatar);
  }

  private void UpdateGearLoadout(bool recreateAvatar)
  {
    GearLoadout gearLoadout = new GearLoadout(Singleton<LoadoutManager>.Instance.GearLoadout);
    foreach (LoadoutSlotType key in this._temporaryGearLoadout.Keys)
    {
      int itemId = this._temporaryGearLoadout[key];
      if (itemId > 0)
        gearLoadout.SetGear(key, itemId);
    }
    if (GameState.HasCurrentGame)
    {
      Singleton<AvatarBuilder>.Instance.UpdateLocalAvatar(gearLoadout);
    }
    else
    {
      Vector3 position = GameState.LocalDecorator.transform.position;
      Quaternion rotation = GameState.LocalDecorator.transform.rotation;
      AvatarBuilder.Destroy(GameState.LocalDecorator.gameObject);
      GameState.LocalDecorator = Singleton<AvatarBuilder>.Instance.CreateLocalAvatar(gearLoadout);
      GameState.LocalDecorator.SetPosition(position, rotation);
      AutoMonoBehaviour<AvatarAnimationManager>.Instance.ResetAnimationState(PageType.Shop);
    }
    this.IsGearLoadoutModified = false;
    foreach (KeyValuePair<LoadoutSlotType, int> keyValuePair in this._temporaryGearLoadout)
    {
      if (keyValuePair.Value != Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(keyValuePair.Key))
        this.IsGearLoadoutModified = true;
    }
    if (!((Object) GameState.LocalDecorator != (Object) null))
      return;
    GameState.LocalDecorator.HideWeapons();
  }
}
