// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.MemberSessionDataView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace Cmune.Core.Models.Views
{
  [Serializable]
  public class MemberSessionDataView
  {
    public string AuthToken { get; set; }

    public int Cmid { get; set; }

    public string Name { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }

    public int Level { get; set; }

    public int XP { get; set; }

    public string ClanTag { get; set; }

    public ChannelType Channel { get; set; }

    public DateTime LoginDate { get; set; }

    public bool IsBanned { get; set; }
  }
}
