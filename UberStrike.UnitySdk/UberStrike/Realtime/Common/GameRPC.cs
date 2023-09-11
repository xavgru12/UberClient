
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
