// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ApplicationMilestoneView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
