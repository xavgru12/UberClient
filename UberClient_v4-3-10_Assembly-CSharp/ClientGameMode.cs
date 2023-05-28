// Decompiled with JetBrains decompiler
// Type: ClientGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public abstract class ClientGameMode : ClientNetworkClass, IGameMode
{
  private Dictionary<int, UberStrike.Realtime.UnitySdk.CharacterInfo> _players;
  private GameMetaData _gameData;

  protected ClientGameMode(RemoteMethodInterface rmi, GameMetaData gameData)
    : base(rmi)
  {
    this._gameData = gameData;
    this._players = new Dictionary<int, UberStrike.Realtime.UnitySdk.CharacterInfo>();
  }

  public UberStrike.Realtime.UnitySdk.CharacterInfo GetPlayerWithID(int actorId)
  {
    if (actorId == GameState.LocalCharacter.ActorId)
      return GameState.LocalCharacter;
    UberStrike.Realtime.UnitySdk.CharacterInfo playerWithId;
    if (this.Players.TryGetValue(actorId, out playerWithId))
      ;
    return playerWithId;
  }

  [NetworkMethod(1)]
  protected virtual void OnPlayerJoined(SyncObject data, Vector3 position)
  {
    if (data.IsEmpty)
      Debug.LogError((object) "ClientGameMode: OnPlayerJoined - SyncObject is empty!");
    else if (data.Id == GameState.LocalCharacter.ActorId)
    {
      GameState.LocalCharacter.ReadSyncData(data);
      if (GameState.LocalCharacter.IsSpectator)
        return;
      this.Players[data.Id] = GameState.LocalCharacter;
      this.HasJoinedGame = true;
    }
    else
    {
      this.Players[data.Id] = new UberStrike.Realtime.UnitySdk.CharacterInfo(data);
      this.Players[data.Id].Position = position;
    }
  }

  [NetworkMethod(2)]
  protected virtual void OnPlayerLeft(int actorId)
  {
    if (!this.Players.Remove(actorId))
      ;
    if (actorId != this.MyActorId)
      return;
    this.HasJoinedGame = false;
  }

  public bool HasJoinedGame { get; protected set; }

  [NetworkMethod(4)]
  protected virtual void OnFullPlayerListUpdate(List<SyncObject> data, List<Vector3> positions)
  {
    for (int index = 0; index < data.Count && index < positions.Count; ++index)
      this.OnPlayerJoined(data[index], positions[index]);
  }

  [NetworkMethod(5)]
  protected virtual void OnGameFrameUpdate(List<SyncObject> data)
  {
    foreach (SyncObject syncObject in data)
    {
      UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo;
      if (!syncObject.IsEmpty && this.Players.TryGetValue(syncObject.Id, out characterInfo))
        characterInfo.ReadSyncData(syncObject);
    }
  }

  [NetworkMethod(3)]
  protected virtual void OnPlayerUpdate(SyncObject data)
  {
    UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo;
    if (data.IsEmpty || !this.Players.TryGetValue(data.Id, out characterInfo))
      return;
    characterInfo.ReadSyncData(data);
  }

  [NetworkMethod(21)]
  protected virtual void OnStartGame() => this.IsGameStarted = true;

  [NetworkMethod(22)]
  protected virtual void OnStopGame() => this.IsGameStarted = false;

  public bool IsMatchRunning { get; protected set; }

  public bool IsGameStarted { get; private set; }

  public GameMetaData GameData => this._gameData;

  public Dictionary<int, UberStrike.Realtime.UnitySdk.CharacterInfo> Players => this._players;

  public int MyActorId => this._rmi.Messenger.PeerListener.ActorId;
}
