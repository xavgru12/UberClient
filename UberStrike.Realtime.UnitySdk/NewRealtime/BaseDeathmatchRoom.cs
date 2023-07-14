// Decompiled with JetBrains decompiler
// Type: NewRealtime.BaseDeathmatchRoom
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.IO;
using UberStrike.Core.Serialization;

namespace NewRealtime
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
