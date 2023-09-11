// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.PhotonPeerListener
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using Cmune.Realtime.Common.Security;
using Cmune.Realtime.Common.Utils;
using Cmune.Realtime.Photon.Client.Network.Utils;
using Cmune.Util;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cmune.Realtime.Photon.Client
{
  public class PhotonPeerListener : IPhotonPeerListener
  {
    private bool _doDisconnect;
    private string _server;
    private PhotonPeerListener.ConnectionEventType _lastEventEnqueued;
    private Queue<PhotonPeerListener.ConnectionEvent> _connectionEvents = new Queue<PhotonPeerListener.ConnectionEvent>();
    private Action<PhotonPeerListener.ConnectionEvent> _castConnectionEvents;
    private PhotonPeerListener.RecieveMessages _recieveMessages;
    private PhotonPeer _peer;
    private SecureMemory<int> _actorId;
    private NetworkState _state = NetworkState.STATE_DISCONNECTED;
    private int _sessionID = -1;
    private float _sendPerSecond = 40f;
    private float _getPerSecond = 40f;
    private bool _isInRoom;
    private float _nextPingUpdate;
    private Dictionary<short, PhotonPeerListener.OperationCallback> _operationsWaitingForResponse = new Dictionary<short, PhotonPeerListener.OperationCallback>();
    private short _invocationId = 1;
    private bool _isShutdown;

    public string Server => this._server;

    public int Latency => this._peer == null ? 0 : Mathf.RoundToInt((float) this._peer.RoundTripTime * 0.5f);

    public bool IsConnecting => this.ConnectionState == NetworkState.STATE_CONNECTING;

    public bool IsConnectedToServer => this.PeerState == PeerStateValue.Connected;

    public bool IsDisconnecting => this.ConnectionState == NetworkState.STATE_DISCONNECTING;

    public bool IsJoining => this.ConnectionState == NetworkState.STATE_JOINING;

    public bool IsLeaving => this.ConnectionState == NetworkState.STATE_LEAVING;

    public bool HasJoinedRoom
    {
      get => this._isInRoom;
      private set => this._isInRoom = value;
    }

    public NetworkState ConnectionState
    {
      get => this._state;
      private set => this._state = value;
    }

    public PeerStateValue PeerState => this._peer == null ? PeerStateValue.Disconnected : this._peer.PeerState;

    public string PeerInfo
    {
      get
      {
        if (this._peer == null)
          return "<Peer Null>";
        return this._peer.RoundTripTime.ToString() + "/" + (object) this._peer.RoundTripTimeVariance + ", <" + (object) this._peer.BytesIn + ", >" + (object) this._peer.BytesOut;
      }
    }

    public string PingInfo => this._peer == null ? string.Empty : string.Format("{0} ms/{1} ms", (object) this._peer.RoundTripTime, (object) this._peer.RoundTripTimeVariance);

    public string DataIOInfo => this._peer == null ? string.Empty : string.Format("{0} kb/{1} kb", (object) Mathf.RoundToInt(ConvertBytes.ToKiloBytes(this._peer.BytesIn)), (object) Mathf.RoundToInt(ConvertBytes.ToKiloBytes(this._peer.BytesOut)));

    public float TargetSendPerSecond
    {
      get => this._sendPerSecond;
      set => this._sendPerSecond = value;
    }

    public float TargetGetPerSecond
    {
      get => this._getPerSecond;
      set => this._getPerSecond = value;
    }

    public int SessionID
    {
      get => this._sessionID;
      private set => this._sessionID = value;
    }

    public bool DoDisconnect
    {
      get => this._doDisconnect;
      set => this._doDisconnect = value;
    }

    public CmuneRoomID CurrentRoom { get; private set; }

    public int ActorId => this._actorId.ReadData(false);

    public int ActorIdSecure => this._actorId.ReadData(true);

    public ushort Ping { get; private set; }

    public long IncomingBytes => this._peer.BytesIn;

    public long OutgoingBytes => this._peer.BytesOut;

    public PhotonPeerListener(bool monitorMemory = false)
    {
      this._actorId = new SecureMemory<int>(0, monitorMemory);
      this._peer = new PhotonPeer((IPhotonPeerListener) this, false);
      this._peer.DebugOut = DebugLevel.ERROR;
      this._peer.TimePingInterval = 1000;
      this._peer.SentCountAllowance = 5;
      this._peer.CommandBufferSize = 500;
      this.ConnectionState = NetworkState.STATE_DISCONNECTED;
    }

    public int ServerTimeTicks => this._peer.ServerTimeInMilliSeconds & int.MaxValue;

    public void FetchServerTime() => this._peer.FetchServerTimestamp();

    public void EnableNetworkSimulation(bool enabled, int incomingLag, int outgoingLag)
    {
      Debug.LogError((object) "PhotonPeerListener: Setting up Network Simulation");
      this._peer.NetworkSimulationSettings.IsSimulationEnabled = enabled;
      this._peer.NetworkSimulationSettings.IncomingJitter = 0;
      this._peer.NetworkSimulationSettings.IncomingLag = incomingLag;
      this._peer.NetworkSimulationSettings.IncomingLossPercentage = 0;
      this._peer.NetworkSimulationSettings.OutgoingJitter = 0;
      this._peer.NetworkSimulationSettings.OutgoingLag = outgoingLag;
      this._peer.NetworkSimulationSettings.OutgoingLossPercentage = 0;
    }

    public void SubscribeToEvents(Action<PhotonPeerListener.ConnectionEvent> process) => this._castConnectionEvents += process;

    public void UnsubscribeToEvents(Action<PhotonPeerListener.ConnectionEvent> process) => this._castConnectionEvents -= process;

    public void SetMessageCallback(PhotonPeerListener.RecieveMessages callback) => this._recieveMessages = callback;

    public void UpdateServerTime()
    {
      if (this._peer == null)
        return;
      this._peer.FetchServerTimestamp();
    }

    public void Update()
    {
      if (this.PeerState > PeerStateValue.Disconnected)
      {
        do
          ;
        while (this._peer.DispatchIncomingCommands());
        this.SendOutgoingCommands();
        if ((double) Time.time > (double) this._nextPingUpdate)
        {
          this._nextPingUpdate = Time.time + 5f;
          this.Ping = (ushort) ((double) this._peer.RoundTripTime * 0.5);
        }
      }
      while (this._connectionEvents.Count > 0)
        this._castConnectionEvents(this._connectionEvents.Dequeue());
    }

    private void SendOutgoingCommands()
    {
      if (this.DoDisconnect)
      {
        this.DoDisconnect = false;
        this._peer.Disconnect();
        this.ConnectionState = NetworkState.STATE_DISCONNECTING;
      }
      else if (this.ConnectionState == NetworkState.STATE_DISCONNECTED)
        this.OnDisconnect();
      this._peer.SendOutgoingCommands();
    }

    public void LeaveCurrentRoom()
    {
      if (this.CurrentRoom.IsEmpty)
        return;
      this.SendOperationToServer(new OperationRequest()
      {
        Parameters = OperationFactory.Create((byte) 89, (object) this.CurrentRoom.GetBytes()),
        OperationCode = (byte) 89
      }, true);
      this.ConnectionState = NetworkState.STATE_LEAVING;
    }

    private void OnRoomLeft() => this.OnRoomLeft(false);

    private void OnRoomLeft(bool reject)
    {
      this.CurrentRoom = CmuneRoomID.Empty;
      this._actorId.WriteData(0);
      if (this.ConnectionState != NetworkState.STATE_DISCONNECTED)
        this.ConnectionState = NetworkState.STATE_LEFT;
      this.HasJoinedRoom = false;
    }

    public void Disconnect(bool force = false)
    {
      if (!this.IsConnectedToServer && !force)
        return;
      this.DoDisconnect = true;
    }

    private void OnDisconnect()
    {
      if (!this.HasJoinedRoom)
        return;
      this.OnRoomLeft();
    }

    public bool Connect(string server, int cmid)
    {
      this._server = server;
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("({0}) - Connect TO {1}", (object) this.SessionID, (object) server);
      this.ConnectionState = NetworkState.STATE_CONNECTING;
      try
      {
        if (!this._peer.Connect(server, cmid.ToString()))
          this.ConnectionState = NetworkState.STATE_DISCONNECTED;
        return this.ConnectionState == NetworkState.STATE_CONNECTING;
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("({0}) - Connect failed with: {1}", (object) this.SessionID, (object) ex.Message);
        this.ConnectionState = NetworkState.STATE_DISCONNECTED;
        this._connectionEvents.Clear();
        this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected, this.CurrentRoom, 0));
        return false;
      }
    }

    public void JoinRoom(RoomMetaData room, int cmid, int accessLevel)
    {
      if (this.IsConnectedToServer)
      {
        OperationRequest request = new OperationRequest()
        {
          Parameters = OperationFactory.Create((byte) 88, (object) RealtimeSerialization.ToBytes((object) room).ToArray(), (object) cmid, (object) accessLevel),
          OperationCode = 88
        };
        int num = 0;
        this.SendOperationToServer(request, true);
        if (CmuneNetworkState.DebugMessaging)
          CmuneDebug.Log("({0}) - JoinRoom {1} on Server with InvocID: {2}", (object) this.SessionID, (object) room.RoomID, (object) num);
        this.ConnectionState = NetworkState.STATE_JOINING;
      }
      else
        CmuneDebug.LogError("({0}) - JoinRoom {1} by {2} failed because not connected yet", (object) this.SessionID, (object) room.RoomID, (object) this.SessionID);
    }

    private void ReadBinaryLobbyListUpdate(IDictionary data)
    {
      byte[] generalArg = OperationUtil.GetGeneralArg<byte[]>(data, (byte) 122);
      try
      {
        if (!(RealtimeSerialization.ToObject(generalArg) is List<RoomMetaData> roomMetaDataList))
          return;
        foreach (RoomMetaData data1 in roomMetaDataList)
          CmuneNetworkState.AddRoom(data1);
      }
      catch
      {
        CmuneDebug.LogError("({0}) - LobbyList Update failed: Byte[] null = {1}", (object) this.SessionID, (object) (generalArg == null));
      }
    }

    private void ReadBinaryLobbyListRemoval(Hashtable data)
    {
      byte[] generalArg = OperationUtil.GetGeneralArg<byte[]>((IDictionary) data, (byte) 123);
      try
      {
        if (!(RealtimeSerialization.ToObject(generalArg) is List<CmuneRoomID> cmuneRoomIdList))
          return;
        foreach (CmuneRoomID id in cmuneRoomIdList)
          CmuneNetworkState.RemoveRoom(id);
      }
      catch
      {
        CmuneDebug.LogError("({0}) - LobbyList Removal failed: Byte[] null = {1}", (object) this.SessionID, (object) (generalArg == null));
      }
    }

    private void OnIncomingMessage(IDictionary<byte, object> data)
    {
      short networkID = OperationUtil.GetArg<short>(data, (byte) 101);
      byte functionID = OperationUtil.GetArg<byte>(data, (byte) 100);
      byte[] bytes = OperationUtil.GetArg<byte[]>(data, (byte) 103);
      if (NetworkStatistics.IsEnabled)
        NetworkStatistics.RecordIncomingCall(networkID.ToString() + "/" + (object) functionID, bytes.Length);
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("({0}) - OnIncomingMessage {1}:{2}", (object) this.SessionID, (object) networkID, (object) functionID);
      try
      {
        object[] objects = RealtimeSerialization.ToObjects(bytes);
        if (this._recieveMessages == null)
          return;
        this._recieveMessages(networkID, functionID, objects);
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("({0}) - OnIncomingMessage {1}:{2} crashed with: {3}\n{4}", (object) this.SessionID, (object) networkID, (object) functionID, (object) ex.Message, (object) ex.StackTrace);
      }
    }

    internal short SendOperationToServerApplication(
      Action<int, object[]> action,
      byte appMethodID,
      params object[] args)
    {
      if (this._peer != null)
      {
        OperationRequest operationRequest = new OperationRequest()
        {
          Parameters = OperationFactory.Create((byte) 66, (object) appMethodID, (object) ++this._invocationId, (object) RealtimeSerialization.ToBytes(args).ToArray()),
          OperationCode = 66
        };
        this._peer.OpCustom(operationRequest.OperationCode, operationRequest.Parameters, true);
        if (action != null)
          this._operationsWaitingForResponse[this._invocationId] = new PhotonPeerListener.OperationCallback(action);
        if (CmuneNetworkState.DebugMessaging)
          CmuneDebug.Log("({0}) - SendOperation: ApplicationMethodId {1} with invocID {2}, has callback: {3}", (object) this.SessionID, (object) appMethodID, (object) this._invocationId, (object) (action != null));
        return this._invocationId;
      }
      CmuneDebug.LogError("({0}) - SendOperationToServerApplication failed because peer NULL", (object) this.SessionID);
      return -1;
    }

    internal bool SendOperationToServer(OperationRequest request, bool isReliable)
    {
      if (this._peer != null)
      {
        if (CmuneNetworkState.DebugMessaging)
          CmuneDebug.Log("({0}) - SendMessage of Type {1}", (object) this.SessionID, (object) request.OperationCode);
        if (NetworkStatistics.IsEnabled)
          NetworkStatistics.RecordOutgoingCall(request);
        return this._peer.OpCustom(request.OperationCode, request.Parameters, isReliable);
      }
      CmuneDebug.LogError("({0}) - SendOperationToServer failed because peer NULL", (object) this.SessionID);
      return false;
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("PeerStatusCallback " + (object) statusCode, new object[0]);
      switch (statusCode)
      {
        case StatusCode.SecurityExceptionOnConnect:
          CmuneDebug.LogError("({0}) SecurityExceptionOnConnect - check if PolicyServer is running", (object) this.SessionID);
          break;
        case StatusCode.ExceptionOnConnect:
        case StatusCode.Exception:
        case StatusCode.InternalReceiveException:
          if (CmuneNetworkState.DebugMessaging)
            CmuneDebug.LogError("({0}) - RETURN Exception: {1}", (object) this.SessionID, (object) statusCode);
          this.ConnectionState = NetworkState.STATE_DISCONNECTED;
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected);
          break;
        case StatusCode.Connect:
          this.ConnectionState = NetworkState.STATE_CONNECTED;
          this.SessionID = CmuneNetworkState.GetNextSessionID();
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.Connected);
          int serverApplication = (int) this.SendOperationToServerApplication((Action<int, object[]>) null, (byte) 1, (object) (byte) 1, (object) Cmune.Realtime.Common.Protocol.Version);
          if (!CmuneNetworkState.DebugMessaging)
            break;
          CmuneDebug.Log("({0}) - SendOperation: PeerSpecification {1} with invocID {2}", (object) this.SessionID, (object) PeerType.GamePeer, (object) serverApplication);
          break;
        case StatusCode.Disconnect:
          this.ConnectionState = NetworkState.STATE_DISCONNECTED;
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected);
          break;
        case StatusCode.QueueOutgoingReliableWarning:
        case StatusCode.QueueOutgoingUnreliableWarning:
        case StatusCode.QueueOutgoingAcksWarning:
        case StatusCode.QueueSentWarning:
          CmuneDebug.LogWarning("({0}) - RETURN <OUT-QUEUE> FILLING UP {1} ", (object) this.SessionID, (object) statusCode);
          break;
        case StatusCode.QueueIncomingReliableWarning:
        case StatusCode.QueueIncomingUnreliableWarning:
          CmuneDebug.LogWarning("({0}) - RETURN <IN-QUEUE> FILLING UP {1} {2}/{3}", (object) this.SessionID, (object) statusCode, (object) this._peer.QueuedIncomingCommands, (object) this._peer.QueuedOutgoingCommands);
          break;
        case StatusCode.TimeoutDisconnect:
          if (CmuneNetworkState.DebugMessaging)
            CmuneDebug.LogError("({0}) - RETURN TimeoutDisconnect", (object) this.SessionID);
          this.ConnectionState = NetworkState.STATE_DISCONNECTED;
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected);
          break;
        case StatusCode.DisconnectByServer:
        case StatusCode.DisconnectByServerUserLimit:
        case StatusCode.DisconnectByServerLogic:
          if (CmuneNetworkState.DebugMessaging)
            CmuneDebug.LogError("({0}) - RETURN DisconnectByServer", (object) this.SessionID);
          this.ConnectionState = NetworkState.STATE_DISCONNECTED;
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.Disconnected);
          break;
        default:
          CmuneDebug.LogError("({0}) - UNHANDLED PeerStatusCallback with returnCode: {1}", (object) this.SessionID, (object) statusCode);
          break;
      }
    }

    private void EnqueueConnectionEvent(PhotonPeerListener.ConnectionEvent ev)
    {
      this._connectionEvents.Enqueue(ev);
      this._lastEventEnqueued = ev.Type;
    }

    private void EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType ev)
    {
      if (this._connectionEvents.Count == 0 || this._lastEventEnqueued != ev)
        this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(ev, this.CurrentRoom, this.ActorIdSecure));
      this._lastEventEnqueued = ev;
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("({0}) - OperationResult with returnCode: {1},  opCode: {2}", (object) this.SessionID, (object) operationResponse.ReturnCode, (object) operationResponse.OperationCode);
      switch (operationResponse.OperationCode)
      {
        case 66:
          if (operationResponse.ReturnCode == (short) 0 && OperationUtil.HasArg((IDictionary<byte, object>) operationResponse.Parameters, (byte) 61))
          {
            short key = OperationUtil.GetArg<short>((IDictionary<byte, object>) operationResponse.Parameters, (byte) 61);
            PhotonPeerListener.OperationCallback operationCallback;
            if (this._operationsWaitingForResponse.TryGetValue(key, out operationCallback))
            {
              this._operationsWaitingForResponse.Remove(key);
              if (OperationUtil.HasArg((IDictionary<byte, object>) operationResponse.Parameters, (byte) 42))
              {
                try
                {
                  if (operationCallback == null || operationCallback.Callback == null)
                    break;
                  operationCallback.Callback((int) operationResponse.ReturnCode, RealtimeSerialization.ToObjects(OperationUtil.GetArg<byte[]>((IDictionary<byte, object>) operationResponse.Parameters, (byte) 42)));
                  break;
                }
                catch
                {
                  CmuneDebug.LogError("({0}) - Error executing Action after recieving response from MessageToApplication", (object) this.SessionID);
                  break;
                }
              }
              else
              {
                CmuneDebug.LogWarning("{0} MessageToApplication return executed no action for invocId {1} because no data attached", (object) this.SessionID, (object) key);
                break;
              }
            }
            else
            {
              CmuneDebug.LogWarning("{0} MessageToApplication return executed no action for invocId {1}", (object) this.SessionID, (object) key);
              break;
            }
          }
          else
          {
            CmuneDebug.LogError("{0} MessageToApplication {1} failed with message: {2}", (object) this.SessionID, (object) operationResponse.ReturnCode, (object) operationResponse.DebugMessage);
            break;
          }
        case 80:
        case 81:
        case 82:
        case 83:
          CmuneDebug.LogError("({0}) - Operation '{1}' not allowed on Server and returned with code {2}", (object) this.SessionID, (object) operationResponse.OperationCode, (object) operationResponse.ReturnCode);
          break;
        case 88:
          this.ConnectionState = NetworkState.STATE_RECEIVING;
          if (operationResponse.ReturnCode == (short) 0)
          {
            int actor = OperationUtil.GetActor((IDictionary<byte, object>) operationResponse.Parameters);
            byte[] bytes = OperationUtil.GetArg<byte[]>((IDictionary<byte, object>) operationResponse.Parameters, (byte) 4);
            bool flag = OperationUtil.GetArg<bool>((IDictionary<byte, object>) operationResponse.Parameters, (byte) 200);
            OperationUtil.GetArg<long>((IDictionary<byte, object>) operationResponse.Parameters, (byte) 201);
            if (actor > 0)
            {
              this._actorId.WriteData(actor);
              this.CurrentRoom = new CmuneRoomID(bytes);
              this.HasJoinedRoom = true;
              this._connectionEvents.Clear();
              if (flag)
                this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.BuildLevel, this.CurrentRoom, this.ActorIdSecure));
              else
                this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.ClearLevel, this.CurrentRoom, this.ActorIdSecure));
              this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.JoinedRoom, this.CurrentRoom, this.ActorIdSecure));
              break;
            }
            CmuneDebug.LogWarning("PhotonGameJoin failed with actorId " + (object) actor, new object[0]);
            this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.JoinFailed, this.CurrentRoom, this.ActorIdSecure, (int) operationResponse.ReturnCode));
            this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.LeftRoom, this.CurrentRoom, this.ActorIdSecure));
            this.DoDisconnect = true;
            break;
          }
          CmuneDebug.LogError("({0}) - PhotonGameJoin failed with code {1} and message {2}", (object) this.SessionID, (object) operationResponse.ReturnCode, (object) operationResponse.DebugMessage);
          this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.JoinFailed, this.CurrentRoom, this.ActorIdSecure, (int) operationResponse.ReturnCode));
          this._connectionEvents.Enqueue(new PhotonPeerListener.ConnectionEvent(PhotonPeerListener.ConnectionEventType.LeftRoom, this.CurrentRoom, this.ActorIdSecure));
          this.DoDisconnect = true;
          break;
        case 89:
          this.EnqueueConnectionEvent(PhotonPeerListener.ConnectionEventType.LeftRoom);
          this.OnRoomLeft();
          break;
        default:
          CmuneDebug.LogError("({0}) - UNHANDLED OperationResult with returnCode: {1},  opCode: {2}", (object) this.SessionID, (object) operationResponse.ReturnCode, (object) operationResponse.OperationCode);
          break;
      }
    }

    public void OnEvent(EventData eventData)
    {
      try
      {
        switch (eventData.Code)
        {
          case 0:
            this.OnIncomingMessage((IDictionary<byte, object>) eventData.Parameters);
            break;
          case 3:
            CmuneNetworkState.ClearRooms();
            this.ReadBinaryLobbyListUpdate((IDictionary) OperationUtil.GetArg<Hashtable>((IDictionary<byte, object>) eventData.Parameters, (byte) 42));
            CmuneEventHandler.Route((object) new RoomListUpdatedEvent(CmuneNetworkState.AllRooms, true));
            break;
          case 4:
            this.ReadBinaryLobbyListUpdate((IDictionary) OperationUtil.GetArg<Hashtable>((IDictionary<byte, object>) eventData.Parameters, (byte) 42));
            CmuneEventHandler.Route((object) new RoomListUpdatedEvent(CmuneNetworkState.AllRooms));
            break;
          case 5:
            this.ReadBinaryLobbyListRemoval(OperationUtil.GetArg<Hashtable>((IDictionary<byte, object>) eventData.Parameters, (byte) 42));
            CmuneEventHandler.Route((object) new RoomListUpdatedEvent(CmuneNetworkState.AllRooms));
            break;
          default:
            CmuneDebug.LogError("({0}) - UNHANDLED EventAction with code: {1} and argCount: {2}", (object) this.SessionID, (object) eventData.Code, (object) eventData.Parameters.Count);
            break;
        }
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("({0}) - CRASH IN EventAction with eventCode: {1}", (object) this.SessionID, (object) eventData.Code);
        CmuneDebug.LogError("{0}{1}", (object) ex.Message, (object) ex.StackTrace);
        CmuneDebug.LogError(OperationUtil.PrintHashtable((IDictionary) eventData.Parameters), new object[0]);
      }
    }

    public void DebugReturn(DebugLevel level, string debug)
    {
      if (!CmuneNetworkState.DebugMessaging)
        return;
      CmuneDebug.LogError("({0}) - DebugReturn: {1}", (object) this.SessionID, (object) debug);
    }

    public void ShutDown()
    {
      if (this._isShutdown)
        return;
      this._isShutdown = true;
      this._recieveMessages = (PhotonPeerListener.RecieveMessages) null;
      this._castConnectionEvents = (Action<PhotonPeerListener.ConnectionEvent>) null;
      if (this._peer.PeerState <= PeerStateValue.Disconnected)
        return;
      this._peer.Disconnect();
      this._peer.StopThread();
    }

    public delegate void RecieveMessages(short networkID, byte functionID, object[] args);

    public enum ConnectionEventType
    {
      None,
      Connected,
      Disconnected,
      JoinedRoom,
      LeftRoom,
      OtherJoined,
      OtherLeft,
      JoinFailed,
      BuildLevel,
      ClearLevel,
    }

    public class ConnectionEvent
    {
      public ConnectionEvent(PhotonPeerListener.ConnectionEventType t, CmuneRoomID id, int actorID)
        : this(t, id, actorID, 0)
      {
      }

      public ConnectionEvent(
        PhotonPeerListener.ConnectionEventType t,
        CmuneRoomID id,
        int actorID,
        int errorCode)
      {
        this.Type = t;
        this.Room = id;
        this.ActorID = actorID;
        this.ErrorCode = errorCode;
      }

      public PhotonPeerListener.ConnectionEventType Type { get; private set; }

      public CmuneRoomID Room { get; private set; }

      public int ActorID { get; private set; }

      public int ErrorCode { get; private set; }
    }

    private class OperationCallback
    {
      public readonly Action<int, object[]> Callback;

      public OperationCallback(Action<int, object[]> action) => this.Callback = action;
    }
  }
}
