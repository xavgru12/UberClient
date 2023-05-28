// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.WeeklySpecialView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
