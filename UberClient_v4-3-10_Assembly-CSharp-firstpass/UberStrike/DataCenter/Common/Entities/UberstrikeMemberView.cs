// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeMemberView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
