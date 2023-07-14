// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ActorInfo
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Text;

namespace UberStrike.Realtime.UnitySdk
{
  public class ActorInfo : CmuneDeltaSync
  {
    [CMUNESYNC(1)]
    private int _cmuneId = 0;
    [CMUNESYNC(2)]
    private string _name = string.Empty;
    [CMUNESYNC(32)]
    private string _clanTag = string.Empty;
    [CMUNESYNC(64)]
    private byte _accessLevel = 0;
    [CMUNESYNC(4)]
    private byte _channel = 0;
    [CMUNESYNC(8)]
    private CmuneRoomID _currentRoom = CmuneRoomID.Empty;
    [CMUNESYNC(16)]
    private ushort _ping = 0;

    protected ActorInfo()
    {
    }

    public ActorInfo(string name, int playerID, ChannelType platform)
    {
      this.PlayerName = name;
      this.ActorId = playerID;
      this.Channel = platform;
    }

    public bool IsLoggedIn => this._cmuneId > 0;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("ActorId: " + (object) this.ActorId);
      stringBuilder.AppendLine("Cmid: " + (object) this._cmuneId);
      stringBuilder.AppendLine("Name: " + this._name);
      stringBuilder.AppendLine("Channel: " + (object) this._channel);
      stringBuilder.AppendLine("Room: " + (object) this._currentRoom);
      return stringBuilder.ToString();
    }

    public CmuneRoomID CurrentRoom
    {
      get => this._currentRoom;
      set => this._currentRoom = value;
    }

    public int ActorId
    {
      get => this.InstanceId;
      set => this.InstanceId = value;
    }

    public ushort Ping
    {
      get => this._ping;
      set => this._ping = value;
    }

    public int Cmid
    {
      get => this._cmuneId;
      set => this._cmuneId = value;
    }

    public string PlayerName
    {
      get => this._name;
      set => this._name = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public ChannelType Channel
    {
      get => (ChannelType) this._channel;
      set => this._channel = (byte) value;
    }

    public string ClanTag
    {
      get => this._clanTag;
      set => this._clanTag = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public int AccessLevel
    {
      get => (int) this._accessLevel;
      set => this._accessLevel = (byte) Math.Min(value, 10);
    }

    [ExtendableEnumBounds(1, 64)]
    public new class FieldTag : CmuneDeltaSync.FieldTag
    {
      public const int Cmid = 1;
      public const int PlayerName = 2;
      public const int Channel = 4;
      public const int CurrentRoom = 8;
      public const int Ping = 16;
      public const int ClanTag = 32;
      public const int AccessLevel = 64;
    }
  }
}
