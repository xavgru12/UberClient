
using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ClanMemberView
  {
    public string Name { get; set; }

    public int Cmid { get; set; }

    public GroupPosition Position { get; set; }

    public DateTime JoiningDate { get; set; }

    public DateTime Lastlogin { get; set; }

    public ClanMemberView()
    {
    }

    public ClanMemberView(
      string name,
      int cmid,
      GroupPosition position,
      DateTime joiningDate,
      DateTime lastLogin)
    {
      this.Cmid = cmid;
      this.Name = name;
      this.Position = position;
      this.JoiningDate = joiningDate;
      this.Lastlogin = lastLogin;
    }

    public override string ToString() => "[Clan member: [Name: " + this.Name + "][Cmid: " + (object) this.Cmid + "][Position: " + (object) this.Position + "][JoiningDate: " + (object) this.JoiningDate + "][Lastlogin: " + (object) this.Lastlogin + "]]";
  }
}
