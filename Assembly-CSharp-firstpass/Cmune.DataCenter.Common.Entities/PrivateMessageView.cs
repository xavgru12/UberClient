using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class PrivateMessageView
	{
		public int PrivateMessageId
		{
			get;
			set;
		}

		public int FromCmid
		{
			get;
			set;
		}

		public string FromName
		{
			get;
			set;
		}

		public int ToCmid
		{
			get;
			set;
		}

		public DateTime DateSent
		{
			get;
			set;
		}

		public string ContentText
		{
			get;
			set;
		}

		public bool IsRead
		{
			get;
			set;
		}

		public bool HasAttachment
		{
			get;
			set;
		}

		public bool IsDeletedBySender
		{
			get;
			set;
		}

		public bool IsDeletedByReceiver
		{
			get;
			set;
		}

		public override string ToString()
		{
			string text = "[Private Message: ";
			string text2 = text;
			text = text2 + "[ID:" + PrivateMessageId.ToString() + "][From:" + FromCmid.ToString() + "][To:" + ToCmid.ToString() + "][Date:" + DateSent.ToString() + "][";
			text2 = text;
			text = text2 + "[Content:" + ContentText + "][Is Read:" + IsRead.ToString() + "][Has attachment:" + HasAttachment.ToString() + "][Is deleted by sender:" + IsDeletedBySender.ToString() + "][Is deleted by receiver:" + IsDeletedByReceiver.ToString() + "]";
			return text + "]";
		}
	}
}
