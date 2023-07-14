// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.TPeer
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace ExitGames.Client.Photon
{
  internal class TPeer : PeerBase
  {
    internal static readonly byte[] tcpHead = new byte[9]
    {
      (byte) 251,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 243,
      (byte) 2
    };
    internal TConnect rt;
    private List<byte[]> outgoingOpList = new List<byte[]>();
    private List<byte[]> incomingList = new List<byte[]>();
    internal MemoryStream outgoingStream;
    private int lastPingResult;
    private byte[] pingRequest = new byte[5]
    {
      (byte) 240,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private byte proxyNodeId = 0;
    internal static readonly byte[] messageHeader = TPeer.tcpHead;

    internal override int QueuedIncomingCommandsCount => this.incomingList.Count;

    internal override int QueuedOutgoingCommandsCount => this.useOutgoingStream ? this.outgoingCommandsInStream : this.outgoingOpList.Count;

    internal byte ProxyNodeId => this.proxyNodeId;

    internal TPeer()
    {
      ++PeerBase.peerCount;
      this.InitOnce();
    }

    internal TPeer(IPhotonPeerListener listener)
      : this()
    {
      this.Listener = listener;
    }

    internal override bool Connect(string serverAddress, string appID, byte nodeId)
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Disconnected)
      {
        this.Listener.DebugReturn(DebugLevel.WARNING, "Connect() can't be called if peer is not Disconnected. Not connecting.");
        return false;
      }
      if (this.debugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "Connect()");
      this.ServerAddress = serverAddress;
      this.proxyNodeId = nodeId;
      this.InitPeerBase();
      this.outgoingStream = new MemoryStream(PeerBase.outgoingStreamBufferSize);
      if (appID == null)
        appID = "Lite";
      for (int index = 0; index < 32; ++index)
        this.INIT_BYTES[index + 9] = index < appID.Length ? (byte) appID[index] : (byte) 0;
      this.rt = new TConnect(this, this.ServerAddress);
      if (!this.rt.StartConnection())
        return false;
      this.peerConnectionState = PeerBase.ConnectionStateValue.Connecting;
      if (this.proxyNodeId > (byte) 0)
        this.SendProxyInit();
      this.EnqueueInit();
      this.SendOutgoingCommands();
      return true;
    }

    private void SendProxyInit()
    {
      if (this.proxyNodeId == (byte) 0)
        return;
      byte[] opData = new byte[2]
      {
        (byte) 241,
        this.proxyNodeId
      };
      if (this.TrafficStatsEnabled)
      {
        ++this.TrafficStatsOutgoing.TotalPacketCount;
        ++this.TrafficStatsOutgoing.TotalCommandsInPackets;
        this.TrafficStatsOutgoing.CountControlCommand(opData.Length);
      }
      this.rt.sendTcp(opData);
    }

    private void EnqueueInit()
    {
      MemoryStream output = new MemoryStream(0);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      byte[] numArray1 = new byte[7];
      numArray1[0] = (byte) 251;
      numArray1[6] = (byte) 1;
      byte[] numArray2 = numArray1;
      int targetOffset = 1;
      Protocol.Serialize(this.INIT_BYTES.Length + numArray2.Length, numArray2, ref targetOffset);
      binaryWriter.Write(numArray2);
      binaryWriter.Write(this.INIT_BYTES);
      byte[] array = output.ToArray();
      if (this.TrafficStatsEnabled)
      {
        ++this.TrafficStatsOutgoing.TotalPacketCount;
        ++this.TrafficStatsOutgoing.TotalCommandsInPackets;
        this.TrafficStatsOutgoing.CountControlCommand(array.Length);
      }
      this.EnqueueMessageAsPayload(true, array, (byte) 0);
    }

    internal override void Disconnect()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnecting)
        return;
      if (this.debugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "Disconnect()");
      this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnecting;
      this.outgoingOpList.Clear();
      this.rt.StopConnection();
    }

    internal void Disconnected()
    {
      this.InitPeerBase();
      this.Listener.OnStatusChanged(StatusCode.Disconnect);
    }

    internal override void StopConnection() => this.rt.StopConnection();

    internal bool EnqueueMessageAsPayload(bool sendReliable, byte[] opMessage, byte channelId)
    {
      if (opMessage == null)
        return false;
      opMessage[5] = channelId;
      opMessage[6] = sendReliable ? (byte) 1 : (byte) 0;
      if (!this.useOutgoingStream)
      {
        this.outgoingOpList.Add(opMessage);
      }
      else
      {
        this.outgoingStream.Write(opMessage, 0, opMessage.Length);
        ++this.outgoingCommandsInStream;
      }
      if (this.outgoingOpList.Count > 0 && this.outgoingOpList.Count % this.warningSize == 0)
        this.Listener.OnStatusChanged(StatusCode.QueueOutgoingReliableWarning);
      this.ByteCountLastOperation = opMessage.Length;
      if (this.TrafficStatsEnabled)
      {
        if (sendReliable)
          this.TrafficStatsOutgoing.CountReliableOpCommand(opMessage.Length);
        else
          this.TrafficStatsOutgoing.CountUnreliableOpCommand(opMessage.Length);
        this.TrafficStatsGameLevel.CountOperation(opMessage.Length);
      }
      return true;
    }

    internal override bool EnqueueOperation(
      Dictionary<byte, object> parameters,
      byte opCode,
      bool sendReliable,
      byte channelId,
      bool encrypt,
      PeerBase.EgMessageType messageType)
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Connected)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send op: Not connected. PeerState: " + (object) this.peerConnectionState);
        this.Listener.OnStatusChanged(StatusCode.SendError);
        return false;
      }
      if ((int) channelId >= (int) this.ChannelCount)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send op: Selected channel (" + (object) channelId + ")>= channelCount (" + (object) this.ChannelCount + ").");
        this.Listener.OnStatusChanged(StatusCode.SendError);
        return false;
      }
      byte[] message = this.SerializeOperationToMessage(opCode, parameters, messageType, encrypt);
      return this.EnqueueMessageAsPayload(sendReliable, message, channelId);
    }

    internal override void InitPeerBase()
    {
      base.InitPeerBase();
      this.outgoingOpList = new List<byte[]>();
      this.incomingList = new List<byte[]>();
    }

    internal override bool SendOutgoingCommands()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || !this.rt.isRunning)
        return false;
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected && this.GetLocalMsTimestamp() - this.lastPingResult > this.timePingInterval)
        this.SendPing();
      if (!this.useOutgoingStream)
      {
        if (this.outgoingOpList.Count > 0)
        {
          List<byte[]> outgoingOpList = this.outgoingOpList;
          this.outgoingOpList = new List<byte[]>();
          for (int index = 0; index < outgoingOpList.Count; ++index)
            this.sendData(outgoingOpList[index]);
        }
      }
      else if (this.outgoingStream.Position > 0L)
      {
        this.sendData(this.outgoingStream.ToArray());
        this.outgoingStream.Position = 0L;
        this.outgoingStream.SetLength(0L);
        this.outgoingCommandsInStream = 0;
      }
      return false;
    }

    internal override void FetchServerTimestamp()
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Connected)
      {
        if (this.debugOut >= DebugLevel.INFO)
          this.Listener.DebugReturn(DebugLevel.INFO, "FetchServerTimestamp() was skipped, as the client is not connected. Current ConnectionState: " + (object) this.peerConnectionState);
        this.Listener.OnStatusChanged(StatusCode.SendError);
      }
      else
      {
        this.SendPing();
        this.serverTimeOffsetIsAvailable = false;
      }
    }

    internal void SendPing()
    {
      int targetOffset = 1;
      Protocol.Serialize(this.GetLocalMsTimestamp(), this.pingRequest, ref targetOffset);
      this.lastPingResult = this.GetLocalMsTimestamp();
      if (this.TrafficStatsEnabled)
        this.TrafficStatsOutgoing.CountControlCommand(this.pingRequest.Length);
      this.sendData(this.pingRequest);
    }

    internal void sendData(byte[] data)
    {
      try
      {
        this.bytesOut += (long) data.Length;
        if (this.TrafficStatsEnabled)
        {
          ++this.TrafficStatsOutgoing.TotalPacketCount;
          ++this.TrafficStatsOutgoing.TotalCommandsInPackets;
        }
        if (this.NetworkSimulationSettings.IsSimulationEnabled)
          this.SendNetworkSimulated((PeerBase.MyAction) (() => this.rt.sendTcp(data)));
        else
          this.rt.sendTcp(data);
      }
      catch (Exception ex)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, ex.ToString());
        SupportClass.WriteStackTrace(ex, Console.Error);
      }
    }

    internal override bool DispatchIncomingCommands()
    {
      lock (this.ActionQueue)
      {
        while (this.ActionQueue.Count > 0)
          this.ActionQueue.Dequeue()();
      }
      byte[] incoming;
      lock (this.incomingList)
      {
        if (this.incomingList.Count <= 0)
          return false;
        incoming = this.incomingList[0];
        this.incomingList.RemoveAt(0);
      }
      this.ByteCountCurrentDispatch = incoming.Length + 3;
      return this.DeserializeMessageAndCallback(incoming);
    }

    internal override void receiveIncomingCommands(byte[] inbuff)
    {
      if (inbuff == null)
      {
        if (this.debugOut < DebugLevel.ERROR)
          return;
        this.EnqueueDebugReturn(DebugLevel.ERROR, "checkAndQueueIncomingCommands() inBuff: null");
      }
      else
      {
        this.timestampOfLastReceive = this.GetLocalMsTimestamp();
        this.bytesIn += (long) (inbuff.Length + 7);
        if (this.TrafficStatsEnabled)
        {
          ++this.TrafficStatsIncoming.TotalPacketCount;
          ++this.TrafficStatsIncoming.TotalCommandsInPackets;
        }
        if (inbuff[0] == (byte) 243 || inbuff[0] == (byte) 244)
        {
          lock (this.incomingList)
          {
            this.incomingList.Add(inbuff);
            if (this.incomingList.Count % this.warningSize != 0)
              return;
            this.EnqueueStatusCallback(StatusCode.QueueIncomingReliableWarning);
          }
        }
        else if (inbuff[0] == (byte) 240)
        {
          this.ReadPingResult(inbuff);
        }
        else
        {
          if (this.debugOut < DebugLevel.ERROR)
            return;
          this.EnqueueDebugReturn(DebugLevel.ERROR, "receiveIncomingCommands() MagicNumber should be 0xF0, 0xF3 or 0xF4. Is: " + (object) inbuff[0]);
        }
      }
    }

    internal void ReceiveProxyResponse(byte proxyResponseByte)
    {
      switch (proxyResponseByte)
      {
        case 0:
          this.Listener.OnStatusChanged(StatusCode.TcpRouterResponseOk);
          break;
        case 1:
          this.Listener.OnStatusChanged(StatusCode.TcpRouterResponseNodeIdUnknown);
          this.Disconnected();
          break;
        case 2:
          this.Listener.OnStatusChanged(StatusCode.TcpRouterResponseEndpointUnknown);
          this.Disconnected();
          break;
        case 16:
          this.Listener.OnStatusChanged(StatusCode.TcpRouterResponseNodeNotReady);
          this.Disconnected();
          break;
        default:
          this.Listener.DebugReturn(DebugLevel.ERROR, string.Format("ERROR: Unknown Proxy-Response: {0}" + (object) proxyResponseByte));
          this.Disconnected();
          break;
      }
    }

    private void ReadPingResult(byte[] inbuff)
    {
      int num1 = 0;
      int num2 = 0;
      int offset = 1;
      Protocol.Deserialize(out num1, inbuff, ref offset);
      Protocol.Deserialize(out num2, inbuff, ref offset);
      this.lastRoundTripTime = this.GetLocalMsTimestamp() - num2;
      if (!this.serverTimeOffsetIsAvailable)
        this.roundTripTime = this.lastRoundTripTime;
      this.UpdateRoundTripTimeAndVariance(this.lastRoundTripTime);
      if (this.serverTimeOffsetIsAvailable)
        return;
      this.serverTimeOffset = num1 + (this.lastRoundTripTime >> 1) - this.GetLocalMsTimestamp();
      this.serverTimeOffsetIsAvailable = true;
    }

    internal override byte[] SerializeOperationToMessage(
      byte opc,
      Dictionary<byte, object> parameters,
      PeerBase.EgMessageType messageType,
      bool encrypt)
    {
      byte[] array;
      lock (this.SerializeMemStream)
      {
        this.SerializeMemStream.Position = 0L;
        this.SerializeMemStream.SetLength(0L);
        if (!encrypt)
          this.SerializeMemStream.Write(TPeer.messageHeader, 0, TPeer.messageHeader.Length);
        Protocol.SerializeOperationRequest(this.SerializeMemStream, opc, parameters, false);
        if (encrypt)
        {
          byte[] buffer = this.CryptoProvider.Encrypt(this.SerializeMemStream.ToArray());
          this.SerializeMemStream.Position = 0L;
          this.SerializeMemStream.SetLength(0L);
          this.SerializeMemStream.Write(TPeer.messageHeader, 0, TPeer.messageHeader.Length);
          this.SerializeMemStream.Write(buffer, 0, buffer.Length);
        }
        array = this.SerializeMemStream.ToArray();
      }
      if (messageType != PeerBase.EgMessageType.Operation)
        array[TPeer.messageHeader.Length - 1] = (byte) messageType;
      if (encrypt)
        array[TPeer.messageHeader.Length - 1] = (byte) ((uint) array[TPeer.messageHeader.Length - 1] | 128U);
      int targetOffset = 1;
      Protocol.Serialize(array.Length, array, ref targetOffset);
      return array;
    }
  }
}
