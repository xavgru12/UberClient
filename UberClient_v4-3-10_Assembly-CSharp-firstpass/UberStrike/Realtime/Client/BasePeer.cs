// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.BasePeer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using ExitGames.Client.Photon;
using System;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

namespace UberStrike.Realtime.Client
{
  public abstract class BasePeer : IDisposable
  {
    private float nextUpdateTime;
    private PhotonPeerListener listener;
    private bool sendThreadShouldRun;

    public TrafficMonitor Monitor { get; private set; }

    public PhotonPeer Peer { get; private set; }

    public bool IsEnabled { get; private set; }

    public float SyncFrequency { get; private set; }

    public int ServerTimeTicks => this.Peer.ServerTimeInMilliSeconds & int.MaxValue;

    public bool IsConnected => this.Peer.PeerState == PeerStateValue.Connected;

    protected BasePeer(int syncFrequency, bool monitorTraffic)
    {
      this.listener = new PhotonPeerListener();
      this.Peer = new PhotonPeer((IPhotonPeerListener) this.listener, ConnectionProtocol.Udp);
      this.SyncFrequency = (float) syncFrequency / 1000f;
      this.Monitor = new TrafficMonitor(monitorTraffic);
      this.IsEnabled = true;
      if (monitorTraffic)
      {
        this.listener.OnError += (Action<string>) (error => this.Monitor.AddEvent(error));
        this.listener.OnConnect += (Action) (() => this.Monitor.AddEvent("Connected"));
        this.listener.OnDisconnect += (Action<StatusCode>) (_param1 => this.Monitor.AddEvent("Disconnected"));
        this.listener.EventDispatcher += new Action<byte, byte[]>(this.Monitor.OnEvent);
      }
      this.listener.OnConnect += new Action(this.OnConnected);
      this.listener.OnDisconnect += new Action<StatusCode>(this.OnDisconnected);
      this.listener.OnError += new Action<string>(this.OnError);
      UnityRuntime.Instance.OnUpdate += new Action(this.SendDispatch);
      this.StartFallbackSendAckThread();
      UnityRuntime.Instance.OnShutdown += new Action(this.StopFallbackSendAckThread);
    }

    public void Dispose()
    {
      if (!this.IsEnabled)
        return;
      this.Disconnect();
      this.IsEnabled = false;
      this.listener.ClearEvents();
      UnityRuntime.Instance.OnUpdate -= new Action(this.SendDispatch);
    }

    public void Connect(string endpointAddress)
    {
      if (this.Monitor.IsEnabled)
        this.Monitor.AddEvent("Connect " + endpointAddress);
      string ipAddress = new ConnectionAddress(endpointAddress).IpAddress;
      if (CrossdomainPolicy.HasValidPolicy(ipAddress))
        this.ConnectToServer(endpointAddress);
      else
        UnityRuntime.Instance.StartCoroutine(CrossdomainPolicy.CheckPolicyRoutine(ipAddress, (Action) (() =>
        {
          if (CrossdomainPolicy.HasValidPolicy(ipAddress))
            this.ConnectToServer(endpointAddress);
          else
            this.OnConnectionFail(endpointAddress);
        })));
    }

    private void ConnectToServer(string endpointAddress)
    {
      if (this.IsEnabled && this.Peer.Connect(endpointAddress, ApiVersion.Current))
        return;
      Debug.LogWarning((object) ("connection failed to " + endpointAddress));
      this.OnConnectionFail(endpointAddress);
    }

    public void Disconnect()
    {
      if (this.Monitor.IsEnabled)
        this.Monitor.AddEvent(nameof (Disconnect));
      this.Peer.SendOutgoingCommands();
      this.Peer.Disconnect();
    }

    private void SendDispatch()
    {
      if (this.Peer.PeerState == PeerStateValue.Disconnected)
        return;
      this.Peer.Service();
    }

    public void StartFallbackSendAckThread()
    {
      if (this.sendThreadShouldRun)
        return;
      this.sendThreadShouldRun = true;
      SupportClass.CallInBackground(new Func<bool>(this.FallbackSendAckThread));
    }

    public void StopFallbackSendAckThread() => this.sendThreadShouldRun = false;

    public bool FallbackSendAckThread()
    {
      if (this.sendThreadShouldRun && this.Peer != null)
        this.Peer.SendAcksOnly();
      return this.sendThreadShouldRun;
    }

    protected void AddRoomLogic(IEventDispatcher evDispatcher, IOperationSender opSender)
    {
      if (this.Monitor.IsEnabled)
        opSender.SendOperation += new RemoteProcedureCall(this.Monitor.SendOperation);
      opSender.SendOperation += new RemoteProcedureCall(this.Peer.OpCustom);
      this.listener.EventDispatcher += new Action<byte, byte[]>(evDispatcher.OnEvent);
    }

    protected void RemoveRoomLogic(IEventDispatcher evDispatcher, IOperationSender opSender)
    {
      if (this.Monitor.IsEnabled)
        opSender.SendOperation -= new RemoteProcedureCall(this.Monitor.SendOperation);
      opSender.SendOperation -= new RemoteProcedureCall(this.Peer.OpCustom);
      this.listener.EventDispatcher -= new Action<byte, byte[]>(evDispatcher.OnEvent);
    }

    public override string ToString() => this.Peer.PeerState.ToString();

    protected abstract void OnConnected();

    protected abstract void OnDisconnected(StatusCode status);

    protected abstract void OnError(string message);

    protected virtual void OnConnectionFail(string endpointAddress)
    {
    }
  }
}
