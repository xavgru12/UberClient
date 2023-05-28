// Decompiled with JetBrains decompiler
// Type: GameStateInterpolator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameStateInterpolator
{
  private Dictionary<int, RemoteCharacterState> _remoteStateByID;
  private Dictionary<byte, RemoteCharacterState> _remoteStateByNumber;
  private float _timeDelta = 0.01f;
  private GameStateInterpolator.INTERPOLATION_STATE _internalState;

  public GameStateInterpolator()
  {
    this._remoteStateByID = new Dictionary<int, RemoteCharacterState>(20);
    this._remoteStateByNumber = new Dictionary<byte, RemoteCharacterState>(20);
    this._internalState = GameStateInterpolator.INTERPOLATION_STATE.PAUSED;
  }

  public void Interpolate()
  {
    if (this._internalState != GameStateInterpolator.INTERPOLATION_STATE.RUNNING)
      return;
    foreach (RemoteCharacterState remoteCharacterState in this._remoteStateByID.Values)
      remoteCharacterState.Interpolate(GameConnectionManager.Client.PeerListener.ServerTimeTicks - 500);
  }

  public void Run() => this._internalState = GameStateInterpolator.INTERPOLATION_STATE.RUNNING;

  public void Pause() => this._internalState = GameStateInterpolator.INTERPOLATION_STATE.PAUSED;

  public void UpdateCharacterInfo(SyncObject update)
  {
    RemoteCharacterState remoteCharacterState;
    if (this._remoteStateByID.TryGetValue(update.Id, out remoteCharacterState))
      remoteCharacterState.RecieveDeltaUpdate(update);
    else
      Debug.LogWarning((object) ("UpdateUberStrike.Realtime.UnitySdk.CharacterInfo but state not found for actor " + (object) update.Id));
  }

  public void UpdatePositionSmooth(List<PlayerPosition> all)
  {
    foreach (PlayerPosition p in all)
    {
      RemoteCharacterState remoteCharacterState;
      if (this._remoteStateByNumber.TryGetValue(p.Player, out remoteCharacterState))
        remoteCharacterState.UpdatePositionSmooth(p);
    }
  }

  public void UpdatePositionHard(byte playerNumber, Vector3 pos)
  {
    RemoteCharacterState remoteCharacterState;
    if (this._remoteStateByNumber.TryGetValue(playerNumber, out remoteCharacterState))
      remoteCharacterState.SetHardPosition(GameConnectionManager.Client.PeerListener.ServerTimeTicks, pos);
    else
      Debug.LogWarning((object) ("UpdatePositionSmooth failed for " + (object) playerNumber + " " + (object) pos));
  }

  public void AddCharacterInfo(UberStrike.Realtime.UnitySdk.CharacterInfo user)
  {
    this._remoteStateByID[user.ActorId] = new RemoteCharacterState(user);
    this._remoteStateByNumber[user.PlayerNumber] = this._remoteStateByID[user.ActorId];
  }

  public void RemoveCharacterInfo(int playerID)
  {
    RemoteCharacterState remoteCharacterState;
    if (!this._remoteStateByID.TryGetValue(playerID, out remoteCharacterState))
      return;
    this._remoteStateByNumber.Remove(remoteCharacterState.Info.PlayerNumber);
  }

  public RemoteCharacterState GetState(int playerID)
  {
    RemoteCharacterState state;
    if (this._remoteStateByID.TryGetValue(playerID, out state))
      return state;
    throw new Exception(string.Format("GameStateInterpolator:GetPlayerState({0}) failed because CharacterState was not inserted", (object) playerID));
  }

  public float SimulationFrequency
  {
    get => this._timeDelta;
    set => this._timeDelta = value;
  }

  public enum INTERPOLATION_STATE
  {
    RUNNING,
    PAUSED,
  }
}
