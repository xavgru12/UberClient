
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
