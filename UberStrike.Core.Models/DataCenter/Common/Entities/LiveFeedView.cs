// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LiveFeedView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class LiveFeedView
  {
    public DateTime Date { get; set; }

    public int Priority { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public int LivedFeedId { get; set; }

    public LiveFeedView()
    {
      this.Date = DateTime.Now;
      this.Priority = 0;
      this.Description = string.Empty;
      this.Url = string.Empty;
      this.LivedFeedId = 0;
    }

    public LiveFeedView(
      DateTime date,
      int priority,
      string description,
      string url,
      int liveFeedId)
    {
      this.Date = date;
      this.Priority = priority;
      this.Description = description;
      this.Url = url;
      this.LivedFeedId = liveFeedId;
    }
  }
}
