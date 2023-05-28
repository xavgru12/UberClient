// Decompiled with JetBrains decompiler
// Type: GameListManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameListManager : Singleton<GameListManager>
{
  private Dictionary<CmuneRoomID, GameMetaData> _gameList = new Dictionary<CmuneRoomID, GameMetaData>();

  private GameListManager() => CmuneEventHandler.AddListener<RoomListUpdatedEvent>(new Action<RoomListUpdatedEvent>(this.OnGameListUpdated));

  public int PlayersCount { get; private set; }

  public IEnumerable<GameMetaData> GameList => (IEnumerable<GameMetaData>) this._gameList.Values;

  public int GamesCount => this._gameList.Count;

  public void Init()
  {
  }

  private void OnGameListUpdated(RoomListUpdatedEvent ev)
  {
    this.PlayersCount = 0;
    this._gameList.Clear();
    foreach (GameMetaData room in (List<RoomMetaData>) ev.Rooms)
    {
      if (room != null)
      {
        this.PlayersCount += room.ConnectedPlayers;
        room.Latency = Singleton<GameServerManager>.Instance.GetServerLatency(room.ServerConnection);
        this._gameList[room.RoomID] = room;
      }
    }
    if (!ev.IsInitialList || !((UnityEngine.Object) PlayPageGUI.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) MenuPageManager.Instance != (UnityEngine.Object) null & MenuPageManager.Instance.IsCurrentPage(PageType.Play)))
      return;
    PlayPageGUI.Instance.RefreshGameList();
  }

  public void UpdateServerLatency(string serverConnection)
  {
    foreach (GameMetaData gameMetaData in this._gameList.Values)
    {
      if (gameMetaData.ServerConnection == serverConnection)
        gameMetaData.Latency = Singleton<GameServerManager>.Instance.GetServerLatency(serverConnection);
    }
  }

  public void ClearGameList() => this._gameList.Clear();

  public bool TryGetGame(CmuneRoomID id, out GameMetaData game) => this._gameList.TryGetValue(id, out game);
}
