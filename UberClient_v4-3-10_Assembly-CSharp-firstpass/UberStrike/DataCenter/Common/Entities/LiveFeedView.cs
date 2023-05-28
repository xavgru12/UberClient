// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LiveFeedView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
      this.Date = DateTime.UtcNow;
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
