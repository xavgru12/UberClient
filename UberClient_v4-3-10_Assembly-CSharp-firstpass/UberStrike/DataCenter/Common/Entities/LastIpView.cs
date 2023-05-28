// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LastIpView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
