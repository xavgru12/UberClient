// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BannedIpView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
