// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.DamageEvent
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using Cmune.Util;
using System.Collections.Generic;

namespace UberStrike.Realtime.Common
{
  public class DamageEvent : IByteArray
  {
    public readonly Dictionary<byte, byte> Damage;
    private byte _bodyPartFlag;
    private int _damageEffectFlag;
    private float _damageEffectValue;

    public DamageEvent() => this.Damage = new Dictionary<byte, byte>(1);

    public static DamageEvent FromBytes(byte[] bytes, ref int idx)
    {
      DamageEvent damageEvent = new DamageEvent();
      idx = damageEvent.FromBytes(bytes, idx);
      return damageEvent;
    }

    public void Clear()
    {
      this._bodyPartFlag = (byte) 0;
      this.Damage.Clear();
    }

    public void AddDamage(
      byte angle,
      short damage,
      byte bodyPart,
      int damageEffectFlag,
      float damageEffectValue)
    {
      if (this.Damage.ContainsKey(angle))
      {
        Dictionary<byte, byte> damage1;
        byte key;
        (damage1 = this.Damage)[key = angle] = (byte) ((uint) damage1[key] + (uint) (byte) damage);
      }
      else
        this.Damage[angle] = (byte) damage;
      this._bodyPartFlag |= bodyPart;
      this._damageEffectFlag = damageEffectFlag;
      this._damageEffectValue = damageEffectValue;
    }

    public byte BodyPartFlag => this._bodyPartFlag;

    public int Count => this.Damage.Count;

    public int DamageEffectFlag => this._damageEffectFlag;

    public float DamgeEffectValue => this._damageEffectValue;

    public override int GetHashCode() => (int) this._bodyPartFlag ^ this.Damage.Count;

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && obj is DamageEvent damageEvent && (int) damageEvent._bodyPartFlag == (int) this._bodyPartFlag && Comparison.IsEqual((object) damageEvent.Damage.Keys, (object) this.Damage.Keys) && Comparison.IsEqual((object) damageEvent.Damage.Values, (object) this.Damage.Values);

    public int FromBytes(byte[] bytes, int idx)
    {
      if (idx + 10 > bytes.Length)
        return int.MaxValue;
      this._damageEffectFlag = DefaultByteConverter.ToInt(bytes, ref idx);
      this._damageEffectValue = DefaultByteConverter.ToFloat(bytes, ref idx);
      this._bodyPartFlag = bytes[idx++];
      int num = (int) bytes[idx++];
      if (num > 0 && idx + 2 * num <= bytes.Length)
      {
        for (int index = 0; index < num; ++index)
          this.Damage[bytes[idx + index]] = bytes[idx + num + index];
      }
      return idx + 2 * num;
    }

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(this.Damage.Count * 2 + 2);
      DefaultByteConverter.FromInt(this._damageEffectFlag, ref bytes);
      DefaultByteConverter.FromFloat(this._damageEffectValue, ref bytes);
      bytes.Add(this._bodyPartFlag);
      bytes.Add((byte) this.Damage.Keys.Count);
      bytes.AddRange((IEnumerable<byte>) this.Damage.Keys);
      bytes.AddRange((IEnumerable<byte>) this.Damage.Values);
      return bytes.ToArray();
    }
  }
}
