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

	public bool IsHealNeedCharge => base.WarmUpTime > 0;

	public bool IsHealOverTime => IncreaseTimes > 0;

	public bool IsHealInstant
	{
		get
		{
			if (!IsHealNeedCharge)
			{
				return !IsHealOverTime;
			}
			return false;
		}
	}

	public string GetHealthBonusDescription()
	{
		int num = (IncreaseTimes == 0) ? 1 : IncreaseTimes;
		switch (HealthIncrease)
		{
		case IncreaseStyle.Absolute:
			return (num * PointsGain).ToString() + "HP";
		case IncreaseStyle.PercentFromMax:
			return Mathf.RoundToInt((float)(200 * num * PointsGain) / 100f).ToString() + "HP";
		case IncreaseStyle.PercentFromStart:
			return Mathf.RoundToInt((float)(100 * num * PointsGain) / 100f).ToString() + "HP";
		default:
			return "n/a";
		}
	}
}
