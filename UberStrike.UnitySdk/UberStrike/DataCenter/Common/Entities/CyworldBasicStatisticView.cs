﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.CyworldBasicStatisticView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UberStrike.DataCenter.Common.Entities
{
  public class CyworldBasicStatisticView : EsnsBasicStatisticView
  {
    public int CyworldId { get; private set; }

    public CyworldBasicStatisticView(int cyworldId, string name, int xp, int level, int cmid)
      : base(name, xp, level, cmid)
    {
      this.CyworldId = cyworldId;
    }

    public CyworldBasicStatisticView(int cyworldId) => this.CyworldId = cyworldId;

    public CyworldBasicStatisticView() => this.CyworldId = 0;

    public static List<CyworldBasicStatisticView> Rank(
      List<CyworldBasicStatisticView> views,
      int friendsDisplayedCount)
    {
      List<CyworldBasicStatisticView> basicStatisticViewList = new List<CyworldBasicStatisticView>();
      CyworldBasicStatisticView basicStatisticView = (CyworldBasicStatisticView) null;
      if (views.Count > 0)
        basicStatisticView = views[0];
      views = views.OrderByDescending<CyworldBasicStatisticView, int>((Func<CyworldBasicStatisticView, int>) (v => v.XP)).ToList<CyworldBasicStatisticView>();
      int num1 = 1;
      foreach (CyworldBasicStatisticView view in views)
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
        if (views[index].CyworldId != basicStatisticView.CyworldId)
        {
          basicStatisticViewList.Add(views[index]);
          ++num2;
        }
      }
      while (basicStatisticViewList.Count < friendsDisplayedCount + 1)
        basicStatisticViewList.Add(new CyworldBasicStatisticView());
      return basicStatisticViewList;
    }
  }
}
