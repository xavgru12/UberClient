using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class GroupInvitationView
	{
		public string InviterName
		{
			get;
			set;
		}

		public int InviterCmid
		{
			get;
			set;
		}

		public int GroupId
		{
			get;
			set;
		}

		public string GroupName
		{
			get;
			set;
		}

		public string GroupTag
		{
			get;
			set;
		}

		public int GroupInvitationId
		{
			get;
			set;
		}

		public string InviteeName
		{
			get;
			set;
		}

		public int InviteeCmid
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public GroupInvitationView()
		{
		}

		public GroupInvitationView(int inviterCmid, int groupId, int inviteeCmid, string message)
		{
			InviterCmid = inviterCmid;
			InviterName = string.Empty;
			GroupName = string.Empty;
			GroupTag = string.Empty;
			GroupId = groupId;
			GroupInvitationId = 0;
			InviteeCmid = inviteeCmid;
			InviteeName = string.Empty;
			Message = message;
		}

		public GroupInvitationView(int inviterCmid, string inviterName, string groupName, string groupTag, int groupId, int groupInvitationId, int inviteeCmid, string inviteeName, string message)
		{
			InviterCmid = inviterCmid;
			InviterName = inviterName;
			GroupName = groupName;
			GroupTag = groupTag;
			GroupId = groupId;
			GroupInvitationId = groupInvitationId;
			InviteeCmid = inviteeCmid;
			InviteeName = inviteeName;
			Message = message;
		}

		public override string ToString()
		{
			string text = "[GroupInvitationDisplayView: [InviterCmid: " + InviterCmid.ToString() + "][InviterName: " + InviterName + "]";
			string text2 = text;
			text = text2 + "[GroupName: " + GroupName + "][GroupTag: " + GroupTag + "][GroupId: " + GroupId.ToString() + "]";
			text2 = text;
			text = text2 + "[GroupInvitationId:" + GroupInvitationId.ToString() + "][InviteeCmid:" + InviteeCmid.ToString() + "][InviteeName:" + InviteeName + "]";
			return text + "[Message:" + Message + "]]";
		}
	}
}
