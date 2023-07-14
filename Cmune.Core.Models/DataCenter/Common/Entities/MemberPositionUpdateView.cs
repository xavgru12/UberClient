// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberPositionUpdateView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
