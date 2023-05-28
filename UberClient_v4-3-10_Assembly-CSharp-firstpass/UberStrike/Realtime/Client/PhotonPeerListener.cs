// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.PhotonPeerListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using ExitGames.Client.Photon;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace UberStrike.Realtime.Client
{
  public sealed class PhotonPeerListener : IPhotonPeerListener
  {
    private Action<byte, byte[]> eventDispatcher;
    private Action onConnect;
    private Action<StatusCode> onDisconnect;
    private Action<string> onError;

    public event Action<byte, byte[]> EventDispatcher
    {
      add => this.eventDispatcher += value;
      remove => this.eventDispatcher -= value;
    }

    public event Action OnConnect
    {
      add => this.onConnect += value;
      remove => this.onConnect -= value;
    }

    public event Action<StatusCode> OnDisconnect
    {
      add => this.onDisconnect += value;
      remove => this.onDisconnect -= value;
    }

    public event Action<string> OnError
    {
      add => this.onError += value;
      remove => this.onError -= value;
    }

    internal void ClearEvents()
    {
      this.eventDispatcher = (Action<byte, byte[]>) null;
      this.onConnect = (Action) null;
      this.onDisconnect = (Action<StatusCode>) null;
      this.onError = (Action<string>) null;
    }

    public void OnEvent(EventData eventData)
    {
      try
      {
        if (this.eventDispatcher == null)
          return;
        this.eventDispatcher(eventData.Code, (byte[]) eventData.Parameters[(byte) 0]);
      }
      catch (SerializationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("OnEvent failed: " + eventData.ToStringFull() + "\n" + ex.ToString()));
        throw ex;
      }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
      if (operationResponse.ReturnCode > (short) 0)
      {
        Debug.LogError((object) ("OnOperationResponse: " + operationResponse.DebugMessage));
        if (this.onError == null)
          return;
        this.onError(operationResponse.DebugMessage);
      }
      else
        Debug.Log((object) ("OnOperationResponse: " + operationResponse.OperationCode.ToString()));
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
      Debug.Log((object) ("PeerStatusCallback " + statusCode.ToString()));
      switch (statusCode)
      {
        case StatusCode.ExceptionOnConnect:
        case StatusCode.Disconnect:
        case StatusCode.Exception:
        case StatusCode.TimeoutDisconnect:
        case StatusCode.DisconnectByServer:
        case StatusCode.DisconnectByServerUserLimit:
        case StatusCode.DisconnectByServerLogic:
          if (this.onDisconnect == null)
            break;
          this.onDisconnect(statusCode);
          break;
        case StatusCode.Connect:
          if (this.onConnect == null)
            break;
          this.onConnect();
          break;
        case StatusCode.SendError:
          Debug.LogWarning((object) "Operation sent without connection to server");
          break;
        default:
          Debug.LogWarning((object) ("Unhandled OnStatusChanged " + statusCode.ToString()));
          break;
      }
    }

    public void DebugReturn(DebugLevel level, string message) => Debug.Log((object) ("DebugReturn " + message));
  }
}
