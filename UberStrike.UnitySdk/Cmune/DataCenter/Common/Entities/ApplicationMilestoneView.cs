// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ApplicationMilestoneView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  public class ApplicationMilestoneView
  {
    public int MilestoneId { get; private set; }

    public int ApplicationId { get; private set; }

    public DateTime MilestoneDate { get; private set; }

    public string Description { get; private set; }

    public ApplicationMilestoneView(
      int milestoneId,
      int applicationId,
      DateTime milestoneDate,
      string description)
    {
      this.MilestoneId = milestoneId;
      this.ApplicationId = applicationId;
      this.MilestoneDate = milestoneDate;
      this.Description = description;
    }
  }
}
