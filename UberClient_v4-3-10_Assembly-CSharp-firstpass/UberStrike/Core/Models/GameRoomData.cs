// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.GameRoomData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class GameRoomData : RoomData
  {
    public int ConnectedPlayers { get; set; }

    public int PlayerLimit { get; set; }

    public int TimeLimit { get; set; }

    public int KillLimit { get; set; }

    public int GameFlags { get; set; }

    public int MapID { get; set; }

    public byte LevelMin { get; set; }

    public byte LevelMax { get; set; }

    public GameModeType GameMode { get; set; }

    public GameBoxType BoxType { get; set; }

    public bool IsPermanentGame { get; set; }

    public bool IsFull => this.ConnectedPlayers >= this.PlayerLimit;
  }
}
