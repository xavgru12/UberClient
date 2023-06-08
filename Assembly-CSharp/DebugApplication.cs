using Cmune.DataCenter.Common.Entities;
using UberStrike.DataCenter.UnitySdk;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugApplication : IDebugPage
{
	public string Title => "App";

	public void Draw()
	{
		GUILayout.Label("Channel: " + ApplicationDataManager.Channel.ToString());
		GUILayout.Label("Version: " + ApplicationDataManager.Version);
		GUILayout.Label("Source: " + Application.srcValue);
		GUILayout.Label("WS API: " + UberStrike.DataCenter.UnitySdk.ApiVersion.Current);
		GUILayout.Label("RT API: " + UberStrike.Realtime.UnitySdk.ApiVersion.Current);
		if (PlayerDataManager.AccessLevel > MemberAccessLevel.Default)
		{
			GUILayout.Label("Member Name: " + PlayerDataManager.Name);
			GUILayout.Label("Member Cmid: " + PlayerDataManager.Cmid.ToString());
			GUILayout.Label("Member Access: " + PlayerDataManager.AccessLevel.ToString());
			GUILayout.Label("Member Tag: " + PlayerDataManager.ClanTag);
			foreach (PhotonServer photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
			{
				GUILayout.Label("Game Server: " + photonServer.Name + " [" + photonServer.MinLatency.ToString() + "] " + photonServer.Data.PeersConnected.ToString() + "/" + photonServer.Data.PlayersConnected.ToString());
			}
		}
		GUILayout.Space(10f);
	}
}
