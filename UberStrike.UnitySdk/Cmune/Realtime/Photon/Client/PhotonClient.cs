﻿// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.PhotonClient
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using Cmune.Realtime.Common;
using Cmune.Util;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using UnityEngine;

namespace Cmune.Realtime.Photon.Client
{
  public class PhotonClient
  {
    private bool _isShutdown;
    private bool _isUpdateCalled;
    public static bool IsPhotonEnabled = true;
    private bool _isStoppingConnection;
    private bool _isStartingConnection;
    private float _connectionTime;
    private MonoBehaviour _mono;
    private PhotonPeerListener _peerListener;
    private RemoteMethodInterface _rmi;
    private NetworkMessenger _messenger;

    public PhotonClient(MonoBehaviour mono, bool monitorMemory = false)
    {
      this._mono = mono;
      this._peerListener = new PhotonPeerListener(monitorMemory);
      this._messenger = new NetworkMessenger(this._peerListener);
      this._rmi = new RemoteMethodInterface(this._messenger);
      this._peerListener.SubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.EventCallback));
      this._peerListener.SetMessageCallback(new PhotonPeerListener.RecieveMessages(this._rmi.RecieveMessage));
    }

    public Coroutine ConnectToRoom(RoomMetaData room, int cmid, MemberAccessLevel accessLevel) => this.PeerListener.IsConnectedToServer && this.PeerListener.Server == room.ServerConnection && !this.PeerListener.HasJoinedRoom ? this._mono.StartCoroutine(this.StartJoiningRoom(room, cmid, (int) accessLevel)) : this._mono.StartCoroutine(this.StartConnection(room.ServerConnection, room, cmid, (int) accessLevel));

    public Coroutine ConnectToServer(string server, int cmid, MemberAccessLevel accessLevel) => this._mono.StartCoroutine(this.StartConnection(server, (RoomMetaData) null, cmid, (int) accessLevel));

    public Coroutine Disconnect() => this._mono.StartCoroutine(this.StopConnection());

    public void ShutDown()
    {
      if (this._isShutdown)
        return;
      this._isShutdown = true;
      this._peerListener.UnsubscribeToEvents(new Action<PhotonPeerListener.ConnectionEvent>(this.EventCallback));
      this._peerListener.ShutDown();
    }

    public void Update()
    {
      if (!this._isUpdateCalled)
        this._isUpdateCalled = true;
      this._peerListener.Update();
    }

    private IEnumerator StartConnection(
      string server,
      RoomMetaData room,
      int cmid,
      int accessLevel)
    {
      ConnectionAddress address = new ConnectionAddress(server);
      if (!this._isStartingConnection && PhotonClient.IsPhotonEnabled && address.IsValid)
      {
        this._isStartingConnection = true;
        while (this._isStoppingConnection)
          yield return (object) new WaitForEndOfFrame();
        if (this.ConnectionState == PhotonClient.ConnectionStatus.STOPPED)
        {
          this.ConnectionState = PhotonClient.ConnectionStatus.POLICY;
          yield return (object) this._mono.StartCoroutine(CrossdomainPolicy.CheckDomain(address.ServerIP));
          if (CrossdomainPolicy.HasValidPolicy(address.ServerIP))
          {
            if (this._peerListener.Connect(address.ConnectionString, cmid))
            {
              this.ConnectionState = PhotonClient.ConnectionStatus.STARTING;
              this._connectionTime = 0.0f;
              float timeout = Time.time + 10f;
              while (this._peerListener.IsConnecting && (double) Time.time < (double) timeout && this.ConnectionState == PhotonClient.ConnectionStatus.STARTING)
              {
                yield return (object) new WaitForEndOfFrame();
                this._connectionTime += Time.deltaTime;
                if (!this._isUpdateCalled)
                {
                  timeout = Time.time;
                  CmuneDebug.LogError("Please call PhotonClient.Update() in an Update loop!", new object[0]);
                }
              }
              if (this._peerListener.IsConnectedToServer && this.ConnectionState == PhotonClient.ConnectionStatus.STARTING)
              {
                if (room != null)
                {
                  this._peerListener.JoinRoom(room, cmid, accessLevel);
                  timeout = Time.time + 30f;
                  while (this._peerListener.IsJoining && (double) Time.time < (double) timeout && PhotonClient.IsPhotonEnabled)
                  {
                    yield return (object) new WaitForEndOfFrame();
                    this._connectionTime += Time.deltaTime;
                  }
                  if (!this._peerListener.HasJoinedRoom || !PhotonClient.IsPhotonEnabled)
                  {
                    this.ConnectionState = PhotonClient.ConnectionStatus.STOPPING;
                    this._peerListener.Disconnect(true);
                    while (this._peerListener.PeerState > PeerStateValue.Disconnected)
                      yield return (object) new WaitForEndOfFrame();
                  }
                }
                else
                  this.ConnectionState = PhotonClient.ConnectionStatus.RUNNING;
              }
              else if (this.ConnectionState != PhotonClient.ConnectionStatus.STOPPED)
              {
                this.ConnectionState = PhotonClient.ConnectionStatus.STOPPING;
                this._peerListener.Disconnect(true);
                while (this._peerListener.PeerState > PeerStateValue.Disconnected)
                  yield return (object) new WaitForEndOfFrame();
              }
            }
            else
            {
              this.ConnectionState = PhotonClient.ConnectionStatus.STOPPED;
              CmuneDebug.LogError("Server has Crossdomain Policy but not reachable: " + address.ConnectionString, new object[0]);
            }
          }
          else
          {
            CmuneDebug.LogError("No Crossdomain Policy hosted on port 843 on : " + address.ServerIP, new object[0]);
            this.EventCallback(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected, CmuneRoomID.Empty, 0));
          }
        }
        else
          CmuneDebug.LogWarning("Please first stop Connection before reconnecting! " + (object) this._peerListener.ConnectionState, new object[0]);
        if (this.ConnectionState == PhotonClient.ConnectionStatus.STOPPING)
        {
          this._peerListener.Disconnect(true);
          while (this._peerListener.PeerState > PeerStateValue.Disconnected)
            yield return (object) new WaitForEndOfFrame();
        }
        this._isStartingConnection = false;
      }
      else if (this._isStartingConnection)
        CmuneDebug.LogWarning("Ignored StartConnection because already running: " + (object) this._peerListener.ConnectionState, new object[0]);
      else if (!address.IsValid)
        CmuneDebug.LogWarning("Ignored StartConnection because address is not valid: " + address.ConnectionString, new object[0]);
    }

    private IEnumerator StartJoiningRoom(RoomMetaData room, int cmid, int accessLevel)
    {
      if (!this._isStartingConnection && PhotonClient.IsPhotonEnabled)
      {
        this._isStartingConnection = true;
        if (this.ConnectionState == PhotonClient.ConnectionStatus.RUNNING)
        {
          if (this._peerListener.IsConnectedToServer)
          {
            this._peerListener.JoinRoom(room, cmid, accessLevel);
            while (this._peerListener.IsJoining)
            {
              this._connectionTime += Time.deltaTime;
              yield return (object) new WaitForEndOfFrame();
            }
          }
        }
        else
          CmuneDebug.LogWarning("Please first start a Connection before joining a room! " + (object) this._peerListener.ConnectionState, new object[0]);
        this._isStartingConnection = false;
      }
      else
        CmuneDebug.LogError("Error: failed to join room: " + room.RoomName + " " + (object) this._isStartingConnection + " " + (object) PhotonClient.IsPhotonEnabled, new object[0]);
    }

    private IEnumerator StopConnection()
    {
      if (!this._isStoppingConnection)
      {
        this._isStoppingConnection = true;
        if (this.ConnectionState != PhotonClient.ConnectionStatus.STOPPED)
        {
          this.ConnectionState = PhotonClient.ConnectionStatus.STOPPING;
          if (!this._isStartingConnection)
          {
            if (this._peerListener.HasJoinedRoom)
            {
              this._peerListener.LeaveCurrentRoom();
              while (this._peerListener.IsLeaving)
                yield return (object) new WaitForEndOfFrame();
            }
            this._peerListener.Disconnect();
            while (this._peerListener.PeerState > PeerStateValue.Disconnected)
              yield return (object) new WaitForEndOfFrame();
          }
        }
        this._isStoppingConnection = false;
      }
    }

    private void EventCallback(PhotonPeerListener.ConnectionEvent ev)
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("EventCallback " + (object) ev.Type, new object[0]);
      switch (ev.Type)
      {
        case PhotonPeerListener.ConnectionEventType.Disconnected:
          this.ConnectionState = PhotonClient.ConnectionStatus.STOPPED;
          this._rmi.UnregisterAllClasses();
          break;
        case PhotonPeerListener.ConnectionEventType.JoinedRoom:
          if (this.ConnectionState == PhotonClient.ConnectionStatus.STOPPING)
          {
            this._peerListener.Disconnect();
            break;
          }
          this.ConnectionState = PhotonClient.ConnectionStatus.RUNNING;
          this._rmi.RegisterAllClasses();
          break;
      }
    }

    public string CurrentConnection => this.PeerListener.Server;

    public string MessagingState => string.Format("{0} {1}", (object) this._peerListener.CurrentRoom.Server, (object) this._peerListener.ConnectionState);

    public PhotonClient.ConnectionStatus ConnectionState { get; private set; }

    public float ConnectionTime => this._connectionTime;

    public RemoteMethodInterface Rmi => this._rmi;

    public PhotonPeerListener PeerListener => this._peerListener;

    public bool IsActive => this._peerListener.PeerState > PeerStateValue.Disconnected;

    public bool IsConnected => this.ConnectionState == PhotonClient.ConnectionStatus.STARTING || this.ConnectionState == PhotonClient.ConnectionStatus.RUNNING;

    public int Latency => this._peerListener.Latency;

    public string Debug => string.Format("Start {0}/Stop {1}, ConState {2}, PeerState {3}/{4}", (object) this._isStartingConnection, (object) this._isStoppingConnection, (object) this.ConnectionState, (object) this._peerListener.ConnectionState, (object) this._peerListener.PeerState);

    public enum ConnectionStatus
    {
      STOPPED = 0,
      RUNNING = 1,
      STARTING = 2,
      STOPPING = 4,
      POLICY = 5,
    }
  }
}
