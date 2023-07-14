// Decompiled with JetBrains decompiler
// Type: NewRealtime.BaseGameServerPeerPeer
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;

namespace NewRealtime
{
  public abstract class BaseGameServerPeerPeer : BasePeer, IEventDispatcher
  {
    public GameServerPeerOperations Operations { get; private set; }

    protected BaseGameServerPeerPeer(float syncFrequency)
      : base(syncFrequency)
    {
      this.Operations = new GameServerPeerOperations();
      this.AddDispatcher((IEventDispatcher) this, (IOperationDispatcher) this.Operations);
    }

    public void OnEvent(byte id, byte[] data)
    {
      switch (id)
      {
        case 1:
          this.GameServerData(data);
          break;
        case 2:
          this.JoinedGameRoom(data);
          break;
        case 3:
          this.LeftGameRoom(data);
          break;
        case 4:
          this.FullGameList(data);
          break;
        case 5:
          this.GameListUpdate(data);
          break;
        case 6:
          this.GamesDeleted(data);
          break;
      }
    }

    protected abstract void OnGameServerData(ServerConnectionView data);

    protected abstract void OnJoinedGameRoom(int roomId);

    protected abstract void OnLeftGameRoom(int roomId);

    protected abstract void OnFullGameList(List<int> gameList);

    protected abstract void OnGameListUpdate(Dictionary<int, int> updatedGames);

    protected abstract void OnGamesDeleted(List<int> deletedGameIds);

    private void GameServerData(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnGameServerData(ServerConnectionViewProxy.Deserialize((Stream) bytes1));
    }

    private void JoinedGameRoom(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnJoinedGameRoom(Int32Proxy.Deserialize((Stream) bytes1));
    }

    private void LeftGameRoom(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnLeftGameRoom(Int32Proxy.Deserialize((Stream) bytes1));
    }

    private void FullGameList(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnFullGameList(ListProxy<int>.Deserialize((Stream) bytes1, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }

    private void GameListUpdate(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnGameListUpdate(DictionaryProxy<int, int>.Deserialize((Stream) bytes1, new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }

    private void GamesDeleted(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnGamesDeleted(ListProxy<int>.Deserialize((Stream) bytes1, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }
  }
}
