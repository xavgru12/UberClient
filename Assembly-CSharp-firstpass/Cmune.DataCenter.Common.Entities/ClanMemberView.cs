using System;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class ClanMemberView
	{
		public string Name
		{
			get;
			set;
		}

		public int Cmid
		{
			get;
			set;
		}

		public GroupPosition Position
		{
			get;
			set;
		}

		public DateTime JoiningDate
		{
			get;
			set;
		}

		public DateTime Lastlogin
		{
			get;
			set;
		}

		public ClanMemberView()
		{
		}

		public ClanMemberView(string name, int cmid, GroupPosition position, DateTime joiningDate, DateTime lastLogin)
		{
			Cmid = cmid;
			Name = name;
			Position = position;
			JoiningDate = joiningDate;
			Lastlogin = lastLogin;
		}

		public override string ToString()
		{
			return "[Clan member: [Name: " + Name + "][Cmid: " + Cmid.ToString() + "][Position: " + Position.ToString() + "][JoiningDate: " + JoiningDate.ToString() + "][Lastlogin: " + Lastlogin.ToString() + "]]";
		}
	}
}
