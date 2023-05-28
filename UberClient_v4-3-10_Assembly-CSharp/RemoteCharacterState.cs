// Decompiled with JetBrains decompiler
// Type: RemoteCharacterState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class RemoteCharacterState : ICharacterState
{
  private int _counter;
  private int _lastInterpolation;
  private BezierSplines _interpolator;
  private UberStrike.Realtime.UnitySdk.CharacterInfo _currentState;

  public RemoteCharacterState(UberStrike.Realtime.UnitySdk.CharacterInfo info)
  {
    this._currentState = info;
    this._interpolator = new BezierSplines();
    this.SetHardPosition(GameConnectionManager.Client.PeerListener.ServerTimeTicks, info.Position);
  }

  private event Action<SyncObject> _updateRecievedEvent;

  public UberStrike.Realtime.UnitySdk.CharacterInfo Info => this._currentState;

  public Vector3 LastPosition => this._interpolator.LatestPosition();

  public BezierSplines GetPositionInterpolator() => this._interpolator;

  public void RecieveDeltaUpdate(SyncObject delta)
  {
    this._currentState.ReadSyncData(delta);
    if (this._updateRecievedEvent == null)
      return;
    this._updateRecievedEvent(delta);
  }

  public void SubscribeToEvents(CharacterConfig config)
  {
    this._updateRecievedEvent = (Action<SyncObject>) null;
    this._updateRecievedEvent += new Action<SyncObject>(config.OnCharacterStateUpdated);
  }

  public void UnSubscribeAll() => this._updateRecievedEvent = (Action<SyncObject>) null;

  public void UpdatePositionSmooth(PlayerPosition p)
  {
    ++this._counter;
    this._interpolator.AddSample(p.Time, p.Position);
  }

  public void SetHardPosition(int time, Vector3 pos)
  {
    this._interpolator.Packets.Clear();
    this._interpolator.LastPacket.Time = 0;
    this._interpolator.AddSample(time, pos);
    this._interpolator.PreviousPacket = this._interpolator.LastPacket;
    this._currentState.Position = pos;
  }

  public void Interpolate(int time)
  {
    if (!this._currentState.IsAlive)
      return;
    Vector3 oPos;
    this._lastInterpolation = this._interpolator.ReadPosition(time, out oPos);
    if (this._lastInterpolation <= 0)
      return;
    this._currentState.Position = oPos;
    this._currentState.Distance = Vector3.Distance(this._interpolator.LastPacket.Pos, oPos);
  }

  internal void UpdatePosition()
  {
    Vector3 vector3 = this._interpolator.LastPacket.Pos - this._interpolator.PreviousPacket.Pos;
    this._currentState.Position = Vector3.Lerp(this._currentState.Position, this._interpolator.PreviousPacket.Pos + vector3 * Time.deltaTime, Time.deltaTime * 10f);
    this._currentState.Position.y = Mathf.Lerp(this._currentState.Position.y, this._interpolator.LastPacket.Pos.y, Time.deltaTime * 10f);
    this._currentState.Velocity = vector3.magnitude;
    this._currentState.Distance = Vector3.Distance(this._interpolator.LastPacket.Pos, this._currentState.Position);
  }

  public string DebugAll()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("time: {0}/{1} ", (object) this._lastInterpolation, (object) this._interpolator.Packets.Count);
    stringBuilder.AppendFormat("cntr: {0}\n", (object) this._counter);
    return stringBuilder.ToString();
  }
}
