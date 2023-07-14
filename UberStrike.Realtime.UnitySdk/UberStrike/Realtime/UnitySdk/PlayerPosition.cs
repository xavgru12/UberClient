// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PlayerPosition
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
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
