using System.IO;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine.Sprites;

namespace UberStrike.Core.Serialization
{
	public static class DailyBonusViewProxy
	{
		public static void Serialize(Stream stream, DailyBonusView instance)
		{
			Int32Proxy.Serialize(stream, instance.Streak);
			Int32Proxy.Serialize(stream, instance.RewardClass);
			StringProxy.Serialize(stream, instance.Label);
			Int32Proxy.Serialize(stream, instance.Amount);
			Int32Proxy.Serialize(stream, instance.Duration);
		}

		public static DailyBonusView Deserialize(Stream bytes)
		{
			return new DailyBonusView
			{
				Streak = Int32Proxy.Deserialize(bytes),
				RewardClass = Int32Proxy.Deserialize(bytes),
				Label = StringProxy.Deserialize(bytes),
				Amount = Int32Proxy.Deserialize(bytes),
				Duration = Int32Proxy.Deserialize(bytes)
			};
		}
	}
}
