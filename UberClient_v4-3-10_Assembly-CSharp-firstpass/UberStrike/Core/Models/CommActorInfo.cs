// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.CommActorInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.Models
{
  [Synchronizable]
  [Serializable]
  public class CommActorInfo
  {
    public int Cmid { get; set; }

    public string PlayerName { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }

    public ChannelType Channel { get; set; }

    public string ClanTag { get; set; }

    public GameRoom CurrentRoom { get; set; }

    public byte ModerationFlag { get; set; }

    public string ModInformation { get; set; }
  }
}
