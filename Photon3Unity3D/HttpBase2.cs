// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.HttpBase2
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ExitGames.Client.Photon
{
  internal class HttpBase2 : PeerBase
  {
    private string HttpPeerID;
    private string UrlParameters;
    private long lastPingTimeStamp;
    private List<byte[]> incomingList = new List<byte[]>();
    private List<WWW> webRequests = new List<WWW>();
    private WWW disconnectRequest;
    internal static readonly byte[] messageHeader = new byte[2]
    {
      (byte) 243,
      (byte) 2
    };

    public override string PeerID => this.HttpPeerID;

    internal HttpBase2()
    {
      ++PeerBase.peerCount;
      this.InitOnce();
      this.usedProtocol = ConnectionProtocol.Http;
    }

    internal HttpBase2(IPhotonPeerListener listener)
      : this()
    {
      this.Listener = listener;
    }

    internal void Request(byte[] data, string urlParamter) => this.Request(data, urlParamter, false);

    internal void Request(byte[] data, string urlParamter, bool isDisconnect)
    {
      string url = this.ServerAddress + urlParamter + this.HttpUrlParameters;
      if (this.debugOut >= DebugLevel.INFO)
        this.Listener.DebugReturn(DebugLevel.INFO, "Request() " + url + ". data.Length: " + (object) (data == null ? 0 : data.Length));
      try
      {
        this.lastPingTimeStamp = (long) this.GetLocalMsTimestamp();
        WWW www = new WWW(url, data);
        this.webRequests.Add(www);
        if (!isDisconnect)
          return;
        this.disconnectRequest = www;
      }
      catch (Exception ex)
      {
        this.EnqueueDebugReturn(DebugLevel.ERROR, "Exception Url: " + url);
        SupportClass.WriteStackTrace(ex, Console.Out);
        this.EnqueueErrorDisconnect(StatusCode.Exception);
      }
    }

    internal bool CheckResult()
    {
      if (this.webRequests == null || this.webRequests.Count == 0 || !this.webRequests[0].isDone)
        return false;
      WWW webRequest = this.webRequests[0];
      this.webRequests.RemoveAt(0);
      if (webRequest.error != null)
      {
        this.EnqueueDebugReturn(DebugLevel.ERROR, "WWW request error: " + webRequest.error + " URL: " + webRequest.url);
        this.EnqueueErrorDisconnect(StatusCode.Exception);
      }
      else if (webRequest == this.disconnectRequest)
      {
        this.disconnectRequest = (WWW) null;
        this.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.Disconnected()));
      }
      else
        this.receiveIncomingCommands(webRequest.bytes);
      return true;
    }

    internal override int QueuedIncomingCommandsCount => 0;

    internal override int QueuedOutgoingCommandsCount => 0;

    internal override bool Connect(string serverAddress, string appID, byte nodeId)
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Disconnected)
      {
        this.Listener.DebugReturn(DebugLevel.WARNING, "Connect() called while peerConnectionState != Disconnected. Nothing done.");
        return false;
      }
      this.peerConnectionState = PeerBase.ConnectionStateValue.Connecting;
      this.ServerAddress = serverAddress;
      this.HttpPeerID = Guid.Empty.ToString();
      this.UrlParameters = "?init";
      if (appID == null)
        appID = "Lite";
      for (int index = 0; index < 32; ++index)
        this.INIT_BYTES[index + 9] = index < appID.Length ? (byte) appID[index] : (byte) 0;
      this.Request(this.INIT_BYTES, "?init");
      return true;
    }

    internal override void Disconnect()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnected || this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnecting)
        return;
      this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnecting;
      foreach (WWW webRequest in this.webRequests)
        webRequest.Dispose();
      this.webRequests = new List<WWW>();
      this.Request(new byte[1]{ (byte) 1 }, this.UrlParameters, true);
    }

    private void EnqueueErrorDisconnect(StatusCode statusCode)
    {
      lock (this)
      {
        if (this.peerConnectionState != PeerBase.ConnectionStateValue.Connected && this.peerConnectionState != PeerBase.ConnectionStateValue.Connecting)
          return;
        this.peerConnectionState = PeerBase.ConnectionStateValue.Disconnecting;
      }
      this.EnqueueStatusCallback(statusCode);
      this.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.Disconnected()));
    }

    internal override void StopConnection() => throw new NotImplementedException();

    internal void Disconnected()
    {
      this.InitPeerBase();
      this.Listener.OnStatusChanged(StatusCode.Disconnect);
    }

    internal override void FetchServerTimestamp()
    {
    }

    internal override bool EnqueueOperation(
      Dictionary<byte, object> parameters,
      byte opCode,
      bool sendReliable,
      byte channelId,
      bool encrypted,
      PeerBase.EgMessageType messageType)
    {
      if (this.peerConnectionState != PeerBase.ConnectionStateValue.Connected)
      {
        if (this.debugOut >= DebugLevel.ERROR)
          this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send op: Not connected. PeerState: " + (object) this.peerConnectionState);
        this.Listener.OnStatusChanged(StatusCode.SendError);
        return false;
      }
      byte[] message = this.SerializeOperationToMessage(opCode, parameters, messageType, encrypted);
      if (message == null)
        return false;
      this.Request(message, this.UrlParameters);
      return true;
    }

    internal override bool DispatchIncomingCommands()
    {
      do
        ;
      while (this.CheckResult());
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
      this.DeserializeMessageAndCallback(incoming);
      return false;
    }

    internal override bool SendOutgoingCommands()
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connected && (long) this.GetLocalMsTimestamp() - this.lastPingTimeStamp > (long) this.timePingInterval)
      {
        this.lastPingTimeStamp = (long) this.GetLocalMsTimestamp();
        this.Request(new byte[1], this.UrlParameters);
      }
      return false;
    }

    internal override byte[] SerializeOperationToMessage(
      byte opCode,
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
          this.SerializeMemStream.Write(HttpBase2.messageHeader, 0, HttpBase2.messageHeader.Length);
        Protocol.SerializeOperationRequest(this.SerializeMemStream, opCode, parameters, false);
        if (encrypt)
        {
          byte[] buffer = this.CryptoProvider.Encrypt(this.SerializeMemStream.ToArray());
          this.SerializeMemStream.Position = 0L;
          this.SerializeMemStream.SetLength(0L);
          this.SerializeMemStream.Write(HttpBase2.messageHeader, 0, HttpBase2.messageHeader.Length);
          this.SerializeMemStream.Write(buffer, 0, buffer.Length);
        }
        array = this.SerializeMemStream.ToArray();
      }
      if (messageType != PeerBase.EgMessageType.Operation)
        array[HttpBase2.messageHeader.Length - 1] = (byte) messageType;
      if (encrypt)
        array[HttpBase2.messageHeader.Length - 1] = (byte) ((uint) array[HttpBase2.messageHeader.Length - 1] | 128U);
      return array;
    }

    internal override void receiveIncomingCommands(byte[] inBuff)
    {
      if (this.peerConnectionState == PeerBase.ConnectionStateValue.Connecting)
      {
        byte[] numArray = new byte[16];
        Buffer.BlockCopy((Array) inBuff, 0, (Array) numArray, 0, 16);
        this.HttpPeerID = new Guid(numArray).ToString();
        this.UrlParameters = "?pid=" + this.HttpPeerID;
        this.peerConnectionState = PeerBase.ConnectionStateValue.Connected;
        this.EnqueueActionForDispatch(new PeerBase.MyAction(((PeerBase) this).InitCallback));
      }
      else
      {
        this.timestampOfLastReceive = this.GetLocalMsTimestamp();
        this.bytesIn += (long) (inBuff.Length + 7);
        if (this.TrafficStatsEnabled)
        {
          ++this.TrafficStatsIncoming.TotalPacketCount;
          ++this.TrafficStatsIncoming.TotalCommandsInPackets;
        }
        if (this.peerConnectionState == PeerBase.ConnectionStateValue.Disconnecting)
          this.EnqueueDebugReturn(DebugLevel.ERROR, "Received while Disconnecting: " + SupportClass.ByteArrayToString(inBuff));
        if (inBuff.Length <= 0)
          return;
        if (inBuff[0] == (byte) 243 || inBuff[0] == (byte) 244)
        {
          this.EnqueueDebugReturn(DebugLevel.ERROR, "receiveIncomingCommands() found magic number instead of count. using complete reply as message.");
          this.incomingList.Add(inBuff);
        }
        else
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(inBuff)))
          {
            short num = binaryReader.ReadInt16();
            for (int index = 0; index < (int) num; ++index)
            {
              int count = binaryReader.ReadInt32();
              byte[] numArray = binaryReader.ReadBytes(count);
              if (numArray[0] == (byte) 243 || numArray[0] == (byte) 244)
              {
                lock (this.incomingList)
                {
                  this.incomingList.Add(numArray);
                  if (this.incomingList.Count % this.warningSize == 0)
                    this.EnqueueStatusCallback(StatusCode.QueueIncomingReliableWarning);
                }
              }
              else if (numArray[0] != (byte) 240 && this.debugOut >= DebugLevel.ERROR)
                this.EnqueueDebugReturn(DebugLevel.ERROR, "receiveIncomingCommands() MagicNumber should be 0xF0, 0xF3 or 0xF4. Is: " + (object) inBuff[0]);
            }
          }
        }
      }
    }

    private class AsyncRequestState
    {
      private Stopwatch Watch;

      public AsyncRequestState()
      {
        this.Watch = new Stopwatch();
        this.Watch.Start();
      }

      public HttpBase2 Base { get; set; }

      public WWW Request { get; set; }

      public byte[] OutgoingData { get; set; }

      public int ElapsedTime => (int) this.Watch.ElapsedMilliseconds;

      public bool Aborted { get; set; }
    }
  }
}
