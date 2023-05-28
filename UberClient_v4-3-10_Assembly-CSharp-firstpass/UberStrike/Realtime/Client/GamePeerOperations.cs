// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.GamePeerOperations
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
  public sealed class GamePeerOperations : IOperationSender
  {
    private byte __id;
    private RemoteProcedureCall sendOperation;

    public event RemoteProcedureCall SendOperation
    {
      add => this.sendOperation += value;
      remove => this.sendOperation -= value;
    }

    public GamePeerOperations(byte id = 0) => this.__id = id;

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
        int num = this.sendOperation((byte) 1, customOpParameters) ? 1 : 0;
      }
    }

    public void SendGetServerLoad()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 2, customOpParameters) ? 1 : 0;
      }
    }

    public void SendGetGameInformation(int number)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, number);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 3, customOpParameters) ? 1 : 0;
      }
    }

    public void SendGetGameListUpdates()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 4, customOpParameters) ? 1 : 0;
      }
    }

    public void SendEnterRoom(
      int roomId,
      string password,
      string clientVersion,
      string authToken,
      string magicHash,
      bool isMac)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, roomId);
        StringProxy.Serialize((Stream) bytes, password);
        StringProxy.Serialize((Stream) bytes, clientVersion);
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
        int num = this.sendOperation((byte) 5, customOpParameters) ? 1 : 0;
      }
    }

    public void SendCreateRoom(
      GameRoomData metaData,
      string password,
      string clientVersion,
      string authToken,
      string magicHash,
      bool isMac)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        GameRoomDataProxy.Serialize((Stream) bytes, metaData);
        StringProxy.Serialize((Stream) bytes, password);
        StringProxy.Serialize((Stream) bytes, clientVersion);
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
        int num = this.sendOperation((byte) 6, customOpParameters) ? 1 : 0;
      }
    }

    public void SendLeaveRoom()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 7, customOpParameters) ? 1 : 0;
      }
    }

    public void SendCloseRoom(int roomId, string authToken, string magicHash)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, roomId);
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, magicHash);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 8, customOpParameters) ? 1 : 0;
      }
    }

    public void SendInspectRoom(int roomId, string authToken)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, roomId);
        StringProxy.Serialize((Stream) bytes, authToken);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 9, customOpParameters) ? 1 : 0;
      }
    }

    public void SendReportPlayer(int cmid, string authToken)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, authToken);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 10, customOpParameters) ? 1 : 0;
      }
    }

    public void SendKickPlayer(int cmid, string authToken, string magicHash)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, magicHash);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 11, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateLoadout()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 12, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdatePing(ushort ping)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        UInt16Proxy.Serialize((Stream) bytes, ping);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 13, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateKeyState(byte state)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ByteProxy.Serialize((Stream) bytes, state);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 14, customOpParameters) ? 1 : 0;
      }
    }

    public void SendRefreshBackendData(string authToken, string magicHash)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, magicHash);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 15, customOpParameters) ? 1 : 0;
      }
    }
  }
}
