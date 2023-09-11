// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ClanMemberView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
