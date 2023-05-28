// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ModerationActionView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  public class ModerationActionView
  {
    public int ModerationActionId { get; private set; }

    public ModerationActionType ActionType { get; private set; }

    public int SourceCmid { get; private set; }

    public string SourceName { get; private set; }

    public int TargetCmid { get; private set; }

    public string TargetName { get; private set; }

    public DateTime ActionDate { get; private set; }

    public int ApplicationId { get; private set; }

    public string Reason { get; private set; }

    public long SourceIp { get; private set; }

    public ModerationActionView(
      int moderationActionId,
      ModerationActionType actionType,
      int sourceCmid,
      string sourceName,
      int targetCmid,
      string targetName,
      DateTime actionDate,
      int applicationId,
      string reason,
      long sourceIp)
    {
      this.ModerationActionId = moderationActionId;
      this.ActionType = actionType;
      this.SourceCmid = sourceCmid;
      this.SourceName = sourceName;
      this.TargetCmid = targetCmid;
      this.TargetName = targetName;
      this.ActionDate = actionDate;
      this.ApplicationId = applicationId;
      this.Reason = reason;
      this.SourceIp = sourceIp;
    }
  }
}
