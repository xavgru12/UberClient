using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class MaliciousIp
	{
		public string IpAddress
		{
			get;
			set;
		}

		public string Reason
		{
			get;
			set;
		}

		public MaliciousIp()
		{
			IpAddress = string.Empty;
			Reason = string.Empty;
		}
	}
}
