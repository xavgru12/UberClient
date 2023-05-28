// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.LobbyRoomOperations
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
  public sealed class LobbyRoomOperations : IOperationSender
  {
    private byte __id;
    private RemoteProcedureCall sendOperation;

    public event RemoteProcedureCall SendOperation
    {
      add => this.sendOperation += value;
      remove => this.sendOperation -= value;
    }

    public LobbyRoomOperations(byte id = 0) => this.__id = id;

    public void SendFullPlayerListUpdate()
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
        int num = this.sendOperation((byte) 1, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdatePlayerRoom(GameRoom room)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        GameRoomProxy.Serialize((Stream) memoryStream, room);
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

    public void SendResetPlayerRoom()
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
        int num = this.sendOperation((byte) 3, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateFriendsList(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 4, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateClanData(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
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

    public void SendUpdateInboxMessages(int cmid, int messageId)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, messageId);
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

    public void SendUpdateInboxRequests(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 7, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateClanMembers(List<int> clanMembers)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) clanMembers, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
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

    public void SendGetPlayersWithMatchingName(string search)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, search);
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

    public void SendChatMessageToAll(string message)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, message);
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

    public void SendChatMessageToPlayer(int cmid, string message)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, message);
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

    public void SendChatMessageToClan(List<int> clanMembers, string message)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) clanMembers, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        StringProxy.Serialize((Stream) bytes, message);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 12, customOpParameters) ? 1 : 0;
      }
    }

    public void SendModerationMutePlayer(int durationInMinutes, int mutedCmid, bool disableChat)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, durationInMinutes);
        Int32Proxy.Serialize((Stream) bytes, mutedCmid);
        BooleanProxy.Serialize((Stream) bytes, disableChat);
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

    public void SendModerationPermanentBan(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
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

    public void SendModerationBanPlayer(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
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

    public void SendModerationKickGame(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 16, customOpParameters) ? 1 : 0;
      }
    }

    public void SendModerationUnbanPlayer(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 17, customOpParameters) ? 1 : 0;
      }
    }

    public void SendModerationCustomMessage(int cmid, string message)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, message);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 18, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSpeedhackDetection()
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
        int num = this.sendOperation((byte) 19, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSpeedhackDetectionNew(List<float> timeDifferences)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<float>.Serialize((Stream) bytes, (ICollection<float>) timeDifferences, new ListProxy<float>.Serializer<float>(SingleProxy.Serialize));
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 20, customOpParameters) ? 1 : 0;
      }
    }

    public void SendPlayersReported(List<int> cmids, int type, string details, string logs)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) cmids, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        Int32Proxy.Serialize((Stream) bytes, type);
        StringProxy.Serialize((Stream) bytes, details);
        StringProxy.Serialize((Stream) bytes, logs);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 21, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateNaughtyList()
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
        int num = this.sendOperation((byte) 22, customOpParameters) ? 1 : 0;
      }
    }

    public void SendClearModeratorFlags(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 23, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSetContactList(List<int> cmids)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) cmids, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 24, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateAllActors()
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
        int num = this.sendOperation((byte) 25, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdateContacts()
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
        int num = this.sendOperation((byte) 26, customOpParameters) ? 1 : 0;
      }
    }

    public void SendTrustedModules(string modules)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, modules);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 27, customOpParameters) ? 1 : 0;
      }
    }

    public void SendAuthentication(string hwid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, hwid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 28, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUberBeatReport(string report)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, report);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 29, customOpParameters) ? 1 : 0;
      }
    }
  }
}
