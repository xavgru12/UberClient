// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ClanView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ClanView : BasicClanView
  {
    public List<ClanMemberView> Members { get; set; }

    public ClanView() => this.Members = new List<ClanMemberView>();

    public ClanView(
      int groupId,
      int membersCount,
      string description,
      string name,
      string motto,
      string address,
      DateTime foundingDate,
      string picture,
      GroupType type,
      DateTime lastUpdated,
      string tag,
      int membersLimit,
      GroupColor colorStyle,
      GroupFontStyle fontStyle,
      int applicationId,
      int ownerCmid,
      string ownerName,
      List<ClanMemberView> members)
      : base(groupId, membersCount, description, name, motto, address, foundingDate, picture, type, lastUpdated, tag, membersLimit, colorStyle, fontStyle, applicationId, ownerCmid, ownerName)
    {
      this.Members = members;
    }

    public override string ToString()
    {
      string str = "[Clan: " + base.ToString() + "[Members:";
      foreach (ClanMemberView member in this.Members)
        str += member.ToString();
      return str + "]";
    }
  }
}
