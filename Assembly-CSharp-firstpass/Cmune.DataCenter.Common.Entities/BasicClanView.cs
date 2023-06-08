using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class BasicClanView
	{
		public int GroupId
		{
			get;
			set;
		}

		public int MembersCount
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Motto
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public DateTime FoundingDate
		{
			get;
			set;
		}

		public string Picture
		{
			get;
			set;
		}

		public GroupType Type
		{
			get;
			set;
		}

		public DateTime LastUpdated
		{
			get;
			set;
		}

		public string Tag
		{
			get;
			set;
		}

		public int MembersLimit
		{
			get;
			set;
		}

		public GroupColor ColorStyle
		{
			get;
			set;
		}

		public GroupFontStyle FontStyle
		{
			get;
			set;
		}

		public int ApplicationId
		{
			get;
			set;
		}

		public int OwnerCmid
		{
			get;
			set;
		}

		public string OwnerName
		{
			get;
			set;
		}

		public BasicClanView()
		{
		}

		public BasicClanView(int groupId, int membersCount, string description, string name, string motto, string address, DateTime foundingDate, string picture, GroupType type, DateTime lastUpdated, string tag, int membersLimit, GroupColor colorStyle, GroupFontStyle fontStyle, int applicationId, int ownerCmid, string ownerName)
		{
			SetClan(groupId, membersCount, description, name, motto, address, foundingDate, picture, type, lastUpdated, tag, membersLimit, colorStyle, fontStyle, applicationId, ownerCmid, ownerName);
		}

		public void SetClan(int groupId, int membersCount, string description, string name, string motto, string address, DateTime foundingDate, string picture, GroupType type, DateTime lastUpdated, string tag, int membersLimit, GroupColor colorStyle, GroupFontStyle fontStyle, int applicationId, int ownerCmid, string ownerName)
		{
			GroupId = groupId;
			MembersCount = membersCount;
			Description = description;
			Name = name;
			Motto = motto;
			Address = address;
			FoundingDate = foundingDate;
			Picture = picture;
			Type = type;
			LastUpdated = lastUpdated;
			Tag = tag;
			MembersLimit = membersLimit;
			ColorStyle = colorStyle;
			FontStyle = fontStyle;
			ApplicationId = applicationId;
			OwnerCmid = ownerCmid;
			OwnerName = ownerName;
		}

		public override string ToString()
		{
			string text = "[Clan: [Id: " + GroupId.ToString() + "][Members count: " + MembersCount.ToString() + "][Description: " + Description + "]";
			string text2 = text;
			text = text2 + "[Name: " + Name + "][Motto: " + Name + "][Address: " + Address + "]";
			text2 = text;
			text = text2 + "[Creation date: " + FoundingDate.ToString() + "][Picture: " + Picture + "][Type: " + Type.ToString() + "][Last updated: " + LastUpdated.ToString() + "]";
			text2 = text;
			text = text2 + "[Tag: " + Tag + "][Members limit: " + MembersLimit.ToString() + "][Color style: " + ColorStyle.ToString() + "][Font style: " + FontStyle.ToString() + "]";
			text2 = text;
			return text2 + "[Application Id: " + ApplicationId.ToString() + "][Owner Cmid: " + OwnerCmid.ToString() + "][Owner name: " + OwnerName + "]]";
		}
	}
}
