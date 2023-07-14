// Decompiled with JetBrains decompiler
// Type: GameConnectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameConnectionManager : AutoMonoBehaviour<GameConnectionManager>
{
  private float _syncTime;
  private float _joinFullHackTime;
  private bool _reponseArrived;
  private bool _isConnectionStarted;
  private PhotonClient _client;
  private GameMetaData _gameRoom;
  private GameMetaData _requestedGameData = GameMetaData.Empty;

  private void Awake()
  {
    this._client = new PhotonClient(true);
    this._client.PeerListener.SubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.OnEventCallback));
  }

  private void Update()
  {
    if (this._client == null || (double) this._syncTime > (double) Time.realtimeSinceStartup)
      return;
    this._syncTime = Time.realtimeSinceStartup + 0.02f;
    this._client.Update();
  }

  private void OnGUI()
  {
    if (!GameState.HasCurrentGame || GameState.CurrentGame.NetworkID == (short) -1 || GameConnectionManager.Client == null || GameConnectionManager.Client.ConnectionState == PhotonClient.ConnectionStatus.RUNNING)
      return;
    GUI.BeginGroup(new Rect((float) (Screen.width - 320) * 0.5f, (float) (Screen.height - 240 - 56) * 0.5f, 320f, 240f), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(0.0f, 0.0f, 320f, 56f), LocalizedStrings.PleaseWait, BlueStonez.tab_strip);
    if (GameConnectionManager.Client.ConnectionState == PhotonClient.ConnectionStatus.STARTING)
      GUI.Button(new Rect(17f, 55f, 286f, 140f), LocalizedStrings.ConnectingToServer, BlueStonez.label_interparkbold_11pt);
    else if (GameConnectionManager.Client.ConnectionState == PhotonClient.ConnectionStatus.STOPPED)
    {
      GUI.Button(new Rect(17f, 55f, 286f, 140f), LocalizedStrings.ServerError, BlueStonez.label_interparkbold_11pt);
      if (GUITools.Button(new Rect(100f, 200f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button))
        Singleton<GameStateController>.Instance.UnloadGameMode();
    }
    GUI.EndGroup();
  }

  private void Reconnect() => this.StartCoroutine(this.StartReconnectionInSeconds(1));

  private void CloseGame() => Singleton<GameStateController>.Instance.LeaveGame();

  private void CloseGameAndBackToServerSelection()
  {
    Singleton<GameServerController>.Instance.SelectedServer = (GameServerView) null;
    Singleton<GameStateController>.Instance.LeaveGame();
  }

  private void OnRequestRoomMetaDataCallback(int result, object[] table)
  {
    this._reponseArrived = true;
    if (table.Length <= 0)
      return;
    this._requestedGameData = (GameMetaData) table[0];
  }

  private void OnEventCallback(PhotonPeerListener.ConnectionEvent ev)
  {
    switch (ev.Type)
    {
      case PhotonPeerListener.ConnectionEventType.Disconnected:
        if (!this._isConnectionStarted || (double) this._joinFullHackTime >= (double) Time.time)
          break;
        PopupSystem.ShowMessage("Connection Error", "You lost the connection to our server!\nDo you want to reconnect?", PopupSystem.AlertType.OKCancel, new Action(this.Reconnect), new Action(this.CloseGameAndBackToServerSelection));
        Screen.lockCursor = false;
        break;
      case PhotonPeerListener.ConnectionEventType.JoinedRoom:
        if (this._gameRoom == null)
          break;
        this._gameRoom.RoomID = ev.Room;
        break;
      case PhotonPeerListener.ConnectionEventType.JoinFailed:
        this._joinFullHackTime = Time.time + 2f;
        if (ev.ErrorCode == 4)
        {
          PopupSystem.ShowMessage("Server Full", "The server is currently full!\nDo you want to try again?", PopupSystem.AlertType.OKCancel, new Action(this.Reconnect), new Action(this.CloseGame));
          break;
        }
        PopupSystem.ShowMessage("Game Full", "The game is currently full!\nDo you want to try again?", PopupSystem.AlertType.OKCancel, new Action(this.Reconnect), new Action(this.CloseGame));
        break;
    }
  }

  protected void OnApplicationQuit()
  {
    if (this._client == null)
      return;
    this._client.ShutDown();
  }

  [DebuggerHidden]
  private IEnumerator StartReconnectionInSeconds(int seconds) => (IEnumerator) new GameConnectionManager.\u003CStartReconnectionInSeconds\u003Ec__Iterator7B()
  {
    seconds = seconds,
    \u003C\u0024\u003Eseconds = seconds,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartRequestRoomMetaData(CmuneRoomID room, Action<int, GameMetaData> action) => (IEnumerator) new GameConnectionManager.\u003CStartRequestRoomMetaData\u003Ec__Iterator7C()
  {
    room = room,
    action = action,
    \u003C\u0024\u003Eroom = room,
    \u003C\u0024\u003Eaction = action,
    \u003C\u003Ef__this = this
  };

  public static void Start(GameMetaData game)
  {
    AutoMonoBehaviour<GameConnectionManager>.Instance._isConnectionStarted = true;
    AutoMonoBehaviour<GameConnectionManager>.Instance._gameRoom = game;
    AutoMonoBehaviour<GameConnectionManager>.Instance._client.ConnectToRoom((RoomMetaData) game, PlayerDataManager.CmidSecure, PlayerDataManager.AccessLevelSecure);
  }

  public static void Stop()
  {
    if (GameState.HasCurrentGame)
      GameState.CurrentGame.Leave();
    AutoMonoBehaviour<GameConnectionManager>.Instance._gameRoom = (GameMetaData) null;
    AutoMonoBehaviour<GameConnectionManager>.Instance._isConnectionStarted = false;
    AutoMonoBehaviour<GameConnectionManager>.Instance._client.Disconnect();
  }

  public static void RequestRoomMetaData(CmuneRoomID room, Action<int, GameMetaData> action) => AutoMonoBehaviour<GameConnectionManager>.Instance.StartCoroutine(AutoMonoBehaviour<GameConnectionManager>.Instance.StartRequestRoomMetaData(room, action));

  public bool IsConnectedToServer(string server) => this._client.IsConnected && this._client.CurrentConnection == server;

  public static bool IsConnected => AutoMonoBehaviour<GameConnectionManager>.Instance._client.ConnectionState == PhotonClient.ConnectionStatus.RUNNING;

  public static RemoteMethodInterface Rmi => AutoMonoBehaviour<GameConnectionManager>.Instance._client.Rmi;

  public static PhotonClient Client => AutoMonoBehaviour<GameConnectionManager>.Instance._client;

  public static string CurrentRoomID => AutoMonoBehaviour<GameConnectionManager>.Instance._client.PeerListener.CurrentRoom.ID;

  public static int CurrentPlayerID => AutoMonoBehaviour<GameConnectionManager>.Instance._client.PeerListener.ActorId;
}
