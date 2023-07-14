// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CommActorInfo
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Text;

namespace UberStrike.Realtime.UnitySdk
{
  public class CommActorInfo : ActorInfo
  {
    [CMUNESYNC(512)]
    private string _modInfo = string.Empty;
    [CMUNESYNC(1024)]
    private byte _moderationFlag = 0;

    public CommActorInfo()
    {
    }

    public CommActorInfo(SyncObject obj) => this.ReadSyncData(obj);

    public CommActorInfo(string name, int actorId, ChannelType channel)
      : base(name, actorId, channel)
    {
      this.PlayerName = name;
      this.ActorId = actorId;
      this.Channel = channel;
    }

    public CommActorInfo(
      string name,
      int actorId,
      int cmuneId,
      string clanTag,
      ChannelType channel)
      : this(name, actorId, channel)
    {
      this.Cmid = cmuneId;
      this.ClanTag = clanTag;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Name: " + this.PlayerName);
      stringBuilder.AppendLine("ActorID: " + (object) this.ActorId);
      stringBuilder.AppendLine("Cmid: " + (object) this.Cmid);
      stringBuilder.AppendLine("Flag: " + (object) this._moderationFlag);
      stringBuilder.AppendLine("Room: " + (object) this.CurrentRoom);
      return stringBuilder.ToString();
    }

    public bool IsInGame => this.CurrentRoom.Number != 88;

    public byte ModerationFlag
    {
      get => this._moderationFlag;
      set => this._moderationFlag = value;
    }

    public string ModInformation
    {
      get => this._modInfo;
      set => this._modInfo = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    [ExtendableEnumBounds(512, 1073741824)]
    public new class FieldTag : CmuneDeltaSync.FieldTag
    {
      public const int ModInformation = 512;
      public const int ModerationFlag = 1024;
    }

    [Flags]
    public enum ModerationTag
    {
      None = 0,
      Muted = 1,
      Ghosted = 2,
      Banned = 4,
      Speedhacking = 8,
      Spamming = 16, // 0x00000010
      Language = 32, // 0x00000020
      Name = 64, // 0x00000040
    }
  }
}
