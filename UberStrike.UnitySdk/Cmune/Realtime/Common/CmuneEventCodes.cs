
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
