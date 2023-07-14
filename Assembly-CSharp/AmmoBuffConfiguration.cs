// Decompiled with JetBrains decompiler
// Type: AmmoBuffConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class AmmoBuffConfiguration : QuickItemConfiguration
{
  private const int MaxAmmo = 200;
  private const int StartAmmo = 100;
  [CustomProperty("AmmoIncrease")]
  public IncreaseStyle AmmoIncrease;
  public int IncreaseFrequency;
  public int IncreaseTimes;
  [CustomProperty("AmmoPoints")]
  public int PointsGain;
  [CustomProperty("RobotDestruction")]
  public int RobotLifeTimeMilliSeconds;
  [CustomProperty("ScrapsDestruction")]
  public int ScrapsLifeTimeMilliSeconds;

  public bool IsNeedCharge => this.WarmUpTime > 0;

  public bool IsOverTime => this.IncreaseTimes > 0;

  public bool IsInstant => !this.IsNeedCharge && !this.IsOverTime;

  public string GetAmmoBonusDescription()
  {
    int num = this.IncreaseTimes != 0 ? this.IncreaseTimes : 1;
    switch (this.AmmoIncrease)
    {
      case IncreaseStyle.Absolute:
        return (num * this.PointsGain).ToString();
      case IncreaseStyle.PercentFromStart:
        return string.Format("{0}% of the start ammo", (object) this.PointsGain);
      case IncreaseStyle.PercentFromMax:
        return string.Format("{0}% of the max ammo", (object) this.PointsGain);
      default:
        return "n/a";
    }
  }
}
