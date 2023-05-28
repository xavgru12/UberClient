// Decompiled with JetBrains decompiler
// Type: DebugConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugConnection : IDebugPage
{
  public string Title => "Connection";

  public void Draw()
  {
    if (GUI.Button(new Rect(20f, 70f, 150f, 20f), "Comm Disconnect"))
      CommConnectionManager.Stop();
    if (GUI.Button(new Rect(180f, 70f, 150f, 20f), "Comm Connect"))
      CommConnectionManager.Client.ConnectToRoom(new RoomMetaData(88, string.Empty, CmuneNetworkManager.CurrentCommServer.ConnectionString), PlayerDataManager.CmidSecure, PlayerDataManager.AccessLevelSecure);
    GUI.Label(new Rect(360f, 70f, 600f, 20f), CommConnectionManager.Client.Debug);
    if (GUI.Button(new Rect(20f, 100f, 150f, 20f), "Lobby Disconnect"))
      LobbyConnectionManager.Stop();
    if (GUI.Button(new Rect(180f, 100f, 150f, 20f), "Lobby Connect"))
      LobbyConnectionManager.Client.ConnectToRoom(new RoomMetaData(66, string.Empty, CmuneNetworkManager.CurrentLobbyServer.ConnectionString), PlayerDataManager.CmidSecure, PlayerDataManager.AccessLevelSecure);
    GUI.Label(new Rect(360f, 100f, 600f, 20f), LobbyConnectionManager.Client.Debug);
    if (GUI.Button(new Rect(20f, 130f, 150f, 20f), "Game Disconnect"))
      GameConnectionManager.Stop();
    if (GUI.Button(new Rect(180f, 130f, 150f, 20f), "Game Connect"))
      GameConnectionManager.Client.ConnectToRoom((RoomMetaData) new GameMetaData(0, string.Empty, Singleton<GameServerController>.Instance.SelectedServer.ConnectionString), PlayerDataManager.CmidSecure, PlayerDataManager.AccessLevelSecure);
    GUI.Label(new Rect(360f, 130f, 600f, 20f), GameConnectionManager.Client.Debug);
  }
}
