// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BannedIpView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
