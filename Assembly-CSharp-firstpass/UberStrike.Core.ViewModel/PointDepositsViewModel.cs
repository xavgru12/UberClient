using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;

namespace UberStrike.Core.ViewModel
{
	[Serializable]
	public class PointDepositsViewModel
	{
		public List<PointDepositView> PointDeposits
		{
			get;
			set;
		}

		public int TotalCount
		{
			get;
			set;
		}
	}
}
