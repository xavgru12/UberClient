// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.GameApplicationRPC
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
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
