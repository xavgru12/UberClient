// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ClanView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
