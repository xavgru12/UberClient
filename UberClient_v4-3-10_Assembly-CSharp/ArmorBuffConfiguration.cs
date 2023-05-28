// Decompiled with JetBrains decompiler
// Type: ArmorBuffConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class ArmorBuffConfiguration : QuickItemConfiguration
{
  private const int MaxArmor = 200;
  private const int StartArmor = 100;
  public IncreaseStyle ArmorIncrease;
  public int IncreaseFrequency;
  public int IncreaseTimes;
  [CustomProperty("ArmorPoints")]
  public int PointsGain;
  [CustomProperty("RobotDestruction")]
  public int RobotLifeTimeMilliSeconds;
  [CustomProperty("ScrapsDestruction")]
  public int ScrapsLifeTimeMilliSeconds;

  public bool IsNeedCharge => this.WarmUpTime > 0;

  public bool IsOverTime => this.IncreaseTimes > 0;

  public bool IsInstant => !this.IsNeedCharge && !this.IsOverTime;

  public string GetArmorBonusDescription()
  {
    int num = this.IncreaseTimes != 0 ? this.IncreaseTimes : 1;
    switch (this.ArmorIncrease)
    {
      case IncreaseStyle.Absolute:
        return (num * this.PointsGain).ToString();
      case IncreaseStyle.PercentFromStart:
        return Mathf.RoundToInt((float) (100 * num * this.PointsGain) / 100f).ToString() + "AP";
      case IncreaseStyle.PercentFromMax:
        return Mathf.RoundToInt((float) (200 * num * this.PointsGain) / 100f).ToString() + "AP";
      default:
        return "n/a";
    }
  }
}
