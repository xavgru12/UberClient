// Decompiled with JetBrains decompiler
// Type: CommConnectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CommConnectionManager : AutoMonoBehaviour<CommConnectionManager>
{
  private PhotonClient _client;
  private ClientCommCenter _commCenter;
  private float _syncTime;
  private float _pollFriendsOnlineStatus;

  private void Awake()
  {
    this._client = new PhotonClient(true);
    this._commCenter = new ClientCommCenter(this._client.Rmi);
  }

  private void Start()
  {
    GameConnectionManager.Client.PeerListener.SubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.OnGameConnectionChange));
    this.StartCoroutine(this.StartCheckingCommServerConnection());
    CmuneEventHandler.AddListener<LoginEvent>(new Action<LoginEvent>(this.OnLoginEvent));
  }

  private void OnLoginEvent(LoginEvent ev) => this._commCenter.Login();

  private void Update()
  {
    if (this._client != null && (double) this._syncTime <= (double) Time.time)
    {
      this._syncTime = Time.time + 0.02f;
      this._client.Update();
    }
    if ((double) this._pollFriendsOnlineStatus >= (double) Time.time)
      return;
    this._pollFriendsOnlineStatus = Time.time + 30f;
    if (!((UnityEngine.Object) MenuPageManager.Instance != (UnityEngine.Object) null) || !MenuPageManager.Instance.IsCurrentPage(PageType.Chat) && !MenuPageManager.Instance.IsCurrentPage(PageType.Inbox) && !MenuPageManager.Instance.IsCurrentPage(PageType.Clans))
      return;
    CommConnectionManager.CommCenter.UpdateContacts();
  }

  public static bool TryGetActor(int cmid, out CommActorInfo actor)
  {
    if (cmid > 0 && CommConnectionManager.CommCenter != null)
      return CommConnectionManager.CommCenter.TryGetActorWithCmid(cmid, out actor) && actor != null;
    actor = (CommActorInfo) null;
    return false;
  }

  public static bool IsPlayerOnline(int cmid) => cmid > 0 && CommConnectionManager.CommCenter != null && CommConnectionManager.CommCenter.HasActorWithCmid(cmid);

  public static void Reconnect()
  {
    CommConnectionManager.Stop();
    AutoMonoBehaviour<CommConnectionManager>.Instance.Awake();
  }

  protected void OnApplicationQuit()
  {
    if (this._client == null)
      return;
    this._client.ShutDown();
  }

  [DebuggerHidden]
  private IEnumerator StartCheckingCommServerConnection() => (IEnumerator) new CommConnectionManager.\u003CStartCheckingCommServerConnection\u003Ec__Iterator7A()
  {
    \u003C\u003Ef__this = this
  };

  public static void Stop()
  {
    if (AutoMonoBehaviour<CommConnectionManager>.Instance._client == null)
      return;
    AutoMonoBehaviour<CommConnectionManager>.Instance._commCenter.Leave();
    AutoMonoBehaviour<CommConnectionManager>.Instance._client.Disconnect();
  }

  public static void Request(
    byte applicationMethod,
    Action<int, object[]> callback,
    params object[] args)
  {
    ServerRequest.Run((MonoBehaviour) AutoMonoBehaviour<CommConnectionManager>.Instance, CmuneNetworkManager.CurrentCommServer.ConnectionString, callback, applicationMethod, args);
  }

  private void OnGameConnectionChange(PhotonPeerListener.ConnectionEvent ev)
  {
    if (!this._commCenter.IsInitialized)
      return;
    switch (ev.Type)
    {
      case PhotonPeerListener.ConnectionEventType.JoinedRoom:
        this._commCenter.UpdatePlayerRoom(ev.Room);
        break;
      case PhotonPeerListener.ConnectionEventType.LeftRoom:
        this._commCenter.ResetPlayerRoom();
        break;
    }
  }

  public static RemoteMethodInterface Rmi => AutoMonoBehaviour<CommConnectionManager>.Instance._client.Rmi;

  public static PhotonClient Client => AutoMonoBehaviour<CommConnectionManager>.Instance._client;

  public static int CurrentPlayerID => AutoMonoBehaviour<CommConnectionManager>.Instance._client.PeerListener.ActorId;

  public static CmuneRoomID CurrentRoomID => AutoMonoBehaviour<CommConnectionManager>.Instance._client.PeerListener.CurrentRoom;

  public static ClientCommCenter CommCenter => AutoMonoBehaviour<CommConnectionManager>.Instance._commCenter;

  public static bool IsConnected => AutoMonoBehaviour<CommConnectionManager>.Instance._client.IsConnected;
}
