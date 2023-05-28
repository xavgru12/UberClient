// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.CommPeerOperations
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
  public sealed class CommPeerOperations : IOperationSender
  {
    private byte __id;
    private RemoteProcedureCall sendOperation;

    public event RemoteProcedureCall SendOperation
    {
      add => this.sendOperation += value;
      remove => this.sendOperation -= value;
    }

    public CommPeerOperations(byte id = 0) => this.__id = id;

    public void SendAuthenticationRequest(string authToken, string magicHash, bool isMac)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, magicHash);
        BooleanProxy.Serialize((Stream) bytes, isMac);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 1, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSendHeartbeatResponse(string authToken, string responseHash)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, responseHash);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 2, customOpParameters) ? 1 : 0;
      }
    }
  }
}
