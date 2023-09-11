// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.ArmorInfo
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public class ArmorInfo : IByteArray
  {
    private const float _naturalDefense = 0.5f;
    private byte _bonusDefense = 0;
    private byte[] _bytes;
    private byte _armorPointCapacity = 0;
    private byte _armorPoints = 0;
    private float _totalDefense;

    public ArmorInfo() => this._bytes = new byte[3];

    public ArmorInfo(byte[] bytes, ref int index)
      : this()
    {
      index = this.FromBytes(bytes, index);
    }

    public short AbsorbDamage(short damage, BodyPart part)
    {
      if (!this.HasArmorPoints)
        return damage;
      int num = Mathf.Clamp(Mathf.CeilToInt(this._totalDefense * (float) damage), 0, (int) this._armorPoints);
      this._armorPoints -= (byte) num;
      return (short) ((int) damage - num);
    }

    public void SimulateAbsorbDamage(
      short damage,
      BodyPart part,
      out short finalDamage,
      out byte finalArmorPoints)
    {
      if (this.HasArmorPoints)
      {
        int num = Mathf.Clamp(Mathf.CeilToInt(this._totalDefense * (float) damage), 0, (int) this._armorPoints);
        finalArmorPoints = (byte) ((uint) this._armorPoints - (uint) num);
        finalDamage = (short) ((int) damage - num);
      }
      else
      {
        finalArmorPoints = this._armorPoints;
        finalDamage = damage;
      }
    }

    public bool HasArmorPoints => this._armorPoints > (byte) 0;

    public bool HasArmor => this._bonusDefense > (byte) 0;

    public void Reset() => this.ArmorPoints = this.ArmorPointCapacity;

    public byte[] GetBytes()
    {
      this._bytes[0] = this._armorPoints;
      this._bytes[1] = this._armorPointCapacity;
      this._bytes[2] = this._bonusDefense;
      return this._bytes;
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this._armorPoints = bytes[idx++];
      this._armorPointCapacity = bytes[idx++];
      this._bonusDefense = bytes[idx++];
      this.AbsorbtionPercentage = this._bonusDefense;
      return idx;
    }

    public override int GetHashCode() => (int) this._armorPoints ^ (int) this._armorPointCapacity ^ (int) this._bonusDefense;

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && this.GetHashCode() == obj.GetHashCode();

    public override string ToString() => string.Format("{0}/{1} @ {2}%", (object) this._armorPoints, (object) this._armorPointCapacity, (object) this._bonusDefense);

    public int ArmorPoints
    {
      get => (int) this._armorPoints;
      set => this._armorPoints = (byte) Mathf.Clamp(value, 0, 200);
    }

    public int ArmorPointCapacity
    {
      get => (int) this._armorPointCapacity;
      set => this._armorPointCapacity = (byte) Mathf.Clamp(value, 0, 200);
    }

    public byte AbsorbtionPercentage
    {
      get => this._bonusDefense;
      set
      {
        this._bonusDefense = (byte) Mathf.Clamp((int) value, 0, 50);
        this._totalDefense = (float) (0.5 + (double) this._bonusDefense / 100.0);
      }
    }
  }
}
