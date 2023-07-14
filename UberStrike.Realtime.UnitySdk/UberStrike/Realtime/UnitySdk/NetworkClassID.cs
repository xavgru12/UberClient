// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.NetworkClassID
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(1, 10)]
  public class NetworkClassID : ExtendableEnum<short>
  {
    public const short ClientSyncCenter = 1;
    public const short ServerSyncCenter = 2;
    public const short LobbyCenter = 3;
    public const short CommCenter = 4;
  }
}
