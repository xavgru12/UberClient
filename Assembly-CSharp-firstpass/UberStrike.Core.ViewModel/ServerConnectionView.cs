using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.ViewModel
{
	[Serializable]
	public class ServerConnectionView
	{
		public string ApiVersion
		{
			get;
			set;
		}

		public int Cmid
		{
			get;
			set;
		}

		public ChannelType Channel
		{
			get;
			set;
		}

		public MemberAccessLevel AccessLevel
		{
			get;
			set;
		}
	}
}
