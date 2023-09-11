
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
