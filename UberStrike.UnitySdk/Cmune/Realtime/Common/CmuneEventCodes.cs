// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.CmuneEventCodes
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(0, 255)]
  public class CmuneEventCodes : ExtendableEnum<byte>
  {
    public const byte Standard = 0;
    public const byte JoinEvent = 1;
    public const byte LeaveEvent = 2;
    public const byte GameListInit = 3;
    public const byte GameListUpdate = 4;
    public const byte GameListRemoval = 5;
    public const byte OldMessage = 100;
  }
}
