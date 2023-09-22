
using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 10)]
  public class NetworkClassID : ExtendableEnum<short>
  {
    public const short ClientSyncCenter = 1;
    public const short ServerSyncCenter = 2;
    public const short LobbyCenter = 3;
    public const short CommCenter = 4;
  }
}
