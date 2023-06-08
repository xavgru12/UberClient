using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.Models.Views
{
	[Serializable]
	public class ItemPrice
	{
		public int Price
		{
			get;
			set;
		}

		public UberStrikeCurrencyType Currency
		{
			get;
			set;
		}

		public int Discount
		{
			get;
			set;
		}

		public int Amount
		{
			get;
			set;
		}

		public PackType PackType
		{
			get;
			set;
		}

		public BuyingDurationType Duration
		{
			get;
			set;
		}

		public bool IsConsumable => Amount > 0;
	}
}
