// Decompiled with JetBrains decompiler
// Type: BezierSplines
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class BezierSplines
{
  public readonly LimitedQueue<BezierSplines.Packet> Packets = new LimitedQueue<BezierSplines.Packet>(10);
  public BezierSplines.Packet PreviousPacket;
  public BezierSplines.Packet LastPacket;
  public Vector3 _velocity;
  public Vector3 _checkVelocity;
  private Vector3 _lastPosition;
  private int debugCounter;

  public void AddSample(int serverTime, Vector3 pos)
  {
    if (this.LastPacket.Time >= serverTime)
      return;
    this.PreviousPacket = this.LastPacket;
    this.LastPacket = new BezierSplines.Packet(serverTime, pos, this.PreviousPacket.Pos - pos);
    this.Packets.Enqueue(this.LastPacket);
  }

  public int ReadPosition(int time, out Vector3 oPos)
  {
    int index = 0;
    if (this.Packets.Count > 0)
    {
      BezierSplines.Packet? nullable1 = new BezierSplines.Packet?();
      BezierSplines.Packet? nullable2 = new BezierSplines.Packet?();
      BezierSplines.Packet? nullable3 = new BezierSplines.Packet?();
      for (index = this.Packets.Count - 1; index > 0; --index)
      {
        BezierSplines.Packet packet = this.Packets[index];
        nullable2 = nullable1;
        nullable1 = new BezierSplines.Packet?(packet);
        if (packet.Time < time)
        {
          if (!nullable2.HasValue && index > 1)
          {
            nullable3 = new BezierSplines.Packet?(this.Packets[index - 1]);
            break;
          }
          break;
        }
      }
      this.debugCounter = index;
      if (nullable1.HasValue && nullable2.HasValue)
      {
        float num = (float) (nullable2.Value.Time - nullable1.Value.Time);
        float t = Mathf.Clamp01(Mathf.Abs((float) (time - nullable1.Value.Time) / num));
        oPos = this.InterpolateBetween(t, nullable1.Value, nullable2.Value);
        this._lastPosition = oPos;
      }
      else if (nullable1.HasValue && nullable3.HasValue)
      {
        oPos = nullable1.Value.Pos + BezierSplines.GetVelocity(nullable1.Value, nullable3.Value) * (float) Mathf.Clamp(time - nullable1.Value.Time, 0, 100);
        this._lastPosition = oPos;
      }
      else
      {
        index = 0;
        oPos = this._lastPosition;
      }
    }
    else
      oPos = this._lastPosition;
    return index;
  }

  private static Vector3 GetVelocity(BezierSplines.Packet a, BezierSplines.Packet b) => (a.Pos - b.Pos) / (float) Mathf.Abs(a.Time - b.Time);

  public Vector3 LatestPosition() => this.Packets.Count > 0 ? ((IEnumerable<BezierSplines.Packet>) this.Packets).Last<BezierSplines.Packet>().Pos : Vector3.zero;

  private Vector3 InterpolateBetween(float t, BezierSplines.Packet a, BezierSplines.Packet b) => Vector3.Lerp(a.Pos, b.Pos, t);

  public static Vector3 GetBSplineAt(float t, Vector3 p1, Vector3 ss1, Vector3 ss2, Vector3 p2)
  {
    Vector3 vector3_1 = p2 - 3f * ss2 + 3f * ss1 - p1;
    Vector3 vector3_2 = 3f * ss2 - 6f * ss1 + 3f * p1;
    Vector3 vector3_3 = 3f * ss1 - 3f * p1;
    Vector3 vector3_4 = p1;
    return vector3_1 * Mathf.Pow(t, 3f) + vector3_2 * Mathf.Pow(t, 2f) + vector3_3 * t + vector3_4;
  }

  [Serializable]
  public struct Packet
  {
    public int Time;
    public Vector3 Pos;
    public Vector3 S1;
    public Vector3 S2;
    public float GameTime;

    public Packet(int t, Vector3 p, Vector3 prev)
    {
      this.Time = t;
      this.Pos = p;
      this.S1 = prev * 0.3f;
      this.S2 = -prev * 0.3f;
      this.GameTime = Time.realtimeSinceStartup;
    }
  }
}
