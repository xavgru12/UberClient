using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class MemberReportView
	{
		public int SourceCmid
		{
			get;
			set;
		}

		public int TargetCmid
		{
			get;
			set;
		}

		public MemberReportType ReportType
		{
			get;
			set;
		}

		public string Reason
		{
			get;
			set;
		}

		public string Context
		{
			get;
			set;
		}

		public int ApplicationId
		{
			get;
			set;
		}

		public string IP
		{
			get;
			set;
		}

		public MemberReportView()
		{
		}

		public MemberReportView(int sourceCmid, int targetCmid, MemberReportType reportType, string reason, string context, int applicationID, string ip)
		{
			SourceCmid = sourceCmid;
			TargetCmid = targetCmid;
			ReportType = reportType;
			Reason = reason;
			Context = context;
			ApplicationId = applicationID;
			IP = ip;
		}

		public override string ToString()
		{
			return "[Member report: [Source CMID: " + SourceCmid.ToString() + "][Target CMID: " + TargetCmid.ToString() + "][Type: " + ReportType.ToString() + "][Reason: " + Reason + "][Context: " + Context + "][Application ID: " + ApplicationId.ToString() + "][IP: " + IP + "]]";
		}
	}
}
