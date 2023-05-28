// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberReportView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MemberReportView
  {
    public int SourceCmid { get; set; }

    public int TargetCmid { get; set; }

    public MemberReportType ReportType { get; set; }

    public string Reason { get; set; }

    public string Context { get; set; }

    public int ApplicationId { get; set; }

    public string IP { get; set; }

    public MemberReportView()
    {
    }

    public MemberReportView(
      int sourceCmid,
      int targetCmid,
      MemberReportType reportType,
      string reason,
      string context,
      int applicationID,
      string ip)
    {
      this.SourceCmid = sourceCmid;
      this.TargetCmid = targetCmid;
      this.ReportType = reportType;
      this.Reason = reason;
      this.Context = context;
      this.ApplicationId = applicationID;
      this.IP = ip;
    }

    public override string ToString() => "[Member report: [Source CMID: " + this.SourceCmid.ToString() + "][Target CMID: " + this.TargetCmid.ToString() + "][Type: " + this.ReportType.ToString() + "][Reason: " + this.Reason + "][Context: " + this.Context + "][Application ID: " + this.ApplicationId.ToString() + "][IP: " + this.IP + "]]";
  }
}
