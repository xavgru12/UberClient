// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.BaseCommPeer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;

namespace UberStrike.Realtime.Client
{
  public abstract class BaseCommPeer : BasePeer, IEventDispatcher
  {
    public CommPeerOperations Operations { get; private set; }

    protected BaseCommPeer(int syncFrequency, bool monitorTraffic = false)
      : base(syncFrequency, monitorTraffic)
    {
      this.Operations = new CommPeerOperations((byte) 1);
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
          this.LoadData(data);
          break;
        case 3:
          this.LobbyEntered(data);
          break;
        case 4:
          this.DisconnectAndDisablePhoton(data);
          break;
      }
    }

    protected abstract void OnHeartbeatChallenge(string challengeHash);

    protected abstract void OnLoadData(ServerConnectionView data);

    protected abstract void OnLobbyEntered();

    protected abstract void OnDisconnectAndDisablePhoton(string message);

    private void HeartbeatChallenge(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnHeartbeatChallenge(StringProxy.Deserialize((Stream) bytes));
    }

    private void LoadData(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnLoadData(ServerConnectionViewProxy.Deserialize((Stream) bytes));
    }

    private void LobbyEntered(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnLobbyEntered();
    }

    private void DisconnectAndDisablePhoton(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnDisconnectAndDisablePhoton(StringProxy.Deserialize((Stream) bytes));
    }
  }
}
