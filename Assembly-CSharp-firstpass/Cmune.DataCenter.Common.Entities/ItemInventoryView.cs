using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class ItemInventoryView
	{
		public int Cmid
		{
			get;
			set;
		}

		public int ItemId
		{
			get;
			set;
		}

		public DateTime? ExpirationDate
		{
			get;
			set;
		}

		public int AmountRemaining
		{
			get;
			set;
		}

		public ItemInventoryView()
		{
		}

		public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining)
		{
			ItemId = itemId;
			ExpirationDate = expirationDate;
			AmountRemaining = amountRemaining;
		}

		public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining, int cmid)
			: this(itemId, expirationDate, amountRemaining)
		{
			Cmid = cmid;
		}

		public override string ToString()
		{
			string text = "[LiveInventoryView: ";
			string str = text;
			text = str + "[Item Id: " + ItemId.ToString() + "]";
			str = text;
			text = str + "[Expiration date: " + ExpirationDate.ToString() + "]";
			str = text;
			text = str + "[Amount remaining:" + AmountRemaining.ToString() + "]";
			return text + "]";
		}
	}
}
