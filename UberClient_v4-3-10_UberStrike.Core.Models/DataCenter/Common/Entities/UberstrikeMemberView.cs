// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeMemberView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
