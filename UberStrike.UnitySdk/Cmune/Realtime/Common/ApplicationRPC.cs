
using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 20)]
  public class ApplicationRPC : ExtendableEnum<byte>
  {
    public const byte PeerSpecification = 1;
    public const byte QueryServerLoad = 2;
    public const byte QueryPerfCounters = 3;
    public const byte GeoLocalization = 4;
  }
}
