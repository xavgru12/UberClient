﻿
using Cmune.Realtime.Common;
using System;

namespace Cmune.Realtime.Photon.Client
{
  public abstract class ClientNetworkClass : CmuneNetworkClass
  {
    private static int NextInstanceId;
    protected RemoteMethodInterface _rmi;
    protected short _networkClassID = -1;

    protected ClientNetworkClass(RemoteMethodInterface rmi)
    {
      this._rmi = rmi;
      this._instanceID = ++ClientNetworkClass.NextInstanceId;
      if (!this.TryGetStaticNetworkClassId(out this._networkClassID))
        throw new Exception("New Instance of StaticNetworkClass without Attribute 'NetworkClass' assigned.");
      this._rmi.RegisterGlobalNetworkClass((INetworkClass) this, this._networkClassID);
    }

    public virtual void Leave()
    {
      if (this._rmi == null || this._rmi.Messenger == null || this._rmi.Messenger.PeerListener.ActorIdSecure <= 0)
        return;
      this.SendMethodToServer((byte) 2, (object) this._rmi.Messenger.PeerListener.ActorIdSecure);
    }

    protected override void Dispose(bool dispose)
    {
      this._rmi.DisposeNetworkClass((INetworkClass) this);
      base.Dispose(dispose);
    }

    protected void SendMethodToServer(byte localAddress, params object[] args) => this._rmi.Messenger.SendMessageToServer(this.NetworkID, true, localAddress, args);

    protected void SendMethodToPlayer(int playerID, byte localAddress, params object[] args) => this._rmi.Messenger.SendMessageToPlayer(playerID, this.NetworkID, localAddress, args);

    protected void SendMethodToAll(byte localAddress, params object[] args) => this._rmi.Messenger.SendMessageToAll(this.NetworkID, localAddress, args);

    protected void SendUnreliableMethodToServer(byte localAddress, params object[] args) => this._rmi.Messenger.SendMessageToServer(this.NetworkID, false, localAddress, args);
  }
}
