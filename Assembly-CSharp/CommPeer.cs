using Cmune.DataCenter.Common.Entities;
using ExitGames.Client.Photon;
using UberStrike.Core.Models;
using UberStrike.Core.ViewModel;
using UberStrike.Realtime.Client;
using UnityEngine;

public class CommPeer : BaseCommPeer
{
	public LobbyRoom Lobby
	{
		get;
		private set;
	}

	public CommPeer()
		: base(100, Application.isEditor)
	{
		Lobby = new LobbyRoom();
		AddRoomLogic(Lobby, Lobby.Operations);
	}

	public CommPeer(int syncFrequency, bool monitorTraffic = false) : base(syncFrequency, monitorTraffic)
	{
	}

	protected override void OnConnected()
	{
		if (PlayerDataManager.IsPlayerLoggedIn)
		{
			base.Operations.SendAuthenticationRequest(PlayerDataManager.AuthToken, PlayerDataManager.MagicHash, ApplicationDataManager.IsMac);
			Singleton<ChatManager>.Instance.UpdateFriendSection();
		}
	}

	protected override void OnDisconnected(StatusCode status)
	{
	}

	protected override void OnError(string message)
	{
	}

	protected override void OnLoadData(ServerConnectionView data)
	{
	}

	protected override void OnLobbyEntered()
	{
		Lobby.SendContactList();
		if (GameState.Current.RoomData != null && GameState.Current.RoomData.Server != null)
		{
			Lobby.UpdatePlayerRoom(new GameRoom
			{
				Server = new ConnectionAddress(GameState.Current.RoomData.Server.ConnectionString),
				Number = GameState.Current.RoomData.Number,
				MapId = GameState.Current.RoomData.MapID
			});
		}
	}

	protected override void OnDisconnectAndDisablePhoton(string message)
	{
		AutoMonoBehaviour<CommConnectionManager>.Instance.DisableNetworkConnection(message);
	}

	protected override void OnHeartbeatChallenge(string challengeHash)
	{
		string responseHash = Heartbeat.Instance.CheckHeartbeat(challengeHash);
		UberBeat.ChildThreadWait.Set();
		base.Operations.SendSendHeartbeatResponse(PlayerDataManager.AuthToken, responseHash);
	}

	protected override void OnLoadoutUpdateResult(MemberOperationResult result)
	{
		PlayerDataManager.result = result;
		PlayerDataManager.IsLoadoutUpdating = false;
	}
}
