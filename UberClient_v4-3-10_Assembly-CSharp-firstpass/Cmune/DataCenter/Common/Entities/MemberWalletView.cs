// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberWalletView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[Wallet: " + "[CMID:" + this.Cmid.ToString() + "][Credits:" + this.Credits.ToString() + "][Credits Expiration:" + this.CreditsExpiration.ToString() + "][Points:" + this.Points.ToString() + "][Points Expiration:" + this.PointsExpiration.ToString() + "]" + "]";
  }
}
