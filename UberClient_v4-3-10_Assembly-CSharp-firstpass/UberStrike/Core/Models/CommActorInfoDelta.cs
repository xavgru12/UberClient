// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.CommActorInfoDelta
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;

namespace UberStrike.Core.Models
{
  public class CommActorInfoDelta
  {
    public readonly Dictionary<CommActorInfoDelta.Keys, object> Changes = new Dictionary<CommActorInfoDelta.Keys, object>();

    public int DeltaMask { get; set; }

    public byte Id { get; set; }

    public void Apply(CommActorInfo instance)
    {
      foreach (KeyValuePair<CommActorInfoDelta.Keys, object> change in this.Changes)
      {
        switch (change.Key)
        {
          case CommActorInfoDelta.Keys.AccessLevel:
            instance.AccessLevel = (MemberAccessLevel) change.Value;
            continue;
          case CommActorInfoDelta.Keys.Channel:
            instance.Channel = (ChannelType) change.Value;
            continue;
          case CommActorInfoDelta.Keys.ClanTag:
            instance.ClanTag = (string) change.Value;
            continue;
          case CommActorInfoDelta.Keys.Cmid:
            instance.Cmid = (int) change.Value;
            continue;
          case CommActorInfoDelta.Keys.CurrentRoom:
            instance.CurrentRoom = (GameRoom) change.Value;
            continue;
          case CommActorInfoDelta.Keys.ModerationFlag:
            instance.ModerationFlag = (byte) change.Value;
            continue;
          case CommActorInfoDelta.Keys.ModInformation:
            instance.ModInformation = (string) change.Value;
            continue;
          case CommActorInfoDelta.Keys.PlayerName:
            instance.PlayerName = (string) change.Value;
            continue;
          default:
            continue;
        }
      }
    }

    public enum Keys
    {
      AccessLevel,
      Channel,
      ClanTag,
      Cmid,
      CurrentRoom,
      ModerationFlag,
      ModInformation,
      PlayerName,
    }
  }
}
