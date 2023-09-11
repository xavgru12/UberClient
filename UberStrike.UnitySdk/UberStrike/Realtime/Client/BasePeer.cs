
using Cmune.DataCenter.Common.Entities;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;
using Uberstrike.Realtime.Client.Base;
using UnityEngine;

namespace Uberstrike.Realtime.Client
{
  public abstract class BasePeer
  {
    private float nextUpdateTime;
    private PhotonPeerListener listener;

    public PhotonPeer Peer { get; private set; }

    public float SyncFrequency { get; set; }

    public bool IsConnected => this.Peer.PeerState == PeerStateValue.Connected;

    protected BasePeer(float syncFrequency)
    {
      this.listener = new PhotonPeerListener();
      this.Peer = new PhotonPeer((IPhotonPeerListener) this.listener);
      this.SyncFrequency = syncFrequency;
    }

    public void Connect(string endpointAddress, int cmuneId, ChannelType channel)
    {
      ServerConnectionView instance = new ServerConnectionView()
      {
        ApiVersion = 0,
        Cmid = cmuneId,
        Channel = channel
      };
      using (MemoryStream memoryStream = new MemoryStream())
      {
        ServerConnectionViewProxy.Serialize((Stream) memoryStream, instance);
        this.Peer.Connect(endpointAddress, Convert.ToBase64String(memoryStream.ToArray()));
      }
    }

    public void Disconnect() => this.Peer.Disconnect();

    public void SyncThread()
    {
      if ((double) Time.realtimeSinceStartup <= (double) this.nextUpdateTime)
        return;
      this.nextUpdateTime = Time.realtimeSinceStartup + this.SyncFrequency;
      if (this.Peer.PeerState != PeerStateValue.Disconnected)
        this.Peer.Service();
    }

    public void AddDispatcher(IEventDispatcher evDispatcher, IOperationDispatcher opSender)
    {
      opSender.SetSender(new Func<byte, Dictionary<byte, object>, bool, bool>(this.Peer.OpCustom));
      this.listener.EventDispatcher += new Action<byte, byte[]>(evDispatcher.OnEvent);
    }

    public void RemovedDispatcher(IEventDispatcher evDispatcher, IOperationDispatcher opSender)
    {
      opSender.SetSender((Func<byte, Dictionary<byte, object>, bool, bool>) null);
      this.listener.EventDispatcher -= new Action<byte, byte[]>(evDispatcher.OnEvent);
    }

    public override string ToString() => this.Peer.PeerState.ToString();
  }
}
