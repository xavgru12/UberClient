// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.StaticRoomID
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(0, 99)]
  public class StaticRoomID : ExtendableEnum<int>
  {
    public const int Auto = 0;
    public const int Test = 1;
    public const int LobbyCenter = 66;
    public const int CommCenter = 88;
  }
}
