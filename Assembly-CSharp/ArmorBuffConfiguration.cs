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

	public string GetArmorBonusDescription()
	{
		int num = (IncreaseTimes == 0) ? 1 : IncreaseTimes;
		switch (ArmorIncrease)
		{
		case IncreaseStyle.Absolute:
			return (num * PointsGain).ToString();
		case IncreaseStyle.PercentFromMax:
			return Mathf.RoundToInt((float)(200 * num * PointsGain) / 100f).ToString() + "AP";
		case IncreaseStyle.PercentFromStart:
			return Mathf.RoundToInt((float)(100 * num * PointsGain) / 100f).ToString() + "AP";
		default:
			return "n/a";
		}
	}
}
