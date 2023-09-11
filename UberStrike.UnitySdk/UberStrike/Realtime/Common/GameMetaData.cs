// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.GameMetaData
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using Cmune.Util;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public class GameMetaData : RoomMetaData, IGameMetaData
  {
    protected int _latency = 0;
    protected int _gameFlags = 0;
    protected int _mapID = 0;
    protected int _timeLimitSeconds = 0;
    protected int _splatLimit = 0;
    protected byte _levelMin = 0;
    protected byte _levelMax = byte.MaxValue;
    protected short _gameMode = 101;
    private string _rountimeString = string.Empty;
    private string _connectedPlayersString = string.Empty;
    private int _playersInGame = 0;
    private string _playersInGameString = string.Empty;

    public static GameMetaData Empty => new GameMetaData();

    protected GameMetaData()
    {
    }

    public GameMetaData(byte[] t, ref int idx)
    {
      try
      {
        idx = this.FromBytes(t, idx);
      }
      catch
      {
        CmuneDebug.LogError("EXCEPTION: Error Deserializing GameMetaData at {0}:\n{1}", (object) idx, (object) CmunePrint.Values((object) t));
      }
    }

    public GameMetaData(int roomNumber, string roomName, string server)
      : base(roomNumber, roomName, server)
    {
    }

    public GameMetaData(
      int roomNumber,
      string roomName,
      string server,
      int mapID,
      string password,
      int maxTimeSeconds,
      int maxPlayers,
      short mode)
      : this(roomNumber, roomName, server)
    {
      this._mapID = mapID;
      this._password = password;
      this.RoundTime = maxTimeSeconds;
      this._maxPlayers = maxPlayers;
      this._gameMode = mode;
    }

    public GameMetaData(
      int mapID,
      string password,
      int maxTimeSeconds,
      int maxPlayers,
      short mode)
    {
      this._mapID = mapID;
      this._password = password;
      this.RoundTime = maxTimeSeconds;
      this._maxPlayers = maxPlayers;
      this._gameMode = mode;
    }

    public bool CanEnterRoom => this.RoomID.CanConnectToServer && this.RoomID.IsVersionCompatible && !this.IsFull;

    public virtual int RoundTime
    {
      get => this._timeLimitSeconds;
      set
      {
        this._timeLimitSeconds = value;
        if (this.IsTimeUnlimited)
          this._rountimeString = "Unlimited";
        else if (this._timeLimitSeconds < 120)
          this._rountimeString = string.Format("{0} Seconds", (object) this._timeLimitSeconds);
        else
          this._rountimeString = string.Format("{0} Minutes", (object) (this._timeLimitSeconds / 60));
      }
    }

    public string RoundTimeString => this._rountimeString;

    public string ConnectedPlayersString => this._connectedPlayersString;

    public override int ConnectedPlayers
    {
      get => base.ConnectedPlayers;
      set
      {
        base.ConnectedPlayers = value;
        this._connectedPlayersString = string.Format("{0} / {1}", (object) this.ConnectedPlayers, (object) this.MaxPlayers);
      }
    }

    public int InGamePlayers
    {
      get => this._playersInGame;
      set
      {
        this._playersInGame = value;
        this._playersInGameString = string.Format("{0} / {1}", (object) value, (object) this.MaxPlayers);
      }
    }

    public string InGamePlayersString => this._playersInGameString;

    public bool IsTimeUnlimited => this._timeLimitSeconds <= 0;

    public virtual int MapID
    {
      get => this._mapID;
      set => this._mapID = value;
    }

    public virtual short GameMode
    {
      get => this._gameMode;
      set => this._gameMode = value;
    }

    public int GameModifierFlags
    {
      get => this._gameFlags;
      set => this._gameFlags = value;
    }

    public bool HasGameFlag(GameFlags.GAME_FLAGS flag) => GameFlags.IsFlagSet(flag, this._gameFlags);

    public int SplatLimit
    {
      get => this._splatLimit;
      set => this._splatLimit = value;
    }

    public int Latency
    {
      get => this._latency;
      set => this._latency = value;
    }

    public int LevelMin
    {
      get => (int) this._levelMin;
      set => this._levelMin = (byte) Mathf.Clamp(value, 0, (int) byte.MaxValue);
    }

    public int LevelMax
    {
      get => (int) this._levelMax;
      set => this._levelMax = (byte) Mathf.Clamp(value, 0, (int) byte.MaxValue);
    }

    public bool HasLevelRestriction => this._levelMin != (byte) 0 || this._levelMax != byte.MaxValue;

    public bool IsLevelAllowed(int level) => level >= (int) this._levelMin && level <= (int) this._levelMax;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(base.ToString());
      stringBuilder.AppendFormat("Map: {0}\n", (object) this._mapID);
      stringBuilder.AppendFormat("Time: {0}\n", (object) this._timeLimitSeconds);
      stringBuilder.AppendFormat("Mode: {0}\n", (object) this._gameMode);
      return stringBuilder.ToString();
    }

    public override byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>((IEnumerable<byte>) base.GetBytes());
      DefaultByteConverter.FromShort((short) this._mapID, ref bytes);
      DefaultByteConverter.FromInt(this._timeLimitSeconds, ref bytes);
      DefaultByteConverter.FromInt(this._splatLimit, ref bytes);
      DefaultByteConverter.FromShort(this._gameMode, ref bytes);
      DefaultByteConverter.FromInt(this._gameFlags, ref bytes);
      DefaultByteConverter.FromByte(this._levelMin, ref bytes);
      DefaultByteConverter.FromByte(this._levelMax, ref bytes);
      DefaultByteConverter.FromInt(this._playersInGame, ref bytes);
      return bytes.ToArray();
    }

    public override int FromBytes(byte[] bytes, int idx)
    {
      idx = base.FromBytes(bytes, idx);
      this._mapID = (int) DefaultByteConverter.ToShort(bytes, ref idx);
      this._timeLimitSeconds = DefaultByteConverter.ToInt(bytes, ref idx);
      this._splatLimit = DefaultByteConverter.ToInt(bytes, ref idx);
      this._gameMode = DefaultByteConverter.ToShort(bytes, ref idx);
      this._gameFlags = DefaultByteConverter.ToInt(bytes, ref idx);
      this._levelMin = DefaultByteConverter.ToByte(bytes, ref idx);
      this._levelMax = DefaultByteConverter.ToByte(bytes, ref idx);
      this._playersInGame = DefaultByteConverter.ToInt(bytes, ref idx);
      this._connectedPlayersString = string.Format("{0} / {1}", (object) this._playersInGame, (object) this.MaxPlayers);
      this.RoundTime = this._timeLimitSeconds;
      this._connectedPlayersString = string.Format("{0} / {1}", (object) this.ConnectedPlayers, (object) this.MaxPlayers);
      return idx;
    }
  }
}
