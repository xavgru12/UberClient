// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BannedIpView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  public class BannedIpView
  {
    public int BannedIpId { get; private set; }

    public long IpAddress { get; private set; }

    public DateTime? BannedUntil { get; private set; }

    public DateTime BanningDate { get; private set; }

    public int SourceCmid { get; private set; }

    public string SourceName { get; private set; }

    public int TargetCmid { get; private set; }

    public string TargetName { get; private set; }

    public string Reason { get; set; }

    public BannedIpView(
      int bannedIpId,
      long ipAddress,
      DateTime? bannedUntil,
      DateTime banningDate,
      int sourceCmid,
      string sourceName,
      int targetCmid,
      string targetName,
      string reason)
    {
      this.BannedIpId = bannedIpId;
      this.IpAddress = ipAddress;
      this.BannedUntil = bannedUntil;
      this.BanningDate = banningDate;
      this.SourceCmid = sourceCmid;
      this.SourceName = sourceName;
      this.TargetCmid = targetCmid;
      this.TargetName = targetName;
      this.Reason = reason;
    }
  }
}
