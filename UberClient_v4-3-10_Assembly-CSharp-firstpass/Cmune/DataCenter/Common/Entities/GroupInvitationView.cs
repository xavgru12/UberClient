// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.GroupInvitationView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[GroupInvitationDisplayView: [InviterCmid: " + this.InviterCmid.ToString() + "][InviterName: " + this.InviterName + "]" + "[GroupName: " + this.GroupName + "][GroupTag: " + this.GroupTag + "][GroupId: " + this.GroupId.ToString() + "]" + "[GroupInvitationId:" + this.GroupInvitationId.ToString() + "][InviteeCmid:" + this.InviteeCmid.ToString() + "][InviteeName:" + this.InviteeName + "]" + "[Message:" + this.Message + "]]";
  }
}
