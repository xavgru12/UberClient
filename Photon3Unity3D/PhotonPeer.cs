// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.PhotonPeer
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Collections.Generic;

namespace ExitGames.Client.Photon
{
  public class PhotonPeer
  {
    public static readonly string PhotonPort = "5055";
    internal static short peerCount;
    internal PeerBase peerBase;

    public DebugLevel DebugOut
    {
      set => this.peerBase.debugOut = value;
      get => this.peerBase.debugOut;
    }

    public IPhotonPeerListener Listener
    {
      get => this.peerBase.Listener;
      protected set => this.peerBase.Listener = value;
    }

    public long BytesIn => this.peerBase.BytesIn;

    public long BytesOut => this.peerBase.BytesOut;

    public int ByteCountCurrentDispatch => this.peerBase.ByteCountCurrentDispatch;

    public int ByteCountLastOperation => this.peerBase.ByteCountLastOperation;

    public bool TrafficStatsEnabled
    {
      get => this.peerBase.TrafficStatsEnabled;
      set => this.peerBase.TrafficStatsEnabled = value;
    }

    public long TrafficStatsElapsedMs => this.peerBase.TrafficStatsEnabledTime;

    public void TrafficStatsReset()
    {
      this.peerBase.InitializeTrafficStats();
      this.peerBase.TrafficStatsEnabled = true;
    }

    public TrafficStats TrafficStatsIncoming => this.peerBase.TrafficStatsIncoming;

    public TrafficStats TrafficStatsOutgoing => this.peerBase.TrafficStatsOutgoing;

    public TrafficStatsGameLevel TrafficStatsGameLevel => this.peerBase.TrafficStatsGameLevel;

    public PeerStateValue PeerState => this.peerBase.peerConnectionState == PeerBase.ConnectionStateValue.Connected && !this.peerBase.ApplicationIsInitialized ? PeerStateValue.InitializingApplication : (PeerStateValue) this.peerBase.peerConnectionState;

    public string PeerID => this.peerBase.PeerID;

    public int CommandBufferSize
    {
      get => this.peerBase.commandBufferSize;
      set
      {
      }
    }

    public int LimitOfUnreliableCommands
    {
      get => this.peerBase.limitOfUnreliableCommands;
      set => this.peerBase.limitOfUnreliableCommands = value;
    }

    public int QueuedIncomingCommands => this.peerBase.QueuedIncomingCommandsCount;

    public int QueuedOutgoingCommands => this.peerBase.QueuedOutgoingCommandsCount;

    public byte ChannelCount
    {
      get => this.peerBase.ChannelCount;
      set => this.peerBase.ChannelCount = value != (byte) 0 && this.PeerState == PeerStateValue.Disconnected ? value : throw new Exception("ChannelCount can only be set while disconnected and must be > 0.");
    }

    public int WarningSize
    {
      get => this.peerBase.warningSize;
      set => this.peerBase.warningSize = value;
    }

    public int SentCountAllowance
    {
      get => this.peerBase.sentCountAllowance;
      set => this.peerBase.sentCountAllowance = value;
    }

    public int TimePingInterval
    {
      get => this.peerBase.timePingInterval;
      set => this.peerBase.timePingInterval = value;
    }

    public int DisconnectTimeout
    {
      get => this.peerBase.DisconnectTimeout;
      set => this.peerBase.DisconnectTimeout = value;
    }

    public int ServerTimeInMilliSeconds => this.peerBase.serverTimeOffsetIsAvailable ? this.peerBase.serverTimeOffset + this.LocalTimeInMilliSeconds : 0;

    public int LocalTimeInMilliSeconds => this.peerBase.GetLocalMsTimestamp();

    public PhotonPeer.GetLocalMsTimestampDelegate LocalMsTimestampDelegate
    {
      set
      {
        if (this.PeerState != PeerStateValue.Disconnected)
          throw new Exception("GetLocalMsTimestamp only settable while disconnected. State: " + (object) this.PeerState);
        this.peerBase.GetLocalMsTimestamp = value;
      }
    }

    public int RoundTripTime => this.peerBase.roundTripTime;

    public int RoundTripTimeVariance => this.peerBase.roundTripTimeVariance;

    public int TimestampOfLastSocketReceive => this.peerBase.timestampOfLastReceive;

    public string ServerAddress
    {
      get => this.peerBase.ServerAddress;
      set
      {
        if (this.UsedProtocol == ConnectionProtocol.Http)
        {
          this.peerBase.ServerAddress = value;
        }
        else
        {
          if (this.DebugOut < DebugLevel.ERROR)
            return;
          this.Listener.DebugReturn(DebugLevel.ERROR, "Failed to set ServerAddress. Can be set only when using HTTP.");
        }
      }
    }

    public string HttpUrlParameters
    {
      get => this.UsedProtocol == ConnectionProtocol.Http ? this.peerBase.HttpUrlParameters : string.Empty;
      set
      {
        if (this.UsedProtocol == ConnectionProtocol.Http)
        {
          this.peerBase.HttpUrlParameters = value;
        }
        else
        {
          if (this.DebugOut < DebugLevel.ERROR)
            return;
          this.Listener.DebugReturn(DebugLevel.ERROR, "Failed to set HttpUrlParameters. Can be set only when using HTTP.");
        }
      }
    }

    public ConnectionProtocol UsedProtocol => this.peerBase.usedProtocol;

    public NetworkSimulationSet NetworkSimulationSettings => this.peerBase.NetworkSimulationSettings;

    public bool UseOutgoingStream
    {
      get => this.peerBase.useOutgoingStream;
      set
      {
        if (this.peerBase.peerConnectionState != PeerBase.ConnectionStateValue.Disconnected)
          throw new NotSupportedException("UseOutgoingStream cannot be changed while the peer is connected. State: " + (object) this.peerBase.peerConnectionState);
        this.peerBase.useOutgoingStream = value;
      }
    }

    public static int OutgoingStreamBufferSize
    {
      get => PeerBase.outgoingStreamBufferSize;
      set => PeerBase.outgoingStreamBufferSize = value;
    }

    public long OutgoingStreamPosition => this.peerBase.OutgoingStreamPosition;

    public int MaximumTransferUnit
    {
      get => this.peerBase.mtu;
      set
      {
        if (this.PeerState != PeerStateValue.Disconnected)
          throw new Exception("MaximumTransferUnit is only settable while disconnected. State: " + (object) this.PeerState);
        if (value < 520)
          value = 520;
        this.peerBase.mtu = value;
      }
    }

    protected internal PhotonPeer(ConnectionProtocol protocolType)
    {
      switch (protocolType)
      {
        case ConnectionProtocol.Udp:
          this.peerBase = (PeerBase) new EnetPeer();
          this.peerBase.usedProtocol = protocolType;
          break;
        case ConnectionProtocol.Tcp:
          this.peerBase = (PeerBase) new TPeer();
          this.peerBase.usedProtocol = protocolType;
          break;
        case ConnectionProtocol.Http:
          this.peerBase = (PeerBase) new HttpBase2();
          this.peerBase.usedProtocol = protocolType;
          break;
      }
    }

    public PhotonPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
      : this(protocolType)
    {
      this.Listener = listener != null ? listener : throw new SystemException("listener cannot be null");
    }

    [Obsolete("Use the constructor with ConnectionProtocol instead.")]
    public PhotonPeer(IPhotonPeerListener listener)
      : this(listener, ConnectionProtocol.Udp)
    {
    }

    [Obsolete("Use the constructor with ConnectionProtocol instead.")]
    public PhotonPeer(IPhotonPeerListener listener, bool useTcp)
      : this(listener, useTcp ? ConnectionProtocol.Tcp : ConnectionProtocol.Udp)
    {
    }

    public virtual bool Connect(string serverAddress, string applicationName) => this.peerBase.Connect(serverAddress, applicationName, (byte) 0);

    public virtual bool Connect(string serverAddress, string applicationName, byte node) => this.peerBase.Connect(serverAddress, applicationName, node);

    public virtual void Disconnect() => this.peerBase.Disconnect();

    public virtual void StopThread() => this.peerBase.StopConnection();

    public virtual void FetchServerTimestamp() => this.peerBase.FetchServerTimestamp();

    public bool IsEncryptionAvailable => this.peerBase.isEncryptionAvailable;

    public bool IsSendingOnlyAcks
    {
      get => this.peerBase.IsSendingOnlyAcks;
      set => this.peerBase.IsSendingOnlyAcks = value;
    }

    public bool EstablishEncryption() => this.peerBase.ExchangeKeysForEncryption();

    public virtual void Service()
    {
      do
        ;
      while (this.DispatchIncomingCommands());
      do
        ;
      while (this.SendOutgoingCommands());
    }

    public virtual bool SendOutgoingCommands()
    {
      if (this.TrafficStatsEnabled)
        this.TrafficStatsGameLevel.SendOutgoingCommandsCalled();
      return this.peerBase.SendOutgoingCommands();
    }

    public virtual bool DispatchIncomingCommands()
    {
      this.peerBase.ByteCountCurrentDispatch = 0;
      if (this.TrafficStatsEnabled)
        this.TrafficStatsGameLevel.DispatchIncomingCommandsCalled();
      return this.peerBase.DispatchIncomingCommands();
    }

    public string VitalStatsToString(bool all)
    {
      if (this.TrafficStatsGameLevel == null)
        return "Stats not available. Use PhotonPeer.TrafficStatsEnabled.";
      return !all ? string.Format("Rtt(variance): {0}({1}). Ms since last receive: {2}. Stats elapsed: {4}sec.\n{3}", (object) this.RoundTripTime, (object) this.RoundTripTimeVariance, (object) (this.peerBase.GetLocalMsTimestamp() - this.TimestampOfLastSocketReceive), (object) this.TrafficStatsGameLevel.ToStringVitalStats(), (object) (this.TrafficStatsElapsedMs / 1000L)) : string.Format("Rtt(variance): {0}({1}). Ms since last receive: {2}. Stats elapsed: {6}sec.\n{3}\n{4}\n{5}", (object) this.RoundTripTime, (object) this.RoundTripTimeVariance, (object) (this.peerBase.GetLocalMsTimestamp() - this.TimestampOfLastSocketReceive), (object) this.TrafficStatsGameLevel.ToStringVitalStats(), (object) this.TrafficStatsIncoming.ToString(), (object) this.TrafficStatsOutgoing.ToString(), (object) (this.TrafficStatsElapsedMs / 1000L));
    }

    public virtual bool OpCustom(
      byte customOpCode,
      Dictionary<byte, object> customOpParameters,
      bool sendReliable)
    {
      return this.OpCustom(customOpCode, customOpParameters, sendReliable, (byte) 0);
    }

    public virtual bool OpCustom(
      byte customOpCode,
      Dictionary<byte, object> customOpParameters,
      bool sendReliable,
      byte channelId)
    {
      return this.peerBase.EnqueueOperation(customOpParameters, customOpCode, sendReliable, channelId, false);
    }

    public virtual bool OpCustom(
      byte customOpCode,
      Dictionary<byte, object> customOpParameters,
      bool sendReliable,
      byte channelId,
      bool encrypt)
    {
      if (encrypt && !this.IsEncryptionAvailable)
        throw new ArgumentException("Can't use encryption yet. Exchange keys first.");
      return this.peerBase.EnqueueOperation(customOpParameters, customOpCode, sendReliable, channelId, encrypt);
    }

    public virtual bool OpCustom(
      OperationRequest operationRequest,
      bool sendReliable,
      byte channelId,
      bool encrypt)
    {
      if (encrypt && !this.IsEncryptionAvailable)
        throw new ArgumentException("Can't use encryption yet. Exchange keys first.");
      return this.peerBase.EnqueueOperation(operationRequest.Parameters, operationRequest.OperationCode, sendReliable, channelId, encrypt);
    }

    public static bool RegisterType(
      Type customType,
      byte code,
      SerializeMethod serializeMethod,
      DeserializeMethod constructor)
    {
      return Protocol.TryRegisterType(customType, code, serializeMethod, constructor);
    }

    public delegate int GetLocalMsTimestampDelegate();
  }
}
