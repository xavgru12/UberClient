// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.DamageEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class DamageEvent
  {
    public Dictionary<byte, byte> Damage { get; set; }

    public byte BodyPartFlag { get; set; }

    public int DamageEffectFlag { get; set; }

    public float DamgeEffectValue { get; set; }

    public int Count => this.Damage == null ? 0 : this.Damage.Count;

    public void Clear()
    {
      if (this.Damage == null)
        this.Damage = new Dictionary<byte, byte>();
      this.BodyPartFlag = (byte) 0;
      this.Damage.Clear();
    }

    public void AddDamage(
      byte angle,
      short damage,
      byte bodyPart,
      int damageEffectFlag,
      float damageEffectValue)
    {
      if (this.Damage == null)
        this.Damage = new Dictionary<byte, byte>();
      if (this.Damage.ContainsKey(angle))
      {
        Dictionary<byte, byte> damage1;
        byte key;
        byte num = (damage1 = this.Damage)[key = angle];
        damage1[key] = (byte) ((uint) num + (uint) (byte) damage);
      }
      else
        this.Damage[angle] = (byte) damage;
      this.BodyPartFlag |= bodyPart;
      this.DamageEffectFlag = damageEffectFlag;
      this.DamgeEffectValue = damageEffectValue;
    }
  }
}
