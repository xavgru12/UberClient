using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class GameServerManager : Singleton<GameServerManager>
{
	private const int ServerUpdateCycle = 30;

	private Dictionary<int, PhotonServer> _gameServers = new Dictionary<int, PhotonServer>();

	public PhotonServer CommServer = PhotonServer.Empty;

	private List<PhotonServer> _sortedServers = new List<PhotonServer>();

	private IComparer<PhotonServer> _comparer;

	private bool _reverseSorting;

	private Dictionary<int, ServerLoadRequest> _loadRequests = new Dictionary<int, ServerLoadRequest>();

	public int PhotonServerCount => _gameServers.Count;

	public int AllPlayersCount
	{
		get;
		private set;
	}

	public int AllGamesCount
	{
		get;
		private set;
	}

	public IEnumerable<PhotonServer> PhotonServerList => _sortedServers;

	public IEnumerable<ServerLoadRequest> ServerRequests => _loadRequests.Values;

	private GameServerManager()
	{
	}

	public void SortServers()
	{
		if (_comparer != null)
		{
			_sortedServers.Sort(_comparer);
			if (_reverseSorting)
			{
				_sortedServers.Reverse();
			}
		}
	}

	public PhotonServer GetBestServer()
	{
		PhotonServer bestServer = GetBestServer(ApplicationDataManager.IsMobile);
		if (ApplicationDataManager.IsMobile && bestServer == null)
		{
			bestServer = GetBestServer(doMobileFilter: false);
		}
		return bestServer;
	}

	private PhotonServer GetBestServer(bool doMobileFilter)
	{
		List<PhotonServer> list = new List<PhotonServer>(_gameServers.Values);
		list.Sort((PhotonServer s, PhotonServer t) => s.Latency - t.Latency);
		PhotonServer photonServer = null;
		for (int i = 0; i < list.Count; i++)
		{
			PhotonServer photonServer2 = list[i];
			if (photonServer2.Latency != 0 && (!doMobileFilter || photonServer2.UsageType == PhotonUsageType.Mobile))
			{
				if (photonServer == null && photonServer2.CheckLatency())
				{
					photonServer = photonServer2;
				}
				else if (photonServer2.CheckLatency() && photonServer2.Latency < 200 && photonServer.Data.PlayersConnected < photonServer2.Data.PlayersConnected)
				{
					photonServer = photonServer2;
				}
			}
		}
		return photonServer;
	}

	internal string GetServerName(GameRoomData room)
	{
		string empty = string.Empty;
		if (room != null && room.Server != null)
		{
			foreach (PhotonServer value in _gameServers.Values)
			{
				if (value.ConnectionString == room.Server.ConnectionString)
				{
					return value.Name;
				}
			}
			return empty;
		}
		return empty;
	}

	public void SortServers(IComparer<PhotonServer> comparer, bool reverse = false)
	{
		_comparer = comparer;
		_reverseSorting = reverse;
		lock (_sortedServers)
		{
			_sortedServers.Clear();
			_sortedServers.AddRange(_gameServers.Values);
		}
		SortServers();
	}

	public void AddTestPhotonGameServer(int id, PhotonServer photonServer)
	{
		_gameServers[id] = photonServer;
	}

	public void AddPhotonGameServer(PhotonView view)
	{
		_gameServers[view.PhotonId] = new PhotonServer(view);
		if (view.MinLatency > 0)
		{
			view.Name = view.Name + " - " + view.MinLatency.ToString() + "ms";
		}
		SortServers();
	}

	public void AddPhotonGameServers(List<PhotonView> servers)
	{
		foreach (PhotonView server in servers)
		{
			AddPhotonGameServer(server);
		}
	}

	public int GetServerLatency(string connection)
	{
		foreach (PhotonServer value in _gameServers.Values)
		{
			if (value.ConnectionString == connection)
			{
				return value.Latency;
			}
		}
		return 0;
	}

	public IEnumerator StartUpdatingServerLoads()
	{
		foreach (PhotonServer value2 in _gameServers.Values)
		{
			if (!_loadRequests.TryGetValue(value2.Id, out ServerLoadRequest value))
			{
				value = ServerLoadRequest.Run(value2, delegate
				{
					UpdateGamesAndPlayerCount();
				});
				_loadRequests.Add(value2.Id, value);
			}
			if (value.RequestState != ServerLoadRequest.RequestStateType.Waiting)
			{
				value.Run();
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator StartUpdatingLatency(Action<float> progressCallback)
	{
		yield return UnityRuntime.StartRoutine(StartUpdatingServerLoads());
		float minTimeout = Time.time + 4f;
		float maxTimeout = Time.time + 10f;
		int num = 0;
		while (num != _loadRequests.Count)
		{
			yield return new WaitForSeconds(1f);
			num = 0;
			foreach (ServerLoadRequest value in _loadRequests.Values)
			{
				if (value.RequestState != ServerLoadRequest.RequestStateType.Waiting)
				{
					num++;
				}
			}
			progressCallback((float)num / (float)_loadRequests.Count);
			if ((num > 0 && Time.time > minTimeout) || Time.time > maxTimeout)
			{
				break;
			}
		}
	}

	private void UpdateGamesAndPlayerCount()
	{
		AllPlayersCount = 0;
		AllGamesCount = 0;
		foreach (PhotonServer value in _gameServers.Values)
		{
			AllPlayersCount += value.Data.PlayersConnected;
			AllGamesCount += value.Data.RoomsCreated;
		}
		SortServers();
	}
}
