// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.BaseGamePeer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.Core.Models;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
  public abstract class BaseGamePeer : BasePeer, IEventDispatcher
  {
    public GamePeerOperations Operations { get; private set; }

    protected BaseGamePeer(int syncFrequency, bool monitorTraffic = false)
      : base(syncFrequency, monitorTraffic)
    {
      this.Operations = new GamePeerOperations((byte) 1);
      this.AddRoomLogic((IEventDispatcher) this, (IOperationSender) this.Operations);
    }

    public void OnEvent(byte id, byte[] data)
    {
      switch (id)
      {
        case 1:
          this.HeartbeatChallenge(data);
          break;
        case 2:
          this.RoomEntered(data);
          break;
        case 3:
          this.RoomEnterFailed(data);
          break;
        case 4:
          this.RequestPasswordForRoom(data);
          break;
        case 5:
          this.RoomLeft(data);
          break;
        case 6:
          this.FullGameList(data);
          break;
        case 7:
          this.GameListUpdate(data);
          break;
        case 8:
          this.GameListUpdateEnd(data);
          break;
        case 9:
          this.GetGameInformation(data);
          break;
        case 10:
          this.ServerLoadData(data);
          break;
        case 11:
          this.DisconnectAndDisablePhoton(data);
          break;
      }
    }

    protected abstract void OnHeartbeatChallenge(string challengeHash);

    protected abstract void OnRoomEntered(GameRoomData game);

    protected abstract void OnRoomEnterFailed(string server, int roomId, string message);

    protected abstract void OnRequestPasswordForRoom(string server, int roomId);

    protected abstract void OnRoomLeft();

    protected abstract void OnFullGameList(List<GameRoomData> gameList);

    protected abstract void OnGameListUpdate(
      List<GameRoomData> updatedGames,
      List<int> removedGames);

    protected abstract void OnGameListUpdateEnd();

    protected abstract void OnGetGameInformation(
      GameRoomData room,
      List<GameActorInfo> players,
      int endTime);

    protected abstract void OnServerLoadData(PhotonServerLoad data);

    protected abstract void OnDisconnectAndDisablePhoton(string message);

    private void HeartbeatChallenge(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnHeartbeatChallenge(StringProxy.Deserialize((Stream) bytes));
    }

    private void RoomEntered(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnRoomEntered(GameRoomDataProxy.Deserialize((Stream) bytes));
    }

    private void RoomEnterFailed(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnRoomEnterFailed(StringProxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes));
    }

    private void RequestPasswordForRoom(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnRequestPasswordForRoom(StringProxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes));
    }

    private void RoomLeft(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnRoomLeft();
    }

    private void FullGameList(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnFullGameList(ListProxy<GameRoomData>.Deserialize((Stream) bytes, new ListProxy<GameRoomData>.Deserializer<GameRoomData>(GameRoomDataProxy.Deserialize)));
    }

    private void GameListUpdate(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnGameListUpdate(ListProxy<GameRoomData>.Deserialize((Stream) bytes, new ListProxy<GameRoomData>.Deserializer<GameRoomData>(GameRoomDataProxy.Deserialize)), ListProxy<int>.Deserialize((Stream) bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }

    private void GameListUpdateEnd(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnGameListUpdateEnd();
    }

    private void GetGameInformation(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnGetGameInformation(GameRoomDataProxy.Deserialize((Stream) bytes), ListProxy<GameActorInfo>.Deserialize((Stream) bytes, new ListProxy<GameActorInfo>.Deserializer<GameActorInfo>(GameActorInfoProxy.Deserialize)), Int32Proxy.Deserialize((Stream) bytes));
    }

    private void ServerLoadData(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnServerLoadData(PhotonServerLoadProxy.Deserialize((Stream) bytes));
    }

    private void DisconnectAndDisablePhoton(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnDisconnectAndDisablePhoton(StringProxy.Deserialize((Stream) bytes));
    }
  }
}
