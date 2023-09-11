
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

    public override string ToString() => "[Member report: [Source CMID: " + (object) this.SourceCmid + "][Target CMID: " + (object) this.TargetCmid + "][Type: " + (object) this.ReportType + "][Reason: " + this.Reason + "][Context: " + this.Context + "][Application ID: " + (object) this.ApplicationId + "][IP: " + this.IP + "]]";
  }
}
