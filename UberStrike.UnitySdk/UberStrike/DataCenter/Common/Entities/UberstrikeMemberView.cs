
using System;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class UberstrikeMemberView
  {
    public PlayerCardView PlayerCardView { get; set; }

    public PlayerStatisticsView PlayerStatisticsView { get; set; }

    public UberstrikeMemberView()
    {
    }

    public UberstrikeMemberView(
      PlayerCardView playerCardView,
      PlayerStatisticsView playerStatisticsView)
    {
      this.PlayerCardView = playerCardView;
      this.PlayerStatisticsView = playerStatisticsView;
    }

    public override string ToString()
    {
      string str1 = "[Uberstrike member view: ";
      string str2 = this.PlayerCardView == null ? str1 + "null" : str1 + this.PlayerCardView.ToString();
      return (this.PlayerStatisticsView == null ? str2 + "null" : str2 + this.PlayerStatisticsView.ToString()) + "]";
    }
  }
}
