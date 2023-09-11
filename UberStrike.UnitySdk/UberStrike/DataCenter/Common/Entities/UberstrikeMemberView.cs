// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeMemberView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
