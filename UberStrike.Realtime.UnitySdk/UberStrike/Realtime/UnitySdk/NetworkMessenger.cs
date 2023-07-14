// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.NetworkMessenger
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class NetworkMessenger
  {
    public Dictionary<short, NetworkMessenger.NetworkClassInfo> CallStatistics = new Dictionary<short, NetworkMessenger.NetworkClassInfo>();
    private PhotonPeerListener _peerListener;

    public NetworkMessenger(PhotonPeerListener peerListener) => this._peerListener = peerListener;

    public bool SendMessageToAll(short networkID, byte localAddress, params object[] args) => this.SendMessageToAll(networkID, true, localAddress, args);

    public bool SendMessageToAll(
      short networkID,
      bool isReliable,
      byte address,
      params object[] args)
    {
      if (networkID <= (short) 0)
        return false;
      if (this.IsConnectionReady)
      {
        byte code = 83;
        this._peerListener.SendOperationToServer(new OperationRequest()
        {
          Parameters = OperationFactory.Create(code, (object) networkID, (object) address, (object) RealtimeSerialization.ToBytes(args).ToArray()),
          OperationCode = code
        }, isReliable);
        return true;
      }
      CmuneDebug.LogWarning("({0}) - SendMessage '{1}:{2}' to Others failed because connection not ready yet!", (object) this.PeerListener.SessionID, (object) networkID, (object) address);
      return false;
    }

    public bool SendMessageToPlayer(
      int playerID,
      short networkID,
      byte address,
      params object[] args)
    {
      if (networkID <= (short) 0)
        return false;
      if (this.IsConnectionReady)
      {
        byte code = 80;
        this._peerListener.SendOperationToServer(new OperationRequest()
        {
          Parameters = OperationFactory.Create(code, (object) playerID, (object) networkID, (object) address, (object) RealtimeSerialization.ToBytes(args).ToArray()),
          OperationCode = code
        }, true);
        return true;
      }
      CmuneDebug.LogWarning("({0}) - SendMessage '{1}:{2}' to Player {3} failed because connection not ready yet!", (object) this.PeerListener.SessionID, (object) networkID, (object) address);
      return false;
    }

    public bool SendMessageToServer(short networkID, byte address, params object[] args) => this.SendMessageToServer(networkID, true, address, args);

    public bool SendMessageToServer(
      short networkID,
      bool isReliable,
      byte address,
      params object[] args)
    {
      if (networkID <= (short) 0)
        return false;
      if (this.IsConnectionReady)
      {
        byte code = 82;
        this._peerListener.SendOperationToServer(new OperationRequest()
        {
          Parameters = OperationFactory.Create(code, (object) networkID, (object) address, (object) RealtimeSerialization.ToBytes(args).ToArray()),
          OperationCode = code
        }, isReliable);
        return true;
      }
      CmuneDebug.LogWarning("({0}) - SendMessage '{1}:{2}' to Server failed because connection not ready yet!", (object) this.PeerListener.SessionID, (object) networkID, (object) address);
      return false;
    }

    public bool SendOperationToServerApplication(
      Action<int, object[]> action,
      byte address,
      params object[] args)
    {
      if (this._peerListener.IsConnectedToServer)
      {
        int serverApplication = (int) this._peerListener.SendOperationToServerApplication(action, address, args);
        return true;
      }
      CmuneDebug.LogWarning("({0}) - MessageToApplication to Server failed because connection not ready yet!", (object) this.PeerListener.SessionID);
      return false;
    }

    public bool IsConnectionReady => this._peerListener.IsConnectedToServer;

    public PhotonPeerListener PeerListener => this._peerListener;

    public class NetworkClassInfo
    {
      public Dictionary<byte, int> _functionCalls = new Dictionary<byte, int>();
      public Dictionary<byte, double> _functionTime = new Dictionary<byte, double>();

      public void AddFunctionCall(byte id, double time)
      {
        if (this._functionCalls.ContainsKey(id))
        {
          Dictionary<byte, int> functionCalls;
          byte key1;
          (functionCalls = this._functionCalls)[key1 = id] = functionCalls[key1] + 1;
          Dictionary<byte, double> functionTime;
          byte key2;
          (functionTime = this._functionTime)[key2 = id] = functionTime[key2] + time;
        }
        else
        {
          this._functionCalls[id] = 1;
          this._functionTime[id] = time;
        }
      }

      public string GetAvarageExecutionTime(byte address) => this._functionTime.ContainsKey(address) ? (this._functionTime[address] / (double) this._functionCalls[address]).ToString("f1") : string.Empty;

      public string GetTotalExecutionTime(byte address) => this._functionTime.ContainsKey(address) ? this._functionTime[address].ToString("f1") : string.Empty;
    }
  }
}
