// Decompiled with JetBrains decompiler
// Type: DebugServerState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugServerState : IDebugPage
{
  public string Title => "Servers";

  public void Draw()
  {
    GUILayout.Label("ALL SERVERS");
    foreach (GameServerView photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
      GUILayout.Label("  " + photonServer.ConnectionString + " " + (object) photonServer.Latency);
    if (Singleton<GameServerController>.Instance.SelectedServer != null)
    {
      GUILayout.Space(10f);
      GUILayout.Label(string.Format("GAMESERVER: {0}, isValid: {1}", (object) Singleton<GameServerController>.Instance.SelectedServer.ConnectionString, (object) Singleton<GameServerController>.Instance.SelectedServer.IsValid));
      GUILayout.Label("  Room ID: " + GameConnectionManager.CurrentRoomID);
      GUILayout.Label("  Player ID: " + (object) GameConnectionManager.CurrentPlayerID);
      GUILayout.Label("  Network Time: " + (object) GameConnectionManager.Client.PeerListener.ServerTimeTicks);
      GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(GameConnectionManager.Client.PeerListener.IncomingBytes).ToString("f2"));
      GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(GameConnectionManager.Client.PeerListener.OutgoingBytes).ToString("f2"));
    }
    if (CmuneNetworkManager.CurrentLobbyServer != null)
    {
      GUILayout.Space(10f);
      GUILayout.Label(string.Format("LOBBYSERVER: {0}, isValid: {1}", (object) CmuneNetworkManager.CurrentLobbyServer.ConnectionString, (object) CmuneNetworkManager.CurrentLobbyServer.IsValid));
      GUILayout.Label("  Player ID: " + (object) LobbyConnectionManager.CurrentPlayerID);
      GUILayout.Label("  Network Time: " + (object) LobbyConnectionManager.Client.PeerListener.ServerTimeTicks);
      GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(LobbyConnectionManager.Client.PeerListener.IncomingBytes).ToString("f2"));
      GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(LobbyConnectionManager.Client.PeerListener.OutgoingBytes).ToString("f2"));
    }
    if (CmuneNetworkManager.CurrentCommServer == null)
      return;
    GUILayout.Space(10f);
    GUILayout.Label(string.Format("COMMSERVER: {0}, isValid: {1}", (object) CmuneNetworkManager.CurrentCommServer.ConnectionString, (object) CmuneNetworkManager.CurrentCommServer.IsValid));
    GUILayout.Label("  Player ID: " + (object) CommConnectionManager.CurrentPlayerID);
    GUILayout.Label("  Network Time: " + (object) CommConnectionManager.Client.PeerListener.ServerTimeTicks);
    GUILayout.Label("  KBytes IN: " + ConvertBytes.ToKiloBytes(CommConnectionManager.Client.PeerListener.IncomingBytes).ToString("f2"));
    GUILayout.Label("  KBytes OUT: " + ConvertBytes.ToKiloBytes(CommConnectionManager.Client.PeerListener.OutgoingBytes).ToString("f2"));
  }
}
