// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.BaseLobbyRoom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
  public abstract class BaseLobbyRoom : IEventDispatcher, IRoomLogic
  {
    IOperationSender IRoomLogic.Operations => (IOperationSender) this.Operations;

    public LobbyRoomOperations Operations { get; private set; }

    protected BaseLobbyRoom() => this.Operations = new LobbyRoomOperations();

    public void OnEvent(byte id, byte[] data)
    {
      switch (id)
      {
        case 5:
          this.PlayerHide(data);
          break;
        case 6:
          this.PlayerLeft(data);
          break;
        case 7:
          this.PlayerUpdate(data);
          break;
        case 8:
          this.UpdateContacts(data);
          break;
        case 9:
          this.FullPlayerListUpdate(data);
          break;
        case 10:
          this.PlayerJoined(data);
          break;
        case 11:
          this.ClanChatMessage(data);
          break;
        case 12:
          this.InGameChatMessage(data);
          break;
        case 13:
          this.LobbyChatMessage(data);
          break;
        case 14:
          this.PrivateChatMessage(data);
          break;
        case 15:
          this.UpdateInboxRequests(data);
          break;
        case 16:
          this.UpdateFriendsList(data);
          break;
        case 17:
          this.UpdateInboxMessages(data);
          break;
        case 18:
          this.UpdateClanMembers(data);
          break;
        case 19:
          this.UpdateClanData(data);
          break;
        case 20:
          this.UpdateActorsForModeration(data);
          break;
        case 21:
          this.ModerationCustomMessage(data);
          break;
        case 22:
          this.ModerationMutePlayer(data);
          break;
        case 23:
          this.ModerationKickGame(data);
          break;
      }
    }

    protected abstract void OnPlayerHide(int cmid);

    protected abstract void OnPlayerLeft(int cmid, bool refreshComm);

    protected abstract void OnPlayerUpdate(CommActorInfo data);

    protected abstract void OnUpdateContacts(List<CommActorInfo> updated, List<int> removed);

    protected abstract void OnFullPlayerListUpdate(List<CommActorInfo> players);

    protected abstract void OnPlayerJoined(CommActorInfo data);

    protected abstract void OnClanChatMessage(int cmid, string name, string message);

    protected abstract void OnInGameChatMessage(
      int cmid,
      string name,
      string message,
      MemberAccessLevel accessLevel,
      byte context);

    protected abstract void OnLobbyChatMessage(int cmid, string name, string message);

    protected abstract void OnPrivateChatMessage(int cmid, string name, string message);

    protected abstract void OnUpdateInboxRequests();

    protected abstract void OnUpdateFriendsList();

    protected abstract void OnUpdateInboxMessages(int messageId);

    protected abstract void OnUpdateClanMembers();

    protected abstract void OnUpdateClanData();

    protected abstract void OnUpdateActorsForModeration(List<CommActorInfo> allHackers);

    protected abstract void OnModerationCustomMessage(string message);

    protected abstract void OnModerationMutePlayer(bool isPlayerMuted);

    protected abstract void OnModerationKickGame();

    private void PlayerHide(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerHide(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void PlayerLeft(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerLeft(Int32Proxy.Deserialize((Stream) bytes), BooleanProxy.Deserialize((Stream) bytes));
    }

    private void PlayerUpdate(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerUpdate(CommActorInfoProxy.Deserialize((Stream) bytes));
    }

    private void UpdateContacts(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnUpdateContacts(ListProxy<CommActorInfo>.Deserialize((Stream) bytes, new ListProxy<CommActorInfo>.Deserializer<CommActorInfo>(CommActorInfoProxy.Deserialize)), ListProxy<int>.Deserialize((Stream) bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }

    private void FullPlayerListUpdate(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnFullPlayerListUpdate(ListProxy<CommActorInfo>.Deserialize((Stream) bytes, new ListProxy<CommActorInfo>.Deserializer<CommActorInfo>(CommActorInfoProxy.Deserialize)));
    }

    private void PlayerJoined(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerJoined(CommActorInfoProxy.Deserialize((Stream) bytes));
    }

    private void ClanChatMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnClanChatMessage(Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes));
    }

    private void InGameChatMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnInGameChatMessage(Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), EnumProxy<MemberAccessLevel>.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes));
    }

    private void LobbyChatMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnLobbyChatMessage(Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes));
    }

    private void PrivateChatMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPrivateChatMessage(Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes));
    }

    private void UpdateInboxRequests(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnUpdateInboxRequests();
    }

    private void UpdateFriendsList(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnUpdateFriendsList();
    }

    private void UpdateInboxMessages(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnUpdateInboxMessages(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void UpdateClanMembers(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnUpdateClanMembers();
    }

    private void UpdateClanData(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnUpdateClanData();
    }

    private void UpdateActorsForModeration(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnUpdateActorsForModeration(ListProxy<CommActorInfo>.Deserialize((Stream) bytes, new ListProxy<CommActorInfo>.Deserializer<CommActorInfo>(CommActorInfoProxy.Deserialize)));
    }

    private void ModerationCustomMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnModerationCustomMessage(StringProxy.Deserialize((Stream) bytes));
    }

    private void ModerationMutePlayer(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnModerationMutePlayer(BooleanProxy.Deserialize((Stream) bytes));
    }

    private void ModerationKickGame(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnModerationKickGame();
    }
  }
}
