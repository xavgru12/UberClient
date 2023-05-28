// Decompiled with JetBrains decompiler
// Type: FastSecureInteger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class FastSecureInteger
{
  private const float syncFrequency = 1f;
  private SecureMemory<int> secureValue;
  private int deltaValue;
  private float nextUpdate;

  public FastSecureInteger(int value)
  {
    this.secureValue = new SecureMemory<int>(0, useAOTCompatibleMode: ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone);
    this.secureValue.WriteData(value);
    this.deltaValue = 0;
  }

  public void Decrement(int value) => this.deltaValue -= value;

  public void Increment(int value) => this.deltaValue += value;

  public int Value
  {
    get
    {
      if (this.deltaValue != 0 && (double) this.nextUpdate < (double) Time.time)
        this.Value = this.secureValue.ReadData(true) + this.deltaValue;
      return this.secureValue.ReadData(false) + this.deltaValue;
    }
    set
    {
      this.secureValue.WriteData(value);
      this.deltaValue = 0;
      this.nextUpdate = Time.time + 1f;
    }
  }

  public override string ToString() => this.Value.ToString();
}
