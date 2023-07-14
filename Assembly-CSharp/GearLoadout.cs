// Decompiled with JetBrains decompiler
// Type: GearLoadout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GearLoadout
{
  private Dictionary<LoadoutSlotType, int> _gears;

  public GearLoadout()
    : this(0, 0, 0, 0, 0, 0, 0)
  {
  }

  public GearLoadout(GearLoadout gearLoadout) => this._gears = new Dictionary<LoadoutSlotType, int>((IDictionary<LoadoutSlotType, int>) gearLoadout._gears);

  public GearLoadout(List<int> gearItemIds)
  {
    this._gears = new Dictionary<LoadoutSlotType, int>();
    int gearItemId1 = gearItemIds.Count <= 6 ? 0 : gearItemIds[6];
    int gearItemId2 = gearItemIds.Count <= 5 ? 0 : gearItemIds[5];
    int gearItemId3 = gearItemIds.Count <= 4 ? 0 : gearItemIds[4];
    int gearItemId4 = gearItemIds.Count <= 3 ? 0 : gearItemIds[3];
    int gearItemId5 = gearItemIds.Count <= 2 ? 0 : gearItemIds[2];
    int gearItemId6 = gearItemIds.Count <= 1 ? 0 : gearItemIds[1];
    int gearItemId7 = gearItemIds.Count <= 0 ? 0 : gearItemIds[0];
    this._gears.Add(LoadoutSlotType.GearHolo, gearItemId1);
    this._gears.Add(LoadoutSlotType.GearHead, gearItemId2);
    this._gears.Add(LoadoutSlotType.GearFace, gearItemId3);
    this._gears.Add(LoadoutSlotType.GearGloves, gearItemId4);
    this._gears.Add(LoadoutSlotType.GearUpperBody, gearItemId5);
    this._gears.Add(LoadoutSlotType.GearLowerBody, gearItemId6);
    this._gears.Add(LoadoutSlotType.GearBoots, gearItemId7);
  }

  public GearLoadout(
    int holoItemId,
    int headItemId,
    int faceItemId,
    int glovesItemId,
    int upperbodyItemId,
    int lowerbodyItemId,
    int bootsItemId)
  {
    this._gears = new Dictionary<LoadoutSlotType, int>();
    this._gears.Add(LoadoutSlotType.GearHolo, holoItemId);
    this._gears.Add(LoadoutSlotType.GearHead, headItemId);
    this._gears.Add(LoadoutSlotType.GearFace, faceItemId);
    this._gears.Add(LoadoutSlotType.GearGloves, glovesItemId);
    this._gears.Add(LoadoutSlotType.GearUpperBody, upperbodyItemId);
    this._gears.Add(LoadoutSlotType.GearLowerBody, lowerbodyItemId);
    this._gears.Add(LoadoutSlotType.GearBoots, bootsItemId);
  }

  public int HoloItemId
  {
    get => this._gears[LoadoutSlotType.GearHolo];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearHolo] = value;
    }
  }

  public int HeadItemId
  {
    get => this._gears[LoadoutSlotType.GearHead];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearHead] = value;
    }
  }

  public int FaceItemId
  {
    get => this._gears[LoadoutSlotType.GearFace];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearFace] = value;
    }
  }

  public int GlovesItemId
  {
    get => this._gears[LoadoutSlotType.GearGloves];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearGloves] = value;
    }
  }

  public int UpperbodyItemId
  {
    get => this._gears[LoadoutSlotType.GearUpperBody];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearUpperBody] = value;
    }
  }

  public int LowerbodyItemId
  {
    get => this._gears[LoadoutSlotType.GearLowerBody];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearLowerBody] = value;
    }
  }

  public int BootsItemId
  {
    get => this._gears[LoadoutSlotType.GearBoots];
    set
    {
      if (value <= 0)
        return;
      this._gears[LoadoutSlotType.GearBoots] = value;
    }
  }

  public List<GameObject> GetGearPrefabs() => new List<GameObject>()
  {
    Singleton<ItemManager>.Instance.GetPrefab(this.HeadItemId),
    Singleton<ItemManager>.Instance.GetPrefab(this.FaceItemId),
    Singleton<ItemManager>.Instance.GetPrefab(this.GlovesItemId),
    Singleton<ItemManager>.Instance.GetPrefab(this.UpperbodyItemId),
    Singleton<ItemManager>.Instance.GetPrefab(this.LowerbodyItemId),
    Singleton<ItemManager>.Instance.GetPrefab(this.BootsItemId)
  };

  public void SetGear(LoadoutSlotType slotType, int itemId)
  {
    if (!this._gears.ContainsKey(slotType))
      return;
    this._gears[slotType] = itemId;
  }
}
