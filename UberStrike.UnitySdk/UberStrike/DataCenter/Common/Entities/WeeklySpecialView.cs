// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.WeeklySpecialView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class WeeklySpecialView
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public string ImageUrl { get; set; }

    public int ItemId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public WeeklySpecialView()
    {
    }

    public WeeklySpecialView(string title, string text, string imageUrl, int itemId)
    {
      this.Title = title;
      this.Text = text;
      this.ImageUrl = imageUrl;
      this.ItemId = itemId;
    }

    public WeeklySpecialView(
      string title,
      string text,
      string imageUrl,
      int itemId,
      int id,
      DateTime startDate,
      DateTime? endDate)
      : this(title, text, imageUrl, itemId)
    {
      this.Id = id;
      this.StartDate = startDate;
      this.EndDate = endDate;
    }
  }
}
