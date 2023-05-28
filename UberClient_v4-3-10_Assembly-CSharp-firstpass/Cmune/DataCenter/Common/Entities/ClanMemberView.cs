// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ClanMemberView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[Clan member: [Name: " + this.Name + "][Cmid: " + this.Cmid.ToString() + "][Position: " + this.Position.ToString() + "][JoiningDate: " + this.JoiningDate.ToString() + "][Lastlogin: " + this.Lastlogin.ToString() + "]]";
  }
}
