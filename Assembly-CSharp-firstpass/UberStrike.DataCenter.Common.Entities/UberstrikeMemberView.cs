using System;

namespace UberStrike.DataCenter.Common.Entities
{
	[Serializable]
	public class UberstrikeMemberView
	{
		public PlayerCardView PlayerCardView
		{
			get;
			set;
		}

		public PlayerStatisticsView PlayerStatisticsView
		{
			get;
			set;
		}

		public UberstrikeMemberView()
		{
		}

		public UberstrikeMemberView(PlayerCardView playerCardView, PlayerStatisticsView playerStatisticsView)
		{
			PlayerCardView = playerCardView;
			PlayerStatisticsView = playerStatisticsView;
		}

		public override string ToString()
		{
			string str = "[Uberstrike member view: ";
			str = ((PlayerCardView == null) ? (str + "null") : (str + PlayerCardView.ToString()));
			str = ((PlayerStatisticsView == null) ? (str + "null") : (str + PlayerStatisticsView.ToString()));
			return str + "]";
		}
	}
}
