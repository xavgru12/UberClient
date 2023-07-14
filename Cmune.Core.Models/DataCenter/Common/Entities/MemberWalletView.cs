// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberWalletView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MemberWalletView
  {
    public int Cmid { get; set; }

    public int Credits { get; set; }

    public int Points { get; set; }

    public DateTime CreditsExpiration { get; set; }

    public DateTime PointsExpiration { get; set; }

    public MemberWalletView()
    {
      this.CreditsExpiration = DateTime.Today;
      this.PointsExpiration = DateTime.Today;
    }

    public MemberWalletView(
      int cmid,
      int? credits,
      int? points,
      DateTime? creditsExpiration,
      DateTime? pointsExpiration)
    {
      if (!credits.HasValue)
        credits = new int?(0);
      if (!points.HasValue)
        points = new int?(0);
      if (!creditsExpiration.HasValue)
        creditsExpiration = new DateTime?(DateTime.MinValue);
      if (!pointsExpiration.HasValue)
        pointsExpiration = new DateTime?(DateTime.MinValue);
      this.SetMemberWallet(cmid, credits.Value, points.Value, creditsExpiration.Value, pointsExpiration.Value);
    }

    public MemberWalletView(
      int cmid,
      int credits,
      int points,
      DateTime creditsExpiration,
      DateTime pointsExpiration)
    {
      this.SetMemberWallet(cmid, credits, points, creditsExpiration, pointsExpiration);
    }

    private void SetMemberWallet(
      int cmid,
      int credits,
      int points,
      DateTime creditsExpiration,
      DateTime pointsExpiration)
    {
      this.Cmid = cmid;
      this.Credits = credits;
      this.Points = points;
      this.CreditsExpiration = creditsExpiration;
      this.PointsExpiration = pointsExpiration;
    }

    public override string ToString() => "[Wallet: " + "[CMID:" + (object) this.Cmid + "][Credits:" + (object) this.Credits + "][Credits Expiration:" + (object) this.CreditsExpiration + "][Points:" + (object) this.Points + "][Points Expiration:" + (object) this.PointsExpiration + "]" + "]";
  }
}
