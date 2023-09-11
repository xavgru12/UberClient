// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberPositionUpdateView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
