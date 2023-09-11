// Decompiled with JetBrains decompiler
// Type: Uberstrike.Realtime.Client.Events.BaseDeathmatchRoom
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;
using UberStrike.Core.Serialization;
using Uberstrike.Realtime.Client.Base;
using Uberstrike.Realtime.Client.Operations;

namespace Uberstrike.Realtime.Client.Events
{
  public abstract class BaseDeathmatchRoom : IEventDispatcher
  {
    public DeathmatchOperations Operations { get; private set; }

    protected BaseDeathmatchRoom() => this.Operations = new DeathmatchOperations();

    public void OnEvent(byte id, byte[] data)
    {
      switch (id)
      {
        case 1:
          this.PlayerKilled(data);
          break;
        case 2:
          this.PlayerShot(data);
          break;
      }
    }

    protected abstract void OnPlayerKilled(int playerId);

    protected abstract void OnPlayerShot(int playerId);

    private void PlayerKilled(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnPlayerKilled(Int32Proxy.Deserialize((Stream) bytes1));
    }

    private void PlayerShot(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
        this.OnPlayerShot(Int32Proxy.Deserialize((Stream) bytes1));
    }
  }
}
