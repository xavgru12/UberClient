// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ApplicationMilestoneView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
