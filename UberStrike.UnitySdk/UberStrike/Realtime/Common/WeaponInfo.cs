// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.WeaponInfo
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using System.Collections.Generic;
using UberStrike.Core.Types;

namespace UberStrike.Realtime.Common
{
  public class WeaponInfo : IByteArray
  {
    private List<int> _itemIDs = new List<int>(5);
    private List<byte> _categories = new List<byte>(5);

    public WeaponInfo()
    {
      this._itemIDs.AddRange((IEnumerable<int>) new int[5]);
      this._categories.AddRange((IEnumerable<byte>) new byte[5]);
    }

    public WeaponInfo(byte[] bytes, ref int index) => index = this.FromBytes(bytes, index);

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(25);
      DefaultByteConverter.FromIntCollection((ICollection<int>) this._itemIDs, ref bytes);
      DefaultByteConverter.FromByteCollection((ICollection<byte>) this._categories, ref bytes);
      return bytes.ToArray();
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this._itemIDs = DefaultByteConverter.ToIntCollection(bytes, ref idx);
      this._categories = DefaultByteConverter.ToByteCollection(bytes, ref idx);
      return idx;
    }

    public void ResetWeaponSlot(WeaponInfo.SlotType slot)
    {
      this.ItemIDs[(int) slot] = 0;
      this.Categories[(int) slot] = (byte) 0;
    }

    public void SetWeaponSlot(WeaponInfo.SlotType slot, int itemId, UberstrikeItemClass type)
    {
      this.ItemIDs[(int) slot] = itemId;
      this.Categories[(int) slot] = (byte) type;
    }

    public string ItemIDsToString() => string.Format("{0}|{1}|{2}|{3}|{4}", (object) this._itemIDs[0], (object) this._itemIDs[1], (object) this._itemIDs[2], (object) this._itemIDs[3], (object) this._itemIDs[4]);

    public string CategoriesToString() => string.Format("{0}|{1}|{2}|{3}|{4}", (object) this._categories[0], (object) this._categories[1], (object) this._categories[2], (object) this._categories[3], (object) this._categories[4]);

    public override string ToString() => this.ItemIDsToString();

    public override int GetHashCode() => this._itemIDs[0] ^ this._itemIDs[1] ^ this._itemIDs[2] ^ this._itemIDs[3] ^ this._itemIDs[4];

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && this.GetHashCode() == obj.GetHashCode();

    public List<int> ItemIDs => this._itemIDs;

    public List<byte> Categories => this._categories;

    public enum SlotType
    {
      Melee,
      Primary,
      Secondary,
      Tertiary,
      Pickup,
    }
  }
}
