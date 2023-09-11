// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.MemberAuthenticationResultView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class MemberAuthenticationResultView
  {
    public MemberAuthenticationResult MemberAuthenticationResult { get; set; }

    public MemberView MemberView { get; set; }

    public PlayerStatisticsView PlayerStatisticsView { get; set; }

    public DateTime ServerTime { get; set; }

    public bool IsAccountComplete { get; set; }

    public bool IsTutorialComplete { get; set; }

    public WeeklySpecialView WeeklySpecial { get; set; }

    public LuckyDrawUnityView LuckyDraw { get; set; }
  }
}
