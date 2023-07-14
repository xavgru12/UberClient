// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.EnetPeer
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace ExitGames.Client.Photon
{
  internal class EnetPeer : PeerBase
  {
    internal static readonly byte[] udpHeader0xF3 = new byte[2]
    {
      (byte) 243,
      (byte) 2
    };
    private Dictionary<byte, EnetChannel> channels = new Dictionary<byte, EnetChannel>();
    private List<NCommand> sentReliableCommands = new List<NCommand>();
    private Queue<NCommand> outgoingAcknowledgementsList = new Queue<NCommand>();
    internal readonly int windowSize = 128;
    internal int[] unsequencedWindow;
    internal int outgoingUnsequencedGroupNumber;
    internal int incomingUnsequencedGroupNumber;
    internal byte udpCommandCount;
    internal byte[] udpBuffer;
    internal int udpBufferIndex;
    internal int commandSize = 12;
    internal int challenge;
    internal NConnect rt;
    internal int ReliableCommandsRepeated;
    internal int packetLoss;
    internal int ReliableCommandsSent;
    internal int packetLossEpoch;
    internal int packetLossVariance;
    internal int packetThrottleEpoch;
    internal int serverSentTime;
    internal static readonly byte[] messageHeader = EnetPeer.udpHeader0xF3;

    internal override int QueuedIncomingCommandsCount
    {
      get
      {
        int incomingCommandsCount = 0;
        foreach (EnetChannel enetChannel in this.channels.Values)
        {
          incomingCommandsCount += enetChannel.incomingReliableCommandsList.Count;
          incomingCommandsCount += enetChannel.incomingUnreliableCommandsList.Count;
        }
        return incomingCommandsCount;
      }
    }

    internal override int QueuedOutgoingCommandsCount
    {
      get
      {
        int outgoingCommandsCount = 0;
        foreach (EnetChannel enetChannel in this.channels.Values)
        {
          outgoingCommandsCount += enetChannel.outgoingReliableCommandsList.Count;
          outgoingCommandsCount += enetChannel.outgoingUnreliableCommandsList.Count;
        }
        return outgoingCommandsCount;
      }
    }

    internal EnetPeer()
    {
      ++PeerBase.peerCount;
      this.InitOnce();
      this.TrafficPackageHeaderSize = 12;
    }

    internal EnetPeer(IPhotonPeerListener listener)
      : this()
    {
      this.Listener = listener;
    }

    internal override bool Connect(string ipport, string appID, byte nodeId)
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Disconnected)
      {
        this.Listener.DebugReturn(DebugLevel.WARNING, "Connect() can't be called if peer is not Disconnected. Not connecting.");
        return false;
      }
      if (this.debugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "Connect()");
      this.ServerAddress = ipport;
      this.InitPeerBase();
      if (appID == null)
        appID = "Lite";
      for (int index = 0; index < 32; ++index)
        this.INIT_BYTES[index + 9] = index < appID.Length ? (byte) appID[index] : (byte) 0;
      this.rt = new NConnect(this, this.ServerAddress);
      if (!this.rt.StartConnection())
        return false;
      if (this.TrafficStatsEnabled)
      {
        this.TrafficStatsOutgoing.ControlCommandBytes += 44;
        ++this.TrafficStatsOutgoing.ControlCommandCount;
      }
      this.peerConnectionState = PeerBase.ConnectionStateValue.Connecting;
      return true;
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
        this.CreateAndEnqueueCommand((byte) 12, new byte[0], byte.MaxValue);
    }

    internal override void Disconnect()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnecting)
        return;
      if (this.debugOut >= DebugLevel.INFO)
        this.Listener.DebugReturn(DebugLevel.INFO, "Disconnect()");
      if (this.outgoingAcknowledgementsList != null)
      {
        lock (this.outgoingAcknowledgementsList)
          this.outgoingAcknowledgementsList.Clear();
      }
      if (this.sentReliableCommands != null)
      {
        lock (this.sentReliableCommands)
          this.sentReliableCommands.Clear();
      }
      if (this.channels != null)
      {
        foreach (EnetChannel enetChannel in this.channels.Values)
          enetChannel.clearAll();
      }
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected)
      {
        NCommand command = new NCommand(this, (byte) 4, (byte[]) null, byte.MaxValue);
        this.queueOutgoingReliableCommand(command);
        if (this.TrafficStatsEnabled)
          this.TrafficStatsOutgoing.CountControlCommand(command.Size);
        this.SendOutgoingCommands();
        this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnecting;
      }
      else
        this.rt.StopConnection();
    }

    internal void Disconnected()
    {
      this.InitPeerBase();
      this.Listener.OnStatusChanged(StatusCode.Disconnect);
    }

    internal override void StopConnection() => this.rt.StopConnection();

    internal bool CreateAndEnqueueCommand(byte commandType, byte[] payload, byte channelNumber)
    {
      if (payload == null)
        return false;
      EnetChannel channel = this.channels[channelNumber];
      this.ByteCountLastOperation = 0;
      int count = this.mtu - 12 - 32;
      if (payload.Length > count)
      {
        int num1 = (payload.Length + count - 1) / count;
        int num2 = channel.outgoingReliableSequenceNumber + 1;
        int num3 = 0;
        for (int srcOffset = 0; srcOffset < payload.Length; srcOffset += count)
        {
          if (payload.Length - srcOffset < count)
            count = payload.Length - srcOffset;
          byte[] numArray = new byte[count];
          Buffer.BlockCopy((Array) payload, srcOffset, (Array) numArray, 0, count);
          NCommand command = new NCommand(this, (byte) 8, numArray, channel.ChannelNumber);
          command.fragmentNumber = num3;
          command.startSequenceNumber = num2;
          command.fragmentCount = num1;
          command.totalLength = payload.Length;
          command.fragmentOffset = srcOffset;
          this.queueOutgoingReliableCommand(command);
          this.ByteCountLastOperation += command.Size;
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsOutgoing.CountFragmentOpCommand(command.Size);
            this.TrafficStatsGameLevel.CountOperation(command.Size);
          }
          ++num3;
        }
      }
      else
      {
        NCommand command = new NCommand(this, commandType, payload, channel.ChannelNumber);
        if (command.commandFlags == (byte) 1)
        {
          this.queueOutgoingReliableCommand(command);
          this.ByteCountLastOperation = command.Size;
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsOutgoing.CountReliableOpCommand(command.Size);
            this.TrafficStatsGameLevel.CountOperation(command.Size);
          }
        }
        else
        {
          this.queueOutgoingUnreliableCommand(command);
          this.ByteCountLastOperation = command.Size;
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsOutgoing.CountUnreliableOpCommand(command.Size);
            this.TrafficStatsGameLevel.CountOperation(command.Size);
          }
        }
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
      return this.CreateAndEnqueueCommand(sendReliable ? (byte) 6 : (byte) 7, message, channelId);
    }

    internal override void InitPeerBase()
    {
      base.InitPeerBase();
      this.peerID = (short) -1;
      this.challenge = SupportClass.ThreadSafeRandom.Next();
      this.ReliableCommandsSent = 0;
      this.ReliableCommandsRepeated = 0;
      this.packetLoss = 0;
      this.channels = new Dictionary<byte, EnetChannel>();
      this.channels[byte.MaxValue] = new EnetChannel(byte.MaxValue, this.commandBufferSize);
      for (byte index = 0; (int) index < (int) this.ChannelCount; ++index)
        this.channels[index] = new EnetChannel(index, this.commandBufferSize);
      lock (this.sentReliableCommands)
        this.sentReliableCommands = new List<NCommand>(this.commandBufferSize);
      lock (this.outgoingAcknowledgementsList)
        this.outgoingAcknowledgementsList = new Queue<NCommand>(this.commandBufferSize);
    }

    internal override bool SendOutgoingCommands()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || !this.rt.isRunning)
        return false;
      int num = 0;
      lock (this.SendOutgoingLockObject)
      {
        this.udpBuffer = new byte[this.mtu];
        this.udpBufferIndex = 12;
        this.udpCommandCount = (byte) 0;
        this.timeInt = this.GetLocalMsTimestamp() - this.timeBase;
        lock (this.outgoingAcknowledgementsList)
        {
          if (this.outgoingAcknowledgementsList.Count > 0)
            num = this.serializeToBuffer(this.outgoingAcknowledgementsList);
        }
        if (!this.isSendingOnlyAcks && this.timeInt > this.timeoutInt && this.sentReliableCommands.Count > 0)
        {
          lock (this.sentReliableCommands)
          {
            NCommand[] array = new NCommand[this.sentReliableCommands.Count];
            this.sentReliableCommands.CopyTo(array);
            foreach (NCommand command in array)
            {
              if (command != null && this.timeInt - command.commandSentTime > command.roundTripTimeout)
              {
                if ((int) command.commandSentCount > this.sentCountAllowance || this.timeInt > command.timeoutTime)
                {
                  if (this.debugOut >= DebugLevel.INFO)
                    this.Listener.DebugReturn(DebugLevel.INFO, "Timeout-disconnect! Command: " + (object) command + " now: " + (object) this.timeInt + " challenge: " + Convert.ToString(this.challenge, 16));
                  this.Listener.OnStatusChanged(StatusCode.TimeoutDisconnect);
                  this.Disconnected();
                  return false;
                }
                this.queueOutgoingReliableCommand(command);
                this.sentReliableCommands.Remove(command);
                ++this.ReliableCommandsRepeated;
                if (this.debugOut >= DebugLevel.INFO)
                  this.Listener.DebugReturn(DebugLevel.INFO, "resending command! " + (object) command + " Now=" + (object) this.timeInt + " Rtt/RttV=" + (object) this.roundTripTime + "/" + (object) this.roundTripTimeVariance + "  command.roundTriptimeOut = " + (object) command.roundTripTimeout + "  lastRoundTripTime=" + (object) this.lastRoundTripTime);
              }
            }
          }
        }
        if (!this.isSendingOnlyAcks && this.peerConnectionState == PeerBase.ConnectionStateValue.Connected && this.sentReliableCommands.Count == 0 && this.timePingInterval > 0 && this.timeInt - this.timeLastAckReceive > this.timePingInterval && this.udpBufferIndex + 12 < this.udpBuffer.Length)
        {
          NCommand command = new NCommand(this, (byte) 5, (byte[]) null, byte.MaxValue);
          this.queueOutgoingReliableCommand(command);
          if (this.TrafficStatsEnabled)
            this.TrafficStatsOutgoing.CountControlCommand(command.Size);
        }
        if (!this.isSendingOnlyAcks)
        {
          foreach (EnetChannel enetChannel in this.channels.Values)
          {
            num += this.serializeToBuffer(enetChannel.outgoingReliableCommandsList);
            num += this.serializeToBuffer(enetChannel.outgoingUnreliableCommandsList);
          }
        }
        if (this.udpCommandCount <= (byte) 0)
          return false;
        if (this.TrafficStatsEnabled)
        {
          ++this.TrafficStatsOutgoing.TotalPacketCount;
          this.TrafficStatsOutgoing.TotalCommandsInPackets += (int) this.udpCommandCount;
        }
        this.sendData(this.udpBuffer, this.udpBufferIndex);
      }
      return num > 0;
    }

    internal void sendData(byte[] data, int length)
    {
      try
      {
        SupportClass.NumberToByteArray(data, 0, this.peerID);
        data[2] = (byte) 0;
        data[3] = this.udpCommandCount;
        SupportClass.NumberToByteArray(data, 4, this.timeInt);
        SupportClass.NumberToByteArray(data, 8, this.challenge);
        this.bytesOut += (long) length;
        if (this.NetworkSimulationSettings.IsSimulationEnabled)
          this.SendNetworkSimulated((PeerBase.MyAction) (() => this.rt.SendUdpPackage(data, length)));
        else
          this.rt.SendUdpPackage(data, length);
      }
      catch (Exception ex)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, ex.ToString());
        SupportClass.WriteStackTrace(ex, Console.Error);
      }
    }

    internal int serializeToBuffer(Queue<NCommand> commandList)
    {
      while (commandList.Count > 0)
      {
        NCommand command = commandList.Peek();
        if (command == null)
        {
          commandList.Dequeue();
        }
        else
        {
          if (this.udpBufferIndex + command.Size > this.udpBuffer.Length)
          {
            if (this.debugOut >= DebugLevel.INFO)
            {
              this.Listener.DebugReturn(DebugLevel.INFO, "UDP package is full. Commands in Package: " + (object) this.udpCommandCount + ". Commands left in queue: " + (object) commandList.Count);
              break;
            }
            break;
          }
          Buffer.BlockCopy((Array) command.Serialize(), 0, (Array) this.udpBuffer, this.udpBufferIndex, command.Size);
          this.udpBufferIndex += command.Size;
          ++this.udpCommandCount;
          if (((int) command.commandFlags & 1) > 0)
            this.queueSentCommand(command);
          commandList.Dequeue();
        }
      }
      return commandList.Count;
    }

    internal void queueSentCommand(NCommand command)
    {
      command.commandSentTime = this.timeInt;
      ++command.commandSentCount;
      if (command.roundTripTimeout == 0)
      {
        command.roundTripTimeout = this.roundTripTime + 4 * this.roundTripTimeVariance;
        command.timeoutTime = this.timeInt + this.DisconnectTimeout;
      }
      else
        command.roundTripTimeout *= 2;
      lock (this.sentReliableCommands)
      {
        if (this.sentReliableCommands.Count == 0)
          this.timeoutInt = command.commandSentTime + command.roundTripTimeout;
        ++this.ReliableCommandsSent;
        this.sentReliableCommands.Add(command);
      }
      if (this.sentReliableCommands.Count < this.warningSize || this.sentReliableCommands.Count % this.warningSize != 0)
        return;
      this.Listener.OnStatusChanged(StatusCode.QueueSentWarning);
    }

    internal void queueOutgoingReliableCommand(NCommand command)
    {
      EnetChannel channel = this.channels[command.commandChannelID];
      lock (channel)
      {
        Queue<NCommand> reliableCommandsList = channel.outgoingReliableCommandsList;
        if (reliableCommandsList.Count >= this.warningSize && reliableCommandsList.Count % this.warningSize == 0)
          this.Listener.OnStatusChanged(StatusCode.QueueOutgoingReliableWarning);
        if (command.reliableSequenceNumber == 0)
          command.reliableSequenceNumber = ++channel.outgoingReliableSequenceNumber;
        reliableCommandsList.Enqueue(command);
      }
    }

    internal void queueOutgoingUnreliableCommand(NCommand command)
    {
      Queue<NCommand> unreliableCommandsList = this.channels[command.commandChannelID].outgoingUnreliableCommandsList;
      if (unreliableCommandsList.Count >= this.warningSize && unreliableCommandsList.Count % this.warningSize == 0)
        this.Listener.OnStatusChanged(StatusCode.QueueOutgoingUnreliableWarning);
      EnetChannel channel = this.channels[command.commandChannelID];
      command.reliableSequenceNumber = channel.outgoingReliableSequenceNumber;
      command.unreliableSequenceNumber = ++channel.outgoingUnreliableSequenceNumber;
      unreliableCommandsList.Enqueue(command);
    }

    internal void queueOutgoingAcknowledgement(NCommand command)
    {
      lock (this.outgoingAcknowledgementsList)
      {
        if (this.outgoingAcknowledgementsList.Count >= this.warningSize && this.outgoingAcknowledgementsList.Count % this.warningSize == 0)
          this.Listener.OnStatusChanged(StatusCode.QueueOutgoingAcksWarning);
        this.outgoingAcknowledgementsList.Enqueue(command);
      }
    }

    internal override bool DispatchIncomingCommands()
    {
      lock (this.ActionQueue)
      {
        while (this.ActionQueue.Count > 0)
          this.ActionQueue.Dequeue()();
      }
      NCommand ncommand1 = (NCommand) null;
      foreach (EnetChannel enetChannel in this.channels.Values)
      {
        if (enetChannel.incomingUnreliableCommandsList.Count > 0)
        {
          int[] array = new int[enetChannel.incomingUnreliableCommandsList.Count];
          enetChannel.incomingUnreliableCommandsList.Keys.CopyTo(array, 0);
          int key1 = int.MaxValue;
          foreach (int key2 in array)
          {
            NCommand unreliableCommands = enetChannel.incomingUnreliableCommandsList[key2];
            if (key2 < enetChannel.incomingUnreliableSequenceNumber || unreliableCommands.reliableSequenceNumber < enetChannel.incomingReliableSequenceNumber)
              enetChannel.incomingUnreliableCommandsList.Remove(key2);
            else if (this.limitOfUnreliableCommands > 0 && enetChannel.incomingUnreliableCommandsList.Count > this.limitOfUnreliableCommands)
              enetChannel.incomingUnreliableCommandsList.Remove(key2);
            else if (key2 < key1 && unreliableCommands.reliableSequenceNumber <= enetChannel.incomingReliableSequenceNumber)
              key1 = key2;
          }
          if (key1 < int.MaxValue)
            ncommand1 = enetChannel.incomingUnreliableCommandsList[key1];
          if (ncommand1 != null)
          {
            enetChannel.incomingUnreliableCommandsList.Remove(ncommand1.unreliableSequenceNumber);
            enetChannel.incomingUnreliableSequenceNumber = ncommand1.unreliableSequenceNumber;
            break;
          }
        }
        if (ncommand1 == null && enetChannel.incomingReliableCommandsList.Count > 0)
        {
          enetChannel.incomingReliableCommandsList.TryGetValue(enetChannel.incomingReliableSequenceNumber + 1, out ncommand1);
          if (ncommand1 != null)
          {
            if (ncommand1.commandType != (byte) 8)
            {
              enetChannel.incomingReliableSequenceNumber = ncommand1.reliableSequenceNumber;
              enetChannel.incomingReliableCommandsList.Remove(ncommand1.reliableSequenceNumber);
              break;
            }
            if (ncommand1.fragmentsRemaining > 0)
            {
              ncommand1 = (NCommand) null;
              break;
            }
            byte[] dst = new byte[ncommand1.totalLength];
            for (int startSequenceNumber = ncommand1.startSequenceNumber; startSequenceNumber < ncommand1.startSequenceNumber + ncommand1.fragmentCount; ++startSequenceNumber)
            {
              NCommand ncommand2 = enetChannel.ContainsReliableSequenceNumber(startSequenceNumber) ? enetChannel.FetchReliableSequenceNumber(startSequenceNumber) : throw new Exception("command.fragmentsRemaining was 0, but not all fragments are found to be combined!");
              Buffer.BlockCopy((Array) ncommand2.Payload, 0, (Array) dst, ncommand2.fragmentOffset, ncommand2.Payload.Length);
              enetChannel.incomingReliableCommandsList.Remove(ncommand2.reliableSequenceNumber);
            }
            if (this.debugOut >= DebugLevel.ALL)
              this.Listener.DebugReturn(DebugLevel.ALL, "assembled fragmented payload from " + (object) ncommand1.fragmentCount + " parts. Dispatching now.");
            ncommand1.Payload = dst;
            ncommand1.Size = 12 * ncommand1.fragmentCount + ncommand1.totalLength;
            enetChannel.incomingReliableSequenceNumber = ncommand1.reliableSequenceNumber + ncommand1.fragmentCount - 1;
            break;
          }
        }
      }
      if (ncommand1 != null && ncommand1.Payload != null)
      {
        this.ByteCountCurrentDispatch = ncommand1.Size;
        if (this.DeserializeMessageAndCallback(ncommand1.Payload))
          return true;
      }
      return false;
    }

    internal override void receiveIncomingCommands(byte[] inBuff)
    {
      this.timestampOfLastReceive = this.GetLocalMsTimestamp();
      try
      {
        int num1 = 0;
        Protocol.Deserialize(out short _, inBuff, ref num1);
        byte num2 = inBuff[num1++];
        byte num3 = inBuff[num1++];
        Protocol.Deserialize(out this.serverSentTime, inBuff, ref num1);
        int num4;
        Protocol.Deserialize(out num4, inBuff, ref num1);
        this.bytesIn += 12L;
        if (this.TrafficStatsEnabled)
        {
          ++this.TrafficStatsIncoming.TotalPacketCount;
          this.TrafficStatsIncoming.TotalCommandsInPackets += (int) num3;
        }
        if ((int) num3 > this.commandBufferSize)
          this.EnqueueDebugReturn(DebugLevel.ALL, "too many incoming commands in packet: " + (object) num3 + " > " + (object) this.commandBufferSize);
        if (num4 != this.challenge)
        {
          if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || this.debugOut < DebugLevel.ALL)
            return;
          this.EnqueueDebugReturn(DebugLevel.ALL, "Info: received package with wrong challenge. challenge in/out:" + (object) num4 + "!=" + (object) this.challenge + " Commands in it: " + (object) num3);
        }
        else
        {
          this.timeInt = this.GetLocalMsTimestamp() - this.timeBase;
          for (int index = 0; index < (int) num3; ++index)
          {
            NCommand readCommand = new NCommand(this, inBuff, ref num1);
            if (readCommand.commandType != (byte) 1)
              this.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.executeCommand(readCommand)));
            else
              this.executeCommand(readCommand);
            if (((int) readCommand.commandFlags & 1) > 0)
            {
              NCommand ack = NCommand.CreateAck(this, readCommand, this.serverSentTime);
              this.queueOutgoingAcknowledgement(ack);
              if (this.TrafficStatsEnabled)
                this.TrafficStatsOutgoing.CountControlCommand(ack.Size);
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.EnqueueDebugReturn(DebugLevel.ERROR, string.Format("Exception while reading commands from incoming data: {0}", (object) ex));
        SupportClass.WriteStackTrace(ex, Console.Error);
      }
    }

    internal bool executeCommand(NCommand command)
    {
      bool flag = true;
      switch (command.commandType)
      {
        case 1:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountControlCommand(command.Size);
          this.timeLastAckReceive = this.timeInt;
          this.lastRoundTripTime = this.timeInt - command.ackReceivedSentTime;
          NCommand ncommand = this.removeSentReliableCommand(command.ackReceivedReliableSequenceNumber, (int) command.commandChannelID);
          if (ncommand != null)
          {
            if (ncommand.commandType == (byte) 12)
            {
              if (this.lastRoundTripTime <= this.roundTripTime)
              {
                this.serverTimeOffset = this.serverSentTime + (this.lastRoundTripTime >> 1) - this.GetLocalMsTimestamp();
                this.serverTimeOffsetIsAvailable = true;
              }
              else
                this.FetchServerTimestamp();
            }
            else
            {
              this.UpdateRoundTripTimeAndVariance(this.lastRoundTripTime);
              if (ncommand.commandType == (byte) 4 && this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnecting)
              {
                if (this.debugOut >= DebugLevel.INFO)
                  this.EnqueueDebugReturn(DebugLevel.INFO, "Received disconnect ACK by server");
                this.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.rt.StopConnection()));
              }
              else if (ncommand.commandType == (byte) 2)
                this.roundTripTime = this.lastRoundTripTime;
            }
            break;
          }
          break;
        case 2:
        case 5:
          if (this.TrafficStatsEnabled)
          {
            this.TrafficStatsIncoming.CountControlCommand(command.Size);
            break;
          }
          break;
        case 3:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountControlCommand(command.Size);
          if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connecting)
          {
            command = new NCommand(this, (byte) 6, this.INIT_BYTES, (byte) 0);
            this.queueOutgoingReliableCommand(command);
            if (this.TrafficStatsEnabled)
              this.TrafficStatsOutgoing.CountControlCommand(command.Size);
            this.peerConnectionState = PeerBase.ConnectionStateValue.Connected;
            break;
          }
          break;
        case 4:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountControlCommand(command.Size);
          StatusCode statusCode = StatusCode.DisconnectByServer;
          if (command.reservedByte == (byte) 1)
            statusCode = StatusCode.DisconnectByServerLogic;
          else if (command.reservedByte == (byte) 3)
            statusCode = StatusCode.DisconnectByServerUserLimit;
          if (this.debugOut >= DebugLevel.INFO)
            this.Listener.DebugReturn(DebugLevel.INFO, "Server sent disconnect. PeerId: " + (object) this.peerID + " RTT/Variance:" + (object) this.roundTripTime + "/" + (object) this.roundTripTimeVariance);
          this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnecting;
          this.Listener.OnStatusChanged(statusCode);
          this.rt.StopConnection();
          break;
        case 6:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountReliableOpCommand(command.Size);
          if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected)
          {
            flag = this.queueIncomingCommand(command);
            break;
          }
          break;
        case 7:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountUnreliableOpCommand(command.Size);
          if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected)
          {
            flag = this.queueIncomingCommand(command);
            break;
          }
          break;
        case 8:
          if (this.TrafficStatsEnabled)
            this.TrafficStatsIncoming.CountFragmentOpCommand(command.Size);
          if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected)
          {
            if (command.fragmentNumber > command.fragmentCount || command.fragmentOffset >= command.totalLength || command.fragmentOffset + command.Payload.Length > command.totalLength)
            {
              if (this.debugOut >= DebugLevel.ERROR)
              {
                this.Listener.DebugReturn(DebugLevel.ERROR, "Received fragment has bad size: " + (object) command);
                break;
              }
              break;
            }
            flag = this.queueIncomingCommand(command);
            if (flag)
            {
              EnetChannel channel = this.channels[command.commandChannelID];
              if (command.reliableSequenceNumber == command.startSequenceNumber)
              {
                --command.fragmentsRemaining;
                int num = command.startSequenceNumber + 1;
                while (command.fragmentsRemaining > 0 && num < command.startSequenceNumber + command.fragmentCount)
                {
                  if (channel.ContainsReliableSequenceNumber(num++))
                    --command.fragmentsRemaining;
                }
              }
              else if (channel.ContainsReliableSequenceNumber(command.startSequenceNumber))
                --channel.FetchReliableSequenceNumber(command.startSequenceNumber).fragmentsRemaining;
            }
            break;
          }
          break;
      }
      return flag;
    }

    internal bool queueIncomingCommand(NCommand command)
    {
      EnetChannel enetChannel = (EnetChannel) null;
      this.channels.TryGetValue(command.commandChannelID, out enetChannel);
      if (enetChannel == null)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, "Received command for non-existing channel: " + (object) command.commandChannelID);
        return false;
      }
      if (this.debugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "queueIncomingCommand( " + (object) command + " )  -  incomingReliableSequenceNumber: " + (object) enetChannel.incomingReliableSequenceNumber);
      if (command.commandFlags == (byte) 1)
      {
        if (command.reliableSequenceNumber <= enetChannel.incomingReliableSequenceNumber)
        {
          if (this.debugOut >= DebugLevel.INFO)
            this.Listener.DebugReturn(DebugLevel.INFO, "incoming command " + command.ToString() + " is old (not saving it). Dispatched incomingReliableSequenceNumber: " + (object) enetChannel.incomingReliableSequenceNumber);
          return false;
        }
        if (enetChannel.ContainsReliableSequenceNumber(command.reliableSequenceNumber))
        {
          if (this.debugOut >= DebugLevel.INFO)
            this.Listener.DebugReturn(DebugLevel.INFO, "Info: command was received before! Old/New: " + (object) enetChannel.FetchReliableSequenceNumber(command.reliableSequenceNumber) + "/" + (object) command + " inReliableSeq#: " + (object) enetChannel.incomingReliableSequenceNumber);
          return false;
        }
        if (enetChannel.incomingReliableCommandsList.Count >= this.warningSize && enetChannel.incomingReliableCommandsList.Count % this.warningSize == 0)
          this.Listener.OnStatusChanged(StatusCode.QueueIncomingReliableWarning);
        enetChannel.incomingReliableCommandsList.Add(command.reliableSequenceNumber, command);
        return true;
      }
      if (command.commandFlags != (byte) 0)
        return false;
      if (this.debugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "unreliable. local: " + (object) enetChannel.incomingReliableSequenceNumber + "/" + (object) enetChannel.incomingUnreliableSequenceNumber + " incoming: " + (object) command.reliableSequenceNumber + "/" + (object) command.unreliableSequenceNumber);
      if (command.reliableSequenceNumber < enetChannel.incomingReliableSequenceNumber)
      {
        if (this.debugOut >= DebugLevel.INFO)
          this.Listener.DebugReturn(DebugLevel.INFO, "incoming reliable-seq# < Dispatched-rel-seq#. not saved.");
        return true;
      }
      if (command.unreliableSequenceNumber <= enetChannel.incomingUnreliableSequenceNumber)
      {
        if (this.debugOut >= DebugLevel.INFO)
          this.Listener.DebugReturn(DebugLevel.INFO, "incoming unreliable-seq# < Dispatched-unrel-seq#. not saved.");
        return true;
      }
      if (enetChannel.ContainsUnreliableSequenceNumber(command.unreliableSequenceNumber))
      {
        if (this.debugOut >= DebugLevel.INFO)
          this.Listener.DebugReturn(DebugLevel.INFO, "command was received before! Old/New: " + (object) enetChannel.incomingUnreliableCommandsList[command.unreliableSequenceNumber] + "/" + (object) command);
        return false;
      }
      if (enetChannel.incomingUnreliableCommandsList.Count >= this.warningSize && enetChannel.incomingUnreliableCommandsList.Count % this.warningSize == 0)
        this.Listener.OnStatusChanged(StatusCode.QueueIncomingUnreliableWarning);
      enetChannel.incomingUnreliableCommandsList.Add(command.unreliableSequenceNumber, command);
      return true;
    }

    internal NCommand removeSentReliableCommand(
      int ackReceivedReliableSequenceNumber,
      int ackReceivedChannel)
    {
      NCommand ncommand = (NCommand) null;
      lock (this.sentReliableCommands)
      {
        foreach (NCommand sentReliableCommand in this.sentReliableCommands)
        {
          if (sentReliableCommand != null && sentReliableCommand.reliableSequenceNumber == ackReceivedReliableSequenceNumber && (int) sentReliableCommand.commandChannelID == ackReceivedChannel)
          {
            ncommand = sentReliableCommand;
            break;
          }
        }
        if (ncommand != null)
        {
          this.sentReliableCommands.Remove(ncommand);
          if (this.sentReliableCommands.Count > 0)
            this.timeoutInt = this.sentReliableCommands[0].commandSentTime + this.sentReliableCommands[0].roundTripTimeout;
        }
        else if (this.debugOut >= DebugLevel.ALL && this.peerConnectionState != PeerBase.ConnectionStateValue.Connected && this.peerConnectionState != PeerBase.ConnectionStateValue.Disconnecting)
          this.Listener.DebugReturn(DebugLevel.ALL, string.Format("No sent command for ACK (Ch: {0} Sq#: {1}). PeerState: {2}.", (object) ackReceivedReliableSequenceNumber, (object) ackReceivedChannel, (object) this.peerConnectionState));
      }
      return ncommand;
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
          this.SerializeMemStream.Write(EnetPeer.messageHeader, 0, EnetPeer.messageHeader.Length);
        Protocol.SerializeOperationRequest(this.SerializeMemStream, opc, parameters, false);
        if (encrypt)
        {
          byte[] buffer = this.CryptoProvider.Encrypt(this.SerializeMemStream.ToArray());
          this.SerializeMemStream.Position = 0L;
          this.SerializeMemStream.SetLength(0L);
          this.SerializeMemStream.Write(EnetPeer.messageHeader, 0, EnetPeer.messageHeader.Length);
          this.SerializeMemStream.Write(buffer, 0, buffer.Length);
        }
        array = this.SerializeMemStream.ToArray();
      }
      if (messageType != PeerBase.EgMessageType.Operation)
        array[EnetPeer.messageHeader.Length - 1] = (byte) messageType;
      if (encrypt)
        array[EnetPeer.messageHeader.Length - 1] = (byte) ((uint) array[EnetPeer.messageHeader.Length - 1] | 128U);
      return array;
    }

    internal string commandListToString(NCommand[] list)
    {
      if (this.debugOut < DebugLevel.ALL)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < list.Length; ++index)
      {
        stringBuilder.Append(index.ToString() + "=");
        stringBuilder.Append((object) list[index]);
        stringBuilder.Append(" # ");
      }
      return stringBuilder.ToString();
    }
  }
}
