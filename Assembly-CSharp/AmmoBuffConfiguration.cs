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

	public bool IsNeedCharge => base.WarmUpTime > 0;

	public bool IsOverTime => IncreaseTimes > 0;

	public bool IsInstant
	{
		get
		{
			if (!IsNeedCharge)
			{
				return !IsOverTime;
			}
			return false;
		}
	}

	public string GetAmmoBonusDescription()
	{
		int num = (IncreaseTimes == 0) ? 1 : IncreaseTimes;
		switch (AmmoIncrease)
		{
		case IncreaseStyle.Absolute:
			return (num * PointsGain).ToString();
		case IncreaseStyle.PercentFromMax:
			return $"{PointsGain}% of the max ammo";
		case IncreaseStyle.PercentFromStart:
			return $"{PointsGain}% of the start ammo";
		default:
			return "n/a";
		}
	}
}
