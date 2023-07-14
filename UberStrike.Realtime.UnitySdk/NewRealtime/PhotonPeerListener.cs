// Decompiled with JetBrains decompiler
// Type: NewRealtime.PhotonPeerListener
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using ExitGames.Client.Photon;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace NewRealtime
{
  public sealed class PhotonPeerListener : IPhotonPeerListener
  {
    private bool isConnected = false;

    public event Action<byte, byte[]> EventDispatcher;

    public void OnEvent(EventData eventData)
    {
      try
      {
        if (this.EventDispatcher == null)
          return;
        this.EventDispatcher(eventData.Code, (byte[]) eventData.Parameters[(byte) 0]);
      }
      catch (SerializationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("PhotonEvent: " + eventData.ToStringFull()));
        Debug.LogWarning((object) ("Source: " + ex.Source));
        Debug.LogWarning((object) ("Stack: " + ex.StackTrace));
        throw new Exception(ex.GetType().ToString() + " thrown when executing EventAction with Id " + (object) eventData.Code);
      }
    }

    public void OnOperationResponse(OperationResponse operationResponse) => Debug.Log((object) ("OperationResult " + (object) operationResponse.OperationCode));

    public void OnStatusChanged(StatusCode statusCode)
    {
      Debug.Log((object) ("PeerStatusCallback " + (object) statusCode));
      switch (statusCode)
      {
        case StatusCode.Connect:
          if (!this.isConnected && this.OnConnect != null)
            this.OnConnect();
          this.isConnected = true;
          break;
        case StatusCode.Disconnect:
        case StatusCode.TimeoutDisconnect:
        case StatusCode.DisconnectByServer:
        case StatusCode.DisconnectByServerUserLimit:
        case StatusCode.DisconnectByServerLogic:
          if (this.isConnected && this.OnDisconnect != null)
            this.OnDisconnect();
          this.isConnected = false;
          break;
      }
    }

    public void DebugReturn(DebugLevel level, string message) => Debug.Log((object) ("DebugReturn " + message));

    public event Action OnConnect;

    public event Action OnDisconnect;
  }
}
