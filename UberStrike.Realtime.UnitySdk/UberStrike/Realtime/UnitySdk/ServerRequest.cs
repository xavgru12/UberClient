// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ServerRequest
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  public class ServerRequest
  {
    protected int _requestTimeout = 5;
    protected PhotonClient _client;
    private MonoBehaviour _mono;

    private event Action<int, object[]> _customCallbackEvent;

    public PhotonClient.ConnectionStatus ConnectionState => this._client.ConnectionState;

    public PeerStateValue PeerState => this._client.PeerListener.PeerState;

    protected ServerRequest(MonoBehaviour mono)
    {
      this._mono = mono;
      this._client = new PhotonClient((bool) (UnityEngine.Object) mono);
    }

    public static void Run(
      MonoBehaviour mono,
      string serverConnection,
      Action<int, object[]> callback,
      byte methodID,
      params object[] args)
    {
      new ServerRequest(mono).Execute(serverConnection, callback, methodID, args);
    }

    public static void Run(
      MonoBehaviour mono,
      string serverConnection,
      byte methodID,
      params object[] args)
    {
      new ServerRequest(mono).Execute(serverConnection, (Action<int, object[]>) null, methodID, args);
    }

    protected bool Execute(
      string serverConnection,
      Action<int, object[]> callback,
      byte methodID,
      params object[] args)
    {
      if (this._client.ConnectionState == PhotonClient.ConnectionStatus.STOPPED)
      {
        this._customCallbackEvent = callback;
        this._mono.StartCoroutine(this.StartUpdateLoop());
        this._mono.StartCoroutine(this.StartRequest(serverConnection, methodID, args));
        return true;
      }
      CmuneDebug.LogWarning("ServerRequest to " + serverConnection + " ignored because connection " + (object) this._client.ConnectionState, new object[0]);
      return false;
    }

    private IEnumerator StartRequest(string serverConnection, byte methodID, params object[] args)
    {
      yield return (object) this._client.ConnectToServer(serverConnection, 0, MemberAccessLevel.Default);
      if (this._client.IsConnected)
        this._client.Rmi.Messenger.SendOperationToServerApplication(new Action<int, object[]>(this.OnRequestCallback), methodID, args);
      else
        this.OnRequestCallback(-1, (object[]) null);
      yield return (object) new WaitForSeconds((float) this._requestTimeout);
      this._client.Disconnect();
    }

    private IEnumerator StartUpdateLoop()
    {
      do
      {
        this._client.Update();
        yield return (object) new WaitForSeconds(0.1f);
      }
      while (this._client.ConnectionState != PhotonClient.ConnectionStatus.STOPPED);
    }

    protected virtual void OnRequestCallback(int result, object[] table)
    {
      if (this._customCallbackEvent == null)
        return;
      this._customCallbackEvent(result, table);
    }
  }
}
