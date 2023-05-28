// Decompiled with JetBrains decompiler
// Type: HealthBuffConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class HealthBuffConfiguration : QuickItemConfiguration
{
  private const int MaxHealth = 200;
  private const int StartHealth = 100;
  [CustomProperty("IncreaseStyle")]
  public IncreaseStyle HealthIncrease;
  [CustomProperty("Frequency")]
  public int IncreaseFrequency;
  [CustomProperty("Times")]
  public int IncreaseTimes;
  [CustomProperty("HealthPoints")]
  public int PointsGain;
  [CustomProperty("RobotDestruction")]
  public int RobotLifeTimeMilliSeconds;
  [CustomProperty("ScrapsDestruction")]
  public int ScrapsLifeTimeMilliSeconds;

  public bool IsHealNeedCharge => this.WarmUpTime > 0;

  public bool IsHealOverTime => this.IncreaseTimes > 0;

  public bool IsHealInstant => !this.IsHealNeedCharge && !this.IsHealOverTime;

  public string GetHealthBonusDescription()
  {
    int num = this.IncreaseTimes != 0 ? this.IncreaseTimes : 1;
    switch (this.HealthIncrease)
    {
      case IncreaseStyle.Absolute:
        return (num * this.PointsGain).ToString() + "HP";
      case IncreaseStyle.PercentFromStart:
        return Mathf.RoundToInt((float) (100 * num * this.PointsGain) / 100f).ToString() + "HP";
      case IncreaseStyle.PercentFromMax:
        return Mathf.RoundToInt((float) (200 * num * this.PointsGain) / 100f).ToString() + "HP";
      default:
        return "n/a";
    }
  }
}
