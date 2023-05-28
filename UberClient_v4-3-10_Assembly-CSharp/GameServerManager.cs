// Decompiled with JetBrains decompiler
// Type: GameServerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class GameServerManager : Singleton<GameServerManager>
{
  private const int ServerUpdateCycle = 30;
  private Dictionary<int, GameServerView> _gameServers = new Dictionary<int, GameServerView>();
  private List<GameServerView> _sortedServers = new List<GameServerView>();
  private IComparer<GameServerView> _comparer;
  private bool _reverseSorting;
  private Dictionary<int, ServerLoadRequest> _loadRequests = new Dictionary<int, ServerLoadRequest>();

  private GameServerManager()
  {
  }

  public int PhotonServerCount => this._gameServers.Count;

  public int AllPlayersCount { get; private set; }

  public int AllGamesCount { get; private set; }

  public IEnumerable<GameServerView> PhotonServerList => (IEnumerable<GameServerView>) this._sortedServers;

  public IEnumerable<ServerLoadRequest> ServerRequests => (IEnumerable<ServerLoadRequest>) this._loadRequests.Values;

  public void SortServers()
  {
    if (this._comparer == null)
      return;
    this._sortedServers.Sort(this._comparer);
    if (!this._reverseSorting)
      return;
    this._sortedServers.Reverse();
  }

  public GameServerView GetBestServer()
  {
    GameServerView bestServer = this.GetBestServer(ApplicationDataManager.IsMobile);
    if (ApplicationDataManager.IsMobile && bestServer == null)
      bestServer = this.GetBestServer(false);
    return bestServer;
  }

  private GameServerView GetBestServer(bool doMobileFilter)
  {
    List<GameServerView> gameServerViewList = new List<GameServerView>((IEnumerable<GameServerView>) this._gameServers.Values);
    gameServerViewList.Sort((Comparison<GameServerView>) ((s, t) => s.Latency - t.Latency));
    GameServerView bestServer = (GameServerView) null;
    for (int index = 0; index < gameServerViewList.Count; ++index)
    {
      GameServerView gameServerView = gameServerViewList[index];
      if (gameServerView.Latency != 0 && (!doMobileFilter || gameServerView.UsageType == PhotonUsageType.Mobile))
      {
        if (bestServer == null && gameServerView.CheckLatency())
          bestServer = gameServerView;
        else if (gameServerView.CheckLatency() && gameServerView.Latency < 200 && bestServer.Data.PlayersConnected < gameServerView.Data.PlayersConnected)
          bestServer = gameServerView;
      }
    }
    return bestServer;
  }

  internal string GetServerName(string connection)
  {
    string serverName = string.Empty;
    foreach (GameServerView gameServerView in this._gameServers.Values)
    {
      if (gameServerView.ConnectionString == connection)
      {
        serverName = gameServerView.Name;
        break;
      }
    }
    return serverName;
  }

  public void SortServers(IComparer<GameServerView> comparer, bool reverse = false)
  {
    this._comparer = comparer;
    this._reverseSorting = reverse;
    lock (this._sortedServers)
    {
      this._sortedServers.Clear();
      this._sortedServers.AddRange((IEnumerable<GameServerView>) this._gameServers.Values);
    }
    this.SortServers();
  }

  public void AddGameServer(PhotonView view)
  {
    this._gameServers[view.PhotonId] = new GameServerView(view);
    if (view.MinLatency > 0)
      view.Name = view.Name + " - " + (object) view.MinLatency + "ms";
    this.SortServers();
  }

  public int GetServerLatency(string connection)
  {
    foreach (GameServerView gameServerView in this._gameServers.Values)
    {
      if (gameServerView.ConnectionString == connection)
        return gameServerView.Latency;
    }
    return 0;
  }

  [DebuggerHidden]
  public IEnumerator StartUpdatingServerLoads() => (IEnumerator) new GameServerManager.\u003CStartUpdatingServerLoads\u003Ec__Iterator6B()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartUpdatingLatency(Action<float> progressCallback) => (IEnumerator) new GameServerManager.\u003CStartUpdatingLatency\u003Ec__Iterator6C()
  {
    progressCallback = progressCallback,
    \u003C\u0024\u003EprogressCallback = progressCallback,
    \u003C\u003Ef__this = this
  };

  private void UpdateGamesAndPlayerCount()
  {
    this.AllPlayersCount = 0;
    this.AllGamesCount = 0;
    foreach (GameServerView gameServerView in this._gameServers.Values)
    {
      this.AllPlayersCount += gameServerView.Data.PlayersConnected;
      this.AllGamesCount += gameServerView.Data.RoomsCreated;
    }
    this.SortServers();
  }
}
