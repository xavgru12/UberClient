using Cmune.DataCenter.Common.Entities;

public class InboxMessage
{
	public bool IsMine => MessageView.FromCmid == PlayerDataManager.Cmid;

	public bool IsAdmin => MessageView.FromCmid == 767;

	public string Content => MessageView.ContentText;

	public string SentDateString => MessageView.DateSent.ToString("MMM") + " " + MessageView.DateSent.Day.ToString() + " at " + MessageView.DateSent.ToShortTimeString();

	public string SenderName
	{
		get;
		private set;
	}

	public PrivateMessageView MessageView
	{
		get;
		private set;
	}

	public InboxMessage(PrivateMessageView view, string senderName)
	{
		MessageView = view;
		SenderName = senderName;
	}
}
