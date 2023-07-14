// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.GameApplicationRPC
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(21, 50)]
  public class GameApplicationRPC : ApplicationRPC
  {
    public const byte RoomRequest = 21;
    public const byte BanPlayer = 22;
    public const byte CustomMessage = 23;
    public const byte MutePlayer = 24;
    public const byte UnmutePlayer = 25;
    public const byte GhostPlayer = 26;
  }
}
