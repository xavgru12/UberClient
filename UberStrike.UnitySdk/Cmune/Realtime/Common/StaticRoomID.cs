
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
