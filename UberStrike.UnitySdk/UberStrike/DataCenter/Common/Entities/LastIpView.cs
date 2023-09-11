// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LastIpView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;

namespace UberStrike.DataCenter.Common.Entities
{
  public class LastIpView
  {
    public long Ip { get; private set; }

    public DateTime LastConnectionDate { get; private set; }

    public List<LinkedMemberView> LinkedMembers { get; private set; }

    public BannedIpView BannedIpView { get; private set; }

    public LastIpView(
      long ip,
      DateTime lastConnectionDate,
      List<LinkedMemberView> linkedMembers,
      BannedIpView bannedIpView)
    {
      this.Ip = ip;
      this.LastConnectionDate = lastConnectionDate;
      this.LinkedMembers = linkedMembers;
      this.BannedIpView = bannedIpView;
    }
  }
}
