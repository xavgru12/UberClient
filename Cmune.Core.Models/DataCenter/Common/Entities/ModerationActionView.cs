// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ModerationActionView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
