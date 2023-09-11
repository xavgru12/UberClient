// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.GameRPC
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;
using Cmune.Realtime.Common;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(21, 50)]
  public class GameRPC : RPC
  {
    public const byte Begin = 21;
    public const byte End = 22;
    public const byte Pause = 23;
    public const byte Continue = 24;
  }
}
