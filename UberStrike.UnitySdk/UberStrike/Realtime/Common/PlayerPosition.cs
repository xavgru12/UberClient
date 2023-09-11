// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.PlayerPosition
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public struct PlayerPosition
  {
    public byte Player;
    public Vector3 Position;
    public int Time;

    public PlayerPosition(byte id, ShortVector3 p, int time)
    {
      this.Player = id;
      this.Position = p.Vector3;
      this.Time = time;
    }
  }
}
