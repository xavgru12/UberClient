using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugServerState : IDebugPage
{
	public string Title => "Network";

	public void Draw()
	{
		GUILayout.Space(10f);
		GUILayout.Label("GAME: " + Singleton<GameStateController>.Instance.Client.Peer.ServerAddress);
		GUILayout.Label("  PeerState: " + Singleton<GameStateController>.Instance.Client.Peer.PeerState.ToString());
		GUILayout.Label("  InRoom: " + Singleton<GameStateController>.Instance.Client.IsInsideRoom.ToString());
		GUILayout.Label("  Network Time: " + Singleton<GameStateController>.Instance.Client.Peer.ServerTimeInMilliSeconds.ToString());
		GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(Singleton<GameStateController>.Instance.Client.Peer.BytesIn).ToString("f2"));
		GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(Singleton<GameStateController>.Instance.Client.Peer.BytesOut).ToString("f2"));
		GUILayout.Space(10f);
		GUILayout.Label("COMM: " + AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.ServerAddress);
		GUILayout.Label("  PeerState: " + AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.PeerState.ToString());
		GUILayout.Label("  Network Time: " + AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.ServerTimeInMilliSeconds.ToString());
		GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.BytesIn).ToString("f2"));
		GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Peer.BytesOut).ToString("f2"));
		GUILayout.Label("ALL SERVERS");
		foreach (PhotonServer photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
		{
			GUILayout.Label("  " + photonServer.ConnectionString + " " + photonServer.Latency.ToString());
		}
	}
}
