// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LastIpView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
