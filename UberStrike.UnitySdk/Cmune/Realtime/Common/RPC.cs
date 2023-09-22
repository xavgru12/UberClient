
using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 20)]
  public class RPC : ExtendableEnum<byte>
  {
    public const byte Join = 1;
    public const byte Leave = 2;
    public const byte PlayerUpdate = 3;
    public const byte FullPlayerListUpdate = 4;
    public const byte DeltaPlayerListUpdate = 5;
    public const byte ResetPlayer = 6;
  }
}
