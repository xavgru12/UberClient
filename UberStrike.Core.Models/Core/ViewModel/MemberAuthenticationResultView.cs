// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.MemberAuthenticationResultView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
