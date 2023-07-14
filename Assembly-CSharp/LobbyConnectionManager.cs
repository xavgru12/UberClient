// Decompiled with JetBrains decompiler
// Type: LobbyConnectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class LobbyConnectionManager : AutoMonoBehaviour<LobbyConnectionManager>
{
  private float _syncTime;
  private PhotonClient _client;
  private ClientLobbyCenter _lobbyCenter;

  private void Awake()
  {
    this._client = new PhotonClient(true);
    this._lobbyCenter = new ClientLobbyCenter(this._client.Rmi);
    this._client.PeerListener.SubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.OnEventCallback));
  }

  private void OnEventCallback(PhotonPeerListener.ConnectionEvent ev)
  {
    if (ev.Type != PhotonPeerListener.ConnectionEventType.Disconnected)
      return;
    Singleton<GameListManager>.Instance.ClearGameList();
  }

  public static void StartConnection()
  {
    if (AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.IsConnected || !CmuneNetworkManager.CurrentLobbyServer.IsValid)
      return;
    AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.ConnectToRoom(new RoomMetaData(66, "The Lobby", CmuneNetworkManager.CurrentLobbyServer.ConnectionString), PlayerDataManager.CmidSecure, PlayerDataManager.AccessLevelSecure);
  }

  public static void Reconnect()
  {
    LobbyConnectionManager.Stop();
    AutoMonoBehaviour<LobbyConnectionManager>.Instance.Awake();
  }

  public static void Stop()
  {
    if (!AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.IsConnected)
      return;
    AutoMonoBehaviour<LobbyConnectionManager>.Instance._lobbyCenter.Leave();
    AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.Disconnect();
  }

  private void Update()
  {
    if (this._client == null || (double) this._syncTime > (double) Time.time)
      return;
    this._syncTime = Time.time + 0.02f;
    this._client.Update();
  }

  protected void OnApplicationQuit()
  {
    if (this._client == null)
      return;
    this._client.ShutDown();
  }

  public static RemoteMethodInterface Rmi => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.Rmi;

  public static PhotonClient Client => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client;

  public static int CurrentPlayerID => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.PeerListener.ActorId;

  public static bool IsConnected => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.IsConnected;

  public static bool IsConnecting => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.ConnectionState == PhotonClient.ConnectionStatus.STARTING;

  public static bool IsInLobby => AutoMonoBehaviour<LobbyConnectionManager>.Instance._client.PeerListener.HasJoinedRoom;
}
