// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.MemberAuthenticationResultView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public string ServerGameVersion { get; set; }

    public bool IsAccountComplete { get; set; }

    public LuckyDrawUnityView LuckyDraw { get; set; }

    public string AuthToken { get; set; }

    public int BanDuration { get; set; }
  }
}
