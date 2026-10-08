using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class IpReputationView
	{
		public bool IsMalicious
		{
			get;
			set;
		}

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

		public IpReputationView()
		{
			IpAddress = string.Empty;
			Reason = string.Empty;
		}
	}
}
