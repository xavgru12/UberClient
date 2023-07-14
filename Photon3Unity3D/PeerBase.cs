// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.PeerBase
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using Photon.SocketServer.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ExitGames.Client.Photon
{
  internal abstract class PeerBase
  {
    internal const int ENET_PEER_PACKET_LOSS_SCALE = 65536;
    internal const int ENET_PEER_DEFAULT_ROUND_TRIP_TIME = 300;
    internal const int ENET_PEER_PACKET_THROTTLE_INTERVAL = 5000;
    public int ByteCountLastOperation;
    public int ByteCountCurrentDispatch;
    internal int TrafficPackageHeaderSize;
    public TrafficStats TrafficStatsIncoming;
    public TrafficStats TrafficStatsOutgoing;
    public TrafficStatsGameLevel TrafficStatsGameLevel;
    private Stopwatch trafficStatsStopwatch;
    private bool trafficStatsEnabled = false;
    internal ConnectionProtocol usedProtocol;
    internal DebugLevel debugOut = DebugLevel.ERROR;
    internal readonly Queue<PeerBase.MyAction> ActionQueue = new Queue<PeerBase.MyAction>();
    internal PhotonPeer.GetLocalMsTimestampDelegate GetLocalMsTimestamp = (PhotonPeer.GetLocalMsTimestampDelegate) (() => Environment.TickCount);
    internal short peerID = -1;
    internal PeerBase.ConnectionStateValue peerConnectionState;
    internal int serverTimeOffset;
    internal bool serverTimeOffsetIsAvailable;
    internal int roundTripTime;
    internal int roundTripTimeVariance;
    internal int lastRoundTripTime;
    internal int lowestRoundTripTime;
    internal int lastRoundTripTimeVariance;
    internal int highestRoundTripTimeVariance;
    internal int timestampOfLastReceive;
    internal int packetThrottleInterval;
    internal static short peerCount;
    internal long bytesOut;
    internal long bytesIn;
    internal int commandBufferSize = 100;
    internal int warningSize = 100;
    internal int sentCountAllowance = 5;
    internal int DisconnectTimeout = 10000;
    internal int timePingInterval = 1000;
    internal byte ChannelCount = 2;
    internal int limitOfUnreliableCommands = 0;
    public DiffieHellmanCryptoProvider CryptoProvider;
    private readonly Random lagRandomizer = new Random();
    private readonly LinkedList<SimulationItem> NetSimListOutgoing = new LinkedList<SimulationItem>();
    private readonly LinkedList<SimulationItem> NetSimListIncoming = new LinkedList<SimulationItem>();
    private NetworkSimulationSet networkSimulationSettings = new NetworkSimulationSet();
    internal byte[] INIT_BYTES = new byte[41];
    internal int timeBase;
    internal int timeInt;
    internal int timeoutInt;
    internal int timeLastAckReceive;
    internal bool ApplicationIsInitialized;
    internal bool isEncryptionAvailable;
    internal bool useOutgoingStream = true;
    internal static int outgoingStreamBufferSize = 10240;
    internal int outgoingCommandsInStream = 0;
    internal readonly object SendOutgoingLockObject = new object();
    protected bool isSendingOnlyAcks;
    internal int mtu = 1200;
    protected MemoryStream SerializeMemStream = new MemoryStream();

    public long TrafficStatsEnabledTime => this.trafficStatsStopwatch != null ? this.trafficStatsStopwatch.ElapsedMilliseconds : 0L;

    public bool TrafficStatsEnabled
    {
      get => this.trafficStatsEnabled;
      set
      {
        this.trafficStatsEnabled = value;
        if (value)
        {
          if (this.trafficStatsStopwatch == null)
            this.InitializeTrafficStats();
          this.trafficStatsStopwatch.Start();
        }
        else
          this.trafficStatsStopwatch.Stop();
      }
    }

    internal string ServerAddress { get; set; }

    internal string HttpUrlParameters { get; set; }

    internal IPhotonPeerListener Listener { get; set; }

    public NetworkSimulationSet NetworkSimulationSettings => this.networkSimulationSettings;

    internal long BytesOut => this.bytesOut;

    internal long BytesIn => this.bytesIn;

    internal abstract int QueuedIncomingCommandsCount { get; }

    internal abstract int QueuedOutgoingCommandsCount { get; }

    public virtual string PeerID => this.peerID.ToString();

    internal void InitOnce()
    {
      this.networkSimulationSettings.SimulationMethod = new ThreadStart(this.NetworkSimRun);
      this.INIT_BYTES[0] = (byte) 243;
      this.INIT_BYTES[1] = (byte) 0;
      this.INIT_BYTES[2] = (byte) 1;
      this.INIT_BYTES[3] = (byte) 6;
      this.INIT_BYTES[4] = (byte) 1;
      this.INIT_BYTES[5] = (byte) 3;
      this.INIT_BYTES[6] = (byte) 0;
      this.INIT_BYTES[7] = (byte) 1;
      this.INIT_BYTES[8] = (byte) 7;
    }

    internal abstract bool Connect(string serverAddress, string appID, byte nodeId);

    internal abstract void Disconnect();

    internal abstract void StopConnection();

    internal abstract void FetchServerTimestamp();

    internal bool EnqueueOperation(
      Dictionary<byte, object> parameters,
      byte opCode,
      bool sendReliable,
      byte channelId,
      bool encrypted)
    {
      return this.EnqueueOperation(parameters, opCode, sendReliable, channelId, encrypted, PeerBase.EgMessageType.Operation);
    }

    internal abstract bool EnqueueOperation(
      Dictionary<byte, object> parameters,
      byte opCode,
      bool sendReliable,
      byte channelId,
      bool encrypted,
      PeerBase.EgMessageType messageType);

    internal abstract bool DispatchIncomingCommands();

    internal abstract bool SendOutgoingCommands();

    internal abstract byte[] SerializeOperationToMessage(
      byte opCode,
      Dictionary<byte, object> parameters,
      PeerBase.EgMessageType messageType,
      bool encrypt);

    internal abstract void receiveIncomingCommands(byte[] inBuff);

    internal void InitCallback()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connecting)
        this.peerConnectionState = PeerBase.ConnectionStateValue.Connected;
      this.ApplicationIsInitialized = true;
      this.FetchServerTimestamp();
      this.Listener.OnStatusChanged(StatusCode.Connect);
    }

    internal long OutgoingStreamPosition
    {
      get
      {
        TPeer tpeer = this as TPeer;
        return this.useOutgoingStream && tpeer != null ? tpeer.outgoingStream.Position : -1L;
      }
    }

    internal bool IsSendingOnlyAcks
    {
      get => this.isSendingOnlyAcks;
      set
      {
        lock (this.SendOutgoingLockObject)
          this.isSendingOnlyAcks = value;
      }
    }

    internal bool ExchangeKeysForEncryption()
    {
      this.isEncryptionAvailable = false;
      this.CryptoProvider = new DiffieHellmanCryptoProvider();
      return this.EnqueueOperation(new Dictionary<byte, object>(1)
      {
        [PhotonCodes.ClientKey] = (object) this.CryptoProvider.PublicKey
      }, PhotonCodes.InitEncryption, true, (byte) 0, false, PeerBase.EgMessageType.InternalOperationRequest);
    }

    internal void DeriveSharedKey(OperationResponse operationResponse)
    {
      if (operationResponse.ReturnCode != (short) 0)
      {
        this.EnqueueDebugReturn(DebugLevel.ERROR, "Establishing encryption keys failed. " + operationResponse.ToStringFull());
        this.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
      }
      else
      {
        byte[] otherPartyPublicKey = (byte[]) operationResponse[PhotonCodes.ServerKey];
        if (otherPartyPublicKey == null || otherPartyPublicKey.Length == 0)
        {
          this.EnqueueDebugReturn(DebugLevel.ERROR, "Establishing encryption keys failed. Server's public key is null or empty. " + operationResponse.ToStringFull());
          this.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
        }
        else
        {
          this.CryptoProvider.DeriveSharedKey(otherPartyPublicKey);
          this.isEncryptionAvailable = true;
          this.EnqueueStatusCallback(StatusCode.EncryptionEstablished);
        }
      }
    }

    internal void EnqueueActionForDispatch(PeerBase.MyAction action)
    {
      lock (this.ActionQueue)
        this.ActionQueue.Enqueue(action);
    }

    internal void EnqueueDebugReturn(DebugLevel level, string debugReturn)
    {
      lock (this.ActionQueue)
        this.ActionQueue.Enqueue((PeerBase.MyAction) (() => this.Listener.DebugReturn(level, debugReturn)));
    }

    internal void EnqueueStatusCallback(StatusCode statusValue)
    {
      lock (this.ActionQueue)
        this.ActionQueue.Enqueue((PeerBase.MyAction) (() => this.Listener.OnStatusChanged(statusValue)));
    }

    internal virtual void InitPeerBase()
    {
      this.TrafficStatsIncoming = new TrafficStats(this.TrafficPackageHeaderSize);
      this.TrafficStatsOutgoing = new TrafficStats(this.TrafficPackageHeaderSize);
      this.TrafficStatsGameLevel = new TrafficStatsGameLevel();
      this.ByteCountLastOperation = 0;
      this.ByteCountCurrentDispatch = 0;
      this.bytesIn = 0L;
      this.bytesOut = 0L;
      this.networkSimulationSettings.LostPackagesIn = 0;
      this.networkSimulationSettings.LostPackagesOut = 0;
      lock (this.NetSimListOutgoing)
        this.NetSimListOutgoing.Clear();
      lock (this.NetSimListIncoming)
        this.NetSimListIncoming.Clear();
      this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnected;
      this.timeBase = this.GetLocalMsTimestamp();
      this.isEncryptionAvailable = false;
      this.ApplicationIsInitialized = false;
      this.roundTripTime = 300;
      this.roundTripTimeVariance = 0;
      this.packetThrottleInterval = 5000;
      this.serverTimeOffsetIsAvailable = false;
      this.serverTimeOffset = 0;
    }

    internal virtual bool DeserializeMessageAndCallback(byte[] inBuff)
    {
      if (inBuff.Length < 2)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, "Incoming UDP data too short! " + (object) inBuff.Length);
        return false;
      }
      if (inBuff[0] != (byte) 243 && inBuff[0] != (byte) 253)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ALL, "No regular operation UDP message: " + (object) inBuff[0]);
        return false;
      }
      byte num1 = (byte) ((uint) inBuff[1] & (uint) sbyte.MaxValue);
      bool flag = ((int) inBuff[1] & 128) > 0;
      MemoryStream memoryStream = (MemoryStream) null;
      if (num1 != (byte) 1)
      {
        try
        {
          if (flag)
          {
            inBuff = this.CryptoProvider.Decrypt(inBuff, 2, inBuff.Length - 2);
            memoryStream = new MemoryStream(inBuff);
          }
          else
          {
            memoryStream = new MemoryStream(inBuff);
            memoryStream.Seek(2L, SeekOrigin.Begin);
          }
        }
        catch (Exception ex)
        {
          if (this.debugOut >= DebugLevel.ERROR)
            this.Listener.DebugReturn(DebugLevel.ERROR, ex.ToString());
          SupportClass.WriteStackTrace(ex, Console.Error);
          return false;
        }
      }
      int num2 = 0;
      switch ((int) num1 - 1)
      {
        case 0:
          this.InitCallback();
          break;
        case 2:
          OperationResponse operationResponse1 = Protocol.DeserializeOperationResponse(memoryStream);
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.CountResult(this.ByteCountCurrentDispatch);
            num2 = this.GetLocalMsTimestamp();
          }
          this.Listener.OnOperationResponse(operationResponse1);
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.TimeForResponseCallback(operationResponse1.OperationCode, this.GetLocalMsTimestamp() - num2);
            break;
          }
          break;
        case 3:
          EventData eventData = Protocol.DeserializeEventData(memoryStream);
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.CountEvent(this.ByteCountCurrentDispatch);
            num2 = this.GetLocalMsTimestamp();
          }
          this.Listener.OnEvent(eventData);
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.TimeForEventCallback(eventData.Code, this.GetLocalMsTimestamp() - num2);
            break;
          }
          break;
        case 6:
          OperationResponse operationResponse2 = Protocol.DeserializeOperationResponse(memoryStream);
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.CountResult(this.ByteCountCurrentDispatch);
            num2 = this.GetLocalMsTimestamp();
          }
          if ((int) operationResponse2.OperationCode == (int) PhotonCodes.InitEncryption)
            this.DeriveSharedKey(operationResponse2);
          else
            this.EnqueueDebugReturn(DebugLevel.ERROR, "Received unknown internal operation. " + operationResponse2.ToStringFull());
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsGameLevel.TimeForResponseCallback(operationResponse2.OperationCode, this.GetLocalMsTimestamp() - num2);
            break;
          }
          break;
        default:
          this.EnqueueDebugReturn(DebugLevel.ERROR, "unexpected msgType " + (object) num1);
          break;
      }
      return true;
    }

    internal void SendNetworkSimulated(PeerBase.MyAction sendAction)
    {
      if (!this.NetworkSimulationSettings.IsSimulationEnabled)
        sendAction();
      else if (this.usedProtocol == ConnectionProtocol.Udp && this.NetworkSimulationSettings.OutgoingLossPercentage > 0 && this.lagRandomizer.Next(101) < this.NetworkSimulationSettings.OutgoingLossPercentage)
      {
        ++this.networkSimulationSettings.LostPackagesOut;
      }
      else
      {
        int num1 = this.networkSimulationSettings.OutgoingLag + (this.networkSimulationSettings.OutgoingJitter <= 0 ? 0 : this.lagRandomizer.Next(this.networkSimulationSettings.OutgoingJitter * 2) - this.networkSimulationSettings.OutgoingJitter);
        int num2 = Environment.TickCount + num1;
        SimulationItem simulationItem = new SimulationItem()
        {
          ActionToExecute = sendAction,
          TimeToExecute = num2,
          Delay = num1
        };
        lock (this.NetSimListOutgoing)
        {
          if (this.NetSimListOutgoing.Count == 0 || this.usedProtocol == ConnectionProtocol.Tcp)
          {
            this.NetSimListOutgoing.AddLast(simulationItem);
          }
          else
          {
            LinkedListNode<SimulationItem> node = this.NetSimListOutgoing.First;
            while (node != null && node.Value.TimeToExecute < num2)
              node = node.Next;
            if (node == null)
              this.NetSimListOutgoing.AddLast(simulationItem);
            else
              this.NetSimListOutgoing.AddBefore(node, simulationItem);
          }
        }
      }
    }

    internal void ReceiveNetworkSimulated(PeerBase.MyAction receiveAction)
    {
      if (!this.networkSimulationSettings.IsSimulationEnabled)
        receiveAction();
      else if (this.usedProtocol == ConnectionProtocol.Udp && this.networkSimulationSettings.IncomingLossPercentage > 0 && this.lagRandomizer.Next(101) < this.networkSimulationSettings.IncomingLossPercentage)
      {
        ++this.networkSimulationSettings.LostPackagesIn;
      }
      else
      {
        int num1 = this.networkSimulationSettings.IncomingLag + (this.networkSimulationSettings.IncomingJitter <= 0 ? 0 : this.lagRandomizer.Next(this.networkSimulationSettings.IncomingJitter * 2) - this.networkSimulationSettings.IncomingJitter);
        int num2 = Environment.TickCount + num1;
        SimulationItem simulationItem = new SimulationItem()
        {
          ActionToExecute = receiveAction,
          TimeToExecute = num2,
          Delay = num1
        };
        lock (this.NetSimListIncoming)
        {
          if (this.NetSimListIncoming.Count == 0 || this.usedProtocol == ConnectionProtocol.Tcp)
          {
            this.NetSimListIncoming.AddLast(simulationItem);
          }
          else
          {
            LinkedListNode<SimulationItem> node = this.NetSimListIncoming.First;
            while (node != null && node.Value.TimeToExecute < num2)
              node = node.Next;
            if (node == null)
              this.NetSimListIncoming.AddLast(simulationItem);
            else
              this.NetSimListIncoming.AddBefore(node, simulationItem);
          }
        }
      }
    }

    protected void NetworkSimRun()
    {
      while (this.networkSimulationSettings.IsSimulationEnabled)
      {
        int tickCount;
        SimulationItem simulationItem1;
        lock (this.NetSimListIncoming)
        {
          tickCount = Environment.TickCount;
          simulationItem1 = (SimulationItem) null;
          while (this.NetSimListIncoming.First != null)
          {
            SimulationItem simulationItem2 = this.NetSimListIncoming.First.Value;
            if (simulationItem2.stopw.ElapsedMilliseconds >= (long) simulationItem2.Delay)
            {
              simulationItem2.ActionToExecute();
              this.NetSimListIncoming.RemoveFirst();
            }
            else
              break;
          }
        }
        lock (this.NetSimListOutgoing)
        {
          tickCount = Environment.TickCount;
          simulationItem1 = (SimulationItem) null;
          while (this.NetSimListOutgoing.First != null)
          {
            SimulationItem simulationItem3 = this.NetSimListOutgoing.First.Value;
            if (simulationItem3.stopw.ElapsedMilliseconds >= (long) simulationItem3.Delay)
            {
              simulationItem3.ActionToExecute();
              this.NetSimListOutgoing.RemoveFirst();
            }
            else
              break;
          }
        }
        Thread.Sleep(2);
      }
      lock (this.NetSimListOutgoing)
      {
        while (this.NetSimListOutgoing.First != null)
        {
          this.NetSimListOutgoing.First.Value.ActionToExecute();
          this.NetSimListOutgoing.RemoveFirst();
        }
      }
    }

    internal void UpdateRoundTripTimeAndVariance(int lastRoundtripTime)
    {
      if (lastRoundtripTime < 0)
        return;
      this.roundTripTimeVariance -= this.roundTripTimeVariance / 4;
      if (lastRoundtripTime >= this.roundTripTime)
      {
        this.roundTripTime += (lastRoundtripTime - this.roundTripTime) / 8;
        this.roundTripTimeVariance += (lastRoundtripTime - this.roundTripTime) / 4;
      }
      else
      {
        this.roundTripTime += (lastRoundtripTime - this.roundTripTime) / 8;
        this.roundTripTimeVariance -= (lastRoundtripTime - this.roundTripTime) / 4;
      }
      if (this.roundTripTime < this.lowestRoundTripTime)
        this.lowestRoundTripTime = this.roundTripTime;
      if (this.roundTripTimeVariance <= this.highestRoundTripTimeVariance)
        return;
      this.highestRoundTripTimeVariance = this.roundTripTimeVariance;
    }

    internal void InitializeTrafficStats()
    {
      this.TrafficStatsIncoming = new TrafficStats(this.TrafficPackageHeaderSize);
      this.TrafficStatsOutgoing = new TrafficStats(this.TrafficPackageHeaderSize);
      this.TrafficStatsGameLevel = new TrafficStatsGameLevel();
      this.trafficStatsStopwatch = new Stopwatch();
    }

    internal static EndPoint GetEndpoint(string addressAndPort)
    {
      if (string.IsNullOrEmpty(addressAndPort))
        return (EndPoint) null;
      int length = addressAndPort.IndexOf(':');
      if (length < 0)
        return (EndPoint) null;
      string hostNameOrAddress = addressAndPort.Substring(0, length);
      int port = (int) short.Parse(addressAndPort.Substring(length + 1));
      foreach (IPAddress hostAddress in Dns.GetHostAddresses(hostNameOrAddress))
      {
        if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
          return (EndPoint) new IPEndPoint(hostAddress, port);
      }
      return (EndPoint) null;
    }

    internal delegate void MyAction();

    public enum ConnectionStateValue : byte
    {
      Disconnected = 0,
      Connecting = 1,
      Connected = 3,
      Disconnecting = 4,
      AcknowledgingDisconnect = 5,
      Zombie = 6,
    }

    internal enum EgMessageType : byte
    {
      Init = 0,
      InitResponse = 1,
      Operation = 2,
      OperationResponse = 3,
      Event = 4,
      InternalOperationRequest = 6,
      InternalOperationResponse = 7,
    }
  }
}
