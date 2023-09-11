// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.FacebookBasicStatisticView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UberStrike.DataCenter.Common.Entities
{
  public class FacebookBasicStatisticView : EsnsBasicStatisticView
  {
    private string _picturePath;

    public long FacebookId { get; set; }

    public string FirstName { get; set; }

    public string PicturePath
    {
      get => this._picturePath;
      set
      {
        if (value.StartsWith("http:"))
          value = value.Replace("http:", "https:");
        this._picturePath = value;
      }
    }

    public FacebookBasicStatisticView(
      long facebookId,
      string firstName,
      string picturePath,
      string name,
      int xp,
      int level,
      int cmid)
      : base(name, xp, level, cmid)
    {
      this.FacebookId = facebookId;
      this.FirstName = firstName;
      this.PicturePath = picturePath;
    }

    public FacebookBasicStatisticView(long facebookId, string firstName, string picturePath)
    {
      this.FacebookId = facebookId;
      this.FirstName = firstName;
      this.PicturePath = picturePath;
    }

    public FacebookBasicStatisticView() => this.FacebookId = 0L;

    public static List<FacebookBasicStatisticView> Rank(
      List<FacebookBasicStatisticView> views,
      int friendsDisplayedCount)
    {
      List<FacebookBasicStatisticView> basicStatisticViewList = new List<FacebookBasicStatisticView>();
      FacebookBasicStatisticView basicStatisticView = (FacebookBasicStatisticView) null;
      if (views.Count > 0)
        basicStatisticView = views[0];
      views = views.OrderByDescending<FacebookBasicStatisticView, int>((Func<FacebookBasicStatisticView, int>) (v => v.XP)).ToList<FacebookBasicStatisticView>();
      int num1 = 1;
      foreach (FacebookBasicStatisticView view in views)
      {
        if (view.Cmid != 0)
        {
          view.SocialRank = num1;
          ++num1;
        }
      }
      basicStatisticViewList.Add(basicStatisticView);
      int num2 = 0;
      for (int index = 0; index < friendsDisplayedCount && index < views.Count; ++index)
      {
        if (views[index].FacebookId != basicStatisticView.FacebookId)
        {
          basicStatisticViewList.Add(views[index]);
          ++num2;
        }
      }
      while (basicStatisticViewList.Count < friendsDisplayedCount + 1)
        basicStatisticViewList.Add(new FacebookBasicStatisticView());
      return basicStatisticViewList;
    }
  }
}
