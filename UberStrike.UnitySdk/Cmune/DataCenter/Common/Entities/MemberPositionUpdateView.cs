
using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MemberPositionUpdateView
  {
    public int GroupId { get; set; }

    public int CmidMakingAction { get; set; }

    public int MemberCmid { get; set; }

    public GroupPosition Position { get; set; }

    public MemberPositionUpdateView()
    {
    }

    public MemberPositionUpdateView(
      int groupId,
      int cmidMakingAction,
      int memberCmid,
      GroupPosition position)
    {
      this.GroupId = groupId;
      this.CmidMakingAction = cmidMakingAction;
      this.MemberCmid = memberCmid;
      this.Position = position;
    }

    public override string ToString() => "[MemberPositionUpdateView: [GroupId:" + (object) this.GroupId + "][CmidMakingAction:" + (object) this.CmidMakingAction + "][MemberCmid:" + (object) this.MemberCmid + "][Position:" + (object) this.Position + "]]";
  }
}
