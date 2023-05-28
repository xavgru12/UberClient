// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberPositionUpdateView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MemberPositionUpdateView
  {
    public int GroupId { get; set; }

    public string AuthToken { get; set; }

    public int MemberCmid { get; set; }

    public GroupPosition Position { get; set; }

    public MemberPositionUpdateView()
    {
    }

    public MemberPositionUpdateView(
      int groupId,
      string authToken,
      int memberCmid,
      GroupPosition position)
    {
      this.GroupId = groupId;
      this.AuthToken = authToken;
      this.MemberCmid = memberCmid;
      this.Position = position;
    }

    public override string ToString() => "[MemberPositionUpdateView: [GroupId:" + this.GroupId.ToString() + "][AuthToken:" + this.AuthToken + "][MemberCmid:" + this.MemberCmid.ToString() + "][Position:" + this.Position.ToString() + "]]";
  }
}
