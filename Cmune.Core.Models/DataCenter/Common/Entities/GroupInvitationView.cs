// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.GroupInvitationView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class GroupInvitationView
  {
    public string InviterName { get; set; }

    public int InviterCmid { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public string GroupTag { get; set; }

    public int GroupInvitationId { get; set; }

    public string InviteeName { get; set; }

    public int InviteeCmid { get; set; }

    public string Message { get; set; }

    public GroupInvitationView()
    {
    }

    public GroupInvitationView(int inviterCmid, int groupId, int inviteeCmid, string message)
    {
      this.InviterCmid = inviterCmid;
      this.InviterName = string.Empty;
      this.GroupName = string.Empty;
      this.GroupTag = string.Empty;
      this.GroupId = groupId;
      this.GroupInvitationId = 0;
      this.InviteeCmid = inviteeCmid;
      this.InviteeName = string.Empty;
      this.Message = message;
    }

    public GroupInvitationView(
      int inviterCmid,
      string inviterName,
      string groupName,
      string groupTag,
      int groupId,
      int groupInvitationId,
      int inviteeCmid,
      string inviteeName,
      string message)
    {
      this.InviterCmid = inviterCmid;
      this.InviterName = inviterName;
      this.GroupName = groupName;
      this.GroupTag = groupTag;
      this.GroupId = groupId;
      this.GroupInvitationId = groupInvitationId;
      this.InviteeCmid = inviteeCmid;
      this.InviteeName = inviteeName;
      this.Message = message;
    }

    public override string ToString() => "[GroupInvitationDisplayView: [InviterCmid: " + (object) this.InviterCmid + "][InviterName: " + this.InviterName + "]" + "[GroupName: " + this.GroupName + "][GroupTag: " + this.GroupTag + "][GroupId: " + (object) this.GroupId + "]" + "[GroupInvitationId:" + (object) this.GroupInvitationId + "][InviteeCmid:" + (object) this.InviteeCmid + "][InviteeName:" + this.InviteeName + "]" + "[Message:" + this.Message + "]]";
  }
}
