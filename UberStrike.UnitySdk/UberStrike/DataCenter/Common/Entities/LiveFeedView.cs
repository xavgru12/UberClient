﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LiveFeedView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
