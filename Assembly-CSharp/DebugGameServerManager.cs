using UnityEngine;

public class DebugGameServerManager : IDebugPage
{
	public string Title => "Requests";

	public void Draw()
	{
		foreach (ServerLoadRequest serverRequest in Singleton<GameServerManager>.Instance.ServerRequests)
		{
			GUILayout.Label(serverRequest.Server.Name + " " + serverRequest.Server.ConnectionString + ", Latency: " + serverRequest.Server.Latency.ToString() + " - " + serverRequest.Server.IsValid.ToString());
			GUILayout.Label("States: " + serverRequest.RequestState.ToString() + " " + serverRequest.Server.Data.State.ToString() + ", PeerState: " + serverRequest.Peer.PeerState.ToString());
			GUILayout.Space(10f);
		}
	}
}
