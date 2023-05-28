// Decompiled with JetBrains decompiler
// Type: ServerLoadRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ServerLoadRequest : ServerRequest
{
  private Action _callback;

  private ServerLoadRequest(GameServerView server, Action callback)
    : base((MonoBehaviour) AutoMonoBehaviour<MonoRoutine>.Instance)
  {
    this._callback = callback;
    this.Server = server;
    this._client.PeerListener.SubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.OnConnectionEvent));
  }

  public ServerLoadRequest.RequestStateType RequestState { get; private set; }

  public GameServerView Server { get; private set; }

  private void OnConnectionEvent(PhotonPeerListener.ConnectionEvent ev)
  {
    if (ev.Type != PhotonPeerListener.ConnectionEventType.Disconnected || this.RequestState != ServerLoadRequest.RequestStateType.Waiting)
      return;
    this.RequestState = ServerLoadRequest.RequestStateType.Down;
  }

  public static ServerLoadRequest Run(GameServerView server, Action callback)
  {
    ServerLoadRequest serverLoadRequest = new ServerLoadRequest(server, callback);
    serverLoadRequest.RunAgain();
    return serverLoadRequest;
  }

  public void RunAgain()
  {
    if (!this.Execute(this.Server.ConnectionString, (Action<int, object[]>) null, (byte) 2, new object[0]))
      return;
    this.RequestState = ServerLoadRequest.RequestStateType.Waiting;
  }

  protected override void OnRequestCallback(int result, object[] table)
  {
    this.RequestState = ServerLoadRequest.RequestStateType.Down;
    base.OnRequestCallback(result, table);
    if (result == 0)
    {
      if (table.Length > 0 && table[0] is ServerLoadData)
      {
        this.Server.Data = (ServerLoadData) table[0];
        this.Server.Data.Latency = this._client.Latency;
        this.Server.Data.State = ServerLoadData.Status.Alive;
        this.RequestState = ServerLoadRequest.RequestStateType.Running;
      }
      else
        this.Server.Data.State = ServerLoadData.Status.NotReachable;
    }
    else
      this.Server.Data.State = ServerLoadData.Status.NotReachable;
    this._callback();
  }

  public enum RequestStateType
  {
    None,
    Waiting,
    Running,
    Down,
  }
}
